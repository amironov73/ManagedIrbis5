// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftCondition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Условие
    /// </summary>
    public abstract class PftCondition
        : PftBoolean
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftCondition()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftCondition
            (
                PftToken token
            )
            : base(token)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftCondition
            (
                params PftNode[] children
            )
            : base(children)
        {
        }

        #endregion

        #region PftNode members

        ///// <inheritdoc cref="PftBoolean.Execute" />
        //[ExcludeFromCodeCoverage]
        //public override void Execute
        //    (
        //        PftContext context
        //    )
        //{
        //    Log.Error
        //        (
        //            "PftCondition::Execute: "
        //            + "must be overridden"
        //        );

        //    throw new PftException("Execute() must be overridden!");
        //}

        #endregion
    }
}
