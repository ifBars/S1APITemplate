using S1API.Entities;
using UnityEngine;

namespace RovingSpecialCustomers.NPCs;

public sealed class TravelAssistant : RovingCrewMember
{
    protected override string CrewId => "*";
    protected override Vector3 VisitOffset => new(1.6f, 0f, 0.8f);

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity("rsc_travel_assistant", "Road", "Assistant")
            .WithSpawnPosition(new Vector3(0f, Utils.Constants.World.HiddenY, 0f))
            .WithAppearanceDefaults(appearance =>
            {
                appearance.Gender = 0.45f;
                appearance.Height = 0.98f;
                appearance.Weight = 0.45f;
                appearance.SkinColor = new Color32(177, 137, 105, 255);
                appearance.LeftEyeLidColor = appearance.SkinColor;
                appearance.RightEyeLidColor = appearance.SkinColor;
                appearance.HairColor = new Color(0.12f, 0.07f, 0.04f);
                appearance.HairPath = "Avatar/Hair/BuzzCut/BuzzCut";
                appearance.WithBodyLayer("Avatar/Layers/Top/T-Shirt", new Color(0.20f, 0.20f, 0.22f));
                appearance.WithBodyLayer("Avatar/Layers/Bottom/Jeans", new Color(0.12f, 0.17f, 0.24f));
                appearance.WithAccessoryLayer("Avatar/Accessories/Feet/Sneakers/Sneakers", Color.white);
            });
    }
}
