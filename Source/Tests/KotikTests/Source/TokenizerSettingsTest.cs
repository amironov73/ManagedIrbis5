// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directive

using System.IO;

using AM.Kotik.Tokenizers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class TokenizerSettingsTest
    : CommonParserTest
{
    [TestMethod]
    public void TokenizerSettings_Construction_1()
    {
        var settings = new TokenizerSettings();
        Assert.IsNotNull (settings.FirstIdentifierLetter);
        Assert.IsNotNull (settings.NextIdentifierLetter);
        Assert.IsNotNull (settings.KnownTerms);
        Assert.IsNotNull (settings.ReservedWords);
        Assert.IsNull (settings.Tokenizers);
    }

    [TestMethod]
    public void TokenizerSettings_Load_1()
    {
        var fileName = Path.Combine (TestDataPath, "tokenizer.settings");
        var settings = TokenizerSettings.Load (fileName);
        Assert.IsNotNull (settings);
        Assert.IsNotNull (settings.FirstIdentifierLetter);
        Assert.AreEqual (3, settings.FirstIdentifierLetter.Length);
        Assert.IsNotNull (settings.NextIdentifierLetter);
        Assert.AreEqual (10, settings.NextIdentifierLetter.Length);
        Assert.IsNotNull (settings.KnownTerms);
        Assert.AreEqual (4, settings.KnownTerms.Length);
        Assert.IsNotNull (settings.ReservedWords);
        Assert.AreEqual (4, settings.ReservedWords.Length);
        Assert.IsNotNull (settings.Tokenizers);
        Assert.AreEqual (3, settings.Tokenizers.Count);
        Assert.IsTrue (settings.Tokenizers[0] is IntegerTokenizer);
        Assert.IsTrue (settings.Tokenizers[1] is CommentTokenizer);
        Assert.IsTrue (settings.Tokenizers[2] is WhitespaceTokenizer);
    }

    [TestMethod]
    [Description ("Сохранение настроек в файл")]
    public void TokenizerSettings_Save_1()
    {
        var fileName = Path.GetTempFileName();
        var settings = TokenizerSettings.CreateDefault();
        settings.Save (fileName);
    }

    [TestMethod]
    [Description ("Сохранение настроек в файл")]
    public void TokenizerSettings_Save_2()
    {
        var fileName = Path.GetTempFileName();
        var settings = TokenizerSettings.CreateDefault();
        settings.Tokenizers = new ()
        {
            new IntegerTokenizer(),
            new CommentTokenizer(),
            new WhitespaceTokenizer()
        };
        settings.Save (fileName);
    }
}
