using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas

public class Instrucciones : MonoBehaviour
{
    public GameObject panelInstrucciones;

    void Start()
    {
        // 💡 Obtenemos la escena actual
        Scene escenaActual = SceneManager.GetActiveScene();
        
        // **Reemplaza "NombreDeTuEscenaDeInstrucciones" con el nombre real de tu escena de inicio/juego**
        // Solo mostramos las instrucciones si estamos en la escena correcta
        if (escenaActual.name == "NombreDeTuEscenaDeInstrucciones") 
        {
            if (panelInstrucciones != null)
            {
                panelInstrucciones.SetActive(true);
            }

            Time.timeScale = 0f;
            PlayerDisparo.puedeDisparar = false;
        }
        else
        {
            // Si no es la escena de instrucciones, aseguramos que el juego continúe con Time.timeScale normal
            Time.timeScale = 1f;
            PlayerDisparo.puedeDisparar = true;
            
            // Opcional: Desactivar el componente Instrucciones para que no se ejecute OcultarPanel por error
            this.enabled = false; 
        }
    }
    
    // ... El resto de tu código (OcultarPanel y ResumeGame) sigue igual.
    public void OcultarPanel()
    {
        StartCoroutine(ResumeGame());
    }

    IEnumerator ResumeGame()
    {
        if (panelInstrucciones != null){
            panelInstrucciones.SetActive(false);}

        Time.timeScale = 1f;
        yield return null;
        PlayerDisparo.puedeDisparar = true;
    }
}