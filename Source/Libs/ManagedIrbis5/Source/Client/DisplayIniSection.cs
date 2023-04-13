// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DisplayIniSection.cs -- DISPLAY-секция INI-файла для клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

using AM.IO;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// DISPLAY-секция INI-файла для клиента.
/// </summary>
/// <remarks>
/// Находится в серверном INI-файле irbisc.ini.
/// </remarks>
[PublicAPI]
public sealed class DisplayIniSection
    : AbstractIniSection
{
    #region Constants

    /// <summary>
    /// Имя секции.
    /// </summary>
    public const string SectionName = "Display";

    #endregion

    #region Properties

    /// <summary>
    /// Размер порции для показа кратких описаний.
    /// </summary>
    [Description ("Размер порции")]
    public int MaxBriefPortion
    {
        get => Section.GetValue ("MaxBriefPortion", 6);
        set => Section.SetValue ("MaxBriefPortion", value);
    }

    /// <summary>
    /// Максимальное количество отмеченных документов.
    /// </summary>
    [Description ("Максимальное количество отмеченных")]
    public int MaxMarked
    {
        get => Section.GetValue ("MaxMarked", 100);
        set => Section.SetValue ("MaxMarked", value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DisplayIniSection()
        : base (SectionName)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DisplayIniSection
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
    public DisplayIniSection
        (
            IniFile.Section section
        )
        : base (section)
    {
        // пустое тело конструктора
    }

    #endregion
}
