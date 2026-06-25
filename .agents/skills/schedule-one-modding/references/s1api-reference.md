# S1API Reference

Use S1API for cross-runtime content: items, NPCs, quests, saveables, products, storage, maps, wrappers, lifecycle, registries.

## Use S1API when

- Creating/registering custom content or NPC/customer/dealer data.
- Persisting custom entities.
- Hooking known lifecycle points instead of guessing scene timing.
- Avoiding direct Mono/IL2CPP differences where S1API already abstracts them.

Use direct Harmony only when S1API lacks the seam or the bug is vanilla runtime behavior.

## Style anchor

Prefer S1API-style fluent builders:

- `With...` methods return the builder.
- Constructors set safe defaults.
- `Build()` validates required identity/config.
- Framework/Unity quirks stay inside the builder.
- Overloads accept friendly inputs when useful.

## Lifecycle

- Registry-backed content is not safe during `OnInitializeMelon()`.
- Prefer the lifecycle event that matches the content type.
- Separate first creation from load restoration.
- Reset one-time static guards when returning to menu if the mod can reload.

## Content rules

- Centralize IDs and save keys.
- Clone similar vanilla definitions, then override only the fields that differ.
- Use `ScriptableObject.CreateInstance<T>()` for ScriptableObject types.
- Keep saveable state out of static fields.
- Log registration failures with item/NPC/quest IDs.

## Validation

- Build target runtime config.
- Inspect S1API loader/registration logs.
- Test creation, reload, and IL2CPP separately when shipping IL2CPP/CrossCompat.
