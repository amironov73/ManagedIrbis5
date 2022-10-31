// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PlatformHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms.VisualStyles;

using Microsoft.Win32;

#endregion

#nullable enable

namespace AeroSuite;

/// <summary>
/// Provides helper functions for platform management
/// </summary>
public static class PlatformHelper
{
    /// <summary>
    /// Initializes the <see cref="PlatformHelper"/> class.
    /// </summary>
    static PlatformHelper()
    {
        Win32NT = Environment.OSVersion.Platform == PlatformID.Win32NT;
        XpOrHigher = Win32NT && Environment.OSVersion.Version.Major >= 5;
        VistaOrHigher = Win32NT && Environment.OSVersion.Version.Major >= 6;
        SevenOrHigher = Win32NT && (Environment.OSVersion.Version >= new Version(6, 1));
        EightOrHigher = Win32NT && (Environment.OSVersion.Version >= new Version(6, 2, 9200));
        VisualStylesEnabled = VisualStyleInformation.IsEnabledByUser;

        SystemEvents.UserPreferenceChanged += (_, _) =>
        {
            var vsEnabled = VisualStyleInformation.IsEnabledByUser;
            if (vsEnabled != VisualStylesEnabled)
            {
                VisualStylesEnabled = vsEnabled;
                //Maybe raise an event here
            }
        };
    }

    /// <summary>
    /// Returns a indicating whether the Operating System is Windows 32 NT based.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the Operating System is Windows 32 NT based; otherwise, <c>false</c>.
    /// </value>
    public static bool Win32NT { get; }

    /// <summary>
    /// Returns a value indicating whether the Operating System is Windows XP or higher.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the Operating System is Windows 8 or higher; otherwise, <c>false</c>.
    /// </value>
    public static bool XpOrHigher { get; }

    /// <summary>
    /// Returns a value indicating whether the Operating System is Windows Vista or higher.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the Operating System is Windows Vista or higher; otherwise, <c>false</c>.
    /// </value>
    public static bool VistaOrHigher { get; }

    /// <summary>
    /// Returns a value indicating whether the Operating System is Windows 7 or higher.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the Operating System is Windows 7 or higher; otherwise, <c>false</c>.
    /// </value>
    public static bool SevenOrHigher { get; }

    /// <summary>
    /// Returns a value indicating whether the Operating System is Windows 8 or higher.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the Operating System is Windows 8 or higher; otherwise, <c>false</c>.
    /// </value>
    public static bool EightOrHigher { get; }

    /// <summary>
    /// Returns a value indicating whether Visual Styles are enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> if Visual Styles are enabled; otherwise, <c>false</c>.
    /// </value>
    public static bool VisualStylesEnabled { get; private set; }
}
