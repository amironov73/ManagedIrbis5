// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* StatementNode.cs -- базовый класс для стейтментов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Базовый класс для стейтментов.
/// </summary>
public class StatementNode
    : AstNode
{
    #region Private members

    /// <summary>
    /// Перед выполнением стейтмента.
    /// </summary>
    protected virtual void PreExecute
        (
            Context context
        )
    {
        // пока этот метод не выполняет никаких действий
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Выполнение действий, связанных с данным узлом.
    /// </summary>
    /// <param name="context">Контекст исполнения программы.</param>
    public virtual void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        // больше никаких действий тут пока не нужно
    }

    #endregion
}
