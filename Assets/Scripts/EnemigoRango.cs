using UnityEngine;

public class EnemigoRango : MonoBehaviour, IRecibeImpactoRetroceso
{

    // Atributos
    public int vida = 1;

    public int daño = 1;                 // Daño al jugador


    // Mvement
    public float velocidad = 2f;
    public float rangoAtaque = 7f;
    public float distanciaMinima = 4f; // Si está más cerca, se aleja un poco
    public float movimientoLateralIntensidad = 0.5f;
    public float movimientoLateralFrecuencia = 2f;

    // Shooting
    [SerializeField]
    private GameObject proyectilPrefab;
    [SerializeField]
    private float fireCooldown = 0.5f;
    [SerializeField]
    private float proyectilSpeed = 7f;

    // Modificacion
    private float fireCooldownTimer = 0f;
    private Transform target;
    private Rigidbody2D rb;
    private float tiempoLateral = 0f;

    public float fuerzaRetroceso = 0.2f;   // reemplaza distanciaRetroceso

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

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
        if (fireCooldownTimer > 0f)
        {
            fireCooldownTimer -= Time.deltaTime;
        }

        Movimiento(); // <-- aquí añadimos el movimiento

        if (fireCooldownTimer <= 0f && HasLineOfSight())
        {
            ShootAtTarget();
            fireCooldownTimer = fireCooldown;
        }
    }

    // Añado mvment
    private void Movimiento()
    {
        if (!target) return;

        Vector2 posicion = rb.position;
        Vector2 objetivo = target.position;
        Vector2 direccion = (objetivo - posicion).normalized;
        float distancia = Vector2.Distance(posicion, objetivo);

        Vector2 destino = Vector2.zero;

       /* if (distancia > rangoAtaque)
        {
            destino = direccion; // Se acerca
        }
        else*/ if (distancia < distanciaMinima)
        {
            destino = -direccion; // Se aleja
        }
        else
        {
            // Diagonal
            tiempoLateral += Time.deltaTime * movimientoLateralFrecuencia;
            Vector2 perpendicular = new Vector2(-direccion.y, direccion.x);
            destino = perpendicular * Mathf.Sin(tiempoLateral) * movimientoLateralIntensidad;
        }

        rb.linearVelocity = destino.normalized * velocidad;
    }


    private bool HasLineOfSight()
    {
        Vector2 a = transform.position;
        Vector2 b = target.position;
        if ((b - a).sqrMagnitude > rangoAtaque * rangoAtaque)
            return false;

        var filter = new ContactFilter2D { useLayerMask = false, useTriggers = false };
        var hits = new RaycastHit2D[8];
        int count = Physics2D.Linecast(a, b, filter, hits);

        Collider2D closest = null;
        float closestDist = float.PositiveInfinity;
        for (int i = 0; i < count; i++)
        {
            var h = hits[i];
            if (!h.collider) continue;
            if (h.collider.transform == transform) continue;
            if (h.distance < closestDist)
            {
                closestDist = h.distance;
                closest = h.collider;
            }
        }

        if (closest == null) return true;
        return closest.CompareTag("Player");
    }

    private void ShootAtTarget()
    {
        Vector3 origin = transform.position;
        GameObject proyectil = Instantiate(proyectilPrefab, origin, Quaternion.identity);
        Vector2 direction = ((Vector2)(target.position - origin)).normalized;
        Proyectil proyectilScript = proyectil.GetComponent<Proyectil>();
        proyectilScript.targetVector = direction;
        proyectilScript.ignoreTags = new string[] { "Enemigo", "Proyectil" }; // ignorar colisiones con enemigos o otros proyectiles
        proyectilScript.speed = proyectilSpeed;

        proyectilScript.esDeEnemigo = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VidaPlayer vidaActual = other.GetComponent<VidaPlayer>();

            if (vidaActual != null)
            {
                vidaActual.RecibirDaño(daño);
            }
        }

    }


    // Ya lo preañado por si hacemos que tenga más vida
    public void RecibeImpactoRetroceso(int cantidadImpacto, Vector2 origenImpacto)
    {

        if (!target) return;

        //Si el enemigo esta feura del rango del jugador no recibe daño (evitar balas perdidas maten enemigos)
        float distanciaAlJugador = Vector2.Distance(transform.position, target.position);
        if (distanciaAlJugador > rangoAtaque)
        {
            return;
        }

        vida -= cantidadImpacto;

        Vector2 direccionRetroceso = ((Vector2)rb.position - origenImpacto).normalized;
        rb.AddForce(direccionRetroceso * fuerzaRetroceso, ForceMode2D.Impulse);

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}

