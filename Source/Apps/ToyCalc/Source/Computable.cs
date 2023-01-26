// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* Computable.cs -- контейнер для хранения аргументов и результатов вычислений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

#endregion

#nullable enable

namespace ToyCalc;

/// <summary>
/// Контейнер для хранения аргументов и результатов вычислений.
/// </summary>
public sealed class Computable
{
    #region Properties

    /// <summary>
    /// Хранимое число с плавающей точкой двойной точности.
    /// </summary>
    public double Value { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Value.ToString (CultureInfo.InvariantCulture);

    #endregion
}
