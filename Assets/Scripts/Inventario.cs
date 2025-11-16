using UnityEngine;
using UnityEngine.UI; // Para Image

public class Inventario : MonoBehaviour
{
    public enum TipoObjeto
    {
        Ninguno,
        PocionVida,
        PocionCarga 
    }

    public TipoObjeto[] slots = new TipoObjeto[3]; // 3 espacios del inventario
    public int slotSeleccionado = 0; // El slot que estamos viendo
    

    private VidaPlayer vidaPlayer;
    private PlayerDisparo playerDisparo; 


    [Header("UI del Inventario")]
    public Image panelInventarioUI; 
    
    [Header("Sprites de Paneles Completos")]
    public Sprite spritePanelPocion; 
    public Sprite spritePanelPocionCarga;


    void Start()
    {
        vidaPlayer = GetComponent<VidaPlayer>();
        playerDisparo = GetComponent<PlayerDisparo>(); 

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = TipoObjeto.Ninguno;
        }

        if (panelInventarioUI != null)
        {
            panelInventarioUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slotSeleccionado = 0;
            ActualizarInventarioUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            slotSeleccionado = 1;
            ActualizarInventarioUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            slotSeleccionado = 2;
            ActualizarInventarioUI();
        }

        // --- USAR OBJETO ---
        // Con la tecla "Y"
        if (Input.GetKeyDown(KeyCode.Y))
        {
            UsarObjetoSeleccionado();
        }
    }

    public bool AgregarObjeto(TipoObjeto objeto)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == TipoObjeto.Ninguno) 
            {
                slots[i] = objeto; // añadimos
                ActualizarInventarioUI(); 
                return true; 
            }
        }
        
        return false;
    }


    void ActualizarInventarioUI()
    {
        if (panelInventarioUI == null) return;

        TipoObjeto objetoEnSlot = slots[slotSeleccionado];

       switch (objetoEnSlot)
        {
            case TipoObjeto.PocionVida:
                panelInventarioUI.sprite = spritePanelPocion;
                panelInventarioUI.gameObject.SetActive(true);
                break;
            
            case TipoObjeto.PocionCarga:
                panelInventarioUI.sprite = spritePanelPocionCarga;
                panelInventarioUI.gameObject.SetActive(true);
                break;



            case TipoObjeto.Ninguno:
            default:
                panelInventarioUI.gameObject.SetActive(false);
                break;
        }
    }


    void UsarObjetoSeleccionado()
    {
        TipoObjeto objeto = slots[slotSeleccionado];

        if (objeto == TipoObjeto.Ninguno)
        {
            return;
        }

        // --- POCIÓN DE VIDA ---
        if (objeto == TipoObjeto.PocionVida)
        {
            if (vidaPlayer != null)
            {
                vidaPlayer.Curar(1); 
            }
            slots[slotSeleccionado] = TipoObjeto.Ninguno;
        }
        
        // ---  POCIÓN DE CARGA ---
        if (objeto == TipoObjeto.PocionCarga)
        {
            if (playerDisparo != null)
            {
                playerDisparo.AumentarCargaMaxima(); 
            }
            slots[slotSeleccionado] = TipoObjeto.Ninguno;
        }

        ActualizarInventarioUI();
    }
}