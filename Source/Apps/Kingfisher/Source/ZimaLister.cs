// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ZimaLister.cs -- составитель списка по "Зимним технологиям"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM;
using AM.Collections;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Trees;

#endregion

#nullable enable

namespace Kingfisher;

/// <summary>
/// Составитель списка по "Зимним технологиям"
/// </summary>
public sealed class ZimaLister
    : IDisposable
{
    #region Properties

    public SyncConnection Provider { get; }

    #endregion

    #region Construction

    public ZimaLister
        (
            string connectionString
        )
    {
        Sure.NotNullNorEmpty (connectionString);

        Provider = ConnectionFactory.Shared.CreateSyncConnection();
        Provider.ParseConnectionString (connectionString);
        if (! Provider.Connect())
        {
            Console.Error.WriteLine (IrbisException.GetErrorDescription (Provider.LastError));
            throw new IrbisException ("Can't connect");
        }

        var maxMfn = Provider.GetMaxMfn();
        Console.WriteLine ($"<p><b>Всего записей в базе данных: {maxMfn}</b></p>");

    }

    #endregion

    #region Private members

    private TreeFile _treeFile = null!;
    private Dictionary<string, Record[]> _dictionary = null!;

    #endregion

    #region Public methods

    public void RetrieveTree()
    {
        var specification = new FileSpecification
        {
            Path = IrbisPath.MasterFile,
            Database = Provider.EnsureDatabase(),
            FileName = "zima.tre"
        };
        var textFile = Provider.RequireTextFile (specification);
        using var reader = new StringReader (textFile);
        _treeFile = TreeFile.ParseStream (reader);

        Console.WriteLine ("<h1>Распределение записей по разделам указателя</h1>");
        Console.WriteLine ("<table>");
        Console.WriteLine ("<thead>");
        Console.WriteLine ("<tr>");
        Console.WriteLine ("<th>Раздел</th>");
        Console.WriteLine ("<th>Количество записей</th>");
        Console.WriteLine ("</tr>");
        Console.WriteLine ("</thead>");
        Console.WriteLine ("<tbody");

        _dictionary = new ();
        _treeFile.Walk (line =>
        {
            var prefix = line.Prefix;
            var value = line.Value;
            if (!string.IsNullOrWhiteSpace (prefix) && value is not null)
            {
                var expression = $"\"RU={prefix} - $\"";
                var found = Provider.SearchRead (expression) ?? Array.Empty<Record>();
                Console.WriteLine ($"<tr><td>{value}</td><td>{found.Length}</td></tr>");
                _dictionary[value] = found;
            }
        });

        Console.WriteLine ("</tbody>");
        Console.WriteLine ("</table>");
    }

    public void ListRecords()
    {
        Console.WriteLine ("<h1>Содержимое разделов указателя</h1>");

        var keys = _dictionary.Keys.Order().ToArray();
        foreach (var key in keys)
        {
            var mfns = _dictionary[key].Select (record => record.Mfn).ToArray();
            if (mfns.IsNullOrEmpty())
            {
                continue;
            }

            Console.WriteLine ();
            Console.WriteLine ($"<h2>{key}</h2>");
            Console.WriteLine ("<ol>");

            var formatted = Provider.FormatRecords (mfns, "@brief");
            if (formatted is not null)
            {
                formatted = formatted.Order().ToArray();
                foreach (var line in formatted)
                {
                    Console.WriteLine ("<li>");
                    Console.WriteLine (line);
                    Console.WriteLine ("</li>");
                }
            }

            Console.WriteLine ("</ol>");

        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Provider.Disconnect();
    }

    #endregion
}
