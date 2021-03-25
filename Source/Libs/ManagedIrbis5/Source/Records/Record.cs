// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedParameter.Local

/* Record.cs -- библиографическая запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;

using ManagedIrbis.Infrastructure;

using static ManagedIrbis.RecordStatus;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Библиографическая запись. Состоит из произвольного количества полей.
    /// </summary>
    [DebuggerDisplay("[{" + nameof(Database) +
        "}] MFN={" + nameof(Mfn) + "} ({" + nameof(Version) + "})")]
    public sealed class Record
        : IRecord,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Запись удалена любым способом (логически или физически).
        /// </summary>
        private const RecordStatus IsDeleted = LogicallyDeleted | PhysicallyDeleted;

        #endregion

        #region Properties

        /// <summary>
        /// База данных, в которой хранится запись.
        /// Для вновь созданных записей -- <c>null</c>.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN (порядковый номер в базе данных) записи.
        /// Для вновь созданных записей равен <c>0</c>.
        /// Для хранящихся в базе записей нумерация начинается
        /// с <c>1</c>.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Версия записи. Для вновь созданных записей равна <c>0</c>.
        /// Для хранящихся в базе записей нумерация версий начинается
        /// с <c>1</c>.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Статус записи. Для вновь созданных записей <c>None</c>.
        /// </summary>
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Признак -- запись помечена как логически удаленная.
        /// </summary>
        public bool Deleted => (Status & IsDeleted) != 0;

        /// <summary>
        /// Список полей.
        /// </summary>
        public List<Field> Fields { get; } = new ();

        /// <summary>
        /// Описание в произвольной форме (опциональное).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Признак того, что запись модифицирована.
        /// </summary>
        public bool Modified { get; internal set; }

        /// <summary>
        /// Индекс документа (поле 920).
        /// </summary>
        public string? Index { get; set; }

        /// <summary>
        /// Ключ для сортировки записей.
        /// </summary>
        public string? SortKey { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// Данное свойство используется, например,
        /// при построении отчета.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление поля в запись.
        /// </summary>
        /// <returns>
        /// Свежедобавленное поле (для цепочечных вызовов).
        /// </returns>
        public Field Add
            (
                int tag,
                string? value = default
            )
        {
            Sure.Positive(tag, nameof(tag));

            var result = new Field { Tag = tag };
            result.DecodeBody(value);
            Fields.Add(result);

            return result;
        } // method Add

        /// <summary>
        /// Добавление в запись непустого поля.
        /// </summary>
        public Record AddNonEmptyField
            (
                int tag,
                string? value
            )
        {
            Sure.Positive(tag, nameof(tag));

            if (!string.IsNullOrEmpty(value))
            {
                var field = new Field {Tag = tag};
                field.DecodeBody(value);
                Fields.Add(field);
            }

            return this;
        } // method AddNonEmptyField

        /// <summary>
        /// Очистка записи (удаление всех полей).
        /// </summary>
        /// <returns>
        /// Ту же самую, но очищенную запись.
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

            for (var i = 0; i < result.Fields.Count; i++)
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
            Status = first.Length == 1
                ? None
                : (RecordStatus) first[1].SafeToInt32();

            line = response.ReadUtf();
            var second = line.Split('#');
            Version = second.Length == 1
                ? 0
                : int.Parse(second[1]);

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
            var result = new StringBuilder(512);

            result.Append(Mfn.ToInvariantString())
                .Append('#')
                .Append(((int) Status).ToInvariantString())
                .Append(delimiter)
                .Append("0#")
                .Append(Version.ToInvariantString())
                .Append(delimiter);

            foreach (var field in Fields)
            {
                result.Append(field).Append(delimiter);
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
            return GetField(tag)?.Value;
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
            var field = GetField(tag);

            if (!ReferenceEquals(field, null))
            {
                return code == '*'
                    ? field.GetValueOrFirstSubField()
                    : field.GetSubFieldValue(code);
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
                        : field.GetSubFieldValue(code);
                    if (!string.IsNullOrEmpty(value))
                    {
                        result.Add(value);
                    }
                }
            }

            return result.ToArray();
        } // method FMA

        /// <summary>
        /// Получение заданного повторения поля с указанной меткой.
        /// </summary>
        public Field? GetField
            (
                int tag,
                int occurrence = 0
            )
        {
            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    if (occurrence == 0)
                    {
                        return field;
                    }

                    --occurrence;
                }
            }

            return null;
        } // method GetField

        /// <summary>
        /// Перечисление полей с указанной меткой.
        /// </summary>
        /// <param name="tag">Искомая метка поля.</param>
        public IEnumerable<Field> EnumerateField
            (
                int tag
            )
        {
            Sure.Positive(tag, nameof(tag));

            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    yield return field;
                }
            }
        } // method EnumerateField

        /// <summary>
        /// Получение поля с указанной меткой
        /// либо создание нового поля, если таковое отсутствует.
        /// </summary>
        /// <param name="tag">Искомая метка поля.</param>
        /// <returns>Найденное либо созданное поле.</returns>
        public Field GetOrAddField
            (
                int tag
            )
        {
            Sure.Positive(tag, nameof(tag));

            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    return field;
                }
            }

            var result = new Field { Tag = tag };
            Fields.Add(result);

            return result;
        } // method GetOrAddField

        /// <summary>
        /// Проверка, есть ли в записи поле с указанной меткой.
        /// </summary>
        public bool HaveField
            (
                int tag
            )
        {
            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    return true;
                }
            }

            return false;
        } // method HaveField

        /// <summary>
        /// Удаление из записи поля с указанной меткой.
        /// </summary>
        /// <param name="tag">Искомая метка.</param>
        /// <returns>this.</returns>
        public Record RemoveField
            (
                int tag
            )
        {
            Sure.Positive(tag, nameof(tag));

            Field? field;
            while ((field = GetField(tag)) is not null)
            {
                Fields.Remove(field);
            }

            return this;
        } // method RemoveField

        public string ToPlainText()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Record>(this, throwOnError);

            // TODO: implement

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Encode("\n");

        #endregion

    } // class Record

} // namespace ManagedIrbis
