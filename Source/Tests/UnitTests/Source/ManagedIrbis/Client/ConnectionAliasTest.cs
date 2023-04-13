// ReSharper disable IdentifierTypo// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Client;

[TestClass]
public class ConnectionAliasTest
{
    private ConnectionAlias _GetAlias() => new ()
        {
            Name = "Name",
            Value = "Value"
        };

    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void ConnectionAlias_Construction_1()
    {
        var alias = new ConnectionAlias();
        Assert.IsNull (alias.Name);
        Assert.IsNull (alias.Value);
    }

    private void _TestSerialization
        (
            ConnectionAlias first
        )
    {
        var bytes = first.SaveToMemory();
        var second = bytes.RestoreObjectFromMemory<ConnectionAlias>();
        Assert.IsNotNull (second);
        Assert.AreEqual (first.Name, second.Name);
        Assert.AreEqual (first.Value, second.Value);
    }

    [TestMethod]
    [Description ("Сериализация")]
    public void ConnectionAlias_Serialization_1()
    {
        var alias = new ConnectionAlias();
        _TestSerialization (alias);

        alias = _GetAlias();
        _TestSerialization (alias);
    }

    [TestMethod]
    [Description ("Верификация")]
    public void ConnectionAlias_Verify_1()
    {
        var alias = new ConnectionAlias();
        Assert.IsFalse (alias.Verify (false));

        alias = _GetAlias();
        Assert.IsTrue (alias.Verify (false));
    }

    [TestMethod]
    [Description ("XML-представление")]
    public void ConnectionAlias_ToXml_1()
    {
        var alias = new ConnectionAlias();
        Assert.AreEqual ("<alias />", XmlUtility.SerializeShort (alias));

        alias = _GetAlias();
        Assert.AreEqual ("<alias name=\"Name\" value=\"Value\" />", XmlUtility.SerializeShort (alias));
    }

    [TestMethod]
    [Description ("JSON-представление")]
    public void ConnectionAlias_ToJson_1()
    {
        var alias = new ConnectionAlias();
        Assert.AreEqual ("{}", JsonUtility.SerializeShort (alias));

        alias = _GetAlias();
        Assert.AreEqual ("{\"name\":\"Name\",\"value\":\"Value\"}", JsonUtility.SerializeShort (alias));
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void ConnectionAlias_ToString_1()
    {
        var alias = new ConnectionAlias();
        Assert.AreEqual ("(null)=(null)", alias.ToString());

        alias = _GetAlias();
        Assert.AreEqual ("Name=Value", alias.ToString());
    }
}
