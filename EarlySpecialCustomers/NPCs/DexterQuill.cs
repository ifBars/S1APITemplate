using S1API.Economy;
using S1API.Entities;
using S1API.Entities.Schedule;
using S1API.GameTime;
using S1API.Map.Buildings;
using S1API.Products;
using S1API.Properties;
using UnityEngine;

namespace EarlySpecialCustomers.NPCs;

/// <summary>A quality-sensitive experimenter who rewards learning product effects early.</summary>
public sealed class DexterQuill : EarlySpecialCustomer
{
    private static readonly Vector3 SpawnPosition = new(-53.67f, 1.063f, 84.8433f);
    private static readonly Vector3 HangoutPosition = new(-64.6576f, 1.065f, 51.3718f);

    protected override string IntroMessage =>
        "Names don't impress me. Effects do. Bring me something interesting and don't cut corners.";

    protected override string MilestoneMessage =>
        "That last batch had potential. Keep experimenting; I'm paying attention.";

    protected override int DealsPerMilestone => 2;

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity("esc_dexter_quill", "Dexter", "Quill")
            .WithAppearanceDefaults(appearance =>
            {
                appearance.Gender = 0f;
                appearance.Height = 1.03f;
                appearance.Weight = 0.30f;
                appearance.SkinColor = new Color32(171, 132, 103, 255);
                appearance.LeftEyeLidColor = appearance.SkinColor;
                appearance.RightEyeLidColor = appearance.SkinColor;
                appearance.EyeBallTint = new Color(0.92f, 0.96f, 1f);
                appearance.PupilDilation = 0.38f;
                appearance.HairColor = new Color(0.05f, 0.05f, 0.05f);
                appearance.HairPath = "Avatar/Hair/BuzzCut/BuzzCut";
                appearance.WithFaceLayer("Avatar/Layers/Face/Face_Agitated", Color.black);
                appearance.WithBodyLayer("Avatar/Layers/Top/RolledButtonUp", new Color(0.78f, 0.82f, 0.84f));
                appearance.WithBodyLayer("Avatar/Layers/Bottom/CargoPants", new Color(0.16f, 0.18f, 0.18f));
                appearance.WithAccessoryLayer("Avatar/Accessories/Head/SmallRoundGlasses/SmallRoundGlasses", Color.black);
                appearance.WithAccessoryLayer("Avatar/Accessories/Feet/Sneakers/Sneakers", new Color(0.20f, 0.20f, 0.22f));
            })
            .WithSpawnPosition(SpawnPosition)
            .EnsureCustomer()
            .WithCustomerDefaults(customer =>
            {
                customer.WithSpending(500f, 1100f)
                    .WithOrdersPerWeek(1, 3)
                    .WithPreferredOrderDay(Day.Wednesday)
                    .WithOrderTime(1500)
                    .WithStandards(CustomerStandard.High)
                    .AllowDirectApproach(true)
                    .GuaranteeFirstSample(true)
                    .WithMutualRelationRequirement(1.5f, 3.0f)
                    .WithCallPoliceChance(0.10f)
                    .WithDependence(0.12f, 1.10f)
                    .WithAffinities(new[]
                    {
                        (DrugType.Marijuana, 0.35f),
                        (DrugType.Cocaine, 0.45f)
                    })
                    .WithPreferredProperties(Property.ThoughtProvoking, Property.Energizing, Property.AntiGravity);
            })
            .WithRelationshipDefaults(relationship =>
            {
                relationship.WithDelta(1.25f)
                    .SetUnlocked(true)
                    .SetUnlockType(NPCRelationship.UnlockType.DirectApproach);
            })
            .WithSchedule(plan =>
            {
                plan.EnsureDealSignal()
                    .UseVendingMachine(900)
                    .WalkTo(HangoutPosition, 1000, faceDestinationDir: true)
                    .StayInBuilding(Building.Get<UpscaleApartments>(), 1200, 180)
                    .WalkTo(SpawnPosition, 1800, faceDestinationDir: true);
            });
    }
}
