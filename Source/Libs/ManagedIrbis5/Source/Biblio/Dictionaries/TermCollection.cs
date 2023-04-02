// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TermCollection.cs -- коллекция терминов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Collections;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Коллекция терминов.
/// </summary>
[PublicAPI]
public sealed class TermCollection
    : NonNullCollection<BiblioTerm>,
    IVerifiable
{
    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<TermCollection> (this, throwOnError);

        // TODO do something

        return verifier.Result;
    }

    #endregion
}
