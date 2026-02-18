# Setup Guide - Your First Unity Scene

This guide will walk you through setting up the Factory Mining Game project from scratch and creating your first playable scene with a working first-person controller.

## Table of Contents
- [Opening the Project in Unity](#opening-the-project-in-unity)
- [Creating Your First Scene](#creating-your-first-scene)
- [Testing Movement](#testing-movement)
- [Troubleshooting](#troubleshooting)
- [Next Steps](#next-steps)

## Opening the Project in Unity

### Step 1: Install Unity Hub and Unity 6.3 LTS

1. **Download Unity Hub**
   - Go to [unity.com/download](https://unity.com/download)
   - Download and install Unity Hub for your operating system

2. **Install Unity 6.3 LTS**
   - Open Unity Hub
   - Click **"Installs"** in the left sidebar
   - Click **"Install Editor"** button
   - Select **"Unity 6.3 LTS"** from the list
   - Choose modules (optional but recommended):
     - ✅ Visual Studio (if you don't have a code editor)
     - ✅ Documentation
     - Platform-specific build support (only if you plan to build for that platform)
   - Click **"Install"**
   - Wait for installation to complete (can take 10-30 minutes)

### Step 2: Clone the Repository

If you haven't already cloned the project:

```bash
# Open terminal/command prompt
git clone https://github.com/babfoon/Factory-Mining-Game.git
cd Factory-Mining-Game
```

### Step 3: Add Project to Unity Hub

1. **Open Unity Hub**
2. Click **"Projects"** in the left sidebar
3. Click **"Add"** button (or **"Open"** dropdown → **"Add project from disk"**)
4. Navigate to where you cloned the repository
5. Select the **Factory-Mining-Game** folder (the one containing the `Assets` folder)
6. Click **"Add Project"** or **"Select Folder"**

The project should now appear in your Unity Hub projects list.

### Step 4: Open the Project

1. Click on **"Factory Mining Game"** in your projects list
2. Unity Editor will launch and begin importing assets

**First-time opening notes:**
- Unity will create a `Library` folder (this is normal, it stores cached data)
- Scripts will be compiled for the first time
- This process can take 1-5 minutes
- You may see "Importing Assets" at the bottom of the screen

**If you see warnings or errors:**
- Ignore warnings about "Scripts have compiler errors" - this is normal during initial import
- If scripts still have errors after import completes, see [Troubleshooting](#troubleshooting)

## Creating Your First Scene

Now let's build a simple test scene where you can walk around!

### Step 1: Create a New Scene

1. In Unity Editor, go to **File → New Scene**
2. Select **"3D (Built-In Render Pipeline)"**
3. Click **"Create"**

You now have a new scene with a main camera and directional light.

### Step 2: Save the Scene

1. Go to **File → Save As...**
2. Create a new folder: Click **"New Folder"**, name it **"Scenes"**
3. Open the Scenes folder
4. Name your scene **"MainScene"**
5. Click **"Save"**

Your scene is now saved at `Assets/Scenes/MainScene.unity`.

### Step 3: Create the Ground

We need something to walk on!

1. In the **Hierarchy** panel (left side), right-click
2. Select **3D Object → Plane**
3. This creates a flat ground plane

**Make the ground bigger:**
1. Select the **Plane** in the Hierarchy
2. In the **Inspector** panel (right side), find the **Transform** component
3. Change **Scale** to:
   - X: **10**
   - Y: **1**
   - Z: **10**

Your ground is now 10x bigger!

### Step 4: Create the Player

Now let's create the player character.

1. In the Hierarchy, right-click
2. Select **3D Object → Capsule**
3. Rename it to **"Player"** (right-click → Rename, or press F2)

**Position the player above the ground:**
1. Select **Player** in the Hierarchy
2. In the Inspector, set **Transform → Position**:
   - X: **0**
   - Y: **1** (so the player doesn't start inside the ground)
   - Z: **0**

### Step 5: Add CharacterController Component

The CharacterController is Unity's built-in component for character movement.

1. Select **Player** in the Hierarchy
2. In the Inspector, click **"Add Component"** button (at the bottom)
3. Type **"Character Controller"**
4. Click **"Character Controller"** to add it

**Configure the CharacterController:**
- **Center**: Leave as (0, 0, 0)
- **Radius**: Leave as 0.5
- **Height**: Leave as 2 (matches the capsule)

### Step 6: Add FirstPersonController Script

Now we'll add our custom movement script!

1. With **Player** still selected
2. Click **"Add Component"** again
3. Type **"First Person Controller"**
4. Click **"FirstPersonController"** from the list

You should now see the script component in the Inspector with settings like:
- Move Speed: 5
- Run Speed Multiplier: 2
- Jump Height: 2
- Gravity: -9.81

These are all configurable! Feel free to adjust them later.

### Step 7: Create the Camera

We need a camera to see through the player's eyes.

1. Delete the default **Main Camera** (select it and press Delete)
2. Right-click on **Player** in the Hierarchy
3. Select **Camera** from the list (or 3D Object → Camera)
4. This creates a Camera as a **child** of the Player

**Position the camera at eye level:**
1. Select **Camera** (under Player) in the Hierarchy
2. In the Inspector, set **Transform → Position**:
   - X: **0**
   - Y: **0.6** (eye level for our capsule)
   - Z: **0**
3. Set **Transform → Rotation**:
   - X: **0**
   - Y: **0**
   - Z: **0**

**Why is the camera a child of Player?**
- When the player moves, the camera moves with it
- This creates the first-person view
- The camera can still rotate independently for looking around

### Step 8: Add CameraController Script

Now let's add mouse look!

1. Select **Camera** (under Player) in the Hierarchy
2. Click **"Add Component"**
3. Type **"Camera Controller"**
4. Click **"CameraController"** from the list

You'll see it has settings for:
- Mouse Sensitivity: 2
- Min Pitch: -90
- Max Pitch: 90

### Step 9: Assign Player Body Reference

The camera needs to know which object to rotate (the player body).

1. With **Camera** selected, look at the **CameraController** component
2. Find the **"Player Body"** field (it's empty and needs a reference)
3. **Drag the Player object** from the Hierarchy into this field
   - OR click the circle icon next to the field and select Player from the list

The Player Body field should now show **"Player (Transform)"**.

### Step 10: Save Everything!

1. Go to **File → Save** (or press Ctrl+S / Cmd+S)
2. Go to **File → Save Project**

Your scene is complete!

## Testing Movement

Time to play!

### Step 1: Enter Play Mode

1. Click the **Play** button at the top of the Unity Editor (▶️ icon)
2. The Game view will activate
3. Your cursor should disappear (it's locked to the window)

### Step 2: Test Controls

Try all the movement controls:

| Input | Action |
|-------|--------|
| **W** | Move forward |
| **A** | Move left |
| **S** | Move backward |
| **D** | Move right |
| **Mouse** | Look around |
| **Space** | Jump |
| **Left Shift** | Run (hold while moving) |
| **ESC** | Unlock cursor (for debugging) |

**What to expect:**
- Smooth movement in all directions
- Camera rotation with mouse
- Jump should work when on the ground
- Running should be noticeably faster
- You can't walk through the ground

### Step 3: Exit Play Mode

1. Click the **Play** button again to stop
2. **IMPORTANT**: Any changes made during Play Mode are lost!
3. Always exit Play Mode before making changes

## Troubleshooting

### Problem: Scripts have compiler errors

**Solution:**
1. Open the **Console** window (Window → General → Console)
2. Look at the error messages
3. Most common issues:
   - **Missing scripts**: Make sure files are in correct folders
   - **Namespace issues**: Scripts should not have namespace declarations
   - **Unity version**: Make sure you're using Unity 6.3 LTS

### Problem: Player falls through the ground

**Solutions:**
- Make sure the Player has a **CharacterController** component
- Check that the Player's Y position is **above** 0 (try Y = 1 or Y = 2)
- Verify the Plane has a **Mesh Collider** (it should by default)

### Problem: Camera doesn't move when player moves

**Solutions:**
- Make sure the **Camera is a child of Player** (indented under Player in Hierarchy)
- Check that Camera's position is set to (0, 0.6, 0) or similar

### Problem: Mouse look doesn't work

**Solutions:**
- Make sure **CameraController** is attached to the **Camera**, not the Player
- Check that **Player Body** field is assigned in CameraController
- Verify you clicked in the Game view (cursor should be locked)
- Try pressing ESC to unlock cursor, then click Game view again

### Problem: Can't jump

**Solutions:**
- Make sure you're on the ground (not already falling)
- Check that Jump Height is greater than 0 in FirstPersonController
- Verify CharacterController's **Step Offset** isn't too high (should be ~0.3)

### Problem: Movement is too slow/fast

**Solution:**
- Select the Player in Hierarchy
- Adjust **Move Speed** in the FirstPersonController component
- Try values between 3 (slow) and 8 (fast)
- Adjust **Run Speed Multiplier** for sprint speed

### Problem: Movement feels floaty or gravity is wrong

**Solution:**
- Select the Player
- Adjust **Gravity** in FirstPersonController
- Default is -9.81 (realistic Earth gravity)
- Try -15 or -20 for snappier, more game-like feel

### Problem: Can't see anything in Game view

**Solutions:**
- Make sure the Camera has the **"MainCamera"** tag (in Inspector)
- Check Camera position is at (0, 0.6, 0) relative to Player
- Verify Player isn't inside the ground
- Try selecting Camera and clicking GameObject → Align View to Selected

## Next Steps

Congratulations! You now have a working first-person controller.

### Experiment and Learn

Try modifying the scripts to learn:
1. **Change movement speed** - Open `FirstPersonController.cs` and adjust default values
2. **Add sprint stamina** - Add a stamina system that depletes when running
3. **Adjust jump mechanics** - Make it a double jump or variable height jump
4. **Add head bob** - Make the camera move slightly when walking

### Understanding the Code

Open the scripts in your code editor and read through them:
- `Assets/Scripts/Player/FirstPersonController.cs`
- `Assets/Scripts/Player/CameraController.cs`

Every line has educational comments explaining:
- **WHY** the code works this way
- **HOW** Unity systems work
- **WHAT** you could expand on

### Add More Objects to the Scene

Try adding obstacles to test collision:
1. Right-click in Hierarchy → 3D Object → Cube
2. Position cubes around the scene
3. Press Play and try to walk into them
4. The CharacterController should prevent you from walking through them

### Build a Simple Level

Create a more interesting test environment:
- Add multiple Planes at different heights (platforms)
- Add Cubes as walls or obstacles
- Change colors using Materials
- Test jumping between platforms

### Prepare for Phase 2

The next phase will add:
- **Voxel terrain** instead of a simple plane
- **Mining mechanics** to break and place blocks
- **Chunk-based world** for infinite terrain
- **Basic inventory** to hold mined resources

Read [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md) to understand how the project will expand.

## Additional Resources

### Unity Documentation
- [CharacterController Manual](https://docs.unity3d.com/Manual/class-CharacterController.html)
- [Input Manager](https://docs.unity3d.com/Manual/class-InputManager.html)
- [Vector3 API](https://docs.unity3d.com/ScriptReference/Vector3.html)

### Learning Unity
- [Unity Learn](https://learn.unity.com/) - Official tutorials
- [Brackeys YouTube](https://www.youtube.com/user/Brackeys) - Excellent Unity tutorials
- [Unity Manual](https://docs.unity3d.com/Manual/index.html) - Complete documentation

### C# Programming
- [Microsoft C# Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [C# Fundamentals for Unity](https://learn.unity.com/tutorial/c-sharp-fundamentals)

## Getting Help

If you're stuck:
1. **Check the Console** window for error messages
2. **Read the code comments** in the scripts - they're there to teach you!
3. **Read PROJECT_STRUCTURE.md** for architecture explanations
4. **Open an Issue** on GitHub with your problem
5. **Ask in Discussions** tab on GitHub

---

**Happy Game Development!** 🎮

You've just taken your first steps in Unity game development. Keep experimenting, keep learning, and most importantly - have fun!
