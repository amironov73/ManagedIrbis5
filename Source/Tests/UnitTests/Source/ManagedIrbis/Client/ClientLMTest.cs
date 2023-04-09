// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

#region Using directives

using System.Text;

using AM.IO;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Client;

[TestClass]
public class ClientLMTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void ClientLM_Construction_1()
    {
        var manager = new ClientLM();
        Assert.AreEqual (ClientLM.DefaultSalt, manager.Salt);
        Assert.AreSame (IrbisEncoding.Ansi, manager.Encoding);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void ClientLM_Construction_2()
    {
        var salt = "Salt";
        var encoding = Encoding.ASCII;
        var manager = new ClientLM (encoding, salt);
        Assert.AreSame (salt, manager.Salt);
        Assert.AreSame (encoding, manager.Encoding);
    }

    [TestMethod]
    [Description ("Вычисление хеша для имени пользователя")]
    public void ClientLM_ComputeHash_1()
    {
        var manager = new ClientLM();
        var actual = manager
            .ComputeHash ("Иркутский государственный технический университет");
        var expected = "\x040E\x00A0\x00A0\x040E\x045B";
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Вычисление хеша для INI-файла")]
    public void ClientLM_ComputeHash_2()
    {
        var user = "Иркутский государственный технический университет";
        var iniFile = new IniFile();
        var section = iniFile.CreateSection ("Main");
        section["User"] = user;

        var manager = new ClientLM();
        section["Common"] = manager.ComputeHash (user);

        Assert.IsTrue (manager.CheckHash (iniFile));
    }

    [TestMethod]
    [Description ("Проверка хеша для INI-файла")]
    public void ClientLM_CheckHash_1()
    {
        var iniFile = new IniFile();
        var section = iniFile.CreateSection ("Main");
        section["User"] = "Иркутский государственный технический университет";
        section["Common"] = "\x040E\x00A0\x00A0\x040E\x045B";

        var manager = new ClientLM();
        Assert.IsTrue (manager.CheckHash (iniFile));
    }

    [TestMethod]
    [Description ("Проверка хеша для INI-файла")]
    public void ClientLM_CheckHash_2()
    {
        var iniFile = new IniFile();
        var section = iniFile.CreateSection ("Main");
        section["User"] = "Иркутский государственный технический университет";
        section["Common"] = "\x040E\x00A0\x00A0\x040E\x045C";

        var manager = new ClientLM();
        Assert.IsFalse (manager.CheckHash (iniFile));
    }

    [TestMethod]
    [Description ("Проверка хеша для INI-файла")]
    public void ClientLM_CheckHash_3()
    {
        var iniFile = new IniFile();
        var section = iniFile.CreateSection ("Main");
        section["User"] = "Иркутский государственный технический университет";

        var manager = new ClientLM();
        Assert.IsFalse (manager.CheckHash (iniFile));
    }
}
