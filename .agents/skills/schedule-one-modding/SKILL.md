---
name: schedule-one-modding
description: Professional Schedule One mod development with C#, Unity, MelonLoader, Harmony, Mono/IL2CPP, S1API, MAPI, SteamNetworkLib, Dedicated Server mods, local decompilation, logs, and prefab/asset inspection. Use when creating, debugging, refactoring, reviewing, packaging, or auditing Schedule One mods or modding workflows.
---

# Schedule One Mod Development

Work from evidence and keep the mod shape small. This skill is public-safe: never embed, commit, package, or redistribute Schedule One assemblies, decompiled dumps, generated IL2CPP assemblies, AssetRipper exports, prefabs, textures, scenes, or other proprietary game files. Ask for user-owned local paths and inspect them locally. When the user asks for game code or ripped assets, explicitly say the artifact cannot include proprietary game files/code and offer a local-inspection workflow instead.

## Fast Router

1. Classify the task first:
   - Patch mod: Harmony around a narrow runtime seam.
   - Feature mod: services, config, UI, scene/runtime systems.
   - Content mod: S1API items, NPCs, quests, saveables, shops, products.
   - Building mod: MAPI buildings, interiors, GLTF, prefab/model loading.
   - Multiplayer mod: SteamNetworkLib/FishNet/Steamworks sync and authority.
   - Dedicated server mod: client/server split, headless safety, server authority.
   - Vanilla mod: direct game patches when helper APIs are too limiting.
2. Confirm runtime: Mono, IL2CPP, CrossCompat, client, server, or mixed. For user reports, map Steam branches first: `none`/`beta` usually IL2CPP; `alternate`/`alternate-beta` usually Mono.
3. Check requirements when the user only has an agent or a fresh machine: .NET SDK, `ilspycmd`, game path/logs, optional editor, optional AssetRipper/Unity. State that Visual Studio/Rider are optional for agent-driven work.
4. If the answer depends on game code, prefab hierarchy, generated wrappers, logs, or runtime state, inspect local evidence before designing the change.
5. Choose the lightest abstraction that fits: S1API, MAPI, SteamNetworkLib, DedicatedServerMod patterns, or direct Harmony.
6. Name the owning lifecycle hook, patch point, save/load path, network handler, or asset path before editing.

## Reference Selection

- `references/local-game-introspection.md`: requirements, user paths, logs, generated assemblies, `ilspycmd`, safe evidence capture, and probe script.
- `references/community-wiki.md`: local wiki routing, Steam branch naming, logs, saves, setup, publishing, and community tools.
- `references/decompilation-workflow.md`: focused `ilspycmd` inspection and Mono/IL2CPP comparison.
- `references/assetripper-workflow.md`: AssetRipper, prefab/resource inspection, Unity project export, script limitations.
- `references/build-config.md`: csproj setup, target frameworks, references, local build properties.
- `references/il2cpp-modding.md`: imports, casts, delegates, injected types, collections, Harmony limits.
- `references/s1api-reference.md`: S1API wrappers, builders, lifecycle hooks, saveables.
- `references/dedicated-server-reference.md`: server/client boundaries, headless safety, authority, commands, config, messages.
- `references/mapi-reference.md`: buildings, interiors, prefabs, models.
- `references/steamnetworklib-reference.md`: multiplayer authority, messaging, sync variables, and validation.
- `references/npc-creation.md`, `references/ui-patterns.md`, `references/vanilla-modding.md`: load only when that domain is active.

Only read the references needed for the current task.

## Default Engineering Shape

- Keep `Core.cs` focused on lifecycle wiring.
- Keep Harmony patches thin; delegate real work to services/adapters.
- Centralize IDs, config keys, scene names, resource names, and log tags.
- Separate runtime state, scene state, persisted state, synchronized state, and user config.
- Prefer S1API-style builders: `With...` methods, safe defaults, `Build()` validation, and framework quirks hidden inside.
- Hide runtime-specific quirks behind helpers for delegates, casts, lookup, assets, generated methods, and network authority.
- Use native game registries, screens, save flows, and lifecycle hooks when they are a close fit.

## S1API NPC Appearance and Impostors

- Treat avatar impostors as prefab appearance defaults, not live appearance changes. Configure them through `NPCPrefabBuilder.WithAppearanceDefaults(...)`, then keep runtime `NPCAppearance` changes focused on the active avatar, clothing, mugshots, and layers.
- For reusable game-owned impostors, expose a public catalog/definition API and keep resolution logic internal. S1API's pattern is `NPCImpostorCatalog`, `AvatarImpostorDefinition`, builder methods such as `WithImpostor(...)`, `WithImpostorTexture(...)`, and `WithRandomImpostor(...)`, plus an internal resolver that handles names, paths, deterministic random selection, and fallback logging.
- When testing custom impostor textures, prefer an embedded assembly resource loaded into a `Texture2D` with existing texture utilities over packaging a one-off asset bundle, unless the mod already needs an asset bundle for other assets.
- Preserve vanilla behavior by default. If no impostor override is configured, leave the base prefab's existing impostor texture intact rather than replacing it with a guessed default.
- If reliable runtime discovery is available, prefer it over a hard-coded catalog. If discovery is incomplete, use a small known-name/path fallback and log misses clearly so catalog drift is visible without breaking prefab creation.

## High-Risk Checks

- Do not infer IL2CPP behavior from Mono source alone; inspect generated wrappers or logs.
- Do not use IL transpilers for IL2CPP builds; prefer prefix/postfix or manually resolved method patches.
- Do not treat a Mono pass as proof of IL2CPP compatibility, or an IL2CPP pass as proof of Mono compatibility; plan separate validation for each runtime.
- For IL2CPP injected components, keep internal selection/configuration objects out of generated Il2Cpp surfaces. If a `MonoBehaviour` or injected type has members that return or accept custom managed helper types, mark those members with `HideFromIl2Cpp` when they are only called from managed mod/API code.
- Do not assume `Registry.GetItem(...)`, scene objects, canvases, shops, or network objects exist during `OnInitializeMelon()`.
- Do not keep gameplay state only in static fields if it must survive save/load.
- Do not publish local decompiled code or AssetRipper output; summarize findings as names, signatures, object paths, short snippets, and behavior.

## Silent-Failure Checklist

1. The DLL is not in `<game>\Mods`, or MelonLoader did not log it as loaded.
2. The mod was built for the wrong backend.
3. The lifecycle hook is too early.
4. Build references or symbols do not match the tested runtime.
5. Conditional compilation missed imports, casts, delegates, or injected types.
6. Static flags or event handlers duplicated across scene reloads.
7. Visual/material/UI changes were not applied to the active instance.
8. Save/load restoration covers creation but not reload.

## Output Expectations

- State Mono-only, IL2CPP-only, or dual-runtime compatibility.
- State the explicit public-safety boundary when decompilation, generated wrappers, or AssetRipper output are involved: no assemblies, decompiled dumps, generated IL2CPP wrappers, prefabs, scenes, textures, or exported Unity projects will be committed, packaged, or redistributed.
- State the local evidence inspected, or say what remains unverified.
- Keep changes minimal and reviewable.
- For messy mods, steer toward thin patches, isolated services, centralized constants/logging, and explicit validation commands.
