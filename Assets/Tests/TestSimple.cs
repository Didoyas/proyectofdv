using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSimple
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestSimpleSimplePasses()
    {
        // Use the Assert class to test conditions
        int r = 2 + 2;
        Assert.AreEqual(4, r, "2 + 2 igual a 4");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestSimpleWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
