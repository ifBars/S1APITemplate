using System;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using RovingSpecialCustomers.Models;
using RovingSpecialCustomers.NPCs;
using UnityEngine;

#if MONO
using S1Customer = ScheduleOne.Economy.Customer;
using S1Effect = ScheduleOne.Effects.Effect;
using S1LevelManager = ScheduleOne.Levelling.LevelManager;
using S1Lobby = ScheduleOne.Networking.Lobby;
using S1TimeManager = ScheduleOne.GameTime.TimeManager;
using S1LandVehicle = ScheduleOne.Vehicles.LandVehicle;
using S1VehicleManager = ScheduleOne.Vehicles.VehicleManager;
#elif IL2CPP
using S1Customer = Il2CppScheduleOne.Economy.Customer;
using S1Effect = Il2CppScheduleOne.Effects.Effect;
using S1LevelManager = Il2CppScheduleOne.Levelling.LevelManager;
using S1Lobby = Il2CppScheduleOne.Networking.Lobby;
using S1TimeManager = Il2CppScheduleOne.GameTime.TimeManager;
using S1LandVehicle = Il2CppScheduleOne.Vehicles.LandVehicle;
using S1VehicleManager = Il2CppScheduleOne.Vehicles.VehicleManager;
#endif

namespace RovingSpecialCustomers.Services;

public static class SpecialCustomerRuntime
{
    private static S1LandVehicle? _activeVehicle;

    public static bool IsReady
    {
        get
        {
            try
            {
                return S1TimeManager.Instance is not null && S1LevelManager.Instance is not null;
            }
            catch
            {
                return false;
            }
        }
    }

    public static bool IsHost
    {
        get
        {
            try
            {
                return S1Lobby.Instance is null || S1Lobby.Instance.IsHost;
            }
            catch
            {
                return true;
            }
        }
    }

    public static int ElapsedDays => S1TimeManager.Instance.ElapsedDays;
    public static int CurrentTime => S1TimeManager.Instance.CurrentTime;

    public static float GetRankBudgetMultiplier()
    {
        try
        {
            return S1LevelManager.GetOrderLimitMultiplier(S1LevelManager.Instance.GetFullRank());
        }
        catch
        {
            return 1f;
        }
    }

    public static void ApplyVisitEffects(RovingCrewLeader leader, IReadOnlyCollection<string> effectIds)
    {
        try
        {
            var customer = leader.gameObject.GetComponent<S1Customer>();
            if (customer is null || customer.CustomerData is null)
            {
                MelonLogger.Warning($"[RSC] Customer data missing for {leader.ID}.");
                return;
            }

            customer.CustomerData.PreferredProperties.Clear();
            var requested = new HashSet<string>(effectIds, StringComparer.OrdinalIgnoreCase);
            var resolved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var paths = new[] { "Properties/Tier1", "Properties/Tier2", "Properties/Tier3", "Properties/Tier4", "Properties/Tier5" };

            foreach (var path in paths)
            {
                var effects = Resources.LoadAll<S1Effect>(path);
                for (var index = 0; index < effects.Length; index++)
                {
                    var effect = effects[index];
                    if (effect is null || !requested.Contains(effect.ID) || !resolved.Add(effect.ID))
                    {
                        continue;
                    }

                    customer.CustomerData.PreferredProperties.Add(effect);
                }
            }

            foreach (var missing in requested.Where(id => !resolved.Contains(id)))
            {
                MelonLogger.Warning($"[RSC] Could not resolve effect asset '{missing}'.");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[RSC] Failed to apply visit effects: {ex.Message}");
        }
    }

    public static void SpawnCrewVehicle(CrewDefinition definition)
    {
        DestroyCrewVehicle();

        try
        {
            var manager = S1VehicleManager.Instance;
            if (manager is null)
            {
                return;
            }

            var code = ResolveVehicleCode(manager, definition.VehicleHints);
            if (string.IsNullOrWhiteSpace(code))
            {
                MelonLogger.Warning($"[RSC] No vehicle prefab matched {definition.DisplayName}; continuing without a vehicle.");
                return;
            }

            _activeVehicle = manager.SpawnAndReturnVehicle(code, definition.VehiclePosition, definition.VehicleRotation, playerOwned: false);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[RSC] Failed to spawn crew vehicle: {ex.Message}");
        }
    }

    public static void DestroyCrewVehicle()
    {
        try
        {
            if (_activeVehicle is not null)
            {
                _activeVehicle.DestroyVehicle();
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[RSC] Failed to remove crew vehicle: {ex.Message}");
        }
        finally
        {
            _activeVehicle = null;
        }
    }

    private static string? ResolveVehicleCode(S1VehicleManager manager, IReadOnlyList<string> hints)
    {
        for (var hintIndex = 0; hintIndex < hints.Count; hintIndex++)
        {
            var hint = hints[hintIndex];
            for (var prefabIndex = 0; prefabIndex < manager.VehiclePrefabs.Count; prefabIndex++)
            {
                var prefab = manager.VehiclePrefabs[prefabIndex];
                if (prefab is null || string.IsNullOrWhiteSpace(prefab.VehicleCode))
                {
                    continue;
                }

                if (prefab.VehicleCode.Contains(hint, StringComparison.OrdinalIgnoreCase))
                {
                    return prefab.VehicleCode;
                }
            }
        }

        return manager.GetVehiclePrefab("shitbox") is not null ? "shitbox" : null;
    }
}
