using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float gravity;
    [SerializeField] public float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float fallOverTime;
    [SerializeField] private float physicsMultiplier;
    [SerializeField] private CameraSettings cameraSettings;
    [SerializeField] private DashSettings dashSettings;

    [Space(20)]

    [SerializeField] private References references;

    private CharacterController cc;
    private Animator animator;

    /// <summary> The input vector for movement </summary>
    private Vector2 movement;

    /// <summary> The change in position applied at the end of this frame </summary>
    private Vector3 delta;

    /// <summary> The current vertical velocity of the player </summary>
    private float vertical;

    /// <summary> The amount of coyote time left </summary>
    private float coyote;

    /// <summary> The time left that a jump will activate </summary>
    private float jump = -1;

    /// <summary> Dash cooldown </summary>
    private float dash = -1;

    /// <summary> The current velocity of the dash </summary>
    private Vector3 velocity;

    /// <summary> The item the player is currently holding </summary>
    [HideInInspector] public Item heldItem;

    /// <summary> Time since fallen over </summary>
    private float fallen = 0;

    /// <summary> All interactables in range </summary>
    private readonly List<IInteractable> interactables = new();

    /// <summary> The nearest interactable in range </summary>
    private IInteractable interactable;

    /// <summary> Wether input is disabled </summary>
    private bool frozen;

    /// <summary> Wether the input is currently  </summary>
    private bool active = true;

    /// <summary> The offset of the camera </summary>
    private Vector3 cameraMax;

    #region Events

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //Cursor.lockState = CursorLockMode.Locked;
        cameraMax = Camera.main.transform.position - transform.position;
    }

    private void Update()
    {
        delta = Vector3.zero;
        frozen = !active;

        ApplyFallen();
        ApplyRotation();
        ApplyJump();
        ApplyGravity();
        ApplyMovement();
        UpdateInteractables();
        UpdateCamera();

        if (dash > 0) dash -= Time.deltaTime;
        references.trail.emitting = dash > dashSettings.dashCooldown - 0.5;

        animator.SetFloat("Fallen", fallen);
        animator.SetFloat("Vertical", vertical);
        animator.SetBool("Grounded", cc.isGrounded);
        animator.SetBool("Holding", heldItem != null);
    }

    private void OnDestroy()
    {
        if (references.marker != null) Destroy(references.marker.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spill")) Fall(other.transform.position);
        if (other.TryGetComponent(out IInteractable interactable)) interactables.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable)) interactables.Remove(interactable);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody == null) return;
        hit.rigidbody.AddForceAtPosition(
            delta * physicsMultiplier,
            hit.point,
            ForceMode.Force
        );

        if (hit.collider.CompareTag("Customer"))
        {
            hit.gameObject.GetComponent<CustomerController>().Collide(this);
            
        }
    }

    public bool IsHolding()
    {
        return heldItem != null;
    }

    public void Pickup(GameObject gameObject)
    {
        if (heldItem != null) return;
        heldItem = gameObject.GetComponent<Item>();
        heldItem.transform.parent = references.itemAnchor;
        heldItem.transform.localRotation = Quaternion.identity;
        heldItem.transform.localPosition = Vector3.zero;
    }

    #endregion

    public void ClearHeld()
    {
        Destroy(heldItem.gameObject);
        heldItem = null;
    }

    public void Throw()
    {
        if (heldItem == null) return;
        heldItem.transform.parent = null;
        Rigidbody rb = heldItem.gameObject.AddComponent<Rigidbody>();
        rb.AddForce(animator.transform.forward * 15 + delta + new Vector3(0, 5, 0), ForceMode.Impulse);
        heldItem = null;   
    }

    public void Fall(Vector3 point)
    {
        if (fallen > 0) return;

        delta = Vector3.zero;
        velocity = (transform.position - point).normalized * 25;
        animator.SetTrigger("Slip");
        fallen = fallOverTime;
        vertical = 20;
        GetComponent<Clang>().ClangPlay();
        Throw();
    }

    public void SpeedUp()
    {
        moveSpeed += 12;
    }
    public void SpeedDown()
    {
        moveSpeed -= 12;
    }
    public void SetActive(bool active)
    {
        Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.Confined;
        this.active = active;
    }
    public void JumpSound()
    {
        AudioManager.SoundEnum[] jumps = { AudioManager.SoundEnum.jump1, AudioManager.SoundEnum.jump2, AudioManager.SoundEnum.jump3, AudioManager.SoundEnum.jump4 };
        AudioManager.instance.PlaySound(jumps[UnityEngine.Random.Range(0, jumps.Length)],100);
    }
    #region Updates

    private void UpdateInteractables()
    {
        interactable = null;
        if (!frozen) interactable = interactables
            .Where(x => x.IsVisible())
            .OrderBy(x => (transform.position - x.transform.position).sqrMagnitude)
            .FirstOrDefault();
        references.marker.interactable = interactable;
    }

    private void ApplyFallen()
    {
        if (fallen <= 0) return;
        fallen -= Time.deltaTime;
        frozen = true;
    }

    private void ApplyJump()
    {
        if (frozen) jump = -1;
        if (coyote < 0) jump -= Time.deltaTime;
        else if (jump >= 0)
        {
            animator.SetTrigger("Jump");
            vertical = jumpPower;
            coyote = -1;
            jump = -1;
        }
    }

    private void ApplyRotation()
    {
        if (frozen) return;

        float y = Camera.main.transform.eulerAngles.y;
        delta = Quaternion.Euler(0, y, 0) * new Vector3(movement.x, 0, movement.y);

        if (delta == Vector3.zero) return;
        Quaternion rotation = Quaternion.LookRotation(delta, Vector3.up);
        animator.transform.rotation = Quaternion.Lerp(
            transform.rotation,
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
        delta.y = vertical;
    }

    private void ApplyMovement()
    {
        delta += velocity;
        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 10);

        if (delta == Vector3.zero) return;
        cc.Move(delta * Time.deltaTime);
    }

    private void UpdateCamera()
    {
        Ray ray;
        RaycastHit hit;

        int mask = LayerMask.GetMask("Structure");

        ray = new Ray(transform.position + new Vector3(0, 1, 0), Vector3.up);
        float y = Physics.Raycast(ray, out hit, cameraMax.y, mask) ? hit.distance : cameraMax.y;

        ray = new Ray(transform.position + new Vector3(0, y - 1, 0), Vector3.left);
        float x = Physics.Raycast(ray, out hit, Mathf.Abs(cameraMax.x), mask) ? -hit.distance : cameraMax.x;
        
        Vector3 target = transform.position + new Vector3(x + 1, y, 0);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target, Time.deltaTime * 10);

        Camera.main.transform.rotation = Quaternion.Euler(-Mathf.Atan(y / x) * Mathf.Rad2Deg, 90, 0);
    }

    #endregion

    #region Input

    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>() * moveSpeed;
        delta = new Vector3(movement.x, 0, movement.y);
        animator.SetFloat("Speed", movement.magnitude);
    }

    public void OnJump()
    {
        jump = coyoteTime;
        JumpSound();
    }

    public void OnDash()
    {
        if (frozen || dash > 0 || movement == Vector2.zero) return;
        velocity = delta * dashSettings.dashPower;
        dash = dashSettings.dashCooldown;
        AudioManager.instance.PlaySound(AudioManager.SoundEnum.dashSound,100);
    }

    public void OnInteract()
    {
        if (frozen || interactable == null || !interactable.IsActive()) return;
        interactable.Interact(this);
    }

    public void OnThrow()
    {
        if (frozen || heldItem == null) return;
        animator.SetTrigger("Throw");
        Util.RunAfter(0.15f, Throw);
    }

    #endregion

    #region Structs

    [Serializable]
    public struct References
    {
        public Transform itemAnchor;
        public Marker marker;
        public TrailRenderer trail;
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
