using UnityEngine;

public class Enemigo : MonoBehaviour
{

    public int vida = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
     private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Proyectil"))
        {
            // Destruye el proyectil
            Destroy(other.gameObject);

            // Resta vida o destruye directamente
            vida--;

            if (vida <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
