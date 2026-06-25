# UI Patterns

Use for phone apps, HUD changes, menus, canvases, buttons, icons, and native screen adaptation.

## Prefer native flow

Prefer existing game screens, state machines, or canvases. Avoid parallel UI when the base game has a matching model.

## Timing

- UI objects often do not exist during `OnInitializeMelon()`.
- Use scene-load hooks and `MelonCoroutines` to wait for the owning canvas/screen.
- Retry over frames with clear timeout/log.
- Reset injected flags on menu return if the scene can reload.

## Structure

```text
UI/
  UiBootstrap.cs
  ScreenAdapter.cs
  ViewModel.cs
  Assets.cs
```

Keep UI wiring separate from gameplay state. Persist/sync through services, not views.

## Asset rules

- Load sprites/textures from embedded resources or local mod asset bundles.
- Cache loaded assets.
- Do not mutate shared materials unless that is intended.
- Clear texture-driven material props before color-only changes.

## IL2CPP caveats

- Convert UnityActions/callbacks explicitly.
- Cache wrapped delegates if listeners must be removed.
- Use IL2CPP-safe casts for game UI components.
- Avoid LINQ-heavy code over Il2Cpp collections.

## Validation

- Open target scene; verify the UI appears once.
- Reload to menu/back; verify no duplicate injection.
- Check logs for missing canvas/screen names.
- Test both mouse/controller paths when the UI is navigable.
