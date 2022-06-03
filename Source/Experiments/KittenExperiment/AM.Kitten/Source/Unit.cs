// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Unit.cs -- замена Void, позволяющая возвращать из парсера пустое значение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kitten;

/// <summary>
/// Замена <see cref="System.Void"/>, позволяющая
/// возвращать из парсера пустое значение.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class Unit
{
    #region Properties

    /// <summary>
    /// Единственное доступное значение данного типа.
    /// </summary>
    public static Unit Value { get; } = new ();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    private Unit()
    {
        // пустое тело метода
    }

    #endregion
}
