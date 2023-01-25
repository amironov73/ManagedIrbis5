// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IBarsikDebugger.cs -- интерфейс отладчика для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Интерфейс отладчика для Барсика.
/// </summary>
public interface IBarsikDebugger
{
    /// <summary>
    /// Пробуждение отладчика при наступлении какого-нибудь события.
    /// </summary>
    public void Raise
        (
            Context context,
            StatementBase? statement
        );

    /// <summary>
    /// Трассировка.
    /// </summary>
    public void Trace (StatementBase statement);
}
