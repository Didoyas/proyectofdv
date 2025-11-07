using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class VidaPlayer : MonoBehaviour
{
    public int vidaMaxima = 3;
    public int vidaActual;
    public float invulnerabilidadTiempo = 1f;       // inmunidad despues de reaparecer
    private bool esInvulnerable = false;

    public GameObject panelMuerte;

    public Vector3 posicionRespawn = new Vector3(-7f, -1f, 0f);

    [Header("UI de Vida")] public Image imagenVidasUI;             // componentes images
    public Sprite[] spritesVidas;           // lista de 4 pngs
    

    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarVidasUI();            // llamo a funcion e imprimo 3vidas
    }

    void Update()
    {

    }

    public void RecibirDaño(int daño)
    {
        if (esInvulnerable)
        {
            return;
        }

        vidaActual = vidaActual - daño;

        if (vidaActual < 0)
        {
            vidaActual = 0;
        }

        ActualizarVidasUI();

        StartCoroutine(HacerInvulnerable());

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void ActualizarVidasUI()
    {
        if (imagenVidasUI != null && spritesVidas.Length == 4)
        {
            // Esta lógica asume que tus sprites están en este orden en el array:
            // spritesVidas[0] = 3 corazones (vidaMaxima)
            // spritesVidas[1] = 2 corazones
            // spritesVidas[2] = 1 corazón
            // spritesVidas[3] = 0 corazones (vida = 0)

            // Mapeo la vida (3, 2, 1, 0) al índice del array (0, 1, 2, 3)
            int indiceSprite = vidaMaxima - vidaActual;

            // comprobar y asi evitar errores que el índice salga deñ rango
            if (indiceSprite >= 0 && indiceSprite < spritesVidas.Length)
            {
                imagenVidasUI.sprite = spritesVidas[indiceSprite];
            }
        }
        else
        {

        }
    }

    public IEnumerator HacerInvulnerable()
    {
        yield return HacerInvulnerable(invulnerabilidadTiempo);
    }

    public IEnumerator HacerInvulnerable(float duracion)
    {
        esInvulnerable = true;
        yield return new WaitForSeconds(duracion);
        esInvulnerable = false;
    }

    void Morir()
    {

        if (panelMuerte != null)
        {
            panelMuerte.SetActive(true);
        }

        Time.timeScale = 0f;
        gameObject.SetActive(false);
    }
    
    public void push()
    {

    }
}