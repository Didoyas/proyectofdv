using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject panelPausa;
    public static bool juegoPausado = false;
    public PlayerInput playerInput;

    public void TogglePause()
    {
        if (juegoPausado) Reanudar();
        else Pausar();
    }

    public void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;

        // BLOQUEA INPUT
        if (playerInput != null)
            playerInput.DeactivateInput();
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;

        // ACTIVA INPUT
        if (playerInput != null)
            playerInput.ActivateInput();
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
    }
}
