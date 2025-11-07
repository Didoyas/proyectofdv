using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class PlayerDisparoTests
{
    private GameObject playerObj;
    private PlayerDisparo disparo;
    private FieldInfo cargasActualesField;
    private MethodInfo actualizarUIMethod;

    [SetUp]
    public void Setup()
    {
        playerObj = new GameObject("PlayerTest");
        disparo = playerObj.AddComponent<PlayerDisparo>();
        disparo.baston = new GameObject("Baston");
        disparo.proyectilPrefab = new GameObject("Proyectil");
        disparo.proyectilPrefab.AddComponent<Proyectil>();
        disparo.maxCargas = 5;
        disparo.recargaTiempo = 0.1f;
        var textObj = new GameObject("CargasText");
        disparo.cargasText = textObj.AddComponent<Text>();

        // Inicializamos disparo
        typeof(PlayerDisparo).GetField("_cargasActuales", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(disparo, disparo.maxCargas);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerObj);
    }

    // Al inicializar el componente, las cargas estan al maximo
    [Test]
    public void Init_CargasMax()
    {
        int cargas = (int)typeof(PlayerDisparo).GetField("_cargasActuales", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(disparo);
        Assert.That(cargas, Is.EqualTo(disparo.maxCargas), $"Vidas tienen que inizializarse al maximo {disparo.maxCargas}");
    }

    // Al inicializar el componente, el player puede disparar
    [Test]
    public void Init_PuedeDispararTrue()
    {
        Assert.IsTrue(PlayerDisparo.puedeDisparar, "Player puede disparar al inicio"); // yooooooooo
    }

    // Al disparar, se reducen las cargas de una unidad
    [Test]
    public void Disparar_Decremento()
    {
        int cargasInicial = (int)typeof(PlayerDisparo).GetField("_cargasActuales", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(disparo);
        
        var method = typeof(PlayerDisparo).GetMethod("Disparar", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(disparo, null);
        
        int cargasFinal = (int)typeof(PlayerDisparo).GetField("_cargasActuales", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(disparo);
        
        Assert.That(cargasFinal, Is.EqualTo(cargasInicial - 1), "Disparar decrementa cargas");
    }

    // Al disparar, se crea un objeto proyectil
    [Test]
    public void Disparar_CreaProyectil()
    {
        int proyectilesInicial = Object.FindObjectsByType<Proyectil>(FindObjectsSortMode.None).Length;

        var method = typeof(PlayerDisparo).GetMethod("Disparar", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(disparo, null);
        
        int proyectilesFinal = Object.FindObjectsByType<Proyectil>(FindObjectsSortMode.None).Length;
        
        Assert.That(proyectilesFinal, Is.EqualTo(proyectilesInicial + 1), "Disparar crea instancia nueva de proyectil");
    }

    // Al disparar, se actualiza el texto de cargas
    [Test]
    public void Disparar_ActualizaUI()
    {
        string textoInicial = disparo.cargasText.text;
        
        var method = typeof(PlayerDisparo).GetMethod("Disparar", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(disparo, null);
        
        string textoFinal = disparo.cargasText.text;
        
        Assert.AreNotEqual(textoInicial, textoFinal, "Disparar tiene que actualizar UI");
    }

    // Las cargas siempre son menores o iguales que el maximo de cargas permitido
    [Test]
    public void Cargas_MenorIgualQueMaxCargas()
    {
        disparo.GetType().GetProperty("cargasActuales", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(disparo, 10);
        
        var method = typeof(PlayerDisparo).GetMethod("ActualizarUI", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(disparo, null);
        
        int cargas = (int)typeof(PlayerDisparo).GetField("_cargasActuales", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(disparo);
        
        Assert.That(cargas, Is.LessThanOrEqualTo(disparo.maxCargas), $"Cargas no puede ser > maxCargas ({disparo.maxCargas})");
    }

    // Las cargas son siempre mayores que cero
    [Test]
    public void Cargas_Positivo()
    {
        typeof(PlayerDisparo).GetField("_cargasActuales", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(disparo, 0);
        
        var method = typeof(PlayerDisparo).GetMethod("Disparar", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(disparo, null);
        
        int cargas = (int)typeof(PlayerDisparo).GetField("_cargasActuales", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(disparo);
        
        Assert.That(cargas, Is.GreaterThanOrEqualTo(0), "Cargas siempre >= 0");
    }
}