// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftRsum.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftRsum
        : PftNumeric
    {
        #region Properties

        /// <summary>
        /// Name of the function.
        /// </summary>
        public string? Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRsum()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRsum
            (
                string name
            )
        {
            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRsum
            (
                string name,
                params PftNode[] children
            )
        {
            Name = name;
            foreach (var node in children)
            {
                Children.Add(node);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRsum
            (
                PftToken token
            )
            : base(token)
        {
            Name = token.Text;
            if (string.IsNullOrEmpty(Name))
            {
                Magna.Error
                    (
                        "PftRsum::Constructor: "
                        + "Name="
                        + Name.ToVisibleString()
                    );

                throw new PftSyntaxException("Name");
            }
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

            PftRsum otherRsum = (PftRsum)otherNode;
            bool result = Name == otherRsum.Name;
            if (!result)
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
            compiler.CompileNodes(Children);

            string actionName = compiler.CompileAction(Children);

            compiler.StartMethod(this);

            string functionName;
            switch (Name)
            {
                case "rsum":
                    functionName = "Sum";
                    break;

                case "rmin":
                    functionName = "Min";
                    break;

                case "rmax":
                    functionName = "Max";
                    break;

                case "ravr":
                    functionName = "Average";
                    break;

                default:
                    throw new PftCompilerException();
            }

            compiler
                .WriteIndent()
                .WriteLine("double result = 0.0;");

            if (!string.IsNullOrEmpty(actionName))
            {
                compiler
                    .WriteIndent()
                    .Write("string text = Evaluate({0});", actionName);

                compiler
                    .WriteIndent()
                    .WriteLine("double[] values = PftUtility.ExtractNumericValues(text);")
                    .WriteIndent()
                    .WriteLine("if (values.Length != 0)")
                    .WriteIndent()
                    .WriteLine("{")
                    .IncreaseIndent()
                    .WriteIndent()
                    .WriteLine("result = values.{0}();", functionName)
                    .DecreaseIndent()
                    .WriteIndent()
                    .WriteLine("}");
            }

            compiler
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
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            Value = 0.0;

            if (!string.IsNullOrEmpty(Name))
            {
                using var guard = new PftContextGuard(context);

                // ibatrak
                // Оказывается, функции RMIN, RMAX, RAVR и RSUM игнорируют
                // состояние повторяющейся группы.
                // То есть, если туда передается поле, то в параметры передается
                // выражение, слепленное из всех повторений
                // Ровно так, если бы поле выводилось без повторяющейся группы.
                // А повторяющаяся группа будет просто играть роль цикла.

                // TODO добавить этот глюк в компилируемый код

                var nestedContext = guard.ChildContext;
                nestedContext.Reset();

                string text = nestedContext.Evaluate(Children);
                double[] values = PftUtility.ExtractNumericValues(text);
                if (values.Length != 0)
                {
                    switch (Name)
                    {
                        case "rsum":
                            Value = values.Sum();
                            break;

                        case "rmin":
                            Value = values.Min();
                            break;

                        case "rmax":
                            Value = values.Max();
                            break;

                        case "ravr":
                            Value = values.Average();
                            break;

                        default:
                            Magna.Error
                            (
                                "PftRsum::Execute: "
                                + "unexpected function name="
                                + Name.ToVisibleString()
                            );

                            throw new PftSyntaxException(this);
                    }
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer
                .SingleSpace()
                .Write(Name)
                .Write('(')
                .WriteNodes(Children)
                .Write(')');
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WriteNullable(Name);
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(Name);
            result.Append('(');
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return result.ToString();
        }

        #endregion
    }
}
