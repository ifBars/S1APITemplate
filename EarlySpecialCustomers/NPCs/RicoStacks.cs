using S1API.Economy;
using S1API.Entities;
using S1API.Entities.Schedule;
using S1API.GameTime;
using S1API.Map.Buildings;
using S1API.Products;
using S1API.Properties;
using UnityEngine;

namespace EarlySpecialCustomers.NPCs;

/// <summary>A less frequent bulk buyer who gives an early operation a larger, riskier target.</summary>
public sealed class RicoStacks : EarlySpecialCustomer
{
    private static readonly Vector3 SpawnPosition = new(-35.7332f, 1.065f, 52.2295f);
    private static readonly Vector3 HangoutPosition = new(-61.2776f, 1.065f, 55.6136f);

    protected override string IntroMessage =>
        "I move volume. Keep the price fair, keep the supply steady, and don't make me chase you.";

    protected override string MilestoneMessage =>
        "Good volume. Keep that consistency and I'll keep bringing bigger orders.";

    protected override int DealsPerMilestone => 2;

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity("esc_rico_stacks", "Rico", "Stacks")
            .WithAppearanceDefaults(appearance =>
            {
                appearance.Gender = 0f;
                appearance.Height = 1.08f;
                appearance.Weight = 0.76f;
                appearance.SkinColor = new Color32(116, 80, 61, 255);
                appearance.LeftEyeLidColor = appearance.SkinColor;
                appearance.RightEyeLidColor = appearance.SkinColor;
                appearance.EyeBallTint = Color.white;
                appearance.PupilDilation = 0.70f;
                appearance.HairColor = new Color(0.03f, 0.02f, 0.01f);
                appearance.HairPath = "Avatar/Hair/Spiky/Spiky";
                appearance.WithFaceLayer("Avatar/Layers/Face/Face_SlightSmile", Color.black);
                appearance.WithBodyLayer("Avatar/Layers/Top/FlannelButtonUp", new Color(0.42f, 0.08f, 0.08f));
                appearance.WithBodyLayer("Avatar/Layers/Bottom/Jorts", new Color(0.12f, 0.18f, 0.28f));
                appearance.WithAccessoryLayer("Avatar/Accessories/Head/PorkpieHat/PorkpieHat", new Color(0.10f, 0.10f, 0.10f));
                appearance.WithAccessoryLayer("Avatar/Accessories/Feet/CombatBoots/CombatBoots", new Color(0.15f, 0.10f, 0.06f));
            })
            .WithSpawnPosition(SpawnPosition)
            .EnsureCustomer()
            .WithCustomerDefaults(customer =>
            {
                customer.WithSpending(700f, 1600f)
                    .WithOrdersPerWeek(1, 2)
                    .WithPreferredOrderDay(Day.Friday)
                    .WithOrderTime(1900)
                    .WithStandards(CustomerStandard.VeryLow)
                    .AllowDirectApproach(true)
                    .GuaranteeFirstSample(true)
                    .WithMutualRelationRequirement(2.0f, 3.5f)
                    .WithCallPoliceChance(0.22f)
                    .WithDependence(0.20f, 1.15f)
                    .WithAffinities(new[]
                    {
                        (DrugType.Marijuana, 0.25f),
                        (DrugType.Cocaine, 0.70f)
                    })
                    .WithPreferredProperties(Property.CalorieDense, Property.Laxative, Property.Cyclopean);
            })
            .WithRelationshipDefaults(relationship =>
            {
                relationship.WithDelta(1.5f)
                    .SetUnlocked(true)
                    .SetUnlockType(NPCRelationship.UnlockType.DirectApproach);
            })
            .WithSchedule(plan =>
            {
                plan.EnsureDealSignal()
                    .WalkTo(HangoutPosition, 1100, faceDestinationDir: true)
                    .StayInBuilding(Building.Get<NorthApartments>(), 1400, 180)
                    .UseVendingMachine(1800)
                    .WalkTo(SpawnPosition, 2200, faceDestinationDir: true);
            });
    }
}
