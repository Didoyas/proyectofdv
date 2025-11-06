using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool juegoIniciado = false;
    public GameObject panelInstrucciones;
    public GameObject panelMuerte;

    void Start()
    {
        if (!juegoIniciado && panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(true);
            Time.timeScale = 0f;
            PlayerDisparo.puedeDisparar = false;
        }
        else
        {
            if (panelInstrucciones != null)
                panelInstrucciones.SetActive(false);
        }

        if (panelMuerte != null)
            panelMuerte.SetActive(false);
    }

    public void OcultarInstrucciones()
    {
        juegoIniciado = true;
        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(false);

        Time.timeScale = 1f;
        PlayerDisparo.puedeDisparar = true;
    }

    public void MostrarMuerte()
    {
        if (panelMuerte != null)
            panelMuerte.SetActive(true);

        Time.timeScale = 0f;
        PlayerDisparo.puedeDisparar = false;
    }

    public void Reintentar()
    {
        juegoIniciado = true;

        Time.timeScale = 1f;
        SceneManager.LoadScene("Nivel1");
    }
}
