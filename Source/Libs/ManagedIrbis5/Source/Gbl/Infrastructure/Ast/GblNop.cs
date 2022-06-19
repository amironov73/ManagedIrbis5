// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblNop.cs -- комментарий (пустой оператор)
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Ast;

//
// Official documentation:
//
// Комментарий. Может находиться между другими операторами
// и содержать любые тексты в строках (до 4-х) после себя.
//

/// <summary>
/// Комментарий (пустой оператор).
/// Данная команда не несет никакой полезной нагрузки,
/// и предназначена для человека
/// либо для временного отключения некоторых операторов
/// </summary>
public sealed class GblNop
    : GblNode
{
    #region Constants

    /// <summary>
    /// Мнемоническое обозначение команды.
    /// </summary>
    public const string Mnemonic = "//";

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Mnemonic;
    }

    #endregion

}
