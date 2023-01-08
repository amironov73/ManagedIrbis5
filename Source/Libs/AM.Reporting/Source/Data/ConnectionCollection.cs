// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ConnectionCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Data;

/// <summary>
/// Represents the collection of <see cref="DataConnectionBase"/> objects.
/// </summary>
public class ConnectionCollection
    : ReportCollectionBase
{
    #region Properties

    /// <summary>
    /// Gets or sets a data connection.
    /// </summary>
    /// <param name="index">The index of a data connection in this collection.</param>
    /// <returns>The data connection with specified index.</returns>
    public DataConnectionBase this [int index]
    {
        get => (DataConnectionBase) List[index]!;
        set => List[index] = value;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionCollection"/> class with default settings.
    /// </summary>
    /// <param name="owner">The owner of this collection.</param>
    public ConnectionCollection
        (
            Base owner
        )
        : base (owner)
    {
        // пустое тело конструктора
    }

    #endregion
}
