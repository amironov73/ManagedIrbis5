// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* Utilities.cs -- conversion metrics utilities
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

#endregion

#nullable enable

namespace NetCoreServer;

/// <summary>
/// Conversion metrics utilities
/// </summary>
public class Utilities
{
    /// <summary>
    /// Generate data size string. Will return a pretty string of bytes, KiB, MiB, GiB, TiB based on the given bytes.
    /// </summary>
    /// <param name="b">Data size in bytes</param>
    /// <returns>String with data size representation</returns>
    public static string GenerateDataSize (double b)
    {
        var builder = StringBuilderPool.Shared.Get();

        var bytes = (long)b;
        var absBytes = Math.Abs (bytes);

        if (absBytes >= (1024L * 1024L * 1024L * 1024L))
        {
            var tb = bytes / (1024L * 1024L * 1024L * 1024L);
            var gb = (bytes % (1024L * 1024L * 1024L * 1024L)) / (1024 * 1024 * 1024);
            builder.Append (tb);
            builder.Append ('.');
            builder.Append ((gb < 100) ? "0" : "");
            builder.Append ((gb < 10) ? "0" : "");
            builder.Append (gb);
            builder.Append (" TiB");
        }
        else if (absBytes >= (1024 * 1024 * 1024))
        {
            var gb = bytes / (1024 * 1024 * 1024);
            var mb = (bytes % (1024 * 1024 * 1024)) / (1024 * 1024);
            builder.Append (gb);
            builder.Append ('.');
            builder.Append ((mb < 100) ? "0" : "");
            builder.Append ((mb < 10) ? "0" : "");
            builder.Append (mb);
            builder.Append (" GiB");
        }
        else if (absBytes >= (1024 * 1024))
        {
            var mb = bytes / (1024 * 1024);
            var kb = (bytes % (1024 * 1024)) / 1024;
            builder.Append (mb);
            builder.Append ('.');
            builder.Append ((kb < 100) ? "0" : "");
            builder.Append ((kb < 10) ? "0" : "");
            builder.Append (kb);
            builder.Append (" MiB");
        }
        else if (absBytes >= 1024)
        {
            var kb = bytes / 1024;
            bytes = bytes % 1024;
            builder.Append (kb);
            builder.Append ('.');
            builder.Append ((bytes < 100) ? "0" : "");
            builder.Append ((bytes < 10) ? "0" : "");
            builder.Append (bytes);
            builder.Append (" KiB");
        }
        else
        {
            builder.Append (bytes);
            builder.Append (" bytes");
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Generate time period string. Will return a pretty string of ns, mcs, ms, s, m, h based on the given nanoseconds.
    /// </summary>
    /// <param name="ms">Milliseconds</param>
    /// <returns>String with time period representation</returns>
    public static string GenerateTimePeriod (double ms)
    {
        var builder = StringBuilderPool.Shared.Get();

        var nanoseconds = (long)(ms * 1000.0 * 1000.0);
        var absNanoseconds = Math.Abs (nanoseconds);

        if (absNanoseconds >= (60 * 60 * 1000000000L))
        {
            var hours = nanoseconds / (60 * 60 * 1000000000L);
            var minutes = ((nanoseconds % (60 * 60 * 1000000000L)) / 1000000000) / 60;
            var seconds = ((nanoseconds % (60 * 60 * 1000000000L)) / 1000000000) % 60;
            var milliseconds = ((nanoseconds % (60 * 60 * 1000000000L)) % 1000000000) / 1000000;
            builder.Append (hours);
            builder.Append (':');
            builder.Append ((minutes < 10) ? "0" : "");
            builder.Append (minutes);
            builder.Append (':');
            builder.Append ((seconds < 10) ? "0" : "");
            builder.Append (seconds);
            builder.Append ('.');
            builder.Append ((milliseconds < 100) ? "0" : "");
            builder.Append ((milliseconds < 10) ? "0" : "");
            builder.Append (milliseconds);
            builder.Append (" h");
        }
        else if (absNanoseconds >= (60 * 1000000000L))
        {
            var minutes = nanoseconds / (60 * 1000000000L);
            var seconds = (nanoseconds % (60 * 1000000000L)) / 1000000000;
            var milliseconds = ((nanoseconds % (60 * 1000000000L)) % 1000000000) / 1000000;
            builder.Append (minutes);
            builder.Append (':');
            builder.Append ((seconds < 10) ? "0" : "");
            builder.Append (seconds);
            builder.Append ('.');
            builder.Append ((milliseconds < 100) ? "0" : "");
            builder.Append ((milliseconds < 10) ? "0" : "");
            builder.Append (milliseconds);
            builder.Append (" m");
        }
        else if (absNanoseconds >= 1000000000)
        {
            var seconds = nanoseconds / 1000000000;
            var milliseconds = (nanoseconds % 1000000000) / 1000000;
            builder.Append (seconds);
            builder.Append ('.');
            builder.Append ((milliseconds < 100) ? "0" : "");
            builder.Append ((milliseconds < 10) ? "0" : "");
            builder.Append (milliseconds);
            builder.Append (" s");
        }
        else if (absNanoseconds >= 1000000)
        {
            var milliseconds = nanoseconds / 1000000;
            var microseconds = (nanoseconds % 1000000) / 1000;
            builder.Append (milliseconds);
            builder.Append ('.');
            builder.Append ((microseconds < 100) ? "0" : "");
            builder.Append ((microseconds < 10) ? "0" : "");
            builder.Append (microseconds);
            builder.Append (" ms");
        }
        else if (absNanoseconds >= 1000)
        {
            var microseconds = nanoseconds / 1000;
            nanoseconds = nanoseconds % 1000;
            builder.Append (microseconds);
            builder.Append ('.');
            builder.Append ((nanoseconds < 100) ? "0" : "");
            builder.Append ((nanoseconds < 10) ? "0" : "");
            builder.Append (nanoseconds);
            builder.Append (" mcs");
        }
        else
        {
            builder.Append (nanoseconds);
            builder.Append (" ns");
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }
}
