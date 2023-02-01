// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Barsor.cs -- аналог Blazor, только для Barsik
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System.Collections.Generic;

using AM.Kotik.Barsik.Ast;
using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/*
    Razor - это синтаксис разметки для внедрения кода на основе .NET в веб-страницы.
    Razor поддерживает C# и использует '@' символ для перехода с HTML на C#.
    Чтобы экранировать символ '@' в Razor разметке, надо удвоить его.
    HTML-атрибуты и содержимое, включающие адреса электронной почты, 
    не расценивают символ '@' как символ перехода.

    Неявные Razor выражения начинаются с '@':

    <p>@DateTime.Now</p>

    Неявные выражения не должны содержать пробелов.

    Явные Razor выражения состоят из символа '@' со сбалансированной скобкой:

    <p>Last week this time: @(DateTime.Now - TimeSpan.FromDays(7))</p>



 */

/// <summary>
/// Аналог Blazor, только для Barsik.
/// </summary>
public sealed class BarsorParser
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BarsorParser
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);
        
        _interpreter = interpreter;
    }

    #endregion
    
    #region Private members

    private readonly Interpreter _interpreter;
    
    /// <summary>
    /// Вызов <c>print</c> с указанным текстом.
    /// </summary>
    private StatementBase Print
        (
            string text
        )
    {
        var constant = new ConstantNode (text);

        return Print (constant);
    }

    /// <summary>
    /// Вызов <c>print</c> с указанным текстом.
    /// </summary>
    private StatementBase Print
        (
            AtomNode node
        )
    {
        var printCall = new CallNode ("print", new [] { node });
        var statement = new SimpleStatement (0, printCall);

        return statement;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор шаблона на стейтменты.
    /// </summary>
    public ProgramNode ParseTemplate
        (
            string templateText
        )
    {
        Sure.NotNull (templateText);

        // TODO не покупаться на @ в именах и в значениях атрибутов.

        var grammar = _interpreter.Settings.Grammar;
        var tokenizer = _interpreter.Settings.Tokenizer;
        var statements = new List<StatementBase>();
        var navigator = new ValueTextNavigator (templateText);
        while (!navigator.IsEOF)
        {
            var nextChar = navigator.PeekChar();
            if (nextChar == '@')
            {
                navigator.ReadChar();
                nextChar = navigator.PeekChar();
                
                if (nextChar == '{')
                {
                    navigator.ReadChar();
                    var source = navigator.ReadUntil ("{", "}", "}");
                    navigator.ReadChar(); // доедаем закрывающий символ '}'
                    if (!source.IsEmpty)
                    {
                        var subProgram = grammar.ParseProgram (source.ToString(), tokenizer);
                        statements.AddRange (subProgram.Statements);
                    }
                }
                
                else if (nextChar == '@')
                {
                    var statement = Print ("@");
                    statements.Add (statement);
                }
                
                else if (nextChar == '(')
                {
                    navigator.ReadChar();
                    var source = navigator.ReadUntil ("(", ")", ")");
                    navigator.ReadChar(); // доедаем закрывающий символ ')'
                    if (!source.IsEmpty)
                    {
                        var expression = grammar.ParseExpression (source.ToString(), tokenizer);
                        var statement = Print (expression);
                        statements.Add (statement);
                    }
                }
                
                else
                {
                    var source = navigator.ReadUntil (' ', '\t', '\r', '\n', '<');
                    if (!source.IsEmpty)
                    {
                        var expression = grammar.ParseExpression (source.ToString(), tokenizer);
                        var statement = Print (expression);
                        statements.Add (statement);
                    }
                }
            }
            else
            {
                var line = navigator.ReadUntil ('@');
                if (!line.IsEmpty)
                {
                    var statement = Print (line.ToString());
                    statements.Add (statement);
                }
            }
        }

        return new ProgramNode (statements);
    }

    #endregion
}
