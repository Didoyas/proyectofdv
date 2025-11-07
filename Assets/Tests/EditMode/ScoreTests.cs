using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class ScoreTests
{
    private GameObject playerObj;
    private Score score;

    [SetUp]
    public void Setup()
    {
        playerObj = new GameObject("PlayerTest");
        score = playerObj.AddComponent<Score>();

        // UI
        score.scoreText = (new GameObject("ScoreText")).AddComponent<Text>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerObj);
    }

    // Al inizializar el componente, el score tiene que ser cero
    [Test]
    public void Init_ScoreCero()
    {
        int initialScore = (int)typeof(Score).GetField("score", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(score);
        Assert.That(initialScore, Is.EqualTo(0), "El score inicial debe ser 0");
    }

    // Al cambiar la variable score del componente (que almacena el score con un entero), ActualizarUI() cambia el texto
    [Test]
    public void ActualizarUI_CambiaTexto()
    {
        var method = typeof(Score).GetMethod("ActualizarUI", BindingFlags.NonPublic | BindingFlags.Instance);

        method.Invoke(score, null);
        // score inicial = 0
        Assert.That(score.scoreText.text, Is.EqualTo("0"), "El score inicial es actualizado a 0");

        var scoreField = typeof(Score).GetField("score", BindingFlags.NonPublic | BindingFlags.Instance);
        scoreField.SetValue(score, 5);
        method.Invoke(score, null);
        Assert.That(score.scoreText.text, Is.EqualTo("5"), "El score es actualizado a 5");
    }
    
    // Al colisionar un objeto que no tiene el tag "Moneda" con el player, su score no incrementa
    [Test]
    public void ColisionSinTagMoneda_NoIncrementaScore()
    {
        var moneda = new GameObject("FalsaMoneda");
        moneda.AddComponent<CircleCollider2D>();
        moneda.tag = "Proyectil"; // Necesitamos un tag que existe entonces ponemos "Proyectil" elegido al azar
        
        var method = typeof(Score).GetMethod("OnTriggerEnter2D", BindingFlags.NonPublic | BindingFlags.Instance);
        int initialScore = (int)typeof(Score).GetField("score", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(score);

        method.Invoke(score, new object[] { moneda.GetComponent<Collider2D>() });

        int finalScore = (int)typeof(Score).GetField("score", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(score);

        Assert.That(initialScore, Is.EqualTo(finalScore), "Objeto con tag != 'Moneda' no incrementa score");
    }
}