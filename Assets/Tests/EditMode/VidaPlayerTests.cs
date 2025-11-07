using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class VidaPlayerTests
{
    private GameObject playerObj;
    private VidaPlayer vida;

    [SetUp]
    public void Setup()
    {
        playerObj = new GameObject("PlayerTest");
        vida = playerObj.AddComponent<VidaPlayer>();
        
        var uiObj = new GameObject("UI");
        vida.imagenVidasUI = uiObj.AddComponent<Image>();
        
        vida.spritesVidas = new Sprite[4]
        {
            Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), Vector2.zero),
            Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), Vector2.zero),
            Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), Vector2.zero),
            Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), Vector2.zero)
        };

        vida.vidaMaxima = 3;
        vida.invulnerabilidadTiempo = 0.01f;

        // inizializacion del componente vida
        vida.vidaActual = vida.vidaMaxima;
        var method = typeof(VidaPlayer).GetMethod("ActualizarVidasUI", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(vida, null);
    }

    [TearDown]
    public void TearDown()
    {
        Time.timeScale = 1f;
        Object.DestroyImmediate(playerObj);
    }

    // Al inicializar el componente, la vida y el sprite iniciales son correctos
    [Test]
    public void Init_VidaMax()
    {
        Assert.That(vida.vidaActual, Is.EqualTo(3), "Vidas tienen que inizializarse al maximo (3)");
        Assert.That(vida.imagenVidasUI.sprite, Is.EqualTo(vida.spritesVidas[0]), "VidasUI tiene index 0");
    }

    // Cada sprite tiene que corresponder con las vidas que tiene el player
    [Test]
    public void ActualizarVidasUI_SpriteCorrectoParaVida()
    {
        for (int hp = 2; hp >= 0; hp--)
        {
            vida.RecibirDaño(1);
            int index = vida.vidaMaxima - vida.vidaActual;

            Assert.AreSame(
                vida.spritesVidas[index],
                vida.imagenVidasUI.sprite,
                $"Index tiene que ser {index} para vida={vida.vidaActual}"
            );
        }
    }
    
    // Al recibir daño, el sprite de las vidas cambia
    [Test]
    public void RecibirDaño_ActualizaVidasUI()
    {
        Sprite spriteInicial = vida.imagenVidasUI.sprite;
        vida.RecibirDaño(1);
        Sprite spriteFinal = vida.imagenVidasUI.sprite;
        
        Assert.AreNotSame(spriteInicial, spriteFinal, "El sprite tiene que cambiar al recibir daño");
    }

    // Al recibir daño (1), las vidas decrementan
    [Test]
    public void RecibirDaño_Decremento()
    {
        vida.RecibirDaño(1);
        Assert.That(vida.vidaActual, Is.EqualTo(2), "Vida tiene que decrementar");
    }

    // Al recibir daño (10), la vida resultante siempre es positiva
    [Test]
    public void RecibirDaño_Positivo()
    {
        vida.vidaActual = 1;
        vida.RecibirDaño(10);
        Assert.That(vida.vidaActual, Is.EqualTo(0), "Vida siempre >= 0");
    }

    // Al recibir daño, las vidas no cambian si player es invulnerable
    [Test]
    public void RecibirDaño_NoDecrementaSiInvulnerable()
    {
        var invulnerableField = typeof(VidaPlayer).GetField("esInvulnerable", BindingFlags.NonPublic | BindingFlags.Instance);
        invulnerableField.SetValue(vida, true);

        int vidaInicial = vida.vidaActual;
        vida.RecibirDaño(1);

        Assert.That(vida.vidaActual, Is.EqualTo(vidaInicial), "Vida no decrementa si player invulnerable");
    }
    
    // Al morir, el panel de muerte tiene que activarse y el player desactivarse
    [Test]
    public void Morir_ActivaPanelDesactivaPlayer()
    {
        var panel = new GameObject("Panel");
        panel.SetActive(false);
        vida.panelMuerte = panel;
        
        vida.RecibirDaño(vida.vidaMaxima);

        Assert.IsTrue(panel.activeSelf, "PanelMuerte tiene que estar activo al morir");
        Assert.IsFalse(playerObj.activeSelf, "Player tiene que estar inactivo al morir");
    }

    // Al morir, el tiempo tiene que pausarse
    [Test]
    public void Morir_PausaTiempo()
    {
        Time.timeScale = 1f;
        vida.panelMuerte = new GameObject("Panel");
        
        vida.RecibirDaño(vida.vidaMaxima);
        
        Assert.That(Time.timeScale, Is.EqualTo(0f), "Morir pausa el tiempo");
    }
}