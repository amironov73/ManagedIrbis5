// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftAssignment.cs -- операция присваивания переменной вычисленного значения выражения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using AM;
using AM.IO;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Операция присваивания переменной вычисленного значения выражения.
/// </summary>
public sealed class PftAssignment
    : PftNode
{
    #region Properties

    /// <summary>
    /// Флаг: мы имеем численное, а не текстовое присваивание.
    /// </summary>
    public bool IsNumeric { get; set; }

    /// <summary>
    /// Имя переменной, которой будет присвоено вычисленное значение.
    /// </summary>
    public string? VaruableName { get; set; }

    /// <summary>
    /// Спецификация индекса.
    /// </summary>
    public IndexSpecification Index { get; set; }

    /// <inheritdoc cref="PftNode.ComplexExpression" />
    public override bool ComplexExpression => true;

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftAssignment()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftAssignment
        (
            string varuableName
        )
    {
        Sure.NotNullNorEmpty (varuableName);

        VaruableName = varuableName;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftAssignment
        (
            bool isNumeric,
            string varuableName,
            params PftNode[] body
        )
    {
        Sure.NotNullNorEmpty (varuableName);
        Sure.NotNull (body);

        IsNumeric = isNumeric;
        VaruableName = varuableName;
        foreach (var node in body)
        {
            Children.Add (node);
        }
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftAssignment
        (
            PftToken token
        )
        : base (token)
    {
        VaruableName = token.Text;
    }

    #endregion

    #region Private members

    // Handle direct variable-to-variable assignment
    // Ignore IsNumeric setting
    private bool HandleDirectAssignment
        (
            PftContext context
        )
    {
        //
        // TODO handle indexing
        //

        var name = VaruableName.ThrowIfNull (nameof (VaruableName));

        if (Children.Count == 1)
        {
            var reference = Children.First() as PftVariableReference;
            if (!ReferenceEquals (reference, null))
            {
                var donorName = reference.Name.ThrowIfNull();
                if (context.Variables.Registry.TryGetValue
                        (
                            donorName,
                            out var donor
                        ))
                {
                    if (donor.IsNumeric)
                    {
                        context.Variables.SetVariable
                            (
                                name,
                                donor.NumericValue
                            );
                    }
                    else
                    {
                        context.Variables.SetVariable
                            (
                                name,
                                donor.StringValue
                            );
                    }

                    return true;
                }
            }
        }

        return false;
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftAssignment) base.Clone();
        result.IsNumeric = IsNumeric;
        result.Index = (IndexSpecification) Index.Clone();

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

        var otherAssignment = (PftAssignment)otherNode;
        if (VaruableName != otherAssignment.VaruableName
            || !IndexSpecification.Compare (Index, otherAssignment.Index)
            || IsNumeric != otherAssignment.IsNumeric)
        {
            throw new PftSerializationException();
        }
    }

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        if (string.IsNullOrEmpty (VaruableName))
        {
            throw new PftCompilerException();
        }

        string? actionName = null;
        IndexInfo? index = null;

        compiler.CompileNodes (Children);
        if (!IsNumeric)
        {
            actionName = compiler.CompileAction (Children);
            index = compiler.CompileIndex (Index);
        }

        compiler.StartMethod (this);

        if (IsNumeric)
        {
            var numeric =
                (
                    Children.FirstOrDefault() as PftNumeric
                )
                .ThrowIfNull ("numeric");

            compiler
                .WriteIndent()
                .Write ("double value = ")
                .CallNodeMethod (numeric);

            compiler
                .WriteIndent()
                .WriteLine
                    (
                        "Context.Variables.SetVariable("
                        + "\"{0}\", value);",
                        CompilerUtility.Escape (VaruableName)!
                    );
        }
        else
        {
            compiler
                .WriteIndent()
                .Write ("string value = "); //-V3010
            if (ReferenceEquals (actionName, null))
            {
                compiler.WriteLine ("string.Empty;");
            }
            else
            {
                compiler
                    .WriteLine ("Evaluate({0});", actionName); //-V3010

                compiler
                    .WriteIndent()
                    .WriteLine
                        (
                            "Context.Variables.SetVariable("
                            + "Context, \"{0}\", "
                            + "{1}, "
                            + "value);",
                            CompilerUtility.Escape (VaruableName)!,
                            index?.Reference
                        );
            }
        }

        compiler.EndMethod (this);
        compiler.MarkReady (this);
    }

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        base.Deserialize (reader);

        IsNumeric = reader.ReadBoolean();
        VaruableName = reader.ReadNullableString();
        Index.Deserialize (reader);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        if (!HandleDirectAssignment (context))
        {
            var name = VaruableName.ThrowIfNull();
            var stringValue = context.Evaluate (Children);

            if (IsNumeric)
            {
                var numeric =
                    (
                        Children.FirstOrDefault() as PftNumeric
                    )
                    .ThrowIfNull();
                var numericValue = numeric.Value;
                context.Variables.SetVariable
                    (
                        name,
                        numericValue
                    );
            }
            else
            {
                context.Variables.SetVariable
                    (
                        context,
                        name,
                        Index,
                        stringValue
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
            Value = VaruableName
        };
        result.Children.Add (name);

        if (Index.Kind != IndexKind.None)
        {
            result.Children.Add (Index.GetNodeInfo());
        }

        var numeric = new PftNodeInfo
        {
            Name = "IsNumeric",
            Value = IsNumeric.ToString()
        };
        result.Children.Add (numeric);

        foreach (var node in Children)
        {
            result.Children.Add (node.GetNodeInfo());
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

        printer
            .SingleSpace()
            .Write ('$')
            .Write (VaruableName)
            .Write (Index.ToText())
            .Write ('=');
        base.PrettyPrint (printer);
        printer.Write (';');
    } // method PrettyPrint

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        writer.Write (IsNumeric);
        writer.WriteNullable (VaruableName);
        Index.Serialize (writer);
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
        builder.AppendFormat ("${0}{1}=", VaruableName, Index.ToText());
        PftUtility.NodesToText (builder, Children);
        builder.Append (';');

        return builder.ReturnShared();
    }

    #endregion
}
