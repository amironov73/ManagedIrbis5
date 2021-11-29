// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Builtins.cs -- встроенные функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Встроенные функции.
    /// </summary>
    public static class Builtins
    {
        #region Public methods

        /// <summary>
        /// Реестр встроенных функций.
        /// </summary>
        public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
        {
            { "bold", new FunctionDescriptor ("bold", Bold) },
            { "cat", new FunctionDescriptor ("cat", Cat) },
            { "debug", new FunctionDescriptor ("debug", Debug) },
            { "dict", new FunctionDescriptor ("dict", Dict) },
            { "dispose", new FunctionDescriptor ("dispose", Dispose) },
            { "error", new FunctionDescriptor ("error", Error) },
            { "italic", new FunctionDescriptor ("italic", Italic) },
            { "len", new FunctionDescriptor ("len", Len) },
            { "list", new FunctionDescriptor ("list", List) },
            { "now", new FunctionDescriptor ("now", Now) },
            { "open", new FunctionDescriptor ("open", Open) },
            { "read", new FunctionDescriptor ("read", Read) },
            { "system", new FunctionDescriptor ("system", System) },
            { "trace", new FunctionDescriptor ("trace", Trace) },
            { "trim", new FunctionDescriptor ("trim", Trim) },
            { "warn", new FunctionDescriptor ("warn", Warn) },
            { "write", new FunctionDescriptor ("write", Write) },
        };

        /// <summary>
        /// Выделение текста жирным шрифтом.
        /// </summary>
        public static dynamic Bold (dynamic?[] args) =>
            "<b>" + args.FirstOrDefault() + "</b>";

        /// <summary>
        /// Чтение содержимого файла.
        /// </summary>
        public static dynamic Cat (dynamic?[] args) =>
            File.ReadAllText ((string) args.FirstOrDefault()!);

        /// <summary>
        /// Выдача отладочного сообщения.
        /// </summary>
        public static dynamic? Debug (dynamic?[] args)
        {
            global::System.Diagnostics.Debug.WriteLine ((object?)args.FirstOrDefault());

            return null;
        }

        /// <summary>
        /// Создание словаря.
        /// </summary>
        public static dynamic Dict (dynamic?[] args) =>
            new Dictionary<dynamic, dynamic?>();

        /// <summary>
        /// Освобождение ресурса.
        /// </summary>
        public static dynamic? Dispose (dynamic?[] args)
        {
            if (args.FirstOrDefault() is IDisposable disposable)
            {
                disposable.Dispose();
            }

            return null;
        }

        /// <summary>
        /// Выдача сообщения в поток ошибок.
        /// </summary>
        public static dynamic Error (dynamic?[] args) =>
            Console.Error.WriteLine (args.FirstOrDefault());

        /// <summary>
        /// Выделение текста курсивом.
        /// </summary>
        public static dynamic Italic (dynamic?[] args) =>
            "<i>" + args.FirstOrDefault() + "</i>";

        /// <summary>
        /// Вычисление длины.
        /// </summary>
        public static dynamic? Len (dynamic?[] args)
        {
            var obj = args.FirstOrDefault();

            return obj switch
            {
                string s => s.Length,
                Array array => array.Length,
                IList list => list.Count,
                ICollection collection => collection.Count,
                _ => 0
            };
        }

        /// <summary>
        /// Список.
        /// </summary>
        public static dynamic List (dynamic?[] args) => new List<dynamic?>();

        /// <summary>
        /// Текущие дата и время.
        /// </summary>
        public static dynamic Now (dynamic?[] args) => DateTime.Now;

        /// <summary>
        /// Открытие файла.
        /// </summary>
        public static dynamic Open (dynamic?[] args)
        {
            var fileName = (string)args.FirstOrDefault()!;

            // TODO разные режимы

            var result = File.OpenRead (fileName);

            return result;
        }

        /// <summary>
        /// Чтение данных из файла.
        /// </summary>
        public static dynamic Read (dynamic?[] args) => throw new NotImplementedException();

        /// <summary>
        /// Выполнение внешней программы и получение ее выходного потока.
        /// </summary>
        public static dynamic System (dynamic?[] args) => throw new NotImplementedException();

        /// <summary>
        /// Трассировочное сообщение.
        /// </summary>
        public static dynamic? Trace (dynamic?[] args)
        {
            Magna.Trace ((string) args.FirstOrDefault()!);

            return null;
        }

        /// <summary>
        /// Обрезка начальных и конечных пробелов в строке.
        /// </summary>
        public static dynamic? Trim (dynamic?[] args)
        {
            var text = (string?) args.FirstOrDefault();

            return string.IsNullOrEmpty (text)
                ? text
                : text.Trim();
        }

        /// <summary>
        /// Предупреждающее сообщение.
        /// </summary>
        public static dynamic Warn (dynamic?[] args) =>
            Magna.Warning (args.FirstOrDefault());

        /// <summary>
        /// Запись данных в файл.
        /// </summary>
        public static dynamic Write (dynamic?[] args) => throw new NotImplementedException();


        #endregion
    }
}
