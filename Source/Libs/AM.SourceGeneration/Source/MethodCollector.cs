// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* MethodCollector.cs -- собирает упоминания методов класса, помеченных указанным атрибутом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

namespace AM.SourceGeneration
{
    /// <summary>
    /// Собирает упоминания методов класса, помеченных указанным атрибутом.
    /// </summary>
    internal sealed class MethodCollector
        : ISyntaxContextReceiver
    {
        #region Properties

        /// <summary>
        /// Собранные упоминания методов класса.
        /// </summary>
        public List<IMethodSymbol> Collected { get; } = new List<IMethodSymbol>();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="attributeName">Имя искомого атрибута.</param>
        public MethodCollector (string attributeName)
            => _attributeName = attributeName;

        #endregion

        #region Private members

        private readonly string _attributeName;

        #endregion

        #region ISyntaxContextReceiver

        /// <inheritdoc cref="ISyntaxContextReceiver.OnVisitSyntaxNode"/>
        public void OnVisitSyntaxNode
            (
                GeneratorSyntaxContext context
            )
        {
            if (context.Node is MethodDeclarationSyntax node
                && node.AttributeLists.Count > 0)
            {
                if (context.SemanticModel.GetDeclaredSymbol (node) is IMethodSymbol method
                    && method.HaveAttribute (_attributeName))
                {
                    Collected.Add (method);
                }
            }
        }

        #endregion
    }
}
