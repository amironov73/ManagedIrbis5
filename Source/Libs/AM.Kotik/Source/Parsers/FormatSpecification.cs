// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FormatSpecification.cs -- спецификация формата для строки
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Спецификация формата для строки.
/// </summary>
public sealed class FormatSpecification
{
    #region Properties

    /// <summary>
    /// Предварительная строка.
    /// </summary>
    public string? Prefix { get; set; }
    
    /// <summary>
    /// Значение, подлежащее форматированию.
    /// </summary>
    public string? Value { get; set; }
    
    /// <summary>
    /// Форматирование, применяемое к значению.
    /// </summary>
    public string? Format { get; set; }

    #endregion
}
