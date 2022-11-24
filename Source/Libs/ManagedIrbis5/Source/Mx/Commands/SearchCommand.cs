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

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Прямой поиск по словарю.
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

        var provider = executive.Provider;
        if (!provider.IsConnected)
        {
            executive.WriteError ("Not connected");
            return false;
        }

        if (arguments.Length != 0)
        {
            var argument = arguments[0].Text;

            if (!string.IsNullOrEmpty (argument))
            {
                var parameters = new SearchParameters
                {
                    Database = provider.EnsureDatabase(),
                    Expression = argument
                };
                if (executive.Limit > 0)
                {
                    parameters.NumberOfRecords = executive.Limit;
                }

                var found = executive.Provider.Search (parameters);
                if (found is null)
                {
                    executive.WriteError ("Error during search");
                    return false;
                }

                executive.WriteMessage ($"Found: {found.Length}");
                executive.Records.Clear();
                for (var i = 0; i < found.Length; i++)
                {
                    var item = found[i];
                    var record = new MxRecord
                    {
                        Database = executive.Provider.Database,
                        Mfn = item.Mfn,
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
}
