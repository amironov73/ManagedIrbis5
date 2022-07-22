﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchQueryParser.cs -- парсер поисковых запросов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

//
// See official documentation at
// http://wiki.elnit.org/index.php/%D0%AF%D0%B7%D1%8B%D0%BA_%D0%B7%D0%B0%D0%BF%D1%80%D0%BE%D1%81%D0%BE%D0%B2_%D0%98%D0%A0%D0%91%D0%98%D0%A1
//
// Запрос для прямого поиска представляет собой
// алгебраическое (поисковое) выражение, в котором
// операндами являются термины словаря, а операторами
// – логические операторы булевой алгебры. Для изменения
// порядка выполнения логических операторов в поисковом
// выражении могут применяться скобки.
//
// Термин словаря включает в себя собственно термин словаря
// и префикс, если таковой используется для данного вида терминов.
//
// В общем виде операнд поискового выражения можно
// представить следующим образом:
//
// "<префикс><термин>$"/(tag1, tag2,...tagN)
//
// где:
// <префикс> - префикс, определяющий вид термина(вид словаря).
// <термин> - собственно термин словаря.
// $ - признак правого усечения термина.Определяет
// совокупность терминов, имеющих начальную
// последовательность символов, совпадающую с указанным
// термином. Может отсутствовать, в этом случае поиск
// идет по точному значению указанного термина.
// " – символ-ограничитель термина (двойные кавычки).
// Должен использоваться обязательно, если термин включает
// в себя символы пробел, круглые скобки, решетка (#),
// а также символы, совпадающие с обозначениями логических
// операторов (см. ниже).
// /(tag1, tag2,...tagN) – конструкция квалификации
// термина.Определяет метки поля, в которых должен
// находиться указанный термин, или точнее – вторую
// часть индексной ссылки.Может отсутствовать, что
// означает отсутствие дополнительных требований
// в части меток полей.
//
// В поисковом выражении могут использоваться следующие
// логические операторы:
//
// + - оператор логического ИЛИ. Соединение двух
// операндов (терминов) логическим оператором ИЛИ
// обозначает требование поиска записей, в которых
// присутствует хотя бы один из терминов.
// * - оператор логического И.Соединение двух терминов
// логическим оператором И обозначает требование поиска
// записей, в которых присутствуют оба термина.
// ^ - оператор логического НЕ.Соединение двух терминов
// логическим оператором НЕ обозначает требование поиска
// записей, в которых присутствует первый термин
// и отсутствует второй. Оператор НЕ не может быть
// одноместным (т.е.данному оператору, как и всем другим,
// должен ОБЯЗАТЕЛЬНО предшествовать термин).
// (G) – первый оператор контекстного И. Соединение
// двух терминов таким оператором контекстного И
// обозначает требование поиска записей, в которых оба
// термина присутствуют в одном и том же поле
// (или точнее – когда у терминов совпадают вторые
// части ссылок).
// (F) – второй оператор контекстного И. Соединение
// двух терминов таким оператором контекстного И
// обозначает требование поиска записей, в которых
// оба термина присутствуют в одном и том же повторении
// поля (или точнее – когда у терминов совпадают
// вторые и третьи части ссылок).
// . – (точка обрамленная пробелами) третий оператор
// контекстного И.Соединение двух терминов таким
// оператором контекстного И обозначает требование
// поиска записей, в которых оба термина присутствуют
// в одном и том же повторении поля друг за другом
// (или точнее – когда у терминов совпадают вторые
// и третьи части ссылок, а третьи части ссылок
// отличаются на единицу).
//
// Логические операторы имеют приоритеты, которые
// определяют порядок их выполнения(в пределах одного
// уровня скобок). Ниже операторы приведены в порядке
// убывания приоритета:
// .
// (F)
// (G)
// * и ^
// +
// Операторы одного приоритета выполняются слева
// направо(в пределах одного уровня скобок).
// Для изменения порядка выполнения логических
// операторов в поисковом выражении могут применяться
// круглые скобки.Выражения в скобках могут
// объединяться только операторами + * ^.
//
// Примеры запросов для прямого поиска:
//
// ("A=Иванов$" +"A=Петров$") * ("V=03" + "V=05")
// "K=трактор$" (F) "K=колесн$" + "K=бульдозер$" (F) "K=гусен$"
// "K=очист$"/(200,922) * "K=вод$"/(200,922)

/// <summary>
/// Parse IRBIS query expression.
/// </summary>
public sealed class SearchQueryParser
{
    #region Properties

    /// <summary>
    /// Tokens.
    /// </summary>
    internal SearchTokenList Tokens { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchQueryParser
        (
            SearchTokenList tokens
        )
    {
        Tokens = tokens;
    }

    #endregion

    #region Private members

    private static void _AssignParentToChildren
        (
            ISearchTree parent
        )
    {
        var children = parent.Children;
        foreach (var child in children)
        {
            child.Parent = parent;
            _AssignParentToChildren (child);
        }
    }

    /// <summary>
    /// Leaf node.
    /// </summary>
    private SearchTerm ParseTerm()
    {
        var result = new SearchTerm();

        var token = Tokens.Current;
        if (token.Kind != SearchTokenKind.Term)
        {
            Magna.Logger.LogError
                (
                    nameof (SearchQueryParser) + "::" + nameof (ParseTerm)
                    + ": unexpected token={Token}",
                    token.ToVisibleString()
                );

            throw new SearchSyntaxException();
        }

        var text = token.Text.RequireSyntax ("token");
        if (text.EndsWith ("$") || text.EndsWith ("@"))
        {
            result.Tail = text.Substring (text.Length - 1, 1);
            text = text.Substring (0, text.Length - 1);
        }

        result.Term = text;

        if (!Tokens.MoveNext())
        {
            return result;
        }

        token = Tokens.Current;
        if (token.Kind != SearchTokenKind.Slash)
        {
            return result;
        }

        Tokens.RequireNext (SearchTokenKind.LeftParenthesis);
        Tokens.RequireNext();

        var context = new List<string>();
        while (true)
        {
            token = Tokens.Current;

            if (token.Kind == SearchTokenKind.RightParenthesis)
            {
                result.Context = context.ToArray();
                if (result.Context.Length == 0)
                {
                    result.Context = null;
                }

                Tokens.MoveNext();

                return result;
            }

            if (token.Kind == SearchTokenKind.Comma)
            {
                Tokens.RequireNext();
                continue;
            }

            if (token.Kind != SearchTokenKind.Term)
            {
                Magna.Logger.LogError
                    (
                        nameof (SearchQueryParser) + "::" + nameof (ParseTerm)
                        + ": unexpected token={Token}",
                        token.ToVisibleString()
                    );

                throw new SearchSyntaxException();
            }

            if (!string.IsNullOrEmpty (token.Text))
            {
                context.Add (token.Text);
            }

            Tokens.RequireNext();
        }
    }

    /// <summary>
    /// Term, Reference or Parenthesis
    /// </summary>
    SearchLevel0 ParseLevel0()
    {
        var result = new SearchLevel0();
        var token = Tokens.Current;

        switch (token.Kind)
        {
            case SearchTokenKind.Term:
                result.Term = ParseTerm();
                break;

            case SearchTokenKind.Hash:
                result.Reference = new SearchReference
                {
                    Number = token.Text
                };
                break;

            case SearchTokenKind.LeftParenthesis:
                result.Parenthesis = ParseLevel7();
                break;
        }

        return result;
    }

    /// <summary>
    /// item separator item
    /// </summary>
    private TLevel ParseLevel<TLevel, TItem>
        (
            Func<TItem> parse,
            SearchTokenKind separator
        )
        where TLevel : ComplexLevel<TItem>, new()
        where TItem : class, ISearchTree
    {
        var result = new TLevel();

        var item = parse();
        result.AddItem (item);

        while (!Tokens.IsEof
               && Tokens.Current.Kind == separator)
        {
            Tokens.RequireNext();
            item = parse();
            result.AddItem (item);
        }

        return result;
    }

    /// <summary>
    /// level0 . level0
    /// </summary>
    private SearchLevel1 ParseLevel1()
    {
        var result = new SearchLevel1();

        var item = ParseLevel0();
        result.AddItem (item);

        while (!Tokens.IsEof
               && Tokens.Current.Kind == SearchTokenKind.Dot)
        {
            Tokens.RequireNext();
            item = ParseLevel0();
            result.AddItem (item);
        }

        return result;
    }

    /// <summary>
    /// level1 (F) level1
    /// </summary>
    private SearchLevel2 ParseLevel2()
    {
        var result = ParseLevel<SearchLevel2, SearchLevel1>
            (
                ParseLevel1,
                SearchTokenKind.F
            );

        return result;
    }

    /// <summary>
    /// level2 (G) level2
    /// </summary>
    private SearchLevel3 ParseLevel3()
    {
        var result = ParseLevel<SearchLevel3, SearchLevel2>
            (
                ParseLevel2,
                SearchTokenKind.G
            );

        return result;
    }

    /// <summary>
    /// level3 * level3
    /// </summary>
    private SearchLevel4 ParseLevel4()
    {
        var result = ParseLevel<SearchLevel4, SearchLevel3>
            (
                ParseLevel3,
                SearchTokenKind.Star
            );

        return result;
    }

    /// <summary>
    /// level4 ^ level4
    /// </summary>
    private SearchLevel5 ParseLevel5()
    {
        var result = ParseLevel<SearchLevel5, SearchLevel4>
            (
                ParseLevel4,
                SearchTokenKind.Hat
            );

        return result;
    }

    /// <summary>
    /// level5 + level5
    /// </summary>
    private SearchLevel6 ParseLevel6()
    {
        var result = ParseLevel<SearchLevel6, SearchLevel5>
            (
                ParseLevel5,
                SearchTokenKind.Plus
            );

        return result;
    }

    /// <summary>
    /// ( tokens )
    /// </summary>
    private SearchLevel7 ParseLevel7()
    {
        var result = new SearchLevel7();
        SearchLevel6 item;

        if (Tokens.Current.Kind == SearchTokenKind.LeftParenthesis)
        {
            Tokens.RequireNext();
            item = ParseLevel6();
            result.AddItem (item);

            if (Tokens.IsEof)
            {
                Magna.Logger.LogError
                    (
                        nameof (SearchQueryParser) + "::" + nameof (ParseLevel7)
                        + ": unexpected end of stream"
                    );

                throw new SearchSyntaxException();
            }

            if (Tokens.Current.Kind != SearchTokenKind.RightParenthesis)
            {
                Magna.Logger.LogError
                    (
                        nameof (SearchQueryParser) + "::" + nameof (ParseLevel7)
                        + ": unexpected token {Token}",
                        Tokens.Current.ToVisibleString()
                    );

                throw new SearchSyntaxException();
            }

            Tokens.MoveNext();
        }
        else
        {
            item = ParseLevel6();
            result.AddItem (item);
        }

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Parse the token list.
    /// </summary>
    public SearchProgram Parse()
    {
        var result = new SearchProgram();

        if (Tokens.Length != 0)
        {
            var entryPoint = ParseLevel6();
            result.EntryPoint = entryPoint;
        }

        _AssignParentToChildren (result);

        return result;
    }

    #endregion
}
