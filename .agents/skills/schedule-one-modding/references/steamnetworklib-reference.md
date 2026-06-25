# SteamNetworkLib Reference

Use SteamNetworkLib for mod-level multiplayer messaging and synchronized state when direct FishNet patches are unnecessary.

## Good fit

- Host-owned shared values.
- Client-owned shared values.
- Custom P2P messages, lobby/member state, version checks.

Use direct networking only when SteamNetworkLib lacks the seam.

Authority split: host/server owns economy, saves, progression, inventory, world mutation; clients own readiness, cosmetics, transient intent; UI/previews/debug visuals stay local.

## Rules

- Initialize after Steamworks is ready.
- Use unique key prefixes per mod.
- HostSyncVar writes must stay host-owned.
- ClientSyncVar writes must stay client-owned.
- Process incoming messages on the required update path.
- Dispose or unsubscribe on lobby leave and mod shutdown.
- Validate custom messages before mutating state.
- Rate-limit high-frequency updates.
- Hide IL2CPP callback/cast/delegate details behind helpers.

## Common failure points

- Steam client open does not prove in-process Steamworks readiness.
- Multiple mods can conflict through shared DLL/version state.
- IL2CPP callback setup can require explicit conversions.
- High-frequency state without rate limits can flood P2P.
- Missing cleanup duplicates handlers after scene/lobby reload.

## Validation

For meaningful changes, test at least:

- host creates lobby
- client joins
- direct send
- broadcast
- sync var convergence
- leave/rejoin cleanup
- version mismatch path
- authority rejection
