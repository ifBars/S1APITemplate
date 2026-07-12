using System;
using System.Collections.Generic;
using System.Linq;
using S1API.Properties;
using S1API.Properties.Interfaces;
using UnityEngine;

namespace RovingSpecialCustomers.Models;

public sealed class CrewDefinition
{
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string[] AcceptedDrugTypes { get; set; } = Array.Empty<string>();
    public string[] VehicleHints { get; set; } = Array.Empty<string>();
    public float BaseBudget { get; set; }
    public int MinUnits { get; set; }
    public int MaxUnits { get; set; }
    public string ExclusiveItemId { get; set; } = string.Empty;
    public string ExclusiveItemName { get; set; } = string.Empty;
    public float ExclusiveItemPrice { get; set; }
    public Vector3 ArrivalPosition { get; set; }
    public Vector3 VehiclePosition { get; set; }
    public Quaternion VehicleRotation { get; set; } = Quaternion.identity;

    public string AcceptedDrugLabel => string.Join(" or ", AcceptedDrugTypes);
}

public static class CrewDefinitions
{
    private static readonly CrewDefinition[] Definitions =
    {
        new()
        {
            Id = "hippies",
            DisplayName = "The Hippies",
            AcceptedDrugTypes = new[] { "Marijuana", "Shrooms" },
            VehicleHints = new[] { "camper", "van", "rv", "shitbox" },
            BaseBudget = 4500f,
            MinUnits = 25,
            MaxUnits = 180,
            ExclusiveItemId = "rsc_hippie_cap",
            ExclusiveItemName = "Sunwashed Tour Cap",
            ExclusiveItemPrice = 325f,
            ArrivalPosition = new Vector3(-53.57f, 1.065f, 67.80f),
            VehiclePosition = new Vector3(-60.0f, 1.065f, 76.0f),
            VehicleRotation = Quaternion.Euler(0f, 220f, 0f)
        },
        new()
        {
            Id = "businessmen",
            DisplayName = "The Business Delegation",
            AcceptedDrugTypes = new[] { "Cocaine" },
            VehicleHints = new[] { "limo", "sedan", "benzies", "shitbox" },
            BaseBudget = 9000f,
            MinUnits = 20,
            MaxUnits = 220,
            ExclusiveItemId = "rsc_executive_cap",
            ExclusiveItemName = "Executive Travel Cap",
            ExclusiveItemPrice = 650f,
            ArrivalPosition = new Vector3(-55.40f, 1.065f, 66.25f),
            VehiclePosition = new Vector3(-61.7f, 1.065f, 74.4f),
            VehicleRotation = Quaternion.Euler(0f, 220f, 0f)
        },
        new()
        {
            Id = "bikers",
            DisplayName = "The Road Crew",
            AcceptedDrugTypes = new[] { "Methamphetamine", "Heroin" },
            VehicleHints = new[] { "bike", "motor", "pickup", "shitbox" },
            BaseBudget = 7000f,
            MinUnits = 30,
            MaxUnits = 240,
            ExclusiveItemId = "rsc_road_cap",
            ExclusiveItemName = "Road Crew Cap",
            ExclusiveItemPrice = 500f,
            ArrivalPosition = new Vector3(-57.10f, 1.065f, 64.80f),
            VehiclePosition = new Vector3(-63.2f, 1.065f, 72.7f),
            VehicleRotation = Quaternion.Euler(0f, 220f, 0f)
        },
        new()
        {
            Id = "party_bus",
            DisplayName = "The Party Bus",
            AcceptedDrugTypes = new[] { "MDMA", "Shrooms" },
            VehicleHints = new[] { "bus", "van", "camper", "shitbox" },
            BaseBudget = 8000f,
            MinUnits = 35,
            MaxUnits = 260,
            ExclusiveItemId = "rsc_party_cap",
            ExclusiveItemName = "Afterparty Cap",
            ExclusiveItemPrice = 550f,
            ArrivalPosition = new Vector3(-58.80f, 1.065f, 63.25f),
            VehiclePosition = new Vector3(-64.9f, 1.065f, 71.0f),
            VehicleRotation = Quaternion.Euler(0f, 220f, 0f)
        }
    };

    private static readonly PropertyBase[] EffectPool =
    {
        Property.Munchies,
        Property.AntiGravity,
        Property.Energizing,
        Property.Focused,
        Property.Euphoric,
        Property.Slippery,
        Property.Electrifying,
        Property.Disorienting,
        Property.Sedating,
        Property.CalorieDense,
        Property.ThoughtProvoking,
        Property.Calming,
        Property.Schizophrenic,
        Property.Spicy,
        Property.Laxative,
        Property.BrightEyed,
        Property.Sneaky,
        Property.Refreshing,
        Property.Athletic,
        Property.Paranoia,
        Property.Foggy,
        Property.Explosive
    };

    public static IReadOnlyList<CrewDefinition> All => Definitions;

    public static CrewDefinition? Get(string id) =>
        Definitions.FirstOrDefault(definition => string.Equals(definition.Id, id, StringComparison.OrdinalIgnoreCase));

    public static PropertyBase? GetEffect(string id) =>
        EffectPool.FirstOrDefault(effect => string.Equals(effect.ID, id, StringComparison.OrdinalIgnoreCase));

    public static string GetEffectName(string id) => GetEffect(id)?.Name ?? id;

    public static string[] PickEffectIds(System.Random random, int count)
    {
        var pool = EffectPool.ToList();
        var result = new List<string>();
        count = Math.Clamp(count, 1, Math.Min(3, pool.Count));

        while (result.Count < count && pool.Count > 0)
        {
            var index = random.Next(pool.Count);
            result.Add(pool[index].ID);
            pool.RemoveAt(index);
        }

        return result.ToArray();
    }
}
