// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#endregion

#nullable enable

namespace UnitTests.AM.Text;

[TestClass]
public sealed class SparcerTest
{
    [TestMethod]
    [Description ("Нулевая строка")]
    public void Sparcer_SparceText_1()
    {
        var sparcer = new Sparcer();
        Assert.IsNull (sparcer.SparceText (null));
    }

    [TestMethod]
    [Description ("Пустая строка")]
    public void Sparcer_SparceText_2()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual (string.Empty, sparcer.SparceText (string.Empty));
    }

    [TestMethod]
    [Description ("Пробельная строка")]
    public void Sparcer_SparceText_3()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual (string.Empty, sparcer.SparceText (" "));
        Assert.AreEqual (string.Empty, sparcer.SparceText ("   "));
    }

    [TestMethod]
    [Description ("Удаление начальных и конечных пробелов")]
    public void Sparcer_SparceText_4()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual ("hello", sparcer.SparceText (" hello"));
        Assert.AreEqual ("hello", sparcer.SparceText ("hello "));
        Assert.AreEqual ("hello", sparcer.SparceText (" hello "));
    }

    [TestMethod]
    [Description ("Удаление лишних пробелов внутри строки")]
    public void Sparcer_SparceText_5()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual ("hello world", sparcer.SparceText ("hello world"));
        Assert.AreEqual ("hello world", sparcer.SparceText ("hello  world"));
        Assert.AreEqual ("hello world", sparcer.SparceText ("hello  world"));
    }

    [TestMethod]
    [Description ("Нормализация пробелов вокруг точки или запятой")]
    public void Sparcer_SparceText_6()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual ("hello, world", sparcer.SparceText ("hello, world"));
        Assert.AreEqual ("hello, world", sparcer.SparceText ("hello,world"));
        Assert.AreEqual ("hello, world", sparcer.SparceText ("hello , world"));
        Assert.AreEqual ("hello, world", sparcer.SparceText ("hello  ,world"));

        Assert.AreEqual ("hello. world", sparcer.SparceText ("hello. world"));
        Assert.AreEqual ("hello. world", sparcer.SparceText ("hello.world"));
        Assert.AreEqual ("hello. world", sparcer.SparceText ("hello . world"));
        Assert.AreEqual ("hello. world", sparcer.SparceText ("hello  .world"));
    }

    [TestMethod]
    [Description ("Нормализация пробелов вокруг закрывающей скобки")]
    public void Sparcer_SparceText_7()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual ("hello) world", sparcer.SparceText ("hello) world"));
        Assert.AreEqual ("hello) world", sparcer.SparceText ("hello)world"));
        Assert.AreEqual ("hello) world", sparcer.SparceText ("hello ) world"));
        Assert.AreEqual ("hello) world", sparcer.SparceText ("hello  )world"));
    }

    [TestMethod]
    [Description ("Нормализация пробелов вокруг открывающей скобки")]
    public void Sparcer_SparceText_8()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual ("hello (world", sparcer.SparceText ("hello( world"));
        Assert.AreEqual ("hello (world", sparcer.SparceText ("hello(world"));
        Assert.AreEqual ("hello (world", sparcer.SparceText ("hello ( world"));
        Assert.AreEqual ("hello (world", sparcer.SparceText ("hello  (world"));
    }

    [TestMethod]
    [Description ("Нормализация пробелов вокруг запятой")]
    public void Sparcer_SparceText_9()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual ("hello, world", sparcer.SparceText ("hello, world"));
        Assert.AreEqual ("hello, world", sparcer.SparceText ("hello,world"));
        Assert.AreEqual ("hello, world", sparcer.SparceText ("hello , world"));
        Assert.AreEqual ("hello, world", sparcer.SparceText ("hello  ,world"));
    }

    [TestMethod]
    [Description ("Нормализация пробелов вокруг кавычек")]
    public void Sparcer_SparceText_10()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual ("hello \"world", sparcer.SparceText ("hello\"world"));
        Assert.AreEqual ("hello \"\" world", sparcer.SparceText ("hello\"\"world"));
    }

    [TestMethod]
    [Description ("Нормализация пробелов вокруг минуса")]
    public void Sparcer_SparceText_11()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual ("hello) - world", sparcer.SparceText ("hello)- world"));
        Assert.AreEqual ("hello-world", sparcer.SparceText ("hello-world"));
        Assert.AreEqual ("hello - world", sparcer.SparceText ("hello - world"));
        Assert.AreEqual ("hello - world", sparcer.SparceText ("hello -world"));
        Assert.AreEqual ("hello - world", sparcer.SparceText ("hello- world"));
    }

    [TestMethod]
    [Description ("Нормализация пробелов вокруг цифр")]
    public void Sparcer_SparceText_12()
    {
        var sparcer = new Sparcer();
        Assert.AreEqual ("123)456", sparcer.SparceText ("123)456"));
    }
}
