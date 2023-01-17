// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Context.cs -- контекст исполнения скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

#nullable enable

namespace AM.Kotik;

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
    public TextReader Input { get; set; }

    /// <summary>
    /// Стандартный выходной поток.
    /// </summary>
    public TextWriter Output { get; set; }

    /// <summary>
    /// Стандартный поток ошибок.
    /// </summary>
    public TextWriter Error { get; set; }

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
        Sure.NotNull (input);
        Sure.NotNull (output);
        Sure.NotNull (error);

        Parent = parent;
        Variables = new ();
        Input = input;
        Output = output;
        Error = error;
    }

    #endregion

    #region Private members



    #endregion

    #region Public methods

    /// <summary>
    /// Создание контекста-потомка.
    /// </summary>
    public Context CreateChildContext ()
    {
        return new Context
            (
                Input,
                Output,
                Error,
                this
            );
    }

    /// <summary>
    /// Дамп переменных.
    /// </summary>
    public void DumpVariables()
    {
        var keys = Variables.Keys.ToArray();

        Array.Sort (keys);
        foreach (var key in keys)
        {
            var value = Variables[key];
            Output.WriteLine
                (
                    value is null
                        ? $"{key}: (null)"
                        : $"{key}: {value.GetType().Name} = {value}"
                );
        }
    }

    /// <summary>
    /// Получение топового контекста, используемого как свалка
    /// для регистрации модулей и сборок.
    /// </summary>
    public Context GetTopContext()
    {
        var result = this;

        while (result.Parent is not null)
        {
            result = result.Parent;
        }

        return result;
    }

    /// <summary>
    /// Вывод на печать значения AST-узла.
    /// </summary>
    public void Print
        (
            AtomNode node
        )
    {
        var value = node.Compute (this);
        KotikUtility.PrintObject (Output, value);
    }

    /// <summary>
    /// Удаление переменной с указанным именем.
    /// Если такой переменной не существует,
    /// то ничего не происходит.
    /// </summary>
    public void RemoveVariable
        (
            string variableName
        )
    {
        Sure.NotNullNorEmpty (variableName);

        for (var ctx = this; ctx is not null; ctx = ctx.Parent)
        {
            if (ctx.Variables.Remove (variableName))
            {
                return;
            }
        }
    }

    /// <summary>
    /// Сброс состояния интерпретатора.
    /// </summary>
    public void Reset()
    {
        Variables.Clear();
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

        // if (Builtins.IsBuiltinFunction (name))
        // {
        //     throw new BarsikException ($"{name} used by builtin function");
        // }
        //
        // if (Defines.ContainsKey (name))
        // {
        //     throw new BarsikException ($"Can't redefine {name}");
        // }

        // for (var context = Parent; context is not null; context = context.Parent)
        // {
        //     if (Defines.ContainsKey (name))
        //     {
        //         throw new BarsikException ($"Can't redefine {name}");
        //     }
        // }

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

        if (//Defines.TryGetValue (name, out value) ||
            Variables.TryGetValue (name, out value))
        {
            return true;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.TryGetVariable (name, out value)
                || context.Variables.TryGetValue (name, out value))
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}
