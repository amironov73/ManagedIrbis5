// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* ScriptOptions.cs -- опции скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

#endregion

#nullable enable

namespace ManagedIrbis.Scripting
{
    /// <summary>
    /// Опции скрипта.
    /// </summary>
    public sealed class ScriptOptions
    {
        #region Properties

        /// <summary>
        /// Режим "приложения".
        /// </summary>
        public bool ApplicationMode { get; set; }

        /// <summary>
        /// Только выполнить, компилировать не надо.
        /// </summary>
        public bool ExecuteOnly { get; set; }

        /// <summary>
        /// Опции для компилятора.
        /// </summary>
        public CSharpCompilationOptions? CompilationOptions { get; set; }

        /// <summary>
        /// Не запускать, только скомпилировать.
        /// </summary>
        public bool CompileOnly { get; set; }

        /// <summary>
        /// Имя выходного файла.
        /// </summary>
        public string OutputName { get; set; } = "Script.dll";

        /// <summary>
        /// Директивы #using.
        /// </summary>
        public List<string> Usings { get; } = new ();

        /// <summary>
        /// Директивы #define.
        /// </summary>
        public List<string> Defines { get; } = new ();

        /// <summary>
        /// Имена файлов, подлежащих компиляции.
        /// </summary>
        public List<string> InputFiles { get; } = new ();

        /// <summary>
        /// Дополнительные ссылки на сборки.
        /// </summary>
        public List<string> References { get; } = new ();

        /// <summary>
        /// Не надо ссылок по умолчанию.
        /// </summary>
        public bool NoDefaultReferences { get; set; }

        #endregion

    } // class ScriptOptions

} // namespace ManagedIrbis.Scripting
