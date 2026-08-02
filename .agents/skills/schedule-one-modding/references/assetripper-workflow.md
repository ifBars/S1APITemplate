# AssetRipper Workflow

Use AssetRipper for local prefab, resource, scene, material, mesh, sprite, and serialized-reference inspection.

## Evidence model

Reconstruct a base-game system from three complementary evidence streams:

1. Read focused C# or generated wrappers for lifecycle, state transitions, and callable seams.
2. Read the AssetRipper Unity project for serialized composition: hierarchy, components, object references, configured values, materials, animations, and scene overrides.
3. Confirm the active runtime instance through logs, hierarchy inspection, screenshots, or a narrow manual test.

Do not treat any one stream as the complete system. C# may omit inspector configuration, an exported prefab may omit scene overrides, and a runtime screenshot may hide the controller that produced the result.

## Safety

- Do not commit or publish exports, dumped prefabs, Unity projects, textures, meshes, scenes, shaders, or game resources.
- Keep exports in ignored local folders.
- Treat output as user-local inspection only. Do not package ripped prefabs/resources; recreate behavior with your own assets or load user-owned local resources at runtime.
- Report only narrow findings: names, paths, component lists, screenshots, or short summaries.

## Script/code expectation

AssetRipper is not the code tool. Scripts are assembly-backed; use `ilspycmd` for Mono assemblies and generated IL2CPP wrappers. IL2CPP script associations may need compatible Cpp2IL-style assemblies; Il2CppInterop modding assemblies are not a general substitute.

## Focused project inspection

1. Start from the visible behavior, relevant class, log frame, object name, UI text, resource name, or scene.
2. Search the smallest relevant export subtree first. On Windows, use windows-fast-search or `rg` for exact identifiers and likely Unity files such as `.prefab`, `.unity`, `.asset`, `.mat`, `.controller`, and `.anim`.
3. Inspect the prefab or scene hierarchy and record component types, active states, transforms, serialized values, and referenced objects.
4. Follow serialized references into nested prefabs, prefab variants, materials, animation controllers, clips, sprites, and ScriptableObjects. Resolve scene-instance or variant overrides before treating prefab defaults as runtime truth.
5. Compare the serialized component graph with the controlling C# lifecycle. Identify which behavior comes from code and which comes from inspector wiring.
6. Reproduce the smallest stable seam: reuse a native controller or lifecycle when appropriate, but provide mod-owned data and assets. If the native presentation is too coupled to game-owned objects, recreate only the observable sequence.
7. Validate the active runtime instance. Confirm hierarchy, transforms, timing, material appearance, input locks, and cleanup rather than stopping at a successful build.

For cutscenes and scripted presentations, trace the trigger, controller, camera target/root, movement path, animation or timeline, fades, text timing, skip input, player/input locking, termination callback, and final teleport or world state.

For imported-model rendering problems, inspect renderer material slots, the exported `.mat` files, shader names, texture assignments, tint/emission/alpha properties, and runtime shader translation. Grey or washed-out surfaces do not prove textures are missing; unsupported shaders or unmapped material properties can produce the same symptom.

## GUI-first workflow

Do not assume a stable headless CLI unless the installed version proves one exists.

1. Open AssetRipper locally.
2. File > Open Folder, select the Schedule One game folder.
3. Wait for files to load.
4. Export to a local ignored Unity project.
5. Open with the matching Unity editor version when needed.
6. Inspect only the relevant prefab/scene/resource/material.

If unavailable, use runtime object inspection, logs, screenshots, or component lists.
