// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EntryIniSection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
///
/// </summary>
/// <remarks>
/// Находится в серверном INI-файле irbisc.ini.
/// </remarks>
public sealed class EntryIniSection
    : AbstractIniSection
{
    #region Constants

    /// <summary>
    /// Имя секции.
    /// </summary>
    public const string SectionName = "Entry";

    #endregion

    #region Properties

    /// <summary>
    /// Имя формата для ФЛК документа в целом.
    /// </summary>
    [XmlElement ("dbnflc")]
    [JsonPropertyName ("dbnflc")]
    public string? DbnFlc
    {
        get => Section.GetValue ("DBNFLC", "DBNFLC");
        set => Section["DBNFLC"] = value;
    }

    /// <summary>
    /// DefFieldNumb.
    /// </summary>
    [XmlElement ("DefFieldNumb")]
    [JsonPropertyName ("DefFieldNumb")]
    public int DefFieldNumb
    {
        get => Section.GetValue ("DefFieldNumb", 10);
        set => Section.SetValue ("DefFieldNumb", value);
    }

    /// <summary>
    /// MaxAddFields.
    /// </summary>
    [XmlElement ("MaxAddFields")]
    [JsonPropertyName ("MaxAddFields")]
    public int MaxAddFields
    {
        get => Section.GetValue ("MaxAddFields", 10);
        set => Section.SetValue ("MaxAddFields", value);
    }

    /// <summary>
    /// Признак автоматической актуализации записей
    /// при корректировке.
    /// </summary>
    public bool RecordUpdate
    {
        get => Utility.ToBoolean (Section.GetValue ("RECUPDIF", "1").ThrowIfNull());
        set => Section.SetValue ("RECUPDIF", value ? "1" : "0");
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public EntryIniSection()
        : base (SectionName)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EntryIniSection
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
    public EntryIniSection
        (
            IniFile.Section section
        )
        : base (section)
    {
        // пустое тело конструктора
    }

    #endregion
}
