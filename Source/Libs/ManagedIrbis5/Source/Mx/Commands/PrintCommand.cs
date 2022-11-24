// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PrintCommand.cs -- вывод описаний найденных записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Вывод на экран (или в файл) найденных записей.
/// </summary>
public sealed class PrintCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PrintCommand()
        : base ("print")
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

        IReadOnlyList<MxRecord> records = executive.Records;
        if (records.Count == 0)
        {
            executive.WriteError ("No records");
        }
        else
        {
            if (!string.IsNullOrEmpty (executive.OrderFormat))
            {
                var mfns = records.Select (r => r.Mfn).ToArray();
                var order = executive.Provider.FormatRecords (mfns, executive.OrderFormat);
                if (order is not null)
                {
                    for (var i = 0; i < order.Length; i++)
                    {
                        records[i].Order = order[i];
                    }

                    records = records.OrderBy (r => r.Order).ToArray();
                }
            }

            if (!string.IsNullOrEmpty (executive.DescriptionFormat))
            {
                var mfns = records.Select (r => r.Mfn).ToArray();
                var formatted = executive.Provider.FormatRecords (mfns, executive.DescriptionFormat);
                if (formatted is not null)
                {
                    for (var i = 0; i < formatted.Length; i++)
                    {
                        records[i].Description = formatted[i];
                    }
                }
            }

            foreach (var record in records)
            {
                if (string.IsNullOrEmpty (record.Description))
                {
                    executive.WriteOutput (record.Mfn.ToInvariantString());
                }
                else
                {
                    executive.WriteOutput (record.Description);
                }
            }
        }

        OnAfterExecute();

        return true;
    }

    #endregion
}
