using UnityEngine;

public class Orbes : MonoBehaviour
{
    
    public int vida = 3;

    
    public float amplitud = 0.5f;   
    public float velocidad = 2f;    

    private Vector3 posicionInicial;
    private float desfase;

    private EnemigoBoss2 boss;

    
    private int danoPorProyectil = 1;  

    void Start()
    {
        posicionInicial = transform.position;
        desfase = Random.Range(0f, Mathf.PI * 2f); 

        boss = FindObjectOfType<EnemigoBoss2>();
        if (boss != null)
        {
            boss.RegistrarOrbe();
        }
    }

    void Update()
    {
        MovimientoVertical();
    }

    void MovimientoVertical()
    {
        float offsetY = Mathf.Sin(Time.time * velocidad + desfase) * amplitud;
        transform.position = posicionInicial + Vector3.up * offsetY;
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }
        else if (!other.CompareTag("Proyectil"))
        {
            return;
        }

        Proyectil p = other.GetComponent<Proyectil>();
        if (p != null && !p.esDeEnemigo)
        {
            RecibirDano(danoPorProyectil);
            Destroy(other.gameObject); 
        }
    }

    public void RecibirDano(int dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            DestruirOrbe();
        }
    }

    void DestruirOrbe()
    {
        if (boss != null)
        {
            boss.OrbeDestruido();
        }

        Destroy(gameObject);
    }
}
