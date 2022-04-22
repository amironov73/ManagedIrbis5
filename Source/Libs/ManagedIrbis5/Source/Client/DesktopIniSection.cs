// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DesktopIniSection.cs -- DESKTOP-секция INI-файла для клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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
    /// Use auto service?
    /// </summary>
    public bool AutoService
    {
        get => GetBoolean("AutoService", "1");
        set => SetBoolean("AutoService", value);
    }

    /// <summary>
    /// Show database context panel?
    /// </summary>
    public bool DBContext
    {
        get => GetBoolean("DBContext", "1");
        set => SetBoolean("DBContext", value);
    }

    /// <summary>
    /// Database context panel is floating?
    /// </summary>
    public bool DBContextFloating
    {
        get => GetBoolean("DBContextFloating", "0");
        set => SetBoolean("DBContextFloating", value);
    }

    /// <summary>
    /// DBOpen panel visible?
    /// </summary>
    public bool DBOpen
    {
        get => GetBoolean("DBOpen", "1");
        set => SetBoolean("DBOpen", value);
    }

    /// <summary>
    /// Whether DBOpen panel is floating?
    /// </summary>
    public bool DBOpenFloating
    {
        get => GetBoolean("DBOpenFloating", "0");
        set => SetBoolean("DBOpenFloating", value);
    }

    /// <summary>
    /// Show the entry panel?
    /// </summary>
    public bool Entry
    {
        get => GetBoolean("Entry", "1");
        set => SetBoolean("Entry", value);
    }

    /// <summary>
    /// Entry panel is floating?
    /// </summary>
    public bool EntryFloating
    {
        get => GetBoolean("EntryFloating", "0");
        set => SetBoolean("EntryFloating", value);
    }

    /// <summary>
    /// Show then main menu?
    /// </summary>
    public bool MainMenu
    {
        get => GetBoolean("MainMenu", "1");
        set => SetBoolean("MainMenu", value);
    }

    /// <summary>
    /// Main menu is floating panel?
    /// </summary>
    public bool MainMenuFloating
    {
        get => GetBoolean("MainMenuFloating", "0");
        set => SetBoolean("MainMenuFloating", value);
    }

    /// <summary>
    /// Show the search panel?
    /// </summary>
    public bool Search
    {
        get => GetBoolean("Search", "1");
        set => SetBoolean("Search", value);
    }

    /// <summary>
    /// Whether the search panel is floating?
    /// </summary>
    public bool SearchFloating
    {
        get => GetBoolean("SearchFloating", "0");
        set => SetBoolean("SearchFloating", value);
    }

    /// <summary>
    /// Use spelling engine?
    /// </summary>
    public bool Spelling
    {
        get => GetBoolean("Spelling", "1");
        set => SetBoolean("Spelling", value);
    }

    /// <summary>
    /// Show the user mode panel?
    /// </summary>
    public bool UserMode
    {
        get => GetBoolean("UserMode", "1");
        set => SetBoolean("UserMode", value);
    }

    /// <summary>
    /// Whether the user mode panel is floating?
    /// </summary>
    public bool UserModeFloating
    {
        get => GetBoolean("UserModeFloating", "0");
        set => SetBoolean("UserModeFloating", value);
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
        : base(iniFile, SectionName)
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
        : base(section)
    {
        // пустое тело конструктора
    }

    #endregion
}
