// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.IO;

#endregion

#nullable enable

namespace UnitTests.AM.IO;

[TestClass]
public sealed class TextReaderUtilityTest
{
    private readonly string NL = Environment.NewLine;

    [TestMethod]
    [Description ("Открытие файла для чтения в текстовом режиме")]
    public void TextReaderUtility_OpenRead_1()
    {
        const string expected = "Hello, world!";
        var encoding = Encoding.ASCII;
        var fileName = Path.GetTempFileName();
        File.WriteAllText (fileName, expected, encoding);

        using var reader = TextReaderUtility.OpenRead (fileName, encoding);
        var actual = reader.ReadToEnd();
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Требование наличия строки в потоке")]
    public void TextReaderUtility_RequireLine_1()
    {
        var testText = "first line" + NL + "second line";
        var counter = 0;
        var reader = new StringReader (testText);
        reader.RequireLine();
        counter++;
        reader.RequireLine();
        counter++;
        try
        {
            reader.RequireLine();
        }
        catch (ArsMagnaException)
        {
            counter++;
        }

        Assert.AreEqual (3, counter);
    }
}
