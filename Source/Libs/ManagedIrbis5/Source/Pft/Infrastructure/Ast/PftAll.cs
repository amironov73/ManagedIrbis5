﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftAll.cs -- проверяет, выполняется ли указанное условие для всех повторений группы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using AM;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Проверяет, выполняется ли указанное условие для всех повторений группы.
    /// </summary>
    public sealed class PftAll
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Проверяемое условие.
        /// </summary>
        public PftCondition? InnerCondition { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    if (!ReferenceEquals(InnerCondition, null))
                    {
                        var nodes = new List<PftNode>
                        {
                            InnerCondition
                        };
                        _virtualChildren.SetChildren(nodes);
                    }
                }

                return _virtualChildren;
            }

            [ExcludeFromCodeCoverage]
            protected set => Magna.Error
                (
                    nameof(PftAll) + "::" + nameof(Children)
                    + ": set value="
                    + value.ToVisibleString()
                );
        }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftAll()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftAll
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.All);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftAll
            (
                [NotNull] PftCondition condition
            )
        {
            InnerCondition = condition;
        }

        #endregion

        #region Private members

        private VirtualChildren? _virtualChildren;

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            var result = (PftAll)base.Clone();

            if (!ReferenceEquals(InnerCondition, null))
            {
                result.InnerCondition = (PftCondition)InnerCondition.Clone();
            }

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

            PftSerializationUtility.CompareNodes
                (
                    InnerCondition,
                    ((PftAll)otherNode).InnerCondition
                );
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            InnerCondition
                = (PftCondition?) PftSerializer.DeserializeNullable(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                Magna.Error
                    (
                        nameof(PftAll) + "::" + nameof(Execute)
                        + ": nested group detected"
                    );

                throw new PftSemanticException("Nested group");
            }

            var condition = InnerCondition.ThrowIfNull(nameof(InnerCondition));

            var group = new PftGroup();

            try
            {
                context.CurrentGroup = group;
                context.VMonitor = false;

                OnBeforeExecution(context);

                var value = false;

                for (
                        context.Index = 0;
                        context.Index < PftConfig.MaxRepeat;
                        context.Index++
                    )
                {
                    context.VMonitor = false;

                    condition.Execute(context);

                    if (!context.VMonitor || context.BreakFlag)
                    {
                        break;
                    }

                    value = condition.Value;
                    if (!value)
                    {
                        break;
                    }
                }

                Value = value;

                OnAfterExecution(context);
            }
            finally
            {
                context.CurrentGroup = null;
            }
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            var result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name)
            };

            if (!ReferenceEquals(InnerCondition, null))
            {
                result.Children.Add(InnerCondition.GetNodeInfo());
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write("all(");
            InnerCondition?.PrettyPrint(printer);
            printer.Write(')');
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, InnerCondition);
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
            result.Append("all(");
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return result.ToString();
        }

        #endregion

    } // class PftAll

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
