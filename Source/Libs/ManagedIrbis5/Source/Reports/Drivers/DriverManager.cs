// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* DriverManager.cs -- управление драйверами отчетов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Reports;

/// <summary>
/// Управление драйверами отчетов.
/// </summary>
public static class DriverManager
{
    #region Constants

    /// <summary>
    /// CSV driver.
    /// </summary>
    public const string Csv = "CSV";

    /// <summary>
    /// Dataset driver.
    /// </summary>
    public const string Dataset = "Dataset";

    /// <summary>
    /// HTML driver.
    /// </summary>
    public const string Html = "HTML";

    /// <summary>
    /// LaTex driver.
    /// </summary>
    public const string Latex = "LaTex";

    /// <summary>
    /// Markdown driver.
    /// </summary>
    public const string Markdown = "Markdown";

    /// <summary>
    /// Plain text driver.
    /// </summary>
    public const string PlainText = "PlainText";

    /// <summary>
    /// RTf driver.
    /// </summary>
    public const string Rtf = "RTF";

    #endregion

    #region Properties

    /// <summary>
    /// Реестр драйверов.
    /// </summary>
    public static Dictionary<string, Type> Registry { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static DriverManager()
    {
        Registry = new Dictionary<string, Type>
        {
            { Dataset, typeof (DatasetDriver) },
            { Csv, typeof (CsvDriver) },
            { Html, typeof (HtmlDriver) },
            { Latex, typeof (LatexDriver) },
            { Markdown, typeof (MarkdownDriver) },
            { PlainText, typeof (PlainTextDriver) },
            { Rtf, typeof (RtfDriver) },
        };
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Get <see cref="ReportDriver"/> by name.
    /// </summary>
    public static ReportDriver? GetDriver
        (
            string name,
            bool throwOnError
        )
    {
        Sure.NotNullNorEmpty (name);

        if (!Registry.TryGetValue (name, out var type))
        {
            Magna.Logger.LogError
                (
                    nameof (DriverManager) + "::" + nameof (GetDriver)
                    + ": driver not found: {Name}",
                    name.ToVisibleString()
                );

            if (throwOnError)
            {
                throw new IrbisException ("driver not found: " + name.ToVisibleString());
            }

            return null;
        }

        var result = (ReportDriver?) Activator.CreateInstance (type);

        return result;
    }

    #endregion
}
