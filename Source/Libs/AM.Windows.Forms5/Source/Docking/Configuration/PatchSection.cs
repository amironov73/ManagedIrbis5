// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* PatchSection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Configuration;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking.Configuration;

/// <summary>
///
/// </summary>
public class PatchSection
    : ConfigurationSection
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableAll", DefaultValue = null)]
    public bool? EnableAll => (bool?)base["enableAll"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableHighDpi", DefaultValue = true)]
    public bool EnableHighDpi => (bool)base["enableHighDpi"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableMemoryLeakFix", DefaultValue = true)]
    public bool EnableMemoryLeakFix => (bool)base["enableMemoryLeakFix"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableMainWindowFocusLostFix", DefaultValue = true)]
    public bool EnableMainWindowFocusLostFix => (bool)base["enableMainWindowFocusLostFix"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableNestedDisposalFix", DefaultValue = true)]
    public bool EnableNestedDisposalFix => (bool)base["enableNestedDisposalFix"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableFontInheritanceFix", DefaultValue = true)]
    public bool EnableFontInheritanceFix => (bool)base["enableFontInheritanceFix"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableContentOrderFix", DefaultValue = true)]
    public bool EnableContentOrderFix => (bool)base["enableContentOrderFix"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableActiveXFix", DefaultValue = false)] // disabled by default to avoid side effect.
    public bool EnableActiveXFix => (bool)base["enableActiveXFix"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableDisplayingPaneFix", DefaultValue = true)]
    public bool EnableDisplayingPaneFix => (bool)base["enableDisplayingPaneFix"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableActiveControlFix", DefaultValue = true)]
    public bool EnableActiveControlFix => (bool)base["enableActiveControlFix"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableFloatSplitterFix", DefaultValue = true)]
    public bool EnableFloatSplitterFix => (bool)base["enableFloatSplitterFix"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableActivateOnDockFix", DefaultValue = true)]
    public bool EnableActivateOnDockFix => (bool)base["enableActivateOnDockFix"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enableSelectClosestOnClose", DefaultValue = true)]
    public bool EnableSelectClosestOnClose => (bool)base["enableSelectClosestOnClose"];

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty ("enablePerScreenDpi", DefaultValue = false)]
    public bool EnablePerScreenDpi => (bool)base["enablePerScreenDpi"];
}
