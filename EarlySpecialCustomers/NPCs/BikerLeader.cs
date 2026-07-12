using S1API.Economy;
using S1API.Entities;
using S1API.Products;
using S1API.Properties;
using UnityEngine;

namespace RovingSpecialCustomers.NPCs;

public sealed class BikerLeader : RovingCrewLeader
{
    protected override string CrewId => "bikers";
    protected override string DialogueContainerId => "RSC_Biker_Visit";

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity("rsc_biker_leader", "Rook", "Maddox")
            .WithSpawnPosition(new Vector3(0f, Utils.Constants.World.HiddenY, 0f))
            .WithAppearanceDefaults(appearance =>
            {
                appearance.Gender = 0f;
                appearance.Height = 1.08f;
                appearance.Weight = 0.74f;
                appearance.SkinColor = new Color32(164, 126, 96, 255);
                appearance.LeftEyeLidColor = appearance.SkinColor;
                appearance.RightEyeLidColor = appearance.SkinColor;
                appearance.HairColor = new Color(0.06f, 0.04f, 0.03f);
                appearance.HairPath = "Avatar/Hair/BuzzCut/BuzzCut";
                appearance.WithFaceLayer("Avatar/Layers/Face/Face_Agitated", Color.black);
                appearance.WithBodyLayer("Avatar/Layers/Top/FlannelButtonUp", new Color(0.30f, 0.04f, 0.04f));
                appearance.WithBodyLayer("Avatar/Layers/Bottom/Jeans", new Color(0.08f, 0.10f, 0.14f));
                appearance.WithAccessoryLayer("Avatar/Accessories/Feet/CombatBoots/CombatBoots", Color.black);
            })
            .EnsureCustomer()
            .WithCustomerDefaults(customer =>
            {
                customer.WithSpending(0f, 0f).WithOrdersPerWeek(0, 0).WithStandards(CustomerStandard.Low)
                    .AllowDirectApproach(false).WithCallPoliceChance(0f).WithDependence(0f, 0f)
                    .WithAffinities(new[] { (DrugType.Marijuana, -0.5f), (DrugType.Shrooms, -1f), (DrugType.Methamphetamine, 1f), (DrugType.Cocaine, -0.5f), (DrugType.MDMA, -1f), (DrugType.Heroin, 0.8f) })
                    .WithPreferredProperties(Property.Energizing);
            })
            .WithRelationshipDefaults(relationship => relationship.SetUnlocked(true))
            .WithSchedule(plan => plan.EnsureDealSignal());
    }
}
