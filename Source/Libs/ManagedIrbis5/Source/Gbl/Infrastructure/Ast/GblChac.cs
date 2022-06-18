// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ChacNode.cs -- замена данных в поле/подполе с учетом регистра символов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Ast;

/// <summary>
/// Замена данные в поле/подполе с учетом регистра символов.
/// </summary>
public sealed class GblChac
    : GblNode
{
    #region Constants

    /// <summary>
    /// Мнемоническое обозначение команды.
    /// </summary>
    public const string Mnemonic = "CHAC";

    #endregion

    #region GblNode members

    /// <inheritdoc cref="GblNode.Execute"/>
    public override void Execute
        (
            GblContext context
        )
    {
        Sure.NotNull(context);

        OnBeforeExecution(context);

        // TODO implement

        OnAfterExecution(context);
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
