using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))] // Asegura que tenemos el componente
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 8f;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // Componentes y Estado
    private Rigidbody2D rb;
    private Animator animator; // Referencia al animator
    private float horizontalInput;
    private bool isGrounded;
    private bool facingRight = true; // Para girar el sprite

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Obtenemos el animator en Start
    }

    void Update()
    {
        // 1. Entrada y Detección
        horizontalInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2. Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 3. Gestionar orientación (Girar el sprite)
        if (horizontalInput > 0 && !facingRight) { Flip(); }
        else if (horizontalInput < 0 && facingRight) { Flip(); }

        // 4. ACTUALIZAR ANIMADOR (NUEVO)
        // Usamos Math.Abs para que Speed siempre sea positiva, aunque corra a la izquierda.
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("verticalVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        // Movimiento físico
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
    }

    // Método para girar el personaje visualmente
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1; // Invierte la escala X para girar
        transform.localScale = scaler;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}