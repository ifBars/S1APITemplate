using System;
using System.Collections.Generic;
using RovingSpecialCustomers.Models;
using S1API.Entities;
using UnityEngine;

namespace RovingSpecialCustomers.NPCs;

public abstract class RovingCrewMember : NPC
{
    private static readonly List<RovingCrewMember> Members = new();
    private static readonly Vector3 HiddenPosition = new(0f, Utils.Constants.World.HiddenY, 0f);

    public sealed override bool IsPhysical => true;
    protected abstract string CrewId { get; }
    protected virtual Vector3 VisitOffset => Vector3.zero;

    protected override void OnCreated()
    {
        base.OnCreated();
        Appearance.Build();
        RequiresRegionUnlocked = false;
        Schedule.Disable();
        Position = HiddenPosition;
        Members.Remove(this);
        Members.Add(this);
    }

    protected override void OnDestroyed()
    {
        Members.Remove(this);
        base.OnDestroyed();
    }

    public static void BeginCrewVisit(CrewDefinition definition)
    {
        foreach (var member in Members.ToArray())
        {
            if (member is null)
            {
                continue;
            }

            if (!string.Equals(member.CrewId, definition.Id, StringComparison.OrdinalIgnoreCase) && member.CrewId != "*")
            {
                member.Position = HiddenPosition;
                member.Schedule.Disable();
                continue;
            }

            member.Position = definition.ArrivalPosition + member.VisitOffset;
            member.Schedule.Disable();
        }
    }

    public static void HideAll()
    {
        foreach (var member in Members.ToArray())
        {
            if (member is null)
            {
                continue;
            }

            member.Position = HiddenPosition;
            member.Schedule.Disable();
        }
    }
}
