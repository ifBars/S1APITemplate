# Early Special Customers

A Schedule I mod built on S1API.Forked that introduces three distinct customer NPCs from the beginning of a save.

## Customers

| Customer | Early-game role | Buying behavior |
| --- | --- | --- |
| Mara Voss | Reliable starter regular | Frequent, smaller marijuana-focused orders with moderate standards |
| Dexter Quill | Product-effect challenge | Less frequent, higher-quality orders centered on unusual effects |
| Rico Stacks | Early bulk target | Larger, less frequent, low-standard orders with more risk |

All three are physical Northtown NPCs, begin unlocked, ignore region gating, use S1API schedules/customer defaults, generate mugshots from their appearance, and persist introduction/deal-milestone state.

## Build

From the repository root:

```powershell
.\setup.ps1
dotnet build .\EarlySpecialCustomers\EarlySpecialCustomers.csproj -c CrossCompat
```

The recommended output is `EarlySpecialCustomers.dll`. Players need MelonLoader and S1API installed.

## Preferences

MelonPreferences category: `EarlySpecialCustomers`

- `IntroMessages`: one introduction per customer per save.
- `MilestoneMessages`: occasional follow-up messages after completed deals.
- `DebugLogs`: completed-deal logging.

## Manual validation

1. Start a new save and confirm all three contacts are unlocked.
2. Confirm each NPC spawns in Northtown and executes its schedule.
3. Confirm each appearance and mugshot builds correctly.
4. Complete deals and verify standard customer behavior plus milestone messages.
5. Save, return to menu, and reload; introduction messages should not repeat.
6. Validate Mono/CrossCompat and Il2cpp separately.
7. Test as host and as a joining client.

## Safety boundary

The project uses the repository's `schedule-one-modding` and `schedule-one-custom-npcs` skills. No Schedule I assemblies, generated IL2CPP wrappers, decompiled dumps, AssetRipper exports, prefabs, scenes, textures, private logs, API keys, or local machine paths are committed.
