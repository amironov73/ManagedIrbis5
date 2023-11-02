// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* SourceGeneratorDiagnostic.cs -- диагностика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Microsoft.CodeAnalysis;

#endregion

#pragma warning disable RS2008 // отслеживание выпуска
#pragma warning disable RS1032 // диагностическое сообщение не должно содержать неправильных символов

namespace AM.SourceGeneration
{
    /// <summary>
    /// Диагностика.
    /// </summary>
    internal sealed class SourceGeneratorDiagnostic<TGenerator>
        where TGenerator : ISourceGenerator
    {
        private readonly DiagnosticDescriptor _errorRule = new DiagnosticDescriptor
            (
                "SG0001",
                "SG0001: Error in source generator",
                "Error in source generator<{0}>: '{1}'.",
                "SourceGenerator",
                DiagnosticSeverity.Error,
                true
            );

        private readonly DiagnosticDescriptor _errorRuleWithLog = new DiagnosticDescriptor
            (
                "SG0001",
                "SG0001: Error in source generator",
                "Error in source generator<{0}>: '{1}'. Log file details: '{2}'",
                "SourceGenerator",
                DiagnosticSeverity.Error,
                true
            );

        private readonly GeneratorExecutionContext _generatorExecutionContext;

        private readonly DiagnosticDescriptor _infoRule = new DiagnosticDescriptor
            (
                "SG0002",
                "SG0002: Source code generated",
                "Source code generated<{0}>",
                "SourceGenerator",
                DiagnosticSeverity.Info,
                true
            );

        private readonly SourceGeneratorOptions<TGenerator> _options;

        public SourceGeneratorDiagnostic
            (
                GeneratorExecutionContext generatorExecutionContext,
                SourceGeneratorOptions<TGenerator> options
            )
        {
            _generatorExecutionContext = generatorExecutionContext;
            _options = options;
        }

        public void ReportInformation
            (
                Location location = null!
            )
        {
            if (location == null!)
            {
                location = Location.None;
            }

            _generatorExecutionContext.ReportDiagnostic (Diagnostic.Create (_infoRule, location,
                typeof (TGenerator).Name));
        }

        public void ReportError
            (
                Exception e,
                Location location = null!
            )
        {
            if (location == null!)
            {
                location = Location.None;
            }

            if (_options.EnableLogging)
            {
                _generatorExecutionContext.ReportDiagnostic (Diagnostic.Create (_errorRuleWithLog, location,
                    typeof (TGenerator).Name, e.Message, _options.LogPath));
            }
            else
            {
                _generatorExecutionContext.ReportDiagnostic (Diagnostic.Create (_errorRule, location,
                    typeof (TGenerator).Name,
                    e.Message));
            }
        }
    }
}
