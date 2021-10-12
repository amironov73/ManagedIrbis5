// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblDebugger.cs -- отладчик глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Отладчик глобальной корректировки.
    /// </summary>
    public sealed class GblDebugger
    {
        #region Properties

        /// <summary>
        /// Узел, подлежащий выполнению на следующем шаге
        /// (если есть).
        /// </summary>
        public GblNode? CurrentNode { get; }

        /// <summary>
        /// Отлаживааемая глобальная корректировка.
        /// </summary>
        public GblNodeCollection Program { get; }

        #endregion

        #region Construciton

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GblDebugger
            (
                GblNodeCollection program
            )
        {
            Program = program;

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Продолжение исполнения программы.
        /// </summary>
        public bool Continue()
        {
            return false;

        } // method Continue

        /// <summary>
        /// Запуск программы на выполнение.
        /// </summary>
        public bool Run()
        {
            return false;

        } // method Run

        /// <summary>
        /// Выполнение одного оператора.
        /// </summary>
        public bool StepOver()
        {
            return false;

        } // method StepOver

        #endregion

    } // class GblDebugger

} // namespace ManagedIrbis.Gbl.Infrastructure
