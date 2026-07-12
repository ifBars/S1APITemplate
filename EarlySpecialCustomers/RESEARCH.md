# Research and implementation basis

## Supplied preview transcript

The intended update describes roving groups, approximately one visit per week, that:

- can arrive at any player level;
- exclusively buy one or two drug categories per group;
- randomize favorite effects each visit;
- buy bulk quantities and serve as an excess-inventory sink;
- pay a substantial effect-match bonus;
- may scale spending with rank;
- provide 2–3 days of advance warning; and
- sell group-exclusive mostly cosmetic items.

The four previewed groups are hippies, businessmen, bikers, and party-bus visitors. The preview explicitly states that details are non-final.

## Exact assembly inspection

`ilspycmd 10.1.0.8386` was run against the user's current private assembly repositories:

- Mono: `Managed/Assembly-CSharp.dll`
- IL2CPP: `MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll`

Focused inspection covered TimeManager, Customer, CustomerData, ProductDefinition, ProductItemInstance, HandoverScreen, LevelManager, ShopInterface, CartelDealManager, VehicleManager, LandVehicle, and SaveManager.

Findings used by the prototype:

- No current Mono or IL2CPP type/string evidence contains an unreleased special-customer system to unlock.
- TimeManager exposes elapsed day/current time and weekly/day events on both backends.
- Customer enjoyment already combines drug affinity, preferred effects, and quality.
- Native customer orders scale spending with LevelManager's order-limit multiplier.
- S1API exposes explicit ContractInfo and NPCCustomer.OfferContract wrappers on both backends.
- Product wrappers expose listed products, drug type, properties, market value, and stable IDs.
- VehicleManager exposes runtime prefab discovery and server-side vehicle spawning.
- Native contracts and customer handovers already provide the networked transaction path.

## Engineering decision

Following the repository-local `schedule-one-modding` skill, the implementation uses:

- S1API for NPCs, customer components, products, contracts, items, messages, and persisted fields;
- a small backend adapter for exact TimeManager, LevelManager, Effect asset, Lobby, and VehicleManager access;
- no IL transpilers;
- no broad Harmony patching; and
- separate Mono and IL2CPP builds and validation.
