// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TextScanner.cs -- упрощенный ввод данных из текстового потока
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Упрощенный ввод данных из текстового потока.
/// </summary>
public sealed class TextScanner
{
    #region Properties

    /// <summary>
    /// Текстовый поток, из которого считываются данные.
    /// </summary>
    public TextReader In { get; }

    /// <summary>
    /// Ввод из консоли.
    /// </summary>
    public static TextScanner Console { get; } = new (System.Console.In);

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextScanner
        (
            TextReader reader
        )
    {
        Sure.NotNull (reader);

        In = reader;
    }

    #endregion

    #region Private members

    private static readonly char[] _standardSkipChars = { ' ', '\n', '\r', '\t' };

    #endregion

    #region Public methods

    /// <summary>
    /// Считывание токена, ограниченного указанными символами.
    /// </summary>
    public string ReadToken
        (
            params char[] skipChars
        )
    {
        var c = (char) In.Read();
        while (Array.IndexOf (skipChars, c) >= 0)
        {
            c = (char) In.Read();
        }

        var builder = StringBuilderPool.Shared.Get();
        while (Array.IndexOf (skipChars, c) < 0)
        {
            builder.Append (c);
            c = (char) In.Read();
        }

        return builder.ReturnShared();
    }

    /// <summary>
    /// Считывание токена, ограниченного стандартными пробельными символами.
    /// </summary>
    public string ReadToken() => ReadToken (_standardSkipChars);

    /// <summary>
    /// Считывание логического значения.
    /// </summary>
    public bool ReadBoolean() => Utility.ToBoolean (ReadToken());

    /// <summary>
    /// Считывание значения денежного типа.
    /// </summary>
    public decimal ReadDecimal() => decimal.Parse (ReadToken(), CultureInfo.InvariantCulture);

    /// <summary>
    /// Считывание числа с плавающей точкой двойной точности.
    /// </summary>
    public double ReadDouble() => double.Parse (ReadToken(), CultureInfo.InvariantCulture);

    /// <summary>
    /// Считывание числа с плавающей точкой одинарной точности.
    /// </summary>
    public float ReadSingle() => float.Parse (ReadToken(), CultureInfo.InvariantCulture);

    /// <summary>
    /// Считывание 32-битного целого числа (возможно, со знаком).
    /// </summary>
    public int ReadInt32() => int.Parse (ReadToken(), CultureInfo.InvariantCulture);

    /// <summary>
    /// Считывание 64-битного целого числа (возможно, со знаком).
    /// </summary>
    public long ReadInt64() => long.Parse (ReadToken(), CultureInfo.InvariantCulture);

    #endregion
}
