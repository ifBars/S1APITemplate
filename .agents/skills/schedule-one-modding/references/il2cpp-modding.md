# IL2CPP Modding

Use for IL2CPP-only, dual-runtime, or Mono/IL2CPP divergence.

## First checks

- Confirm IL2CPP branch/install (`none` or `beta` in common Steam naming).
- Verify `MelonLoader\Il2CppAssemblies` exists and is fresh.
- Inspect generated wrappers with `ilspycmd`; Mono source is not proof.
- Build/test IL2CPP separately from Mono.

## Boundary strategy

- Keep `#if MONO` / `#if IL2CPP` near imports, aliases, delegates, casts, injected types, and adapters.
- Keep business logic runtime-neutral behind small helpers.

## Common differences

- Namespaces often use `Il2Cpp*` prefixes.
- Delegate and UnityAction conversion may need explicit wrappers.
- Custom runtime components need `[RegisterTypeInIl2Cpp]` plus required constructors.
- Managed-only helper methods on injected types may need `[HideFromIl2Cpp]`.
- Il2Cpp collections are not normal CLR collections; prefer explicit loops.
- Cast with `Cast<T>()`, `TryCast<T>()`, or `Il2CppType.Of<T>()` patterns where needed.

## Harmony rules

- Prefer prefix/postfix patches.
- Do not use IL transpilers for IL2CPP builds.
- Resolve generated RPC/wrapper methods by stable prefix/signature, not suffixes.
- Keep patches thin and guarded with logging.

## Failure checklist

- Wrong backend DLL loaded.
- Wrong target framework or assembly references.
- Missing generated assemblies.
- Missing delegate conversion.
- Missing injected constructors.
- CLR cast or LINQ over Il2Cpp collection.
- Patch target changed after game update.
- Scene/network object accessed too early.
