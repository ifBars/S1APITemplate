# NPC Creation

Use S1API first for custom NPCs, customers, dealers, relationships, dialogue, schedules, and persisted state.

## Workflow

1. Define stable NPC IDs and constants.
2. Classify as customer, dealer, generic NPC, or quest actor.
3. Use S1API builders for identity, appearance, schedule, relationship, customer/dealer data, inventory.
4. Register at the lifecycle point S1API expects.
5. Separate first creation from save/load restoration.
6. Validate spawn, interaction, schedule, save, reload, and shipped runtimes.

## Design rules

- Prefer builders over large mutable setup blocks.
- Keep dialogue and schedule definitions data-like and readable.
- Resolve relationships by IDs/types, not live NPC instances in prefab setup.
- Centralize IDs, variable names, dialogue keys, and spawn locations.
- Use vanilla NPCs as local references; do not publish decompiled code.

## Common failure points

- Registering before the required game/S1API lifecycle.
- Static state instead of saveable state.
- Referencing a live NPC during prefab setup.
- Schedule actions targeting missing regions/surfaces/scene objects.
- Appearance paths or resources copied from stale dumps.
- Mono-only code paths in an IL2CPP build.

## Evidence to inspect

- Similar vanilla NPC/customer/dealer classes.
- S1API builders/examples in the local repo.
- Runtime logs for registration or prefab errors.
- AssetRipper/runtime inspection only for local appearance/prefab path questions.
