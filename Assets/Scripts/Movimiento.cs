using UnityEngine;
using UnityEngine.UI;

public class Movimiento : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public Text dashText;

    public float dashSpeed = 16f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private Vector2 dashDirection;

    private float _dashTimer;
    private float dashTimer
    {
        get { return _dashTimer; }
        set { _dashTimer = Mathf.Max(0f, value); }
    }

    private float _cooldownTimer;
    private float cooldownTimer
    {
        get { return _cooldownTimer; }
        set
        {
            _cooldownTimer = Mathf.Max(0f, value);
            ActualizarUI();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement.Normalize();

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.Space) && dashTimer <= 0f && cooldownTimer <= 0f && movement != Vector2.zero)
        {
            dashDirection = movement;
            dashTimer = dashDuration;
        }

        if (dashTimer > 0f)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                cooldownTimer = dashCooldown;
            }
        }
        else
        {
            rb.linearVelocity = movement * moveSpeed;
        }
    }

    void ActualizarUI()
    {
        if (dashText != null)
        {
            float newAlpha = cooldownTimer <= 0f ? 1f : 0.2f;
            Color newColor = dashText.color;
            newColor.a = newAlpha;
            dashText.color = newColor;
        }
    }
}