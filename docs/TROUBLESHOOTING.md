# Troubleshooting

If you're reading this, something went wrong.
Don't worry though, as this is the most thorough guide to help you!

## `./Impostor.Server: line 1: ELF: not found` (plus other errors)

No idea where you got that system. But we clearly do **NOT** support it.

## `cannot execute binary file: Exec format error`

Please check that you have downloaded the right version of Impostor, as we mantain two CPU architectures (x64 and ARM).  
Unless you are running Impostor on a SBC (Single-Board Computer), like the Raspberry Pi, you most likely want to use the x64 version.

## `./Impostor.Server: Permission denied`

This is an error related to Linux file permissions.
Some files do not hold their executable bit (the permission that allows them to run) during a download.
You can solve this by doing: `chmod +x Impostor.Server`

## `You are using an older version of the game`

You are using a version of Impostor that is not designed for the version of the game you're playing. The game does not really check who is outdated and blames it on the user.

Look at which version of the game you're playing, which you can see in the top left corner of the main menu, then download an Impostor version for that game. Every [release on the release page](https://github.com/Impostor/Impostor/releases) shows which version of the game it is compatible with. If your game version is newer than the latest release, check if the [latest build from AppVeyor works](https://ci.appveyor.com/project/Impostor/Impostor/branch/master).

## `You disconnected from the server. Reliable Packet 1 ...`

Please double-check that you have followed the [Server Configuration](Server-configuration.md) correctly.
**NOTE: Your public ip does not start with `10`, `127` or `192`**
Also check if the port Impostor (ListenPort) is listening on is correctly port-forwarded for UDP (or TCP/UDP).

## `Could not load file or assembly...`

Please check that you only have **working** plugins in the `plugins` folder.
This error can be caused by non-plugin files or plugins that are not working correctly.

## My question is not yet answered and I'm still having problems!

That's unfortunate. Join the [Impostor Discord](https://discord.gg/Mk3w6Tb), ask your question there and we'll try to help you out. Note that we're not always available, so it may take some time to get an answer.
