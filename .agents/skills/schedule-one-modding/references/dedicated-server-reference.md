# Dedicated Server Reference

Use for dedicated server mods, headless behavior, server/client split, commands, permissions, config, FishNet, custom messaging, or shared APIs.

## Boundaries

- Classify side first: server-only, client-only, shared, or CrossCompat.
- Server owns gameplay state, commands, economy, inventory, saves, and messages.
- Client owns UI, camera, input, visuals, and local-player presentation.
- Keep `#if SERVER`, `#if CLIENT`, `#if MONO`, and `#if IL2CPP` at boundaries.

## Engineering shape

- Keep bootstraps orchestration-only.
- Move behavior into services, adapters, commands, config, or messages.
- Expose public APIs intentionally; keep coordination private/internal.
- Add XML docs for public or protected framework APIs.
- Validate public arguments and log expected failures with stable tags.
- Do not leak raw Unity, FishNet, Steamworks, or Il2Cpp types through supported APIs unless documented.

## Runtime rules

- Never require canvas, camera, input, local-player view, or client scenes on the server.
- Prefer server-originated state and client presentation updates.
- Keep custom messages shape-stable and fail clearly.
- Check command permissions before mutating state.
- Make config defaults server-safe.

## Validation

- Build touched configs: `Mono_Server`, `Mono_Client`, `Il2cpp_Server`, `Il2cpp_Client` when refs exist.
- Test headless startup, commands, permissions, messages, save/load, reconnects, cleanup.
- Report which side and runtime were verified.
