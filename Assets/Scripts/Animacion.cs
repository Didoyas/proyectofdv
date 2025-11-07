using UnityEngine;

public class Animacion : MonoBehaviour
{
    public Transform spriteTransform;

    public Movimiento movimientoJugador;

    public float amplitud = 0.1f;
    public float frecuencia = 6f;

    private Vector3 posicionInicial;

    void Start()
    {
        if (spriteTransform == null)
            spriteTransform = transform;

        posicionInicial = spriteTransform.localPosition;
    }

    void Update()
    {
        bool seEstaMoviendo = true;

        // Velocidad de Movimiento.cs
        if (movimientoJugador != null)
        {
            seEstaMoviendo = movimientoJugador.VelocidadActual.magnitude > 0.1f;
        }

        if (seEstaMoviendo)
        {
            float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * frecuencia) * amplitud;
            spriteTransform.localPosition = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
        }
        else
        {
            spriteTransform.localPosition = Vector3.Lerp(
                spriteTransform.localPosition,
                posicionInicial,
                Time.deltaTime * 5f
            );
        }
    }
}