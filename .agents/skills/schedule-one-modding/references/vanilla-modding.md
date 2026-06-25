# Vanilla Modding

Use direct Harmony patches when helper libraries do not expose the needed game seam.

## Workflow

1. Start from the exact symptom, method, class, or log line.
2. Inspect one target type with `ilspycmd`.
3. Patch the owning method with prefix/postfix where possible.
4. Delegate behavior to services.
5. Log failures and use the safest fallback.

## Rules

- Prefer S1API/MAPI/SteamNetworkLib for their domains.
- Patch by `nameof(...)` or explicit signature when stable.
- Avoid IL transpilers for IL2CPP.
- Resolve generated IL2CPP methods by prefix/signature.
- Keep runtime-specific casts, delegates, and collection access in adapters.
- Do not publish decompiled game methods; summarize names, signatures, and behavior.
