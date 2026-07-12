using System;
using MelonLoader;
using S1API.Entities;
using S1API.Map;
using S1API.Saveables;

namespace EarlySpecialCustomers.NPCs;

/// <summary>
/// Shared runtime behavior for early special customers.
/// Persistent customer configuration remains in each NPC's ConfigurePrefab method.
/// </summary>
public abstract class EarlySpecialCustomer : NPC
{
    [Serializable]
    private sealed class PersistedState
    {
        public bool IntroSent;
        public int CompletedDeals;
    }

    [SaveableField("EarlySpecialCustomerState")]
    private PersistedState _state = new();

    private Action? _dealCompletedHandler;

    public sealed override bool IsPhysical => true;

    protected abstract string IntroMessage { get; }
    protected abstract string MilestoneMessage { get; }
    protected abstract int DealsPerMilestone { get; }

    protected override void OnCreated()
    {
        base.OnCreated();
        Appearance.Build();
        RequiresRegionUnlocked = false;
        Region = Region.Northtown;
        Schedule.Enable();
        WireCustomerEvents();
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();

        if (!_state.IntroSent && Core.IntroMessagesEnabled)
        {
            SendTextMessage(IntroMessage);
            _state.IntroSent = true;
            RequestGameSave();
        }
    }

    protected override void OnDestroyed()
    {
        if (_dealCompletedHandler is not null)
        {
            Customer.OnDealCompleted -= _dealCompletedHandler;
        }

        base.OnDestroyed();
    }

    private void WireCustomerEvents()
    {
        _dealCompletedHandler ??= HandleDealCompleted;
        Customer.OnDealCompleted -= _dealCompletedHandler;
        Customer.OnDealCompleted += _dealCompletedHandler;
    }

    private void HandleDealCompleted()
    {
        _state.CompletedDeals++;

        if (Core.MilestoneMessagesEnabled && DealsPerMilestone > 0 && _state.CompletedDeals % DealsPerMilestone == 0)
        {
            SendTextMessage(MilestoneMessage);
        }

        if (Core.DebugLogsEnabled)
        {
            MelonLogger.Msg($"[{ID}] completed deals: {_state.CompletedDeals}");
        }

        RequestGameSave();
    }
}
