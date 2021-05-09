// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnassignedGetOnlyAutoProperty

/* PftFormatter.cs -- PFT-форматтер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using AM;

using ManagedIrbis.Pft.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Временная заглушка для форматтера
    /// </summary>
    public class PftFormatter
        : IPftFormatter
    {
        #region Properties

        /// <summary>
        /// Сколько времени заняло форматирование.
        /// </summary>
        public TimeSpan Elapsed { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public PftFormatter()
            : this (new PftContext(null))
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="context">Корневой контекст.</param>
        public PftFormatter
            (
                PftContext context
            )
        {
            Context = context;
        }

        #endregion

        #region IPftFormatter members

        /// <inheritdoc cref="IPftFormatter.Program"/>
        public PftProgram? Program { get; set; }

        public virtual bool SupportsExtendedSyntax { get; }

        public PftContext Context { get; set; }

        /// <summary>
        /// Нормальный результат расформатирования.
        /// </summary>
        public string Output => Context.Text;

        /// <summary>
        /// Поток ошибок.
        /// </summary>
        public string Error => Context.Output.ErrorText;

        /// <summary>
        /// Поток предупреждений.
        /// </summary>
        public string Warning => Context.Output.WarningText;

        /// <summary>
        /// Have error?
        /// </summary>
        public bool HaveError => Context.Output.HaveError;

        /// <summary>
        /// Have warning.
        /// </summary>
        public bool HaveWarning => Context.Output.HaveWarning;


        public virtual string FormatRecord
            (
                Record? record
            )
        {
            if (ReferenceEquals(Program, null))
            {
                Magna.Error
                    (
                        "PftFormatter::Format: "
                        + "program was not set"
                    );

                throw new PftException("Program was not set");
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Context.ClearAll();
            Context.Record = record;
            Context.Procedures = Program.Procedures;
            Program.Execute(Context);

            var result = Context.GetProcessedOutput();

            stopwatch.Stop();
            Elapsed = stopwatch.Elapsed;

            return result;
        }

        public virtual string FormatRecord(int mfn) =>
            throw new NotImplementedException();

        public virtual string[] FormatRecords(int[] mfns) =>
            throw new NotImplementedException();

        public virtual void ParseProgram(string source)
        {
            Program = PftUtility.CompileProgram(source);
        }

        public virtual void SetProvider
            (
                ISyncProvider provider
            )
        {
            Context.SetProvider(provider);
        }

        #endregion

        #region IDisposable members

        public virtual void Dispose()
        {
            // TODO: implement
        }

        #endregion

    } // class PftFormatter

} // namespace ManagedIrbis.Pft
