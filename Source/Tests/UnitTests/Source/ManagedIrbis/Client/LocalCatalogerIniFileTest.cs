// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.IO;

using AM.IO;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Client;

[TestClass]
public sealed class LocalCatalogerIniFileTest
    : Common.CommonUnitTest
{
    private LocalCatalogerIniFile _GetFile()
    {
        var fileName = Path.Combine (Irbis64RootPath, "cirbisc.ini");
        var iniFile = new IniFile (fileName, IrbisEncoding.Ansi);
        var result = new LocalCatalogerIniFile (iniFile);

        return result;
    }
    
    [TestMethod]
    [Description ("Конструктор")]
    public void LocalCatalogerIniFile_Construction_1()
    {
        var file = new IniFile();
        var iniFile = new LocalCatalogerIniFile (file);
        Assert.IsNotNull (iniFile.Context);
        Assert.IsNotNull (iniFile.Ini);
        Assert.IsNotNull (iniFile.Main);
        Assert.AreSame (file, iniFile.Ini);
    }

    [TestMethod]
    [Description ("Получение IP-адреса сервера")]
    public void LocalCatalogerIniFile_ServerIP_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("127.0.0.1", file.ServerIP);
    }

    [TestMethod]
    [Description ("Получение IP-порта сервера")]
    public void LocalCatalogerIniFile_ServerPort_1()
    {
        var file = _GetFile();
        Assert.AreEqual (6666, file.ServerPort);
    }

    [TestMethod]
    [Description ("Получение организации, на которую приобретен ИРБИС")]
    public void LocalCatalogerIniFile_Organization_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("Иркутский государственный технический университет", file.Organization);
    }

    [TestMethod]
    [Description ("Получение логина для входа на сервер ИРБИС")]
    public void LocalCatalogerIniFile_UserName_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("miron", file.UserName);
    }

    [TestMethod]
    [Description ("Получение пароля для входа на сервер ИРБИС")]
    public void LocalCatalogerIniFile_UserPassword_1()
    {
        var file = _GetFile();
        Assert.IsNull (file.UserPassword);
    }

    [TestMethod]
    [Description ("Получение строки подключения к серверу ИРБИС")]
    public void LocalCatalogerIniFile_BuildConnectionString_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("host=127.0.0.1;port=6666;username=miron;", file.BuildConnectionString());
    }

    [TestMethod]
    [Description ("Получение произвольного значения")]
    public void LocalCatalogerIniFile_GetValue_1()
    {
        var file = _GetFile();
        Assert.AreEqual 
            (
                "Arial",
                file.GetValue (nameof (LocalCatalogerIniFile.Main), "FontName")
            );
    }

    [TestMethod]
    [Description ("Загрузка локального файла")]
    public void LocalCatalogerIniFile_Load_1()
    {
        var fileName = Path.Combine (Irbis64RootPath, "cirbisc.ini");
        var iniFile = LocalCatalogerIniFile.Load (fileName);
        Assert.IsNotNull (iniFile.Context);
        Assert.IsNotNull (iniFile.Ini);
        Assert.IsNotNull (iniFile.Main);
    }

}
