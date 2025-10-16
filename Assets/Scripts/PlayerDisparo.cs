using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerDisparo : MonoBehaviour
{

    public GameObject baston, proyectilPrefab; //Generador de proyectiles (baston), Prefab del protyectil
    public float proyectilSpeed = 10f; //velocidad de lanzamiento del proyectil

    public Text cargasText; //texto UI de las cargas actuales

    public int maxCargas = 20; //maximo de cargas         
    private int _cargasActuales; //cargas actuales
    private int cargasActuales
    {
        get { return _cargasActuales; }
        set
        {
            _cargasActuales = value;
            ActualizarUI();
        }
    }
    public float recargaTiempo = 3f; //tiempo de recarga de las cargas

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cargasActuales = maxCargas; //cargas al maximo
        StartCoroutine(Recargar()); //Inicia rutina de recarga
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cargasActuales > 0) //si el jugador hace click izquierdo y las cargas son mayores a 0 
        {
            GameObject proyectil = Instantiate(proyectilPrefab, baston.transform.position, Quaternion.identity); //se instancia proyectil 

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //posicion del puntero del raton 
            Vector2 direction = (mouseWorldPos - baston.transform.position).normalized; //diferencia entre la camara y puntero del raton 
            direction.Normalize(); // normaliza 

            Proyectil proyectilScript = proyectil.GetComponent<Proyectil>();
            proyectilScript.targetVector = direction; // direccion del proyectil 
            proyectilScript.speed = proyectilSpeed; //velocidad del proyectil 

            cargasActuales--; //se reduce una carga al disparar
        }
    }

    IEnumerator Recargar() //funcion de recarga de cargas 
    {
        while (true) //durante siempre 
        {
            yield return new WaitForSeconds(recargaTiempo); //pusar hasta que pase el tiempo de recarga 

            if (cargasActuales < maxCargas) // si las cargas actuuales son menores a las maximas
            {
                cargasActuales = cargasActuales + 5; //se recuperan 5 cargas 
                if (cargasActuales > maxCargas) cargasActuales = maxCargas;
            }
        }
    }

    void ActualizarUI() // funcion para actuializar el texto de cargas en panmtalla
    {
        if (cargasText != null)
        {
            cargasText.text = "Cargas " + cargasActuales + " / " + maxCargas;
        }
    }
}
