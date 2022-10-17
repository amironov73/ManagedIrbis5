// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

#endregion

#nullable enable

namespace UnitTests.AM.IO;

[TestClass]
public sealed class TextWriterUtilityTest
{
    [TestMethod]
    [Description ("Добавление текста в конец файл")]
    public void TextWriterUtility_Append_1()
    {
        var encoding = Encoding.ASCII;

        var fileName = Path.GetTempFileName();
        File.WriteAllText (fileName, "Hello, ", encoding);
        using (var writer = TextWriterUtility.Append (fileName, encoding))
        {
            writer.Write ("world!");
        }

        var actual = File.ReadAllText (fileName, encoding);
        Assert.AreEqual ("Hello, world!", actual);
    }

    [TestMethod]
    [Description ("Создание файла в указанной кодировке")]
    public void TextWriterUtility_Create_1()
    {
        const string expected = "Hello, world!";
        var encoding = Encoding.ASCII;
        var fileName = Path.GetTempFileName();
        File.Delete (fileName);
        using (var writer = TextWriterUtility.Create (fileName, encoding))
        {
            writer.Write (expected);
        }

        var actual = File.ReadAllText (fileName, encoding);
        Assert.AreEqual (expected, actual);
    }
}
