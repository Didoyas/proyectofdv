using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambioEscena : MonoBehaviour
{
    public string nombreEscena;
    public GameObject panelConfirmacion; // panel con botones
    public float cooldown = 5f;

    private bool puedeActivar = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!puedeActivar) return;

        if (other.CompareTag("Player") && !string.IsNullOrEmpty(nombreEscena))
        {
            panelConfirmacion.SetActive(true);
        }
    }

    public void Cambiar()
    {
        if (!puedeActivar) return;

        if (!string.IsNullOrEmpty(nombreEscena))
        {
            panelConfirmacion.SetActive(true);
        }
    }

    // Llamado desde botón "Aceptar"
    public void Aceptar()
    {
        panelConfirmacion.SetActive(false);
        if (!string.IsNullOrEmpty(nombreEscena))
            SceneManager.LoadScene(nombreEscena);
    }

    // Llamado desde botón "Cancelar"
    public void Cancelar()
    {
        panelConfirmacion.SetActive(false);
        StartCoroutine(CooldownCoroutine());
    }

    public void CancelarSinCoroutine()
    {
        panelConfirmacion.SetActive(false);
    }

    private IEnumerator CooldownCoroutine()
    {
        puedeActivar = false;
        yield return new WaitForSeconds(cooldown);
        puedeActivar = true;
    }
}
