// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* LoadBusinessObjectEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting.Data;

/// <summary>
/// Provides data for <see cref="LoadBusinessObjectEventHandler"/> event.
/// </summary>
public class LoadBusinessObjectEventArgs
{
    #region Properties

    /// <summary>
    /// Parent object for this data source.
    /// </summary>
    public object? Parent { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="parent"></param>
    internal LoadBusinessObjectEventArgs
        (
            object? parent
        )
    {
        Parent = parent;
    }

    #endregion
}
