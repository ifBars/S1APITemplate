using System;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using RovingSpecialCustomers.Items;
using RovingSpecialCustomers.Models;
using RovingSpecialCustomers.NPCs;
using S1API.Economy;
using S1API.Products;

namespace RovingSpecialCustomers.Services;

public static class RovingVisitCoordinator
{
    private static SpecialCustomerDispatcher? _dispatcher;
    private static float _tickAccumulator;
    private static int _lastOfferRetryHour = -1;

    public static void Bind(SpecialCustomerDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _tickAccumulator = 0f;
        _lastOfferRetryHour = -1;
    }

    public static void Unbind(SpecialCustomerDispatcher dispatcher)
    {
        if (ReferenceEquals(_dispatcher, dispatcher))
        {
            _dispatcher = null;
            SpecialCustomerRuntime.DestroyCrewVehicle();
            RovingCrewMember.HideAll();
        }
    }

    public static void Tick(float deltaTime)
    {
        if (_dispatcher is null || !Core.GameLoaded)
        {
            return;
        }

        _tickAccumulator += deltaTime;
        if (_tickAccumulator < 1f)
        {
            return;
        }

        _tickAccumulator = 0f;
        if (!SpecialCustomerRuntime.IsReady || !SpecialCustomerRuntime.IsHost)
        {
            return;
        }

        var state = _dispatcher.State;
        EnsureVisitScheduled(state);

        var day = SpecialCustomerRuntime.ElapsedDays;
        var time = SpecialCustomerRuntime.CurrentTime;

        if (!state.NoticeSent && day >= state.NoticeDay)
        {
            SendAdvanceNotice(state);
        }

        if (!state.Active && day >= state.ArrivalDay && time >= Utils.Constants.Timing.ArrivalTime)
        {
            StartVisit(state);
        }

        if (state.Active && !state.DealOffered)
        {
            var hour = time / 100;
            if (hour != _lastOfferRetryHour)
            {
                _lastOfferRetryHour = hour;
                TryOfferActiveContract(notifyOnFailure: false);
            }
        }

        if (state.Active && day > state.ArrivalDay)
        {
            EndVisit(state);
        }
    }

    public static string GetActiveVisitSummary()
    {
        if (_dispatcher is null || !_dispatcher.State.Active)
        {
            return "No special customer crew is currently in town.";
        }

        var state = _dispatcher.State;
        var definition = CrewDefinitions.Get(state.CrewId);
        if (definition is null)
        {
            return "The current crew data is unavailable.";
        }

        var effects = string.Join(", ", state.PreferredEffectIds.Select(CrewDefinitions.GetEffectName));
        return $"{definition.DisplayName} wants {definition.AcceptedDrugLabel}. Bonus effects: {effects}. Remaining group budget: ${state.RemainingBudget:0}.";
    }

    public static bool TryOfferActiveContract(bool notifyOnFailure = true)
    {
        if (_dispatcher is null)
        {
            return false;
        }

        var state = _dispatcher.State;
        var definition = CrewDefinitions.Get(state.CrewId);
        var leader = RovingCrewLeader.Get(state.CrewId);
        if (!state.Active || state.DealOffered || definition is null || leader is null)
        {
            return false;
        }

        var candidates = ProductManager.ListedProducts
            .Where(product => definition.AcceptedDrugTypes.Contains(product.DrugType.ToString(), StringComparer.OrdinalIgnoreCase))
            .ToArray();

        if (candidates.Length == 0)
        {
            if (notifyOnFailure)
            {
                _dispatcher.SendSystemMessage($"{definition.DisplayName} is ready to buy, but you do not currently have a listed {definition.AcceptedDrugLabel} product.");
            }
            return false;
        }

        var selected = candidates
            .Select(product => new { Product = product, Match = GetEffectMatch(product, state.PreferredEffectIds) })
            .OrderByDescending(entry => entry.Match)
            .ThenByDescending(entry => entry.Product.MarketValue)
            .First();

        var marketValue = Math.Max(1f, selected.Product.MarketValue);
        var effectMultiplier = 1f + 0.5f * selected.Match;
        var rawQuantity = (int)Math.Floor(state.RemainingBudget / (marketValue * effectMultiplier));
        var quantity = Math.Clamp(rawQuantity, definition.MinUnits, definition.MaxUnits);
        if (quantity >= 20)
        {
            quantity = Math.Max(definition.MinUnits, (int)Math.Round(quantity / 5f) * 5);
        }

        var payment = Math.Min(state.RemainingBudget, marketValue * quantity * effectMultiplier);
        payment = (float)Math.Round(payment / 10f) * 10f;
        if (payment <= 0f)
        {
            return false;
        }

        var contract = new ContractInfo
        {
            Payment = payment,
            Expires = true,
            ExpiresAfterMinutes = 840,
            PickupScheduleIndex = 0
        };
        contract.AddProductById(selected.Product.ID, quantity, Quality.Standard)
            .WithWindow(1000, 2300);

        if (!leader.Customer.OfferContract(contract))
        {
            if (notifyOnFailure)
            {
                _dispatcher.SendSystemMessage($"{definition.DisplayName} could not create a bulk order right now. Try again after listing a matching product.");
            }
            return false;
        }

        state.DealOffered = true;
        state.OfferedProductId = selected.Product.ID;
        state.OfferedQuantity = quantity;
        state.OfferedPayment = payment;
        _dispatcher.SendSystemMessage($"{definition.DisplayName} offered ${payment:0} for {quantity} units of {selected.Product.Name}. Effect match bonus: {selected.Match * 100f:0}%.");
        _dispatcher.SaveState();
        return true;
    }

    public static void HandleDealCompleted(string crewId)
    {
        if (_dispatcher is null)
        {
            return;
        }

        var state = _dispatcher.State;
        if (!state.Active || !string.Equals(state.CrewId, crewId, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        state.DealCompleted = true;
        state.RemainingBudget = Math.Max(0f, state.RemainingBudget - state.OfferedPayment);
        _dispatcher.SendSystemMessage($"Bulk sale complete. {CrewDefinitions.Get(crewId)?.DisplayName ?? "The crew"} has ${state.RemainingBudget:0} left in its visit budget.");
        _dispatcher.SaveState();
    }

    public static void TryPurchaseExclusive(string crewId)
    {
        if (_dispatcher is null)
        {
            return;
        }

        var state = _dispatcher.State;
        var definition = CrewDefinitions.Get(crewId);
        if (!state.Active || definition is null || !string.Equals(state.CrewId, crewId, StringComparison.OrdinalIgnoreCase))
        {
            _dispatcher.SendSystemMessage("That crew is not currently visiting Highland Point.");
            return;
        }

        if (state.ExclusivePurchased)
        {
            _dispatcher.SendSystemMessage("You already bought this crew's exclusive item during the current visit.");
            return;
        }

        if (ExclusiveItemRegistry.TryPurchase(definition, out var message))
        {
            state.ExclusivePurchased = true;
            _dispatcher.SaveState();
        }

        _dispatcher.SendSystemMessage(message);
    }

    private static void EnsureVisitScheduled(RovingVisitState state)
    {
        if (state.HasScheduledVisit)
        {
            if (state.Active)
            {
                RestoreActiveVisit(state);
            }
            return;
        }

        var currentDay = SpecialCustomerRuntime.ElapsedDays;
        var seed = unchecked((currentDay + 1) * 7919 + (state.VisitSequence + 1) * 104729);
        var random = new Random(seed);
        var isFirstVisit = state.VisitSequence == 0;
        var minDays = isFirstVisit ? Utils.Constants.Timing.FirstVisitMinDays : Utils.Constants.Timing.RepeatVisitMinDays;
        var maxDays = isFirstVisit ? Utils.Constants.Timing.FirstVisitMaxDays : Utils.Constants.Timing.RepeatVisitMaxDays;
        var daysUntilArrival = random.Next(minDays, maxDays);
        var noticeLead = random.Next(Utils.Constants.Timing.NoticeMinDays, Utils.Constants.Timing.NoticeMaxDays);
        var definitions = CrewDefinitions.All;
        var definition = definitions[random.Next(definitions.Count)];

        state.VisitSequence++;
        state.ScheduledFromDay = currentDay;
        state.ArrivalDay = currentDay + daysUntilArrival;
        state.NoticeDay = Math.Max(currentDay, state.ArrivalDay - noticeLead);
        state.CrewId = definition.Id;
        state.PreferredEffectIds = CrewDefinitions.PickEffectIds(random, random.Next(1, 4));
        state.BudgetMultiplier = 0.85f + (float)random.NextDouble() * 0.30f;
        _dispatcher?.SaveState();
    }

    private static void SendAdvanceNotice(RovingVisitState state)
    {
        var definition = CrewDefinitions.Get(state.CrewId);
        if (definition is null || _dispatcher is null)
        {
            return;
        }

        state.NoticeSent = true;
        var effects = string.Join(", ", state.PreferredEffectIds.Select(CrewDefinitions.GetEffectName));
        var days = Math.Max(0, state.ArrivalDay - SpecialCustomerRuntime.ElapsedDays);
        _dispatcher.SendSystemMessage($"Travel notice: {definition.DisplayName} arrives in about {days} day(s). They only buy {definition.AcceptedDrugLabel}. Requested bonus effects: {effects}.");
        _dispatcher.SaveState();
    }

    private static void StartVisit(RovingVisitState state)
    {
        var definition = CrewDefinitions.Get(state.CrewId);
        var leader = RovingCrewLeader.Get(state.CrewId);
        if (definition is null || leader is null || _dispatcher is null)
        {
            MelonLogger.Error($"[RSC] Could not start visit for '{state.CrewId}'.");
            state.ResetCurrentVisit();
            _dispatcher?.SaveState();
            return;
        }

        state.Active = true;
        state.VisitBudget = definition.BaseBudget * SpecialCustomerRuntime.GetRankBudgetMultiplier() * state.BudgetMultiplier;
        state.VisitBudget = (float)Math.Round(state.VisitBudget / 50f) * 50f;
        state.RemainingBudget = state.VisitBudget;
        RovingCrewMember.BeginCrewVisit(definition);
        SpecialCustomerRuntime.ApplyVisitEffects(leader, state.PreferredEffectIds);
        SpecialCustomerRuntime.SpawnCrewVehicle(definition);

        var effects = string.Join(", ", state.PreferredEffectIds.Select(CrewDefinitions.GetEffectName));
        _dispatcher.SendSystemMessage($"{definition.DisplayName} has arrived. Group budget: ${state.VisitBudget:0}. Accepted drugs: {definition.AcceptedDrugLabel}. Bonus effects: {effects}. Their exclusive item is {definition.ExclusiveItemName} (${definition.ExclusiveItemPrice:0}).");
        _dispatcher.SaveState();
        TryOfferActiveContract(notifyOnFailure: true);
    }

    private static void RestoreActiveVisit(RovingVisitState state)
    {
        var definition = CrewDefinitions.Get(state.CrewId);
        var leader = RovingCrewLeader.Get(state.CrewId);
        if (definition is null || leader is null)
        {
            return;
        }

        RovingCrewMember.BeginCrewVisit(definition);
        SpecialCustomerRuntime.ApplyVisitEffects(leader, state.PreferredEffectIds);
    }

    private static void EndVisit(RovingVisitState state)
    {
        var definition = CrewDefinitions.Get(state.CrewId);
        RovingCrewMember.HideAll();
        SpecialCustomerRuntime.DestroyCrewVehicle();
        _dispatcher?.SendSystemMessage($"{definition?.DisplayName ?? "The visiting crew"} has left Highland Point.");
        state.ResetCurrentVisit();
        _dispatcher?.SaveState();
        _lastOfferRetryHour = -1;
    }

    private static float GetEffectMatch(ProductDefinition product, IReadOnlyCollection<string> preferredEffectIds)
    {
        if (preferredEffectIds.Count == 0)
        {
            return 0f;
        }

        var matched = product.Properties.Count(property => preferredEffectIds.Contains(property.ID, StringComparer.OrdinalIgnoreCase));
        return Math.Clamp((float)matched / preferredEffectIds.Count, 0f, 1f);
    }
}
