# DeadGalaxy

"DeadGalaxy" — small shooter game/engine project based on raylib and .NET 8.

Project still under active development, anything in this repo is subject to change.

## Features

- Deferred rendering pipeline (implementing deferred lighting for point lights)
  - Geometry rendering stage
  - Point lights rendering stage
  - Final rendering stage
  - Debug rendering stage
- Flexible scene loading featuring linked resources and entities loading
- Flexible configuration management with automatic saving changes to file
- In-game debug information overlay
  - Rendering stages preview
  - FPS counter
- In-game debug console interface supporting commands input and logs output
  - Help command
  - Level loading command
  - Configuration value set command
- Logging with automatic saving to file and debug console
- World model based on raylib's "cubicmesh" generation algorithm

## Contribution

Fill free to create issues and proposals.\
I will also be glad any help.

## Open software licenses

This project uses open software licensed under various licenses:
- [raylib](https://github.com/raysan5/raylib)
- [Raylib-cs](https://github.com/ChrisDill/Raylib-cs)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- [Texture packs by Philip Klevestav](https://philipk.net)

Full license texts available in Open Software Licenses file.

## License

Developed by  [Kabanov Kirill (@Kiriller12)](https://github.com/Kiriller12)

Licensed under GNU General Public License v3.0.\
Full license text available in LICENCE file.
