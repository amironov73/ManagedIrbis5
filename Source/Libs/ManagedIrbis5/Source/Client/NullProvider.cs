// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* NullProvider.cs -- null provider used for testing
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives


#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Null provider used for testing.
    /// </summary>
    public sealed class NullProvider
        : IrbisProvider
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NullProvider()
        {
            Database = "IBIS";
        }

        #endregion

    } // class NullProvider

} // namespace ManagedIrbis.Client
