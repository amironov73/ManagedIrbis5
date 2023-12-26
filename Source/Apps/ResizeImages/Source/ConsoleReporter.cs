// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* ConsoleReporter.cs -- рапортует о прогрессе в консоли
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

namespace ResizeImages;

/// <summary>
/// Рапортует о прогрессе в консоли.
/// </summary>
internal sealed class ConsoleReporter
    : IExtendedProgress<int>
{
    #region Private members

    private int _maximum;

    #endregion

    #region IProgress members

    /// <inheritdoc cref="IProgress{T}.Report"/>
    public void Report
        (
            int value
        )
    {
        var text = $"{value}".PadRight (Console.BufferWidth);
        Console.Write (text);
        Console.Write ('\r');
    }

    #endregion

    #region IExtendedProgress members

    /// <inheritdoc cref="IExtendedProgress{T}.SetMaximum"/>
    public void SetMaximum
        (
            int maximum
        )
    {
        _maximum = maximum;
    }

    public void ExtendedReport
        (
            int value,
            string? message
        )
    {
        var text = $"{value}/{_maximum}: {message}".PadRight (Console.BufferWidth);
        Console.Write (text);
        Console.Write ('\r');
    }

    /// <inheritdoc cref="IExtendedProgress{T}.ReportError"/>
    public void ReportError
        (
            int value,
            string? message
        )
    {
        Console.Error.WriteLine ($"{value}: {message}");
    }

    #endregion
}
