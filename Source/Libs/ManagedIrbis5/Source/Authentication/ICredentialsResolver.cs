// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* ICredentialsResolver.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Authentication;

/// <summary>
/// Resolve missing connection elements.
/// </summary>
public interface ICredentialsResolver
{
    /// <summary>
    /// Resolve the credentials.
    /// </summary>
    bool ResolveCredentials
        (
            IrbisCredentials credentials,
            ConnectionElement elements
        );
}
