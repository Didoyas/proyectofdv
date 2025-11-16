using UnityEngine;

public class PocionCarga : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario inventario = other.GetComponent<Inventario>();

            if (inventario != null)
            {
                bool recogido = inventario.AgregarObjeto(Inventario.TipoObjeto.PocionCarga);
                
                if (recogido)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}