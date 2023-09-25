// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExecutionResult.cs -- результат исполнения скрипта
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Lexey.Barsik;

/// <summary>
/// Результат исполнения скрипта.
/// </summary>
public sealed class ExecutionResult
{
    #region Properties

    /// <summary>
    /// Явно затребован выход?
    /// </summary>
    public bool ExitRequested { get; set; }

    /// <summary>
    /// Код выхода.
    /// </summary>
    public int ExitCode { get; set; }

    /// <summary>
    /// Сообщение.
    /// </summary>
    public string? Message { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"[Exit code={ExitCode}] {Message}";
    }

    #endregion
}
