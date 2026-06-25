# MAPI Reference

Use MAPI for custom buildings, interiors, GLTF/GLB models, placeable structures, and building-like prefab content.

## Workflow

1. Identify the lifecycle owner and placement path.
2. Inspect local prefab/material/resource evidence if vanilla assets matter.
3. Keep model paths, material names, placement IDs, and dimensions in constants.
4. Put builders/placement services outside Harmony patches.
5. Validate asset loading on each shipped runtime.

## Rules

- Prefer MAPI builders for geometry and building definitions.
- Keep raw Unity manipulation at the boundary.
- Clone materials before mutation.
- Retry scene-dependent setup if the scene or registry is not ready.
- Do not infer current prefab hierarchy from old dumps.
- Do not publish exported Unity projects, dumped prefabs, textures, meshes, or resource files.
