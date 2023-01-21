// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* AstNode.cs -- абстрактный узел AST
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Абстрактный узел AST.
/// </summary>
public abstract class AstNode
{
    #region Public methods

    /// <summary>
    /// Общая реализация дампа узла.
    /// </summary>
    internal void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer,
            string? value
        )
    {
        for (var i = 0; i < level; i++)
        {
            writer.Write ("| ");
        }

        writer.Write ("+ ");

        if (!string.IsNullOrEmpty (name))
        {
            writer.Write (name);
            writer.Write (": ");
        }

        writer.WriteLine (value);
    }

    /// <summary>
    /// Дамп узла как элемента иерархии.
    /// </summary>
    internal virtual void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        DumpHierarchyItem (name, level, writer, GetType().Name);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => GetType().Name;

    #endregion
}
