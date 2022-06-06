// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* DesktopIniSection.cs -- DESKTOP-секция INI-файла для клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// DESKTOP-секция INI-файла для клиента.
/// </summary>
/// <remarks>
/// Находится в клиентском INI-файле cirbisc.ini.
/// </remarks>
public sealed class DesktopIniSection
    : AbstractIniSection
{
    #region Constants

    /// <summary>
    /// Имя секции.
    /// </summary>
    public const string SectionName = "DESKTOP";

    #endregion

    #region Properties

    /// <summary>
    /// Использовать авто-сервис?
    /// </summary>
    [Description ("Автосервис")]
    public bool AutoService
    {
        get => GetBoolean ("AutoService", "1");
        set => SetBoolean ("AutoService", value);
    }

    /// <summary>
    /// Показываль панель контекста базы данных?
    /// </summary>
    [Description ("Показывать панель контекста?")]
    public bool DBContext
    {
        get => GetBoolean ("DBContext", "1");
        set => SetBoolean ("DBContext", value);
    }

    /// <summary>
    /// Панель контекста плавающая?
    /// </summary>
    [Description ("Панель контекста плавающая?")]
    public bool DBContextFloating
    {
        get => GetBoolean ("DBContextFloating", "0");
        set => SetBoolean ("DBContextFloating", value);
    }

    /// <summary>
    /// Показывать панель открытия базы данных?
    /// </summary>
    [Description ("Панель базы данных")]
    public bool DBOpen
    {
        get => GetBoolean ("DBOpen", "1");
        set => SetBoolean ("DBOpen", value);
    }

    /// <summary>
    /// Панель открытия базы данных плавающая?
    /// </summary>
    [Description ("Панель базы плавающая?")]
    public bool DBOpenFloating
    {
        get => GetBoolean ("DBOpenFloating", "0");
        set => SetBoolean ("DBOpenFloating", value);
    }

    /// <summary>
    /// Показывать панель ввода?
    /// </summary>
    [Description ("Показывать панель ввода?")]
    public bool Entry
    {
        get => GetBoolean ("Entry", "1");
        set => SetBoolean ("Entry", value);
    }

    /// <summary>
    /// Панель ввода плавающая?
    /// </summary>
    [Description ("Панель ввода плавающая?")]
    public bool EntryFloating
    {
        get => GetBoolean ("EntryFloating", "0");
        set => SetBoolean ("EntryFloating", value);
    }

    /// <summary>
    /// Показывать главное меню?
    /// </summary>
    [Description ("Показывать главное меню?")]
    public bool MainMenu
    {
        get => GetBoolean ("MainMenu", "1");
        set => SetBoolean ("MainMenu", value);
    }

    /// <summary>
    /// Главное меню плавающее?
    /// </summary>
    [Description ("Главное меню плавающее?")]
    public bool MainMenuFloating
    {
        get => GetBoolean ("MainMenuFloating", "0");
        set => SetBoolean ("MainMenuFloating", value);
    }

    /// <summary>
    /// Показывать панель поиска?
    /// </summary>
    [Description ("Показывать панель поиска?")]
    public bool Search
    {
        get => GetBoolean ("Search", "1");
        set => SetBoolean ("Search", value);
    }

    /// <summary>
    /// Панель поиска плавающая?
    /// </summary>
    [Description ("Панель поиска плавающая?")]
    public bool SearchFloating
    {
        get => GetBoolean ("SearchFloating", "0");
        set => SetBoolean ("SearchFloating", value);
    }

    /// <summary>
    /// Проверка орфографии.
    /// </summary>
    [Description ("Проверка орфографии")]
    public bool Spelling
    {
        get => GetBoolean ("Spelling", "1");
        set => SetBoolean ("Spelling", value);
    }

    /// <summary>
    /// Показывать панель пользовательских режимов?
    /// </summary>
    [Description ("Панель пользовательских режимов?")]
    public bool UserMode
    {
        get => GetBoolean ("UserMode", "1");
        set => SetBoolean ("UserMode", value);
    }

    /// <summary>
    /// Панель пользовательских режимов плавающая?
    /// </summary>
    [Description ("Плавающая панель?")]
    public bool UserModeFloating
    {
        get => GetBoolean ("UserModeFloating", "0");
        set => SetBoolean ("UserModeFloating", value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DesktopIniSection()
        : base (new IniFile(), SectionName)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DesktopIniSection
        (
            IniFile iniFile
        )
        : base (iniFile, SectionName)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DesktopIniSection
        (
            IniFile.Section section
        )
        : base (section)
    {
        // пустое тело конструктора
    }

    #endregion
}
