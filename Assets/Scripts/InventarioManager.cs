using UnityEngine;

public class InventarioManager : MonoBehaviour
{
    public static InventarioManager instance;

    public int vidas = 0;
    public int cargas = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}