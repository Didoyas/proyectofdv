using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mercado : MonoBehaviour
{
    [Header("Textos")]
    public Text numCargas;
    public Text numVidas;
    int nCargas = 0;
    int nVidas = 0;
    public Text precioCargas;
    public Text precioVidas;
    public Score score;
    public Text invCargas;
    public Text invVidas;
    public Button comprar;

    [Header("Precios")]
    public int vidas = 0;
    public int cargas = 0;
    int total;
    bool puedeComprar = true;

    private void Start()
    {
        total = InventarioManager.instance.cargas + InventarioManager.instance.vidas;
        ActualizarPrecios();
        ActualizarUI();
    }

    public void IncrementoCargas()
    {
        if(total < 3)
        {
            nCargas++;
            total++;
            ActualizarUI();
        }
    }

    public void DecrementoCargas()
    {
        if(nCargas > 0)
        {
            nCargas--;
            total--;
            ActualizarUI();
        }
    }
    
    public void IncrementoVidas()
    {
        if(total < 3)
        {
            nVidas++;
            total++;
            ActualizarUI();
        }
    }

    public void DecrementoVidas()
    {
        if(nVidas > 0)
        {
            nVidas--;
            total--;
            ActualizarUI();
        }
    }

    public void Comprar()
    {
        int precio = nCargas*cargas + nVidas*vidas;
        if(puedeComprar)
        {
            if(ScoreManager.instance.score >= precio)
            {
                ScoreManager.instance.score -= precio;
                score.ActualizarUI();

                InventarioManager.instance.cargas += nCargas;
                InventarioManager.instance.vidas += nVidas;

                nVidas = 0;
                nCargas = 0;

                ActualizarUI();
            }
            else
            {
                StartCoroutine(CompraFallida());
            }
        }
    }

    void ActualizarUI()
    {
        if (numCargas != null && numVidas != null)
        {
            numCargas.text = nCargas.ToString();
            numVidas.text = nVidas.ToString();
            invCargas.text = InventarioManager.instance.cargas.ToString();
            invVidas.text = InventarioManager.instance.vidas.ToString();
        }
    }

    void ActualizarPrecios()
    {
        if (precioCargas != null && precioVidas != null)
        {
            precioCargas.text = cargas.ToString();
            precioVidas.text = vidas.ToString();
        }
    }

    IEnumerator CompraFallida()
    {
        Color comprarColor = comprar.image.color;
        Color scoreColor = score.scoreText.color;
        comprar.image.color = Color.red;
        score.scoreText.color = Color.red;
        puedeComprar = false;

        yield return new WaitForSeconds(1f);

        puedeComprar = true;
        comprar.image.color = comprarColor;
        score.scoreText.color = scoreColor;

    }
}