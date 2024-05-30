// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* MathModel.cs -- представляет бизнес-логике приложения
 * Ars Magna project, http://arsmagna.ru
 */

namespace HelloMvvm;

internal interface IMathModel
{
    /// <summary>
    /// Сложение двух чисел.
    /// </summary>
    double Add (double a, double b);
}

/// <summary>
/// Model представляет бизнес-логику приложения.
/// </summary>
internal sealed class MathModel : IMathModel
{
    #region Public methods

    /// <inheritdoc cref="IMathModel.Add"/>
    public double Add (double a, double b) => a + b;

    #endregion
}
