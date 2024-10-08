﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable DelegateSubtraction
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
// ReSharper disable RedundantAssignment
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

#pragma warning disable 649

/* BatchSearcher.cs -- пакетный поиск в каталоге
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Batch;

/// <summary>
/// Пакетный поиск в каталоге.
/// </summary>
public sealed class BatchSearcher
{
    #region Constants

    /// <summary>
    /// Default batch size.
    /// </summary>
    public const int DefaultBatchSize = 100;

    /// <summary>
    /// Default operation.
    /// </summary>
    public const string DefaultOperation = "+";

    #endregion

    #region Properties

    /// <summary>
    /// Batch size.
    /// </summary>
    public int BatchSize { get; set; }

    /// <summary>
    /// Connection.
    /// </summary>
    public ISyncProvider Connection { get; private set; }

    /// <summary>
    /// Database name.
    /// </summary>
    public string Database { get; private set; }

    /// <summary>
    /// Operation.
    /// </summary>
    public string Operation { get; set; }

    /// <summary>
    /// Prefix
    /// </summary>
    public string? Prefix { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public BatchSearcher
        (
            ISyncProvider connection,
            string database,
            string? prefix
        )
    {
        BatchSize = DefaultBatchSize;
        Connection = connection;
        Database = database;
        Prefix = prefix;
        Operation = DefaultOperation;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Build search query from specified terms.
    /// </summary>
    public string BuildExpression
        (
            IEnumerable<string> terms
        )
    {
        var result = SearchUtility.ConcatTerms
            (
                Prefix,
                Operation,
                terms
            );

        return result;
    }

    /// <summary>
    /// Search for specified terms.
    /// </summary>
    public int[] Search
        (
            IEnumerable<string> terms
        )
    {
        var batchSize = BatchSize;
        if (batchSize < 1)
        {
            Magna.Logger.LogError
                (
                    nameof (BatchSearcher) + "::" + nameof (Search)
                    + ": batchSize={BatchSize}",
                    batchSize
                );

            throw new ArgumentOutOfRangeException();
        }

        var packages = terms
            .Chunk (batchSize)
            .ToArray();
        var totalSize = packages.Sum (p => p.Length);
        if (totalSize == 0)
        {
            return Array.Empty<int>();
        }

        var result = new List<int> (totalSize);
        foreach (var package in packages)
        {
            var expression = BuildExpression (package);
            var parameters = new SearchParameters
            {
                Database = Database,
                Expression = expression
            };
            var found = Connection.Search (parameters);
            var mfns = FoundItem.ToMfn (found);
            result.AddRange (mfns);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Search for specified terms.
    /// </summary>
    public Record[] SearchRead
        (
            IEnumerable<string> terms
        )
    {
        var batchSize = BatchSize;
        if (batchSize < 1)
        {
            Magna.Logger.LogError
                (
                    nameof (BatchSearcher) + "::" + nameof (SearchRead)
                    + ": batchSize={BatchSize}",
                    batchSize
                );

            throw new ArgumentOutOfRangeException();
        }

        var packages = terms
            .Chunk (batchSize)
            .ToArray();
        var totalSize = packages.Sum (p => p.Length);
        if (totalSize == 0)
        {
            return Array.Empty<Record>();
        }

        var result = new List<Record> (totalSize);
        foreach (var package in packages)
        {
            var expression = BuildExpression (package);
            var found = Connection.SearchReadRecords (expression);
            if (found is not null)
            {
                result.AddRange (found);
            }
        }

        return result.ToArray();
    }

    #endregion
}
