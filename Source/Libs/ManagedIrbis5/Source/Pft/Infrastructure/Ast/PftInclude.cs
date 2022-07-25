// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* PftInclude.cs -- включение тела скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Включение тела скрипта.
/// </summary>
public sealed class PftInclude
    : PftNode
{
    #region Properties

    /// <summary>
    /// Parsed program of the included file.
    /// </summary>
    public PftProgram? Program { get; set; }

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (ReferenceEquals(_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (!ReferenceEquals(Program, null))
                {
                    nodes.Add(Program);
                }
                _virtualChildren.SetChildren(nodes);
            }

            return _virtualChildren;
        }

        protected set
        {
            Magna.Logger.LogError
                (
                    nameof (PftInclude) + "::" + nameof (Children)
                    + ": set value={Value}",
                    value.ToVisibleString()
                );
        }
    }

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax
    {
        get { return true; }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftInclude()
    {
    }

    /// <summary>
    /// Construction.
    /// </summary>
    public PftInclude
        (
            string name
        )
    {
        Text = name;
    } // constructor

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftInclude
        (
            PftToken token
        )
        : base(token)
    {
    } // constructor

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    private void ParseProgram
        (
            PftContext context,
            string fileName
        )
    {
        var ext = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(ext))
        {
            fileName += ".pft";
        }

        var specification = new FileSpecification
        {
            Path = IrbisPath.MasterFile,
            Database = context.Provider.Database,
            FileName = fileName
        };
        var source = context.Provider.ReadTextFile
            (
                specification
            );
        if (string.IsNullOrEmpty(source))
        {
            return;
        }

        var lexer = new PftLexer();
        var tokens = lexer.Tokenize(source);
        var parser = new PftParser(tokens);
        Program = parser.Parse();
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="PftNode.Clone" />
    public override object Clone()
    {
        var result = (PftInclude)base.Clone();

        if (!ReferenceEquals(Program, null))
        {
            result.Program = (PftProgram)Program.Clone();
        }

        return result;
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        if (string.IsNullOrEmpty(Text))
        {
            throw new PftCompilerException();
        }

        if (Program is null)
        {
            using var context = new PftContext(null);
            context.SetProvider(compiler.Provider);
            ParseProgram(context, Text);
        }

        var program = (PftProgram)Program.ThrowIfNull().Clone();
        program.Optimize();

        compiler.RenumberNodes(program);
        program.Compile(compiler);

        compiler.StartMethod(this);

        compiler
            .WriteIndent()
            .CallNodeMethod(program);

        compiler.EndMethod(this);
        compiler.MarkReady(this);
    } // method Compile

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        base.Deserialize(reader);

        Program = (PftProgram?) PftSerializer.DeserializeNullable(reader);
    } // method Deserialize

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        OnBeforeExecution(context);

        if (ReferenceEquals(Program, null))
        {
            try
            {
                if (string.IsNullOrEmpty(Text))
                {
                    throw new PftSyntaxException();
                }
                ParseProgram
                    (
                        context,
                        Text
                    );
            }
            catch (Exception exception)
            {
                var pftException = new PftException
                    (
                        "Include: " + Text.ToVisibleString(),
                        exception
                    );

                throw pftException;
            }
        }

        if (!ReferenceEquals(Program, null))
        {
            Program.Execute(context);
        }

        OnAfterExecution(context);
    }

    /// <inheritdoc cref="PftNode.GetNodeInfo" />
    public override PftNodeInfo GetNodeInfo()
    {
        var result = new PftNodeInfo
        {
            Node = this,
            Name = "Include",
            Value = Text
        };

        if (!ReferenceEquals(Program, null))
        {
            var program = new PftNodeInfo
            {
                Name = "Program"
            };
            result.Children.Add(program);
            program.Children.Add(Program.GetNodeInfo());
        }

        return result;
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        base.Serialize(writer);

        PftSerializer.SerializeNullable(writer, Program);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => "include(" + Text + ")";

    #endregion
}
