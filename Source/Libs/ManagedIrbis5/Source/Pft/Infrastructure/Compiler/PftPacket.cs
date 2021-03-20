// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftPacket.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PftPacket
        : MarshalByRefObject
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        public PftContext Context { get; private set; }

        /// <summary>
        /// Current field (if any).
        /// </summary>
        public FieldSpecification? CurrentField { get; set; }

        /// <summary>
        /// In group?
        /// </summary>
        public bool InGroup { get; set; }

        /// <summary>
        /// Breakpoints.
        /// </summary>
        public Dictionary<int, object> Breakpoints { get; } = new();

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftPacket
            (
                PftContext context
            )
        {
            Context = context;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Call the debugger.
        /// </summary>
        protected void CallDebugger()
        {
            var debugger = Context.Debugger;
            if (!ReferenceEquals(debugger, null))
            {
                var eventArgs = new PftDebugEventArgs
                    (
                        Context,
                        null
                    );
                debugger.Activate(eventArgs);
            }
        }

        /// <summary>
        /// Can output according given value.
        /// </summary>
        public bool CanOutput
            (
                FieldSpecification specification,
                string? value
            )
        {
            var result = !string.IsNullOrEmpty(value);
            if (specification.Command == 'n')
            {
                result = !result;
            }

            return result;
        }


        /// <summary>
        /// Debugger hook.
        /// </summary>
        protected void DebuggerHook
            (
                int nodeId
            )
        {
            if (Breakpoints.ContainsKey(nodeId))
            {
                CallDebugger();
            }
        }

        /// <summary>
        /// Do the conditional literal.
        /// </summary>
        protected void DoConditionalLiteral
            (
                string? literalText,
                FieldSpecification field,
                bool isSuffix
            )
        {
            if (string.IsNullOrEmpty(literalText))
            {
                return;
            }

            var flag = isSuffix
                ? IsLastRepeat(field)
                : IsFirstRepeat(field);

            if (flag)
            {
                var value = GetValue(field);

                if (CanOutput(field, value))
                {
                    if (Context.UpperMode)
                    {
                        literalText = IrbisText.ToUpper(literalText);
                    }

                    Context.Write(null, literalText);
                    Context.OutputFlag = true;
                }
            }
        }

        /// <summary>
        /// Do field.
        /// </summary>
        protected void DoField
            (
                FieldSpecification field,
                Action? leftHand,
                Action? rightHand
            )
        {
            if (ReferenceEquals(Context.Record, null))
            {
                return;
            }

            CurrentField = field;

            var command = field.Command;

            if (command == 'v')
            {
                if (InGroup)
                {
                    DoFieldV(field, leftHand, rightHand);
                }
                else
                {
                    DoRepeatableAction
                        (
                            () => DoFieldV(field, leftHand, rightHand)
                        );
                }
            }
            else if (command == 'd')
            {
                var value = GetValue(field);
                if (!string.IsNullOrEmpty(value))
                {
                    leftHand?.Invoke();
                }
            }
            else if (command == 'n')
            {
                var value = GetValue(field);
                if (string.IsNullOrEmpty(value))
                {
                    leftHand?.Invoke();
                }
            }

            CurrentField = null;
        }

        private void DoFieldD
            (
                FieldSpecification field,
                Action? leftHand
            )
        {
        }

        private void DoFieldG
            (
                Field? field,
                FieldSpecification spec,
                Action? leftHand,
                Action? rightHand
            )
        {
            if (ReferenceEquals(field, null))
            {
                return;
            }

            leftHand?.Invoke();

            // TODO implement properly

            string value = field.ToText();
            if (!string.IsNullOrEmpty(value))
            {
                if (Context.UpperMode)
                {
                    value = IrbisText.ToUpper(value);
                }
                Context.Write(null, value);
            }

            Context.OutputFlag = true;
            Context.VMonitor = true;

            rightHand?.Invoke();
        }

        private void DoFieldV
            (
                FieldSpecification field,
                Action? leftHand,
                Action? rightHand
            )
        {
            leftHand?.Invoke();

            var value = GetValue(field);
            if (!string.IsNullOrEmpty(value))
            {
                if (Context.UpperMode)
                {
                    value = IrbisText.ToUpper(value);
                }
                Context.Write(null, value);
            }

            if (HaveField(field))
            {
                Context.OutputFlag = true;
                Context.VMonitor = true;
            }

            rightHand?.Invoke();
        }

        /// <summary>
        /// Do global variable.
        /// </summary>
        protected void DoGlobal
            (
                int index,
                FieldSpecification spec,
                Action? leftHand,
                Action? rightHand
            )
        {
            var fields = Context.Globals.Get(index);
            if (fields.Length == 0)
            {
                return;
            }

            if (InGroup)
            {
                var field = fields.GetOccurrence(Context.Index);
                DoFieldG(field, spec, leftHand, rightHand);
            }
            else
            {
                for
                    (
                        Context.Index = 0;
                        Context.Index < PftConfig.MaxRepeat;
                        Context.Index++
                    )
                {
                    Context.OutputFlag = false;

                    var field = fields.GetOccurrence(Context.Index);
                    DoFieldG(field, spec, leftHand, rightHand);

                    if (!Context.OutputFlag || Context.BreakFlag)
                    {
                        break;
                    }
                }

                Context.Index = 0;
            }
        }

        /// <summary>
        /// Do group.
        /// </summary>
        protected void DoGroup
            (
                Action action
            )
        {
            InGroup = true;
            DoRepeatableAction(action);
            InGroup = false;
            Context.Index = 0;
        }

        /// <summary>
        /// Do the repeatable literal.
        /// </summary>
        protected void DoRepeatableLiteral
            (
                string? text,
                FieldSpecification field,
                bool isPrefix,
                bool plus
            )
        {
            var flag = field.Command == 'g'
                ? HaveGlobal(field)
                : HaveField(field);

            if (flag)
            {
                var value = GetValue(field);

                flag = CanOutput(field, value);
            }

            if (flag && plus)
            {
                flag = isPrefix
                    ? !IsFirstRepeat(field)
                    : !IsLastRepeat(field);
            }

            if (flag)
            {
                if (Context.UpperMode
                    && !ReferenceEquals(text, null))
                {
                    text = IrbisText.ToUpper(text);
                }
                Context.Write(null, text);
                Context.OutputFlag = true;
            }
        }

        private void DoRepeatableAction
            (
                Action action
            )
        {
            for
                (
                    Context.Index = 0;
                    Context.Index < PftConfig.MaxRepeat;
                    Context.Index++
                )
            {
                Context.OutputFlag = false;

                action();

                if (!Context.OutputFlag || Context.BreakFlag) //-V3022
                {
                    break;
                }
            }

            Context.Index = 0;
        }

        /// <summary>
        /// Evaluate as string.
        /// </summary>
        protected string? Evaluate
            (
                Action action
            )
        {
            using var guard = new PftContextGuard(Context);
            Context = guard.ChildContext;
            action();
            var result = Context.ToString();
            Context = guard.ParentContext;

            return result;
        }

        private string? GetValue
            (
                FieldSpecification spec
            )
        {
            var field = spec.Command == 'g'
                ? Context.Globals.Get(spec.Tag).GetOccurrence(Context.Index)
                : Context.Record.Fields.GetField(spec.Tag, Context.Index);
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            var result = PftUtility.GetFieldValue
                (
                    Context,
                    field,
                    spec.SubField,
                    spec.SubFieldRepeat
                );

            return result;
        }

        /// <summary>
        /// Goto new position.
        /// </summary>
        public void Goto
            (
                int newPosition
            )
        {
            var current = Context.Output.GetCaretPosition();
            var delta = newPosition - current;
            if (delta > 0)
            {
                Context.Write
                    (
                        null,
                        new string(' ', delta)
                    );
            }
            else
            {
                Context.WriteLine(null);
                Context.Write
                    (
                        null,
                        new string(' ', newPosition - 1)
                    );
            }
        }

        /// <summary>
        /// Have the field?
        /// </summary>
        protected bool HaveField
            (
                FieldSpecification spec
            )
        {
            var field = Context.Record.Fields.GetField
                (
                    spec.Tag,
                    Context.Index
                );

            return !ReferenceEquals(field, null);
        }

        /// <summary>
        /// Have the field?
        /// </summary>
        protected bool HaveGlobal
            (
                FieldSpecification spec
            )
        {
            var field = Context.Globals.Get(spec.Tag)
                .GetOccurrence(Context.Index);

            return !ReferenceEquals(field, null);
        }

        /// <summary>
        /// Signal output.
        /// </summary>
        protected void HaveOutput()
        {
            Context.OutputFlag = true;
        }

        /// <summary>
        /// First repeat?
        /// </summary>
        protected bool IsFirstRepeat
            (
                FieldSpecification field
            )
        {
            return Context.Index == 0;
        }

        /// <summary>
        /// Last repeat?
        /// </summary>
        protected bool IsLastRepeat
            (
                FieldSpecification field
            )
        {
            // ReSharper disable once PossibleNullReferenceException

            var count = Context.Record.Fields.GetFieldCount(field.Tag);

            return Context.Index >= count - 1;
        }

        /// <summary>
        /// Start evaluate.
        /// </summary>
        protected PftContext StartEvaluate()
        {
            var result = Context;
            Context = new PftContext(result);

            return result;
        }

        /// <summary>
        /// End evaluate.
        /// </summary>
        protected string EndEvaluate
            (
                PftContext previousContext
            )
        {
            var result = Context.ToString();
            Context = previousContext;

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the packet agains the record.
        /// </summary>
        public virtual string Execute
            (
                Record record
            )
        {
            Context.ClearAll();
            Context.Record = record;

            return String.Empty;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Context.ToString();
        }

        #endregion
    }
}
