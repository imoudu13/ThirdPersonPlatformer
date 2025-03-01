using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float acceleration = 10f;
    public float airControlFactor = 0.5f; // less control while jumping
    public float jumpForce = 10f;
    [SerializeField] public float gravityMultiplier = 4f; // add gravity when jumping
    public float dashForce = 15f;
    public float dashDuration = 0.3f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        Move();
        Jump();
        ApplyExtraGravity();

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; 
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 moveDir = (cameraForward * v + cameraRight * h).normalized;

        float controlFactor = isGrounded ? 1f : airControlFactor; // so you have less control in the air
        Vector3 targetVelocity = moveDir * moveSpeed * controlFactor;

        // smooth accel & decel
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z), acceleration * Time.deltaTime);
    }

    void Jump()
    {
        if (jumpCount < maxJumps && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpCount++;
        }
    }

    void ApplyExtraGravity()
    {
        if (!isGrounded)
        {
            rb.linearVelocity += (Vector3.down * gravityMultiplier * Time.deltaTime);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        Vector3 dashDirection = rb.linearVelocity.normalized;
        if (dashDirection.magnitude == 0) 
            dashDirection = cameraTransform.forward;

        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
