// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* LocalCatalogerIniFile.cs -- локальный INI-файл для АРМ Каталогизатор (cirbisc.ini)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

using AM;
using AM.IO;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// Локальный INI-файл для АРМ Каталогизатор (cirbisc.ini).
/// </summary>
[PublicAPI]
public sealed class LocalCatalogerIniFile
{
    #region Properties

    /// <summary>
    /// INI-файл.
    /// </summary>
    [Browsable (false)]
    public IniFile Ini { get; private set; }

    /// <summary>
    /// Секция <c>[Context]</c>.
    /// </summary>
    [Description ("Секция [Context]")]
    public ContextIniSection Context { get; }

    /// <summary>
    /// Секция <c>[Desktop]</c>.
    /// </summary>
    [Description ("Секция [Desktop]")]
    public DesktopIniSection Desktop { get; }

    /// <summary>
    /// Секция <c>[Magna]</c> (наша).
    /// </summary>
    [Description ("Секция [Magna]")]
    public IniFile.Section MagnaSection =>
        Ini.GetOrCreateSection ("Magna");

    /// <summary>
    /// Секция <c>[Main]</c>.
    /// </summary>
    [Description ("Секция [Main]")]
    public IniFile.Section Main =>
        Ini.GetOrCreateSection ("Main");

    /// <summary>
    /// Организация, на которую куплен ИРБИС.
    /// </summary>
    [Description ("Организация")]
    public string? Organization => TextUtility.FixEncoding 
        (
            Main["User"],
            IrbisEncoding.Ansi,
            IrbisEncoding.Utf8
        );

    /// <summary>
    /// IP-адрес ИРБИС-сервера.
    /// </summary>
    [Description ("IP-адрес ИРБИС-сервера")]
    public string ServerIP => Main["ServerIP"] ?? "127.0.0.1";

    /// <summary>
    /// Номер порта, на котором ИРБИС-сервер ожидает подключения.
    /// </summary>
    [Description ("Номер порта")]
    public ushort ServerPort => Convert.ToUInt16 (Main["ServerPort"] ?? "6666");

    /// <summary>
    /// Логин, используемый пользователем.
    /// </summary>
    [Description ("Логин")]
    public string? UserName
    {
        get
        {
            var result = Context.UserName
                ?? MagnaSection[nameof (UserName)];
            if (!string.IsNullOrEmpty (result))
            {
                result = IrbisUtility.Decrypt (result);
            }

            return result;
        }
    }

    /// <summary>
    /// Пароль для автоматического входа на сервер.
    /// </summary>
    [Description ("Пароль")]
    public string? UserPassword
    {
        get
        {
            var result = Context.Password
                ?? MagnaSection[nameof (UserPassword)];
            if (!string.IsNullOrEmpty (result))
            {
                result = IrbisUtility.Decrypt (result);
            }

            return result;
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LocalCatalogerIniFile
        (
            IniFile iniFile
        )
    {
        Sure.NotNull (iniFile);

        Ini = iniFile;
        Context = new ContextIniSection (iniFile);
        Desktop = new DesktopIniSection (iniFile);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение строки подключения по хранящимся
    /// в INI-файле настройкам.
    /// </summary>
    public string BuildConnectionString()
    {
        var settings = new ConnectionSettings
        {
            Host = ServerIP,
            Port = ServerPort,
            Username = UserName.EmptyToNull() ?? string.Empty,
            Password = UserPassword.EmptyToNull() ?? string.Empty
        };

        return settings.ToString();
    }

    /// <summary>
    /// Получение значения из указанных секции и ключа.
    /// </summary>
    public string? GetValue
        (
            string sectionName,
            string keyName,
            string? defaultValue = null
        )
    {
        Sure.NotNullNorEmpty (sectionName);
        Sure.NotNullNorEmpty (keyName);

        var result = Ini.GetValue
            (
                sectionName,
                keyName,
                defaultValue
            );

        return result;
    }

    /// <summary>
    /// Загрузка из указанного локального файла.
    /// </summary>
    public static LocalCatalogerIniFile Load
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var iniFile = new IniFile();
        iniFile.Read (fileName, IrbisEncoding.Ansi);
        var result = new LocalCatalogerIniFile (iniFile);

        return result;
    }

    #endregion
}
