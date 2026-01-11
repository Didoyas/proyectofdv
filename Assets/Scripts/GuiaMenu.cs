using UnityEngine;

public class GuiaMenu : MonoBehaviour
{
    public GameObject AjustesTutorial;

    public void BotonSi()
    {
        PanelAjustes.panelesActivos = true;
        AjustesTutorial.SetActive(false);
    }

    public void BotonNo()
    {
        PanelAjustes.panelesActivos = false;
        AjustesTutorial.SetActive(false);
    }
}
