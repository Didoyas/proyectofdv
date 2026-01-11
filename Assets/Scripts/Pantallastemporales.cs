using UnityEngine;

public class Pantallastemporales : MonoBehaviour
{
    
    public GameObject canvasPantallaTemporal;
     private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!PanelAjustes.panelesActivos) return;

        if (collision.CompareTag("Player"))
        {
            // Activamos la pantalla mientras el jugador esté dentro
            canvasPantallaTemporal.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (!PanelAjustes.panelesActivos) return;

        if (collision.CompareTag("Player"))
        {
            // Desactivamos la pantalla al salir
            canvasPantallaTemporal.SetActive(false);
        }
    }

    void Update()
{
    if (!PanelAjustes.panelesActivos && canvasPantallaTemporal.activeSelf)
    {
        canvasPantallaTemporal.SetActive(false);
    }
}
}
