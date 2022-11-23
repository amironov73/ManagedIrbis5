// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ListCommand.cs -- вывод списка найденных MFN
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System;

using AM;

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Вывод списка найденных MFN.
/// </summary>
public sealed class ListCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ListCommand()
        : base ("list")
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

        executive.ClearOutput();

        var records = executive.Records.ToArray();

        if (records.Length == 0)
        {
            executive.WriteMessage ("No records");
        }
        else
        {
            var first = true;
            foreach (var record in records)
            {
                if (!first)
                {
                    executive.MxConsole.Write (", ");
                }

                executive.MxConsole.Write (record.Mfn.ToInvariantString());
            }

            executive.MxConsole.Write (Environment.NewLine);
        }

        OnAfterExecute();

        return true;
    }

    #endregion
}
