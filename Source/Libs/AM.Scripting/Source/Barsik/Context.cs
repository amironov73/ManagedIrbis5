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
using System.Reflection;

using AM.Collections;
using AM.IO;

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
    /// Точки останова.
    /// </summary>
    public Dictionary<StatementNode, Breakpoint> Breakpoints { get; }

    /// <summary>
    /// Отладчик.
    /// </summary>
    public IBarsikDebugger? Debugger { get; set; }

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

    /// <summary>
    /// Функции.
    /// </summary>
    public Dictionary<string, FunctionDescriptor> Functions { get; }

    /// <summary>
    /// Используемые пространства имен.
    /// </summary>
    public Dictionary<string, object?> Namespaces { get; }

    /// <summary>
    /// Обработчик внешнего кода.
    /// </summary>
    public ExternalCodeHandler? ExternalCodeHandler { get; set; }

    /// <summary>
    /// Загруженные модули.
    /// </summary>
    public List<IBarsikModule> Modules { get; }

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
        Functions = new ();
        Breakpoints = new ();
        Namespaces = new ();
        Modules = new ();
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

    /// <summary>
    /// Дамп пространств имен.
    /// </summary>
    public void DumpNamespaces()
    {
        var keys = Namespaces.Keys.ToArray();
        if (keys.IsNullOrEmpty())
        {
            Output.WriteLine ("(no namespaces)");
            return;
        }

        Array.Sort (keys);
        foreach (var key in keys)
        {
            Output.WriteLine (key);
        }
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

    /// <summary>
    /// Получение типа по его имени.
    /// </summary>
    public Type? FindType
        (
            AtomNode node
        )
    {
        Sure.NotNull (node);

        if (node is VariableNode variableNode)
        {
            if (TryGetVariable (variableNode.Name, out var variableValue))
            {
                if (variableValue is Type alreadyHaveType)
                {
                    return alreadyHaveType;
                }

                if (variableValue is string typeName1)
                {
                    return FindType (typeName1);
                }
            }

            return FindType (variableNode.Name);
        }

        var value = node.Compute (this);
        if (value is string typeName2)
        {
            return FindType (typeName2);
        }

        var typeName3 = BarsikUtility.ToString (value);

        return FindType (typeName3);
    }

    /// <summary>
    /// Получение типа по его имени.
    /// </summary>
    public Type? FindType
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        switch (name)
        {
            case "bool": return typeof (bool);
            case "byte": return typeof (byte);
            case "sbyte": return typeof (sbyte);
            case "short": return typeof (short);
            case "char": return typeof (char);
            case "ushort": return typeof (ushort);
            case "int": return typeof (int);
            case "uint": return typeof (uint);
            case "long": return typeof (long);
            case "ulong": return typeof (ulong);
            case "decimal": return typeof (decimal);
            case "float": return typeof (float);
            case "double": return typeof (double);
            case "object": return typeof (object);
            case "string": return typeof (string);
        }

        Type? result = Type.GetType (name, false);
        if (result is not null)
        {
            return result;
        }

        foreach (var ns in Namespaces.Keys)
        {
            var fullName = ns + "." + name;
            result = Type.GetType (fullName, false);
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// Поиск функции в текущем и в родительском контекстах.
    /// </summary>
    public FunctionDescriptor GetFunction
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        if (Functions.TryGetValue (name, out var result))
        {
            return result;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.Functions.TryGetValue (name, out result))
            {
                return result;
            }
        }

        if (Builtins.Registry.TryGetValue (name, out result))
        {
            return result;
        }

        throw new Exception ($"Function {name} not found");
    }

    /// <summary>
    /// Загрузка модуля.
    /// </summary>
    public void LoadModule
        (
            string moduleName
        )
    {
        // TODO поиск по путям

        Sure.NotNullNorEmpty (moduleName);

        moduleName = moduleName.Trim();

        Assembly? assembly;
        if (File.Exists (moduleName))
        {
            var fullPath = Path.GetFullPath (moduleName);
            assembly = Assembly.LoadFile (fullPath);
        }
        else
        {
            assembly = Assembly.Load (moduleName);
        }

        var types = assembly.GetTypes()
            .Where (type => type.IsAssignableTo (typeof (IBarsikModule)))
            .ToArray();
        foreach (var type in types)
        {
            var alreadyHave = false;
            foreach (var module in Modules)
            {
                if (module.GetType() == type)
                {
                    alreadyHave = true;
                    break;
                }
            }

            if (alreadyHave)
            {
                continue;
            }

            var instance = (IBarsikModule?) Activator.CreateInstance (type);
            if (instance is not null)
            {
                if (instance.AttachModule (this))
                {
                    Output.WriteLine ($"Module loaded: {instance}");
                    Modules.Add (instance);
                }
            }
        }
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
