// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IErrorInfo.cs -- интерфейс для информации об ошибке
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM;

/// <summary>
/// Интерфейс для информации об ошибке.
/// </summary>
public interface IErrorInfo
{
    /// <summary>
    /// Описание ошибки.
    /// </summary>
    string ErrorDescription { get; }
}
