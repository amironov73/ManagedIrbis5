// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* LocalFstProcessor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM.IO;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

#endregion

namespace ManagedIrbis.Fst
{
    /// <summary>
    /// Local FST processor.
    /// </summary>
    public sealed class LocalFstProcessor
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        public IrbisProvider Provider { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalFstProcessor
            (
                string rootPath,
                string database
            )
        {
            var environment = new LocalProvider(rootPath)
                {
                    Database = database
                };
            Provider = environment;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Read FST file from local file.
        /// </summary>
        public FstFile? ReadFile
            (
                string fileName
            )
        {
            string content = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            StringReader reader = new StringReader(content);
            FstFile result = FstFile.ParseStream(reader);
            if (result.Lines.Count == 0)
            {
                return null;
            }
            result.FileName = Path.GetFileName(fileName);

            return result;
        }

        /// <summary>
        /// Transform record.
        /// </summary>
        public Record TransformRecord
            (
                Record record,
                string format
            )
        {
            var program = PftUtility.CompileProgram(format);
            var context = new PftContext(null)
            {
                Record = record
            };

            context.SetProvider(Provider);
            program.Execute(context);
            string transformed = context.Text;

            var result = new Record
            {
                Database = Provider.Database
            };
            string[] lines = transformed.Split((char) 0x07);
            string[] separators = {"\r\n", "\r", "\n"};
            foreach (string line in lines)
            {
                string[] parts = line.Split
                    (
                        separators,
                        StringSplitOptions.RemoveEmptyEntries
                    );

                if (parts.Length == 0)
                {
                    continue;
                }

                string tag = parts[0];
                for (int i = 1; i < parts.Length; i++)
                {
                    string body = parts[i];
                    if (string.IsNullOrEmpty(body))
                    {
                        continue;
                    }
                    var field = RecordFieldUtility.Parse(tag, body);

                    SubField[] badSubFields
                        = field.Subfields
                        .Where(sf => string.IsNullOrEmpty(sf.Value))
                        .ToArray();
                    foreach (SubField subField in badSubFields)
                    {
                        field.Subfields.Remove(subField);
                    }

                    if (!string.IsNullOrEmpty(field.Value)
                        || field.Subfields.Count != 0)
                    {
                        result.Fields.Add(field);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Transform the record.
        /// </summary>
        public Record TransformRecord
            (
                Record record,
                FstFile fstFile
            )
        {
            string format = fstFile.ConcatenateFormat();
            var result = TransformRecord
                (
                    record,
                    format
                );

            return result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Provider.Dispose();
        }

        #endregion

    } // class LocalFstProcessor

} // namespace ManagedIrbis.Fst
