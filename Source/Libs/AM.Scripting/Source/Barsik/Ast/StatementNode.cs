// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* StatementNode.cs -- базовый класс для стейтментов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Базовый класс для стейтментов.
/// </summary>
public class StatementNode
    : AstNode
{
    #region Properties

    /// <summary>
    /// Позиция начального символа стейтмента в коде.
    /// </summary>
    public SourcePosition StartPosition { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public StatementNode()
    {
        StartPosition = default;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StatementNode
        (
            SourcePosition startPosition
        )
    {
        StartPosition = startPosition;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StatementNode
        (
            SourcePos position
        )
    {
        StartPosition = new SourcePosition (position);
    }

    #endregion

    #region Private members

    /// <summary>
    /// Перед выполнением стейтмента.
    /// </summary>
    protected virtual void PreExecute
        (
            Context context
        )
    {
        // пока этот метод не выполняет никаких действий
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Выполнение действий, связанных с данным узлом.
    /// </summary>
    /// <param name="context">Контекст исполнения программы.</param>
    public virtual void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        // больше никаких действий тут пока не нужно
    }

    #endregion
}
