// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftWith.cs -- конструкция with в PFT
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using AM;
using AM.Collections;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Конструкция with в PFT.
/// </summary>
/// <example>
/// <code>
/// with $x in v692, v910
/// do
///     $x = '^zNewSubField', $x;
/// end
/// </code>
/// </example>
public sealed class PftWith
    : PftNode
{
    #region Properties

    /// <summary>
    /// Ссылка на переменную.
    /// </summary>
    public PftVariableReference? Variable { get; set; }

    /// <summary>
    /// Перечень полей.
    /// </summary>
    public NonNullCollection<FieldSpecification> Fields { get; private set; }

    /// <summary>
    /// Тело конструкции.
    /// </summary>
    public PftNodeCollection Body { get; private set; }

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (_virtualChildren is null)
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (Variable is not null)
                {
                    nodes.Add (Variable);
                }

                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }
        [ExcludeFromCodeCoverage]
        protected set
        {
            // Nothing to do here

            Magna.Logger.LogError
                (
                    nameof (PftWith) + "::" + nameof (Children)
                    + ": set value={Value}",
                    value.ToVisibleString()
                );
        }
    }

    /// <inheritdoc cref="PftNode.ComplexExpression" />
    public override bool ComplexExpression => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftWith()
    {
        Fields = new NonNullCollection<FieldSpecification>();
        Body = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftWith
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.With);

        Fields = new NonNullCollection<FieldSpecification>();
        Body = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftWith
        (
            string variableName,
            string[] fields,
            params PftNode[] body
        )
        : this()
    {
        Variable = new PftVariableReference (variableName);
        foreach (var field in fields)
        {
            var specification = new FieldSpecification (field);
            Fields.Add (specification);
        }

        foreach (var node in body)
        {
            Body.Add (node);
        }
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftWith)base.Clone();

        result._virtualChildren = null;

        if (!ReferenceEquals (Variable, null))
        {
            result.Variable = (PftVariableReference)Variable.Clone();
        }

        result.Fields = new NonNullCollection<FieldSpecification>();
        foreach (var field in Fields)
        {
            result.Fields.Add
                (
                    (FieldSpecification)field.Clone()
                );
        }

        result.Body = Body.CloneNodes (result).ThrowIfNull();

        return result;
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.CompareNode" />
    internal override void CompareNode
        (
            PftNode otherNode
        )
    {
        base.CompareNode (otherNode);

        var otherWith = (PftWith)otherNode;
        PftSerializationUtility.CompareNodes
            (
                Variable,
                otherWith.Variable
            );
        PftSerializationUtility.CompareLists
            (
                Fields,
                otherWith.Fields
            );
        PftSerializationUtility.CompareLists
            (
                Body,
                otherWith.Body
            );
    }

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        base.Deserialize (reader);

        Variable = (PftVariableReference?)PftSerializer
            .DeserializeNullable (reader);
        var count = reader.ReadPackedInt32();
        for (var i = 0; i < count; i++)
        {
            var field = new FieldSpecification();
            field.Deserialize (reader);
            Fields.Add (field);
        }

        PftSerializer.Deserialize (reader, Body);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        OnBeforeExecution (context);

        if (Variable is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftWith) + "::" + nameof (Execute)
                    + ": variable not specified"
                );

            throw new PftException ("With: variable not specified");
        }

        var name = Variable.Name.ThrowIfNull();

        using (var guard = new PftContextGuard (context))
        {
            var localContext = guard.ChildContext;
            localContext.Output = context.Output;

            var localManager
                = new PftVariableManager (context.Variables);

            localContext.SetVariables (localManager);

            var variable = new PftVariable
                (
                    name,
                    false
                );

            localManager.Registry.Add
                (
                    name,
                    variable
                );

            foreach (var field in Fields)
            {
                var tag = field.Tag;
                var lines = field.SubField == SubField.NoCode
                    ? PftUtility.GetFieldValue
                        (
                            context,
                            tag,
                            field.FieldRepeat
                        )
                    : PftUtility.GetSubFieldValue
                        (
                            context,
                            tag,
                            field.FieldRepeat,
                            field.SubField,
                            field.SubFieldRepeat
                        );

                var lines2 = new List<string>();
                foreach (var line in lines)
                {
                    variable.StringValue = line;

                    localContext.Execute (Body);

                    var value = variable.StringValue;
                    if (!string.IsNullOrEmpty (value))
                    {
                        lines2.Add (value);
                    }
                }

                var flag = lines2.Count != lines.Length;
                if (!flag)
                {
                    for (var i = 0; i < lines.Length; i++)
                    {
                        if (lines[i] != lines2[i])
                        {
                            flag = true;
                            break;
                        }
                    }
                }

                if (flag)
                {
                    var value = string.Join
                        (
                            Environment.NewLine,
                            lines2.ToArray()
                        );

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
            Name = "With"
        };

        var fields = new PftNodeInfo
        {
            Name = "Fields",
            Value = string.Join
                (
                    ", ",
                    Fields
                )
        };
        result.Children.Add (fields);

        var body = new PftNodeInfo
        {
            Name = "Body"
        };
        result.Children.Add (body);
        foreach (var node in Body)
        {
            body.Children.Add (node.GetNodeInfo());
        }

        return result;
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        printer.EatWhitespace();
        printer.EatNewLine();
        printer.WriteLine();
        printer
            .WriteIndent()
            .Write ("with ");
        if (!ReferenceEquals (Variable, null))
        {
            Variable.PrettyPrint (printer);
        }

        printer.SingleSpace();
        printer.Write ("in ");
        var first = true;
        foreach (var field in Fields)
        {
            if (!first)
            {
                printer.Write (", ");
            }

            printer.Write (field.ToString());
            first = false;
        }

        printer.WriteLine()
            .WriteIndent()
            .WriteLine ("do")
            .IncreaseLevel()
            .WriteIndent()
            .WriteNodes (Body);
        printer.EatWhitespace();
        printer.EatNewLine();
        printer
            .DecreaseLevel()
            .WriteLine()
            .WriteIndent()
            .Write ("end");

        //if (!ReferenceEquals(Variable, null))
        //{
        //    printer.Write(" /* with ");
        //    Variable.PrettyPrint(printer);
        //}
        printer.WriteLine();
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        base.Serialize (writer);

        PftSerializer.SerializeNullable (writer, Variable);
        writer.WritePackedInt32 (Fields.Count);
        foreach (var field in Fields)
        {
            field.Serialize (writer);
        }

        PftSerializer.Serialize (writer, Body);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    [DebuggerStepThrough]
    protected internal override bool ShouldSerializeText()
    {
        return false;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Object.ToString" />
    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append ("with ");
        result.Append (Variable);
        result.Append (" in ");
        PftUtility.FieldsToText (result, Fields);
        result.Append (" do");
        PftUtility.NodesToText (result, Body);
        result.Append (" end");

        return result.ToString();
    }

    #endregion
}
