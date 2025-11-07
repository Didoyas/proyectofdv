using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Reflection;

public class MovimientoTests
{
    private GameObject playerObj;
    private Movimiento movimiento;

    [SetUp]
    public void Setup()
    {
        // gameObject de testing con componente movimiento
        playerObj = new GameObject("PlayerTest");
        movimiento = playerObj.AddComponent<Movimiento>();

        // InputActionAsset
        movimiento.playerInputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
        movimiento.playerInputActionAsset.AddActionMap("Player");
        var map = movimiento.playerInputActionAsset.FindActionMap("Player");
        map.AddAction("Move");
        map.AddAction("Dash");

        movimiento.Awake(); // llamada a awake() manual
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerObj);
    }

    // Verifica si tras el Awake() del jugador (concretamente del script Movimiento) las variables InputActions no son null
    [Test]
    public void Awake_EncuentraInputActions()
    {
        var moveField = typeof(Movimiento).GetField("moveAction", BindingFlags.NonPublic | BindingFlags.Instance);
        var dashField = typeof(Movimiento).GetField("dashAction", BindingFlags.NonPublic | BindingFlags.Instance);

        var moveAction = moveField.GetValue(movimiento) as InputAction;
        var dashAction = dashField.GetValue(movimiento) as InputAction;

        Assert.NotNull(moveAction, "Move InputAction no es null tras Awake()");
        Assert.NotNull(dashAction, "Dash InputAction no es null tras Awake()");
    }

    // Verifica si las variables dashTimer y cooldownTimer son positivas incluso cuando se les forza un valor negativo
    [Test]
    public void DashYCooldown_SiemprePositivos()
    {
        var dashTimerField = typeof(Movimiento).GetField("_dashTimer", BindingFlags.NonPublic | BindingFlags.Instance);
        var cooldownTimerField = typeof(Movimiento).GetField("_cooldownTimer", BindingFlags.NonPublic | BindingFlags.Instance);
        
        dashTimerField.SetValue(movimiento, -5f);
        cooldownTimerField.SetValue(movimiento, -3f);
        
        movimiento.GetType().GetProperty("dashTimer", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(movimiento, -5f);
        movimiento.GetType().GetProperty("cooldownTimer", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(movimiento, -3f);

        Assert.That((float)dashTimerField.GetValue(movimiento), Is.GreaterThanOrEqualTo(0f), "dashTimer siempre >= 0");
        Assert.That((float)cooldownTimerField.GetValue(movimiento), Is.GreaterThanOrEqualTo(0f), "cooldownTimer (dash) siempre >= 0");
    }

    /*// A Test behaves as an ordinary method
    [Test]
    public void TestSimpleSimplePasses()
    {
        // Use the Assert class to test conditions
        int r = 2 + 2;
        Assert.AreEqual(4, r, "2 + 2 igual a 4");
    }

    [Test]
    public void TestSimpleFails()
    {
        // Use the Assert class to test conditions
        int r = 2 + 1;
        Assert.AreEqual(4, r, "2 + 1 not igual a 4");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestSimpleWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
