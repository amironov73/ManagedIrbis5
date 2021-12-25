// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Context.cs -- контекст исполнения скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.IO;

using Pidgin;
using Pidgin.Comment;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Контекст исполнения скрипта.
/// </summary>
public sealed class Context
{
    #region Properties

    /// <summary>
    /// Родительский контекст.
    /// </summary>
    public Context? Parent { get; }

    /// <summary>
    /// Переменные.
    /// </summary>
    public Dictionary<string, dynamic?> Variables { get; }

    /// <summary>
    /// Стандартный входной поток.
    /// </summary>
    public TextReader Input { get; }

    /// <summary>
    /// Стандартный выходной поток.
    /// </summary>
    public TextWriter Output { get; private set; }

    /// <summary>
    /// Выходной поток ошибок.
    /// </summary>
    public TextWriter Error { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Context
        (
            TextReader input,
            TextWriter output,
            TextWriter error,
            Context? parent = null
        )
    {
        Parent = parent;
        Variables = new ();
        Input = input;
        Output = output;
        Error = error;
    }

    #endregion

    #region Private members

    /// <summary>
    /// Делаем контекст внимательным к выводу текста.
    /// </summary>
    internal void MakeAttentive()
    {
        if (Output is not AttentiveWriter)
        {
            Output = new AttentiveWriter (Output);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание контекста-потомка.
    /// </summary>
    public Context CreateChild ()
    {
        return new Context
            (
                Input,
                Output,
                Error,
                this
            );
    }

    // /// <summary>
    // /// Дамп пространств имен.
    // /// </summary>
    // public void DumpNamespaces()
    // {
    //     var keys = Namespaces.Keys.ToArray();
    //     if (keys.IsNullOrEmpty())
    //     {
    //         Output.WriteLine ("(no namespaces)");
    //         return;
    //     }
    //
    //     Array.Sort (keys);
    //     foreach (var key in keys)
    //     {
    //         Output.WriteLine (key);
    //     }
    // }

    /// <summary>
    /// Дамп переменных.
    /// </summary>
    public void DumpVariables()
    {
        var keys = Variables.Keys.ToArray();
        // if (keys.IsNullOrEmpty())
        // {
        //     Output.WriteLine ("(no variables)");
        //     return;
        // }

        Array.Sort (keys);
        foreach (var key in keys)
        {
            var value = Variables[key];
            if (value is null)
            {
                Output.WriteLine ($"{key}: (null)");
            }
            else
            {
                Output.WriteLine ($"{key}: {value.GetType().Name} = {value}");
            }
        }
    }

    // /// <summary>
    // /// Получение типа по его имени.
    // /// </summary>
    // public Type? FindType
    //     (
    //         string name
    //     )
    // {
    //     Type? result = Type.GetType (name, false);
    //     if (result is not null)
    //     {
    //         return result;
    //     }
    //
    //     foreach (var ns in Namespaces.Keys)
    //     {
    //         var fullName = ns + "." + name;
    //         result = Type.GetType (fullName, false);
    //         if (result is not null)
    //         {
    //             return result;
    //         }
    //     }
    //
    //     return null;
    // }

    // /// <summary>
    // /// Поиск функции в текущем и в родительском контекстах.
    // /// </summary>
    // public FunctionDescriptor GetFunction
    //     (
    //         string name
    //     )
    // {
    //     Sure.NotNullNorEmpty (name);
    //
    //     if (Functions.TryGetValue (name, out var result))
    //     {
    //         return result;
    //     }
    //
    //     for (var context = Parent; context is not null; context = context.Parent)
    //     {
    //         if (context.Functions.TryGetValue (name, out result))
    //         {
    //             return result;
    //         }
    //     }
    //
    //     if (Builtins.Registry.TryGetValue (name, out result))
    //     {
    //         return result;
    //     }
    //
    //     throw new Exception ($"Function {name} not found");
    // }



    /// <summary>
    /// Вывод на печать значения AST-узла.
    /// </summary>
    public void Print
        (
            AtomNode node
        )
    {
        var value = node.Compute (this);
        BarsikUtility.PrintObject (Output, value);
    }

    /// <summary>
    /// Установка значения переменной
    /// (с сохранением ее места в контексте).
    /// </summary>
    public void SetVariable
        (
            string name,
            dynamic? value
        )
    {
        Sure.NotNullNorEmpty (name);

        if (Variables.ContainsKey (name))
        {
            Variables[name] = value;
            return;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.Variables.ContainsKey (name))
            {
                context.Variables[name] = value;
                return;
            }
        }

        Variables[name] = value;
    }

    /// <summary>
    /// Поиск переменной в текущем и в родительских контекстах.
    /// </summary>
    public bool TryGetVariable
        (
            string name,
            out dynamic? value
        )
    {
        Sure.NotNullNorEmpty (name);

        if (Variables.TryGetValue (name, out value))
        {
            return true;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.Variables.TryGetValue (name, out value))
            {
                return true;
            }
        }

        return false;
    }

    #endregion

}
