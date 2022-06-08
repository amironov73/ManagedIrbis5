// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Context.cs -- контекст исполнения скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Контекст исполнения скрипта.
/// </summary>
public sealed class Context
{
    #region Properties

    /// <summary>
    /// Родительский контекст.
    /// </summary>
    public Context? Parent { get; }

    /// <summary>
    /// Переменные.
    /// </summary>
    public Dictionary<string, dynamic?> Variables { get; }

    /// <summary>
    /// Стандартный входной поток.
    /// </summary>
    public TextReader Input { get; set; }

    /// <summary>
    /// Стандартный выходной поток.
    /// </summary>
    public TextWriter Output { get; set; }

    /// <summary>
    /// Стандартный поток ошибок.
    /// </summary>
    public TextWriter Error { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Context
        (
            TextReader input,
            TextWriter output,
            TextWriter error,
            Context? parent = null
        )
    {
        Sure.NotNull (input);
        Sure.NotNull (output);
        Sure.NotNull (error);

        Parent = parent;
        Variables = new ();
        Input = input;
        Output = output;
        Error = error;
    }

    #endregion
}
