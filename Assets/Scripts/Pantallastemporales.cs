using UnityEngine;

public class Pantallastemporales : MonoBehaviour
{
    
    public GameObject canvasPantallaTemporal;

    
    public float duracionPantalla = 3f; 
    private bool yaActivado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!yaActivado && collision.CompareTag("Player"))
        {
            yaActivado = true;
            StartCoroutine(MostrarPantallaTemporal());
        }
    }

    private System.Collections.IEnumerator MostrarPantallaTemporal()
    {
        canvasPantallaTemporal.SetActive(true);

        yield return new WaitForSeconds(duracionPantalla);

        canvasPantallaTemporal.SetActive(false);
    }
}
