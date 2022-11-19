// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* RepeatingGroup.cs -- повторяющаяся группа при форматировании записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Повторяющаяся группа при форматировании записи.
/// </summary>
public sealed class RepeatingGroup
{
    #region Properties

    /// <summary>
    /// Библиографическая запись.
    /// </summary>
    public Record Record { get; set; }

    /// <summary>
    /// Метки повторяющихся полей.
    /// </summary>
    public int[] Tags { get; set; }

    /// <summary>
    /// Количество повторений.
    /// </summary>
    public int Count { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RepeatingGroup
        (
            Record record,
            int tag
        )
        : this (record, new[] { tag })
    {
        // пустое тело конструктора.
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RepeatingGroup
        (
            Record record,
            int[] tags
        )
    {
        Sure.NotNull (record);
        Sure.NotNull (tags);

        Record = record;
        Tags = tags;

        Count = 0;
        foreach (var tag in tags)
        {
            var count = record.Count (tag);
            if (count >= Count)
            {
                Count = count;
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Перечисление полей, входящих в повторяющуюся группу.
    /// </summary>
    public IEnumerable<Field> EnumerateFields()
    {
        foreach (var tag in Tags)
        {
            foreach (var field in Record.EnumerateField (tag))
            {
                yield return field;
            }
        }
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public Enumerator GetEnumerator() => new (this);

    /// <summary>
    /// Выполненение указанных действий для каждого повторения в группе.
    /// </summary>
    public RepeatingGroup ForEach
        (
            Action<Repeat> action
        )
    {
        Sure.NotNull (action);

        foreach (var repeat in this)
        {
            action (repeat);
        }

        return this;
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    public RepeatingGroup SetValue
        (
            char code,
            string? newValue
        )
    {
        foreach (var repeat in this)
        {
            repeat.SetValue (code, newValue);
        }

        return this;
    }

    /// <summary>
    /// Замена значения подполя.
    /// </summary>
    public RepeatingGroup ReplaceValue
        (
            char code,
            string? oldValue,
            string? newValue
        )
    {
        foreach (var repeat in this)
        {
            repeat.ReplaceValue (code, oldValue, newValue);
        }

        return this;
    }

    /// <summary>
    /// Команда вывода значения поля до первого разделителя
    /// или подполя с заданным кодом (с учетом повторяющихся групп).
    /// </summary>
    public static bool V
        (
            TextWriter output,
            Record? record,
            int tag,
            char? code = null,
            string? prefix = null,
            string? before = null,
            string? after = null,
            string? suffix = null,
            bool skipFirst = false,
            bool skipLast = false
        )
    {
        Sure.NotNull (output);

        if (record is null)
        {
            return false;
        }

        var result = false;
        var flag = false;
        var count = record.Count (tag);
        var index = 0;

        foreach (var field in record.EnumerateField (tag))
        {
            var value = code is null
                ? field.Value
                : field.GetFirstSubFieldValue (code.Value);
            if (!string.IsNullOrEmpty (value))
            {
                if (!flag)
                {
                    if (!string.IsNullOrEmpty (prefix))
                    {
                        output.Write (prefix);
                    }

                    flag = true;
                }

                if (!string.IsNullOrEmpty(before))
                {
                    if (index == 0)
                    {
                        if (!skipFirst)
                        {
                            output.Write (before);
                        }
                    }
                    else
                    {
                        output.Write (before);
                    }
                }

                output.Write (value);
                result = true;

                if (!string.IsNullOrEmpty (after))
                {
                    if (index == count - 1)
                    {
                        if (!skipLast)
                        {
                            output.Write (after);
                        }
                    }
                    else
                    {
                        output.Write (after);
                    }
                }
            }

            ++index;
        }

        if (flag)
        {
            if (!string.IsNullOrEmpty (suffix))
            {
                output.Write (suffix);
            }
        }

        return result;
    }

    #endregion

    #region Repeat

    /// <summary>
    /// Одно повторение группы.
    /// </summary>
    public readonly struct Repeat
    {
        #region Properties

        /// <summary>
        /// Ссылка на группу.
        /// </summary>
        public RepeatingGroup Group { get; }

        /// <summary>
        /// Индекс повторения (нумерация с нуля).
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Признак последнего повторения.
        /// </summary>
        public bool IsLast => Index == Group.Count - 1;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Repeat
            (
                RepeatingGroup group,
                int index
            )
            : this()
        {
            Sure.NotNull (group);
            Sure.NonNegative (index);

            Group = group;
            Index = index;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Перечисление полей, входящих в повторяющуюся группу.
        /// </summary>
        public IEnumerable<Field> EnumerateFields()
        {
            foreach (var tag in Group.Tags)
            {
                var field = Group.Record.GetField (tag, Index);
                if (field is not null)
                {
                    yield return field;
                }
            }
        }

        /// <summary>
        /// Значение до разделителя первого поля, среди заданных при создании группы.
        /// </summary>
        public string? FM()
        {
            return Group.Record.GetField (Group.Tags[0], Index)
                ?.Value;
        }

        /// <summary>
        /// Значение до разделителя текущего повторения поля с указанной меткой.
        /// </summary>
        public string? FM
            (
                int tag
            )
        {
            return Group.Record.GetField (tag, Index)?.Value;
        }

        /// <summary>
        /// Значение подполя текущего повторения поля с указанными меткой и кодом.
        /// </summary>
        public string? FM
            (
                int tag,
                char code
            )
        {
            return Group.Record.GetField (tag, Index)
                ?.GetFirstSubFieldValue (code);
        }

        /// <summary>
        /// Значение подполя с указанным кодом текущего повторения первого поля,
        /// заданного при создании группы.
        /// </summary>
        public string? FM
            (
                char code
            )
        {
            return Group.Record.GetField (Group.Tags[0], Index)
                ?.GetFirstSubFieldValue (code);
        }

        /// <summary>
        /// Замена значения подполя.
        /// </summary>
        public Repeat ReplaceValue
            (
                char code,
                string? oldValue,
                string? newValue
            )
        {
            foreach (var field in EnumerateFields())
            {
                if (field.GetSubFieldValue (code).SameString (oldValue))
                {
                    field.SetSubFieldValue (code, newValue);
                }
            }

            return this;
        }

        /// <summary>
        /// Установка значения подполя.
        /// </summary>
        public Repeat SetValue
            (
                char code,
                string? newValue
            )
        {
            foreach (var field in EnumerateFields())
            {
                field.SetSubFieldValue (code, newValue);
            }

            return this;
        }

        #endregion
    }

    #endregion

    #region Enumerator

    /// <inheritdoc cref="IEnumerator{T}"/>
    public struct Enumerator
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Enumerator
            (
                RepeatingGroup group
            )
            : this()
        {
            _group = group;
            _index = -1;
        }

        #endregion

        #region Private members

        private readonly RepeatingGroup _group;
        private int _index;

        #endregion

        #region IEnumeator members

        /// <inheritdoc cref="IEnumerator.MoveNext"/>
        public bool MoveNext() => ++_index < _group.Count;

        /// <inheritdoc cref="IEnumerator{T}.Current"/>
        public Repeat Current => new (_group, _index);

        #endregion
    }

    #endregion
}
