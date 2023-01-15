// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ValueHolder.cs -- костыль для хранения структуры в куче
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Костыль для хранения структуры в куче.
/// </summary>
public sealed class ValueHolder<TValue>
    where TValue: struct
{
    #region Properties

    /// <summary>
    /// Хранимое значение.
    /// </summary>
    public TValue Value { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ValueHolder
        (
            TValue value
        )
    {
        Value = value;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="ToString"/>
    public override string ToString() => Value.ToString()!;

    #endregion
}
