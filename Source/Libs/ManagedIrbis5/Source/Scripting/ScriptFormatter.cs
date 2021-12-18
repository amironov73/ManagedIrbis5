// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* ScriptFormatter.cs -- форматирует запись с помощью C#-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

// IL3000: Avoid accessing Assembly file path when publishing as a single file
#pragma warning disable IL3000

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using AM;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

#endregion

#nullable enable

namespace ManagedIrbis.Scripting
{
    /// <summary>
    /// Форматирует запись с помощью C#-скрипта.
    /// </summary>
    public sealed class ScriptFormatter
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Синхронный провайдер (на всякий случай).
        /// </summary>
        public ISyncProvider Provider { get; }

        /// <summary>
        /// Поток для вывода ошибок компиляции и подобного контента.
        /// </summary>
        public TextWriter ErrorWriter { get; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ScriptFormatter
            (
                ISyncProvider provider,
                ScriptCache? cache = null,
                TextWriter? errorWriter = null
            )
        {
            Provider = provider;
            _cache = cache;
            ErrorWriter = errorWriter ?? Console.Error;
        }

        #endregion

        #region Private members

        private readonly ScriptCache? _cache;

        class LocalOptions
        {
            /// <summary>
            /// Ссылки на сборки.
            /// </summary>
            public List<MetadataReference> References { get; } = new ();

            /// <summary>
            /// Добавление ссылок на сборки по умолчанию.
            /// </summary>
            public void AddDefaultReferences()
            {
                AddReference ("System.Runtime");
                AddReference (typeof (object));
                AddReference (typeof (Console));
                AddReference (typeof (System.Collections.IEnumerable));
                AddReference (typeof (List<>));
                AddReference (typeof (Encoding));
                AddReference (typeof (File));
                AddReference (typeof (Enumerable));
                AddReference ("System.ComponentModel");
                AddReference ("System.Data.Common");
                AddReference ("System.Linq.Expressions");

                AddReference (typeof (Utility));
                AddReference (typeof (ISyncProvider));

                AddReference (typeof (Microsoft.Extensions.Logging.Abstractions.NullLogger));
                AddReference (typeof (Microsoft.Extensions.Logging.Logger<>));
            }

            /// <summary>
            /// Добавление ссылки на указанную сборку.
            /// </summary>
            public void AddReference (string assemblyRef)
            {
                AddReference (Assembly.Load (assemblyRef));
            }

            /// <summary>
            /// Добавление ссылки на указанную сборку.
            /// </summary>
            private void AddReference (Assembly assembly)
            {
                // TODO: в single-exe-application .Location возвращает string.Empty
                // consider using the AppContext.BaseDirectory
                References.Add (MetadataReference.CreateFromFile (assembly.Location));
            }

            /// <summary>
            /// Добавление ссылки на сборку, содержащую указанный тип.
            /// </summary>
            private void AddReference (Type type)
            {
                AddReference (type.Assembly);
            }
        }

        /// <summary>
        /// Компилируем сборку в память.
        /// </summary>
        private byte[]? CompileCode
            (
                string sourceCode
            )
        {
            var localOptions = new LocalOptions();
            localOptions.AddDefaultReferences();

            var transformer = new ScriptTransformer();
            var fullScript = transformer.TransformScript (sourceCode);
            foreach (var additionalReference in transformer.References)
            {
                localOptions.AddReference (additionalReference);
            }

            var syntaxTree = CSharpSyntaxTree.ParseText (fullScript);
            var compilationOptions = new CSharpCompilationOptions (OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create
                (
                    "FormattingScript",
                    new[] { syntaxTree },
                    localOptions.References,
                    compilationOptions
                );
            var memory = new MemoryStream();
            var emit = compilation.Emit (memory);
            if (!emit.Success)
            {
                var failures = emit.Diagnostics.Where
                    (
                        diagnostic => diagnostic.IsWarningAsError
                                      || diagnostic.Severity == DiagnosticSeverity.Error
                    );

                foreach (var failure in failures)
                {
                    Console.Error.WriteLine ($"{failure.Id}: {failure.GetMessage()}");
                }

                return null;
            }

            return memory.ToArray();
        }

        /// <summary>
        /// Получение сборки для указанного исходного кода.
        /// </summary>
        private Assembly? GetAssemblyForCode
            (
                string sourceCode
            )
        {
            var binary = _cache?.GetAssembly (sourceCode);
            if (binary is null)
            {
                binary = CompileCode (sourceCode);
                if (binary is not null)
                {
                    _cache?.StoreAssembly (sourceCode, binary);
                }
            }

            if (binary is not null)
            {
                return Assembly.Load (binary);
            }

            return null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Расформатирование одной записи.
        /// </summary>
        public string FormatRecord
            (
                string sourceCode,
                Record record
            )
        {
            var output = new StringWriter();
            var instance = GetContextInstance (sourceCode, output);
            if (instance is null)
            {
                throw new IrbisException ("Can't create formatter instance");
            }

            instance.UserData = UserData;
            instance.BeforeAll();
            instance.Record = record;
            instance.FormatRecord();
            instance.Record = null;
            instance.AfterAll();

            return output.ToString();
        }

        /// <summary>
        /// Расформатирование нескольких записей.
        /// </summary>
        public string FormatRecords
            (
                string sourceCode,
                IEnumerable<Record> records
            )
        {
            var output = new StringWriter();
            var instance = GetContextInstance (sourceCode, output);
            if (instance is null)
            {
                throw new IrbisException ("Can't create formatter instance");
            }

            instance.UserData = UserData;
            instance.BeforeAll();
            foreach (var record in records)
            {
                instance.Record = record;
                instance.FormatRecord();
            }

            instance.Record = null;
            instance.AfterAll();

            return output.ToString();
        }

        /// <summary>
        /// Получение экземпляра контекста для расформатирования.
        /// </summary>
        public ScriptContext? GetContextInstance
            (
                string sourceCode,
                TextWriter output
            )
        {
            var assembly = GetAssemblyForCode (sourceCode);
            if (assembly is null)
            {
                return null;
            }

            var scriptType = assembly.GetTypes()
                .FirstOrDefault (type => type.IsAssignableTo (typeof (ScriptContext)))
                .ThrowIfNull ("Can't find script context type");
            var result = (ScriptContext?)Activator.CreateInstance
                (
                    scriptType,
                    Provider,
                    output
                );

            return result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            _cache?.Dispose();
        }

        #endregion
    }
}
