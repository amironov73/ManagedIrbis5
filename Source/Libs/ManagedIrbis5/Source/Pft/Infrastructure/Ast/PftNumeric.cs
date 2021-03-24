// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PftNumeric.cs -- некое числовое значение (абстрактный класс)
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Некое числовое значение (абстрактный класс).
    /// От этого класса наследуются все математические выражения.
    /// </summary>
    public abstract class PftNumeric
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Вычисленное значение.
        /// </summary>
        public double Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftNumeric()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftNumeric
            (
                double value
            )
        {
            Value = value;
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftNumeric
            (
                PftToken token
            )
            : base(token)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftNumeric
            (
                params PftNode[] children
            )
            : base(children)
        {
        }

        #endregion

    } // class PftNumeric

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
