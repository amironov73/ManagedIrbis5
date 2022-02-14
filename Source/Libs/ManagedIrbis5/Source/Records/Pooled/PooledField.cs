// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* PooledField.cs -- поле библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/// <summary>
/// Поле библиографической записи, адаптированное для пулинга.
/// </summary>
[XmlRoot ("field")]
public sealed class PooledField
{
    #region Properties

    /// <summary>
    /// Метка поля.
    /// </summary>
    [XmlAttribute ("tag")]
    [JsonPropertyName ("tag")]
    public int Tag { get; set; }

    /// <summary>
    /// Список подполей.
    /// </summary>
    [XmlArrayItem ("subfield")]
    [JsonPropertyName ("subfields")]
    public List<PooledSubField> Subfields { get; private set; } = default!;

    #endregion

    #region Public methods

    /// <summary>
    /// Инициализация.
    /// </summary>
    public void Init
        (
            int tag
        )
    {
        Tag = tag;
        Subfields = new();
    }

    /// <summary>
    /// Возврат в пул.
    /// </summary>
    public void Dispose()
    {
        Tag = default;
        Subfields = default!;
    }

    #endregion


}
