// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* Directive.cs -- директива языка Барсик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Директива языка Барсик, например, загрузка сборки.
    /// </summary>
    public sealed class Directive
    {
        #region Properties

        /// <summary>
        /// Код директивы.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Аргумент директивы.
        /// </summary>
        public string? Argument { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Directive
            (
                string code,
                string? argument
            )
        {
            Code = code;
            Argument = argument;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Загрузка указанной сборки.
        /// </summary>
        private void Reference
            (
                string assembly
            )
        {
            Sure.NotNullNorEmpty (assembly);

            Assembly.Load (assembly);
        }

        /// <summary>
        /// Загрузка и исполнение скрипта из указанного файла.
        /// </summary>
        private void LoadScript
            (
                string scriptPath,
                Context context
            )
        {
            Sure.NotNullNorEmpty (scriptPath);

            var sourceCode = File.ReadAllText (scriptPath);
            var program = Grammar.ParseProgram (sourceCode);
            program.Execute (context);
        }

        /// <summary>
        /// Подключение пространства имен.
        /// </summary>
        private void UsingNamespace
            (
                string name,
                Context context
            )
        {
            Sure.NotNullNorEmpty (name);

            context.Namespaces[name] = null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Исполнение директивы.
        /// </summary>
        public void Execute
            (
                Context context
            )
        {
            switch (Code)
            {
                case "r":
                    Reference (Argument.ThrowIfNullOrEmpty ());
                    break;

                case "l":
                    LoadScript (Argument.ThrowIfNullOrEmpty (), context);
                    break;

                case "u":
                    UsingNamespace (Argument.ThrowIfNullOrEmpty (), context);
                    break;
            }
        }

        #endregion

        #region Object

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"#{Code} {Argument}";
        }

        #endregion
    }
}
