// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using ManagedIrbis.Workspace;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Worksheet;

[TestClass]
public sealed class WorksheetItemTest
    : Common.CommonUnitTest
{
    private WorksheetItem _GetItem()
    {
        return new ()
        {
            Tag = "A",
            Title = "НАЗВАНИЕ",
            Repeatable = false,
            Help = "0",
            EditMode = "2",
            InputInfo = ",T=,@!tmovzh"
        };
    }

    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void WorksheetItem_Construction_1()
    {
        var item = new WorksheetItem();
        Assert.IsNull (item.DefaultValue);
        Assert.IsNull (item.FormalVerification);
        Assert.IsNull (item.Help);
        Assert.IsNull (item.Hint);
        Assert.IsNull (item.EditMode);
        Assert.IsNull (item.InputInfo);
        Assert.IsFalse (item.Repeatable);
        Assert.IsNull (item.Reserved);
        Assert.IsNull (item.Tag);
        Assert.IsNull (item.Title);
        Assert.IsNull (item.UserData);
    }

    private void _TestSerialization
        (
            WorksheetItem first
        )
    {
        var bytes = first.SaveToMemory();
        var second = bytes.RestoreObjectFromMemory<WorksheetItem>();

        Assert.IsNotNull (second);
        Assert.AreEqual (first.DefaultValue, second.DefaultValue);
        Assert.AreEqual (first.FormalVerification, second.FormalVerification);
        Assert.AreEqual (first.Help, second.Help);
        Assert.AreEqual (first.Hint, second.Hint);
        Assert.AreEqual (first.InputInfo, second.InputInfo);
        Assert.AreEqual (first.EditMode, second.EditMode);
        Assert.AreEqual (first.Repeatable, second.Repeatable);
        Assert.AreEqual (first.Reserved, second.Reserved);
        Assert.AreEqual (first.Tag, second.Tag);
        Assert.AreEqual (first.Title, second.Title);
        Assert.IsNull (second.UserData);
    }

    [TestMethod]
    [Description ("Сериализация")]
    public void WorksheetItem_Serialization_1()
    {
        var item = new WorksheetItem();
        _TestSerialization (item);

        item.UserData = "User data";
        _TestSerialization (item);

        item = _GetItem();
        _TestSerialization (item);
    }

    [TestMethod]
    [Description ("Разбор потока символов")]
    public void WorksheetItem_ParseStream_1()
    {
        const string text = "A\r\nНАЗВАНИЕ \r\n0\r\n0\r\n2\r\n,T=,@!tmovzh\r\n\r\n      \r\n\r\n\r\n";
        using var reader = new StringReader (text);
        var item = WorksheetItem.ParseStream (reader);
        Assert.AreEqual ("A", item.Tag);
        Assert.AreEqual ("НАЗВАНИЕ", item.Title);
        Assert.IsFalse (item.Repeatable);
        Assert.AreEqual ("0", item.Help);
        Assert.AreEqual ("2", item.EditMode);
        Assert.AreEqual (",T=,@!tmovzh", item.InputInfo);
        Assert.IsNull (item.FormalVerification);
        Assert.IsNull (item.Hint);
        Assert.IsNull (item.DefaultValue);
        Assert.IsNull (item.Reserved);
        Assert.IsNull (item.UserData);
    }

    [TestMethod]
    [Description ("Кодирование в поток символов")]
    public void WorksheetItem_Encode_1()
    {
        var item = _GetItem();
        using var writer = new StringWriter();
        item.Encode (writer);
        Assert.AreEqual
            (
                "A\nНАЗВАНИЕ\n0\n0\n2\n,T=,@!tmovzh\n\n\n\n\n",
                writer.ToString().DosToUnix()
            );
    }

    [TestMethod]
    [Description ("Верификация")]
    public void WorksheetItem_Verify_1()
    {
        var item = new WorksheetItem();
        Assert.IsFalse (item.Verify (false));

        item = _GetItem();
        Assert.IsTrue (item.Verify (false));
    }

    [TestMethod]
    [Description ("XML-представление")]
    public void WorksheetItem_ToXml_1()
    {
        var item = new WorksheetItem();
        Assert.AreEqual
            (
                "<line />",
                XmlUtility.SerializeShort (item)
            );

        item = _GetItem();
        Assert.AreEqual
            (
                "<line><tag>A</tag><title>НАЗВАНИЕ</title><help>0</help><input-mode>2</input-mode><input-info>,T=,@!tmovzh</input-info></line>",
                XmlUtility.SerializeShort (item)
            );
    }

    [Ignore]
    [TestMethod]
    [Description ("JSON-представление")]
    public void WorksheetItem_ToJson_1()
    {
        var item = new WorksheetItem();
        Assert.AreEqual
            (
                "{}",
                JsonUtility.SerializeShort (item)
            );

        item = _GetItem();
        Assert.AreEqual
            (
                "{'tag':'A','title':'НАЗВАНИЕ','help':'0','input-mode':'2','input-info':',T=,@!tmovzh'}",
                JsonUtility.SerializeShort (item)
            );
    }

    [TestMethod]
    [Description ("Представление в виде плоского текста")]
    public void WorksheetItem_ToString_1()
    {
        var item = new WorksheetItem();
        Assert.AreEqual
            (
                "(null): (null) [False][(null)]",
                item.ToString().DosToUnix()
            );

        item = _GetItem();
        Assert.AreEqual
            (
                "A: НАЗВАНИЕ [False][2]",
                item.ToString().DosToUnix()
            );
    }
}
