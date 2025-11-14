using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena2 : MonoBehaviour
{
    public string nombreEscena;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"OnTriggerEnter2D: {gameObject.name} triggered by {other.gameObject.name}");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected — cargando escena: " + nombreEscena);
            if (!string.IsNullOrEmpty(nombreEscena))
                SceneManager.LoadScene(nombreEscena);
            else
                Debug.LogWarning("nombreEscena no está definido en el inspector.");
        }
    }
}