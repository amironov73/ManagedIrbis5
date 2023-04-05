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
public sealed class BibTexFileTest
{
    private BibTexFile _GetFile()
    {
        return new BibTexFile
        {
            Records =
            {
                new BibTexRecord
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
                },

                new BibTexRecord
                {
                    Type = RecordType.Book,
                    Tag = "Миронов2022",
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
                            Value = "2022"
                        },
                        new()
                        {
                            Tag = KnownTags.Edition,
                            Value = "6-е издание"
                        }
                    }
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

    private void _Compare
        (
            BibTexFile first,
            BibTexFile second
        )
    {

        Assert.AreEqual (first.Records.Count, second.Records.Count);
        for (var i = 0; i < first.Records.Count; i++)
        {
            _Compare (first.Records[i], second.Records[i]);
        }
    }

    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void BibTexFile_Construction_1()
    {
        var bibtex = new BibTexFile();
        Assert.IsNull (bibtex.UserData);
        Assert.IsNotNull (bibtex.Records);
        Assert.AreEqual (0, bibtex.Records.Count);
    }

    private void _Serialization
        (
            BibTexFile first
        )
    {
        var memory = first.SaveToMemory();
        var second = memory.RestoreObjectFromMemory<BibTexFile>();
        Assert.IsNotNull (second);
        _Compare (first, second);
        Assert.IsNull (second.UserData);
    }

    [TestMethod]
    [Description ("Сериализация")]
    public void BibTexFile_Serialization_1()
    {
        var bibtex = new BibTexFile();
        _Serialization (bibtex);

        bibtex = _GetFile();
        bibtex.UserData = "User data";
        _Serialization (bibtex);
    }

    [TestMethod]
    [Description ("Верификация")]
    public void BibTexFile_Verification_1()
    {
        var bibtex = new BibTexFile();
        Assert.IsFalse (bibtex.Verify (false));

        bibtex = _GetFile();
        Assert.IsTrue (bibtex.Verify (false));
    }

    [TestMethod]
    [Description ("XML-представление")]
    public void BibTexFile_ToXml_1()
    {
        var bibtex = new BibTexFile();
        Assert.AreEqual
            (
                "<bibtex />",
                XmlUtility.SerializeShort (bibtex)
            );

        bibtex = _GetFile();
        Assert.AreEqual
            (
                "<bibtex><record type=\"book\" tag=\"Миронов2021\"><field tag=\"booktitle\" value=\"Программирование для ИРБИС64\" /><field tag=\"year\" value=\"2021\" /><field tag=\"edition\" value=\"5-е издание\" /></record><record type=\"book\" tag=\"Миронов2022\"><field tag=\"booktitle\" value=\"Программирование для ИРБИС64\" /><field tag=\"year\" value=\"2022\" /><field tag=\"edition\" value=\"6-е издание\" /></record></bibtex>",
                XmlUtility.SerializeShort (bibtex)
            );
    }

    [TestMethod]
    [Description ("JSON-представление")]
    public void BibTexFile_ToJson_1()
    {
        var bibtex = new BibTexFile();
        Assert.AreEqual
            (
                "{\"records\":[]}",
                JsonUtility.SerializeShort (bibtex)
            );

        bibtex = _GetFile();
        Assert.AreEqual
            (
                "{\"records\":[{\"type\":\"book\",\"tag\":\"\\u041C\\u0438\\u0440\\u043E\\u043D\\u043E\\u04322021\",\"fields\":[{\"tag\":\"booktitle\",\"value\":\"\\u041F\\u0440\\u043E\\u0433\\u0440\\u0430\\u043C\\u043C\\u0438\\u0440\\u043E\\u0432\\u0430\\u043D\\u0438\\u0435 \\u0434\\u043B\\u044F \\u0418\\u0420\\u0411\\u0418\\u042164\"},{\"tag\":\"year\",\"value\":\"2021\"},{\"tag\":\"edition\",\"value\":\"5-\\u0435 \\u0438\\u0437\\u0434\\u0430\\u043D\\u0438\\u0435\"}]},{\"type\":\"book\",\"tag\":\"\\u041C\\u0438\\u0440\\u043E\\u043D\\u043E\\u04322022\",\"fields\":[{\"tag\":\"booktitle\",\"value\":\"\\u041F\\u0440\\u043E\\u0433\\u0440\\u0430\\u043C\\u043C\\u0438\\u0440\\u043E\\u0432\\u0430\\u043D\\u0438\\u0435 \\u0434\\u043B\\u044F \\u0418\\u0420\\u0411\\u0418\\u042164\"},{\"tag\":\"year\",\"value\":\"2022\"},{\"tag\":\"edition\",\"value\":\"6-\\u0435 \\u0438\\u0437\\u0434\\u0430\\u043D\\u0438\\u0435\"}]}]}",
                JsonUtility.SerializeShort (bibtex)
            );
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void BibTextFile_ToString_1()
    {
        var bibtex = new BibTexFile();
        Assert.AreEqual ("Records: 0", bibtex.ToString());

        bibtex = _GetFile();
        Assert.AreEqual ("Records: 2", bibtex.ToString());
    }

}
