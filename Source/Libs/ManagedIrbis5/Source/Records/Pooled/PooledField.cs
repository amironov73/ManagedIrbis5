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

using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Text;

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
    public PooledList<PooledSubField> Subfields { get; private set; } = default!;

    #endregion

    #region Private members

    internal RecordPool _pool = default!;

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление подполя.
    /// </summary>
    /// <param name="code">Код добавляемого подполя.</param>
    public PooledField Add
        (
            char code
        )
    {
        var subfield = _pool.GetSubField (code);
        Subfields.Add (subfield);

        return this;
    }

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

        // ReSharper disable ConditionIsAlwaysTrueOrFalse
        if (Subfields is not null)
        // ReSharper restore ConditionIsAlwaysTrueOrFalse
        {
            foreach (var subfield in Subfields)
            {
                _pool.Return (subfield);
            }

            Subfields.Clear();
        }

        Subfields = default!;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var length = 4 + Subfields.Sum
            (
                sf => sf.Value!.Length
                      + (sf.Code == default ? 0 : 2)
            );
        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (length);
        builder.Append (Tag.ToInvariantString())
            .Append ('#');
        foreach (var subfield in Subfields)
        {
            builder.Append (subfield);
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
