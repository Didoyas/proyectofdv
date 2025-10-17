using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Instrucciones : MonoBehaviour
{
    public GameObject panelInstrucciones;

    void Start()
    {
        if (panelInstrucciones != null){
            panelInstrucciones.SetActive(true);}

        Time.timeScale = 0f;
        PlayerDisparo.puedeDisparar = false;
    }
    

    public void OcultarPanel()
    {
        StartCoroutine(ResumeGame());
    }

    IEnumerator ResumeGame()
    {
        if (panelInstrucciones != null){
            panelInstrucciones.SetActive(false);}

        Time.timeScale = 1f;
        yield return null;
        PlayerDisparo.puedeDisparar = true;
    }
}