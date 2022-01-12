// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ErrorInfo.cs -- информация об ошибке
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM;

/// <summary>
/// Информация об ошибке.
/// </summary>
/// <typeparam name="T">Тип для кода ошибки.</typeparam>
public class ErrorInfo<T>
    : IErrorInfo
{
    #region Properties

    /// <summary>
    /// Код ошибки.
    /// </summary>
    public T Code { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ErrorInfo
        (
            T code
        )
    {
        Code = code;
        ErrorDescription = "No description";
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ErrorInfo
        (
            T code,
            string description
        )
    {
        Code = code;
        ErrorDescription = description;
    }

    #endregion

    #region IErrorInfo members

    /// <inheritdoc cref="IErrorInfo.ErrorDescription"/>
    public string ErrorDescription { get; }

    #endregion
}
