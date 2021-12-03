// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* DefinitionNode.cs -- псевдо-узел: определение функции в скрипте
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
    /// Псевдо-узел: определение функции в скрипте.
    /// </summary>
    sealed class DefinitionNode : PseudoNode
    {
        public DefinitionNode
            (
                string theName,
                IEnumerable<string>? argumentNames,
                IEnumerable<StatementNode>? body
            )
        {
            this.theName = theName;
            theArguments = new ();
            theBody = new ();
            if (argumentNames is not null)
            {
                theArguments.AddRange (argumentNames);
            }

            if (body is not null)
            {
                theBody.AddRange (body);
            }
        }

        internal readonly string theName;
        public readonly List<string> theArguments;
        internal readonly List<StatementNode> theBody;

        // public override void Execute (Context context)
        // {
        //     context.Output.WriteLine ($"Function {theName} ({string.Join (',', theArguments)})");
        //     foreach (var statement in theBody)
        //     {
        //         context.Output.WriteLine (statement);
        //     }
        //
        // }
    }
}
