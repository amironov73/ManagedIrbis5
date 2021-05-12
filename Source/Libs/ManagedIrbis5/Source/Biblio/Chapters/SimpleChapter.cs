// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SimpleChapter.cs -- простая глава
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

using AM;

using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Простая глава.
    /// </summary>
    public class SimpleChapter
        : ChapterWithRecords
    {
        #region Properties

        /// <summary>
        /// Filter.
        /// </summary>
        [JsonPropertyName("search")]
        public string? SearchExpression { get; set; }

        /// <summary>
        /// Format.
        /// </summary>
        [JsonPropertyName("format")]
        public string? Format { get; set; }

        /// <summary>
        /// Order.
        /// </summary>
        [JsonPropertyName("order")]
        public string? Order { get; set; }

        #endregion

        #region Private members

        /// <summary>
        /// Get description format from chapter hierarchy.
        /// </summary>
        protected virtual string GetDescriptionFormat()
        {
            var result = GetProperty<SimpleChapter, string?>
                (
                    chapter => chapter.Format
                );
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException("format not set");
            }

            return result;
        }

        /// <summary>
        /// Get description format from chapter hierarchy.
        /// </summary>
        protected virtual string GetOrderFormat()
        {
            var result = GetProperty<SimpleChapter, string?>
                (
                    chapter => chapter.Order
                );
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException("order not set");
            }

            return result;
        }

        static string Propis
            (
                int number
            )
        {
            var integers = new int[4];
            var strings = new [,]
            {
                {" миллиард", " миллиарда", " миллиардов"},
                {" миллион", " миллиона", " миллионов"},
                {" тысяча", " тысячи", " тысяч"},
                {"", "", ""}

            };

            integers[0] = (number - number % 1000000000) / 1000000000;
            integers[1] = (number % 1000000000 - number % 1000000) / 1000000;
            integers[2] = (number % 1000000 - number % 1000) / 1000;
            integers[3] = number % 1000;

            var result = new StringBuilder();
            for (var i = 0; i < 4; i++)
            {
                if (integers[i] != 0)
                {
                    if ((integers[i] - integers[i] % 100) / 100 != 0)
                    {
                        switch ((integers[i] - integers[i] % 100) / 100)
                        {
                            case 1: result.Append(" сто"); break;
                            case 2: result.Append(" двести"); break;
                            case 3: result.Append(" триста"); break;
                            case 4: result.Append(" четыреста"); break;
                            case 5: result.Append(" пятьсот"); break;
                            case 6: result.Append(" шестьсот"); break;
                            case 7: result.Append(" семьсот"); break;
                            case 8: result.Append(" восемьсот"); break;
                            case 9: result.Append(" девятьсот"); break;
                        }
                    }

                    if ((integers[i] % 100 - integers[i] % 100 % 10) / 10 != 1)
                    {
                        switch ((integers[i] % 100 - integers[i] % 100 % 10) / 10)
                        {
                            case 2: result.Append(" двадцать"); break;
                            case 3: result.Append(" тридцать"); break;
                            case 4: result.Append(" сорок"); break;
                            case 5: result.Append(" пятьдесят"); break;
                            case 6: result.Append(" шестьдесят"); break;
                            case 7: result.Append(" семьдесят"); break;
                            case 8: result.Append(" восемьдесят"); break;
                            case 9: result.Append(" девяносто"); break;
                        }
                    }

                    switch (integers[i] % 100)
                    {
                        case 1:  result.Append(i == 2 ? " одна" : " один"); break;
                        case 2:  result.Append(i == 2 ? " две" : " два"); break;
                        case 3:  result.Append(" три"); break;
                        case 4:  result.Append(" четыре"); break;
                        case 5:  result.Append(" пять"); break;
                        case 6:  result.Append(" шесть"); break;
                        case 7:  result.Append(" семь"); break;
                        case 8:  result.Append(" восемь"); break;
                        case 9:  result.Append(" девять"); break;
                        case 10: result.Append(" десять"); break;
                        case 11: result.Append(" одиннадцать"); break;
                        case 12: result.Append(" двенадцать"); break;
                        case 13: result.Append(" тринадцать"); break;
                        case 14: result.Append(" четырнадцать"); break;
                        case 15: result.Append(" пятнадцать"); break;
                        case 16: result.Append(" шестнадцать"); break;
                        case 17: result.Append(" семнадцать"); break;
                        case 18: result.Append(" восемнадцать"); break;
                        case 19: result.Append(" девятнадцать"); break;
                    }

                    if (integers[i] % 100 >= 10 && integers[i] % 100 <= 19)
                    {
                        result.Append($" ");
                        result.Append(strings[i, 2]);
                        result.Append(' ');
                    }
                    else
                    {
                        switch (integers[i] % 100)
                        {
                            case 1: result.Append(' '); result.Append(strings[i, 0]); result.Append(' '); break;
                            case 2:
                            case 3:
                            case 4: result.Append(' '); result.Append(strings[i, 1]); result.Append(' '); break;
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9: result.Append(' '); result.Append(strings[i, 2]); result.Append(' '); break;

                        }
                    }
                }

            }

            return result.ToString();

        } // method Propis

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.BuildItems" />
        public override void BuildItems
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            log.WriteLine("Begin build items {0}", this);

            var processor = context.Processor
                .ThrowIfNull("context.Processor");

            using (var formatter
                = processor.AcquireFormatter(context))
            {
                var descriptionFormat = GetDescriptionFormat();
                descriptionFormat = processor.GetText
                    (
                        context,
                        descriptionFormat
                    )
                    .ThrowIfNull("processor.GetText");
                formatter.ParseProgram(descriptionFormat);

                var mfns = Records.Select(r => r.Mfn).ToArray();
                var formatted = formatter.FormatRecords(mfns);

                Items ??= new ItemCollection();

                for (var i = 0; i < Records.Count; i++)
                {
                    log.Write(".");
                    var record = Records[i];
                    var description = formatted[i]
                        .TrimEnd('\u001F');

                    // TODO handle string.IsNullOrEmpty(description)

                    description = BiblioUtility.AddTrailingDot(description);

                    var item = new BiblioItem
                    {
                        Chapter = this,
                        Record = record,
                        Description = description
                    };
                    Items.Add(item);
                }
                log.WriteLine(" done");
            }

            using (var formatter
                = processor.AcquireFormatter(context))
            {
                var orderFormat = GetOrderFormat();
                orderFormat = processor.GetText
                    (
                        context,
                        orderFormat
                    )
                    .ThrowIfNull("processor.GetText");
                formatter.ParseProgram(orderFormat);

                var mfns = Records.Select(r => r.Mfn).ToArray();
                var formatted = formatter.FormatRecords(mfns);

                var fioRegex = new Regex(@"^[А-Я]\.(\s+[А-Я]\.)");

                for (var i = 0; i < Items.Count; i++)
                {
                    log.Write(".");
                    var item = Items[i];
                    var order = formatted[i].TrimEnd('\u001F');

                    // TODO handle string.IsNullOrEmpty(order)

                    if (order.StartsWith("["))
                    {
                        order = order.Substring(1);
                    }

                    var firstChar = order.FirstChar();
                    if (char.IsDigit(firstChar))
                    {
                        var numberText = "";
                        while (order.Length != 0)
                        {
                            firstChar = order.FirstChar();
                            if (!char.IsDigit(firstChar) && char.IsWhiteSpace(firstChar))
                            {
                                break;
                            }

                            if (char.IsDigit(firstChar))
                            {
                                numberText += firstChar;
                            }

                            order = order.Substring(1);
                        }

                        numberText = numberText.Trim();
                        var numberValue = int.Parse(numberText);
                        numberText = Propis(numberValue).Trim();
                        numberText = char.ToUpperInvariant(numberText.FirstChar())
                                     + numberText.Substring(1);
                        order = numberText + " " + order;
                    }

                    var match = fioRegex.Match(order);
                    if (match.Success)
                    {
                        var length = match.Value.Length;
                        order = order.Substring(length).TrimStart();
                    }

                    //item.Order = RichText.Decode(order);
                    item.Order = order;
                }
                log.WriteLine(" done");
            }

            Items.SortByOrder();

            log.WriteLine("Items: {0}", Items.Count);

            foreach (var chapter in Children)
            {
                chapter.BuildItems(context);
            }

            log.WriteLine("End build items {0}", this);

        } // method BuildItems

        /// <inheritdoc cref="BiblioChapter.GatherRecords" />
        public override void GatherRecords
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            log.WriteLine("Begin gather records {0}", this);

            try
            {
                var processor = context.Processor
                    .ThrowIfNull("context.Processor");
                using (var formatter = processor.AcquireFormatter(context))
                {
                    var provider = context.Provider;
                    var records = Records
                        .ThrowIfNull("Records");

                    var searchExpression = SearchExpression
                        .ThrowIfNull("SearchExpression");
                    formatter.ParseProgram(searchExpression);
                    var record = new Record();
                    searchExpression = formatter.FormatRecord(record);

                    var parameters = new SearchParameters
                    {
                        Database = context.Provider.Database,
                        Expression = searchExpression
                    };
                    var found = provider.Search(parameters);
                    if (found is not null)
                    {
                        log.WriteLine("Found: {0} record(s)", found.Length);

                        log.Write("Reading records");

                        // Пробуем не загружать записи,
                        // а предоставить заглушки

                        foreach (var item in found)
                        {
                            log.Write(".");
                            record = new Record
                            {
                                Mfn = item.Mfn
                            };
                            records.Add(record);
                            context.Records.Add(record);
                        }
                    }

                    log.WriteLine(" done");
                }

                foreach (var chapter in Children)
                {
                    chapter.GatherRecords(context);
                }

            }
            catch (Exception exception)
            {
                log.WriteLine($"Exception: {exception}");
                throw;
            }

            log.WriteLine("End gather records {0}", this);

        } // method GatherRecords

        /// <inheritdoc cref="BiblioChapter.Render" />
        public override void Render
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            log.WriteLine("Begin render {0}", this);

            var processor = context.Processor
                .ThrowIfNull("context.Processor");
            var report = processor.Report
                .ThrowIfNull("processor.Report");


            var showOrder = false;
                // TODO: implement
                /* context.Document.CommonSettings.Value<bool?>("showOrder") ?? false; */

            if (Records.Count != 0
                || Duplicates.Count != 0
                || Children.Count != 0)
            {
                RenderTitle(context);

                for (var i = 0; i < Items?.Count; i++)
                {
                    log.Write(".");
                    var item = Items[i];
                    var number = item.Number;
                    var description = item.Description
                        .ThrowIfNull("item.Description");

                    ReportBand band = new ParagraphBand
                        (
                            number.ToInvariantString() + ") "
                        );
                    report.Body.Add(band);
                    band.Cells.Add(new SimpleTextCell(
                            description
                            //RichText.Encode3(description, UnicodeRange.Russian, "\\f2")
                        ));

                    var record = item.Record;

                    // Для отладки: проверить упорядочение
                    if (showOrder)
                    {
                        if (!ReferenceEquals(record, null))
                        {
                            band = new ParagraphBand("MFN " + record.Mfn + " " + item.Order);
                            report.Body.Add(band);
                            report.Body.Add(new ParagraphBand());
                        }
                    }

                }

                log.WriteLine(" done");

            }

            RenderDuplicates(context);

            RenderChildren(context);

            log.WriteLine(string.Empty);
            log.WriteLine("End render {0}", this);

        } // method Render

        #endregion

    } // class SimpleChapter

} // namespace ManagedIrbis.Biblio
