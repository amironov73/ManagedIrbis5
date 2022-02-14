// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* PooledSubField.cs -- подполе библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/// <summary>
/// Подполе библиографической записи, адаптированное для пулинга.
/// </summary>
[XmlRoot ("subfield")]
public sealed class PooledSubField
{
    #region Properties

    /// <summary>
    /// Код подполя.
    /// </summary>
    public char Code { get; set; }

    /// <summary>
    /// Значение подполя.
    /// </summary>
    public string? Value { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Инициализация.
    /// </summary>
    /// <param name="code">Код подполя.</param>
    /// <param name="value">Значение подполя.</param>
    public void Init
        (
            char code,
            string? value
        )
    {
        Code = code;
        Value = value;
    }

    /// <summary>
    /// Возврат в пул.
    /// </summary>
    public void Dispose()
    {
        Code = default;
        Value = default;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Code == default
            ? Value ?? string.Empty
            : "^" + char.ToLowerInvariant (Code) + Value;
    }

    #endregion
}
