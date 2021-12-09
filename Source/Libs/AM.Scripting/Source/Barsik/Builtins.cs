// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Builtins.cs -- встроенные функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
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
            { "delete", new FunctionDescriptor ("delete", Delete) },
            { "dict", new FunctionDescriptor ("dict", Dict) },
            { "dispose", new FunctionDescriptor ("dispose", Dispose) },
            { "error", new FunctionDescriptor ("error", Error) },
            { "format", new FunctionDescriptor ("format", Format) },
            { "have_var", new FunctionDescriptor ("have_var", HaveVariable) },
            { "italic", new FunctionDescriptor ("italic", Italic) },
            { "now", new FunctionDescriptor ("now", Now) },
            { "readln", new FunctionDescriptor ("readln", Readln) },
            { "system", new FunctionDescriptor ("system", System) },
            { "trace", new FunctionDescriptor ("trace", Trace) },
            { "trim", new FunctionDescriptor ("trim", Trim) },
            { "warn", new FunctionDescriptor ("warn", Warn) },
        };

        /// <summary>
        /// Выделение текста жирным шрифтом.
        /// </summary>
        public static dynamic Bold (Context context, dynamic?[] args) =>
            "<b>" + args.FirstOrDefault() + "</b>";

        /// <summary>
        /// Чтение содержимого файла.
        /// </summary>
        public static dynamic Cat (Context context, dynamic?[] args) =>
            File.ReadAllText ((string) args.FirstOrDefault()!);

        /// <summary>
        /// Выдача отладочного сообщения.
        /// </summary>
        public static dynamic? Debug (Context context, dynamic?[] args)
        {
            global::System.Diagnostics.Debug.WriteLine ((object?)args.FirstOrDefault());

            return null;
        }

        /// <summary>
        /// Удаление из текущего контекста указанной переменной.
        /// </summary>
        public static dynamic? Delete (Context context, dynamic?[] args)
        {
            var name = (string) args[0]!;
            context.Variables.Remove (name);

            return null;
        }

        /// <summary>
        /// Создание словаря.
        /// </summary>
        public static dynamic Dict (Context context, dynamic?[] args) =>
            new Dictionary<dynamic, dynamic?>();

        /// <summary>
        /// Освобождение ресурса.
        /// </summary>
        public static dynamic? Dispose (Context context, dynamic?[] args)
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
        public static dynamic Error (Context context, dynamic?[] args) =>
            Console.Error.WriteLine (args.FirstOrDefault());

        /// <summary>
        /// Форматирование.
        /// </summary>
        public static dynamic Format (Context context, dynamic?[] args)
        {
            var format = (string) args[0]!;
            var other = args.Select (o => (object?) o).Skip (1).ToArray();
            var result = string.Format (CultureInfo.InvariantCulture, format, other);

            return result;
        }

        /// <summary>
        /// Проверка существования переменной с указанным именем
        /// (в любом контексте).
        /// </summary>
        public static dynamic HaveVariable (Context context, dynamic?[] args)
        {
            var name = (string) args[0]!;
            return context.TryGetVariable (name, out _);
        }

        /// <summary>
        /// Выделение текста курсивом.
        /// </summary>
        public static dynamic Italic (Context context, dynamic?[] args) =>
            "<i>" + args.FirstOrDefault() + "</i>";

        /// <summary>
        /// Текущие дата и время.
        /// </summary>
        public static dynamic Now (Context context, dynamic?[] args) => DateTime.Now;

        /// <summary>
        /// Чтение данных из файла.
        /// </summary>
        public static dynamic? Readln (Context context, dynamic?[] args)
        {
            return context.Input.ReadLine();
        }

        /// <summary>
        /// Выполнение внешней программы и получение ее выходного потока.
        /// </summary>
        public static dynamic System (Context context, dynamic?[] args) => throw new NotImplementedException();

        /// <summary>
        /// Трассировочное сообщение.
        /// </summary>
        public static dynamic? Trace (Context context, dynamic?[] args)
        {
            Magna.Trace ((string) args.FirstOrDefault()!);

            return null;
        }

        /// <summary>
        /// Обрезка начальных и конечных пробелов в строке.
        /// </summary>
        public static dynamic? Trim (Context context, dynamic?[] args)
        {
            var text = (string?) args.FirstOrDefault();

            return string.IsNullOrEmpty (text)
                ? text
                : text.Trim();
        }

        /// <summary>
        /// Предупреждающее сообщение.
        /// </summary>
        public static dynamic Warn (Context context, dynamic?[] args) =>
            Magna.Warning (args.FirstOrDefault());

        /// <summary>
        /// Запись данных в файл.
        /// </summary>
        public static dynamic Write (Context context, dynamic?[] args) => throw new NotImplementedException();


        #endregion
    }
}
