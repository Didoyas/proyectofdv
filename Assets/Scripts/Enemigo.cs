using UnityEngine;

public class Enemigo : MonoBehaviour, IRecibeImpacto
{

    public int vida = 1;

    public int daño = 1;                 // Daño al jugador

    [SerializeField]
    private GameObject proyectilPrefab;
    [SerializeField]
    private float fireCooldown = 1f;
    [SerializeField]
    private float proyectilSpeed = 5f;
    private float fireCooldownTimer = 0f;
    [SerializeField]
    private float sightRange = 15f;
    private Transform target;

    void Awake()
    {
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
        if (fireCooldownTimer > 0f) fireCooldownTimer -= Time.deltaTime;

        if (fireCooldownTimer <= 0f && HasLineOfSight())
        {
            ShootAtTarget();
            fireCooldownTimer = fireCooldown;
        }
    }

    public void RecibeImpacto(int cantidadImpacto)
    {
        vida -= cantidadImpacto;
        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    private bool HasLineOfSight()
    {
        Vector2 a = transform.position;
        Vector2 b = target.position;
        // Check whether the player is within range
        if ((b - a).sqrMagnitude > sightRange * sightRange) return false;

        var filter = new ContactFilter2D { useLayerMask = false, useTriggers = false };
        var hits = new RaycastHit2D[8];
        int count = Physics2D.Linecast(a, b, filter, hits);

        //Obtain the closest object to the enemy.
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

        //The player is within range, and nothing is in the way.
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
        proyectilScript.ignoreTags = new string[] { "Enemigo" }; // Ignore collisions with enemies
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

}

