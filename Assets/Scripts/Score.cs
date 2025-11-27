using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;

    private void Start()
    {
        ActualizarUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Moneda"))
        {
            Destroy(other.gameObject);
            ScoreManager.instance.score++;
            ActualizarUI();
        }
    }

    public void ActualizarUI()
    {
        if (scoreText != null)
        {
            scoreText.text = ScoreManager.instance.score.ToString();
        }
    }
}