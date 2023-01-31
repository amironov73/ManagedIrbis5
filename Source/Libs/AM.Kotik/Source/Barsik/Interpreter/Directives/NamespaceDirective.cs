// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* NamespaceDirective.cs -- дамп пространств имен
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Директива: дамп пространств имен.
/// </summary>
public sealed class NamespaceDirective
    : DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NamespaceDirective()
        : base ("ns")
    {
        // пустое тело метода
    }

    #endregion

    #region DirectiveBase members

    /// <inheritdoc cref="DirectiveBase.Execute"/>
    public override void Execute
        (
            Context context,
            string? argument
        )
    {
        context.DumpNamespaces();
    }

    #endregion
}
