using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float dashSpeedMultiplier = 2f;
    public float dashDuration = 0.5f;
    public Transform cameraTransform;
    public TextMeshProUGUI scoreText;

    private Rigidbody rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private int maxJumps = 2; // For double jump
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        // Update UI
        if (scoreText != null)
        {
            scoreText.text = "Score: " + CoinCollector.score;
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Get the camera's forward direction, ignoring vertical tilt
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Ignore up/down tilt
        cameraForward.Normalize(); // Keep movement smooth

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Move relative to the camera
        Vector3 moveDir = (cameraForward * v + cameraRight * h).normalized;
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
    }


    void Jump()
    {
        if (jumpCount < maxJumps && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpCount++;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float originalSpeed = moveSpeed;
        moveSpeed *= dashSpeedMultiplier; // Increase speed

        yield return new WaitForSeconds(dashDuration);

        moveSpeed = originalSpeed; // Reset speed
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
