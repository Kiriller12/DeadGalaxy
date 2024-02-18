# DeadGalaxy

"DeadGalaxy" — small shooter game/engine project based on raylib and .NET 8.

The project is still under active development, anything in this repo is a subject to change.

## Features

- Deferred rendering pipeline (implementing deferred lighting for point lights)
  - Geometry rendering => Diffuse, Normal, Specular, Depth textures
  - Point lights rendering => Light texture (Diffuse light + Specular light)
  - Final rendering => Final image generation
  - Debug rendering overlay
- Flexible scene loading featuring linked resources and entities loading
- Flexible configuration management with automatic saving to the file
- In-game debug information overlay
  - Rendering stages preview
  - FPS counter
- In-game debug console interface supporting commands input and logs output
  - Help command
  - Level loading command
  - Configuration value set command
- Logging with automatic saving to the file and debug console
- World model based on the raylib's "cubicmesh" generation algorithm

### Controls

W, A, S, D: camera movement\
Q, E: camera rotation\
F1: debug console

### Debug console commands

help - commands help\
load \<scene\> - loads game scene by its name\
set \<setting\> \<value\> - sets configuration setting value

## Video preview

Click on the image to watch video or use the following link:\
https://youtu.be/peX24NMFGS0

[![Watch the video](https://img.youtube.com/vi/peX24NMFGS0/maxresdefault.jpg)](https://youtu.be/peX24NMFGS0)

## Contribution

Fill free to create issues and proposals.\
I will also be glad for any help.

## Open software licenses

This project uses open software licensed under various licenses:
- [raylib](https://github.com/raysan5/raylib)
- [Raylib-cs](https://github.com/ChrisDill/Raylib-cs)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- [Texture packs by Philip Klevestav](https://philipk.net)

Full license texts are available in the Open Software Licenses file.

## License

Developed by [Kabanov Kirill (@Kiriller12)](https://github.com/Kiriller12)

Licensed under GNU General Public License v3.0.\
Full license text is available in the LICENCE file.
