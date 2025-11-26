using UnityEngine;

public class Llave : MonoBehaviour
{

    public float distanciaDeteccion = 3f;   // Distancia a la que la llave "detecta" al jugador
    public float velocidad = 5f;            // Velocidad de seguimiento
    public float distanciaDestruccion = 1f; // Distancia para destruir la puerta
    private Transform jugador;

    public Transform puerta;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Buscar al jugador por tag
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
        }  

    }

    // Update is called once per frame
    void Update()
    {
 
        if (jugador == null || puerta == null) return;

        
            
        // Distancia entre la llave y el jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // Si el jugador está cerca, la llave lo sigue
        if (distancia <= distanciaDeteccion && distancia > 0.3f)
        {
            transform.position = Vector3.Lerp(transform.position, jugador.position, velocidad * Time.deltaTime);
        }

        // Verificar cercanía con la puerta
        
            float distanciaPuerta = Vector3.Distance(transform.position, puerta.position);

            if (distanciaPuerta <= distanciaDestruccion)
            {
                Destroy(puerta.gameObject);   // Destruir la puerta
                Destroy(gameObject); // Destruir la llave
            }
        }

    
}
