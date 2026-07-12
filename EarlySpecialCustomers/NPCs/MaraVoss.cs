using S1API.Economy;
using S1API.Entities;
using S1API.Entities.Schedule;
using S1API.GameTime;
using S1API.Map.Buildings;
using S1API.Products;
using S1API.Properties;
using UnityEngine;

namespace EarlySpecialCustomers.NPCs;

/// <summary>A forgiving, frequent buyer intended to smooth out the first few in-game weeks.</summary>
public sealed class MaraVoss : EarlySpecialCustomer
{
    private static readonly Vector3 SpawnPosition = new(-53.5701f, 1.065f, 67.7955f);
    private static readonly Vector3 HangoutPosition = new(-28.060f, 1.065f, 62.070f);

    protected override string IntroMessage =>
        "You're the new supplier, right? I buy small, but I buy often. Keep it simple and we'll get along.";

    protected override string MilestoneMessage =>
        "You've been reliable. I'll keep checking in when I need another order.";

    protected override int DealsPerMilestone => 3;

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity("esc_mara_voss", "Mara", "Voss")
            .WithAppearanceDefaults(appearance =>
            {
                appearance.Gender = 1f;
                appearance.Height = 0.96f;
                appearance.Weight = 0.42f;
                appearance.SkinColor = new Color32(196, 151, 116, 255);
                appearance.LeftEyeLidColor = appearance.SkinColor;
                appearance.RightEyeLidColor = appearance.SkinColor;
                appearance.EyeBallTint = Color.white;
                appearance.PupilDilation = 0.58f;
                appearance.HairColor = new Color(0.18f, 0.08f, 0.04f);
                appearance.HairPath = "Avatar/Hair/bowlcut/BowlCut";
                appearance.WithFaceLayer("Avatar/Layers/Face/Freckles", new Color(0.25f, 0.12f, 0.08f));
                appearance.WithBodyLayer("Avatar/Layers/Top/T-Shirt", new Color(0.20f, 0.48f, 0.32f));
                appearance.WithBodyLayer("Avatar/Layers/Bottom/Jeans", new Color(0.12f, 0.18f, 0.30f));
                appearance.WithAccessoryLayer("Avatar/Accessories/Feet/Sneakers/Sneakers", Color.white);
            })
            .WithSpawnPosition(SpawnPosition)
            .EnsureCustomer()
            .WithCustomerDefaults(customer =>
            {
                customer.WithSpending(250f, 550f)
                    .WithOrdersPerWeek(2, 4)
                    .WithPreferredOrderDay(Day.Monday)
                    .WithOrderTime(900)
                    .WithStandards(CustomerStandard.Moderate)
                    .AllowDirectApproach(true)
                    .GuaranteeFirstSample(true)
                    .WithMutualRelationRequirement(1.0f, 2.5f)
                    .WithCallPoliceChance(0.05f)
                    .WithDependence(0.08f, 1.05f)
                    .WithAffinities(new[]
                    {
                        (DrugType.Marijuana, 0.80f),
                        (DrugType.Cocaine, -0.40f)
                    })
                    .WithPreferredProperties(Property.Munchies, Property.CalorieDense, Property.BrightEyed);
            })
            .WithRelationshipDefaults(relationship =>
            {
                relationship.WithDelta(1.0f)
                    .SetUnlocked(true)
                    .SetUnlockType(NPCRelationship.UnlockType.DirectApproach);
            })
            .WithSchedule(plan =>
            {
                plan.EnsureDealSignal()
                    .WalkTo(HangoutPosition, 800, faceDestinationDir: true)
                    .StayInBuilding(Building.Get<NorthApartments>(), 1030, 120)
                    .UseVendingMachine(1330)
                    .WalkTo(SpawnPosition, 1700, faceDestinationDir: true);
            });
    }
}
