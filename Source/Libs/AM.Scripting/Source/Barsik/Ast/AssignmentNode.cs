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
        public string VariableName { get; }
        public AtomNode Expression { get; }

        public AssignmentNode (string variableName, AtomNode expression)
        {
            VariableName = variableName;
            Expression = expression;
        }

        public override void Execute (Context context)
        {
            context.Variables[VariableName] = Expression.Compute (context);
        }

        public override string ToString() => $"Assignment: {VariableName} = {Expression};";
    }
}
