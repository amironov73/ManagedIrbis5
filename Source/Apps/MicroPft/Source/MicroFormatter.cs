// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* MicroFormatter.cs -- форматтер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM;

using ManagedIrbis;

using MicroPft.Ast;

#endregion

#nullable enable

namespace MicroPft;

/// <summary>
/// Форматтер.
/// </summary>
public sealed class MicroFormatter
{
    #region Private members

    private PftNode[] _nodes = null!;

    #endregion

    #region Public methods

    /// <summary>
    /// Расформатирование указанной записи.
    /// </summary>
    public string Format
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var context = new PftContext
        {
            Record = record
        };
        context.Reset();
        foreach (var node in _nodes)
        {
            node.Execute (context);
        }

        return context.Output.ToString();
    }
    
    /// <summary>
    /// Разбор формата.
    /// </summary>
    public void Parse
        (
            string format
        )
    {
        Sure.NotNull (format);
        
        _nodes = Grammar.Parse (format).ToArray();
    }

    #endregion
}
