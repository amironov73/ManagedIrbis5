// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement

#region Using directives

using System;
using System.Collections.Generic;

using ManagedIrbis;

#endregion

#nullable enable

namespace IrbisExamples;

public class FullTextSearch
{
    public static void DoSearch()
    {
        var connectionString = "host=127.0.0.1;user=librarian;password=secret;db=IBIS;";
        using var connection = new ConnectionBuilder()
            .WithConnectionString (connectionString)
            .Build();

        try
        {
            connection.Connect();

            var search = new SearchParameters();
            var fulltext = new TextParameters();
            search.Database = connection.Database;
            search.Expression = "K=прогр$";
            fulltext.Request = "Android";

            var found = connection.FullTextSearch (search, fulltext);
            if (found?.Pages != null)
            {
                Console.WriteLine ($"Найдено: {found.Pages.Length}");
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception);
        }
    }
}
