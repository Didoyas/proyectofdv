using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Moneda"))
        {
            Destroy(other.gameObject);
            score++;
            ActualizarUI();
        }
    }

    void ActualizarUI() // funcion para actuializar el texto de cargas en panmtalla
    {
        if (scoreText != null)
        {
            scoreText.text = "" + score;
        }
    }
}