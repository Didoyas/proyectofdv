using UnityEngine;

public class PuertaFinal : MonoBehaviour
{

    public GameObject objetoAVigilar; // objeto cuya destrucción activa la puerta

    void Update()
    {
        if (objetoAVigilar == null) // Si el objeto ya fue destruido
        {
            DestruirPuerta();
        }
    }

    void DestruirPuerta()
    {
        
        Destroy(gameObject); // Destruye la puerta
        
    }
}
