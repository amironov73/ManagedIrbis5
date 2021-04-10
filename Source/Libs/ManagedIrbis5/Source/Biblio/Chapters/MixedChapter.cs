// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MixedChapter.cs --
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
using AM.Text;
using AM.Text.Output;



using ManagedIrbis.Client;
using ManagedIrbis.Fields;
using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>
    public class MixedChapter
        : CumulatingSubChapter
    {
        #region BiblioChapter members

        /// <inheritdoc cref="MenuSubChapter.BuildItems" />
        public override void BuildItems
            (
                BiblioContext context
            )
        {
            if (Records.Count == 0)
            {
                return;
            }

            var log = context.Log;
            Record? record = null;

            log.WriteLine("Begin build items {0}", this);
            Items = new ItemCollection();

            var settings = Settings;
            if (ReferenceEquals(settings, null))
            {
                return;
            }

            var processor = context.Processor
                .ThrowIfNull("context.Processor");
            var provider = context.Provider;
            var nonSpec = new List<Record>();

            var generalFormat = settings.GetSetting("general");
            var orderFormat = settings.GetSetting("order");
            var partFormat = settings.GetSetting("format");
            var normalFormat = settings.GetSetting("normal");
            if (string.IsNullOrEmpty(generalFormat)
                || string.IsNullOrEmpty(orderFormat)
                || string.IsNullOrEmpty(partFormat)
                || string.IsNullOrEmpty(normalFormat))
            {
                return;
            }

            using (var formatter = processor.AcquireFormatter(context))
            {
                generalFormat = processor.GetText(context, generalFormat)
                    .ThrowIfNull("generalFormat");
                formatter.ParseProgram(generalFormat);

                for (var i = 0; i < Records.Count; i++)
                {
                    record = Records[i];
                    var bookInfo = new BookInfo(provider, record);
                    if (!bookInfo.Worksheet.SameString( "SPEC"))
                    {
                        nonSpec.Add(record);
                        continue;
                    }

                    log.Write("o");
                    var header = formatter.FormatRecord(record.Mfn);
                    if (!string.IsNullOrEmpty(header))
                    {
                        header = header.Trim();
                    }

                    var item = new BiblioItem
                    {
                        Record = record,
                        Chapter = this
                    };

                    var bookGroup = Groups.FirstOrDefault
                        (
                            g => g.Header == header
                        );
                    if (ReferenceEquals(bookGroup, null))
                    {
                        bookGroup = new Multivolume
                        {
                            Header = header
                        };
                        Groups.Add(bookGroup);
                    }
                    bookGroup.Add(item);
                }

                partFormat = processor.GetText(context, partFormat)
                    .ThrowIfNull("partFormat");
                formatter.ParseProgram(partFormat);

                foreach (var grp in Groups)
                {
                    foreach (var item in grp)
                    {
                        log.Write(":");

                        var description = formatter.FormatRecord(item.Record)
                            .TrimEnd('\u001F');

                        // TODO handle string.IsNullOrEmpty(description)

                        description = BiblioUtility.AddTrailingDot(description);
                        item.Description = description;
                    }
                }

                normalFormat = processor.GetText(context, normalFormat)
                    .ThrowIfNull("normalFormat");
                formatter.ParseProgram(normalFormat);

                foreach (var rec in nonSpec)
                {
                    log.Write(".");
                    var description = formatter.FormatRecord(rec)
                        .TrimEnd('\u001F');

                    // TODO handle string.IsNullOrEmpty(description)

                    description = BiblioUtility.AddTrailingDot(description);

                    var item = new BiblioItem
                    {
                        Chapter = this,
                        Record = rec,
                        Description = description
                    };
                    var bookGroup = new Multivolume
                    {
                        Header = description,
                        Single = true
                    };
                    Groups.Add(bookGroup);
                    bookGroup.Add(item);
                }

                orderFormat = processor.GetText(context, orderFormat)
                    .ThrowIfNull("orderFormat");
                formatter.ParseProgram(orderFormat);

                foreach (var bookGroup in Groups)
                {
                    record = bookGroup.First().Record
                        .ThrowIfNull("bookGroup.Record");
                    var order = formatter.FormatRecord(record.Mfn);
                    if (!string.IsNullOrEmpty(order))
                    {
                        order = order.Trim();
                    }

                    order = CleanOrder(order);

                    bookGroup.Order = order;

                    if (!bookGroup.Single)
                    {
                        foreach (var item in bookGroup)
                        {
                            order = formatter.FormatRecord(item.Record.Mfn);
                            if (!string.IsNullOrEmpty(order))
                            {
                                order = order.Trim();
                            }

                            item.Order = order;
                        }
                    }
                }

                processor.ReleaseFormatter(context, formatter);
            }

            Groups = Groups.OrderBy(x => x.Order).ToList();
            Items.Clear();
            foreach (var bookGroup in Groups)
            {
                OrderGroup(bookGroup);
                var item = new BiblioItem
                {
                    Description = bookGroup.Header,
                    Record = new Record(), // TODO ???
                    UserData = bookGroup
                };
                bookGroup.Item = item;
                Items.Add(item);
            }

            log.WriteLine("End build items {0}", this);
        }


        #endregion

        #region Object members

        #endregion
    }
}

