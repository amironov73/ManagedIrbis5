// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LocalNode.cs -- задание локальных переменных в текущем контексте
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Lexey.Barsik.Ast;

/// <summary>
/// Задание локальных переменных в текущем контексте.
/// По выходу из контекста переменные будут уничтожены.
/// </summary>
public sealed class LocalNode
    : PseudoNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LocalNode
        (
            int line,
            IList<string> names
        )
        : base (line)
    {
        Sure.NotNull (names);

        _names = names;
    }

    #endregion

    #region Private members

    private readonly IList<string> _names;

    #endregion

    #region Public methods

    /// <summary>
    /// Создание локальных переменных в указанном контексте.
    /// </summary>
    public Context ApplyTo
        (
            Context context
        )
    {
        Sure.NotNull (context);

        var result = context.CreateChildContext();
        foreach (var name in _names)
        {
            result.Variables[name] = null;
        }

        return result;
    }

    #endregion
}
