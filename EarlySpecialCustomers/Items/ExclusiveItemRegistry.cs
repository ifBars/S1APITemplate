using System;
using MelonLoader;
using S1API.Console;
using S1API.Items;
using S1API.Money;
using RovingSpecialCustomers.Models;

namespace RovingSpecialCustomers.Items;

public static class ExclusiveItemRegistry
{
    private const string BaseClothingId = "cap";
    private const string BaseAccessoryPath = "Avatar/Accessories/Head/Cap/Cap";
    private static bool _initialized;

    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        CreateCap("rsc_hippie_cap", "Sunwashed Tour Cap", "A faded cap sold only by the roving hippie crew.", ClothingColor.Lime, 325f);
        CreateCap("rsc_executive_cap", "Executive Travel Cap", "A restrained black cap sold only by the visiting business delegation.", ClothingColor.Black, 650f);
        CreateCap("rsc_road_cap", "Road Crew Cap", "A charcoal cap sold only by the roving road crew.", ClothingColor.Charcoal, 500f);
        CreateCap("rsc_party_cap", "Afterparty Cap", "A loud cap sold only by the party bus crew.", ClothingColor.HotPink, 550f);
        _initialized = true;
    }

    public static bool TryPurchase(CrewDefinition definition, out string message)
    {
        var item = ItemManager.GetItemDefinition(definition.ExclusiveItemId);
        if (item is null)
        {
            message = $"The exclusive item '{definition.ExclusiveItemName}' is unavailable.";
            return false;
        }

        var cash = Money.GetCashBalance();
        if (cash < definition.ExclusiveItemPrice)
        {
            message = $"You need ${definition.ExclusiveItemPrice:0} in cash for {definition.ExclusiveItemName}.";
            return false;
        }

        Money.ChangeCashBalance(-definition.ExclusiveItemPrice, visualizeChange: true, playCashSound: true);
        ConsoleHelper.AddItemToInventory(definition.ExclusiveItemId, 1);
        message = $"Purchased {definition.ExclusiveItemName} for ${definition.ExclusiveItemPrice:0}.";
        return true;
    }

    private static void CreateCap(string id, string name, string description, ClothingColor color, float price)
    {
        try
        {
            if (ItemManager.GetItemDefinition(id) is not null)
            {
                return;
            }

            var builder = ClothingItemCreator.CloneFrom(BaseClothingId);
            if (builder is null)
            {
                MelonLogger.Error($"[RSC] Could not clone '{BaseClothingId}' for '{id}'.");
                return;
            }

            builder.WithBasicInfo(id, name, description)
                .WithClothingAsset(BaseAccessoryPath)
                .WithColorable(false)
                .WithDefaultColor(color)
                .WithPricing(price, 0.5f)
                .Build();
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[RSC] Failed to create exclusive item '{id}': {ex.Message}");
        }
    }
}
