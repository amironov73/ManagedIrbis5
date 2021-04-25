// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

/* DigestAuthenticator.cs -- костыль, позволяющий аутентифицироваться методом Digest
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.Net;

using RestSharp;
using RestSharp.Authenticators;

#endregion

#nullable enable

#pragma warning disable 618

namespace RestfulIrbis
{
    /// <summary>
    /// Костыль, позволяющий аутентифицироваться методом Digest
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DigestAuthenticator
        : IAuthenticator
    {
        #region Properties

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DigestAuthenticator
            (
                string username,
                string password
            )
        {
            UserName = username;
            Password = password;
        }

        #endregion

        #region IAuthenticator members

        /// <inheritdoc/>
        public void Authenticate
            (
                IRestClient client,
                IRestRequest request
            )
        {
            // TODO: нужен актуальный способ, а не костыль

            request.Credentials = new NetworkCredential
                (
                    UserName,
                    Password
                );
        }

        #endregion
    }
}
