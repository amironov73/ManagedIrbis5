// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* AsnClass.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Rdf;

/// <summary>
/// Интерфейс хранилища триплетов.
/// </summary>
public interface ITripleStore
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="graph"></param>
    /// <returns></returns>
    bool Add (IGraph graph);

    /// <summary>
    ///
    /// </summary>
    /// <param name="graphUri"></param>
    /// <returns></returns>
    bool HasGraph (Uri graphUri);

    /// <summary>
    ///
    /// </summary>
    /// <param name="graphUri"></param>
    IGraph this [Uri graphUri] { get; }
}
