// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.StableDiffusion.PromptEngineering;

#endregion

namespace UnitTests.StableDiffusion.PromptEngineering;

[TestClass]
public sealed class LeetSpeakTest
{
    [TestMethod]
    [Description ("Нечего заменять")]
    public void LeetSpeak_Translate_1()
    {
        Assert.AreEqual ("xxx", LeetSpeak.Translate ("xxx"));
    }

    [TestMethod]
    [Description ("Есть что заменять")]
    public void LeetSpeak_Translate_2()
    {
        Assert.AreEqual ("Ann474r15", LeetSpeak.Translate ("Anna Faris"));
    }

    [TestMethod]
    [Description ("Пустая строка")]
    public void LeetSpeak_Translate_3()
    {
        Assert.AreEqual (string.Empty, LeetSpeak.Translate (string.Empty));
    }

    [TestMethod]
    [Description ("Начальные пробелы")]
    public void LeetSpeak_Translate_4()
    {
        Assert.AreEqual ("Ann4", LeetSpeak.Translate (" Anna"));
    }

    [TestMethod]
    [Description ("Конечные пробелы")]
    public void LeetSpeak_Translate_5()
    {
        Assert.AreEqual ("Ann4", LeetSpeak.Translate ("Anna "));
    }

    [TestMethod]
    [Description ("Сплошные пробелы")]
    public void LeetSpeak_Translate_6()
    {
        Assert.AreEqual (string.Empty, LeetSpeak.Translate ("  "));
    }
}
