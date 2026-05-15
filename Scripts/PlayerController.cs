using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 input;

    // Flip için:
    private SpriteRenderer sr;

    public Vector2 CurrentVelocity => rb.velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        sr = GetComponent<SpriteRenderer>(); // Player’ýn SpriteRenderer’ýný yakala
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        input = new Vector2(x, y).normalized;

        // Flip: saða gidince normal, sola gidince ters
        if (sr != null && Mathf.Abs(x) > 0.01f)
            sr.flipX = x < 0f;
    }

    private void FixedUpdate()
    {
        rb.velocity = input * speed;
    }
}