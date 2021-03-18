// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftContext.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

using AM;
using AM.Text;



using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Text;


using Newtonsoft.Json;

#endregion

// ReSharper disable ConvertIfStatementToNullCoalescingExpression

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Контекст форматирования
    /// </summary>

    public sealed class PftContext
        : MarshalByRefObject,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// Text driver.
        /// </summary>
        public TextDriver Driver { get; private set; }

        /// <summary>
        /// Родительский контекст.
        /// </summary>
        public PftContext Parent { get { return _parent; } }

        /// <summary>
        /// Текущая форматируемая запись.
        /// </summary>
        [CanBeNull]
        public Record Record { get; set; }

        /// <summary>
        /// Alternative record (for nested context).
        /// </summary>
        public Record AlternativeRecord { get; set; }

        /// <summary>
        /// Выходной буфер, в котором накапливается результат
        /// форматирования, а также ошибки и предупреждения.
        /// </summary>
        public PftOutput Output { get; internal set; }

        /// <summary>
        /// Накопленный текст в основном потоке выходного буфера,
        /// т. е. собственно результат расформатирования записи.
        /// </summary>
        public string Text { get { return Output.ToString(); } }

        #region Режим вывода

        /// <summary>
        /// Режим вывода полей.
        /// </summary>
        public PftFieldOutputMode FieldOutputMode { get; set; }

        /// <summary>
        /// Режим перевода текста в верхний регистр при выводе полей.
        /// </summary>
        public bool UpperMode { get; set; }

        #endregion

        #region Работа с группами

        /// <summary>
        /// Текущая группа (если есть).
        /// </summary>
        [CanBeNull]
        public PftGroup CurrentGroup { get; set; }

        /// <summary>
        /// Номер повторения в текущей группе.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Флаг, устанавливается при наличии вывода при заданном повторении.
        /// </summary>
        public bool OutputFlag { get; internal set; }

        /// <summary>
        /// Флаг, устанавливается при срабатывании оператора break.
        /// </summary>
        public bool BreakFlag { get; internal set; }

        /// <summary>
        /// Текущее обрабатываемое поле записи, если есть.
        /// </summary>
        [CanBeNull]
        public PftField CurrentField { get; set; }

        #endregion

        /// <summary>
        /// Глобальные переменные.
        /// </summary>
        public PftGlobalManager Globals { get; private set; }

        /// <summary>
        /// Нормальные переменные.
        /// </summary>
        public PftVariableManager Variables { get; private set; }

        /// <summary>
        /// Функции, зарегистрированные в данном контексте.
        /// </summary>
        public PftFunctionManager Functions { get; private set; }

        /// <summary>
        /// Процедуры, видимые из данного контекста.
        /// </summary>
        public PftProcedureManager Procedures { get; internal set; }

        /// <summary>
        /// Универсальный счетчик.
        /// </summary>
        public int UniversalCounter { get; set; }

        /// <summary>
        /// Debugger (if attached).
        /// </summary>
        [CanBeNull]
        public PftDebugger Debugger { get; set; }

        /// <summary>
        /// Post processing flags.
        /// </summary>
        public PftCleanup PostProcessing { get; set; }

        /// <summary>
        /// Eat new newline.
        /// </summary>
        public bool EatNextNewLine;

        /// <summary>
        /// Отслеживает, был ли вывод из поля с помощью vXXX.
        /// </summary>
        public bool VMonitor;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftContext
            (
                [CanBeNull] PftContext parent
            )
        {
            Log.Trace("PftContext::Constructor");

            _parent = parent;

            Provider = ReferenceEquals(parent, null)
                ? new LocalProvider()
                : parent.Provider;

            PftOutput parentBuffer = ReferenceEquals(parent, null)
                ? null
                : parent.Output;

            Output = new PftOutput(parentBuffer);

            Driver = ReferenceEquals(parent, null)
                ? new PlainTextDriver(Output)
                : parent.Driver;

            Globals = ReferenceEquals(parent, null)
                ? new PftGlobalManager()
                : parent.Globals;

            Variables = ReferenceEquals(parent, null)
                ? new PftVariableManager(null)
                : parent.Variables;

            Procedures = ReferenceEquals(parent, null)
                ? new PftProcedureManager()
                : parent.Procedures;

            if (!ReferenceEquals(parent, null))
            {
                CurrentGroup = parent.CurrentGroup;
                CurrentField = parent.CurrentField;
                Index = parent.Index;
            }

            Record = ReferenceEquals(parent, null)
                ? new Record()
                : parent.Record;

            AlternativeRecord = ReferenceEquals(parent, null)
                ? null
                : parent.AlternativeRecord;

            Functions = new PftFunctionManager();

            Debugger = ReferenceEquals(parent, null)
                ? null
                : parent.Debugger;
        }

        #endregion

        #region Private members

        // ReSharper disable InconsistentNaming
        private readonly PftContext _parent;

        // ReSharper restore InconsistentNaming

        #endregion

        #region Public methods

        /// <summary>
        /// Activate debugger (if attached).
        /// </summary>
        public void ActivateDebugger
            (
                PftNode node
            )
        {
            Code.NotNull(node, "node");

            Log.Trace("PftContext::ActivateDebugger");

            if (!ReferenceEquals(Debugger, null))
            {
                PftDebugEventArgs args = new PftDebugEventArgs
                    (
                        this,
                        node
                    );
                Debugger.Activate(args);
            }
        }

        /// <summary>
        /// Полная очистка всех потоков: и основного,
        /// и предупреждений, и ошибок.
        /// </summary>
        public PftContext ClearAll()
        {
            Log.Trace("PftContext::ClearAll");

            Output.ClearText();
            Output.ClearError();
            Output.ClearWarning();

            return this;
        }

        /// <summary>
        /// Очистка основного выходного потока.
        /// </summary>
        public PftContext ClearText()
        {
            Log.Trace("PftContext::ClearText");

            Output.ClearText();

            return this;
        }

        /// <summary>
        /// Выполнить повторяющуюся группу.
        /// </summary>
        public void DoRepeatableAction
            (
                Action<PftContext> action,
                int count
            )
        {
            Code.NotNull(action, "action");
            Code.Nonnegative(count, "count");

            Log.Trace("PftContext::DoRepeatableAction");

            count = Math.Min(count, PftConfig.MaxRepeat);

            for (Index = 0; Index < count; Index++)
            {
                OutputFlag = false;

                action(this);

                if (!OutputFlag || BreakFlag)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Выполнить повторяющуюся группу
        /// максимально возможное число раз.
        /// </summary>
        public void DoRepeatableAction
            (
                Action<PftContext> action
            )
        {
            DoRepeatableAction(action, int.MaxValue);
        }

        /// <summary>
        /// Вычисление выражения во временной копии контекста.
        /// </summary>
        public string Evaluate
            (
                PftNode node
            )
        {
            Code.NotNull(node, "node");

            Log.Trace("PftContext::Evaluate");

            using (PftContextGuard guard = new PftContextGuard(this))
            {
                PftContext copy = guard.ChildContext;
                node.Execute(copy);
                string result = copy.ToString();

                return result;
            }
        }

        /// <summary>
        /// Вычисление выражения во временной копии контекста.
        /// </summary>
        public string Evaluate
            (
                IEnumerable<PftNode> items
            )
        {
            Code.NotNull(items, "items");

            Log.Trace("PftContext::Evaluate");

            using (PftContextGuard guard = new PftContextGuard(this))
            {
                PftContext copy = guard.ChildContext;
                foreach (PftNode node in items)
                {
                    node.Execute(copy);
                }
                string result = copy.ToString();

                return result;
            }
        }

        /// <summary>
        /// Execute the nodes.
        /// </summary>
        public void Execute
            (
                [CanBeNull] IEnumerable<PftNode> nodes
            )
        {
            Log.Trace("PftContext::Execute");

            if (!ReferenceEquals(nodes, null))
            {
                foreach (PftNode node in nodes)
                {
                    node.Execute(this);
                }
            }
        }

        /// <summary>
        /// Get processed output.
        /// </summary>
        public string GetProcessedOutput()
        {
            string result = Output.Text;

            if ((PostProcessing & PftCleanup.Rtf) != 0)
            {
                result = RichTextStripper.StripRichTextFormat(result);
            }

            if ((PostProcessing & PftCleanup.Html) != 0)
            {
                result = HtmlText.ToPlainText(result);
            }

            if ((PostProcessing & PftCleanup.DoubleText) != 0)
            {
                result = IrbisText.CleanupText(result);
            }

            if ((PostProcessing & PftCleanup.ContextMarkup) != 0)
            {
                result = IrbisText.CleanupMarkup(result);
            }

            if (ReferenceEquals(result, null))
            {
                result = string.Empty;
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Get root context.
        /// </summary>
        public PftContext GetRootContext()
        {
            PftContext result = this;

            while (!ReferenceEquals(result.Parent, null))
            {
                result = result.Parent;
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Get boolean argument value.
        /// </summary>
        [CanBeNull]
        public bool? GetBooleanArgument
            (
                PftNode[] arguments,
                int index
            )
        {
            Code.NotNull(arguments, "arguments");

            PftNode node = arguments.GetOccurrence(index);
            if (ReferenceEquals(node, null))
            {
                return null;
            }

            bool? result = null;

            PftCondition condition = node as PftCondition;
            if (ReferenceEquals(condition, null))
            {
                string text = GetStringArgument(arguments, index);
                bool boolVal;
                if (BooleanUtility.TryParse(text, out boolVal))
                {
                    result = boolVal;
                }
                int intVal;
                if (NumericUtility.TryParseInt32(text, out intVal))
                {
                    result = intVal != 0;
                }
            }
            else
            {
                Evaluate(condition);
                result = condition.Value;
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Get numeric argument value.
        /// </summary>
        [CanBeNull]
        public double? GetNumericArgument
            (
                PftNode[] arguments,
                int index
            )
        {
            Code.NotNull(arguments, "arguments");

            PftNode node = arguments.GetOccurrence(index);
            if (ReferenceEquals(node, null))
            {
                return null;
            }

            double? result = null;

            PftNumeric numeric = node as PftNumeric;
            if (ReferenceEquals(numeric, null))
            {
                string text = GetStringArgument(arguments, index);
                double val;
                if (NumericUtility.TryParseDouble(text, out val))
                {
                    result = val;
                }
            }
            else
            {
                Evaluate(numeric);
                result = numeric.Value;
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Get string argument value.
        /// </summary>
        [CanBeNull]
        public string GetStringArgument
            (
                PftNode[] arguments,
                int index
            )
        {
            Code.NotNull(arguments, "arguments");

            PftNode node = arguments.GetOccurrence(index);
            if (ReferenceEquals(node, null))
            {
                return null;
            }

            string result = Evaluate(node);

            return result;
        }

        //=================================================

        /// <summary>
        /// Get string argument value.
        /// </summary>
        [CanBeNull]
        public string GetStringValue
            (
                PftNode[] arguments,
                int index
            )
        {
            Code.NotNull(arguments, "arguments");

            string result = GetStringArgument(arguments, index);
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }

            double? number = GetNumericArgument(arguments, index);
            if (number.HasValue)
            {
                result = number.Value.ToInvariantString();
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Временное переключение контекста (например,
        /// при вычислении строковых функций).
        /// </summary>
        public PftContext Push()
        {
            PftContext result = new PftContext(this);

            return result;
        }

        /// <summary>
        /// Pop the context.
        /// </summary>
        public void Pop()
        {
            if (!ReferenceEquals(Parent, null))
            {
                Parent.BreakFlag |= BreakFlag;
                Parent.VMonitor |= VMonitor;
            }
        }

        /// <summary>
        /// Сбрасывает контекст в исходное состояние:
        /// нет повторяющейся группы, нет повторяющегося поля.
        /// </summary>
        public void Reset()
        {
            CurrentField = null;
            CurrentGroup = null;
            Index = 0;
        }

        /// <summary>
        /// Set provider.
        /// </summary>
        public void SetProvider
            (
                IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Provider = provider;
        }

        /// <summary>
        /// Set variables.
        /// </summary>
        /// <param name="variables"></param>
        public void SetVariables
            (
                PftVariableManager variables
            )
        {
            Code.NotNull(variables, "variables");

            Variables = variables;
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public PftContext Write
            (
                [CanBeNull] PftNode node,
                [CanBeNull] string output
            )
        {
            if (!string.IsNullOrEmpty(output))
            {
                Output.Write(output);
            }
            EatNextNewLine = false;

            return this;
        }

        /// <summary>
        /// Write and set <see cref="OutputFlag"/>.
        /// </summary>
        public PftContext WriteAndSetFlag
            (
                [CanBeNull] PftNode node,
                [CanBeNull] string output
            )
        {
            if (!string.IsNullOrEmpty(output))
            {
                Output.Write(output);
                OutputFlag = true;
            }
            EatNextNewLine = false;

            return this;
        }

        /// <summary>
        /// Write line.
        /// </summary>
        public PftContext WriteLine
            (
                [CanBeNull] PftNode node,
                [CanBeNull] string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Output.WriteLine(value);
            }

            return this;
        }

        /// <summary>
        /// Write line.
        /// </summary>
        public PftContext WriteLine
            (
                [CanBeNull] PftNode node
            )
        {
            Output.WriteLine();

            return this;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Log.Trace("PftContext::Dispose");

            Provider.Dispose();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Output.ToString();
        }

        #endregion
    }
}
