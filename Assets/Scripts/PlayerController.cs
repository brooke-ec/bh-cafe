using UnityEngine.InputSystem;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float gravity;
    [SerializeField] public float moveSpeed;
    [SerializeField] public int life;
    [SerializeField] private float jumpPower;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float physicsMultiplier;
    [SerializeField] private CameraSettings cameraSettings;
    [SerializeField] private DashSettings dashSettings;
    [SerializeField] private float fallOverTime;

    [Space(20)]

    [SerializeField] private References references;

    private CharacterController cc;

    /// <summary> The target distance of the camera from the player </summary>
    private float targetZoom;

    /// <summary> The input vector for movement </summary>
    private Vector2 movement;

    /// <summary> The change in position applied at the end of this frame </summary>
    private Vector3 velocity;

    /// <summary> The current vertical velocity of the player </summary>
    private float vertical;

    /// <summary> The input vector of the look control </summary>
    private Vector2 look;

    /// <summary> The amount of coyote time left </summary>
    private float coyote;

    /// <summary> The time left that a jump will activate </summary>
    private float jump = -1;

    /// <summary> Dash cooldown </summary>
    private float dash = -1;

    /// <summary> The current velocity of the dash </summary>
    private Vector3 dashVelocity;

    /// <summary> The item the player is currently holding </summary>
    private GameObject heldItem;

    /// <summary> Time since fallen over</summary>
    private float fallOverSince = -1;

    /// <summary> check if slipped</summary>
    private float slipped = 0;

    /// <summary> The current interactable </summary>
    public Interactable interactable;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        targetZoom = references.camera.position.z;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        ApplyRotation();
        ApplyDash();
        ApplyJump();
        ApplyGravity();
        ApplyFallOver();
        ApplyMovement();
        UpdateZoom();

        references.animator.SetBool("Grounded", cc.isGrounded);
        references.animator.SetFloat("Vertical", vertical);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out interactable);

        
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Spill")&& slipped > 0.1)
        {
            slipped = -1;
            FallOver();
        }
        else if (other.CompareTag("Spill") && slipped >=0 )
        {
            slipped += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spill"))
        {
            slipped = 0;
        }

        if (other.gameObject == interactable.gameObject) interactable = null;

        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody == null) return;
        hit.rigidbody.AddForceAtPosition(
            velocity * physicsMultiplier,
            hit.point,
            ForceMode.Force
        );
        if (hit.collider.CompareTag("Customer") && fallOverSince<0)
        {
            Debug.Log("Hit WIth Customer");
            life -= 1;
            FallOver();
        }
    }

    public void ApplyDash()
    {
        velocity += dashVelocity;
        dashVelocity = Vector3.Lerp(dashVelocity, Vector3.zero, Time.deltaTime * 10);
        dash -= Time.deltaTime;
    }

    public void ApplyJump()
    {
        if (coyote < 0) jump -= Time.deltaTime;
        else if (jump >= 0)
        {
            references.animator.SetTrigger("Jump");
            vertical = jumpPower;
            coyote = -1;
            jump = -1;
        }
    }

    private void ApplyRotation()
    {
        float x = Util.ClampOut(references.cameraAnchor.eulerAngles.x + look.y, 80, 280);
        float y = references.cameraAnchor.eulerAngles.y + look.x;
        references.cameraAnchor.eulerAngles = new Vector3(x, y, 0);

        velocity = Quaternion.Euler(0, y, 0) * new Vector3(movement.x, 0, movement.y);

        if (velocity == Vector3.zero) return;
        Quaternion rotation = Quaternion.LookRotation(velocity, Vector3.up);
        references.animator.transform.rotation = Quaternion.Lerp(
            references.animator.transform.rotation,
            rotation,
            Time.deltaTime * 8
        );
    }

    private void ApplyGravity()
    {
        if (cc.isGrounded) coyote = coyoteTime;
        else coyote -= Time.deltaTime;

        if (cc.isGrounded && vertical < 0) vertical = -1;
        else vertical -= gravity * Time.deltaTime;
        velocity.y = vertical;
    }

    private void ApplyMovement()
    {
        if (velocity == Vector3.zero) return;
        cc.Move(velocity * Time.deltaTime);
    }

    private void UpdateZoom()
    {
        Ray ray = new Ray(references.cameraAnchor.position, -references.cameraAnchor.forward);
        float zoom = Physics.Raycast(ray, out RaycastHit hit, Mathf.Abs(targetZoom)) ? 1 - hit.distance : targetZoom;

        references.camera.localPosition = Vector3.Lerp(
            references.camera.localPosition,
            new Vector3(0, 0, zoom),
            Time.deltaTime * 10
        );
    }

    private void ApplyFallOver()
    {
        if(fallOverSince < 0)
        {
            return;
        }
        else if(fallOverSince<fallOverTime)
        {
            velocity = Vector3.zero;
            fallOverSince += Time.deltaTime;

        }
        else
        {
            GetUp();
        }
    }

    private void FallOver()
    {
        references.animator.SetInteger("FallOver", 1);
        fallOverSince = 0;
    }

    private void GetUp()
    {
        references.animator.SetInteger("FallOver", 0);
        fallOverSince = -1;
    }


    #region Input

    public void OnLook(InputValue input)
    {
        look = input.Get<Vector2>() * cameraSettings.lookSensitivity;
    }

    public void OnZoom(InputValue input) 
    {
        Vector2 delta = input.Get<Vector2>();
        targetZoom = Mathf.Clamp(
            targetZoom + delta.y * cameraSettings.zoomSpeed,
            -cameraSettings.maxZoom,
            -cameraSettings.minZoom
        );
    }

    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>() * moveSpeed;
        velocity = new Vector3(movement.x, 0, movement.y);
        references.animator.SetFloat("Speed", movement.magnitude);
    }

    public void OnJump()
    {
        jump = coyoteTime;
    }

    public void OnDash()
    {
        if (dash > 0) return;
        dashVelocity = velocity * dashSettings.dashPower;
        dash = dashSettings.dashCooldown;
    }

    #endregion

    #region Structs

    [Serializable]
    public struct References
    {
        public Transform cameraAnchor;
        public Transform camera;
        public Animator animator;
    }

    [Serializable]
    public struct CameraSettings
    {
        public float lookSensitivity;
        public float zoomSpeed;
        public float minZoom;
        public float maxZoom;
    }

    [Serializable]
    public struct DashSettings
    {
        public float dashPower;
        public float dashCooldown;
    }

    #endregion
}
