// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LockCommand.cs -- работа с блокировками баз данных и отдельных записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;
using System.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Работа с блокировками как баз данных, так и отдельных записей.
/// </summary>
public sealed class LockCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LockCommand()
        : base ("lock")
    {
        // пустое тело конструктора
    }

    #endregion

    #region MxCommand members

    /// <inheritdoc cref="MxCommand.Execute" />
    public override bool Execute
        (
            MxExecutive executive,
            MxArgument[] arguments
        )
    {
        OnBeforeExecute();


        OnAfterExecute();

        return true;
    }

    #endregion
}
