// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftBoolean.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PftBoolean
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Boolean value.
        /// </summary>
        public virtual bool Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftBoolean()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftBoolean
            (
                PftToken token
            )
            : base(token)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftBoolean
            (
                params PftNode[] children
            )
            : base(children)
        {
        }

        #endregion

        #region PftNode members

        ///// <inheritdoc cref="PftNode.Execute" />
        //public override void Execute
        //    (
        //        PftContext context
        //    )
        //{
        //    OnBeforeExecution(context);

        //    base.Execute(context);

        //    OnAfterExecution(context);
        //}

        #endregion
    }
}
