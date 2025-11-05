using UnityEngine;

public class Caja : MonoBehaviour, IRecibeImpactoRetroceso
{

    public int vida = 1;
    private Rigidbody2D rb;
    public float fuerzaRetroceso = 0.2f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        // Bloquear rotación en eje Z
        rb.freezeRotation = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecibeImpactoRetroceso(int cantidadImpacto, Vector2 origenImpacto)
    {
        vida -= cantidadImpacto;

        Vector2 direccionRetroceso = ((Vector2)rb.position - origenImpacto).normalized;
        rb.AddForce(direccionRetroceso * fuerzaRetroceso, ForceMode2D.Impulse);

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}
