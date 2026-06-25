# Community Wiki

Use source markdown under `s1modding.github.io/content/docs/`; ignore generated `public/`.

## Useful pages

- `modusers/common_terms/index.md`: branches, logs, saves.
- `modusers/troubleshooting/index.md`: loading failures, backend mismatch, `cpp2il_out` locks.
- `moddevs/environment_setup.md`: frameworks, MelonLoader paths, references, publicizer note.
- `moddevs/reading_game_code.md`: Mono-first decompiler workflow.
- `moddevs/ripping_the_project.md`: AssetRipper GUI project export.
- `moddevs/il2cpp.md`: IL2CPP imports, types, casts, injected classes, coroutines.
- `moddevs/patching.md`: Harmony prefix/postfix and transpiler warning.
- `moddevs/melonloader_utilities.md`: logging, callbacks, prefs, paths, coroutines.
- `moddevs/publishing.md`: Thunderstore/Nexus packaging.

## Branch and log facts

Common branch mapping:

```text
none/beta = IL2CPP
alternate/alternate-beta = Mono
```

Useful logs and paths:

```text
<game>\MelonLoader\Latest.log
C:\Users\<user>\AppData\LocalLow\TVGS\Schedule I\Player.log
C:\Users\<user>\AppData\LocalLow\TVGS\Schedule I\Saves\<steam_id>
```

Ask for narrow logs before guessing. Back up saves before persistence tests.

## Publishing reminders

Thunderstore packages need root-level `manifest.json`, icon, readme, optional changelog/license, and DLL. Uploads are hard to undo; validate metadata first.

For Nexus, make backend clear in filename/description and prefer one DLL per ZIP.
