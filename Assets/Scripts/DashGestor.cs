using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashGestor : MonoBehaviour
{
    public Image panelDashUI;    // Layout Group
    public Sprite dashParaUsar, imagenDashEnUso; // Imagen Dash


    // CAMBIA IMAGEN
    public void ActualizarImagen(bool dashActivo)
    {
        if (panelDashUI == null) return;

        if (dashActivo)
        {
            panelDashUI.sprite = imagenDashEnUso;
        }
        else
        {
            panelDashUI.sprite = dashParaUsar;
        }
        
    }


}
