# Project Structure Documentation

This document explains the architectural decisions, folder organization, and best practices for the Factory Mining Game project.

## Table of Contents
- [Philosophy](#philosophy)
- [Folder Structure](#folder-structure)
- [Naming Conventions](#naming-conventions)
- [ScriptableObject Pattern](#scriptableobject-pattern)
- [Adding New Features](#adding-new-features)
- [Best Practices](#best-practices)

## Philosophy

### Why This Structure?

The project is organized with **infinite expandability** in mind. As the game grows from a simple first-person controller to a complex factory automation system, the structure should:

1. **Scale effortlessly** - Adding new systems shouldn't require restructuring
2. **Stay organized** - Related code lives together, reducing search time
3. **Prevent conflicts** - Clear boundaries between systems minimize bugs
4. **Welcome beginners** - Intuitive organization helps new contributors
5. **Support collaboration** - Multiple developers can work on different systems

### Core Principles

- **Separation of Concerns**: Each folder has a single, clear purpose
- **Modularity**: Systems are independent and communicate through clean interfaces
- **Data-Driven Design**: Game data (items, recipes) lives in ScriptableObjects, not code
- **Educational First**: Structure teaches good practices by example

## Folder Structure

### Overview

```
Factory-Mining-Game/
├── Assets/                      # All Unity assets
│   ├── Scripts/                 # All C# code
│   │   ├── Player/              # Player-related systems
│   │   ├── World/               # World generation and voxels
│   │   ├── Inventory/           # Item management
│   │   ├── Factory/             # Automation systems
│   │   └── Core/                # Shared utilities
│   ├── Prefabs/                 # Reusable game objects
│   ├── Materials/               # Visual materials
│   ├── Resources/               # Runtime-loaded assets
│   ├── ScriptableObjects/       # Data files
│   └── Scenes/                  # Game scenes (you create this)
├── README.md                    # Project overview
├── PROJECT_STRUCTURE.md         # This file
└── SETUP_GUIDE.md               # Beginner setup instructions
```

### Detailed Breakdown

#### `Assets/Scripts/Player/`
**Purpose**: Everything related to player control and interaction

**Contains:**
- `FirstPersonController.cs` - WASD movement, jumping, running
- `CameraController.cs` - Mouse look and camera control
- Future: `PlayerInteraction.cs`, `PlayerInventoryUI.cs`, `PlayerStats.cs`

**Why separate from other scripts?**
- Player code changes frequently during development
- Isolated from world/factory logic for easier testing
- Clear ownership: "player does X" lives here

#### `Assets/Scripts/World/`
**Purpose**: Terrain generation, voxel systems, and chunk management

**Will contain:**
- `ChunkGenerator.cs` - Creates terrain chunks
- `VoxelData.cs` - Defines voxel types and properties
- `WorldManager.cs` - Coordinates chunk loading/unloading
- `TerrainNoise.cs` - Procedural generation algorithms

**Design notes:**
- World is independent of player (could have multiplayer)
- Chunk-based design allows infinite worlds
- Voxel data separated from rendering for performance

#### `Assets/Scripts/Inventory/`
**Purpose**: Item storage, management, and UI

**Will contain:**
- `InventorySystem.cs` - Core inventory logic
- `ItemStack.cs` - Represents items with quantity
- `StorageContainer.cs` - Chests and storage boxes
- `InventoryUI.cs` - Visual inventory display

**Design notes:**
- Inventory is separate from player (machines also have inventories)
- Uses ScriptableObjects for item definitions
- UI separated from logic for flexibility

#### `Assets/Scripts/Factory/`
**Purpose**: Machines, conveyors, automation, and production

**Will contain:**
- `Machine.cs` - Base class for all machines
- `ConveyorBelt.cs` - Item transportation
- `Assembler.cs`, `Smelter.cs` - Specific machines
- `PowerGenerator.cs` - Energy systems
- `RecipeProcessor.cs` - Handles crafting recipes

**Design notes:**
- Largest system in the game eventually
- Uses inheritance (Machine base class)
- ScriptableObjects for recipes and machine configs
- Independent of world generation (can work on flat terrain)

#### `Assets/Scripts/Core/`
**Purpose**: Shared utilities, managers, and helper classes

**Will contain:**
- `GameManager.cs` - Overall game state management
- `SaveLoadSystem.cs` - Persistence
- `ObjectPooling.cs` - Performance optimization
- `Extensions.cs` - C# extension methods
- `Constants.cs` - Game-wide constant values

**Design notes:**
- No Core script should depend on Player/World/Factory
- Should be reusable across different projects
- Keep minimal - only truly shared code

#### `Assets/Prefabs/`
**Purpose**: Reusable game objects saved as prefabs

**Will contain:**
- `Player.prefab` - Complete player setup
- `ConveyorBelt.prefab` - Placeable conveyor
- `Machine_Smelter.prefab` - Factory machines
- `VoxelBlock.prefab` - Block templates

**Organization:**
```
Prefabs/
├── Player/
├── World/
├── Factory/
│   ├── Machines/
│   └── Conveyors/
└── UI/
```

#### `Assets/Materials/`
**Purpose**: Visual materials, shaders, and textures

**Will contain:**
- `M_Stone.mat` - Voxel materials
- `M_Conveyor.mat` - Animated conveyor material
- `SH_Voxel.shader` - Custom voxel shader

**Naming convention:**
- Materials: `M_Name`
- Shaders: `SH_Name`
- Textures: `T_Name`

#### `Assets/Resources/`
**Purpose**: Assets loaded at runtime with `Resources.Load()`

**Will contain:**
- UI sprites
- Audio clips
- Localization files
- Procedurally generated textures

**Warning**: Resources folder makes build sizes larger. Use sparingly. Prefer AssetBundles or Addressables for large projects.

#### `Assets/ScriptableObjects/`
**Purpose**: Data-driven design files (see [ScriptableObject Pattern](#scriptableobject-pattern))

**Will contain:**
- `Items/` - Item definitions
- `Recipes/` - Crafting recipes
- `Machines/` - Machine configurations
- `World/` - Biome definitions

**Example structure:**
```
ScriptableObjects/
├── Items/
│   ├── Item_IronOre.asset
│   ├── Item_IronPlate.asset
│   └── Item_IronRod.asset
├── Recipes/
│   └── Recipe_IronPlate.asset
└── Machines/
    └── Machine_Smelter.asset
```

## Naming Conventions

Following C# and Unity standards:

### C# Classes and Files
- **PascalCase** for classes, methods, properties
  ```csharp
  public class FirstPersonController : MonoBehaviour
  public void MovePlayer()
  public float MoveSpeed { get; set; }
  ```

- **camelCase** for private fields, local variables, parameters
  ```csharp
  private float currentSpeed;
  private void CalculateVelocity(float deltaTime)
  {
      float acceleration = 10f;
  }
  ```

- **Prefix with underscore** (optional style for private fields)
  ```csharp
  private float _moveSpeed;  // Some teams prefer this
  ```

- **UPPER_SNAKE_CASE** for constants
  ```csharp
  public const float MAX_JUMP_HEIGHT = 5f;
  private const int CHUNK_SIZE = 16;
  ```

### Unity-Specific Conventions

- **Prefixes for assets:**
  - Scripts: No prefix (e.g., `FirstPersonController.cs`)
  - Prefabs: No prefix or `PF_` (e.g., `Player.prefab` or `PF_Player.prefab`)
  - Materials: `M_` (e.g., `M_Stone.mat`)
  - Textures: `T_` (e.g., `T_Stone_Diffuse.png`)
  - Scenes: No prefix (e.g., `MainScene.unity`)

- **SerializeField** for private fields in Inspector:
  ```csharp
  [SerializeField] private Transform playerBody;
  [SerializeField] private float jumpHeight = 2f;
  ```

- **Header** for organization in Inspector:
  ```csharp
  [Header("Movement Settings")]
  [SerializeField] private float moveSpeed = 5f;
  
  [Header("Jump Settings")]
  [SerializeField] private float jumpHeight = 2f;
  ```

## ScriptableObject Pattern

### What is a ScriptableObject?

A ScriptableObject is Unity's way of creating **data containers** that exist outside of scenes and GameObjects. They're perfect for:
- Item definitions
- Character stats
- Crafting recipes
- Machine configurations
- Game balance values

### Why Use ScriptableObjects?

#### 1. Data-Driven Design
```csharp
// ❌ Bad: Hardcoded in script
if (itemName == "Iron Ore") {
    weight = 10;
    value = 5;
}

// ✅ Good: Data in ScriptableObject
public ItemDefinition itemData; // Drag & drop in Inspector
float weight = itemData.weight;
```

#### 2. Easy Balancing
- Change values in the Inspector without recompiling code
- Test different configurations quickly
- Designers can balance the game without touching code

#### 3. Memory Efficiency
- One ScriptableObject instance shared by all references
- No duplicate data in memory

#### 4. Version Control Friendly
- Data files are separate from code
- Designers and programmers work on different files

### Example: Item System

**Step 1: Create the ScriptableObject class**
```csharp
// Assets/Scripts/Core/ItemDefinition.cs
using UnityEngine;

[CreateAssetMenu(fileName = "Item_New", menuName = "Factory Game/Item")]
public class ItemDefinition : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;
    
    [Header("Properties")]
    public int maxStackSize = 100;
    public float weight = 1f;
    
    [Header("Gameplay")]
    public bool isPlaceable;
    public GameObject prefab;
}
```

**Step 2: Create instances**
- Right-click in `Assets/ScriptableObjects/Items/`
- Choose `Create → Factory Game → Item`
- Name it `Item_IronOre`
- Fill in the values in Inspector

**Step 3: Use in code**
```csharp
public class InventorySlot : MonoBehaviour
{
    public ItemDefinition item;
    public int quantity;
    
    public void DisplayItem()
    {
        // Access ScriptableObject data
        nameText.text = item.itemName;
        iconImage.sprite = item.icon;
        quantityText.text = $"{quantity}/{item.maxStackSize}";
    }
}
```

### Best Practices for ScriptableObjects

1. **Use them for static data** (items, recipes, configs)
2. **Don't store runtime state** (use regular classes for that)
3. **Organize in folders** (`Items/`, `Recipes/`, etc.)
4. **Use clear naming** (`Item_IronOre`, `Recipe_IronPlate`)
5. **Add validation** (OnValidate method to check values)

## Adding New Features

### Step-by-Step Guide

#### 1. Plan Your Feature
- Which system does it belong to? (Player, World, Factory, etc.)
- What data does it need? (Consider ScriptableObjects)
- How does it interact with existing systems?

#### 2. Create Necessary Scripts
```bash
# Example: Adding a stamina system to player
Assets/Scripts/Player/StaminaSystem.cs
Assets/Scripts/Player/StaminaUI.cs
```

#### 3. Follow Existing Patterns
- Look at `FirstPersonController.cs` for structure examples
- Use `#region` blocks to organize
- Add XML documentation comments
- Write educational inline comments

#### 4. Create Required Assets
```bash
# If you need UI
Assets/Prefabs/UI/StaminaBar.prefab

# If you need data
Assets/ScriptableObjects/Player/DefaultStamina.asset
```

#### 5. Test Integration
- Does it work with existing systems?
- Does it break anything?
- Is the code clear and educational?

### Example: Adding a New Machine Type

**Step 1: Create ScriptableObject for machine config**
```csharp
// Assets/Scripts/Factory/MachineDefinition.cs
[CreateAssetMenu(fileName = "Machine_New", menuName = "Factory Game/Machine")]
public class MachineDefinition : ScriptableObject
{
    public string machineName;
    public float powerConsumption;
    public float processSpeed;
    // ... more config
}
```

**Step 2: Create the machine script**
```csharp
// Assets/Scripts/Factory/Smelter.cs
public class Smelter : Machine  // Inherit from base Machine class
{
    public MachineDefinition config;
    
    protected override void ProcessItems()
    {
        // Smelter-specific logic
    }
}
```

**Step 3: Create the prefab**
- Create GameObject in scene
- Add Smelter script
- Add necessary visuals
- Save as `Assets/Prefabs/Factory/Machines/Smelter.prefab`

**Step 4: Create configuration asset**
- Right-click in `Assets/ScriptableObjects/Machines/`
- Create → Factory Game → Machine
- Name it `Machine_Smelter.asset`
- Assign to prefab's Smelter component

## Best Practices

### MonoBehaviour Usage

#### When to use MonoBehaviour
- Needs to be attached to a GameObject
- Uses Unity lifecycle methods (Update, Start, etc.)
- Needs Transform, Renderer, or other component access
- Requires Coroutines

```csharp
// ✅ Good: Needs to be in the scene
public class FirstPersonController : MonoBehaviour
{
    void Update() { /* Called every frame */ }
}
```

#### When NOT to use MonoBehaviour
- Pure data classes
- Utility/helper functions
- Math or calculation classes

```csharp
// ✅ Good: Just data, no need for MonoBehaviour
public class ItemStack
{
    public ItemDefinition item;
    public int quantity;
}

// ✅ Good: Static utilities
public static class MathHelpers
{
    public static float Remap(float value, float from1, float from2, float to1, float to2)
    {
        return (value - from1) / (from2 - from1) * (to2 - to1) + to1;
    }
}
```

### Serialization Best Practices

#### What Gets Saved
- Public fields (automatically shown in Inspector)
- `[SerializeField]` private fields

```csharp
public float moveSpeed = 5f;              // ✅ Serialized, public
[SerializeField] private float jumpHeight; // ✅ Serialized, private
private float currentVelocity;             // ❌ Not serialized
```

#### Keep Serialized Data Simple
```csharp
// ✅ Good: Simple types serialize well
[SerializeField] private float speed;
[SerializeField] private List<string> tags;
[SerializeField] private ItemDefinition[] items;

// ❌ Avoid: Complex types may not serialize
private Dictionary<string, Action> callbacks; // Dictionaries don't serialize
private Action onComplete;                    // Delegates don't serialize
```

#### Use NonSerialized When Needed
```csharp
[System.NonSerialized] public float runtimeCalculation;
// Won't be saved, won't show in Inspector
```

### Performance Tips

#### 1. Cache Component References
```csharp
// ❌ Bad: Gets component every frame
void Update()
{
    GetComponent<Rigidbody>().AddForce(Vector3.up);
}

// ✅ Good: Cache in Start
private Rigidbody rb;

void Start()
{
    rb = GetComponent<Rigidbody>();
}

void Update()
{
    rb.AddForce(Vector3.up);
}
```

#### 2. Avoid Empty Update Methods
```csharp
// ❌ Bad: Empty Update is called every frame
void Update()
{
    // Nothing here
}

// ✅ Good: Remove if not needed
// (Just delete it)
```

#### 3. Use Object Pooling for Frequent Instantiation
```csharp
// For items on conveyors, bullets, particles, etc.
// Create once, reuse many times
```

#### 4. Keep Update Light
```csharp
// ❌ Bad: Heavy calculations every frame
void Update()
{
    foreach (Enemy enemy in FindObjectsOfType<Enemy>())
    {
        CalculatePath(enemy);
    }
}

// ✅ Good: Use InvokeRepeating or Coroutines
void Start()
{
    InvokeRepeating(nameof(UpdatePaths), 0f, 0.5f); // Every 0.5 seconds
}
```

### Code Organization

#### Use Regions
```csharp
public class FirstPersonController : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 velocity;
    
    #endregion
    
    #region Unity Methods
    
    void Start() { }
    void Update() { }
    
    #endregion
    
    #region Movement Methods
    
    void HandleMovement() { }
    void HandleJump() { }
    
    #endregion
}
```

#### Keep Methods Small
```csharp
// ❌ Bad: 100+ line method
void Update()
{
    // Movement code...
    // Jump code...
    // Interact code...
    // UI code...
}

// ✅ Good: Broken into clear methods
void Update()
{
    HandleMovement();
    HandleJump();
    HandleInteraction();
    UpdateUI();
}
```

### Educational Comments

Write comments that teach **WHY**, not just what:

```csharp
// ❌ Bad: States the obvious
moveSpeed = 5f; // Set move speed to 5

// ✅ Good: Explains the purpose
// Default walking speed. Players feel sluggish below 3, too fast above 7.
// This value is tuned for our capsule collider size (radius 0.5, height 2).
moveSpeed = 5f;

// ✅ Good: Explains Unity-specific concepts
// Time.deltaTime makes movement frame-rate independent.
// On a 60 FPS system, deltaTime ≈ 0.016 seconds.
// On a 30 FPS system, deltaTime ≈ 0.033 seconds.
// Multiplying by deltaTime ensures we move the same distance per second regardless of FPS.
transform.position += movement * Time.deltaTime;
```

### Error Handling

```csharp
void Start()
{
    // ✅ Good: Check for required components
    characterController = GetComponent<CharacterController>();
    if (characterController == null)
    {
        Debug.LogError("FirstPersonController requires a CharacterController component!");
    }
}
```

## Summary

This structure is designed to:
- **Scale** as the project grows
- **Teach** good Unity practices
- **Organize** code logically
- **Prevent** common mistakes
- **Welcome** new contributors

Follow these guidelines, and the project will remain clean and manageable even as it grows to thousands of lines of code!

---

**Questions?** Open an issue or check the [SETUP_GUIDE.md](SETUP_GUIDE.md) for more help.
