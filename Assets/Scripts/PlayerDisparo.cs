using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem; 

public class PlayerDisparo : MonoBehaviour
{

    public GameObject baston, proyectilPrefab; //Generador de proyectiles (baston), Prefab del protyectil

    public BalaGestor uiBalas; 
    
    public static bool puedeDisparar = true;
    public int maxCargas = 5; //maximo de cargas         
    private int _cargasActuales; //cargas actuales
    public float recargaTiempo = 1f; 
    private int cargasActuales
    {
        get { return _cargasActuales; }
        set
        {
            _cargasActuales = Mathf.Min(Mathf.Max(value, 0), maxCargas);
            
            if(uiBalas != null) uiBalas.ActualizarBalas(_cargasActuales);

        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (uiBalas != null)
        {
            uiBalas.DibujarCasillas(maxCargas);
        }

        cargasActuales = maxCargas; // Esto activará el 'set' y pintará las balas llenas
        StartCoroutine(Recargar()); }

    // Update is called once per frame
    void Update()
    {
        if (puedeDisparar && Mouse.current.leftButton.wasPressedThisFrame && cargasActuales > 0) //si el jugador hace click izquierdo y las cargas son mayores a 0 
        {

            Disparar();

        }
    }

    void Disparar()
    {

        GameObject proyectil = Instantiate(proyectilPrefab, baston.transform.position, Quaternion.identity); //se instancia proyectil 

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //posicion del puntero del raton 
        mouseWorldPos.z = 0f;
        Vector2 direction = (mouseWorldPos - baston.transform.position).normalized; //diferencia entre la camara y puntero del raton 
        direction.Normalize(); // normaliza 

        Proyectil proyectilScript = proyectil.GetComponent<Proyectil>();
        
        if(proyectilScript != null) 
        {
             proyectilScript.targetVector = new Vector2(direction.x, direction.y);
             proyectilScript.ignoreTags = new string[] { "Player", "Proyectil", "Moneda", "Untagged", "Llave" }; 
        }

        cargasActuales--; //se reduce una carga al disparar
        
    }

    IEnumerator Recargar() //funcion de recarga de cargas 
    {
        while (true) //durante siempre 
        {
            yield return new WaitForSeconds(recargaTiempo); //pusar hasta que pase el tiempo de recarga 

            if (cargasActuales < maxCargas) // si las cargas actuuales son menores a las maximas
            {
                cargasActuales++;
            }
        }
    }

    
    public void AumentarCargaMaxima()
    {
        maxCargas++;
        if(uiBalas != null) uiBalas.DibujarCasillas(maxCargas);
        cargasActuales = maxCargas;
        }
    }
