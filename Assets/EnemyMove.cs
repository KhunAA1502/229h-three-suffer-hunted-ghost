using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float patrolDistance = 5f;
    [SerializeField] private bool facingRight = true;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Edge Check")] // ส่วนเพิ่มเติมสำหรับตรวจสอบขอบ
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private float edgeCheckDistance = 0.5f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool movingRight = true;
    private bool isGrounded;
    private bool isAtEdge; // ตัวแปรเพิ่มเติมสำหรับตรวจสอบขอบ

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Update()
    {
        // ตรวจสอบพื้น
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        // ตรวจสอบว่ามีพื้นข้างหน้าหรือไม่ (ส่วนเพิ่มเติม)
        RaycastHit2D edgeHit = Physics2D.Raycast(edgeCheck.position, Vector2.down, edgeCheckDistance, groundLayer);
        isAtEdge = edgeHit.collider == null;

        // เปลี่ยนทิศทางเมื่อถึงจุดสิ้นสุดหรือไม่มีพื้นข้างหน้า
        if ((Mathf.Abs(transform.position.x - startPosition.x) >= patrolDistance && isGrounded) || isAtEdge)
        {
            movingRight = !movingRight;
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            float moveDirection = movingRight ? 1 : -1;
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 endPositionRight = Application.isPlaying ? 
            new Vector2(startPosition.x + patrolDistance, transform.position.y) : 
            new Vector2(transform.position.x + patrolDistance, transform.position.y);
        
        Vector2 endPositionLeft = Application.isPlaying ? 
            new Vector2(startPosition.x - patrolDistance, transform.position.y) : 
            new Vector2(transform.position.x - patrolDistance, transform.position.y);
        
        Gizmos.DrawLine(transform.position, endPositionRight);
        Gizmos.DrawLine(transform.position, endPositionLeft);

        // วาดเส้นตรวจสอบขอบ (ส่วนเพิ่มเติม)
        if (edgeCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * edgeCheckDistance);
        }
    }
}