using UnityEngine;

public class PantallaFinal : MonoBehaviour
{
    public GameObject canvasPantallaFinal;    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvasPantallaFinal.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
