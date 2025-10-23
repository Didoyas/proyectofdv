using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{

    public Transform player; // El objeto del jugador
    public Vector3 offset= new Vector3(0, 0, -10); // Distancia de la cámara respecto al jugador
    public float smoothSpeed = 0.125f; // Velocidad de la camara en el seguimiento al jugador

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = player.position + offset; // La camara inicia en la posicion del jugador
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 nuevaPos = player.position + offset; // Nueva posicion de la camara
        Vector3 smoothedPos = Vector3.Lerp(transform.position, nuevaPos, smoothSpeed); //movimiento de la camara
        transform.position = new Vector3(smoothedPos.x, smoothedPos.y, transform.position.z); //actualizacion de posicion de la camara

    }
}
