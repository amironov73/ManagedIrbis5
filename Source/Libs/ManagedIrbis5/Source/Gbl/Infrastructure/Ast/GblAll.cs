// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblAll.cs -- дополнение новой записи всеми полями текущей записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Ast;
//
// Official documentation:
//
// Оператор можно использовать в группе операторов после
// операторов NEWMFN или CORREC. Он дополняет записи всеми
// полями текущей записи. Т.е. это способ, например,
// создать новую запись и наполнить ее содержимым текущей записи.
// Или можно вызвать на корректировку другую запись (CORREC),
// очистить ее (EMPTY) и наполнить содержимым текущей записи.
//

/// <summary>
/// Добавление в новую запись всех полей текущей записи.
/// </summary>
public sealed class GblAll
    : GblNode
{
    #region Constants

    /// <summary>
    /// Мнемоническое обозначение команды.
    /// </summary>
    public const string Mnemonic = "ALL";

    #endregion

    #region GblNode members

    /// <inheritdoc cref="GblNode.Execute"/>
    public override void Execute
        (
            GblContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        // TODO implement

        OnAfterExecution (context);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Mnemonic;
    }

    #endregion
}
