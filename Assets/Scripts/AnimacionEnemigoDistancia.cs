using UnityEngine;

public class AnimacionEnemigoRango : MonoBehaviour
{
    public Transform spriteTransform;
    public Rigidbody2D rb;

    [Header("Balanceo")]
    public float anguloMaximo = 6f;
    public float frecuencia = 6f;
    public float velocidadMinima = 0.05f;

    [Header("Flip")]
    public bool mirarDireccionMovimiento = true;

    private Quaternion rotacionInicial;
    private Vector2 ultimaPosicion;
    private float velocidadActual;
    private bool mirandoDerecha = true;

    void Start()
    {
        if (spriteTransform == null)
            spriteTransform = transform;

        if (rb == null)
            rb = GetComponentInParent<Rigidbody2D>();

        rotacionInicial = spriteTransform.localRotation;
        ultimaPosicion = rb.position;
    }

    void FixedUpdate()
    {
        Vector2 posicionActual = rb.position;

        // Velocidad real
        velocidadActual = (posicionActual - ultimaPosicion).magnitude / Time.fixedDeltaTime;

        // Flip según movimiento horizontal
        float deltaX = posicionActual.x - ultimaPosicion.x;
        if (mirarDireccionMovimiento && Mathf.Abs(deltaX) > 0.001f)
        {
            if (deltaX > 0 && !mirandoDerecha)
                Flip();
            else if (deltaX < 0 && mirandoDerecha)
                Flip();
        }

        ultimaPosicion = posicionActual;
    }

    void Update()
    {
        bool seEstaMoviendo = velocidadActual > velocidadMinima;

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

    void Flip()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = spriteTransform.localScale;
        escala.x *= -1;
        spriteTransform.localScale = escala;
    }
}
