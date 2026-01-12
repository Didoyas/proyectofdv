using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    private InputActionMap playerActionMap;
    private InputAction moveAction;
    private InputAction dashAction;
    private Rigidbody2D rb;
    public InputActionAsset playerInputActionAsset;
    [SerializeField] private Text dashText;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 16f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private DashGestor dashGestor;
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

    public void Awake()
    {
        playerActionMap = playerInputActionAsset.FindActionMap("Player");
        moveAction = playerActionMap.FindAction("Move");
        dashAction = playerActionMap.FindAction("Dash");
    }

    void OnEnable() { playerActionMap.Enable(); }
    void OnDisable() { playerActionMap.Disable(); }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movement = moveAction.ReadValue<Vector2>();
        movement.Normalize();

        bool dashPressed = dashAction.WasPressedThisFrame();
        if (dashPressed && dashTimer <= 0f && cooldownTimer <= 0f && movement != Vector2.zero)
        {
            dashDirection = movement;
            dashTimer = dashDuration;

            VidaPlayer vidaPlayer = GetComponent<VidaPlayer>();
            if (vidaPlayer != null)
            {
                StartCoroutine(vidaPlayer.HacerInvulnerable(dashDuration));
            }
        }

        if (dashGestor != null)
            dashGestor.ActualizarImagen(dashTimer > 0f);
    }

    void FixedUpdate()
    {
        Vector2 movement = moveAction.ReadValue<Vector2>();
        movement.Normalize();

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.fixedDeltaTime;

        if (dashTimer > 0f)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0f)
                cooldownTimer = dashDuration;
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

    // Animacion
    public Vector2 VelocidadActual => rb.linearVelocity;

    // CLAVE PARA LA VOLTERETA
    public bool EstaHaciendoDash => dashTimer > 0f;

    // Para saber hacia qué lado se hace el dash
    public float DireccionDashX => dashDirection.x;

}
