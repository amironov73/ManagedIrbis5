// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AtomNode.cs -- узел, в котором происходят некие вычисления
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Lexey.Ast;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Lexey.Barsik.Ast;

/// <summary>
/// Узел, в котором происходят некие вычисления,
/// возможно, константные.
/// </summary>
[PublicAPI]
public abstract class AtomNode
    : AstNode
{
    #region Public methods

    /// <summary>
    /// Вычисление значения, связанного сданным узлом.
    /// </summary>
    public abstract dynamic? Compute
        (
            Context context
        );

    /// <summary>
    /// Присвоение значения данному узлу.
    /// </summary>
    public virtual dynamic? Assign
        (
            Context context,
            string? operation,
            dynamic? value
        )
    {
        // по умолчанию узлы не поддерживают присваивание
        Magna.Logger.LogInformation ("Assignment in {Type}", GetType().FullName);
        throw new NotSupportedException ($"Assignment in {GetType().FullName}");
    }

    #endregion
}
