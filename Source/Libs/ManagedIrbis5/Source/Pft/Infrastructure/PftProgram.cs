﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftProgram.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.Text.Output;



using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Text;


#endregion

// ReSharper disable VirtualMemberCallInConstructor

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// AST root node.
    /// </summary>

    public class PftProgram
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Procedures.
        /// </summary>
        public PftProcedureManager Procedures { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProgram()
        {
            Procedures = new PftProcedureManager();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProgram
            (
                params PftNode[] nodes
            )
            : this ()
        {
            foreach (PftNode node in nodes)
            {
                Children.Add(node);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Dump the program.
        /// </summary>
        public string DumpToText()
        {
            AbstractOutput output = new TextOutput();
            PftNodeInfo nodeInfo = GetNodeInfo();
            PftNodeInfo.Dump(output, nodeInfo, 0);

            return output.ToString();
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

            Procedures.Deserialize(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            Code.NotNull(context, "context");

            try
            {
                context.Procedures = Procedures;
                base.Execute(context);
            }
            catch (PftBreakException exception)
            {
                // It was break operator

                Log.TraceException
                    (
                        "PftProgram::Execute",
                        exception
                    );

                if (!ReferenceEquals(context.Parent, null))
                {
                    throw;
                }
            }
            catch (PftExitException exception)
            {
                // It was exit operator

                Log.TraceException
                    (
                        "PftProgram::Execute",
                        exception
                    );

                if (!ReferenceEquals(context.Parent, null))
                {
                    throw;
                }
            }
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            Procedures.Serialize(writer);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString" />
        public override string ToString()
        {
            using (PftPrettyPrinter printer = new PftPrettyPrinter())
            {
                PrettyPrint(printer);
                string result = printer.ToString();

                return result;
            }
        }

        #endregion
    }
}
