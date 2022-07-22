// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BbkRange.cs -- интервал индексов ББК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;
using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization;

/// <summary>
/// Интервал индексов ББК вроде такого:
/// 84.3/5
/// </summary>
public sealed class BbkRange
{
    #region Properties

    /// <summary>
    /// Начальное значение индекса.
    /// </summary>
    public string FirstIndex { get; }

    /// <summary>
    /// Оригинальное значение (со слешем).
    /// </summary>
    public string OriginalIndex { get; }

    /// <summary>
    /// Конечное значение индекса.
    /// </summary>
    public string LastIndex { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public BbkRange
        (
            string originalIndex
        )
    {
        OriginalIndex = originalIndex;

        var slashPosition = OriginalIndex.IndexOf ('/');
        if (slashPosition < 0)
        {
            FirstIndex = OriginalIndex;
            LastIndex = OriginalIndex;
            return;
        }

        if (slashPosition == 0)
        {
            Magna.Logger.LogError
                (
                    nameof (BbkRange) + "::Constructor"
                    + ": index can't start with /: {Index}",
                    originalIndex
                );

            throw new BbkException ("Индекс не может начинаться со слеша");
        }

        if (OriginalIndex.LastIndexOf ('/') != slashPosition)
        {
            Magna.Logger.LogError
                (
                    nameof (BbkRange) + "::Constructor"
                    + ": more than one /: {Index}",
                    originalIndex
                );

            throw new BbkException ("Индекс не может содержать "
                                    + "больше одного слэша");
        }

        var totalLength = OriginalIndex.Length;
        var suffixLength = totalLength - slashPosition - 1;
        if (suffixLength == 0)
        {
            Magna.Logger.LogError
                (
                    nameof (BbkRange) + "::Constructor"
                    + ": index can't end with /: {Index}",
                    originalIndex
                );

            throw new BbkException ("Индекс не может заканчиваться слэшом");
        }

        var prefixLenght = slashPosition - suffixLength;
        if (prefixLenght < 0)
        {
            Magna.Logger.LogError
                (
                    nameof (BbkRange) + "::Constructor"
                    + ": prefix is shorter than suffix: {Index}",
                    originalIndex
                );

            throw new BbkException ("Префикс короче суффикса!");
        }

        FirstIndex = OriginalIndex.Substring
            (
                0,
                slashPosition
            );

        LastIndex = OriginalIndex.Substring
            (
                0,
                prefixLenght
            )
            + OriginalIndex.Substring
            (
                slashPosition + 1,
                suffixLength
            );
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение списка всех индексов из диапазона.
    /// </summary>
    public string[] GetAllIndexes()
    {
        var result = new List<string>();

        NumberText first = FirstIndex;
        NumberText last = LastIndex;
        NumberText current = first.Clone();
        while (current <= last)
        {
            result.Add (current.ToString());
            current = current.Increment();
        }

        return result.ToArray();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return OriginalIndex;
    }

    #endregion
}
