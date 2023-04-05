// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.BibTex;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.BibTex;

[TestClass]
public sealed class BibTexFieldTest
{
    private BibTexField _GetField()
    {
        return new BibTexField()
        {
            Tag = KnownTags.BookTitle,
            Value = "Программирование для ИРБИС64"
        };
    }

    private void _Compare
        (
            BibTexField first,
            BibTexField second
        )
    {
        Assert.AreEqual (first.Tag, second.Tag);
        Assert.AreEqual (first.Value, second.Value);
    }

    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void BibTexField_Construction_1()
    {
        var field = new BibTexField();
        Assert.IsNull (field.Tag);
        Assert.IsNull (field.Value);
        Assert.IsNull (field.UserData);
    }

    private void _Serialization
        (
            BibTexField first
        )
    {
        var memory = first.SaveToMemory();
        var second = memory.RestoreObjectFromMemory<BibTexField>();
        Assert.IsNotNull (second);
        _Compare (first, second);
        Assert.IsNull (second.UserData);
    }

    [TestMethod]
    [Description ("Сериализация")]
    public void BibTexField_Serialization_1()
    {
        var field = new BibTexField();
        _Serialization (field);

        field = _GetField();
        field.UserData = "User data";
        _Serialization (field);
    }

    [TestMethod]
    [Description ("Верификация")]
    public void BibTexField_Verification_1()
    {
        var field = new BibTexField();
        Assert.IsFalse (field.Verify (false));

        field = _GetField();
        Assert.IsTrue (field.Verify (false));
    }

    [TestMethod]
    [Description ("XML-представление")]
    public void BibTexField_ToXml_1()
    {
        var field = new BibTexField();
        Assert.AreEqual
            (
                "<field />",
                XmlUtility.SerializeShort (field)
            );

        field = _GetField();
        Assert.AreEqual
            (
                "<field tag=\"booktitle\" value=\"Программирование для ИРБИС64\" />",
                XmlUtility.SerializeShort (field)
            );
    }

    [TestMethod]
    [Description ("JSON-представление")]
    public void BibTexField_ToJson_1()
    {
        var field = new BibTexField();
        Assert.AreEqual
            (
                "{}",
                JsonUtility.SerializeShort (field)
            );

        field = _GetField();
        Assert.AreEqual
            (
                "{\"tag\":\"booktitle\",\"value\":\"\\u041F\\u0440\\u043E\\u0433\\u0440\\u0430\\u043C\\u043C\\u0438\\u0440\\u043E\\u0432\\u0430\\u043D\\u0438\\u0435 \\u0434\\u043B\\u044F \\u0418\\u0420\\u0411\\u0418\\u042164\"}",
                JsonUtility.SerializeShort (field)
            );
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void BibTextField_ToString_1()
    {
        var field = new BibTexField();
        Assert.AreEqual ("(null)=(null)", field.ToString());

        field = _GetField();
        Assert.AreEqual ("booktitle=Программирование для ИРБИС64", field.ToString());
    }
}
