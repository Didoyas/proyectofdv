using UnityEngine;

public class EnemigoBoss2 : MonoBehaviour, IRecibeImpactoRetroceso
{
    public enum TipoAtaqueBoss { Circulo360, Rush }

    public int vida = 5;

    public int daño = 1;
    public float velocidad = 1.5f;
    public float rangoAtaque = 7f;
    public float distanciaMinima = 4f;
    public float movimientoLateralIntensidad = 0.5f;
    public float movimientoLateralFrecuencia = 2f;

    [Header("Ataque en Arco")]
    public int balasPorDisparo = 5;
    [Range(0, 360)] public float anguloArco = 90f;
    public float cooldownArco = 2f;
    public float velocidadArco = 3f;

    [Header("Ataque 360")]
    public int balas360 = 20;
    public float cooldownAtaqueFuerte = 15f;
    public float velocidad360 = 4f;
    private float temporizadorAtaqueFuerte = 0f;

    [Header("Ataque Rush")]
    public float fuerzaRush = 20f;
    public int dañoRush = 2;

    private bool cargandoAtaqueFuerte = false;
    private float tiempoCargaRestante = 0f;
    private float duracionCarga = 1f;
    private Vector2 posicionBaseVibracion;
    private TipoAtaqueBoss ataqueFuerteSeleccionado;
    private TipoAtaqueBoss ultimoAtaqueFuerte = TipoAtaqueBoss.Rush;

    private bool haciendoRush = false;
    private int contadorRush = 0;
    private int maxRushContador = 3;

    private bool disparandoOleadas = false;
    private int contadorOleadas = 0;
    private int maxOleadas = 5;
    private float temporizadorOleada = 0f;
    private float intervaloOleada = 0.5f;

    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private GameObject monedaPrefab;
    [SerializeField] private float cooldownDisparo = 0.5f;
    [SerializeField] private float velocidadProyectil = 5f;

    private float temporizadorCooldownDisparo = 0f;
    private Transform objetivo;
    private Rigidbody2D rb;
    public float fuerzaRetroceso = 0.2f;

    void Awake()
    {
        objetivo = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        if (objetivo == null)
        {
            throw new System.Exception("Jugador no encontrado");
        }
    }

    void Start()
    {
        temporizadorAtaqueFuerte = cooldownAtaqueFuerte;
    }

    void Update()
    {
        if (objetivo == null) return;

        if (cargandoAtaqueFuerte)
        {
            ProcesarVibracion();
            return;
        }

        if (haciendoRush)
        {
            if (rb.linearVelocity.magnitude < 0.5f)
            {
                AlTerminarRush();
            }
            return;
        }

        if (disparandoOleadas)
        {
            temporizadorOleada -= Time.deltaTime;
            if (temporizadorOleada <= 0f)
            {
                float pasoAngulo = 360f / balas360;
                float desfase = (contadorOleadas % 2 == 0) ? 0f : pasoAngulo / 2f;

                DispararEnArco(transform.position, Vector2.right, balas360, 360f, velocidad360, desfase);
                contadorOleadas++;
                temporizadorOleada = intervaloOleada;

                if (contadorOleadas >= maxOleadas)
                {
                    TerminarSecuenciaAtaqueFuerte();
                }
            }
            return;
        }

        if (temporizadorCooldownDisparo > 0f) temporizadorCooldownDisparo -= Time.deltaTime;
        if (temporizadorAtaqueFuerte > 0f) temporizadorAtaqueFuerte -= Time.deltaTime;

        Movimiento();

        if (TieneLineaDeVision() && temporizadorCooldownDisparo <= 0f)
        {
            if (temporizadorAtaqueFuerte <= 0f)
            {
                contadorRush = 0;
                ComenzarCargaAtaqueFuerte();
            }
            else
            {
                Vector2 direccionJugador = (objetivo.position - transform.position).normalized;
                DispararEnArco(transform.position, direccionJugador, balasPorDisparo, anguloArco, velocidadArco);
                temporizadorCooldownDisparo = cooldownArco;
            }
        }
    }

    private void ComenzarCargaAtaqueFuerte()
    {
        cargandoAtaqueFuerte = true;
        tiempoCargaRestante = duracionCarga;
        posicionBaseVibracion = transform.position;
        rb.linearVelocity = Vector2.zero;

        if (contadorRush > 0 && contadorRush < maxRushContador)
        {
            ataqueFuerteSeleccionado = TipoAtaqueBoss.Rush;
        }
        else
        {
            if (ultimoAtaqueFuerte == TipoAtaqueBoss.Rush)
                ataqueFuerteSeleccionado = TipoAtaqueBoss.Circulo360;
            else
                ataqueFuerteSeleccionado = TipoAtaqueBoss.Rush;

            ultimoAtaqueFuerte = ataqueFuerteSeleccionado;
        }
    }

    private void ProcesarVibracion()
    {
        tiempoCargaRestante -= Time.deltaTime;

        float intensidad = 0.2f;
        Vector2 offset = Random.insideUnitCircle * intensidad;
        transform.position = posicionBaseVibracion + offset;

        if (tiempoCargaRestante <= 0f)
        {
            transform.position = posicionBaseVibracion;
            cargandoAtaqueFuerte = false;

            EjecutarAtaqueFuerte();
        }
    }

    private void EjecutarAtaqueFuerte()
    {
        switch (ataqueFuerteSeleccionado)
        {
            case TipoAtaqueBoss.Circulo360:
                disparandoOleadas = true;
                contadorOleadas = 0;
                temporizadorOleada = 0f;
                break;

            case TipoAtaqueBoss.Rush:
                if (objetivo != null)
                {
                    haciendoRush = true;
                    Vector2 dir = (objetivo.position - transform.position).normalized;
                    rb.AddForce(dir * fuerzaRush, ForceMode2D.Impulse);
                }
                else
                {
                    TerminarSecuenciaAtaqueFuerte();
                }
                break;
        }
    }

    private void AlTerminarRush()
    {
        haciendoRush = false;
        contadorRush++;

        if (contadorRush < maxRushContador)
        {
            ComenzarCargaAtaqueFuerte();
        }
        else
        {
            TerminarSecuenciaAtaqueFuerte();
        }
    }

    private void TerminarSecuenciaAtaqueFuerte()
    {
        temporizadorAtaqueFuerte = cooldownAtaqueFuerte;
        temporizadorCooldownDisparo = 1f;
        contadorRush = 0;
        haciendoRush = false;
        cargandoAtaqueFuerte = false;
        disparandoOleadas = false;
    }

    private void Movimiento()
    {
        if (!objetivo) return;

        if (cargandoAtaqueFuerte || haciendoRush || disparandoOleadas) return;

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        if (distancia > rangoAtaque)
        {
            Vector2 direccion = (objetivo.position - transform.position).normalized;
            rb.linearVelocity = direccion * velocidad;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void DispararEnArco(Vector2 origen, Vector2 direccionCentral, int cantidadBalas, float anguloTotal, float velocidad, float desfaseAngulo = 0f)
    {
        if (cantidadBalas <= 0) return;

        if (cantidadBalas == 1)
        {
            CrearProyectil(origen, direccionCentral, velocidad);
            return;
        }

        float anguloInicial = Mathf.Atan2(direccionCentral.y, direccionCentral.x) * Mathf.Rad2Deg;
        float mitadArco = anguloTotal / 2f;
        float anguloPaso = anguloTotal / (cantidadBalas - 1);

        if (Mathf.Abs(anguloTotal - 360f) < 0.1f)
        {
            anguloPaso = anguloTotal / cantidadBalas;
        }

        float anguloActual = anguloInicial - mitadArco + desfaseAngulo;

        for (int i = 0; i < cantidadBalas; i++)
        {
            float rad = anguloActual * Mathf.Deg2Rad;
            Vector2 dirBala = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            CrearProyectil(origen, dirBala, velocidad);

            anguloActual += anguloPaso;
        }
    }

    private void CrearProyectil(Vector2 pos, Vector2 dir, float velocidad)
    {
        GameObject p = Instantiate(proyectilPrefab, pos, Quaternion.identity);
        Proyectil script = p.GetComponent<Proyectil>();

        if (script != null)
        {
            script.targetVector = dir;
            script.speed = velocidad;
            script.esDeEnemigo = true;
            script.ignoreTags = new string[] { "Enemigo", "Proyectil", "Moneda", "Untagged", "Llave" };
        }
    }


    private bool TieneLineaDeVision()
    {
        Vector2 a = transform.position;
        Vector2 b = objetivo.position;
        if ((b - a).sqrMagnitude > rangoAtaque * rangoAtaque)
            return false;

        var filter = new ContactFilter2D { useLayerMask = false, useTriggers = false };
        var hits = new RaycastHit2D[8];
        int count = Physics2D.Linecast(a, b, filter, hits);

        Collider2D masCercano = null;
        float distanciaMasCercana = float.PositiveInfinity;
        for (int i = 0; i < count; i++)
        {
            var h = hits[i];
            if (!h.collider) continue;
            if (h.collider.transform == transform) continue;
            if (h.distance < distanciaMasCercana)
            {
                distanciaMasCercana = h.distance;
                masCercano = h.collider;
            }
        }

        if (masCercano == null) return true;
        return masCercano.CompareTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero;

            VidaPlayer vidaActual = other.GetComponent<VidaPlayer>();

            if (vidaActual != null)
            {
                vidaActual.RecibirDaño(daño);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Obstaculos") || other.gameObject.layer == LayerMask.NameToLayer("Paredes"))
        {
            rb.linearVelocity = Vector2.zero;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero;

            VidaPlayer vidaActual = collision.gameObject.GetComponent<VidaPlayer>();
            if (vidaActual != null)
            {
                vidaActual.RecibirDaño(daño);
            }
        }
        else if (!collision.gameObject.CompareTag("Proyectil"))
        {
            rb.linearVelocity = Vector2.zero;
        }
    }


    public void RecibeImpactoRetroceso(int cantidadImpacto, Vector2 origenImpacto)
    {

        if (!objetivo) return;

        float distanciaAlJugador = Vector2.Distance(transform.position, objetivo.position);
        if (distanciaAlJugador > rangoAtaque)
        {
            return;
        }

        vida -= cantidadImpacto;

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        {
            Vector3 origin = transform.position;
            GameObject moneda = Instantiate(monedaPrefab, origin, Quaternion.identity);
        }
    }
}
