using UnityEngine;

public class EnemigoPesado : MonoBehaviour, IRecibeImpactoRetroceso
{

    private Rigidbody2D rb;
    public float velocidad = 2f;          // Velocidad lenta
    public float rangoDeteccion = 8f;    // Distancia máxima para detectar al jugador

    public float rangoAtaque = 1f;        // Distancia de ataque cuerpo a cuerpo
    public int vida = 5;
    public int daño = 2;                 // Daño al jugador
    public float tiempoEntreAtaques = 2f; // Enfriamiento entre ataques

    private Transform target;           // Referencia al jugador
    private float tiempoUltimoAtaque = 0f;

    private Vector2 posicionInicial; // Posición donde empezó el enemigo

    private float distanciaRetroceso = 1f; 

    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;    
        rb.freezeRotation = true;
        posicionInicial = rb.position; // Guardamos la posición inicial

        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null)
        {
            throw new System.Exception("No se encontró el jugador");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        float distancia = Vector2.Distance(transform.position, target.position);

        // Si el jugador está dentro del rango de detección
        if (distancia <= rangoDeteccion)
        {
            // Moverse hacia el jugador
            MoverHacia(target.position);

            // Atacar si está suficientemente cerca
            if (distancia <= rangoAtaque && Time.time >= tiempoUltimoAtaque + tiempoEntreAtaques)
            {
                Atacar();
                tiempoUltimoAtaque = Time.time;
            }
        }
        else
        {
            // Regresar a la posición inicial
            float distanciaAlInicio = Vector2.Distance(rb.position, posicionInicial);
            if (distanciaAlInicio > 0.05f) //rango para no estar buscando el inicio en bucle 
            {
                MoverHacia(posicionInicial);
            }
            else
            {
                rb.position = posicionInicial; // asegurar que quede exacto
            }
        }
        

    }

    
    void MoverHacia(Vector2 destino)
    {
        Vector2 direccion = (destino - rb.position).normalized;
        rb.linearVelocity = direccion * velocidad;
    }


    void Atacar()
    {
        Debug.Log("El enemigo pesado ataca al jugador!");

        // Buscar componente de salud en el jugador
        VidaPlayer saludJugador = target.GetComponent<VidaPlayer>();
        if (saludJugador != null)
        {
            saludJugador.RecibirDaño(daño);
        }
    }

    public void RecibeImpactoRetroceso(int cantidadImpacto, Vector2 origenImpacto)
    {
        vida -= cantidadImpacto;

        // Calculamos la dirección opuesta al impacto
        Vector2 direccionRetroceso = ((Vector2)rb.position - origenImpacto).normalized;

        // Teletransportar un poco hacia atrás
        rb.position += direccionRetroceso * distanciaRetroceso;

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}
