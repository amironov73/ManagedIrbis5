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

using AM;
using AM.IO;
using AM.Text;

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
    /// Имя функции.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Массив аргументов.
    /// </summary>
    public PftNodeCollection Arguments { get; private set; } = null!;

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
    /// Конструктор по умолчанию.
    /// </summary>
    public PftFunctionCall()
    {
        _Initialize();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftFunctionCall
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        Name = name;
        _Initialize();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftFunctionCall
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Identifier);

        Name = token.Text;
        _Initialize();
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    private void _Initialize()
    {
        Arguments = new PftNodeCollection (this);
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="PftNode.Clone" />
    public override object Clone()
    {
        var result = (PftFunctionCall) base.Clone();
        result._virtualChildren = null;
        result.Arguments = Arguments.CloneNodes (result).ThrowIfNull();

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
        Sure.NotNull (compiler);

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
    }

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        base.Deserialize (reader);

        Name = reader.ReadNullableString();
        PftSerializer.Deserialize (reader, Arguments);
    }

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

        var descriptor = context.Functions.FindFunction (name);
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
            var procedure = context.Procedures.FindProcedure (name);

            if (procedure is not null)
            {
                var expression = context.GetStringArgument (arguments, 0);
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
    }

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
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        printer.EatWhitespace();
        printer
            .SingleSpace()
            .Write (Name)
            .Write ('(')
            .WriteNodes (", ", Arguments)
            .Write (')');
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        base.Serialize (writer);

        writer.WriteNullable (Name);
        PftSerializer.Serialize (writer, Arguments);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="PftNode.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (Name);
        builder.Append ('(');
        PftUtility.NodesToText (",", builder, Arguments);
        builder.Append (')');

        return builder.ReturnShared();
    }

    #endregion
}
