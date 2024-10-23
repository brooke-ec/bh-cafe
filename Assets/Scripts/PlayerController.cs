using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;

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
    private Vector3 delta;

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
    private Vector3 velocity;

    /// <summary> The item the player is currently holding </summary>
    private GameObject heldItem;

    /// <summary> Time since fallen over </summary>
    private float fallen = 0;

    /// <summary> A list of interactables in range </summary>
    private List<Interactable> interactables = new List<Interactable>();

    /// <summary> The nearest interactable in range </summary>
    private Interactable closestInteractable;

    /// <summary> Wether input is disabled </summary>
    private bool disabled = false;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        targetZoom = references.camera.position.z;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        delta = Vector3.zero;
        disabled = false;

        ApplyFallen();
        ApplyRotation();
        ApplyJump();
        ApplyGravity();
        ApplyMovement();
        UpdateZoom();
        UpdateInteractables();

        if (dash > 0) dash -= Time.deltaTime;

        references.animator.SetFloat("Fallen", fallen);
        references.animator.SetFloat("Vertical", vertical);
        references.animator.SetBool("Grounded", cc.isGrounded);
        references.animator.SetBool("Holding", heldItem != null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spill")) Fall(other.transform.position);
        if (other.TryGetComponent(out Interactable interactable)) interactables.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            interactables.Remove(interactable);
            interactable.active = false;
        }
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
            Fall(hit.point);
            life--;
        }
    }

    public bool IsHolding()
    {
        return heldItem != null;
    }

    public void Pickup(GameObject gameObject)
    {
        if (heldItem != null) return;
        heldItem = gameObject;
        heldItem.transform.parent = references.itemAnchor;
        heldItem.transform.localRotation = Quaternion.identity;
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localScale = Vector3.one;
    }

    public void Throw()
    {
        if (heldItem == null) return;
        heldItem.transform.parent = null;
        Rigidbody rb = heldItem.AddComponent<Rigidbody>();
        rb.AddForce(references.animator.transform.forward * 15 + delta + new Vector3(0, 5, 0), ForceMode.Impulse);
        heldItem = null;   
    }

    private void Fall(Vector3 point)
    {
        if (fallen > 0) return;

        Throw();
        velocity = (transform.position - point).normalized * 25;
        references.animator.SetTrigger("Slip");
        fallen = fallOverTime;
        vertical = 20;
    }

    #region Updates

    private void UpdateInteractables()
    {
        float closest = float.PositiveInfinity;
        closestInteractable = null;

        foreach (Interactable interactable in interactables)
        {
            interactable.active = false;
            Vector3 distance = transform.position - interactable.transform.position;
            if (distance.sqrMagnitude < closest) closestInteractable = interactable;
        }

        if (closestInteractable != null) closestInteractable.active = true;
    }

    public void ApplyFallen()
    {
        if (fallen <= 0) return;
        fallen -= Time.deltaTime;
        disabled = true;
    }

    public void ApplyJump()
    {
        if (disabled) jump = -1;
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

        if (disabled) return;

        delta = Quaternion.Euler(0, y, 0) * new Vector3(movement.x, 0, movement.y);

        if (delta == Vector3.zero) return;
        Quaternion rotation = Quaternion.LookRotation(delta, Vector3.up);
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
        delta.y = vertical;
    }

    private void ApplyMovement()
    {
        delta += velocity;
        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 10);

        if (delta == Vector3.zero) return;
        cc.Move(delta * Time.deltaTime);
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

    #endregion

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
        delta = new Vector3(movement.x, 0, movement.y);
        references.animator.SetFloat("Speed", movement.magnitude);
    }

    public void OnJump()
    {
        jump = coyoteTime;
    }

    public void OnDash()
    {
        if (dash > 0 || disabled || movement == Vector2.zero) return;
        velocity = delta * dashSettings.dashPower;
        dash = dashSettings.dashCooldown;
    }

    public void OnInteract()
    {
        if (closestInteractable == null) return;
        closestInteractable.Interact();
    }

    public void OnThrow()
    {
        if (disabled || heldItem == null) return;
        references.animator.SetTrigger("Throw");
        Util.RunAfter(0.15f, Throw);
    }

    #endregion

    #region Structs

    [Serializable]
    public struct References
    {
        public Transform cameraAnchor;
        public Transform camera;
        public Animator animator;
        public Transform itemAnchor;
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
