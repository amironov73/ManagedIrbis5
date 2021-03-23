// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftProcedureDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftProcedureDefinition
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Procedure itself.
        /// </summary>
        public PftProcedure? Procedure { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Procedure, null))
                    {
                        nodes.AddRange(Procedure.Body);
                    }
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set => Magna.Error
                (
                    "PftProcedureDefinition::Children: "
                    + "set value="
                    + value.ToVisibleString()
                );
        } // property Children

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedureDefinition()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedureDefinition
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="PftNode.Clone" />
        public override object Clone()
        {
            PftProcedureDefinition result
                = (PftProcedureDefinition) base.Clone();

            if (!ReferenceEquals(Procedure, null))
            {
                result.Procedure = (PftProcedure) Procedure.Clone();
            }

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

            bool flag = reader.ReadBoolean();
            if (flag)
            {
                Procedure = new PftProcedure();
                Procedure.Deserialize(reader);
            }
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            // Do something?

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "Procedure"
            };

            if (!ReferenceEquals(Procedure, null))
            {
                result.Value = Procedure.Name;

                PftNodeInfo body = new PftNodeInfo
                {
                    Name = "Body"
                };
                result.Children.Add(body);

                foreach (PftNode node in Procedure.Body)
                {
                    body.Children.Add(node.GetNodeInfo());
                }
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

            if (ReferenceEquals(Procedure, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                Procedure.Serialize(writer);
            }
        }

        #endregion
    }
}
