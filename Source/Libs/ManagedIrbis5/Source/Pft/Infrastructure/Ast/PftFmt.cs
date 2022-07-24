// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftFmt.cs -- форматирование чисел по принципам .NET
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;

using AM;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Форматирование чисел с плавающей точкой по принципам .NET.
    /// </summary>
    /// <example>
    /// <code>
    /// fmt(3+0.14,'E4')
    /// </code>
    /// </example>
    public sealed class PftFmt
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Спецификация формата для числа.
        /// Если спецификация пуста, то применяется форматирование,
        /// принятое в .NET по умолчанию ("G").
        /// </summary>
        public PftNodeCollection Format { get; private set; }

        /// <summary>
        /// Собственно число, подлежащее форматированию.
        /// </summary>
        public PftNumeric? Number { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    var nodes = new List<PftNode>();
                    if (Number is { } number)
                    {
                        nodes.Add(number);
                    }

                    nodes.AddRange(Format);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            [ExcludeFromCodeCoverage]
            protected set
            {
                // Nothing to do here

                Magna.Error
                    (
                        "PftFmt::Children: "
                        + "set value="
                        + value.ToVisibleString()
                    );

            }
        } // property Children

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFmt()
        {
            Format = new PftNodeCollection(this);
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFmt
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Fmt);

            Format = new PftNodeCollection(this);
        } // constructor

        #endregion

        #region Private members

        private VirtualChildren? _virtualChildren;

        #endregion

        #region Public methods

        /// <summary>
        /// Format the number according specified format.
        /// </summary>
        public static void FormatNumber
            (
                PftContext context,
                PftNode? node,
                double number,
                string? format
            )
        {
            string output;

            if (string.IsNullOrEmpty(format))
            {
                output = number.ToString
                    (
                        CultureInfo.InvariantCulture
                    );
            }
            else
            {
                output = number.ToString
                    (
                        format,
                        CultureInfo.InvariantCulture
                    );
            }

            context.Write(node, output);
        } // method FormatNumber

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            var result = (PftFmt) base.Clone();
            result._virtualChildren = null;
            result.Format = Format.CloneNodes(result).ThrowIfNull();

            if (!ReferenceEquals(Number, null))
            {
                result.Number = (PftNumeric) Number.Clone();
            }

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
            base.CompareNode(otherNode);

            var otherFmt = (PftFmt)otherNode;
            PftSerializationUtility.CompareLists
                (
                    Format,
                    otherFmt.Format
                );
            PftSerializationUtility.CompareNodes
                (
                    Number,
                    otherFmt.Number
                );
        } // method CompareNode

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (Number is null)
            {
                return;
            }

            var defaultFormat = Format.Count == 0
                ? "G"
                : null;

            Number.Compile(compiler);
            compiler.CompileNodes(Format);

            var actionName = compiler.CompileAction(Format);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .Write("double value = ")
                .CallNodeMethod(Number);

            if (defaultFormat is null)
            {
                compiler
                    .WriteIndent()
                    .WriteLine("string format = Evaluate({0});", actionName);
            }
            else
            {
                compiler
                    .WriteIndent()
                    .WriteLine("string format = \"{0}\";", defaultFormat);
            }

            compiler
                .WriteIndent()
                .WriteLine("string text = value.ToString(format, CultureInfo.InvariantCulture);")
                .WriteIndent()
                .WriteLine("Context.Write(null, text);");

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

            PftSerializer.Deserialize(reader, Format);
            Number = (PftNumeric?) PftSerializer.DeserializeNullable(reader);
        } // method Deserialize

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (Number is { } number)
            {
                number.Execute(context);
                var value = number.Value;
                var format = context.Evaluate(Format);

                FormatNumber
                    (
                        context,
                        this,
                        value,
                        format
                    );
            }

            OnAfterExecution(context);
        } // method Execute

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            var result = new PftNodeInfo
            {
                Node = this,
                Name = "Fmt"
            };

            if (Number is { } number)
            {
                result.Children.Add(number.GetNodeInfo());
            }

            if (Format.Count != 0)
            {
                var format = new PftNodeInfo
                {
                    Name = "Format"
                };

                result.Children.Add(format);
                foreach (var node in Format)
                {
                    format.Children.Add(node.GetNodeInfo());
                }
            }

            return result;
        } // method GetNodeInfo

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write("fmt(");
            Number?.PrettyPrint(printer);
            printer.EatWhitespace();
            printer
                .Write(", ")
                .Write(Format)
                .Write(')');
        } // method PrentyPrint

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.Serialize(writer, Format);
            PftSerializer.SerializeNullable(writer, Number);
        } // method Serialize

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("fmt(");
            if (!ReferenceEquals(Number, null))
            {
                result.Append(Number);
            }

            result.Append(',');
            var first = true;
            foreach (var node in Format)
            {
                if (!first)
                {
                    result.Append(' ');
                }

                result.Append(node);
                first = false;
            }

            result.Append(')');

            return result.ToString();
        } // method ToString

        #endregion

    } // class PftFmt

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
