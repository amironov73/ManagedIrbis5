// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* AtomNode.cs -- узел, в котором происходят некие вычисления
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Узел, в котором происходят некие вычисления,
/// возможно, константные.
/// </summary>
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
        throw new NotSupportedException ($"Assignment in {GetType().FullName}");
    }

    #endregion
}
