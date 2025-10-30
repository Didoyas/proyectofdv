using UnityEngine;
using System.Collections;

public class VidaPlayer : MonoBehaviour
{

    
    public int vidaMaxima = 3;
    public int vidaActual;
    public float invulnerabilidadTiempo = 1f;       // inmunidad despues de reaparecer
    private bool esInvulnerable = false;

    public Vector3 posicionRespawn = new Vector3(-7f, -1f, 0f);

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaActual = vidaMaxima;
        
    }

    // Update is called once per frame
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

        StartCoroutine(HacerInvulnerable());

        if (vidaActual <= 0)
        {
            Morir();
        }
    }


    IEnumerator HacerInvulnerable()
    {
        esInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilidadTiempo);
        esInvulnerable = false;
    }
    
    void Morir()
    {
        transform.position = posicionRespawn;
        vidaActual = vidaMaxima;
    }
    public void push()
    {
        
    }
}
