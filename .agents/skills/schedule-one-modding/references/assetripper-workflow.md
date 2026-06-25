# AssetRipper Workflow

Use AssetRipper for local prefab, resource, scene, material, mesh, sprite, and serialized-reference inspection.

## Safety

- Do not commit or publish exports, dumped prefabs, Unity projects, textures, meshes, scenes, shaders, or game resources.
- Keep exports in ignored local folders.
- Treat output as user-local inspection only. Do not package ripped prefabs/resources; recreate behavior with your own assets or load user-owned local resources at runtime.
- Report only narrow findings: names, paths, component lists, screenshots, or short summaries.

## Script/code expectation

AssetRipper is not the code tool. Scripts are assembly-backed; use `ilspycmd` for Mono assemblies and generated IL2CPP wrappers. IL2CPP script associations may need compatible Cpp2IL-style assemblies; Il2CppInterop modding assemblies are not a general substitute.

## GUI-first workflow

Do not assume a stable headless CLI unless the installed version proves one exists.

1. Open AssetRipper locally.
2. File > Open Folder, select the Schedule One game folder.
3. Wait for files to load.
4. Export to a local ignored Unity project.
5. Open with the matching Unity editor version when needed.
6. Inspect only the relevant prefab/scene/resource/material.

If unavailable, use runtime object inspection, logs, screenshots, or component lists.
