// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* OsRelease.cs -- файл /etc/os-release
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.Versioning;

using AM.IO.PropFiles;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Linux;

/// <summary>
/// Файл <c>/etc/os-release</c>,
/// содержащий сведения о выпуске операционной системы.
/// </summary>
[PublicAPI]
[SupportedOSPlatform ("linux")]
public sealed class OsRelease
{
    #region Properties

    /// <summary>
    /// 
    /// </summary>
    [Prop ("PRETTY_NAME")]
    public string? PrettyName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("NAME")]
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("VERSION_ID")]
    public string? VersionId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("VERSION")]
    public string? Version { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("VERSION_CODENAME")]
    public string? VersionCodename { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("ID")]
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("ID_LIKE")]
    public string? IdLike { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("HomeUrl")]
    public string? HomeUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("SUPPORT_URL")]
    public string? SupportUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Prop ("BUG_REPORT_URL")]
    public string? BugReportUrl { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Prop ("PRIVACY_POLICY_URL")]
    public string? PrivacyPolicyUrl { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Чтение файла <c>/etc/os-release</c>.
    /// </summary>
    public static OsRelease ReadFile
        (
            string fileName = "/etc/os-release"
        )
    {
        Sure.FileExists (fileName);

        var result = new OsRelease();
        PropFile.Read (result, fileName);

        return result;
    }

    #endregion
}
