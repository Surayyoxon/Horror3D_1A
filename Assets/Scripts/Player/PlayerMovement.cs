using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float crouchSpeed = 2f;

    [Header("Jump")]
    public float jumpHeight = 1.5f;

    [Header("Gravity")]
    public float gravity = -20f;

    [Header("Crouch")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;

    [Header("Footsteps")]
    public AudioClip[] footstepSounds;
    public float walkStepRate = 0.5f;
    public float sprintStepRate = 0.35f;
    public float crouchStepRate = 0.8f;

    private CharacterController controller;
    private AudioSource audioSource;
    private Vector3 velocity;

    private float stepTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        HandleCrouch();
        HandleFootsteps();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move =
            transform.right * x +
            transform.forward * z;

        float currentSpeed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed = sprintSpeed;

        if (Input.GetKey(KeyCode.LeftControl))
            currentSpeed = crouchSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controller.height = crouchHeight;
        }
        else
        {
            controller.height = standingHeight;
        }
    }

    void HandleFootsteps()
    {
        if (!controller.isGrounded)
            return;

        bool isMoving =
            Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 ||
            Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0;

        if (!isMoving)
        {
            stepTimer = 0f;
            return;
        }

        float stepRate = walkStepRate;

        if (Input.GetKey(KeyCode.LeftShift))
            stepRate = sprintStepRate;

        if (Input.GetKey(KeyCode.LeftControl))
            stepRate = crouchStepRate;

        stepTimer += Time.deltaTime;

        if (stepTimer >= stepRate)
        {
            PlayFootstep();
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        if (footstepSounds == null || footstepSounds.Length == 0)
            return;

        AudioClip clip =
            footstepSounds[Random.Range(0, footstepSounds.Length)];

        audioSource.PlayOneShot(clip);
    }
}
