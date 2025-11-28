using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class ScoreTests
{
    private ScoreManager sm;
    private GameObject playerObj;
    private Score score;

    [SetUp]
    public void Setup()
    {
        sm = new GameObject("ScoreManager").AddComponent<ScoreManager>();
        sm.Awake();

        playerObj = new GameObject("PlayerTest");
        score = playerObj.AddComponent<Score>();

        // UI
        score.scoreText = (new GameObject("ScoreText")).AddComponent<Text>();
    }

    [TearDown]
    public void TearDown()
    {
        if(ScoreManager.instance != null)
            Object.DestroyImmediate(ScoreManager.instance.gameObject);
        Object.DestroyImmediate(playerObj);
    }

    // Al inizializar el componente, el score tiene que ser cero
    /*[Test]
    public IEnumerator Init_ScoreCero()
    {
        Assert.That(ScoreManager.instance.score, Is.EqualTo(0), "Score inicial debe ser 0");
        yield break;
    }*/

    // Al cambiar la variable score del componente (que almacena el score con un entero), ActualizarUI() cambia el texto
    [Test]
    public void ActualizarUI_CambiaTexto()
    {
        ScoreManager.instance.score = 0;
        score.ActualizarUI();
        Assert.That(score.scoreText.text, Is.EqualTo("0"), "El score inicial es actualizado a 0");

        ScoreManager.instance.score = 5;
        score.ActualizarUI();
        Assert.That(score.scoreText.text, Is.EqualTo("5"), "El score es actualizado a 5");
    }
    
    // Al colisionar un objeto que no tiene el tag "Moneda" con el player, su score no incrementa
    [Test]
    public async Task ColisionSinTagMoneda_NoIncrementaScore()
    {
        ScoreManager.instance.score = 0;

        GameObject obj = new GameObject("Test");
        var col = obj.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        obj.tag = "Untagged";

        // Forzamos colision
        obj.transform.position = playerObj.transform.position;
        await Task.Yield();

        Assert.That(ScoreManager.instance.score, Is.EqualTo(0), "Objeto con tag != 'Moneda' no incrementa score");

        Object.Destroy(obj);
    }
}