// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SearchCommand.cs -- поиск по словарю
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Поиск по словарю
/// </summary>
public sealed class SearchCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SearchCommand()
        : base ("Search")
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

        if (!executive.Provider.IsConnected)
        {
            executive.WriteError ("Not connected");

            return false;
        }

        if (arguments.Length != 0)
        {
            var argument = arguments[0].Text;

            Debug.WriteLine (argument);

            if (!string.IsNullOrEmpty (argument))
            {
                var found = executive.Provider.Search (argument);
                var foundCount = found.Length;
                executive.WriteMessage ($"Found: {found.Length}");

                if (executive.Limit > 0)
                {
                    found = found.Take (executive.Limit).ToArray();
                    if (found.Length < foundCount)
                    {
                        executive.WriteMessage ($"Limited to {found.Length} records");
                    }
                }

                executive.Records.Clear();
                for (var i = 0; i < found.Length; i++)
                {
                    var mfn = found[i];
                    var record = new MxRecord
                    {
                        Database = executive.Provider.Database,
                        Mfn = mfn,
                    };
                    executive.Records.Add (record);
                }

                executive.History.Push (argument);
            }
        }

        OnAfterExecute();

        return true;
    }

    #endregion

    #region Object members

    #endregion
}
