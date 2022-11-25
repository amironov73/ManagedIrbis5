// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MxArgument.cs -- аргумент команды
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx;

/// <summary>
/// Аргумент MX-команды.
/// </summary>
public sealed class MxArgument
{
    #region Properties

    /// <summary>
    /// Пустой массив аргументов.
    /// </summary>
    public static readonly MxArgument[] Empty = Array.Empty<MxArgument>();

    /// <summary>
    /// Текст аргумента.
    /// </summary>
    public string? Text { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Text.ToVisibleString();
    }

    #endregion
}
