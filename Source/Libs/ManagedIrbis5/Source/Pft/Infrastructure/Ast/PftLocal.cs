﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftLocal.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Задаёт локальный контекст для переменных.
    /// </summary>
    /// <example>
    /// <code>
    /// $x = 'outer';
    /// local $x, $y,
    /// do
    ///   $x = 'inner';
    ///   $y = 'another';
    ///   $x # $y #
    /// end
    ///
    /// $x
    /// </code>
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftLocal
        : PftNode
    {
        #region Properties

        /// <summary>
        /// List of names.
        /// </summary>
        [NotNull]
        public NonNullCollection<string> Names { get; private set; }

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
        public PftLocal()
        {
            Names = new NonNullCollection<string>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftLocal
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Local);

            Names = new NonNullCollection<string>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="PftNode.Clone" />
        public override object Clone()
        {
            PftLocal result = (PftLocal) base.Clone();

            result.Names = new NonNullCollection<string>();
            result.Names.AddRange(Names);

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
            base.Deserialize(reader);

            Names.Clear();
            Names.AddRange(reader.ReadStringArray());
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            using (PftContextGuard guard = new PftContextGuard(context))
            {
                PftContext localContext = guard.ChildContext;
                localContext.Output = context.Output;
                PftVariableManager localManager
                    = new PftVariableManager(context.Variables);
                localContext.SetVariables(localManager);

                foreach (string name in Names)
                {
                    PftVariable variable = new PftVariable
                        (
                            name,
                            false
                        );

                    localManager.Registry.Add
                        (
                            name,
                            variable
                        );
                }

                localContext.Execute(Children);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "Local",
                Value = string.Join
                    (
                        ", ",
                        Names.ToArray()
                    )
            };

            foreach (PftNode node in Children)
            {
                result.Children.Add(node.GetNodeInfo());
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

            printer
                .WriteLine()
                .WriteIndent()
                .Write("local ");

            bool first = true;
            foreach (string name in Names)
            {
                if (!first)
                {
                    printer.Write(", ");
                }
                printer.Write('$');
                printer.Write(name);
                first = false;
            }

            printer
                .WriteLine()
                .WriteIndent()
                .WriteLine("do");

            printer.IncreaseLevel();
            printer.WriteNodes(Children);
            printer.DecreaseLevel();
            printer.EatWhitespace();
            printer.EatNewLine();
            printer.WriteLine();
            printer
                .WriteIndent()
                .WriteLine("end");
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WriteArray(Names.ToArray());
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
            StringBuilder result = new StringBuilder();
            result.Append("local ");
            result.Append
                (
                    StringUtility.Join
                        (
                            ", ",
                            Names.Select(name => "$" + name)
                        )
                );
            result.Append(" do ");
            PftUtility.NodesToText(result, Children);
            result.Append(" end");

            return result.ToString();
        }

        #endregion
    }
}
