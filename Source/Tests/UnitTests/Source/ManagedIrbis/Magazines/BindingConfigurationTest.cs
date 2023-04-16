// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Magazines;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Magazines;

[TestClass]
public sealed class BindingConfigurationTest
    : Common.CommonUnitTest
{
    private BindingConfiguration _GetConfiguration() =>
        new ()
        {
            GoodWorksheet = new [] { "NJ" },
            BadPlace = new [] { "Ф403" }
        };

    private void _Compare
        (
            BindingConfiguration first,
            BindingConfiguration second
        )
    {
        Assert.IsTrue (first.GoodStatus is not null == second.GoodStatus is not null);
        if (first.GoodStatus is not null && second.GoodStatus is not null)
        {
            Assert.AreEqual (first.GoodStatus.Length, second.GoodStatus.Length);
            for (var i = 0; i < first.GoodStatus.Length; i++)
            {
                Assert.AreEqual (first.GoodStatus[i], second.GoodStatus[i]);
            }
        }

        Assert.IsTrue (first.GoodWorksheet is not null == second.GoodWorksheet is not null);
        if (first.GoodWorksheet is not null && second.GoodWorksheet is not null)
        {
            Assert.AreEqual (first.GoodWorksheet.Length, second.GoodWorksheet.Length);
            for (var i = 0; i < first.GoodWorksheet.Length; i++)
            {
                Assert.AreEqual (first.GoodWorksheet[i], second.GoodWorksheet[i]);
            }
        }

        Assert.IsTrue (first.BadPlace is not null == second.BadPlace is not null);
        if (first.BadPlace is not null && second.BadPlace is not null)
        {
            Assert.AreEqual (first.BadPlace.Length, second.BadPlace.Length);
            for (var i = 0; i < first.BadPlace.Length; i++)
            {
                Assert.AreEqual (first.BadPlace[i], second.BadPlace[i]);
            }
        }
    }

    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void BindingConfiguration_Construction_1()
    {
        var configuration = new BindingConfiguration();
        Assert.IsNotNull (configuration.GoodStatus);
        Assert.AreNotEqual (0, configuration.GoodStatus.Length);
        Assert.IsNotNull (configuration.GoodWorksheet);
        Assert.AreNotEqual (0, configuration.GoodWorksheet.Length);
        Assert.IsNull (configuration.BadPlace);
    }

    [TestMethod]
    [Description ("Присвоение")]
    public void BindingConfiguration_Construction_2()
    {
        var configuration = new BindingConfiguration
        {
            GoodStatus = new [] { "0", "1" },
            GoodWorksheet = new [] { "NJ", "NJK" },
            BadPlace = new [] { "Ф403", "Ф604" }
        };
        Assert.IsNotNull (configuration.GoodStatus);
        Assert.AreEqual (2, configuration.GoodStatus.Length);
        Assert.IsNotNull (configuration.GoodWorksheet);
        Assert.AreEqual (2, configuration.GoodWorksheet.Length);
        Assert.IsNotNull (configuration.BadPlace);
        Assert.AreEqual (2, configuration.BadPlace.Length);
    }

    [TestMethod]
    [Description ("Проверка места хранения")]
    public void BindingConfiguration_CheckPlace_1()
    {
        var configuration = _GetConfiguration();
        Assert.IsFalse (configuration.CheckPlace (null));
        Assert.IsFalse (configuration.CheckPlace (string.Empty));
        Assert.IsTrue (configuration.CheckPlace ("ФП"));
        Assert.IsFalse (configuration.CheckPlace ("Ф403"));
    }

    [TestMethod]
    [Description ("Проверка статуса экземпляра")]
    public void BindingConfiguration_CheckStatus_1()
    {
        var configuration = _GetConfiguration();
        Assert.IsFalse (configuration.CheckStatus (null));
        Assert.IsFalse (configuration.CheckStatus (string.Empty));
        Assert.IsTrue (configuration.CheckStatus ("0"));
        Assert.IsFalse (configuration.CheckStatus ("1"));
    }

    [TestMethod]
    [Description ("Проверка рабочего листа")]
    public void BindingConfiguration_CheckWorksheet_1()
    {
        var configuration = _GetConfiguration();
        Assert.IsFalse (configuration.CheckWorksheet (null));
        Assert.IsFalse (configuration.CheckWorksheet (string.Empty));
        Assert.IsTrue (configuration.CheckWorksheet ("NJ"));
        Assert.IsFalse (configuration.CheckWorksheet ("ASP"));
    }

    [TestMethod]
    [Description ("Получение конфигурации по умолчанию")]
    public void BindingConfiguration_GetDefault_1()
    {
        var configuration = BindingConfiguration.GetDefault();
        Assert.IsNotNull (configuration.GoodStatus);
        Assert.AreNotEqual (0, configuration.GoodStatus.Length);
        Assert.IsNotNull (configuration.GoodWorksheet);
        Assert.AreNotEqual (0, configuration.GoodWorksheet.Length);
        Assert.IsNull (configuration.BadPlace);
    }

    [TestMethod]
    [Description ("Чтение конфигурации из файла")]
    public void BindingConfiguration_LoadConfiguration_1()
    {
        var fileName = Path.Combine (TestDataPath, "binding-configuration.json");
        var configuration = BindingConfiguration.LoadConfiguration (fileName);
        Assert.IsNotNull (configuration);
        Assert.IsNotNull (configuration.GoodStatus);
        Assert.AreEqual (2, configuration.GoodStatus.Length);
        Assert.AreEqual ("0", configuration.GoodStatus[0]);
        Assert.AreEqual ("1", configuration.GoodStatus[1]);
        Assert.IsNotNull (configuration.GoodWorksheet);
        Assert.AreEqual (2, configuration.GoodWorksheet.Length);
        Assert.AreEqual ("NJ", configuration.GoodWorksheet[0]);
        Assert.AreEqual ("NJK", configuration.GoodWorksheet[1]);
        Assert.IsNotNull (configuration.BadPlace);
        Assert.AreEqual (2, configuration.BadPlace.Length);
        Assert.AreEqual ("Ф403", configuration.BadPlace[0]);
        Assert.AreEqual ("Ф603", configuration.BadPlace[1]);
    }

    [TestMethod]
    [Description ("Запись конфигурации в файл")]
    public void BindingConfiguration_SaveConfiguration_1()
    {
        var fileName = Path.GetTempFileName();
        var first = _GetConfiguration();
        first.SaveConfiguration (fileName);
        Assert.IsTrue (File.Exists (fileName));
        var second = BindingConfiguration.LoadConfiguration (fileName);
        _Compare (first, second);
    }

    private void _TestSerialization
        (
            BindingConfiguration first
        )
    {
        var memory = first.SaveToMemory();
        var second = memory.RestoreObjectFromMemory<BindingConfiguration>();
        Assert.IsNotNull (second);
        _Compare (first, second);
    }

    [TestMethod]
    [Description ("Сериализация")]
    public void BindingConfiguration_Serialization_1()
    {
        var configuration = new BindingConfiguration();
        _TestSerialization (configuration);

        configuration = _GetConfiguration();
        _TestSerialization (configuration);
    }

    [TestMethod]
    [Description ("Верификация")]
    public void BindingConfiguration_Verification_1()
    {
        var configuration = new BindingConfiguration();
        Assert.IsTrue (configuration.Verify (false));

        configuration = _GetConfiguration();
        Assert.IsTrue (configuration.Verify (false));
    }

    [TestMethod]
    [Description ("XML-представление")]
    public void BindingConfiguration_ToXml_1()
    {
        var configuration = new BindingConfiguration();
        Assert.AreEqual
            (
                "<binding><good-status>0</good-status><good-worksheet>NJ</good-worksheet></binding>",
                XmlUtility.SerializeShort (configuration)
            );

        configuration = _GetConfiguration();
        Assert.AreEqual
            (
                "<binding><good-status>0</good-status><good-worksheet>NJ</good-worksheet><BadPlace><bad-place>Ф403</bad-place></BadPlace></binding>",
                XmlUtility.SerializeShort (configuration)
            );
    }

    [TestMethod]
    [Description ("JSON-представление")]
    public void BindingConfiguration_ToJson_1()
    {
        var configuration = new BindingConfiguration();
        Assert.AreEqual
            (
                "{\"goodStatus\":[\"0\"],\"goodWorksheet\":[\"NJ\"]}",
                JsonUtility.SerializeShort (configuration)
            );

        configuration = _GetConfiguration();
        Assert.AreEqual
            (
                "{\"goodStatus\":[\"0\"],\"goodWorksheet\":[\"NJ\"],\"badPlace\":[\"\\u0424403\"]}",
                JsonUtility.SerializeShort (configuration)
            );
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void BindingConfiguration_ToString_1()
    {
        var configuration = new BindingConfiguration();
        Assert.AreEqual
            (
                "GoodStatus: [0], GoodWorksheet: [NJ], BadPlace: (null)",
                configuration.ToString()
            );

        configuration = _GetConfiguration();
        Assert.AreEqual
            (
                "GoodStatus: [0], GoodWorksheet: [NJ], BadPlace: [Ф403]",
                configuration.ToString()
            );
    }
}
