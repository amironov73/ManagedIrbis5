// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DataColumnInfo.cs -- сведения о колонке таблицы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.Data;

/// <summary>
/// Сведения о колонке таблицы с данными.
/// </summary>
[Serializable]
[XmlRoot ("column")]
public sealed class DataColumnInfo
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя колонки, используемое в таблице.
    /// </summary>
    [DefaultValue (null)]
    [JsonPropertyName ("name")]
    [XmlAttribute ("name")]
    [DisplayName ("Имя")]
    [Description ("Имя, используемое в таблице")]
    public string? Name { get; set; }

    /// <summary>
    /// Заглавие, отображаемое в пользовательском интерфейсе.
    /// </summary>
    [DefaultValue (null)]
    [JsonPropertyName ("title")]
    [XmlAttribute ("title")]
    [DisplayName ("Заглавие")]
    [Description ("Заглавие, отображаемое в пользовательском интерфейсе")]
    public string? Title { get; set; }

    /// <summary>
    /// Ширина, используемая при отображении в пользовательском интерфейсе.
    /// </summary>
    [DefaultValue (0)]
    [XmlAttribute ("width")]
    [JsonPropertyName ("width")]
    [DisplayName ("Ширина")]
    [Description ("Ширина, используемая при отображении в пользовательском интерфейсе")]
    public int Width { get; set; }

    /// <summary>
    /// Тип данных в колонке.
    /// </summary>
    [DefaultValue (null)]
    [JsonPropertyName ("type")]
    [XmlAttribute ("type")]
    [DisplayName ("Тип данных")]
    [Description ("Тип данных в колонке")]
    public string? Type { get; set; }

    /// <summary>
    /// Значение по умолчанию.
    /// </summary>
    [DefaultValue (null)]
    [XmlAttribute ("defaultValue")]
    [JsonPropertyName ("defaultValue")]
    [DisplayName ("Значение по умолчанию")]
    [Description ("Значение по умолчанию")]
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Замороженная колонка?
    /// </summary>
    [DefaultValue (false)]
    [XmlAttribute ("frozen")]
    [JsonPropertyName ("frozen")]
    [DisplayName ("Заморозка")]
    [Description ("Замороженная колонка?")]
    public bool Frozen { get; set; }

    /// <summary>
    /// Скрытая колонка?
    /// </summary>
    [DefaultValue (false)]
    [XmlAttribute ("invisible")]
    [JsonPropertyName ("invisible")]
    [DisplayName ("Скрытая")]
    [Description ("Скрытая колонка?")]
    public bool Invisible { get; set; }

    /// <summary>
    /// Колонка только для чтения.
    /// </summary>
    [DefaultValue (false)]
    [XmlAttribute ("readOnly")]
    [JsonPropertyName ("readOnly")]
    [DisplayName ("Только для чтения")]
    [Description ("Колонка для чтения")]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Данные в таблице отсортированы по этой колоке?
    /// </summary>
    [DefaultValue (false)]
    [XmlAttribute ("sorted")]
    [JsonPropertyName ("sorted")]
    [DisplayName ("Сортировка")]
    [Description ("Данные отсортированы по этой колонке?")]
    public bool Sorted { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public object? UserData { get; set; }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Name = reader.ReadNullableString();
        Title = reader.ReadNullableString();
        Width = reader.ReadPackedInt32();
        Type = reader.ReadNullableString();
        DefaultValue = reader.ReadNullableString();
        Frozen = reader.ReadBoolean();
        Invisible = reader.ReadBoolean();
        ReadOnly = reader.ReadBoolean();
        Sorted = reader.ReadBoolean();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Name)
            .WriteNullable (Title)
            .WritePackedInt32 (Width)
            .WriteNullable (Type)
            .WriteNullable (DefaultValue);
        writer.Write (Frozen);
        writer.Write (Invisible);
        writer.Write (ReadOnly);
        writer.Write (Sorted);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<DataColumnInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Name);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Name.ToVisibleString();
    }

    #endregion
}
