using System;
using System.Collections.Generic;
using RovingSpecialCustomers.Services;
using S1API.Entities;

namespace RovingSpecialCustomers.NPCs;

public abstract class RovingCrewLeader : RovingCrewMember
{
    private static readonly Dictionary<string, RovingCrewLeader> Leaders = new(StringComparer.OrdinalIgnoreCase);
    private Action? _dealCompletedHandler;

    protected abstract string DialogueContainerId { get; }

    protected override void OnCreated()
    {
        base.OnCreated();
        Leaders[CrewId] = this;
        WireCustomerEvents();
        SetupDialogue();
    }

    protected override void OnDestroyed()
    {
        if (_dealCompletedHandler is not null)
        {
            Customer.OnDealCompleted -= _dealCompletedHandler;
        }

        if (Leaders.TryGetValue(CrewId, out var current) && ReferenceEquals(current, this))
        {
            Leaders.Remove(CrewId);
        }

        base.OnDestroyed();
    }

    public static RovingCrewLeader? Find(string crewId) =>
        Leaders.TryGetValue(crewId, out var leader) ? leader : null;

    private void WireCustomerEvents()
    {
        _dealCompletedHandler ??= () => RovingVisitCoordinator.HandleDealCompleted(CrewId);
        Customer.OnDealCompleted -= _dealCompletedHandler;
        Customer.OnDealCompleted += _dealCompletedHandler;
    }

    private void SetupDialogue()
    {
        Dialogue.BuildAndRegisterContainer(DialogueContainerId, container =>
        {
            container.AddNode("ENTRY", "We're only in Highland Point for a short stop. What do you need?", choices =>
            {
                choices.Add("RSC_DETAILS", "What are you buying?", "EXIT")
                    .Add("RSC_OFFER", "Check for a bulk order", "EXIT")
                    .Add("RSC_BUY", "Buy your exclusive item", "EXIT")
                    .Add("RSC_LEAVE", "Nothing", "EXIT");
            });
            container.AddNode("EXIT", "Keep an eye on the clock.");
        });

        Dialogue.OnChoiceSelected("RSC_DETAILS", () =>
        {
            SendTextMessage(RovingVisitCoordinator.GetActiveVisitSummary());
            Dialogue.StopOverride();
        });

        Dialogue.OnChoiceSelected("RSC_OFFER", () =>
        {
            RovingVisitCoordinator.TryOfferActiveContract(notifyOnFailure: true);
            Dialogue.StopOverride();
        });

        Dialogue.OnChoiceSelected("RSC_BUY", () =>
        {
            RovingVisitCoordinator.TryPurchaseExclusive(CrewId);
            Dialogue.StopOverride();
        });

        Dialogue.OnChoiceSelected("RSC_LEAVE", Dialogue.StopOverride);
        Dialogue.UseContainerOnInteract(DialogueContainerId);
    }
}
