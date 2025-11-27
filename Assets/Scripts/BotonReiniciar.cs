using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonReiniciar : MonoBehaviour
{
    // Función que se llama al presionar el botón de Reiniciar
    public void ReiniciarJuego()
    {

        GameObject[] monedas = GameObject.FindGameObjectsWithTag("Moneda");

        foreach (GameObject moneda in monedas)
        {
            Destroy(moneda);
        }

        // 1. Asegúrate de que el tiempo vuelva a ser normal
        Time.timeScale = 1f;
        // reiniciamos score (monedas)
        ScoreManager.instance.score = 0;

        // 2. Obtén el índice de la escena actual y cárgala de nuevo
        // Esto reiniciará todo el nivel
        int indiceEscenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(indiceEscenaActual);

        // Si tienes una escena de "Instrucciones" con un índice fijo (ej: 0) y el juego es el índice 1:
        // SceneManager.LoadScene(1);
    }
}