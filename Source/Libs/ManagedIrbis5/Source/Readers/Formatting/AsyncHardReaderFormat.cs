// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* AsyncHardReaderFormat.cs -- асинхронный захардкоженный формат
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;
using AM.AppServices;
using AM.Collections;
using AM.Text;

using ManagedIrbis.Formatting;
using ManagedIrbis.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

#endregion

#pragma warning disable CA1822 // member can be marked as static

#nullable enable

namespace ManagedIrbis.Readers.Formatting;

/// <summary>
/// Асинхронный захардкоженный формат.
/// </summary>
public sealed class AsyncHardReaderFormat
{
    #region Properties

    /// <summary>
    /// Разделитель областей.
    /// </summary>
    public string? AreaSeparator { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public AsyncHardReaderFormat()
        : this
            (
                Magna.Host,
                new NullProvider()
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AsyncHardReaderFormat
        (
            IHost host,
            IAsyncProvider provider,
            ReaderConfiguration? configuration = null,
            IStringLocalizer? localizer = null,
            IFormatDriver? driver = null
        )
    {
        Sure.NotNull (provider);

        AreaSeparator = string.Empty;
        _logger = LoggingUtility.GetLogger (host.Services, typeof (HardFormat));

        var services = host.Services;
        _configuration = configuration
                         ?? services.GetService<ReaderConfiguration> ()
                         ?? ReaderConfiguration.GetDefault();

        _localizer = localizer
                     ?? (IStringLocalizer?) services.GetService<IStringLocalizer<HardFormat>>()
                     ?? new NullLocalizer();

        _driver = driver
                  ?? services.GetService<IFormatDriver>()
                  ?? new PlainFormatDriver();

        _provider = provider;
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;

    private readonly ReaderConfiguration _configuration;

    private readonly IAsyncProvider _provider;

    private readonly IStringLocalizer _localizer;

    private readonly IFormatDriver _driver;

    /// <summary>
    /// Ограничители, после которых нельзя ставить точку.
    /// </summary>
    private static readonly char[] _delimiters = { '!', '?', '.', ',' };

    private static void _AddSeparator
        (
            StringBuilder builder
        )
    {
        var lastChar = builder.LastNonSpaceChar();
        if (lastChar != '-') // проверяем, есть ли предыдущий разделитель
        {
            var needDot = Array.IndexOf (_delimiters, lastChar) < 0;
            builder.Append (needDot ? ". - " : " - ");
        }
    }

    private static void _AddWithSeparator
        (
            StringBuilder builder,
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            _AddSeparator (builder);
            builder.Append (text);
        }
    }

    private static bool _Append
        (
            StringBuilder builder,
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            builder.Append (text);
            return true;
        }

        return false;
    }

    private static bool _AppendWithPrefix
        (
            StringBuilder builder,
            string? text,
            string? prefix
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            builder.AppendWithPrefix (text, prefix);

            return true;
        }

        return false;
    }

    private static bool _AppendWithPrefixAndSuffix
        (
            StringBuilder builder,
            string? text,
            string? prefix,
            string? suffix
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            builder.AppendWithPrefixAndSuffix (text, prefix, suffix);

            return true;
        }

        return false;
    }

    private static string? _FMA (string? separator, Record record, int tag)
    {
        var values = record.FMA (tag);
        if (values.IsNullOrEmpty())
        {
            return null;
        }

        return string.Join (separator, (IEnumerable<string?>) values);
    }

    private static string? _FMA (string? separator, Record record, int tag, char code)
    {
        var values = record.FMA (tag, code);
        if (values.IsNullOrEmpty())
        {
            return null;
        }

        return string.Join (separator, (IEnumerable<string?>) values);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Переход на новую строку.
    /// </summary>
    public void NewLine
        (
            StringBuilder builder
        )
    {
        Sure.NotNull (builder);

        builder.Trim();
        if (builder.Length == 0)
        {
            // не переходим на новую строку при пустом тексте
            return;
        }

        builder.Append ('\n');
    }

    /// <summary>
    /// Переход к новой области описания.
    /// </summary>
    public void NewArea
        (
            StringBuilder builder
        )
    {
        Sure.NotNull (builder);

        if (string.IsNullOrEmpty (AreaSeparator))
        {
            return;
        }

        var builderLength = builder.Length;
        if (builderLength == 0)
        {
            // текст библиографического описания не должен
            // начинаться с перехода на новую область
            return;
        }

        // не должно быть подряд двух переходов на новую область
        var areaLength = AreaSeparator.Length;
        var enable = areaLength < builderLength;
        if (enable)
        {
            var text = builder.ToString (builderLength - areaLength, areaLength);
            enable = text != AreaSeparator;
        }

        if (enable)
        {
            builder.Append (AreaSeparator);
        }
    }

    /// <summary>
    /// Переход к новой области описания.
    /// </summary>
    public void NewArea
        (
            StringBuilder builder,
            string? areaText
        )
    {
        Sure.NotNull (builder);

        if (!string.IsNullOrEmpty (areaText))
        {
            NewArea (builder);
            builder.Append (areaText);
        }
    }

    /// <summary>
    /// Получение номера читательского билета.
    /// </summary>
    public string? Ticket
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return _configuration.GetTicket (record);
    }

    /// <summary>
    /// Получение идентификатора читателя.
    /// </summary>
    public string? Identifier
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return _configuration.GetReaderId (record);
    }

    /// <summary>
    /// ФИО читателя.
    /// </summary>
    public string FullName
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var builder = StringBuilderPool.Shared.Get();
        FullName (builder, record);

        return builder.ReturnShared();
    }

    /// <summary>
    /// ФИО читателя.
    /// </summary>
    public void FullName
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        builder.AppendWithSeparator
            (
                " ",
                record.FM (10),
                record.FM (11),
                record.FM (12)
            );
    }

    /// <summary>
    /// ФИО читателя с годом рождения.
    /// </summary>
    public string FullNameWithYear
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var builder = StringBuilderPool.Shared.Get();
        FullNameWithYear (builder, record);

        return builder.ReturnShared();
    }

    /// <summary>
    /// ФИО читателя с годом рождения.
    /// </summary>
    public void FullNameWithYear
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        FullName (builder, record);
        _AppendWithPrefix (builder, record.FM (21), ", ");
        builder.Append ('.');
    }

    /// <summary>
    /// Краткое описание.
    /// </summary>
    public string Brief
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var builder = StringBuilderPool.Shared.Get();
        Brief (builder, record);

        return builder.ReturnShared();
    }

    /// <summary>
    /// Краткое описание.
    /// </summary>
    public void Brief
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        _logger.LogTrace ("Begin BriefDescription for MFN {Mfn}", record.Mfn);

        FullNameWithYear (builder, record);

        _AddWithSeparator (builder, Identifier (record));

        // категории читателя
        var categories = record.FMA (50);
        if (!categories.IsNullOrEmpty())
        {
            _AddSeparator (builder);
            builder.Append (_localizer["(Кат. "]);
            builder.AppendWithSeparator (", ", categories);
            builder.Append (')');
        }

        // места обслуживания
        var places = record.FMA (52, 'c');
        if (!places.IsNullOrEmpty())
        {
            _AddSeparator (builder);
            builder.Append (_localizer["Записан в "]);
            builder.AppendWithSeparator (", ", places.Distinct());
            builder.Append (')');
        }

        _logger.LogTrace ("End BriefDescription for MFN {Mfn}", record.Mfn);
    }


    #endregion
}
