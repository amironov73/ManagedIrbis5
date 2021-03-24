// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FunctionParameter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Function parameter.
    /// </summary>
    public enum FunctionParameter
    {
        /// <summary>
        /// String.
        /// </summary>
        String = (int)'s',

        /// <summary>
        /// Required string.
        /// </summary>
        RequiredString = (int)'S',

        /// <summary>
        /// Numeric.
        /// </summary>
        Numeric = (int)'n',

        /// <summary>
        /// Required numeric.
        /// </summary>
        RequiredNumeric = (int)'N',

        /// <summary>
        /// Boolean.
        /// </summary>
        Boolean = (int)'b',

        /// <summary>
        /// Required boolean.
        /// </summary>
        RequiredBoolean = (int)'B'
    }
}
