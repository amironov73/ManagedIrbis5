// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SemiConnectedClient.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Connected IRBIS client with some local functionality
    /// </summary>
    public class SemiConnectedClient
        : ConnectedClient
    {
        #region Constructrion

        /// <summary>
        /// Constructor.
        /// </summary>
        public SemiConnectedClient()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SemiConnectedClient
            (
                IIrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region IrbisProvider members

        /// <inheritdoc cref="ConnectedClient.FormatRecord" />
        public override string FormatRecord
            (
                Record record,
                string format
            )
        {
            // TODO some caching

            var program = PftUtility.CompileProgram(format);
            var context = new PftContext(null)
            {
                Record = record
            };
            context.SetProvider(this);
            program.Execute(context);
            string result = context.GetProcessedOutput();

            return result;
        }

        #endregion
    }
}
