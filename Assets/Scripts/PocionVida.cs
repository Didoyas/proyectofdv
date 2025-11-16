using UnityEngine;

public class PocionVida : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos si Player lo toca
        if (other.CompareTag("Player"))
        {
            // Intentamos obtener el componente Inventario del jugador
            Inventario inventario = other.GetComponent<Inventario>();

            if (inventario != null)
            {   
                bool recogido = inventario.AgregarObjeto(Inventario.TipoObjeto.PocionVida);
                
                if (recogido)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}