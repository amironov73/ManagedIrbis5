// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftProgram.cs -- корень AST-дерева
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;
using AM.Text.Output;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

/// <summary>
/// Корень дерева AST.
/// </summary>
public sealed class PftProgram
    : PftNode
{
    #region Properties

    /// <summary>
    /// Процедуры.
    /// </summary>
    public PftProcedureManager Procedures { get; internal set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftProgram()
    {
        Procedures = new PftProcedureManager();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftProgram
        (
            params PftNode[] nodes
        )
        : this()
    {
        Sure.NotNull (nodes);

        foreach (PftNode node in nodes)
        {
            Children.Add (node);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Dump the program.
    /// </summary>
    public string DumpToText()
    {
        var output = new TextOutput();
        var nodeInfo = GetNodeInfo();
        PftNodeInfo.Dump (output, nodeInfo, 0);

        return output.ToString() ?? string.Empty;
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        base.Deserialize (reader);

        Procedures.Deserialize (reader);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        try
        {
            context.Procedures = Procedures;
            base.Execute (context);
        }
        catch (PftBreakException exception)
        {
            // It was break operator

            Magna.Logger.LogTrace
                (
                    exception,
                    nameof (PftProgram) + "::" + nameof (Execute)
                );

            if (context.Parent is not null)
            {
                throw;
            }
        }
        catch (PftExitException exception)
        {
            // It was exit operator

            Magna.Logger.LogTrace
                (
                    exception,
                    nameof (PftProgram) + "::" + nameof (Execute)
                );

            if (context.Parent is not null)
            {
                throw;
            }
        }
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        base.Serialize (writer);

        Procedures.Serialize (writer);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="PftNode.ToString" />
    public override string ToString()
    {
        using var printer = new PftPrettyPrinter();
        PrettyPrint (printer);
        string result = printer.ToString();

        return result;
    }

    #endregion
}
