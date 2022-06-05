// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ContextIniSection.cs -- CONTEXT-секция INI-файла для клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// CONTEXT-секция INI-файла для клиента.
/// </summary>
public sealed class ContextIniSection
    : AbstractIniSection
{
    #region Constants

    /// <summary>
    /// Имя секции INI-файла.
    /// </summary>
    public const string SectionName = "CONTEXT";

    #endregion

    #region Properties

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    public string? Database
    {
        get => Section["DBN"];
        set => Section["DBN"] = value;
    }

    /// <summary>
    /// Имя формата отображения.
    /// </summary>
    public string? DisplayFormat
    {
        get => Section["PFT"];
        set => Section["PFT"] = value;
    }

    /// <summary>
    /// Текущий MFN.
    /// </summary>
    public int Mfn
    {
        get => Section.GetValue ("CURMFN", 0);
        set => Section.SetValue ("CURMFN", value);
    }

    /// <summary>
    /// Пароль.
    /// </summary>
    public string? Password
    {
        get => Section["UserPassword"] ?? Section["Password"];
        set => Section["UserPassword"] = value;
    }

    /// <summary>
    /// Текст запроса..
    /// </summary>
    public string? Query
    {
        // TODO использовать UTF8

        get => Section["QUERY"];
        set => Section["QUERY"] = value;
    }

    /// <summary>
    /// Поисковый префикс.
    /// </summary>
    public string? SearchPrefix
    {
        get => Section["PREFIX"];
        set => Section["PREFIX"] = value;
    }

    /// <summary>
    /// Имя пользователя (логин).
    /// </summary>
    public string? UserName
    {
        get => Section["UserName"];
        set => Section["UserName"] = value;
    }

    /// <summary>
    /// Код рабочего листа.
    /// </summary>
    public string? Worksheet
    {
        get => Section["WS"];
        set => Section["WS"] = value;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ContextIniSection()
        : base (SectionName)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ContextIniSection
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
    public ContextIniSection
        (
            IniFile.Section section
        )
        : base (section)
    {
        // пустое тело конструктора
    }

    #endregion
}
