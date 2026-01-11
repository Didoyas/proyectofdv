using UnityEngine;

public class Animacion : MonoBehaviour
{
    public Transform spriteTransform;
    public Movimiento movimientoJugador;
    public SpriteRenderer spriteRenderer;

    public float anguloMaximo = 8f;
    public float frecuencia = 6f;

    private Quaternion rotacionInicial;

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
        bool seEstaMoviendo = true;

        if (movimientoJugador != null)
        {
            Vector2 velocidad = movimientoJugador.VelocidadActual;
            seEstaMoviendo = velocidad.magnitude > 0.1f;

            // 👉 FLIP izquierda / derecha (A / D)
            if (Mathf.Abs(velocidad.x) > 0.01f)
            {
                spriteRenderer.flipX = velocidad.x < 0;
            }
        }

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
