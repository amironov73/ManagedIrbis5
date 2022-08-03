// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* VersionUtils.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Reflection;

using ManagedIrbis.Epub.Schema;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Internal;

/// <summary>
///
/// </summary>
internal static class VersionUtils
{
    /// <summary>
    ///
    /// </summary>
    public static string GetVersionString
        (
            EpubVersion epubVersion
        )
    {
        var epubVersionType = typeof (EpubVersion);
        var fieldInfo = epubVersionType.GetRuntimeField (epubVersion.ToString());
        if (fieldInfo != null)
        {
            return fieldInfo.GetCustomAttribute<VersionStringAttribute>()?.Version ?? string.Empty;
        }
        else
        {
            return epubVersion.ToString();
        }
    }
}
