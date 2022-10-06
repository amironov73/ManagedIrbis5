// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Dummy.cs -- простой класс для тестов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace SiberianExperiments;

/// <summary>
/// Простой класс для тестов.
/// </summary>
public class Dummy
{
    #region Properties

    /// <summary>
    /// Целое число.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Некий текст.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Некий статус (логический).
    /// </summary>
    public bool Status => Number % 2 == 0;

    #endregion

    #region Public methods

    /// <summary>
    /// Генерация списка.
    /// </summary>
    public static IList<Dummy> GenerateList
        (
            int length
        )
    {
        var result = new List<Dummy> (length);
        for (var i = 1; i <= length; i++)
        {
            var dummy = new Dummy
            {
                Number = i,
                Text = "Квадрат: " + (i * i).ToInvariantString()
            };
            result.Add (dummy);
        }

        return result;
    }

    #endregion
}
