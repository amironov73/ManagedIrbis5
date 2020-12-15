// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Record.cs -- библиографическая запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using AM;
using AM.Collections;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Библиографическая запись. Состоит из произвольного количества полей.
    /// </summary>
    [DebuggerDisplay("[{Database}] MFN={Mfn} ({Version})")]
    public sealed class Record
    {
        #region Properties

        /// <summary>
        /// Имя базы данных, в которой хранится запись.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN записи.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Версия записи.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Статус записи.
        /// </summary>
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Список полей.
        /// </summary>
        public List<Field> Fields { get; } = new ();

        /// <summary>
        /// Описание в произвольной форме (опциональное).
        /// </summary>
        public string? Description { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление поля в запись.
        /// </summary>
        /// <returns>
        /// Свежедобавленное поле.
        /// </returns>
        public Field Add
            (
                int tag,
                string? value = null
            )
        {
            var result = new Field {Tag = tag};
            result.DecodeBody(value);
            Fields.Add(result);

            return result;
        } // method Add

        /// <summary>
        /// Очистка записи (удаление всех полей).
        /// </summary>
        /// <returns>
        /// Очищенную запись.
        /// </returns>
        public Record Clear()
        {
            Fields.Clear();

            return this;
        } // method Clear

        /// <summary>
        /// Создание глубокой копии записи.
        /// </summary>
        public Record Clone()
        {
            var result = (Record) MemberwiseClone();

            for (int i = 0; i < result.Fields.Count; i++)
            {
                result.Fields[i] = result.Fields[i].Clone();
            }

            return result;
        } // method Clone

        /// <summary>
        /// Декодирование ответа сервера.
        /// </summary>
        public void Decode
            (
                Response response
            )
        {
            var line = response.ReadUtf();

            var first = line.Split('#');
            Mfn = int.Parse(first[0]);
            Status = (RecordStatus) first[1].SafeToInt32();

            line = response.ReadUtf();
            var second = line.Split('#');
            Version = int.Parse(second[1]);

            while (!response.EOT)
            {
                line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var field = new Field();
                field.Decode(line);
                Fields.Add(field);
            }
        } // method Decode

        /// <summary>
        /// Кодирование записи в текстовое представление.
        /// </summary>
        public string Encode
            (
                string delimiter = IrbisText.IrbisDelimiter
            )
        {
            StringBuilder result = new StringBuilder(512);
            result.Append(Mfn.ToInvariantString())
                .Append('#')
                .Append(((int) Status).ToInvariantString())
                .Append(delimiter)
                .Append("0#")
                .Append(Version.ToInvariantString())
                .Append(delimiter);

            foreach (var field in Fields)
            {
                result.Append(field)
                    .Append(delimiter);
            }

            return result.ToString();
        } // method Encode

        /// <summary>
        /// Получить текст поля до разделителей подполей
        /// первого повторения поля с указанной меткой.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <returns>Значение поля или <c>null</c>.</returns>
        public string? FM
            (
                int tag
            )
        {
            return GetFirstField(tag)?.Value;
        } // method FM

        /// <summary>
        /// Текст первого подполя с указанным тегом и кодом.
        /// </summary>
        public string? FM
            (
                int tag,
                char code
            )
        {
            var field = GetFirstField(tag);

            if (!ReferenceEquals(field, null))
            {
                return code == '*'
                    ? field.GetValueOrFirstSubField()
                    : field.GetFirstSubFieldValue(code);
            }

            return null;
        } // method FM

        /// <summary>
        /// Текст всех полей с указанным тегом.
        /// </summary>
        public string[] FMA
            (
                int tag
            )
        {
            var result = new LocalList<string>();

            foreach (var field in Fields)
            {
                if (field.Tag == tag
                    && !string.IsNullOrEmpty(field.Value))
                {
                    result.Add(field.Value);
                }
            }

            return result.ToArray();
        } // method FMA

        /// <summary>
        /// Текст всех подполей с указанным тегом и кодом.
        /// </summary>
        // ReSharper disable InconsistentNaming
        public string[] FMA
            (
                int tag,
                char code
            )
        {
            var result = new LocalList<string>();

            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    var value = code == '*'
                        ? field.GetValueOrFirstSubField()
                        : field.GetFirstSubFieldValue(code);
                    if (!string.IsNullOrEmpty(value))
                    {
                        result.Add(value);
                    }
                }
            }

            return result.ToArray();
        } // method FMA

        /// <summary>
        /// Получение первого поля с указанной меткой.
        /// </summary>
        public Field? GetFirstField
            (
                int tag
            )
        {
            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    return field;
                }
            }

            return null;
        } // method GetFirstField

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Encode("\n");
        } // method ToString

        #endregion

    } // class Record

} // namespace ManagedIrbis
