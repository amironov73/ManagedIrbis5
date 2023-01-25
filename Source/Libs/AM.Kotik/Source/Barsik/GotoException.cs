// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* GotoException.cs -- исключение, генерируемое оператором goto
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System;

namespace AM.Kotik.Barsik;

/// <summary>
/// Исключение, генерируемое оператором goto.
/// </summary>
public sealed class GotoException
    : Exception
{
    #region Properties

    /// <summary>
    /// Имя метки.
    /// </summary>
    public string Label { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public GotoException
        (
            string label
        )
    {
        Sure.NotNullNorEmpty (label);

        Label = label;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Exception.ToString"/>
    public override string ToString() => $"Goto {Label}";

    #endregion
}
