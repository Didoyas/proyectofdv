using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public AudioClip sonidoRecoger;

    private void Start()
    {
        ActualizarUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Moneda"))
        {
            AudioManager.PlaySFX(sonidoRecoger, other.transform.position);
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