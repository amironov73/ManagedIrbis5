// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ExemplarManager.cs -- manages exemplars of the books/magazines etc
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text.Output;

using ManagedIrbis.Batch;
using ManagedIrbis.Readers;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Manages exemplars of the books/magazines etc.
    /// </summary>
    public sealed class ExemplarManager
    {
        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        public ISyncProvider Connection { get; }

        /// <summary>
        /// Brief format name.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// List of exemplars.
        /// </summary>
        public ReadOnlyCollection<ExemplarInfo> List => _list.AsReadOnly();

        /// <summary>
        /// Output.
        /// </summary>
        public AbstractOutput? Output => _output;

        /// <summary>
        /// Prefix.
        /// </summary>
        public string Prefix { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExemplarManager
            (
                ISyncProvider connection,
                AbstractOutput? output = null
            )
        {
            Connection = connection;
            _output = output;
            Prefix = "IN=";
            Format = "@brief";
            _list = new List<ExemplarInfo>();
            _newspapers = new Dictionary<string, bool>();
        }

        #endregion

        #region Private members

        private readonly List<ExemplarInfo> _list;

        private readonly Dictionary<string, bool> _newspapers;

        private readonly AbstractOutput? _output;

        private static string GetYear
            (
                Record record
            )
        {
            var result = record.FM(210, 'd');
            if (result.IsEmpty())
            {
                result = record.FM(461, 'h');
            }

            if (result.IsEmpty())
            {
                result = record.FM(461, 'z');
            }

            if (result.IsEmpty())
            {
                var workList = record.FM(920);
                if (workList.SameString("NJ"))
                {
                    result = record.FM(934);
                }
            }

            if (result.IsEmpty())
            {
                return result ?? string.Empty;
            }

            var match = Regex.Match(result, @"\d{4}");
            if (match.Success)
            {
                result = match.Value;
            }

            return result;

        } // method GetYear

        private static string GetPrice
            (
                Record record,
                ExemplarInfo exemplar
            )
        {
            if (!exemplar.Price.IsEmpty())
            {
                return exemplar.Price;
            }

            var price = record.FM(10, 'd');

            return !price.IsEmpty() ? price : string.Empty;

        } // method GetPrice

        #endregion

        #region Public methods

        /// <summary>
        /// Add given exemplar to the collection.
        /// </summary>
        public ExemplarManager Add
            (
                ExemplarInfo exemplar
            )
        {
            if (string.IsNullOrEmpty(exemplar.Number))
            {
                return this;
            }

            if (Find(exemplar.Number) == null)
            {
                _list.Add(exemplar);
            }

            return this;
        }

        /// <summary>
        /// Add many.
        /// </summary>
        public ExemplarManager AddRange
            (
                IEnumerable<ExemplarInfo> exemplars
            )
        {
            foreach (var exemplar in exemplars)
            {
                Add(exemplar);
            }

            return this;
        }

        /// <summary>
        /// Clear the list of exemplars.
        /// </summary>
        public ExemplarManager Clear()
        {
            _list.Clear();

            return this;
        }

        /// <summary>
        /// Get bibliographic description.
        /// </summary>
        public string GetDescription
            (
                Record record
            )
        {
            var result = record.Description;
            if (string.IsNullOrEmpty(result))
            {
                result = Connection.FormatRecord
                    (
                        Format,
                        record.Mfn
                    );
                record.Description = result;
            }

            if (string.IsNullOrEmpty(result))
            {
                Magna.Error
                    (
                        "ExemplarManager::GetDescription: "
                        + "empty description"
                    );

                throw new IrbisException("Empty description");
            }

            return result;
        }

        /// <summary>
        /// Get bibliographic description.
        /// </summary>
        public string? GetDescription
            (
                Record? record,
                ExemplarInfo exemplar
            )
        {
            string? result;

            if (!ReferenceEquals(record, null))
            {
                result = GetDescription(record);
            }
            else
            {
                result = Connection.FormatRecord
                    (
                        Format,
                        exemplar.Mfn
                    );
            }

            return result;
        }

        /// <summary>
        /// Extend info.
        /// </summary>
        public ExemplarInfo Extend
            (
                ExemplarInfo exemplar,
                Record? record
            )
        {
            if (exemplar.Mfn <= 0)
            {
                Magna.Error
                    (
                        nameof(ExemplarManager) + "::" + nameof(Extend)
                        + ": MFN="
                        + exemplar.Mfn
                    );

                throw new IrbisException("MFN <= 0");
            }

            exemplar.Description = GetDescription
                (
                    record,
                    exemplar
                );

            if (!ReferenceEquals(record, null))
            {
                var workList = record.FM(920);

                if (string.IsNullOrEmpty(exemplar.ShelfIndex))
                {
                    exemplar.ShelfIndex = Utility.NonEmpty
                        (
                            record.FM(906),
                            record.FM(621),
                            record.FM(686)
                        );
                }

                if (exemplar.ShelfIndex.IsEmpty()
                    && workList.SameString("NJ"))
                {
                    var consolidatedIndex = record.FM(933);
                    if (!string.IsNullOrEmpty(consolidatedIndex))
                    {
                        var expression = $"\"I={consolidatedIndex}\"";
                        var consolidatedRecord = Connection.SearchReadOneRecord(expression);
                        if (consolidatedRecord is not null)
                        {
                            exemplar.ShelfIndex = Utility.NonEmpty
                                (
                                    consolidatedRecord.FM(906),
                                    consolidatedRecord.FM(621),
                                    consolidatedRecord.FM(686)
                                );
                        }
                    }

                }

                exemplar.Year = GetYear(record);
                exemplar.Price = GetPrice(record, exemplar);
                exemplar.Issue = record.FM(936);
            }

            return exemplar;

        } // method Extend

        /// <summary>
        ///
        /// </summary>
        public ExemplarInfo? Find
            (
                string? number
            )
        {
            if (string.IsNullOrEmpty(number))
            {
                return null;
            }

            return _list.FirstOrDefault
                (
                    e => e.Number.SameString(number)
                         || e.Barcode.SameString(number)
                );
        }

        /// <summary>
        /// Parses the record for exemplars.
        /// </summary>
        public ExemplarInfo[] FromRecord
            (
                Record record
            )
        {
            var result = ExemplarInfo.Parse(record);

            foreach (var exemplar in result)
            {
                Extend(exemplar, record);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the record is newspaper/magazine
        /// or not.
        /// </summary>
        public bool IsNewspaper
            (
                Record record
            )
        {
            var worklist = record.FM(920);
            if (worklist.IsEmpty())
            {
                return false;
            }

            if (!worklist.SameString("NJ"))
            {
                return false;
            }

            var index = record.FM(933);
            if (string.IsNullOrEmpty(index))
            {
                return false;
            }

            if (_newspapers.TryGetValue(index, out var result))
            {
                return result;
            }

            var main = Connection.SearchReadOneRecord($"\"I={index}\"");
            if (ReferenceEquals(main, null))
            {
                return false;
            }

            var kind = main.FM(110, 'b');
            result = kind.SameString("c");
            _newspapers[index] = result;
            return result;
        }

        /// <summary>
        /// List library places.
        /// </summary>
        public ChairInfo[] ListPlaces()
        {
            var result = ChairInfo.Read
                (
                    Connection,
                    "mhr.mnu",
                    false
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Load from file.
        /// </summary>
        public void LoadFromFile
            (
                string fileName
            )
        {
            var loaded = SerializationUtility
                .RestoreArrayFromFile<ExemplarInfo>(fileName);

            if (ReferenceEquals(loaded, null))
            {
                Magna.Error
                    (
                        "ExemplarManager::LoadFromFile: "
                        + "failed to load from: "
                        + fileName
                    );

                throw new IrbisException
                    (
                        "Failed to load exemplars from file"
                    );
            }

            foreach (var exemplar in loaded)
            {
                if (string.IsNullOrEmpty(exemplar.Number))
                {
                    continue;
                }
                var copy = Find(exemplar.Number);
                if (copy == null)
                {
                    _list.Add(exemplar);
                }
            }
        }

        /// <summary>
        /// Reads exemplar for given number.
        /// </summary>
        public ExemplarInfo? Read
            (
                string number
            )
        {
            var record = Connection.SearchReadOneRecord($"\"{Prefix}{number}\"");
            if (ReferenceEquals(record, null))
            {
                return null;
            }

            var fields = record.Fields.GetField(ExemplarInfo.ExemplarTag);
            foreach (var field in fields)
            {
                var result = ExemplarInfo.Parse(field);
                if (result.Barcode.SameString(number)
                    || result.Number.SameString(number))
                {
                    result.Record = record;
                    result.Mfn = record.Mfn;

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads exemplar for given number.
        /// </summary>
        public ExemplarInfo? ReadExtend
            (
                string number
            )
        {
            var record = Connection.SearchReadOneRecord($"\"{Prefix}{number}\"");
            if (ReferenceEquals(record, null))
            {
                return null;
            }

            var fields = record.Fields.GetField(ExemplarInfo.ExemplarTag);
            foreach (var field in fields)
            {
                var result = ExemplarInfo.Parse(field);
                if (result.Barcode.SameString(number)
                    || result.Number.SameString(number))
                {
                    result.Record = record;
                    result.Mfn = record.Mfn;
                    Extend(result, record);

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Read configuration.
        /// </summary>
        public void ReadConfiguration
            (
                string fileName,
                Encoding? encoding
            )
        {
            encoding ??= Encoding.UTF8;

            using var ini = new IniFile(fileName, encoding);
            var section = ini.GetSection("Main");
            if (!ReferenceEquals(section, null))
            {
                var format = section.GetValue("Format", Format);
                if (!string.IsNullOrEmpty(format))
                {
                    Format = format;
                }
                var prefix = section.GetValue("Prefix", Prefix);
                if (!string.IsNullOrEmpty(prefix))
                {
                    Prefix = prefix;
                }
            }
        }

        /// <summary>
        /// Save the configuration.
        /// </summary>
        public void SaveConfiguration
            (
                string fileName,
                Encoding? encoding
            )
        {
            encoding ??= Encoding.UTF8;

            using var ini = File.Exists(fileName)
                ? new IniFile(fileName, encoding, true)
                : new IniFile { Encoding = encoding };
            var section = ini.GetOrCreateSection("Main");
            section["Format"] = Format;
            section["Prefix"] = Prefix;

            ini.Save(fileName);
        }

        /// <summary>
        /// Save to the file.
        /// </summary>
        public void SaveToFile
            (
                string fileName
            )
        {
            _list.ToArray().SaveToZipFile(fileName);
        }

        /// <summary>
        /// Read many.
        /// </summary>
        public ExemplarManager ReadRange
            (
                string? place,
                string searchExpression
            )
        {
            var reader = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database!,
                    searchExpression,
                    1000
                );
            foreach (var record in reader)
            {
                var exemplars = FromRecord(record!);
                if (!string.IsNullOrEmpty(place))
                {
                    exemplars = exemplars
                        .Where(e => e.Place.SameString(place))
                        .ToArray();
                }
                foreach (var exemplar in exemplars)
                {
                    Add(exemplar);
                }
            }

            return this;
        }

        /// <summary>
        /// Remove
        /// </summary>
        public ExemplarManager Remove
            (
                string? number
            )
        {
            if (string.IsNullOrEmpty(number))
            {
                return this;
            }

            var found = Find(number);
            if (found != null)
            {
                _list.Remove(found);
            }

            return this;
        }

        /// <summary>
        /// Write line.
        /// </summary>
        public void WriteLine
            (
                string format,
                params object[] args
            )
        {
            _output?.WriteLine(format, args);
        }

        /// <summary>
        /// Write delimiter.
        /// </summary>
        public void WriteDelimiter()
        {
            WriteLine
                (
                    new string('=', 60)
                );
        }

        #endregion

    } // namespace ExemplarManager

} // namespace ManagedIrbis.Fields

