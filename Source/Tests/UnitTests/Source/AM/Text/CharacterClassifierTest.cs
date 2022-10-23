// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#endregion

#nullable enable

namespace UnitTests.AM.Text;

[TestClass]
public sealed class CharacterClassifierTest
{
    [TestMethod]
    [Description ("Обработка пустой строки")]
    public void CharacterClassifier_DetectCharacterClasses_1()
    {
        var text = string.Empty;
        var classes = CharacterClassifier.DetectCharacterClasses (text);
        Assert.AreEqual (CharacterClass.None, classes);
    }

    [TestMethod]
    [Description ("Только символы базовой латиницы")]
    public void CharacterClassifier_DetectCharacterClasses_2()
    {
        const string text = "Hello";
        var classes = CharacterClassifier.DetectCharacterClasses (text);
        Assert.AreEqual (CharacterClass.BasicLatin, classes);
    }

    [TestMethod]
    [Description ("Только символы кириллицы")]
    public void CharacterClassifier_DetectCharacterClasses_3()
    {
        const string text = "Привет";
        var classes = CharacterClassifier.DetectCharacterClasses (text);
        Assert.AreEqual (CharacterClass.Cyrillic, classes);
    }

    [TestMethod]
    [Description ("Только цифры")]
    public void CharacterClassifier_DetectCharacterClasses_4()
    {
        const string text = "2128506";
        var classes
            = CharacterClassifier.DetectCharacterClasses (text);
        Assert.AreEqual (CharacterClass.Digit, classes);
    }

    [TestMethod]
    [Description ("Только управляющие символы")]
    public void CharacterClassifier_DetectCharacterClasses_5()
    {
        const string text = "\r\n";
        var classes = CharacterClassifier.DetectCharacterClasses (text);
        Assert.AreEqual (CharacterClass.ControlCharacter, classes);
    }

    [TestMethod]
    [Description ("Смесь базовой латиницы, управляющих символов, кириллицы и цифр")]
    public void CharacterClassifier_DetectCharacterClasses_6()
    {
        const string text = "Hello, Привет, 2128506\r\n";
        var classes = CharacterClassifier.DetectCharacterClasses (text);
        Assert.AreEqual
            (
                CharacterClass.BasicLatin
                | CharacterClass.ControlCharacter
                | CharacterClass.Cyrillic
                | CharacterClass.Digit,
                classes
            );
    }

    [TestMethod]
    [Description ("Проверка на смесь кириллицы и латиницы")]
    public void CharacterClassifier_IsBothCyrillicAndLatin_1()
    {
        var text = "Hello, 2128506\r\n";
        var classes = CharacterClassifier.DetectCharacterClasses (text);
        Assert.IsFalse (CharacterClassifier.IsBothCyrillicAndLatin (classes));

        text = "Привет, 2128506\r\n";
        classes = CharacterClassifier.DetectCharacterClasses (text);
        Assert.IsFalse (CharacterClassifier.IsBothCyrillicAndLatin (classes));

        text = "Hello, Привет, 2128506\r\n";
        classes = CharacterClassifier.DetectCharacterClasses (text);
        Assert.IsTrue (CharacterClassifier.IsBothCyrillicAndLatin (classes));

        text = "2128506\r\n";
        classes = CharacterClassifier.DetectCharacterClasses (text);
        Assert.IsFalse (CharacterClassifier.IsBothCyrillicAndLatin (classes));
    }
}
