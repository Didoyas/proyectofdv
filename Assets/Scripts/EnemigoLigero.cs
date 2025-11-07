using UnityEngine;

public class EnemigoLigero : MonoBehaviour, IRecibeImpactoRetroceso
{
    private Rigidbody2D rb;
    public float velocidad = 28f;          // Velocidad lenta
    public float rangoDeteccion = 7f;    // Distancia máxima para detectar al jugador

    public float rangoAtaque = 1f;        // Distancia de ataque cuerpo a cuerpo
    public int vida = 2;
    public int daño = 1;                 // Daño al jugador
    public float tiempoEntreAtaques = 0.5f; // Enfriamiento entre ataques
    public float tiempoCargaAtaque = 0f; // Tiempo antes de golpear

    private Transform target;           // Referencia al jugador
    private float tiempoUltimoAtaque = 0f;

    private Vector2 posicionInicial; // Posición donde empezó el enemigo

    private bool puedeMoverse = true;      // Flag para controlar movimiento
    private bool estaAtacando = false;  // Previene múltiples ataques solapados

    public float fuerzaRetroceso = 400f;   // reemplaza distanciaRetroceso
    public float tiempoStun = 0f;      // tiempo que queda inmóvil tras recibir golpe
    private GameObject monedaPrefab;

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
          if (target == null || !puedeMoverse) return;

        float distancia = Vector2.Distance(transform.position, target.position);

        if (distancia <= rangoDeteccion)
        {
            MoverHacia(target.position);

            if (distancia <= rangoAtaque && Time.time >= tiempoUltimoAtaque + tiempoEntreAtaques && !estaAtacando)
            {
                StartCoroutine(AtacarConRetraso());
            }
        }
        else
        {
            RegresarAlInicio();
        }
    }

     void MoverHacia(Vector2 destino)
    {
        Vector2 direccion = (destino - rb.position).normalized;
        Vector2 nuevaPos = rb.position + direccion * velocidad * Time.deltaTime;
        rb.MovePosition(nuevaPos);
    }
   

    void RegresarAlInicio()
    {
        float distanciaAlInicio = Vector2.Distance(rb.position, posicionInicial);
        if (distanciaAlInicio > 0.05f)
        {
            MoverHacia(posicionInicial);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private System.Collections.IEnumerator AtacarConRetraso()
    {
                estaAtacando = true;
        puedeMoverse = false;
        rb.linearVelocity = Vector2.zero;

        
        yield return new WaitForSeconds(tiempoCargaAtaque);

        float distanciaActual = Vector2.Distance(transform.position, target.position);
        if (distanciaActual <= rangoAtaque)
        {
            Atacar();
            tiempoUltimoAtaque = Time.time;
        }
        else
        {
            Debug.Log("Jugador fuera de rango al terminar la carga, se cancela el ataque.");
        }

        yield return new WaitForSeconds(tiempoEntreAtaques);

        puedeMoverse = true;
        estaAtacando = false;
    }

    void Atacar()
    {
        // Sonido ataque
        Debug.Log("El enemigo pesado ataca al jugador");

        // Buscar componente de salud en el jugador
        VidaPlayer vidaActual = target.GetComponent<VidaPlayer>();
        if (vidaActual != null)
        {
            vidaActual.RecibirDaño(daño);
        }
    }


    public void RecibeImpactoRetroceso(int cantidadImpacto, Vector2 origenImpacto)
    {
        vida -= cantidadImpacto;

        Vector2 direccionRetroceso = ((Vector2)rb.position - origenImpacto).normalized;
        rb.AddForce(direccionRetroceso * fuerzaRetroceso, ForceMode2D.Impulse);

        StartCoroutine(StunTemporal());

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Un Stun
    private System.Collections.IEnumerator StunTemporal()
    {
        puedeMoverse = false;
        yield return new WaitForSeconds(tiempoStun);
        puedeMoverse = true;
    }

    void OnDestroy()
    {
        // Evita crear monedas si la escena se está cerrando o recargando
        if (gameObject.scene.isLoaded)
        {
            Vector3 origin = transform.position;
            GameObject moneda = Instantiate(monedaPrefab, origin, Quaternion.identity);
        }
    }
}
