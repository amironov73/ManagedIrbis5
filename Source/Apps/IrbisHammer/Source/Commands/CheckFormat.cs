// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* CheckFormat.cs -- проверка форматов на наличие ошибок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace IrbisHammer.Commands;

/// <summary>
/// Проверка форматов на наличие ошибок.
/// </summary>
public sealed class CheckFormat
    : ToolCommand
{
    #region ToolCommand members

    /// <inheritdoc cref="ToolCommand.CommandName"/>
    public override string? CommandName => "Проверка форматов на наличие ошибок";

    /// <inheritdoc cref="ToolCommand.ExecuteCommand"/>
    public override void ExecuteCommand
        (
            HammerContext context
        )
    {
        throw new NotImplementedException();
    }

    #endregion
}
