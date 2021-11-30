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

/* SelTabPar64.cs -- файл описания таблиц для ИРБИС64 в директории БД CMPL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Tables
{
    //
    // Official documentation:
    // http://sntnarciss.ru/irbis/spravka/px000020.htm
    //

    /// <summary>
    /// Файл описания таблиц для ИРБИС64 в директории БД CMPL.
    /// </summary>
    [XmlRoot ("selTabPar")]
    public sealed class SelTabPar64
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Таблицы.
        /// </summary>
        [XmlElement ("table")]
        [JsonPropertyName ("tables")]
        [Description ("Таблицы")]
        public NonNullCollection<AcquisitionTable> Tables { get; } = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текстового потока.
        /// </summary>
        public static SelTabPar64 ParseStream
            (
                TextReader reader
            )
        {
            Sure.NotNull (reader);

            // TODO проверить, правильно ли считывает

            var result = new SelTabPar64();
            while (true)
            {
                var name = reader.ReadLine();
                if (string.IsNullOrEmpty (name))
                {
                    break;
                }

                var table = new AcquisitionTable
                {
                    TableName = name,
                    SelectionMethod = reader.ReadLine().SafeToInt32(),
                    Worksheet = reader.ReadLine().EmptyToNull(),
                    Format = reader.ReadLine().EmptyToNull(),
                    Filter = reader.ReadLine().EmptyToNull(),
                    ModelField = reader.ReadLine().EmptyToNull()
                };
                result.Tables.Add (table);

                // набор строк, описывающих таблицу,
                // заканчивается строкой ‘*****’.

                reader.ReadLine();
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            reader.ReadCollection (Tables);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer.WriteCollection (Tables);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<SelTabPar64> (this, throwOnError);

            verifier
                .Positive (Tables.Count);

            foreach (var table in Tables)
            {
                verifier.VerifySubObject (table);
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return "Tables: " + Tables.Count.ToInvariantString();
        }

        #endregion
    }
}
