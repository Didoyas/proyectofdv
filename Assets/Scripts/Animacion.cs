using UnityEngine;

public class Animacion : MonoBehaviour
{
    public Transform spriteTransform;
    public Movimiento movimientoJugador;
    public SpriteRenderer spriteRenderer;

    [Header("Balanceo normal")]
    public float anguloMaximo = 8f;
    public float frecuencia = 6f;

    [Header("Dash")]
    public float velocidadVoltereta = 720f; // grados/segundo

    private Quaternion rotacionInicial;
    private bool estabaDashing = false;
    private float rotacionDash = 0f;
    private float direccionGiro = 1f;

    void Start()
    {
        if (spriteTransform == null)
            spriteTransform = transform;

        if (spriteRenderer == null)
            spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();

        rotacionInicial = spriteTransform.localRotation;
    }

    void Update()
    {
        if (movimientoJugador == null) return;

        // ───────── DASH ─────────
        if (movimientoJugador.EstaHaciendoDash)
        {
            if (!estabaDashing)
            {
                rotacionDash = 0f;
                estabaDashing = true;

                // 👉 determinar sentido del giro
                float dirX = movimientoJugador.DireccionDashX;
                direccionGiro = Mathf.Abs(dirX) < 0.01f ? 1f : Mathf.Sign(dirX);
            }

            rotacionDash += velocidadVoltereta * direccionGiro * Time.deltaTime;
            spriteTransform.localRotation = Quaternion.Euler(0f, 0f, rotacionDash);
            return;
        }
        else if (estabaDashing)
        {
            estabaDashing = false;
            spriteTransform.localRotation = rotacionInicial;
        }

        // ───────── MOVIMIENTO NORMAL ─────────
        Vector2 velocidad = movimientoJugador.VelocidadActual;
        bool seEstaMoviendo = velocidad.magnitude > 0.1f;

        if (Mathf.Abs(velocidad.x) > 0.01f)
            spriteRenderer.flipX = velocidad.x < 0;

        if (seEstaMoviendo)
        {
            float angulo = Mathf.Sin(Time.time * frecuencia) * anguloMaximo;
            spriteTransform.localRotation = Quaternion.Euler(0f, 0f, angulo);
        }
        else
        {
            spriteTransform.localRotation = Quaternion.Lerp(
                spriteTransform.localRotation,
                rotacionInicial,
                Time.deltaTime * 5f
            );
        }
    }
}
