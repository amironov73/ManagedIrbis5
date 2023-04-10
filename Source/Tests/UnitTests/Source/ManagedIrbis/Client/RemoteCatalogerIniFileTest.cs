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
public sealed class RemoteCatalogerIniFileTest
    : Common.CommonUnitTest
{
    private RemoteCatalogerIniFile _GetFile()
    {
        var fileName = Path.Combine (Irbis64RootPath, "irbisc.ini");
        var iniFile = new IniFile (fileName, IrbisEncoding.Ansi);
        var result = new RemoteCatalogerIniFile (iniFile);

        return result;
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void RemoteCatalogerIniFile_Construction_1()
    {
        var iniFile = new IniFile();
        var file = new RemoteCatalogerIniFile (iniFile);
        Assert.AreSame (iniFile, file.Ini);
    }

    [TestMethod]
    [Description ("Получение значения AutoinFile")]
    public void RemoteCatalogerIniFile_AutoinFile_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("autoin.gbl", file.AutoinFile);
    }

    [TestMethod]
    [Description ("Получение значения AutoMerge")]
    public void RemoteCatalogerIniFile_AutoMerge_1()
    {
        var file = _GetFile();
        Assert.IsFalse (file.AutoMerge);
    }

    [TestMethod]
    [Description ("Получение значения BriefPft")]
    public void RemoteCatalogerIniFile_BriefPft_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("BRIEF", file.BriefPft);
    }

    [TestMethod]
    [Description ("Получение значения ClientTimeLive")]
    public void RemoteCatalogerIniFile_ClientTimeLive_1()
    {
        var file = _GetFile();
        Assert.AreEqual (15, file.ClientTimeLive);
    }

    [TestMethod]
    [Description ("Получение значения CopyMnu")]
    public void RemoteCatalogerIniFile_CopyMnu_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("FSTW.MNU", file.CopyMnu);
    }

    [TestMethod]
    [Description ("Получение значения CountTag")]
    public void RemoteCatalogerIniFile_CountTag_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("999", file.CountTag);
    }

    [TestMethod]
    [Description ("Получение значения DatabaseList")]
    public void RemoteCatalogerIniFile_DatabaseList_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("dbnam2.mnu", file.DatabaseList);
    }

    [TestMethod]
    [Description ("Получение значения DbnFlc")]
    public void RemoteCatalogerIniFile_DbnFlc_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("DBNFLC", file.DbnFlc);
    }

    [TestMethod]
    [Description ("Получение значения DefaultDbn")]
    public void RemoteCatalogerIniFile_DefaultDb_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("ISTU", file.DefaultDb);
    }

    [TestMethod]
    [Description ("Получение значения EmptyDbn")]
    public void RemoteCatalogerIniFile_EmptyDbn_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("BLANK", file.EmptyDbn);
    }

    [TestMethod]
    [Description ("Получение значения ExemplarTag")]
    public void RemoteCatalogerIniFile_ExemplarTag_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("910", file.ExemplarTag);
    }

    [TestMethod]
    [Description ("Получение значения ExportMenu")]
    public void RemoteCatalogerIniFile_ExportMenu_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("EXPORTW.MNU", file.ExportMenu);
    }

    [TestMethod]
    [Description ("Получение значения FormatMenu")]
    public void RemoteCatalogerIniFile_FormatMenu_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("FMT31.MNU", file.FormatMenu);
    }

    [TestMethod]
    [Description ("Получение значения HelpDbn")]
    public void RemoteCatalogerIniFile_HelpDbn_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("HELP", file.HelpDbn);
    }

    [TestMethod]
    [Description ("Получение значения ImportMenu")]
    public void RemoteCatalogerIniFile_ImportMenu_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("IMPORTW.MNU", file.ImportMenu);
    }

    [TestMethod]
    [Description ("Получение значения IndexPrefix")]
    public void RemoteCatalogerIniFile_IndexPrefix_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("I=", file.IndexPrefix);
    }

    [TestMethod]
    [Description ("Получение значения IndexTag")]
    public void RemoteCatalogerIniFile_IndexTag_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("903", file.IndexTag);
    }

    [TestMethod]
    [Description ("Получение значения IriMenu")]
    public void RemoteCatalogerIniFile_IriMenu_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("iri.mnu", file.IriMenu);
    }

    [TestMethod]
    [Description ("Получение значения MaxBriefPortion")]
    public void RemoteCatalogerIniFile_MaxBriefPortion_1()
    {
        var file = _GetFile();
        Assert.AreEqual (10, file.MaxBriefPortion);
    }

    [TestMethod]
    [Description ("Получение значения MaxMarked")]
    public void RemoteCatalogerIniFile_MaxMarked_1()
    {
        var file = _GetFile();
        Assert.AreEqual (10, file.MaxMarked);
    }

    [TestMethod]
    [Description ("Получение значения PftMenu")]
    public void RemoteCatalogerIniFile_PftMenu_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("PFTw.MNU", file.PftMenu);
    }

    [TestMethod]
    [Description ("Получение значения PftOpt")]
    public void RemoteCatalogerIniFile_PftOpt_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("PFTw_H.OPT", file.PftOpt);
    }

    [TestMethod]
    [Description ("Получение значения SearchIni")]
    public void RemoteCatalogerIniFile_SearchIni_1()
    {
        var file = _GetFile();
        Assert.AreEqual (string.Empty, file.SearchIni);
    }

    [TestMethod]
    [Description ("Получение значения SandardDbn")]
    public void RemoteCatalogerIniFile_StandardDbn_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("IBIS", file.StandardDbn);
    }

    [TestMethod]
    [Description ("Получение значения WorkDirectory")]
    public void RemoteCatalogerIniFile_WorkDirectory_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("c:\\irbiswrk\\", file.WorkDirectory);
    }

    [TestMethod]
    [Description ("Получение значения WsOpt")]
    public void RemoteCatalogerIniFile_WsOpt_1()
    {
        var file = _GetFile();
        Assert.AreEqual ("WS31.OPT", file.WsOpt);
    }

    [TestMethod]
    [Description ("Установка значения")]
    public void RemoteCatalogerIniFile_SetValue_1()
    {
        var iniFile = new IniFile();
        var file = new RemoteCatalogerIniFile (iniFile);
        const string main = RemoteCatalogerIniFile.Main;
        const string key = "PRMARCFORMAT";
        const string expected = "MARC";
        file.SetValue (main, key, expected);
        var actual = file.GetValue (main, key, "!");
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Установка значения")]
    public void RemoteCatalogerIniFile_SetValue_2()
    {
        var iniFile = new IniFile();
        var file = new RemoteCatalogerIniFile (iniFile);
        const string main = RemoteCatalogerIniFile.Main;
        const string key = "KKKFONTSIZE";
        const int expected = 12;
        file.SetValue (main, key, expected);
        var actual = file.GetValue (main, key, 0);
        Assert.AreEqual (expected, actual);
    }
}
