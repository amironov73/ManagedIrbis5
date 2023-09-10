// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Record.cs -- библиографическая запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Библиографическая запись. Состоит из произвольного количества полей.
    /// </summary>
    [DebuggerDisplay ("[{" + nameof (Database) +
                      "}] MFN={" + nameof (Mfn) + "} ({" + nameof (Version) + "})")]
    public sealed class Record
        : IEnumerable<Field>
    {
        #region Constants

        /// <summary>
        /// Запись удалена любым способом (логически или физически).
        /// </summary>
        private const RecordStatus IsDeleted = RecordStatus.LogicallyDeleted | RecordStatus.PhysicallyDeleted;

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
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Установка статуса записи.
        /// </summary>
        /// <param name="status">Новый статус записи.</param>
        /// <returns>this</returns>
        public Record Add
            (
                RecordStatus status
            )
        {
            Status = status;
            return this;
        }

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <returns>
        /// Свежедобавленное поле (для цепочечных вызовов).
        /// </returns>
        public Record Add
            (
                int tag,
                string? value = default
            )
        {
            var field = new Field (tag);
            if (!ReferenceEquals (value, null))
            {
                field.DecodeBody (value);
            }

            Fields.Add (field);

            return this;
        }

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <returns>
        /// this (для цепочечных вызовов).
        /// </returns>
        public Record Add
            (
                int tag,
                SubField subfield1
            )
        {
            var field = new Field (tag) { subfield1 };
            Fields.Add (field);

            return this;
        }

        /// <summary>
        /// Добавление полей в конец записи.
        /// </summary>
        /// <returns>
        /// this (для цепочечных вызовов).
        /// </returns>
        public Record Add
            (
                int tag,
                SubField subfield1,
                SubField subfield2
            )
        {
            var field = new Field (tag) { subfield1, subfield2 };
            Fields.Add (field);

            return this;
        }

        /// <summary>
        /// Добавление полей в конец записи.
        /// </summary>
        /// <returns>
        /// this (для цепочечных вызовов).
        /// </returns>
        public Record Add
            (
                int tag,
                SubField subfield1,
                SubField subfield2,
                SubField subfield3
            )
        {
            var field = new Field (tag) { subfield1, subfield2, subfield3 };
            Fields.Add (field);

            return this;
        }

        /// <summary>
        /// Добавление полей в конец записи.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="subfields">Подполя.</param>
        /// <returns>this (для цепочечных вызовов).</returns>
        public Record Add
            (
                int tag,
                params SubField[] subfields
            )
        {
            var field = new Field (tag);
            field.Subfields.AddRange (subfields);
            Fields.Add (field);

            return this;
        }

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code">Код подполя.</param>
        /// <param name="value">Значение подполя (опционально).</param>
        /// <returns>this</returns>
        public Record Add
            (
                int tag,
                char code,
                string? value = default
            )
        {
            var field = new Field (tag);
            field.Subfields.Add (new SubField (code, value));
            Fields.Add (field);

            return this;
        }

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code1">Код подполя.</param>
        /// <param name="value1">Значение подполя.</param>
        /// <param name="code2">Код подполя.</param>
        /// <param name="value2">Значение подполя (опционально).</param>
        /// <returns>this</returns>
        public Record Add
            (
                int tag,
                char code1,
                string? value1,
                char code2,
                string? value2 = default
            )
        {
            var field = new Field (tag);
            field.Subfields.Add (new SubField (code1, value1));
            field.Subfields.Add (new SubField (code2, value2));
            Fields.Add (field);

            return this;
        }

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code1">Код подполя.</param>
        /// <param name="value1">Значение подполя.</param>
        /// <param name="code2">Код подполя.</param>
        /// <param name="value2">Значение подполя.</param>
        /// <param name="code3">Код подполя.</param>
        /// <param name="value3">Значение подполя (опционально).</param>
        /// <returns>this</returns>
        public Record Add
            (
                int tag,
                char code1,
                string? value1,
                char code2,
                string? value2,
                char code3,
                string? value3 = default
            )
        {
            var field = new Field (tag);
            field.Subfields.Add (new SubField (code1, value1));
            field.Subfields.Add (new SubField (code2, value2));
            field.Subfields.Add (new SubField (code3, value3));
            Fields.Add (field);

            return this;
        }

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="subfields">Коды и значения подполей.</param>
        /// <returns>this (для цепочечных вызовов)</returns>
        public Record Add
            (
                int tag,
                string[] subfields
            )
        {
            var field = Field.WithSubFields (tag, subfields);
            Fields.Add (field);

            return this;
        }

        /// <summary>
        /// Добавление в запись непустого поля.
        /// </summary>
        public Record AddNonEmptyField
            (
                int tag,
                string? value
            )
        {
            if (!ReferenceEquals (value, null))
            {
                var field = new Field { Tag = tag };
                field.DecodeBody (value);
                Fields.Add (field);
            }

            return this;
        }

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
        }

        /// <summary>
        /// Создание глубокой копии записи.
        /// </summary>
        public Record Clone()
        {
            var result = (Record)MemberwiseClone();

            for (var i = 0; i < result.Fields.Count; i++)
            {
                result.Fields[i] = result.Fields[i].Clone();
            }

            return result;
        }

        /// <summary>
        /// Декодирование ответа сервера.
        /// </summary>
        public void Decode
            (
                Response response
            )
        {
            try
            {
                var line = response.ReadUtf();

                var first = line.Split ('#');
                Mfn = int.Parse (first[0]);
                Status = first.Length == 1
                    ? RecordStatus.None
                    : (RecordStatus)first[1].SafeToInt32();

                line = response.ReadUtf();
                var second = line.Split ('#');
                Version = second.Length == 1
                    ? 0
                    : int.Parse (second[1]);

                while (!response.EOT)
                {
                    line = response.ReadUtf();
                    if (string.IsNullOrEmpty (line))
                    {
                        break;
                    }

                    var field = new Field();
                    field.Decode (line);
                    Fields.Add (field);
                }
            }
            catch (Exception exception)
            {
                throw new IrbisException
                    (
                        nameof (Record) + "::" + nameof (Decode),
                        exception
                    );
            }
        }

        /// <summary>
        /// Декодирование ответа сервера.
        /// </summary>
        public void Decode
            (
                string[] lines
            )
        {
            try
            {
                var line = lines[0];

                var first = line.Split ('#');
                Mfn = int.Parse (first[0]);
                Status = first.Length == 1
                    ? RecordStatus.None
                    : (RecordStatus)first[1].SafeToInt32();

                line = lines[1];
                var second = line.Split ('#');
                Version = second.Length == 1
                    ? 0
                    : int.Parse (second[1]);

                for (var i = 2; i < lines.Length; i++)
                {
                    line = lines[i];
                    if (string.IsNullOrEmpty (line))
                    {
                        break;
                    }

                    var field = new Field();
                    field.Decode (line);
                    Fields.Add (field);
                }
            }
            catch (Exception exception)
            {
                throw new IrbisException
                    (
                        nameof (Record) + "::" + nameof (Decode),
                        exception
                    );
            }
        }

        /// <summary>
        /// Кодирование записи.
        /// </summary>
        public string Encode
            (
                string? delimiter = Constants.IrbisDelimiter
            )
        {
            var result = new StringBuilder (512);

            result.Append (Mfn.ToInvariantString())
                .Append ('#')
                .Append (((int)Status).ToInvariantString())
                .Append (delimiter)
                .Append ("0#")
                .Append (Version.ToInvariantString())
                .Append (delimiter);

            foreach (var field in Fields)
            {
                result.Append (field).Append (delimiter);
            }

            return result.ToString();
        }

        /// <summary>
        /// Получить текст поля до разделителей подполей
        /// первого повторения поля с указанной меткой.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <returns>Значение поля или <c>null</c>.</returns>
        public string? FM (int tag) => GetField (tag)?.Value;

        /// <summary>
        /// Текст первого подполя с указанным тегом и кодом.
        /// </summary>
        public string? FM
            (
                int tag,
                char code
            )
        {
            var field = GetField (tag);

            if (!ReferenceEquals (field, null))
            {
                return code == '*'
                    ? field.GetValueOrFirstSubField()
                    : field.GetSubFieldValue (code);
            }

            return default;
        }

        /// <summary>
        /// Текст всех полей с указанным тегом.
        /// </summary>
        public string[] FMA
            (
                int tag
            )
        {
            var result = new List<string>();

            foreach (var field in Fields)
            {
                if (field.Tag == tag
                    && !ReferenceEquals (field.Value, null))
                {
                    result.Add (field.Value);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Текст всех подполей с указанным тегом и кодом.
        /// </summary>
        public string[] FMA
            (
                int tag,
                char code
            )
        {
            var result = new List<string>();

            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    var value = code == '*'
                        ? field.GetValueOrFirstSubField()
                        : field.GetSubFieldValue (code);
                    if (!ReferenceEquals (value, null))
                    {
                        result.Add (value);
                    }
                }
            }

            return result.ToArray();
        }

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
        }

        /// <summary>
        /// Перечисление полей с указанной меткой.
        /// </summary>
        /// <param name="tag">Искомая метка поля.</param>
        public IEnumerable<Field> EnumerateField
            (
                int tag
            )
        {
            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    yield return field;
                }
            }
        }

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
            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    return field;
                }
            }

            var result = new Field { Tag = tag };
            Fields.Add (result);

            return result;
        }

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
        }

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
            while (GetField (tag) is { } field)
            {
                Fields.Remove (field);
            }

            return this;
        }

        /// <summary>
        /// Формирует плоское текстовое представление записи.
        /// </summary>
        public string ToPlainText()
        {
            var result = new StringBuilder();

            foreach (var field in Fields)
            {
                result.AppendFormat ("{0}#", field.Tag);
                foreach (var subField in field.Subfields)
                {
                    if (subField.Code != SubField.NoCode)
                    {
                        result.Append ('^');
                        result.Append (subField.Code);
                    }

                    result.Append (subField.Value);
                }

                result.AppendLine();
            }

            return result.ToString();
        }

        #endregion

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<Field> GetEnumerator() => Fields.GetEnumerator();

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Encode ("\n");
    }
}
