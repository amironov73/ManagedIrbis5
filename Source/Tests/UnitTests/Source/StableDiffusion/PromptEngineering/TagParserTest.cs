// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

#region Using directives


using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.StableDiffusion.PromptEngineering;

#endregion

namespace UnitTests.StableDiffusion.PromptEngineering;

[TestClass]
public sealed class TagParserTest
{
    [TestMethod]
    [Description ("Без разделителей")]
    public void TagParser_Parse_1()
    {
        var parser = new TagParser();
        var result = parser.Parse ("hello world");
        Assert.IsNotNull (result);
        Assert.AreEqual (1, result.Count);
        Assert.AreEqual ("hello world", result[0].Title);
    }

    [TestMethod]
    [Description ("С разделителями")]
    public void TagParser_Parse_2()
    {
        var parser = new TagParser();
        var result = parser.Parse ("hello world, again");
        Assert.IsNotNull (result);
        Assert.AreEqual (2, result.Count);
        Assert.AreEqual ("hello world", result[0].Title);
        Assert.AreEqual ("again", result[1].Title);
    }

    [TestMethod]
    [Description ("С избыточными разделителями")]
    public void TagParser_Parse_3()
    {
        var parser = new TagParser();
        var result = parser.Parse ("hello world,, again");
        Assert.IsNotNull (result);
        Assert.AreEqual (2, result.Count);
        Assert.AreEqual ("hello world", result[0].Title);
        Assert.AreEqual ("again", result[1].Title);
    }

    [TestMethod]
    [Description ("С избыточными разделителями")]
    public void TagParser_Parse_4()
    {
        var parser = new TagParser();
        var result = parser.Parse (",hello world,again,");
        Assert.IsNotNull (result);
        Assert.AreEqual (2, result.Count);
        Assert.AreEqual ("hello world", result[0].Title);
        Assert.AreEqual ("again", result[1].Title);
    }

    [TestMethod]
    [Description ("Только разделители")]
    public void TagParser_Parse_5()
    {
        var parser = new TagParser();
        var result = parser.Parse (", , ,");
        Assert.IsNotNull (result);
        Assert.AreEqual (0, result.Count);
    }

    [TestMethod]
    [Description ("LORA")]
    public void TagParser_Parse_6()
    {
        var parser = new TagParser();
        var result = parser.Parse ("pretty <lora:Jeff Milton:1>, woman");
        Assert.IsNotNull (result);
        Assert.AreEqual (3, result.Count);
        Assert.AreEqual ("pretty", result[0].Title);
        Assert.AreEqual ("<lora:Jeff Milton:1>", result[1].Title);
        Assert.AreEqual ("woman", result[2].Title);
    }

    [TestMethod]
    [Description ("Скобки")]
    public void TagParser_Parse_7()
    {
        var parser = new TagParser();
        var result = parser.Parse ("(pretty woman) (walking down) the street");
        Assert.IsNotNull (result);
        Assert.AreEqual (3, result.Count);
        Assert.AreEqual ("pretty woman", result[0].Title);
        Assert.AreEqual ("walking down", result[1].Title);
        Assert.AreEqual ("the street", result[2].Title);
    }

    [TestMethod]
    [Description ("Скобки")]
    public void TagParser_Parse_8()
    {
        var parser = new TagParser();
        var result = parser.Parse ("((pretty woman)) (((walking down))) the street");
        Assert.IsNotNull (result);
        Assert.AreEqual (3, result.Count);
        Assert.AreEqual ("pretty woman", result[0].Title);
        Assert.AreEqual ("walking down", result[1].Title);
        Assert.AreEqual ("the street", result[2].Title);
    }

    [TestMethod]
    [Description ("Скобки")]
    public void TagParser_Parse_9()
    {
        var parser = new TagParser();
        var result = parser.Parse ("[pretty woman] {walking down} the street");
        Assert.IsNotNull (result);
        Assert.AreEqual (3, result.Count);
        Assert.AreEqual ("pretty woman", result[0].Title);
        Assert.AreEqual ("walking down", result[1].Title);
        Assert.AreEqual ("the street", result[2].Title);
    }

    [TestMethod]
    [Description ("Посторонние символы")]
    public void TagParser_Parse_10()
    {
        var parser = new TagParser();
        var result = parser.Parse ("Here? It is!");
        Assert.IsNotNull (result);
        Assert.AreEqual (2, result.Count);
        Assert.AreEqual ("Here", result[0].Title);
        Assert.AreEqual ("It is", result[1].Title);
    }

    [TestMethod]
    [Description ("Пустая строка")]
    public void TagParser_Parse_11()
    {
        var parser = new TagParser();
        var result = parser.Parse (string.Empty);
        Assert.IsNotNull (result);
        Assert.AreEqual (0, result.Count);
    }

    [TestMethod]
    [Description ("Пустая строка")]
    public void TagParser_Parse_12()
    {
        var parser = new TagParser();
        var result = parser.Parse (" ");
        Assert.IsNotNull (result);
        Assert.AreEqual (0, result.Count);
    }

}
