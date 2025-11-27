using UnityEngine;
using System.Collections;

public class EnemigoBoss: MonoBehaviour, IRecibeImpactoRetroceso
{
    [Header("Vida")]
    public int vidaMax = 60;
    public int vida;

    [Header("Movimiento")]
    public float velocidad = 4f;
    public float velocidadFuria = 7f;
    public float distanciaMinima = 4f;

    [Header("Melee")]
    public float rangoMelee = 1.6f;
    public float meleeCooldown = 1f;
    public int dañoMelee = 2;
    private float meleeTimer = 0f;

    [Header("Proyectiles")]
    public GameObject proyectilPrefab;
    public float proyectilSpeed = 6f;

    [Header("Disparo normal")]
    public float fireCooldown = 1f;
    private float fireTimer = 0f;

    [Header("Bullet Hell")]
    public float circleCooldown = 4f;
    private float circleTimer = 0f;

    public float coneCooldown = 3f;
    private float coneTimer = 0f;

    public float spiralCooldown = 0.2f;
    private float spiralTimer = 0f;
    private float spiralAngle = 0f;

    public float wallCooldown = 6f;
    private float wallTimer = 0f;

    [Header("Dash")]
    public float dashCooldown = 6f;
    public float dashForce = 900f;
    private float dashTimer = 0f;

    [Header("Drops")]
    public GameObject monedaPrefab;

    private Rigidbody2D rb;
    private Transform target;

    private float tiempoLateral = 0f;

    void Awake()
    {
        vida = vidaMax;
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (target == null)
            Debug.LogError("BossFinal: Jugador no encontrado (tag 'Player')");
        if (rb == null)
            Debug.LogError("BossFinal: Rigidbody2D no encontrado en el boss");
    }

    void Update()
    {
        if (!target || rb == null) return;

        // Timers
        fireTimer -= Time.deltaTime;
        meleeTimer -= Time.deltaTime;
        circleTimer -= Time.deltaTime;
        coneTimer -= Time.deltaTime;
        spiralTimer -= Time.deltaTime;
        wallTimer -= Time.deltaTime;
        dashTimer -= Time.deltaTime;

        float vidaRatio = (float)vida / vidaMax;
        float dist = Vector2.Distance(rb.position, (Vector2)target.position);

        if (vidaRatio > 0.66f)
        {
            Fase1(dist);
        }
        else if (vidaRatio > 0.33f)
        {
            Fase2(dist);
        }
        else
        {
            Fase3(dist);
        }
    }

    // -------------------------- FASE 1 -------------------------
    void Fase1(float dist)
    {
        Perseguir();

        if (dist <= rangoMelee && meleeTimer <= 0f)
            AtaqueMelee();

        if (fireTimer <= 0f)
        {
            DisparoSimple();
            fireTimer = fireCooldown;
        }
    }

    // -------------------------- FASE 2 --------------------------
    void Fase2(float dist)
    {
        MovimientoLateral();

        if (fireTimer <= 0f)
        {
            DisparoSimple();
            fireTimer = fireCooldown * 0.8f;
        }

        if (circleTimer <= 0f)
        {
            BulletCircle(20, proyectilSpeed * 0.9f);
            circleTimer = circleCooldown;
        }

        if (coneTimer <= 0f)
        {
            BulletCone(7, 90f, proyectilSpeed * 1.2f);
            coneTimer = coneCooldown;
        }
    }

    // -------------------------- FASE 3 --------------------------
    void Fase3(float dist)
    {
        MovimientoLateral(velocidadFuria);

        if (dashTimer <= 0f)
        {
            Dash();
            dashTimer = dashCooldown;
        }

        if (spiralTimer <= 0f)
        {
            BulletSpiral(4, proyectilSpeed, 10f);
            spiralTimer = spiralCooldown;
        }

        if (circleTimer <= 0f)
        {
            BulletCircle(26, proyectilSpeed * 1.2f);
            circleTimer = circleCooldown * 0.7f;
        }

        if (wallTimer <= 0f)
        {
            BulletWallHorizontal(16, proyectilSpeed);
            wallTimer = wallCooldown;
        }
    }

    // ---------------------- MOVIMIENTO ---------------------------
    void Perseguir()
    {
        if (rb == null || target == null) return;
        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.linearVelocity = dir * velocidad;
    }

    void MovimientoLateral(float vel = -1)
    {
        if (rb == null || target == null) return;
        if (vel < 0) vel = velocidad;

        Vector2 pos = rb.position;
        Vector2 dir = ((Vector2)target.position - pos).normalized; // <-- CORRECTO (cast)
        float dist = Vector2.Distance(pos, (Vector2)target.position);

        if (dist < distanciaMinima)
        {
            rb.linearVelocity = -dir * vel;
        }
        else
        {
            tiempoLateral += Time.deltaTime * 2f;
            Vector2 perpendicular = new Vector2(-dir.y, dir.x);
            rb.linearVelocity = perpendicular * Mathf.Sin(tiempoLateral) * vel;
        }
    }

    // ---------------------- ATAQUES ------------------------------
    void AtaqueMelee()
    {
        meleeTimer = meleeCooldown;

        if (Vector2.Distance(rb.position, (Vector2)target.position) > rangoMelee) return;

        VidaPlayer v = target.GetComponent<VidaPlayer>();
        if (v != null) v.RecibirDaño(dañoMelee);
    }

    void DisparoSimple()
    {
        DispararHacia(target.position);
    }

    void DispararHacia(Vector2 destino)
    {
        Vector2 dir = ((Vector2)destino - rb.position).normalized;

        GameObject p = Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
        Proyectil sc = p.GetComponent<Proyectil>();
        sc.targetVector = dir;
        sc.speed = proyectilSpeed;
        sc.esDeEnemigo = true;
        sc.ignoreTags = new string[] { "Enemigo", "Proyectil", "Moneda" };
    }

    // ---------------------- BULLET HELL --------------------------

    public void BulletCircle(int cantidad, float speed)
    {
        for (int i = 0; i < cantidad; i++)
        {
            float ang = (360f / cantidad) * i;
            Vector2 dir = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad));

            CrearBala(dir, speed);
        }
    }

    public void BulletSpiral(int balasPorVuelta, float speed, float incremento)
    {
        for (int i = 0; i < balasPorVuelta; i++)
        {
            float ang = spiralAngle + (360f / balasPorVuelta) * i;
            Vector2 dir = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad));

            CrearBala(dir, speed);
        }

        spiralAngle += incremento;
    }

    public void BulletCone(int cantidad, float spread, float speed)
    {
        Vector2 baseDir = ((Vector2)target.position - rb.position).normalized;
        float baseAng = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < cantidad; i++)
        {
            float offset = Mathf.Lerp(-spread / 2f, spread / 2f, i / (float)(cantidad - 1));
            float ang = (baseAng + offset) * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
            CrearBala(dir, speed);
        }
    }

    public void BulletWallHorizontal(int cantidad, float speed)
    {
        if (Camera.main == null) return;
        float xmin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float xmax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float y = transform.position.y;

        for (int i = 0; i < cantidad; i++)
        {
            float x = Mathf.Lerp(xmin, xmax, i / (float)(cantidad - 1));

            Vector3 pos = new Vector3(x, y, 0);
            CrearBalaDesde(pos, Vector2.down, speed);
        }
    }

    void CrearBala(Vector2 dir, float speed)
    {
        GameObject p = Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
        Proyectil sc = p.GetComponent<Proyectil>();
        sc.targetVector = dir;
        sc.speed = speed;
        sc.esDeEnemigo = true;
        sc.ignoreTags = new string[] { "Enemigo", "Proyectil", "Moneda" };
    }

    void CrearBalaDesde(Vector2 pos, Vector2 dir, float speed)
    {
        GameObject p = Instantiate(proyectilPrefab, pos, Quaternion.identity);
        Proyectil sc = p.GetComponent<Proyectil>();
        sc.targetVector = dir;
        sc.speed = speed;
        sc.esDeEnemigo = true;
        sc.ignoreTags = new string[] { "Enemigo", "Proyectil", "Moneda" };
    }

    // ---------------------- DASH -------------------------------
    void Dash()
    {
        if (rb == null || target == null) return;
        // cast correcto a Vector2 para evitar errores con Vector3
        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.AddForce(dir * dashForce, ForceMode2D.Impulse); // ForceMode2D correcto
    }

    // --------------------- RETROCESO / MUERTE ---------------------
    public void RecibeImpactoRetroceso(int dmg, Vector2 origen)
    {
        vida -= dmg;

        if (rb != null)
        {
            Vector2 dir = ((Vector2)transform.position - origen).normalized;
            rb.AddForce(dir * 600f, ForceMode2D.Impulse);
        }

        if (vida <= 0) Die();
    }

    void Die()
    {
        if (gameObject.scene.isLoaded && monedaPrefab != null)
            Instantiate(monedaPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
