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

/* Triple.cs -- триплет
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Rdf;

/// <summary>
/// Триплет.
/// </summary>
public sealed class Triple
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public INode Subject { get; }

    /// <summary>
    ///
    /// </summary>
    public INode Predicate { get; }

    /// <summary>
    ///
    /// </summary>
    public INode Object { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Triple
        (
            INode subject,
            INode predicate,
            INode obj
        )
    {
        Subject = subject;
        Predicate = predicate;
        Object = obj;
    }

    #endregion
}
