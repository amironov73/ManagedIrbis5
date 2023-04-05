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
public sealed class BibTexRecordTest
{
    private BibTexRecord _GetRecord()
    {
        return new BibTexRecord
        {
            Type = RecordType.Book,
            Tag = "Миронов2021",
            Fields =
            {
                new()
                {
                    Tag = KnownTags.BookTitle,
                    Value = "Программирование для ИРБИС64"
                },
                new()
                {
                    Tag = KnownTags.Year,
                    Value = "2021"
                },
                new()
                {
                    Tag = KnownTags.Edition,
                    Value = "5-е издание"
                }
            }
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

    private void _Compare
        (
            BibTexRecord first,
            BibTexRecord second
        )
    {
        Assert.AreEqual (first.Tag, second.Tag);
        Assert.AreEqual (first.Type, second.Type);
        Assert.AreEqual (first.Fields.Count, second.Fields.Count);
        for (var i = 0; i < first.Fields.Count; i++)
        {
            _Compare (first.Fields[i], second.Fields[i]);
        }
    }

    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void BibTexRecord_Construction_1()
    {
        var record = new BibTexRecord();
        Assert.IsNull (record.Type);
        Assert.IsNull (record.Tag);
        Assert.IsNull (record.UserData);
        Assert.IsNotNull (record.Fields);
        Assert.AreEqual (0, record.Fields.Count);
    }

    private void _Serialization
        (
            BibTexRecord first
        )
    {
        var memory = first.SaveToMemory();
        var second = memory.RestoreObjectFromMemory<BibTexRecord>();
        Assert.IsNotNull (second);
        _Compare (first, second);
        Assert.IsNull (second.UserData);
    }

    [TestMethod]
    [Description ("Сериализация")]
    public void BibTexRecord_Serialization_1()
    {
        var record = new BibTexRecord();
        _Serialization (record);

        record = _GetRecord();
        record.UserData = "User data";
        _Serialization (record);
    }

    [TestMethod]
    [Description ("Верификация")]
    public void BibTexRecord_Verification_1()
    {
        var record = new BibTexRecord();
        Assert.IsFalse (record.Verify (false));

        record = _GetRecord();
        Assert.IsTrue (record.Verify (false));
    }

    [TestMethod]
    [Description ("XML-представление")]
    public void BibTexRecord_ToXml_1()
    {
        var record = new BibTexRecord();
        Assert.AreEqual
            (
                "<record />",
                XmlUtility.SerializeShort (record)
            );

        record = _GetRecord();
        Assert.AreEqual
            (
                "<record type=\"book\" tag=\"Миронов2021\"><field tag=\"booktitle\" value=\"Программирование для ИРБИС64\" /><field tag=\"year\" value=\"2021\" /><field tag=\"edition\" value=\"5-е издание\" /></record>",
                XmlUtility.SerializeShort (record)
            );
    }

    [TestMethod]
    [Description ("JSON-представление")]
    public void BibTexRecord_ToJson_1()
    {
        var record = new BibTexRecord();
        Assert.AreEqual
            (
                "{\"fields\":[]}",
                JsonUtility.SerializeShort (record)
            );

        record = _GetRecord();
        Assert.AreEqual
            (
                "{\"type\":\"book\",\"tag\":\"\\u041C\\u0438\\u0440\\u043E\\u043D\\u043E\\u04322021\",\"fields\":[{\"tag\":\"booktitle\",\"value\":\"\\u041F\\u0440\\u043E\\u0433\\u0440\\u0430\\u043C\\u043C\\u0438\\u0440\\u043E\\u0432\\u0430\\u043D\\u0438\\u0435 \\u0434\\u043B\\u044F \\u0418\\u0420\\u0411\\u0418\\u042164\"},{\"tag\":\"year\",\"value\":\"2021\"},{\"tag\":\"edition\",\"value\":\"5-\\u0435 \\u0438\\u0437\\u0434\\u0430\\u043D\\u0438\\u0435\"}]}",
                JsonUtility.SerializeShort (record)
            );
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void BibTextRecord_ToString_1()
    {
        var record = new BibTexRecord();
        Assert.AreEqual ("(null): (null)", record.ToString());

        record = _GetRecord();
        Assert.AreEqual ("book: Миронов2021", record.ToString());
    }
}
