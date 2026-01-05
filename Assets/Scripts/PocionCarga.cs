using UnityEngine;

public class PocionCarga : MonoBehaviour
{
    public AudioClip sonidoRecoger;

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
                    AudioManager.PlaySFX(sonidoRecoger, transform.position);
                    Destroy(gameObject);
                }
            }
        }
    }
}