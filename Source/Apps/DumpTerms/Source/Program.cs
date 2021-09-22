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

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

/*
    Утилита для дампа терминов поискового словаря.

    DumpTerms <connectionString> <prefix>

 */

#region Using directives

using System;

using AM;

using ManagedIrbis;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace DumpTerms
{
    /// <summary>
    /// Единственный класс, содержащий всю функциональность утилиты.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Дамп термина.
        /// </summary>
        private static void DumpTerm
            (
                ISyncProvider connection,
                Term term
            )
        {
            var text = term.Text ?? string.Empty;
            Console.WriteLine (text);

            var parameters = new PostingParameters
            {
                Database = connection.Database,
                Terms = new [] { text }
            };
            var postings = connection.ReadPostings (parameters);
            if (postings is null)
            {
                Console.WriteLine ("\t(null)");
                return;
            }

            Console.WriteLine ($"\tPosting count: {postings.Length}");
            foreach (var posting in postings)
            {
                Console.WriteLine
                    (
                        "\tMFN={0} Tag={1} Occ={2} Count={3}",
                        posting.Mfn,
                        posting.Tag,
                        posting.Occurrence,
                        posting.Count
                    );

                try
                {
                    var record = connection.ReadRecord (posting.Mfn);
                    if (record is not null)
                    {
                        var field = record.Fields
                            .GetField (posting.Tag)
                            .GetOccurrence (posting.Occurrence - 1);
                        if (field is not null)
                        {
                            Console.WriteLine
                                (
                                    "\t{0}",
                                    field.ToText()
                                );
                            Console.WriteLine();
                        }

                        Console.WriteLine();
                    }
                } // try

                catch
                {
                    // Nothing to do here
                }

            } // foreach

            Console.WriteLine();

        } // method DumpTerm

        /// <summary>
        /// Собственно точка входа в программу.
        /// </summary>
        static int Main
            (
                string[] args
            )
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine ("Usage: DumpTerms <connectionString> <termPrefix>");

                return 1;
            }

            var connectionString = args[0];
            var termPrefix = args[1];

            try
            {
                using var connection = ConnectionFactory.Shared.CreateSyncConnection();
                connection.ParseConnectionString (connectionString);
                if (!connection.Connect())
                {
                    Console.Error.WriteLine ("Can't connect");
                    return 1;
                }

                var parameters = new TermParameters
                {
                    Database = connection.Database,
                    StartTerm = termPrefix,
                    NumberOfTerms = 100
                };
                var terms = connection.ReadTerms (parameters);
                if (terms is null)
                {
                    Console.Error.WriteLine ("Can't read terms");
                    return 1;
                }

                Console.WriteLine ("Found terms: {0}", terms.Length);
                foreach (var term in terms)
                {
                    DumpTerm (connection, term);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return 1;
            }

            return 0;

        } // method Main

    } // class Program

} // namespace DumpTerms
