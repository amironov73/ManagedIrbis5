// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* PointD.cs -- структура, хранящая координаты X и Y как числа с плавающей точкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// Простая структура, хранящая координаты X и Y
/// как числа с плавающей точкой.
/// </summary>
[Serializable]
public struct PointD
{
    #region Fields

    /// <summary>
    /// Координата X.
    /// </summary>
    public double X;

    /// <summary>
    /// Координата Y,
    /// </summary>
    public double Y;

    #endregion

    #region Construction

    /// <summary>
    /// Создает объект <see cref="PointD" /> из двух двойных значений.
    /// </summary>
    /// <param name="x">Координата X.</param>
    /// <param name="y">Координата Y.</param>
    public PointD
        (
            double x,
            double y
        )
    {
        X = x;
        Y = y;
    }

    #endregion
}
