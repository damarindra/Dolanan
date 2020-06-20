> Dolanan is still far away for Production!

# Dolanan
Is a MonoGame extension contains basic function for creating game, such as sprite, camera, collision etc. Dolanan will focus on
usability and simplicity for creating 2D game.

## How to use it?
Dolanan is just a library project, it is separated from your main project. There is many way to using Dolanan Library, so do whatever you comfortable with it.
This is just an example how to do it, you can follow it if you want.

1. Clone this project.

2. Create new MonoGame project, detailed step by step [read here](https://docs.monogame.net/articles/introduction/create_project.html)

3. Create sln

    - Visual Studio -> open your `MonoGameProject.csproj`, it will automatically create sln
    
    - Rider -> Create a new sln, after sln created -> right click `MyProject` solution -> Add -> Add Existing Project -> `MonoGameProject.csproj`
    
4. Add `Dolanan.csproj` as reference

    - Visual Studio -> right click `MyProject` solution -> Add -> Existing Project -> `Dolanan.csproj`
    
    - Rider -> right click `MyProject` solution -> Add -> Add Existing Project -> `Dolanan.csproj`

5. [Optional] Create a new `SharedProject` for your main Game code. Right click `MonoGameProject.csproj`, Add reference and select the `MainGame.shproj`.

6. Happy coding! Remember, if you don't doing step 5, you can just code inside your `MonoGameProject.csproj`.

### Content.mgcb ?

Dolanan shared project using the `Content.mgcb`, so you need to include the `Content.mgcb` into your `MonoGameProject.csproj`, edit by hand the `MonoGameProject.csproj` and add this line code

```
<Project>
  <!-- Other Line Code -->

  <ItemGroup>
  
    <!-- ADD ALL MGCB HERE -->
    
    <MonoGameContentReference Include="..\Dolanan\Content\Content.mgcb">
      <Link>Content\DolananContent.mgcb</Link>
    </MonoGameContentReference>
    
    <!-- If you used another Shared Project for your Main Game Code -->
    <MonoGameContentReference Include="..\MainGame\Content\Content.mgcb">
      <Link>Content\Content.mgcb</Link>
    </MonoGameContentReference>
    
  </ItemGroup>
</Project>
```

## Project Details

|**Projects**        | Desc                                                                    |
|--------------------|--------------------------------------------------------------------------|
|**Dolanan**         | is the main library shared project                                       |
|**Sample**          | Main Game code                                                           |
|**WinDX**           | Windows DX platform, it uses Sample as the sample of the game            |
|**DesktopGL**       | Open GL platform, it uses Sample as the sample of the game               |
|**PipelineReader**  | Reader for all custom pipeline type, including aseprite                  |
|**AsepritePipeline**| AsepritePipeline, read the json produced by Aseprite                     |

## Documentation


## Dependency

"ImGui.NET" Version="1.75.0"

"Sigil" Version="5.0.0"

## Related tools
- Aseprite (https://www.aseprite.org/)
