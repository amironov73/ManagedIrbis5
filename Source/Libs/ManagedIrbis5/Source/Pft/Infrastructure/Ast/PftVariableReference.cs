// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftVariableReference.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftVariableReference
        : PftNumeric
    {
        #region Properties

        /// <summary>
        /// Name of the variable.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Index.
        /// </summary>
        public IndexSpecification Index { get; set; }

        /// <summary>
        /// Subfield code.
        /// </summary>
        public char SubFieldCode { get; set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableReference()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableReference
            (
                string name
            )
        {
            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableReference
            (
                string name,
                int index
            )
        {
            Name = name;
            Index = IndexSpecification.GetLiteral(index);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableReference
            (
                string name,
                char code
            )
        {
            Name = name;
            SubFieldCode = code;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableReference
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Variable);

            var text = token.Text.ThrowIfNull("token.Text");
            if (text.StartsWith("$"))
            {
                text = text.Substring(1);
            }
            Name = text;
        }

        #endregion

        #region Private members

        private static ReadOnlyMemory<char> _ReadTo
            (
                StringReader reader,
                char delimiter
            )
        {
            var result = new StringBuilder();

            while (true)
            {
                var next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                var c = (char)next;
                if (c == delimiter)
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString().AsMemory();
        }

        private static Field _ParseLine
            (
                string line
            )
        {
            var reader = new StringReader(line);
            var result = new Field
            {
                Value = _ReadTo(reader, '^')
            };

            while (true)
            {
                var next = reader.Read();
                if (next < 0)
                {
                    break;
                }

                var code = char.ToLower((char)next);
                var text = _ReadTo(reader, '^');
                var subField = new SubField
                {
                    Code = code,
                    Value = text
                };
                result.Subfields.Add(subField);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Do the variable.
        /// </summary>
        public static double DoVariable
            (
                PftContext context,
                PftNode node,
                string name,
                IndexSpecification index,
                char subField
            )
        {
            var result = 0.0;

            var variable = context.Variables.GetExistingVariable(name);
            if (ReferenceEquals(variable, null))
            {
                Magna.Error
                    (
                        "PftVariableReference::DoVariable: "
                        + "unknown variable="
                        + name.ToVisibleString()
                    );

                throw new PftSemanticException
                    (
                        "unknown variable: " + name
                    );
            }

            if (variable.IsNumeric)
            {
                result = variable.NumericValue;
                context.Write
                    (
                        node,
                        result.ToInvariantString()
                    );
            }
            else
            {
                var output = variable.StringValue ?? string.Empty;

                if (index.Kind != IndexKind.None)
                {
                    var lines = output.SplitLines();

                    lines = PftUtility.GetArrayItem
                        (
                            context,
                            lines,
                            index
                        );

                    if (subField != SubField.NoCode)
                    {
                        var list = new List<string>();

                        foreach (var line in lines)
                        {
                            var field = _ParseLine(line);
                            var text = field.GetFirstSubFieldValue
                                (
                                    subField
                                );
                            if (!text.IsEmpty)
                            {
                                list.Add(text.ToString());
                            }
                        }

                        lines = list.ToArray();
                    }

                    output = string.Join
                        (
                            Environment.NewLine,
                            lines
                        );
                }
                else
                {
                    if (subField != SubField.NoCode)
                    {
                        var lines = output.SplitLines();

                        var list = new List<string>();

                        foreach (var line in lines)
                        {
                            var field = _ParseLine(line);
                            var text = field.GetFirstSubFieldValue
                                (
                                    subField
                                );
                            if (!text.IsEmpty)
                            {
                                list.Add(text.ToString());
                            }
                        }

                        lines = list.ToArray();
                        output = string.Join
                            (
                                Environment.NewLine,
                                lines
                            );
                    }
                }

                context.Write(node, output);
            }

            return result;
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            var result
                = (PftVariableReference)base.Clone();

            result.Index = (IndexSpecification)Index.Clone();

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
            base.CompareNode(otherNode);

            var otherVariable
                = (PftVariableReference)otherNode;
            if (Name != otherVariable.Name
                || !IndexSpecification.Compare(Index, otherVariable.Index)
                || SubFieldCode != otherVariable.SubFieldCode)
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
            if (ReferenceEquals(Name, null))
            {
                throw new PftCompilerException();
            }

            var index = compiler.CompileIndex(Index);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine
                    (
                        "double result = PftVariableReference.DoVariable("
                        + "Context, null, \"{0}\", {1}, '\\x{2:X4}');",
                        CompilerUtility.Escape(Name),
                        index.Reference,
                        (int)SubFieldCode
                    )
                .WriteIndent()
                .WriteLine("return result;");

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Name = reader.ReadNullableString();
            Index.Deserialize(reader);
            SubFieldCode = reader.ReadChar();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            Value = DoVariable
                (
                    context,
                    this,
                    Name.ThrowIfNull("Name"),
                    Index,
                    SubFieldCode
                );

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            var result = base.GetNodeInfo();

            if (Index.Kind != IndexKind.None)
            {
                result.Children.Add(Index.GetNodeInfo());
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.Write(ToString());
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WriteNullable(Name);
            Index.Serialize(writer);
            writer.Write(SubFieldCode);
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append("$");
            result.Append(Name);
            if (Index.Kind != IndexKind.None)
            {
                result.Append(Index.ToText());
            }

            if (SubFieldCode != SubField.NoCode)
            {
                result.Append('^');
                result.Append(SubFieldCode);
            }


            return result.ToString();
        }

        #endregion
    }
}
