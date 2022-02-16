// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FieldDescriptor.cs -- описание поля таблицы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Data;

/// <summary>
/// Описание поля таблицы
/// </summary>
[XmlRoot ("field")]
public sealed class FieldDescriptor
    : IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя поля в классе, имя колонки в таблице.
    /// </summary>
    [XmlAttribute ("name")]
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Тип данных.
    /// </summary>
    [XmlAttribute ("type")]
    [JsonPropertyName ("type")]
    public DataType Type { get; set; }

    /// <summary>
    /// Длина поля (для типов, где это имеет смысл).
    /// </summary>
    [XmlAttribute ("length")]
    [JsonPropertyName ("length")]
    public int Length { get; set; }

    /// <summary>
    /// Может содержать значение <c>null</c>?
    /// </summary>
    [XmlAttribute ("nullable")]
    [JsonPropertyName ("nullable")]
    public bool Nullable { get; set; }

    /// <summary>
    /// Идентичность?
    /// </summary>
    [XmlAttribute ("identity")]
    [JsonPropertyName ("identity")]
    public bool Identity { get; set; }

    /// <summary>
    /// Имя, отображаемое в пользовательском интерфейсе.
    /// </summary>
    [XmlAttribute ("display")]
    [JsonPropertyName ("display")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Подробное описание, отображаемое в пользовательском интерфейсе.
    /// </summary>
    [XmlAttribute ("description")]
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Порядковый номер.
    /// </summary>
    [XmlAttribute ("order")]
    [JsonPropertyName ("order")]
    public int Order { get; set; }

    /// <summary>
    /// Скрыт (не отображается) в пользовательском интерфейсе.
    /// </summary>
    [XmlAttribute ("hidden")]
    [JsonPropertyName ("hidden")]
    public bool Hidden { get; set; }

    /// <summary>
    /// Ширина колонки в пользовательском интерфейсе.
    /// </summary>
    [XmlAttribute ("width")]
    [JsonPropertyName ("width")]
    public int ColumnWidth { get; set; }

    /// <summary>
    /// Запрещено редактировать в пользовательском интерфейсе.
    /// </summary>
    [XmlAttribute ("readonly")]
    [JsonPropertyName ("readonly")]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Представляет собой первичный ключ для таблицы?
    /// </summary>
    [XmlAttribute ("primaryKey")]
    [JsonPropertyName ("primaryKey")]
    public bool PrimaryKey { get; set; }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<FieldDescriptor> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Name)
            .Assert (Type != DataType.None);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{Type} {Name}";
    }

    #endregion
}
