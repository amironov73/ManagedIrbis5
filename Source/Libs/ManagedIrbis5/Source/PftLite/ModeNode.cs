// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ModeNode.cs -- переключение режима вывода поля/подполя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Переключение режима вывода поля/подполя.
/// </summary>
internal sealed class ModeNode
    : PftNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ModeNode
        (
            char mode,
            bool upper
        )
    {
        _mode = mode;
        _upper = upper;
    }

    #endregion

    #region Private members

    private readonly char _mode;

    private readonly bool _upper;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"mode_{_mode}_{_upper}";
    }

    #endregion
}
