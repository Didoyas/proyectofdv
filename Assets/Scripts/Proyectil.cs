using UnityEngine;

public class Proyectil : MonoBehaviour
{

    public float speed = 10f; // velocidad base del proyectil
	public float maxLifeTime = 3f; // tiempo de vida del proyectil 
	public Vector2 targetVector; 

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
        if (other.CompareTag("Tilemap") || other.CompareTag("Enemigo") )
        {
            Destroy(gameObject); // destruccion si toca pared
        }
    }
}
