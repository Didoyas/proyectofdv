using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalaGestor : MonoBehaviour
{
    public GameObject balaPrefab;   // IMAGEN de la bala
    public Transform contenedor;    // Layout Group
    
    public Sprite balaLlena;       
    public Sprite balaVacia;  

    // Lista para guardar las referencias a las imágenes creadas y no buscarlas todo el rato
    private List<Image> balasImages = new List<Image>();

    // CREA los huecos
    public void DibujarCasillas(int maxCargas)
    {
        foreach (Transform child in contenedor)
        {
            Destroy(child.gameObject);
        }
        balasImages.Clear();

        for (int i = 0; i < maxCargas; i++)
        {
            GameObject nuevaBala = Instantiate(balaPrefab, contenedor);
            balasImages.Add(nuevaBala.GetComponent<Image>());
        }
    }

    // CAMBIA COLORES 
    public void ActualizarBalas(int cargasActuales)
    {
        for (int i = 0; i < balasImages.Count; i++)
        {
            if (i < cargasActuales)
            {
                balasImages[i].sprite = balaLlena; // Bala disponible
            }
            else
            {
                balasImages[i].sprite = balaVacia; 
            }
        }
    }
}
