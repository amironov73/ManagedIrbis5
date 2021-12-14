// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ConstantNode.cs -- константное значение
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Констатное значение.
    /// </summary>
    sealed class ConstantNode : AtomNode
    {
        #region Construction

        public ConstantNode
            (
                dynamic? value
            )
        {
            _value = value;

            if (value is string rawText)
            {
                _value = Resolve.UnescapeText (rawText);
            }
        }

        #endregion

        #region Private members

        private readonly dynamic? _value;

        #endregion

        #region AtomNode members

        /// <inheritdoc cref="AtomNode.Compute"/>
        public override dynamic? Compute
            (
                Context context
            )
        {
            return _value;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return $"constant '{_value}'";
        }

        #endregion
    }
}
