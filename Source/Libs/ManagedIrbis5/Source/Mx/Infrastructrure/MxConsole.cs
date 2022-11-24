// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MxConsole.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.ConsoleIO;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Infrastructrure;

/// <summary>
///
/// </summary>
public class MxConsole
    : IMxConsole
{
    #region IMxConsole members

    /// <inheritdoc cref="IMxConsole.BackgroundColor" />
    public ConsoleColor BackgroundColor
    {
        get => ConsoleInput.BackgroundColor;
        set => ConsoleInput.BackgroundColor = value;
    }

    /// <inheritdoc cref="IMxConsole.ForegroundColor" />
    public ConsoleColor ForegroundColor
    {
        get => ConsoleInput.ForegroundColor;
        set => ConsoleInput.ForegroundColor = value;
    }

    /// <inheritdoc cref="IMxConsole.Write" />
    public void Write
        (
            string text
        )
    {
        ConsoleInput.Write(text);
    }

    /// <inheritdoc cref="IMxConsole.ReadLine" />
    public string? ReadLine()
    {
        return ConsoleInput.ReadLine();
    }

    /// <inheritdoc cref="IMxConsole.Clear" />
    public void Clear()
    {
        ConsoleInput.Clear();
    }

    #endregion
}
