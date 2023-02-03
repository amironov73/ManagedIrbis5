// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* FormatNode.cs -- строка форматирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Строка форматирования вида `$"{z} = {x} + {y}"`.
/// </summary>
internal sealed class FormatNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FormatNode
        (
            FormatSpecification[] specification
        )
    {
        Sure.NotNull (specification);
        
        _specification = specification;
    }

    #endregion

    #region Private members

    private readonly FormatSpecification[] _specification;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute 
        (
            Context context
        )
    {
        var result = new StringBuilder();

        var interpreter = context.Interpreter.ThrowIfNull();
        foreach (var specification in _specification)
        {
            result.Append (specification.Prefix);
            if (!string.IsNullOrEmpty (specification.Value))
            {
                var atom = interpreter.EvaluateAtom (specification.Value);
                var value = atom.Compute (context);
                if (value is not null)
                {
                    if (!string.IsNullOrEmpty (specification.Format)
                        && value is IFormattable formattable)
                    {
                        var text = formattable.ToString (specification.Format, null);
                        result.Append (text);
                    }
                    else
                    {
                        result.Append (value);
                    }
                }
            }
        }

        return result.ToString();
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
    internal override void DumpHierarchyItem 
        (
            string? name, 
            int level, 
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer);

        var count = 0;
        foreach (var specification in _specification)
        {
            DumpHierarchyItem ("Item", level + 1, writer, count.ToInvariantString());
            DumpHierarchyItem ("Prefix", level + 2, writer, specification.Prefix.ToVisibleString());
            DumpHierarchyItem ("Value", level + 2, writer, specification.Value.ToVisibleString());
            DumpHierarchyItem ("Format", level + 2, writer, specification.Format.ToVisibleString());
            count++;
        }
    }

    #endregion
}
