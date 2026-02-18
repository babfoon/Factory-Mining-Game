using UnityEngine;

/// <summary>
/// Mouse look controller for first-person camera.
/// Handles mouse input to rotate the camera (look up/down) and player body (look left/right).
/// Includes pitch clamping to prevent camera flipping and cursor locking for immersive gameplay.
/// </summary>
/// <remarks>
/// This script should be attached to the CAMERA object (which is a child of the Player).
/// 
/// Why separate camera from player body?
/// 1. Vertical rotation (pitch/looking up-down) affects only the camera
/// 2. Horizontal rotation (yaw/looking left-right) affects the entire player body
/// 3. This prevents gimbal lock and makes movement feel natural
/// 4. The player capsule rotates horizontally so movement direction follows where you look
/// 
/// In the Inspector, you MUST assign the Player's Transform to the "playerBody" field.
/// </remarks>
public class CameraController : MonoBehaviour
{
    #region Variables

    [Header("Mouse Settings")]
    [Tooltip("Mouse sensitivity for looking around. Higher = faster camera movement. 2-3 is comfortable for most players.")]
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Camera Limits")]
    [Tooltip("Minimum pitch angle (looking down). -90 means straight down.")]
    [SerializeField] private float minPitch = -90f;

    [Tooltip("Maximum pitch angle (looking up). 90 means straight up.")]
    [SerializeField] private float maxPitch = 90f;

    [Header("References")]
    [Tooltip("Reference to the player body Transform. This is what rotates left/right. Drag the Player GameObject here.")]
    [SerializeField] private Transform playerBody;

    // Private variables for internal state

    /// <summary>
    /// Current vertical rotation of the camera in degrees.
    /// Positive = looking up, Negative = looking down.
    /// We track this separately because we need to clamp it to prevent over-rotation.
    /// </summary>
    private float currentPitch = 0f;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Called once when the script is first enabled.
    /// Used to lock and hide the cursor for first-person gameplay.
    /// </summary>
    void Start()
    {
        // CURSOR LOCKING EXPLANATION:
        // In a first-person game, we want the cursor:
        // 1. Hidden (invisible) so it doesn't block the view
        // 2. Locked to the center of the screen so mouse movement rotates the camera
        // 3. Not able to leave the game window
        
        // CursorLockMode.Locked:
        // - Hides the cursor
        // - Locks it to the center of the game window
        // - Makes mouse input values continuous (you can keep turning indefinitely)
        Cursor.lockState = CursorLockMode.Locked;
        
        // Make cursor invisible
        // Even when unlocked, we might want it hidden in some situations
        Cursor.visible = false;

        // Defensive programming: Check if playerBody reference is assigned
        if (playerBody == null)
        {
            Debug.LogError("CameraController: Player Body reference not assigned! Drag the Player GameObject to the Player Body field in the Inspector.");
        }
    }

    /// <summary>
    /// Called every frame to handle mouse input and camera rotation.
    /// </summary>
    /// <remarks>
    /// We use Update() instead of FixedUpdate() because:
    /// 1. Mouse input should be checked as frequently as possible for smooth camera movement
    /// 2. Camera rotation doesn't involve physics, so it doesn't need to be in FixedUpdate
    /// 3. Update() runs at the frame rate, giving the most responsive camera control
    /// </remarks>
    void Update()
    {
        // Handle cursor lock/unlock for debugging and UI
        HandleCursorToggle();

        // Only process mouse look if cursor is locked (during gameplay)
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            HandleMouseLook();
        }
    }

    #endregion

    #region Camera Methods

    /// <summary>
    /// Handles mouse input to rotate the camera vertically and player body horizontally.
    /// Implements pitch clamping to prevent the camera from flipping over.
    /// </summary>
    private void HandleMouseLook()
    {
        // MOUSE INPUT EXPLANATION:
        // Input.GetAxis("Mouse X") returns the amount the mouse moved horizontally since last frame
        // Input.GetAxis("Mouse Y") returns the amount the mouse moved vertically since last frame
        // These are NOT absolute positions, but delta (change) values
        // 
        // Example:
        // - Move mouse right quickly: Mouse X might be 5.0
        // - Move mouse right slowly: Mouse X might be 0.5
        // - Don't move mouse: Mouse X = 0
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // HORIZONTAL ROTATION (Looking left/right):
        // We rotate the PLAYER BODY, not the camera
        // This is because:
        // 1. When you look left/right, your whole body should turn
        // 2. Movement direction (WASD) is relative to player body rotation
        // 3. If only the camera rotated, pressing W would move you in the wrong direction
        
        // Transform.Rotate() rotates the transform by the specified angles
        // Vector3.up means rotate around the Y axis (vertical axis pointing up)
        // Space.Self means rotate relative to the object's own rotation (local space)
        playerBody.Rotate(Vector3.up * mouseX);

        // VERTICAL ROTATION (Looking up/down):
        // We rotate only the CAMERA, not the player body
        // Why?
        // 1. Looking up/down shouldn't change which direction you walk
        // 2. Your character's body doesn't tilt when you look up in real life
        // 3. Prevents weird collision issues with tilted collision capsule

        // Subtract mouseY because:
        // - Mouse moving up (positive Y) should make camera look up (negative pitch)
        // - This matches standard FPS controls
        // - Different games have different conventions, but this is most common
        currentPitch -= mouseY;

        // PITCH CLAMPING:
        // Mathf.Clamp restricts a value between a min and max
        // Without clamping, the camera could flip upside down (gimbal lock)
        // 
        // Example:
        // - currentPitch = -100 → Clamp to -90 (can't look further down)
        // - currentPitch = 45 → Stays 45 (within limits)
        // - currentPitch = 100 → Clamp to 90 (can't look further up)
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        // EULER ANGLES EXPLANATION:
        // Euler angles represent rotation as three angles (X, Y, Z) in degrees
        // localEulerAngles is rotation relative to parent (the player body)
        // 
        // For a first-person camera:
        // - X axis (pitch): Looking up/down (what we're changing here)
        // - Y axis (yaw): Looking left/right (handled by player body rotation)
        // - Z axis (roll): Tilting head (we don't use this for FPS)
        
        // We only modify X (pitch), keeping Y and Z at 0
        // This means the camera looks straight ahead (relative to player) when pitch = 0
        transform.localEulerAngles = new Vector3(currentPitch, 0f, 0f);

        // Note: We use localEulerAngles instead of eulerAngles because:
        // - local = relative to parent (player body)
        // - world = absolute world rotation
        // Since the camera is a child of player, local rotation is what we want
    }

    /// <summary>
    /// Handles ESC key to unlock/lock the cursor for debugging and UI interaction.
    /// Useful during development to access the Unity Editor without clicking out of the game view.
    /// </summary>
    private void HandleCursorToggle()
    {
        // Check if Escape key was pressed this frame
        // GetKeyDown returns true only on the frame the key is first pressed (not held)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle cursor lock state
            // If locked, unlock it. If unlocked, lock it.
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // Unlock cursor for UI interaction or debugging
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Lock cursor back for gameplay
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Public method to set mouse sensitivity at runtime.
    /// Useful for implementing a settings menu where players can adjust sensitivity.
    /// </summary>
    /// <param name="sensitivity">New sensitivity value. Typically between 1 and 5.</param>
    public void SetMouseSensitivity(float sensitivity)
    {
        // Clamp to reasonable values to prevent extreme sensitivities
        mouseSensitivity = Mathf.Clamp(sensitivity, 0.1f, 10f);
    }

    /// <summary>
    /// Public method to reset camera rotation to looking straight ahead.
    /// Useful when respawning player or transitioning between game states.
    /// </summary>
    public void ResetCameraRotation()
    {
        currentPitch = 0f;
        transform.localEulerAngles = Vector3.zero;
    }

    #endregion

    // POTENTIAL EXPANSIONS for learning:
    //
    // 1. SMOOTHED CAMERA MOVEMENT:
    //    - Add Mathf.Lerp() or Mathf.SmoothDamp() to smooth camera rotation
    //    - Makes camera movement less "snappy" and more cinematic
    //    - Useful for reducing motion sickness in some players
    //
    // 2. FIELD OF VIEW (FOV) ZOOM:
    //    - Add a zoom key (right mouse button)
    //    - Use Camera.fieldOfView to zoom in/out
    //    - Interpolate FOV changes smoothly with Lerp
    //    - Reduce mouse sensitivity while zoomed for precision
    //
    // 3. HEAD BOB:
    //    - Add slight up/down motion while walking
    //    - Use Mathf.Sin(Time.time * bobSpeed) for smooth oscillation
    //    - Scale bob amount based on movement speed
    //    - Disable while in air to prevent disorientation
    //
    // 4. CAMERA SHAKE:
    //    - Add shake effect for impacts, explosions, or landing
    //    - Use Random.insideUnitSphere for random offset
    //    - Decrease shake intensity over time
    //    - Apply to localPosition, not rotation
    //
    // 5. LOOK AT TARGET:
    //    - Add optional target Transform to look at
    //    - Use Transform.LookAt() for automatic targeting
    //    - Useful for cutscenes or interacting with objects
    //
    // 6. INVERTED Y-AXIS OPTION:
    //    - Add boolean invertYAxis toggle
    //    - If true, don't negate mouseY when calculating pitch
    //    - Some players prefer inverted controls (flight sim style)
    //
    // 7. GAMEPAD SUPPORT:
    //    - Add Input.GetAxis("Right Stick X") and "Right Stick Y"
    //    - Apply different sensitivity for gamepad vs mouse
    //    - Add dead zone handling for stick drift
    //
    // 8. CAMERA COLLISION:
    //    - Raycast backward from player to camera position
    //    - If hits something, move camera forward to prevent clipping
    //    - Useful for third-person camera or tight spaces
}
