using UnityEngine;

/// <summary>
/// First-person character controller for player movement.
/// Handles WASD movement, running, jumping, and gravity using Unity's CharacterController component.
/// This script is designed to be educational for beginners learning Unity and C#.
/// </summary>
/// <remarks>
/// This controller uses CharacterController instead of Rigidbody for several reasons:
/// 1. CharacterController provides built-in collision detection without physics simulation
/// 2. It's more predictable and easier to control than physics-based movement
/// 3. It doesn't interact with Rigidbody physics, preventing unwanted physics interactions
/// 4. It's specifically designed for character movement with features like ground detection
/// 
/// Attach this script to your Player GameObject which must also have a CharacterController component.
/// </remarks>
public class FirstPersonController : MonoBehaviour
{
    #region Variables

    [Header("Movement Settings")]
    [Tooltip("Base movement speed in units per second. 5 is a comfortable walking speed.")]
    [SerializeField] private float moveSpeed = 5f;

    [Tooltip("Multiplier applied to moveSpeed when running. 2x means running is twice as fast.")]
    [SerializeField] private float runSpeedMultiplier = 2f;

    [Header("Jump Settings")]
    [Tooltip("Maximum height the player can jump. Higher values = higher jumps.")]
    [SerializeField] private float jumpHeight = 2f;

    [Tooltip("Gravity acceleration. -9.81 is realistic Earth gravity. More negative = faster falling.")]
    [SerializeField] private float gravity = -9.81f;

    [Header("Ground Detection")]
    [Tooltip("Distance to check for ground below the player. Should be slightly larger than CharacterController's skin width.")]
    [SerializeField] private float groundCheckDistance = 0.2f;

    // Private variables for internal state
    // These are not shown in the Inspector because they change during gameplay

    /// <summary>
    /// Reference to the CharacterController component attached to this GameObject.
    /// Cached in Start() to avoid repeated GetComponent calls which are expensive.
    /// </summary>
    private CharacterController characterController;

    /// <summary>
    /// Current vertical velocity (Y-axis).
    /// Positive = moving up (jumping), Negative = falling due to gravity.
    /// This accumulates over time as gravity is applied each frame.
    /// </summary>
    private Vector3 velocity;

    /// <summary>
    /// Is the player currently touching the ground?
    /// Used to determine if the player can jump and to reset falling velocity.
    /// </summary>
    private bool isGrounded;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Called once when the script is first enabled, before the first Update().
    /// This is where we initialize components and set up references.
    /// </summary>
    void Start()
    {
        // Get the CharacterController component attached to this GameObject
        // We cache this reference because GetComponent is relatively expensive
        // and we'll need to use it every frame in Update()
        characterController = GetComponent<CharacterController>();

        // Defensive programming: Check if CharacterController exists
        // If someone forgets to add it, we'll get a helpful error message
        if (characterController == null)
        {
            Debug.LogError("FirstPersonController requires a CharacterController component! Please add one to " + gameObject.name);
        }
    }

    /// <summary>
    /// Called every frame. This is where we handle player input and update movement.
    /// </summary>
    /// <remarks>
    /// Update() is called once per frame by Unity's game loop.
    /// On a 60 FPS game, this runs 60 times per second.
    /// On a 30 FPS game, this runs 30 times per second.
    /// This is why we use Time.deltaTime - to make movement frame-rate independent.
    /// </remarks>
    void Update()
    {
        // First, check if the player is on the ground
        HandleGroundDetection();

        // Then handle horizontal movement (WASD)
        HandleMovement();

        // Then handle jumping (Space)
        HandleJump();

        // Finally, apply gravity to pull the player down
        ApplyGravity();
    }

    #endregion

    #region Movement Methods

    /// <summary>
    /// Checks if the player is standing on solid ground.
    /// Updates the isGrounded variable which is used for jump detection.
    /// </summary>
    private void HandleGroundDetection()
    {
        // CharacterController has a built-in isGrounded property that uses a small sphere cast
        // to detect if there's ground beneath the character.
        // It checks slightly below the character's feet (determined by the skin width).
        isGrounded = characterController.isGrounded;

        // When we're grounded and still falling (velocity.y < 0), we need to reset the velocity
        // to a small negative value to keep us "stuck" to the ground.
        // Without this, velocity.y would keep accumulating and we'd have a delay before jumping.
        if (isGrounded && velocity.y < 0)
        {
            // Set to a small negative value rather than 0 to ensure we stay grounded
            // This helps with walking down slopes and stairs
            velocity.y = -2f;
        }
    }

    /// <summary>
    /// Handles horizontal player movement (forward, backward, left, right) based on WASD input.
    /// Also handles running when Left Shift is held.
    /// </summary>
    private void HandleMovement()
    {
        // INPUT SYSTEM EXPLANATION:
        // Input.GetAxis() returns a value between -1 and 1:
        // - "Horizontal" axis: A = -1, D = 1, nothing = 0
        // - "Vertical" axis: S = -1, W = 1, nothing = 0
        // GetAxis() provides smooth interpolation, unlike GetKey() which is binary (on/off)
        
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float verticalInput = Input.GetAxis("Vertical");     // W/S or Up/Down arrows

        // VECTOR3 MATH EXPLANATION:
        // We need to move relative to where the player is facing, not world directions.
        // transform.right = the player's right direction (red axis in scene view)
        // transform.forward = the player's forward direction (blue axis in scene view)
        
        // Calculate movement direction relative to player's rotation
        // If player faces north and presses W, we want to move north
        // If player faces east and presses W, we want to move east
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;

        // Determine current speed (walking or running)
        // Input.GetKey() checks if a key is currently held down
        // LeftShift makes the player run
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= runSpeedMultiplier; // Apply run multiplier
        }

        // FRAME-RATE INDEPENDENCE:
        // Time.deltaTime is the time elapsed since the last frame (in seconds)
        // At 60 FPS: deltaTime ≈ 0.0167 seconds (1/60)
        // At 30 FPS: deltaTime ≈ 0.0333 seconds (1/30)
        // 
        // By multiplying movement by deltaTime, we ensure consistent speed regardless of FPS:
        // 60 FPS: moves (speed * 0.0167) per frame = speed units per second
        // 30 FPS: moves (speed * 0.0333) per frame = speed units per second
        // Without deltaTime, the player would move faster on higher FPS!

        // CharacterController.Move() moves the character by the specified vector
        // It handles collision detection automatically
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Handles jump input and applies jump velocity when the player presses Space.
    /// Only allows jumping when the player is grounded.
    /// </summary>
    private void HandleJump()
    {
        // Input.GetButtonDown() returns true only on the frame the button is FIRST pressed
        // This is different from GetButton() which returns true every frame while held
        // Using GetButtonDown() prevents infinite jumping while holding space
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // JUMP PHYSICS EXPLANATION:
            // To jump to a specific height, we need to calculate initial velocity.
            // Using physics equation: v² = u² + 2as
            // Where: v = final velocity (0 at peak), u = initial velocity (what we want),
            //        a = acceleration (gravity), s = displacement (jump height)
            // 
            // Rearranging: u = √(2 * |gravity| * height)
            // We use Mathf.Sqrt() for square root and negate gravity since it's negative
            
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
            // Example with default values:
            // jumpHeight = 2, gravity = -9.81
            // velocity.y = √(2 * 9.81 * 2) = √39.24 ≈ 6.26
            // This initial upward velocity of 6.26 will reach a height of 2 units
        }
    }

    /// <summary>
    /// Applies gravity to the player's vertical velocity and moves the player vertically.
    /// This creates realistic falling motion.
    /// </summary>
    private void ApplyGravity()
    {
        // GRAVITY ACCUMULATION:
        // Gravity is an acceleration, not a velocity.
        // Each frame, we add (gravity * deltaTime) to the current velocity.
        // This makes the player fall faster and faster over time (realistic acceleration).
        //
        // Example with gravity = -9.81:
        // Frame 1: velocity.y = -9.81 * 0.0167 = -0.164 (slow fall)
        // Frame 2: velocity.y = -0.164 + (-9.81 * 0.0167) = -0.328 (faster)
        // Frame 3: velocity.y = -0.328 + (-9.81 * 0.0167) = -0.492 (even faster)
        // And so on... this creates acceleration!
        
        velocity.y += gravity * Time.deltaTime;

        // Apply the vertical velocity to actually move the character
        // We multiply by deltaTime again to convert velocity (units/second) to displacement (units)
        characterController.Move(velocity * Time.deltaTime);

        // Note: We don't reset velocity.y here because it should persist across frames
        // It only resets when we hit the ground (in HandleGroundDetection)
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Optional: Draw gizmos in the Scene view for debugging ground detection.
    /// This helps visualize where the ground check is happening.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Only draw if we have a CharacterController
        if (characterController == null) return;

        // Draw a red sphere at the ground check position
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 groundCheckPos = transform.position - new Vector3(0, characterController.height / 2, 0);
        Gizmos.DrawWireSphere(groundCheckPos, groundCheckDistance);
    }

    #endregion

    // POTENTIAL EXPANSIONS for learning:
    // 
    // 1. CROUCHING SYSTEM:
    //    - Add a crouch key (C or Ctrl)
    //    - Reduce CharacterController.height when crouching
    //    - Reduce moveSpeed while crouched
    //    - Check if there's room to stand up before uncrouching
    //
    // 2. STAMINA SYSTEM:
    //    - Add a stamina variable (float currentStamina, float maxStamina)
    //    - Drain stamina while running
    //    - Prevent running when stamina is depleted
    //    - Regenerate stamina over time when not running
    //    - Display stamina bar UI
    //
    // 3. SLIDING/MOMENTUM:
    //    - Store previous movement direction
    //    - Lerp between old and new direction for smooth direction changes
    //    - Add acceleration and deceleration curves
    //
    // 4. DOUBLE JUMP:
    //    - Add a jumpCount variable
    //    - Allow jumping again in air if jumpCount < maxJumps (e.g., 2)
    //    - Reset jumpCount when grounded
    //
    // 5. FOOTSTEP SOUNDS:
    //    - Add AudioSource component
    //    - Play footstep sound based on movement speed
    //    - Vary pitch/volume for different surfaces
    //
    // 6. SLOPE HANDLING:
    //    - Check ground angle using raycast hit normal
    //    - Prevent movement up slopes steeper than maxSlopeAngle
    //    - Add sliding down steep slopes
}
