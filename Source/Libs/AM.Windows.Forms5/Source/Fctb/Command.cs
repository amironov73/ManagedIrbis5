// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Command.cs -- базовый класс для команд
 * Ars Magna project, http://arsmagna.ru
 */

#pragma warning disable CS8618 // nullable warning

#nullable enable

namespace Fctb;

/// <summary>
/// Базовый класс для команд.
/// </summary>
public abstract class Command
{
    #region Properties

    /// <summary>
    /// Текстбокс.
    /// </summary>
    public TextSource textSource;

    #endregion

    #region Public methods

    /// <summary>
    /// Выполнение команды.
    /// </summary>
    public abstract void Execute();

    #endregion
}
