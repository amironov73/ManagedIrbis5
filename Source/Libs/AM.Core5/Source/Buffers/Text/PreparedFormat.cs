// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* Utf16PreparedFormat.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;
using System.Buffers;

#endregion

namespace AM.Buffers.Text;

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1>
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    ///
    /// </summary>
    public int MinSize { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        Sure.NotNull (format);

        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    #endregion

    #region Private members

    private readonly Utf16FormatSegment[] _segments;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public string Format
        (
            T1 arg1
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }
    
    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        Sure.NotNull (format);

        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2
        )
        where TBufferWriter : IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3
        )
        where TBufferWriter : IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat (string format)
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5
        )
    {
        var sb = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6
        )
    {
        var sb = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5, arg6);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6, T7>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,T6 arg6,
            T7 arg7
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg7, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg7));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg7, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg8, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg8));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg7, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg8, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg9, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg9));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg7, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg8, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg9, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg10, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg10));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg7, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg8, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg9, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg10, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg10));
                    break;
                }
                case 10:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg11, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg11));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg7, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg8, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg9, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg10, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg10));
                    break;
                }
                case 10:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg11, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg11));
                    break;
                }
                case 11:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg12, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg12));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
/// 
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    /// <summary>
    /// 
    /// </summary>
    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13
        )
        where TBufferWriter: IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg7, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg8, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg9, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg10, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg10));
                    break;
                }
                case 10:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg11, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg11));
                    break;
                }
                case 11:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg12, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg12));
                    break;
                }
                case 12:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg13, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg13));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
/// 
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14
        )
        where TBufferWriter : IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg7, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg8, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg9, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg10, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg10));
                    break;
                }
                case 10:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg11, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg11));
                    break;
                }
                case 11:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg12, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg12));
                    break;
                }
                case 12:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg13, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg13));
                    break;
                }
                case 13:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg14, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg14));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
/// 
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    /// <summary>
    /// 
    /// </summary>
    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15
        )
        where TBufferWriter : IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg7, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg8, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg9, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg10, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg10));
                    break;
                }
                case 10:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg11, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg11));
                    break;
                }
                case 11:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg12, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg12));
                    break;
                }
                case 12:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg13, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg13));
                    break;
                }
                case 13:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg14, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg14));
                    break;
                }
                case 14:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg15, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg15));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
/// 
/// </summary>
public sealed partial class Utf16PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
{
    /// <summary>
    /// 
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 
    /// </summary>
    public int MinSize { get; }

    /// <summary>
    /// 
    /// </summary>
    private readonly Utf16FormatSegment[] _segments;

    /// <summary>
    /// 
    /// </summary>
    public Utf16PreparedFormat
        (
            string format
        )
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf16Parse (format);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15,
            T16 arg16
        )
    {
        var builder = new Utf16ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15, arg16);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15,
            T16 arg16
        )
        where TBufferWriter : IBufferWriter<char>
    {
        var formatSpan = FormatString.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf16FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg1, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg2, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg3, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg4, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg5, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg6, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg7, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg8, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg9, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg10, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg10));
                    break;
                }
                case 10:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg11, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg11));
                    break;
                }
                case 11:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg12, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg12));
                    break;
                }
                case 12:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg13, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg13));
                    break;
                }
                case 13:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg14, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg14));
                    break;
                }
                case 14:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg15, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg15));
                    break;
                }
                case 15:
                {
                    Utf16FormatHelper.FormatTo (ref buffer, arg16, item.Alignment,
                        formatSpan.Slice (item.Offset, item.Count), nameof (arg16));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5, arg6);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6, T7>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6,
        T7 arg7)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg7, item.Alignment, item.StandardFormat, nameof (arg7));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6,
        T7 arg7, T8 arg8)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg7, item.Alignment, item.StandardFormat, nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg8, item.Alignment, item.StandardFormat, nameof (arg8));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6,
        T7 arg7, T8 arg8, T9 arg9)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg7, item.Alignment, item.StandardFormat, nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg8, item.Alignment, item.StandardFormat, nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg9, item.Alignment, item.StandardFormat, nameof (arg9));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6,
        T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg7, item.Alignment, item.StandardFormat, nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg8, item.Alignment, item.StandardFormat, nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg9, item.Alignment, item.StandardFormat, nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg10, item.Alignment, item.StandardFormat, nameof (arg10));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
        T11 arg11)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6,
        T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg7, item.Alignment, item.StandardFormat, nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg8, item.Alignment, item.StandardFormat, nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg9, item.Alignment, item.StandardFormat, nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg10, item.Alignment, item.StandardFormat, nameof (arg10));
                    break;
                }
                case 10:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg11, item.Alignment, item.StandardFormat, nameof (arg11));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
        T11 arg11, T12 arg12)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6,
        T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg7, item.Alignment, item.StandardFormat, nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg8, item.Alignment, item.StandardFormat, nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg9, item.Alignment, item.StandardFormat, nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg10, item.Alignment, item.StandardFormat, nameof (arg10));
                    break;
                }
                case 10:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg11, item.Alignment, item.StandardFormat, nameof (arg11));
                    break;
                }
                case 11:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg12, item.Alignment, item.StandardFormat, nameof (arg12));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] segments;
    readonly byte[] utf8PreEncodedbuffer;

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    public string Format (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
        T11 arg11, T12 arg12, T13 arg13)
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    public void FormatTo<TBufferWriter> (ref TBufferWriter sb, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6,
        T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                {
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = sb.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    sb.Advance (item.Count);
                    break;
                }
                case 0:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;
                }
                case 1:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;
                }
                case 2:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;
                }
                case 3:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;
                }
                case 4:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;
                }
                case 5:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;
                }
                case 6:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg7, item.Alignment, item.StandardFormat, nameof (arg7));
                    break;
                }
                case 7:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg8, item.Alignment, item.StandardFormat, nameof (arg8));
                    break;
                }
                case 8:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg9, item.Alignment, item.StandardFormat, nameof (arg9));
                    break;
                }
                case 9:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg10, item.Alignment, item.StandardFormat, nameof (arg10));
                    break;
                }
                case 10:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg11, item.Alignment, item.StandardFormat, nameof (arg11));
                    break;
                }
                case 11:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg12, item.Alignment, item.StandardFormat, nameof (arg12));
                    break;
                }
                case 12:
                {
                    Utf8FormatHelper.FormatTo (ref sb, arg13, item.Alignment, item.StandardFormat, nameof (arg13));
                    break;
                }
                default:
                    break;
            }
        }
    }
}

/// <summary>
///
/// </summary>
public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    ///
    /// </summary>
    public int MinSize { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Utf8PreparedFormat
        (
            string format
        )
    {
        Sure.NotNull (format);

        FormatString = format;
        _segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    #endregion

    #region Private members

    private readonly Utf8FormatSegment[] _segments;
    private readonly byte[] utf8PreEncodedbuffer;

    #endregion

    #region Publuc methods

    /// <summary>
    ///
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14
        )
    {
        var sb = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref sb, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter builder,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14
        )
        where TBufferWriter: IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = builder.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    builder.Advance (item.Count);
                    break;

                case 0:
                    Utf8FormatHelper.FormatTo (ref builder, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;

                case 1:
                    Utf8FormatHelper.FormatTo (ref builder, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;

                case 2:
                    Utf8FormatHelper.FormatTo (ref builder, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;

                case 3:
                    Utf8FormatHelper.FormatTo (ref builder, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;

                case 4:
                    Utf8FormatHelper.FormatTo (ref builder, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;

                case 5:
                    Utf8FormatHelper.FormatTo (ref builder, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;

                case 6:
                    Utf8FormatHelper.FormatTo (ref builder, arg7, item.Alignment, item.StandardFormat, nameof (arg7));
                    break;

                case 7:
                    Utf8FormatHelper.FormatTo (ref builder, arg8, item.Alignment, item.StandardFormat, nameof (arg8));
                    break;

                case 8:
                    Utf8FormatHelper.FormatTo (ref builder, arg9, item.Alignment, item.StandardFormat, nameof (arg9));
                    break;

                case 9:
                    Utf8FormatHelper.FormatTo (ref builder, arg10, item.Alignment, item.StandardFormat, nameof (arg10));
                    break;

                case 10:
                    Utf8FormatHelper.FormatTo (ref builder, arg11, item.Alignment, item.StandardFormat, nameof (arg11));
                    break;

                case 11:
                    Utf8FormatHelper.FormatTo (ref builder, arg12, item.Alignment, item.StandardFormat, nameof (arg12));
                    break;

                case 12:
                    Utf8FormatHelper.FormatTo (ref builder, arg13, item.Alignment, item.StandardFormat, nameof (arg13));
                    break;

                case 13:
                    Utf8FormatHelper.FormatTo (ref builder, arg14, item.Alignment, item.StandardFormat, nameof (arg14));
                    break;

                default:
                    break;
            }
        }
    }

    #endregion
}

/// <summary>
/// /
/// </summary>
public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
{
    public string FormatString { get; }
    public int MinSize { get; }

    readonly Utf8FormatSegment[] _segments;
    readonly byte[] utf8PreEncodedbuffer;

    #region Construction

    public Utf8PreparedFormat (string format)
    {
        FormatString = format;
        _segments = PreparedFormatHelper.Utf8Parse (format, out utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15
        )
    {
        var builder = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter buffer,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15
        )
        where TBufferWriter : IBufferWriter<byte>
    {
        var formatSpan = utf8PreEncodedbuffer.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = buffer.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    buffer.Advance (item.Count);
                    break;

                case 0:
                    Utf8FormatHelper.FormatTo (ref buffer, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;

                case 1:
                    Utf8FormatHelper.FormatTo (ref buffer, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;

                case 2:
                    Utf8FormatHelper.FormatTo (ref buffer, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;

                case 3:
                    Utf8FormatHelper.FormatTo (ref buffer, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;

                case 4:
                    Utf8FormatHelper.FormatTo (ref buffer, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;

                case 5:
                    Utf8FormatHelper.FormatTo (ref buffer, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;

                case 6:
                    Utf8FormatHelper.FormatTo (ref buffer, arg7, item.Alignment, item.StandardFormat, nameof (arg7));
                    break;

                case 7:
                    Utf8FormatHelper.FormatTo (ref buffer, arg8, item.Alignment, item.StandardFormat, nameof (arg8));
                    break;

                case 8:
                    Utf8FormatHelper.FormatTo (ref buffer, arg9, item.Alignment, item.StandardFormat, nameof (arg9));
                    break;

                case 9:
                    Utf8FormatHelper.FormatTo (ref buffer, arg10, item.Alignment, item.StandardFormat, nameof (arg10));
                    break;

                case 10:
                    Utf8FormatHelper.FormatTo (ref buffer, arg11, item.Alignment, item.StandardFormat, nameof (arg11));
                    break;

                case 11:
                    Utf8FormatHelper.FormatTo (ref buffer, arg12, item.Alignment, item.StandardFormat, nameof (arg12));
                    break;

                case 12:
                    Utf8FormatHelper.FormatTo (ref buffer, arg13, item.Alignment, item.StandardFormat, nameof (arg13));
                    break;

                case 13:
                    Utf8FormatHelper.FormatTo (ref buffer, arg14, item.Alignment, item.StandardFormat, nameof (arg14));
                    break;

                case 14:
                    Utf8FormatHelper.FormatTo (ref buffer, arg15, item.Alignment, item.StandardFormat, nameof (arg15));
                    break;

                default:
                    break;
            }
        }
    }

    #endregion
}

/// <summary>
///
/// </summary>
public sealed partial class Utf8PreparedFormat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    ///
    /// </summary>
    public int MinSize { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Utf8PreparedFormat
        (
            string format
        )
    {
        Sure.NotNull (format);

        FormatString = format;
        _segments = PreparedFormatHelper.Utf8Parse (format, out _utf8PreEncodedbuffer);

        var size = 0;
        foreach (var item in _segments)
        {
            if (!item.IsFormatArgument)
            {
                size += item.Count;
            }
        }

        MinSize = size;
    }

    #endregion

    #region Private members

    private readonly Utf8FormatSegment[] _segments;
    private readonly byte[] _utf8PreEncodedbuffer;

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public string Format
        (
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15,
            T16 arg16
        )
    {
        var builder = new Utf8ValueStringBuilder (true);
        try
        {
            FormatTo (ref builder, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15, arg16);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// /
    /// </summary>
    public void FormatTo<TBufferWriter>
        (
            ref TBufferWriter builder,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15,
            T16 arg16
        )
        where TBufferWriter: IBufferWriter<byte>
    {
        var formatSpan = _utf8PreEncodedbuffer.AsSpan();

        foreach (var item in _segments)
        {
            switch (item.FormatIndex)
            {
                case Utf8FormatSegment.NotFormatIndex:
                    var strSpan = formatSpan.Slice (item.Offset, item.Count);
                    var span = builder.GetSpan (item.Count);
                    strSpan.TryCopyTo (span);
                    builder.Advance (item.Count);
                    break;

                case 0:
                    Utf8FormatHelper.FormatTo (ref builder, arg1, item.Alignment, item.StandardFormat, nameof (arg1));
                    break;

                case 1:
                    Utf8FormatHelper.FormatTo (ref builder, arg2, item.Alignment, item.StandardFormat, nameof (arg2));
                    break;

                case 2:
                    Utf8FormatHelper.FormatTo (ref builder, arg3, item.Alignment, item.StandardFormat, nameof (arg3));
                    break;

                case 3:
                    Utf8FormatHelper.FormatTo (ref builder, arg4, item.Alignment, item.StandardFormat, nameof (arg4));
                    break;

                case 4:
                    Utf8FormatHelper.FormatTo (ref builder, arg5, item.Alignment, item.StandardFormat, nameof (arg5));
                    break;

                case 5:
                    Utf8FormatHelper.FormatTo (ref builder, arg6, item.Alignment, item.StandardFormat, nameof (arg6));
                    break;

                case 6:
                    Utf8FormatHelper.FormatTo (ref builder, arg7, item.Alignment, item.StandardFormat, nameof (arg7));
                    break;

                case 7:
                    Utf8FormatHelper.FormatTo (ref builder, arg8, item.Alignment, item.StandardFormat, nameof (arg8));
                    break;

                case 8:
                    Utf8FormatHelper.FormatTo (ref builder, arg9, item.Alignment, item.StandardFormat, nameof (arg9));
                    break;

                case 9:
                    Utf8FormatHelper.FormatTo (ref builder, arg10, item.Alignment, item.StandardFormat, nameof (arg10));
                    break;

                case 10:
                    Utf8FormatHelper.FormatTo (ref builder, arg11, item.Alignment, item.StandardFormat, nameof (arg11));
                    break;

                case 11:
                    Utf8FormatHelper.FormatTo (ref builder, arg12, item.Alignment, item.StandardFormat, nameof (arg12));
                    break;

                case 12:
                    Utf8FormatHelper.FormatTo (ref builder, arg13, item.Alignment, item.StandardFormat, nameof (arg13));
                    break;

                case 13:
                    Utf8FormatHelper.FormatTo (ref builder, arg14, item.Alignment, item.StandardFormat, nameof (arg14));
                    break;

                case 14:
                    Utf8FormatHelper.FormatTo (ref builder, arg15, item.Alignment, item.StandardFormat, nameof (arg15));
                    break;

                case 15:
                    Utf8FormatHelper.FormatTo (ref builder, arg16, item.Alignment, item.StandardFormat, nameof (arg16));
                    break;

                default:
                    break;
            }
        }
    }

    #endregion
}
