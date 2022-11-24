// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UseNullableAnnotationInsteadOfAttribute

/* NameValue.cs -- типизированная пара "имя-значение"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM;

/// <summary>
/// Пара "имя-значение".
/// </summary>
public sealed class NameValue<TValue>
{
    #region Properties

    /// <summary>
    /// Имя.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Значение.
    /// </summary>
    [AllowNull]
    public TValue Value { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public NameValue()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="name">Имя.</param>
    /// <param name="value">Значение.</param>
    public NameValue
        (
            string? name,
            [AllowNull] TValue value
        )
    {
        Name = name;
        Value = value;
    }

    #endregion

    #region Object constructor

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{Name}: {Value}";
    }

    #endregion
}
