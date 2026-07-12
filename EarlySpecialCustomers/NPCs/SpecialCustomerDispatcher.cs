using RovingSpecialCustomers.Models;
using RovingSpecialCustomers.Services;
using S1API.Entities;
using S1API.Saveables;

namespace RovingSpecialCustomers.NPCs;

public sealed class SpecialCustomerDispatcher : NPC
{
    public static SpecialCustomerDispatcher? Instance { get; private set; }

    [SaveableField("RovingSpecialCustomerVisits")]
    private RovingVisitState _state = new();

    public override bool IsPhysical => false;
    internal RovingVisitState State => _state;

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity("rsc_dispatcher", "Travel", "Wire");
    }

    protected override void OnCreated()
    {
        base.OnCreated();
        Instance = this;
        ClearConversationCategories();
        RovingVisitCoordinator.Bind(this);
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();
        _state ??= new RovingVisitState();
        Instance = this;
        RovingVisitCoordinator.Bind(this);
    }

    protected override void OnDestroyed()
    {
        RovingVisitCoordinator.Unbind(this);
        if (ReferenceEquals(Instance, this))
        {
            Instance = null;
        }
        base.OnDestroyed();
    }

    internal void Tick(float deltaTime) => RovingVisitCoordinator.Tick(deltaTime);
    internal void SendSystemMessage(string message) => SendTextMessage(message);
    internal void SaveState() => RequestGameSave();
}
