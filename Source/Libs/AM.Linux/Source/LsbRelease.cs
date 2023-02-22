// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* LsbRelease.cs -- файл /etc/lsb-release
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Runtime.Versioning;

using AM.IO.PropFiles;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Linux;

/// <summary>
/// Файл <c>/etc/lsb-release</c>,
/// содержащий сведения о выпуске операционной системы.
/// </summary>
[PublicAPI]
[SupportedOSPlatform ("linux")]
public sealed class LsbRelease
{
    #region Properties

    /// <summary>
    /// 
    /// </summary>
    [Prop ("DISTRIB_ID")]
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("DISTRIB_RELEASE")]
    public string? Release { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("DISTRIB_RELEASE")]
    public string? Codename { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("DISTRIB_DESCRIPTION")]
    public string? Description { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Чтение файла <c>/etc/lsb-release</c>.
    /// </summary>
    public static LsbRelease? ReadFile
        (
            string fileName = "/etc/lsb-release"
        )
    {
        Sure.NotNullNorEmpty (fileName);

        if (!File.Exists (fileName))
        {
            return null;
        }

        var result = new LsbRelease();
        PropFile.Read (result, fileName);

        return result;
    }

    #endregion
}
