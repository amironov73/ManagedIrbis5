// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable MustUseReturnValue

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis;

[TestClass]
public sealed class ConnectionSettingsTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void ConnectionSettings_Construction_1()
    {
        var settings = new ConnectionSettings();

        Assert.AreEqual (ConnectionSettings.DefaultHost, settings.Host);
        Assert.AreEqual (ConnectionSettings.DefaultPort, settings.Port);
        Assert.AreEqual (string.Empty, settings.Username);
        Assert.AreEqual (string.Empty, settings.Password);
        Assert.IsNull (settings.Database);
        Assert.IsNull (settings.Workstation);
        Assert.IsNull (settings.NetworkLogging);
        Assert.IsNull (settings.SocketTypeName);
        Assert.IsFalse (settings.Debug);
        Assert.AreEqual (0, settings.RetryLimit);
        Assert.IsNull (settings.WebCgi);
        Assert.IsNull (settings.UserData);
    }

    [TestMethod]
    [Description ("Применение настроек по умолчанию")]
    public void ConnectionSettings_ApplyDefaults_1()
    {
        var settings = new ConnectionSettings
        {
            Workstation = null,
            Host = null,
            Port = 0
        };

        settings.ApplyDefaults();

        Assert.AreEqual (ConnectionSettings.DefaultWorkstation, settings.Workstation);
        Assert.AreEqual (ConnectionSettings.DefaultHost, settings.Host);
        Assert.AreEqual (ConnectionSettings.DefaultPort, settings.Port);
    }

    [TestMethod]
    [Description ("Клонирование")]
    public void ConnectionSettings_Clone_1()
    {
        var sourceSettings = new ConnectionSettings
        {
            Host = "HiddenHost",
            Port = 5555,
            Username = "librarian",
            Password = "secret",
            Workstation = "A",
            Database = "KZD",
            RetryLimit = 3
        };

        var clonedSettings = sourceSettings.Clone();

        Assert.AreEqual (sourceSettings.Host, clonedSettings.Host);
        Assert.AreEqual (sourceSettings.Port, clonedSettings.Port);
        Assert.AreEqual (sourceSettings.Username, clonedSettings.Username);
        Assert.AreEqual (sourceSettings.Password, clonedSettings.Password);
        Assert.AreEqual (sourceSettings.Workstation, clonedSettings.Workstation);
        Assert.AreEqual (sourceSettings.Database, clonedSettings.Database);
        Assert.AreEqual (sourceSettings.NetworkLogging, clonedSettings.NetworkLogging);
        Assert.AreEqual (sourceSettings.SocketTypeName, clonedSettings.SocketTypeName);
        Assert.AreEqual (sourceSettings.Debug, clonedSettings.Debug);
        Assert.AreEqual (sourceSettings.RetryLimit, clonedSettings.RetryLimit);
        Assert.AreEqual (sourceSettings.WebCgi, clonedSettings.WebCgi);
        Assert.AreEqual (sourceSettings.UserData, clonedSettings.UserData);
    }

    [TestMethod]
    [Description ("Простейшее кодирование настроек в текстовое представление")]
    public void ConnectionSettings_Encode_1()
    {
        var settings = new ConnectionSettings
        {
            Host = "HiddenHost",
            Port = 5555,
            Username = "librarian",
            Password = "secret",
            Workstation = "A",
            Database = "KZD",
            RetryLimit = 3
        };
        var encoded = settings.Encode();

        Assert.AreEqual
            (
                "host=HiddenHost;port=5555;database=KZD;username=librarian;password=secret;workstation=A;retry=3;",
                encoded
            );
    }

    [TestMethod]
    [Description ("Примитивное шифрование настроек")]
    public void ConnectionSettings_Encrypt_1()
    {
        var settings = new ConnectionSettings
        {
            Host = "HiddenHost",
            Port = 5555,
            Username = "librarian",
            Password = "secret",
            Workstation = "A",
            Database = "KZD",
            RetryLimit = 3
        };
        var encrypted = settings.Encrypt();

        Assert.AreEqual
            (
                "Y2Tk8shMSUnN88gvLtmszciZk5lUlFiUmZjHyFacmlyUWsLI7B3lwsjoyMDAzMAAAA==",
                encrypted
            );
    }
}
