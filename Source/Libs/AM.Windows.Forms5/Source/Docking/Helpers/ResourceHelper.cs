// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* ResourceHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Resources;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
internal static class ResourceHelper
{
    #region Private members

    private static ResourceManager? _resourceManager;

    // TODO исправить путь к ресурсам
    private static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager
            (
                "WeifenLuo.WinFormsUI.Docking.Strings",
                typeof (ResourceHelper).Assembly
            );

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public static string? GetString
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        return ResourceManager.GetString (name);
    }

    #endregion
}
