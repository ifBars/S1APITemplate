# Roving Special Customers

A Schedule I mod prototype based on the developer-previewed special-customer update described in the supplied video transcript.

This is **not** a collection of permanent starter customers. It implements periodic, temporary customer crews:

- Hippies: marijuana and shrooms.
- Business delegation: cocaine.
- Road crew: methamphetamine and heroin.
- Party bus: MDMA and shrooms.

## Prototype loop

1. One crew is scheduled roughly every 5–9 in-game days. The first visit can occur early.
2. The Travel Wire contact sends a notice 2–3 days before arrival.
3. Each visit rolls 1–3 preferred effects.
4. The crew appears with a lead buyer, a companion, and a runtime-selected vehicle.
5. The mod selects the best currently listed product in the crew's accepted drug categories.
6. It generates one native bulk contract with a rank-scaled budget and up to a 50% preferred-effect premium.
7. The crew also sells one visit-exclusive cosmetic.
8. The crew leaves on the following in-game day.

Native S1API/base-game contracts own acceptance, delivery, handover, payment, receipts, save behavior, and transaction RPCs.

## Build

Copy `example.build.props` to `local.build.props` and correct the install paths.

```powershell
dotnet build .\RovingSpecialCustomers.csproj -c Mono
dotnet build .\RovingSpecialCustomers.csproj -c Il2cpp
```

- `Mono` targets the alternate/alternate-beta branch.
- `Il2cpp` targets the default/beta branch with generated MelonLoader wrappers.
- Players need MelonLoader and S1API.Forked at runtime.

## Current limitations

- This is an independently implemented prototype based on non-final preview information, not leaked developer source.
- One lead buyer represents the crew's combined budget. A companion and vehicle provide the group presentation.
- The first prototype is host-authoritative. Clients should see networked NPCs/vehicles and native contract state, but host-only interaction is the supported test path.
- Arrival uses a text notice rather than a custom cutscene.
- Exclusive items are themed variants built from an existing cap resource; no custom or extracted assets are included.
- Product selection is based on products currently listed for sale when the crew arrives or when the player asks the lead buyer to check again.

## Manual validation

Test each backend separately:

1. Start a new save and verify a Travel Wire contact exists.
2. Confirm a notice arrives before the first visit.
3. Confirm only the active crew and companion move into Northtown.
4. Confirm a vehicle spawns and is removed when the crew departs.
5. List accepted and rejected drug types and verify only accepted types are selected.
6. Compare a no-effect match and full-effect match; the latter should pay up to 50% more.
7. Accept and complete the native bulk contract.
8. Purchase the exclusive item once; verify a second purchase is rejected during that visit.
9. Save/reload before notice, during a visit, after accepting a deal, and after departure.
10. Repeat on Mono and IL2CPP, then test as host with one client.

## Public-safety boundary

The repository contains no Schedule I assemblies, generated IL2CPP wrappers, decompiled dumps, prefabs, scenes, textures, downloaded videos, API keys, or local machine paths. The private assembly repositories are development/build evidence only.
