// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* PftFunctionCall.cs -- вызов функции по ее имени
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Вызов функции по ее имени.
/// </summary>
public sealed class PftFunctionCall
    : PftNode
{
    #region Properties

    /// <summary>
    /// Function name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Array of arguments.
    /// </summary>
    public PftNodeCollection Arguments { get; private set; }

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (ReferenceEquals (_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                nodes.AddRange (Arguments);
                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }
        protected set
        {
            Magna.Logger.LogError
                (
                    nameof (PftFunctionCall) + "::" + nameof (Children)
                    + ": set value={Value}",
                    value.ToVisibleString()
                );
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftFunctionCall()
    {
        Arguments = new PftNodeCollection (this);
    } // constructor

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftFunctionCall
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name, nameof (name));

        Name = name;
        Arguments = new PftNodeCollection (this);
    } // constructor

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftFunctionCall
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Identifier);

        Name = token.Text;
        Arguments = new PftNodeCollection (this);
    } // constructor

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="PftNode.Clone" />
    public override object Clone()
    {
        var result = (PftFunctionCall)base.Clone();
        result._virtualChildren = null;
        result.Arguments = Arguments.CloneNodes (result)
            .ThrowIfNull();

        return result;
    } // method Clone

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.CompareNode" />
    internal override void CompareNode
        (
            PftNode otherNode
        )
    {
        base.CompareNode (otherNode);

        var otherCall = (PftFunctionCall)otherNode;
        if (Name != otherCall.Name)
        {
            throw new PftSerializationException();
        }

        PftSerializationUtility.CompareLists
            (
                Arguments,
                otherCall.Arguments
            );
    }

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        if (string.IsNullOrEmpty (Name))
        {
            throw new PftCompilerException();
        }

        compiler.CompileNodes (Arguments);

        var actionName = compiler.CompileAction (Arguments);

        compiler.StartMethod (this);

        // TODO implement properly

        compiler
            .WriteIndent()
            .WriteLine ("string value = Evaluate({0});", actionName)
            .WriteIndent()
            .WriteLine ("Context.Write(null, value);");

        compiler.EndMethod (this);
        compiler.MarkReady (this);
    } // method Compile

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        base.Deserialize (reader);

        Name = reader.ReadNullableString();
        PftSerializer.Deserialize (reader, Arguments);
    } // method Deserialize

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        var name = Name;
        if (string.IsNullOrEmpty (name))
        {
            Magna.Logger.LogError
                (
                    nameof (PftFunctionCall) + "::" + nameof (Execute)
                    + ": name is not specified"
                );

            throw new PftSyntaxException (this);
        }

        var arguments = Arguments.ToArray();

        var descriptor = context.Functions
            .FindFunction (name);
        if (descriptor is not null)
        {
            descriptor.Function?.Invoke
                (
                    context,
                    this,
                    arguments
                );
        }
        else
        {
            var procedure = context.Procedures
                .FindProcedure (name);

            if (!ReferenceEquals (procedure, null))
            {
                var expression
                    = context.GetStringArgument (arguments, 0);
                procedure.Execute
                    (
                        context,
                        expression
                    );
            }
            else
            {
                PftFunctionManager.ExecuteFunction
                    (
                        name,
                        context,
                        this,
                        arguments
                    );
            }
        }

        OnAfterExecution (context);
    } // method Execute

    /// <inheritdoc cref="PftNode.GetNodeInfo" />
    public override PftNodeInfo GetNodeInfo()
    {
        var result = new PftNodeInfo
        {
            Node = this,
            Name = SimplifyTypeName (GetType().Name)
        };

        var name = new PftNodeInfo
        {
            Name = "Name",
            Value = Name
        };
        result.Children.Add (name);

        var arguments = new PftNodeInfo
        {
            Name = "Arguments"
        };
        arguments.Children.AddRange
            (
                Arguments.Select (node => node.GetNodeInfo())
            );
        result.Children.Add (arguments);

        return result;
    } // method GetNodeInfo

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        printer.EatWhitespace();
        printer
            .SingleSpace()
            .Write (Name)
            .Write ('(')
            .WriteNodes (", ", Arguments)
            .Write (')');
    } // method PrettyPrint

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        base.Serialize (writer);

        writer.WriteNullable (Name);
        PftSerializer.Serialize (writer, Arguments);
    } // method Serialize

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="PftNode.ToString" />
    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append (Name);
        result.Append ('(');
        PftUtility.NodesToText (",", result, Arguments);
        result.Append (')');

        return result.ToString();
    } // method ToString

    #endregion
} // class PftFunctionCall

// namespace ManagedIrbis.Pft.Infrastructure.Ast
