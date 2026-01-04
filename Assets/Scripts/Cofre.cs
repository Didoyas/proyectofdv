using UnityEngine;

public class Cofre : MonoBehaviour
{
    [System.Serializable]
    public class Drop
    {
        public GameObject objeto;
        public float probabilidad;
    }

    public Drop[] posiblesDrops;
    public Transform[] puntosDeSalida; 
    public float distanciaParaAbrir = 2f;

    private bool abierto = false;
    private Transform jugador;

    [SerializeField] private Sprite spriteAbierto;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (abierto) return;

        float distancia = Vector3.Distance(jugador.position, transform.position);

        if (distancia <= distanciaParaAbrir && Input.GetKeyDown(KeyCode.E))
        {
            AbrirCofre();
        }
    }

    void AbrirCofre()
    {
        abierto = true;

        if (spriteRenderer != null && spriteAbierto != null)
        {
            spriteRenderer.sprite = spriteAbierto;
        }

        SoltarObjetos();
    }

    void SoltarObjetos()
    {
        if (puntosDeSalida == null || puntosDeSalida.Length == 0) return;

        foreach (Transform punto in puntosDeSalida)
        {
            Drop dropElegido = ElegirDrop(); 
            if (dropElegido != null)
            {
                Instantiate(dropElegido.objeto, punto.position, Quaternion.identity);
            }
        }
    }

    Drop ElegirDrop()
    {
        float totalProbabilidad = 0f;

        foreach (Drop drop in posiblesDrops)
        {
            totalProbabilidad += drop.probabilidad;
        }

        float valorAleatorio = Random.Range(0f, totalProbabilidad);
        float acumulado = 0f;

        foreach (Drop drop in posiblesDrops)
        {
            acumulado += drop.probabilidad;
            if (valorAleatorio <= acumulado)
            {
                return drop;
            }
        }

        return null;
    }
}