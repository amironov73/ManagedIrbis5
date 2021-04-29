// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedType.Global

/* PftContext.cs -- контекст, в котором исполняется PFT-скрипт
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Контекст, в котором исполняется PFT-скрипт.
    /// </summary>
    public sealed class PftContext
        : MarshalByRefObject,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// ИРБИС-провайдер.
        /// По умолчанию пустой провайдер (ничего не делает).
        /// Чтобы скрипт делал хоть что-нибудь осмысленное,
        /// провайдер нужно заменить (установить с помощью
        /// <see cref="SetProvider"/>) на нормальный,
        /// например, <see cref="ManagedIrbis.Direct.DirectProvider"/>.
        /// </summary>
        public ISyncProvider Provider { get; private set; }

        /// <summary>
        /// Драйвер вывода текста.
        /// По умолчанию -- простой плоский текст.
        /// </summary>
        public TextDriver Driver { get; }

        /// <summary>
        /// Родительский контекст.
        /// </summary>
        public PftContext? Parent { get; }

        /// <summary>
        /// Текущая форматируемая запись.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Альтернативная форматируемая запись (для вложенного контекста).
        /// </summary>
        public Record? AlternativeRecord { get; set; }

        /// <summary>
        /// Выходной буфер, в котором накапливается результат
        /// форматирования, а также ошибки и предупреждения.
        /// </summary>
        public PftOutput Output { get; internal set; }

        /// <summary>
        /// Накопленный текст в основном потоке выходного буфера,
        /// т. е. собственно результат расформатирования записи.
        /// </summary>
        public string Text => Output.ToString();

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
        public PftGroup? CurrentGroup { get; set; }

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
        public PftField? CurrentField { get; set; }

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
        public PftDebugger? Debugger { get; set; }

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
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительский контекст.
        /// Если не <c>null</c>, то от него наследуются
        /// ИРБИС-провайдер, драйвер текста, глобальные переменные
        /// и прочее.</param>
        public PftContext
            (
                PftContext? parent
            )
        {
            Parent = parent;

            Provider = ReferenceEquals(parent, null)
                ? new NullProvider()
                : parent.Provider;

            var parentBuffer = parent?.Output;

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

            AlternativeRecord = parent?.AlternativeRecord;

            Functions = new PftFunctionManager();

            Debugger = parent?.Debugger;
        } // constructor

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
            Magna.Trace(nameof(PftContext) + "::" + nameof(ActivateDebugger));

            if (Debugger is not null)
            {
                var args = new PftDebugEventArgs
                    {
                        Context = this,
                        Node = node
                    };
                Debugger.Activate(args);
            }
        }

        /// <summary>
        /// Полная очистка всех потоков: и основного,
        /// и предупреждений, и ошибок.
        /// </summary>
        public PftContext ClearAll()
        {
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
            Output.ClearText();

            return this;
        }

        /// <summary>
        /// Выполнить повторяющуюся группу.
        /// </summary>
        public void DoRepeatableAction
            (
                Action<PftContext> action,
                int count = int.MaxValue
            )
        {
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
        /// Вычисление выражения во временной копии контекста.
        /// </summary>
        public string Evaluate
            (
                PftNode node
            )
        {
            Magna.Trace("PftContext::Evaluate");

            using var guard = new PftContextGuard(this);
            var copy = guard.ChildContext;
            node.Execute(copy);
            var result = copy.ToString();

            return result;
        }

        /// <summary>
        /// Вычисление выражения во временной копии контекста.
        /// </summary>
        public string Evaluate
            (
                IEnumerable<PftNode> items
            )
        {
            Magna.Trace("PftContext::Evaluate");

            using var guard = new PftContextGuard(this);
            var copy = guard.ChildContext;
            foreach (var node in items)
            {
                node.Execute(copy);
            }
            var result = copy.ToString();

            return result;
        }

        /// <summary>
        /// Execute the nodes.
        /// </summary>
        public void Execute
            (
                IEnumerable<PftNode>? nodes
            )
        {
            Magna.Trace("PftContext::Execute");

            if (!ReferenceEquals(nodes, null))
            {
                foreach (var node in nodes)
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
            var result = Output.Text;

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

            return result ?? string.Empty;
        }

        //=================================================

        /// <summary>
        /// Get root context.
        /// </summary>
        public PftContext GetRootContext()
        {
            var result = this;

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
        public bool? GetBooleanArgument
            (
                PftNode[] arguments,
                int index
            )
        {
            var node = arguments.GetOccurrence(index);
            if (ReferenceEquals(node, null))
            {
                return null;
            }

            bool? result = null;

            if (!(node is PftCondition condition))
            {
                var text = GetStringArgument(arguments, index);
                if (bool.TryParse(text, out var boolVal))
                {
                    result = boolVal;
                }

                if (int.TryParse(text, out var intVal))
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
        public double? GetNumericArgument
            (
                PftNode[] arguments,
                int index
            )
        {
            var node = arguments.GetOccurrence(index);
            if (ReferenceEquals(node, null))
            {
                return null;
            }

            double? result = null;

            var numeric = node as PftNumeric;
            if (ReferenceEquals(numeric, null))
            {
                var text = GetStringArgument(arguments, index);
                if (double.TryParse(text, out var val))
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
        public string? GetStringArgument
            (
                PftNode[] arguments,
                int index
            )
        {
            var node = arguments.GetOccurrence(index);
            if (ReferenceEquals(node, null))
            {
                return null;
            }

            var result = Evaluate(node);

            return result;
        }

        //=================================================

        /// <summary>
        /// Get string argument value.
        /// </summary>
        public string? GetStringValue
            (
                PftNode[] arguments,
                int index
            )
        {
            var result = GetStringArgument(arguments, index);
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }

            var number = GetNumericArgument(arguments, index);
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
            var result = new PftContext(this);

            return result;
        }

        /// <summary>
        /// Pop the context.
        /// </summary>
        public void Pop()
        {
            if (Parent is not null)
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
        public void SetProvider (ISyncProvider provider) => Provider = provider;

        /// <summary>
        /// Set variables.
        /// </summary>
        /// <param name="variables"></param>
        public void SetVariables
            (
                PftVariableManager variables
            )
        {
            Variables = variables;
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public PftContext Write
            (
                PftNode? _,
                string? output
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
                PftNode? _,
                string? output
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
        /// Вывод текста с последующим переводом строки.
        /// </summary>
        public PftContext WriteLine
            (
                PftNode? _,
                string? text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                Output.WriteLine(text);
            }

            return this;
        }

        /// <summary>
        /// Перевод строки.
        /// </summary>
        public PftContext WriteLine
            (
                PftNode? _
            )
        {
            Output.WriteLine();

            return this;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose() => Provider.Dispose();

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Output.ToString();

        #endregion

    } // class PftContext

} // namespace ManagedIrbis.Pft.Infrastructure
