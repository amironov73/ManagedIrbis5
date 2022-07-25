// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftFieldAssignment.cs -- присваивание значения полю записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Присваивание значения полю записи.
/// </summary>
public sealed class PftFieldAssignment
    : PftNode
{
    #region Properties

    /// <summary>
    /// Поле библиографической записи.
    /// </summary>
    public PftField? Field { get; set; }

    /// <summary>
    /// Expression.
    /// </summary>
    public PftNodeCollection Expression { get; private set; }

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (ReferenceEquals (_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (!ReferenceEquals (Field, null))
                {
                    nodes.Add (Field);
                }

                nodes.AddRange (Expression);
                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }

        [ExcludeFromCodeCoverage]
        protected set
        {
            Magna.Logger.LogError
                (
                    nameof (PftFieldAssignment) + "::" + nameof (Children)
                    + ": set value={Value}",
                    value.ToVisibleString()
                );
        }
    }

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    /// <inheritdoc cref="PftNode.ComplexExpression" />
    public override bool ComplexExpression => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftFieldAssignment()
    {
        Expression = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftFieldAssignment
        (
            string fieldSpec
        )
    {
        Sure.NotNullNorEmpty (fieldSpec);

        Field = new PftV (fieldSpec);
        Expression = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftFieldAssignment
        (
            string fieldSpec,
            params PftNode[] bodyNodes
        )
    {
        Sure.NotNullNorEmpty (fieldSpec);
        Sure.NotNull (bodyNodes);

        Field = new PftV (fieldSpec);
        Expression = new PftNodeCollection (this);
        Expression.AddRange (bodyNodes);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftFieldAssignment
        (
            PftToken token
        )
        : base (token)
    {
        Field = new PftV (token.Text.ThrowIfNull());
        Expression = new PftNodeCollection (this);
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftFieldAssignment) base.Clone();

        if (Field is not null)
        {
            result.Field = (PftField) Field.Clone();
        }

        result.Expression = Expression.CloneNodes (result).ThrowIfNull();

        return result;
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.CompareNode"/>
    internal override void CompareNode
        (
            PftNode otherNode
        )
    {
        Sure.NotNull (otherNode);

        base.CompareNode (otherNode);

        var otherAssignment = (PftFieldAssignment) otherNode;
        PftSerializationUtility.CompareNodes
            (
                Field,
                otherAssignment.Field
            );
        PftSerializationUtility.CompareLists
            (
                Expression,
                otherAssignment.Expression
            );
    }

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        base.Deserialize (reader);

        Field = (PftField?)PftSerializer.DeserializeNullable (reader);
        PftSerializer.Deserialize (reader, Expression);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        var field = Field;
        if (field is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftFieldAssignment) + "::" + nameof (Execute)
                    + ": field is not set"
                );

            throw new IrbisException ("Field is null");
        }

        var fieldTag = field.Tag;
        if (string.IsNullOrEmpty (fieldTag))
        {
            Magna.Logger.LogError
                (
                    nameof (PftFieldAssignment) + "::" + nameof (Execute)
                    + ": field tag is not set"
                );

            throw new IrbisException ("Field tag is null");
        }

        var value = context.Evaluate (Expression);
        var command = field.Command;
        var tag = fieldTag.SafeToInt32();
        if (command == 'g' || command == 'G')
        {
            // TODO support field repeat
            context.Globals[tag] = value;
        }
        else
        {
            if (field.SubField == SubField.NoCode)
            {
                PftUtility.AssignField
                    (
                        context,
                        tag,
                        field.FieldRepeat,
                        value
                    );
            }
            else
            {
                PftUtility.AssignSubField
                    (
                        context,
                        tag,
                        field.FieldRepeat,
                        field.SubField,
                        field.SubFieldRepeat,
                        value
                    );
            }
        }

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.GetNodeInfo" />
    public override PftNodeInfo GetNodeInfo()
    {
        var result = new PftNodeInfo
        {
            Node = this,
            Name = "FieldAssignment"
        };

        if (!ReferenceEquals (Field, null))
        {
            result.Children.Add (Field.GetNodeInfo());
        }

        if (Expression.Count != 0)
        {
            var expression = new PftNodeInfo
            {
                Name = "Expression"
            };
            result.Children.Add (expression);

            foreach (var node in Expression)
            {
                expression.Children.Add (node.GetNodeInfo());
            }
        }

        return result;
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        printer.WriteIndentIfNeeded();
        Field?.PrettyPrint (printer);

        printer.Write ('=');
        foreach (var node in Expression)
        {
            node.PrettyPrint (printer);
        }

        printer.Write (';');
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        PftSerializer.SerializeNullable (writer, Field);
        PftSerializer.Serialize (writer, Expression);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    [DebuggerStepThrough]
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (Field);
        builder.Append ('=');
        PftUtility.NodesToText (builder, Expression);
        builder.Append (';');

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
