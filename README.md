> Dolanan is still far away for Production!

# Dolanan
Is a MonoGame extension contains basic function for creating game, such as sprite, camera, collision etc. Dolanan will focus on
usability and simplicity for creating 2D game.

## How to use it?
Dolanan is just an library project, it is separated from your main project. So what you need to do is
1. Create a MonoGame project, you can use any target platform, or a library project. Don't know how to do it, [read here](https://docs.monogame.net/articles/introduction/create_project.html)
2. Create a sln

    - Visual Studio, open your MonoGame project, it will automatically create sln
    - Rider, create a new sln, add reference to `MyProject.csproj`
3. Clone this project, what you need is only `Dolanan` project. Other than that, is just example how to use Dolanan and Cross Platform.
4. Add reference to Dolanan, and you can use create a game with Dolanan Library.

## Project Details

**Dolanan**         -> is the main library

**DolananEditor**   -> Editor extension using dear ImGUI.

**DolananSample**   -> example on how to use Dolanan Library, and as a OpenGL platform

**Platform_WinDX**  -> Windows DX platform, it uses DolananSample as the sample of the game

**AsepritePipeline**-> AsepritePipeline, read the json produced by Aseprite

## Documentation


## Dependency
- MonoGame Portable > 3.7.1.189 (https://monogame.net/)
- Sigil 5.0.0 (https://github.com/kevin-montrose/Sigil)

## Related tools
- Aseprite (https://www.aseprite.org/)
