using UnityEngine;

public class EnemigoLigero : MonoBehaviour, IRecibeImpactoRetroceso
{
    private Rigidbody2D rb;

    [Header("Movimiento")]
    public float velocidad = 3f;
    public float rangoDeteccion = 7f;
    public float rangoAtaque = 1f;

    [Header("Combate")]
    public int vida = 2;
    public int daño = 1;
    public float tiempoEntreAtaques = 0.5f;
    public float tiempoCargaAtaque = 0f;

    [Header("Retroceso")]
    public float fuerzaRetroceso = 400f;
    public float tiempoStun = 0f;

    public GameObject monedaPrefab;

    private Transform target;
    private Vector2 posicionInicial;

    private bool puedeMoverse = true;
    private bool estaAtacando = false;
    private float tiempoUltimoAtaque = 0f;

    // Movimiento controlado
    private Vector2 direccionMovimiento;
    private bool debeMoverse = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        posicionInicial = rb.position;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null)
        {
            Debug.LogError("No se encontró el jugador");
        }
    }

    void Update()
    {
        if (target == null || !puedeMoverse)
        {
            debeMoverse = false;
            return;
        }

        float distancia = Vector2.Distance(rb.position, target.position);

        if (distancia <= rangoDeteccion)
        {
            direccionMovimiento = (target.position - (Vector3)rb.position).normalized;
            debeMoverse = true;

            if (distancia <= rangoAtaque &&
                Time.time >= tiempoUltimoAtaque + tiempoEntreAtaques &&
                !estaAtacando)
            {
                StartCoroutine(AtacarConRetraso());
            }
        }
        else
        {
            Vector2 dirInicio = posicionInicial - rb.position;

            if (dirInicio.magnitude > 0.05f)
            {
                direccionMovimiento = dirInicio.normalized;
                debeMoverse = true;
            }
            else
            {
                debeMoverse = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!debeMoverse) return;

        Vector2 nuevaPos = rb.position + direccionMovimiento * velocidad * Time.fixedDeltaTime;
        rb.MovePosition(nuevaPos);
    }

    private System.Collections.IEnumerator AtacarConRetraso()
    {
        estaAtacando = true;
        puedeMoverse = false;
        debeMoverse = false;

        yield return new WaitForSeconds(tiempoCargaAtaque);

        float distanciaActual = Vector2.Distance(rb.position, target.position);
        if (distanciaActual <= rangoAtaque)
        {
            Atacar();
            tiempoUltimoAtaque = Time.time;
        }

        yield return new WaitForSeconds(tiempoEntreAtaques);

        puedeMoverse = true;
        estaAtacando = false;
    }

    void Atacar()
    {
        VidaPlayer vidaPlayer = target.GetComponent<VidaPlayer>();
        if (vidaPlayer != null)
        {
            vidaPlayer.RecibirDaño(daño);
        }
    }

    public void RecibeImpactoRetroceso(int cantidadImpacto, Vector2 origenImpacto)
    {
        vida -= cantidadImpacto;

        Vector2 direccionRetroceso = (rb.position - origenImpacto).normalized;
        rb.AddForce(direccionRetroceso * fuerzaRetroceso, ForceMode2D.Impulse);

        StartCoroutine(StunTemporal());

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator StunTemporal()
    {
        puedeMoverse = false;
        debeMoverse = false;

        yield return new WaitForSeconds(tiempoStun);

        puedeMoverse = true;
    }

    void OnDestroy()
    {
        if (gameObject.scene.isLoaded && monedaPrefab != null)
        {
            Instantiate(monedaPrefab, transform.position, Quaternion.identity);
        }
    }
}