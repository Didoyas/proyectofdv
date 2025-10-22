
using NUnit.Framework;

public class Test
{
    [Test]
    public void True()
    {
        Assert.IsTrue(true, "Ejemplo 1.");
    }

    [Test]
    public void Suma()
    {
        int r = 2 + 2;
        Assert.AreEqual(4, r, "2 + 2 igual a 4");
    }
}
