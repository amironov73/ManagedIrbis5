// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftProcedureDefinition.cs -- определение процедуры в PFT
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Определение процедуры в PFT
/// </summary>
public sealed class PftProcedureDefinition
    : PftNode
{
    #region Properties

    /// <summary>
    /// Procedure itself.
    /// </summary>
    public PftProcedure? Procedure { get; set; }

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (ReferenceEquals (_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (!ReferenceEquals (Procedure, null))
                {
                    nodes.AddRange (Procedure.Body);
                }

                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }
        protected set
        {
            Magna.Logger.LogError
                (
                    nameof (PftProcedureDefinition) + "::" + nameof (Children)
                    + ": set value={Value}",
                    value.ToVisibleString()
                );
        }
    }

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftProcedureDefinition()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftProcedureDefinition
        (
            PftToken token
        )
        : base (token)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="PftNode.Clone" />
    public override object Clone()
    {
        var result = (PftProcedureDefinition)base.Clone();

        if (Procedure is not null)
        {
            result.Procedure = (PftProcedure) Procedure.Clone();
        }

        return result;
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

        var flag = reader.ReadBoolean();
        if (flag)
        {
            Procedure = new PftProcedure();
            Procedure.Deserialize (reader);
        }
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        // Do something?

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.GetNodeInfo" />
    public override PftNodeInfo GetNodeInfo()
    {
        var result = new PftNodeInfo
        {
            Node = this,
            Name = "Procedure"
        };

        if (!ReferenceEquals (Procedure, null))
        {
            result.Value = Procedure.Name;

            var body = new PftNodeInfo
            {
                Name = "Body"
            };
            result.Children.Add (body);

            foreach (var node in Procedure.Body)
            {
                body.Children.Add (node.GetNodeInfo());
            }
        }

        return result;
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        if (ReferenceEquals (Procedure, null))
        {
            writer.Write (false);
        }
        else
        {
            writer.Write (true);
            Procedure.Serialize (writer);
        }
    }

    #endregion
}
