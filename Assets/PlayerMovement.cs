using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public float dashSpeed = 10f;
    [SerializeField] public float dashDuration = 0.5f;
    [SerializeField] public float jumpForce = 10f;
    
    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private int maxJumpCount = 2; // this is so the player can double jump

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = cameraTransform.forward * v + cameraTransform.right * h;
        moveDir.y = 0;  // Prevent vertical movement
        rb.MovePosition(rb.position + moveDir * speed * Time.deltaTime);
    }

    void Jump()
    {
        if (jumpCount < maxJumpCount && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpCount++;
        }
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
