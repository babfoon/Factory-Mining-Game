# Factory Mining Game

A 3D first-person mining and factory automation game inspired by Minecraft and Satisfactory, built with Unity 6.

## Description

Factory Mining Game combines the exploration and resource gathering of Minecraft with the factory automation and logistics of Satisfactory. Mine resources from a procedurally generated voxel world, then use them to build automated factories with conveyor belts, machines, and complex production chains.

## Unity Version

**Unity 6.3 LTS** (Long Term Support)

This project is built using Unity's latest LTS release for stability and long-term support.

## Features

### Current Features (Phase 1: Foundation) ✅
- **First-Person Movement System**: Smooth WASD movement with running and jumping
- **Camera Controls**: Mouse look with pitch clamping and cursor management
- **Educational Code**: Heavily commented scripts designed for learning Unity development
- **Organized Project Structure**: Scalable folder organization for future expansion

### Planned Features (Future Phases)
- **Procedural World Generation**: Infinite terrain using chunk-based loading
- **Voxel Mining System**: Dig and place blocks like Minecraft
- **Inventory Management**: Store and organize collected resources
- **Factory Automation**: Build machines that process resources automatically
- **Conveyor Belt System**: Transport items between machines
- **Crafting & Recipes**: Combine resources to create new items
- **Power Generation**: Energy systems to run your factories
- **Advanced Logistics**: Splitters, mergers, and smart routing

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

1. **Unity Hub** - Download from [unity.com](https://unity.com/download)
2. **Unity 6.3 LTS** - Install through Unity Hub
3. **Git** - For version control ([git-scm.com](https://git-scm.com/))
4. **A code editor** (optional but recommended):
   - Visual Studio 2022 (included with Unity)
   - Visual Studio Code with C# extension
   - JetBrains Rider

### Clone the Repository

```bash
git clone https://github.com/babfoon/Factory-Mining-Game.git
cd Factory-Mining-Game
```

### Open in Unity Hub

1. Open **Unity Hub**
2. Click **"Add"** or **"Open"** button
3. Navigate to the cloned repository folder
4. Select the folder and click **"Open"**
5. Unity Hub will recognize it as a Unity project
6. Click on the project to open it in Unity Editor

**Note:** The first time Unity opens the project, it will:
- Generate the `Library` folder (this is normal and can take a few minutes)
- Import all assets
- Compile scripts

### Create Your First Scene

Follow the detailed instructions in [SETUP_GUIDE.md](SETUP_GUIDE.md) to create your first playable scene with the player controller.

**Quick Start:**
1. Create new Scene: `File → New Scene → 3D (Built-In Render Pipeline)`
2. Save as "MainScene" in a new `Assets/Scenes/` folder
3. Add Ground plane and Player capsule
4. Attach `FirstPersonController` and `CameraController` scripts
5. Press Play to test!

## Project Structure

The project follows a modular folder structure designed for scalability:

```
Assets/
├── Scripts/          # All C# code
│   ├── Player/       # First-person controller, input, interaction
│   ├── World/        # Terrain generation, voxels, chunks
│   ├── Inventory/    # Item management, storage UI
│   ├── Factory/      # Machines, conveyors, automation logic
│   └── Core/         # Utilities, managers, shared systems
├── Prefabs/          # Reusable game objects
├── Materials/        # Visual materials and shaders
├── Resources/        # Runtime-loaded assets
└── ScriptableObjects/ # Data-driven design (items, recipes, etc.)
```

For a detailed explanation of the architecture and organization philosophy, see [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md).

## Development Roadmap

### Phase 1: Core Foundation ✅ (Current)
- [x] Project setup and folder structure
- [x] First-person movement controller
- [x] Camera look system
- [x] Educational documentation

### Phase 2: World Generation & Voxels
- [ ] Chunk-based terrain system
- [ ] Procedural terrain generation
- [ ] Voxel block system
- [ ] Mining mechanics (break and place blocks)
- [ ] Simple inventory UI

### Phase 3: Factory Basics
- [ ] Basic machines (ore processor, assembler)
- [ ] Conveyor belt system
- [ ] Item transportation
- [ ] Power generation
- [ ] Machine placement system

### Phase 4: Advanced Systems
- [ ] Recipe system with ScriptableObjects
- [ ] Complex production chains
- [ ] Factory optimization tools
- [ ] Multiplayer support (stretch goal)
- [ ] Save/Load system

## Contributing

This project is designed to be educational and extensible. Contributions are welcome!

### How to Extend the Project

1. **Follow the existing structure**: Place scripts in appropriate folders
2. **Write educational code**: Add comments explaining your logic
3. **Use ScriptableObjects for data**: Keep code and data separate
4. **Test your changes**: Ensure existing systems still work
5. **Document new features**: Update README and relevant docs

### Coding Standards

- Follow C# naming conventions (see [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md))
- Add XML documentation comments to public methods
- Use `#region` blocks to organize code
- Explain complex logic with inline comments
- Keep scripts focused on a single responsibility

## Learning Resources

New to Unity? Check out these resources:

- [Unity Learn](https://learn.unity.com/) - Official tutorials
- [Unity Manual](https://docs.unity3d.com/Manual/index.html) - Documentation
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/) - Code reference
- [Brackeys YouTube](https://www.youtube.com/brackeys) - Unity tutorials

## License

MIT License - See LICENSE file for details.

This project is free to use, modify, and distribute. Attribution is appreciated but not required.

## Acknowledgments

- Inspired by **Minecraft** (Mojang) for voxel mining mechanics
- Inspired by **Satisfactory** (Coffee Stain Studios) for factory automation
- Built with **Unity** game engine

## Support

For questions, issues, or feature requests:
- Open an [Issue](https://github.com/babfoon/Factory-Mining-Game/issues)
- Check the [Discussions](https://github.com/babfoon/Factory-Mining-Game/discussions) tab
- Read the [SETUP_GUIDE.md](SETUP_GUIDE.md) for common problems

---

**Happy Building!** 🏭⛏️
