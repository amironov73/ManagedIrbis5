// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* AssignmentNode.cs -- присваивание переменной результата вычисления выражения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Присваивание переменной результата вычисления выражения.
    /// </summary>
    sealed class AssignmentNode : StatementNode
    {
        public AssignmentNode
            (
                TargetNode target,
                AtomNode expression
            )
        {
            _target = target;
            _expression = expression;
        }

        private readonly TargetNode _target;
        private readonly AtomNode _expression;

        public override void Execute
            (
                Context context
            )
        {
            var variableName = _target.VariableName;
            var memberName = _target.MemberName;
            var computedValue = _expression.Compute (context);
            var index = _target.Index;

            if (index is null && memberName is null)
            {
                context.Variables[variableName] = computedValue;
                return;
            }

            if (!context.TryGetVariable (variableName, out var variableValue))
            {
                var type = context.FindType (variableName);
                if (type is null)
                {
                    context.Error.WriteLine ($"Variable or type {variableName} not found");

                    return;
                }

                if (memberName is not null)
                {
                    var property = type.GetProperty (memberName);
                    if (property is not null)
                    {
                        property.SetValue (null, computedValue);
                        return;
                    }

                    var field = type.GetField (memberName);
                    if (field is not null)
                    {
                        field.SetValue (null, computedValue);
                        return;
                    }
                }

                return;
            }

            if (variableValue is null)
            {
                return;
            }

            if (memberName is not null)
            {

                var type = ((object)variableValue).GetType();
                var property = type.GetProperty (memberName);
                if (property is not null)
                {
                    property.SetValue (variableValue, computedValue);
                }
                else
                {
                    var field = type.GetField (memberName);
                    if (field is not null)
                    {
                        field.SetValue (variableValue, computedValue);
                    }
                }
            }

            if (index is not null)
            {
                var indexValue = index.Compute (context);

                variableValue[indexValue] = computedValue;
            }
        }

        public override string ToString() => $"Assignment: {_target.VariableName} = {_expression};";
    }
}
