﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftParallelWith.cs -- параллельная версия "with"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Параллельная версия "with"
    /// </summary>
    public sealed class PftParallelWith
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Variable reference.
        /// </summary>
        public PftVariableReference? Variable { get; set; }

        /// <summary>
        /// Fields.
        /// </summary>
        public NonNullCollection<FieldSpecification> Fields { get; private set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax"/>
        public override bool ExtendedSyntax => true;

        /// <inheritdoc cref="PftNode.Children"/>
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    var nodes = new List<PftNode>();
                    if (!ReferenceEquals(Variable, null))
                    {
                        nodes.Add(Variable);
                    }
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set => Magna.Error
                (
                    "PftParallelWith::Children: "
                    + "set value="
                    + value.ToVisibleString()
                );
        } // property Children

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParallelWith()
        {
            Fields = new();
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParallelWith
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Parallel);
            Fields = new();
        } // constructor

        #endregion

        #region Private members

        private VirtualChildren? _virtualChildren;

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="PftNode.Clone" />
        public override object Clone()
        {
            var result = (PftParallelWith)base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(Variable, null))
            {
                result.Variable = (PftVariableReference)Variable.Clone();
            }

            result.Fields = new NonNullCollection<FieldSpecification>();
            foreach (var field in Fields)
            {
                result.Fields.Add
                (
                    (FieldSpecification)field.Clone()
                );
            }

            return result;
        } // method Clone

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            base.Execute(context);

            // TODO: implement

            OnAfterExecution(context);
        } // method Execute

        #endregion

    } // class PftParallelWith

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
