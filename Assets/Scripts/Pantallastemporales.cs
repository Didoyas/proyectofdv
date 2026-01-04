using UnityEngine;

public class Pantallastemporales : MonoBehaviour
{
    
    public GameObject canvasPantallaTemporal;

    
    public float duracionPantalla = 3f; 
     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Activamos la pantalla mientras el jugador esté dentro
            canvasPantallaTemporal.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Desactivamos la pantalla al salir
            canvasPantallaTemporal.SetActive(false);
        }
    }
}
