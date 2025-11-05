/*using UnityEngine;

public class Proyectil : MonoBehaviour
{

    public float speed = 10f; // velocidad base del proyectil
    public float maxLifeTime = 3f; // tiempo de vida del proyectil 

    public int daño = 1;                 // Daño al jugador
    public Vector2 targetVector;
    [SerializeField]
    private string[] _ignoreTags = new string[0]; // Tags a ignorar por el collider del proyectil
    public string[] ignoreTags
    {
        get { return _ignoreTags; }
        set { _ignoreTags = value ?? new string[0]; }
    }
    public bool esDeJugador = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, maxLifeTime); //el proyectil se destruye pasado su tiempo de vida 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * targetVector * Time.deltaTime); //movimiento de el proyectil 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobar si la colision se debería ignorar
        for (int i = 0; i < ignoreTags.Length; i++)
        {
            if (!string.IsNullOrEmpty(ignoreTags[i]) && other.CompareTag(ignoreTags[i])) return;
        }

        IRecibeImpactoRetroceso recibeImpactoRetroceso = other.GetComponent<IRecibeImpactoRetroceso>();
        if (recibeImpactoRetroceso != null)
        {
            recibeImpactoRetroceso.RecibeImpactoRetroceso(1, transform.position);
            Destroy(gameObject);
            return;
        }

        // Aplicar impacto al objeto que colisiona
        IRecibeImpacto recibeImpacto = other.GetComponent<IRecibeImpacto>();
        if (recibeImpacto != null)
        {
            recibeImpacto.RecibeImpacto(1);
        }

        if (other.CompareTag("Player"))
        {
            VidaPlayer vidaActual = other.GetComponent<VidaPlayer>();

            if (vidaActual != null)
            {
                vidaActual.RecibirDaño(daño);
            }
        }

        Destroy(gameObject);
    }
    
}*/


using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 targetVector;
    public string[] ignoreTags;
    public bool esDeEnemigo = false; // 🔹 para marcar si la bala es enemiga

    private void Update()
    {
        transform.Translate(targetVector * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    foreach (string tag in ignoreTags)
    {
        if (other.CompareTag(tag))
            return;
    }

    if (esDeEnemigo && other.CompareTag("Player"))
        {
            VidaPlayer vida = other.GetComponent<VidaPlayer>();
            if (vida != null)
            {
                vida.RecibirDaño(1);
            }
            Destroy(gameObject);
            return;
        }

    if (!esDeEnemigo && (other.CompareTag("Enemigo") || other.CompareTag("Caja")))
        {
        
        IRecibeImpactoRetroceso recibeImpactoRetroceso = other.GetComponent<IRecibeImpactoRetroceso>();
        if (recibeImpactoRetroceso != null)
        {
            recibeImpactoRetroceso.RecibeImpactoRetroceso(1, transform.position);
            Destroy(gameObject);
            return;
        }

        IRecibeImpacto recibeImpacto = other.GetComponent<IRecibeImpacto>();
        if (recibeImpacto != null)
        {
            recibeImpacto.RecibeImpacto(1);
        }
        Destroy(gameObject);
        return;
    }
    Destroy(gameObject);
}
}
