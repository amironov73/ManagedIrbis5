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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using AM.Collections;
using AM.Kotik.Barsik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Контекст исполнения скрипта.
/// </summary>
public sealed class Context
{
    #region Properties

    /// <summary>
    /// Интерпретатор, к которому привязан контекст.
    /// </summary>
    public Interpreter? Interpreter { get; internal set; }

    /// <summary>
    /// Выходной поток, ассоциированный с интерпретатором.
    /// </summary>
    public TextWriter? Output => Interpreter?.Output;

    /// <summary>
    /// Поток ошибок, ассоциированный с интерпретатором.
    /// </summary>
    public TextWriter? Error => Interpreter?.Error;

    /// <summary>
    /// Входной поток, ассоциированный с интерпретатором.
    /// </summary>
    public TextReader? Input => Interpreter?.Input;

    /// <summary>
    /// Родительский контекст.
    /// </summary>
    public Context? Parent { get; }

    /// <summary>
    /// Переменные.
    /// </summary>
    public Dictionary<string, dynamic?> Variables { get; }

    /// <summary>
    /// Функции.
    /// </summary>
    public Dictionary<string, FunctionDescriptor> Functions { get; }

    /// <summary>
    /// Используемые пространства имен.
    /// </summary>
    public Dictionary<string, object?> Namespaces { get; }

    /// <summary>
    /// Опциональный префикс, используемый, например, в операторе "new"
    /// при инициализации свойств свежесозданного объекта.
    /// </summary>
    public AtomNode? With { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Context
        (
            Context? parent = null
        )
    {
        Parent = parent;
        Interpreter = parent?.Interpreter;
        Variables = new ();
        Functions = new ();
        Namespaces = new ();

        var comparer = OperatingSystem.IsWindows()
            ? StringComparer.InvariantCultureIgnoreCase
            : StringComparer.InvariantCulture;
        _inclusions = new (comparer);
    }

    #endregion

    #region Private members

    /// <summary>
    /// Здесь запоминаются вложенные скрипты.
    /// </summary>
    internal readonly Dictionary<string, ProgramNode> _inclusions;

    /// <summary>
    /// Конструирование типа, если необходимо.
    /// </summary>
    private Type ConstructType
        (
            Type mainType,
            Type[]? typeArguments
        )
    {
        if (typeArguments is null)
        {
            return mainType;
        }

        if (!mainType.IsGenericType)
        {
            // TODO более осмысленная реакция
            return mainType;
        }

        return mainType.MakeGenericType (typeArguments);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Подключение модуля.
    /// </summary>
    public Context AttachModule
        (
            IBarsikModule instance
        )
    {
        Sure.NotNull (instance);

        var interpreter = GetTopContext().Interpreter.ThrowIfNull();
        if (instance.AttachModule (interpreter))
        {
            // Output.WriteLine ($"Module loaded: {instance}");
            interpreter.Modules.Add (instance);
        }

        return this;
    }

    /// <summary>
    /// Создание контекста-потомка.
    /// </summary>
    public Context CreateChildContext ()
    {
        return new Context (this);
    }

    /// <summary>
    /// Дамп пространств имен.
    /// </summary>
    public void DumpNamespaces()
    {
        var interpreter = GetTopContext().Interpreter;
        var keys = Namespaces.Keys.ToArray();
        if (keys.IsNullOrEmpty())
        {
            interpreter?.Output.WriteLine ("(no namespaces)");
            return;
        }

        Array.Sort (keys);
        foreach (var key in keys)
        {
            interpreter?.Output.WriteLine (key);
        }
    }

    /// <summary>
    /// Дамп переменных.
    /// </summary>
    public void DumpVariables()
    {
        var keys = Variables.Keys.ToArray();

        Array.Sort (keys);
        var interpreter = GetTopContext().Interpreter;
        foreach (var key in keys)
        {
            var value = Variables[key];
            interpreter?.Output.WriteLine
                (
                    value is null
                        ? $"{key}: (null)"
                        : $"{key}: {value.GetType().Name} = {value}"
                );
        }

        if (keys.Length == 0)
        {
            interpreter?.Output.WriteLine ("(no variables in the context)");
        }
    }

    /// <summary>
    /// Поиск функции с указанным именем.
    /// Отличается тем, что не швыряется исключениями.
    /// </summary>
    public bool FindFunction
        (
            string name,
            [MaybeNullWhen (false)] out FunctionDescriptor result
        )
    {
        Sure.NotNullNorEmpty (name);

        if (Functions.TryGetValue (name, out result))
        {
            return true;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.Functions.TryGetValue (name, out result))
            {
                return true;
            }
        }

        if (Builtins.Registry.TryGetValue (name, out result))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Получение типа по его имени.
    /// </summary>
    public Type? FindType
        (
            string name,
            IList<string>? arguments = null
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

            // наши псевдо-типы
            case "list": return typeof (BarsikList);
            case "dict": return typeof (BarsikDictionary);
        }

        Type[]? typeArguments = null;
        if (arguments is not null)
        {
            var list = new List<Type>();
            foreach (var oneArgument in arguments)
            {
                var foundType = FindType (oneArgument);
                if (foundType is null)
                {
                    // TODO более осмысленная реакция на ошибку
                    return null;
                }

                list.Add (foundType);
            }

            typeArguments = list.ToArray();
        }

        if (typeArguments is not null && !name.Contains ('`'))
        {
            // TODO разбирать на имя типа и сборку
            name += $"`{typeArguments.Length}";
        }

        var result = Type.GetType (name, false);
        if (result is not null)
        {
            return ConstructType (result, typeArguments);
        }

        var topContext = GetTopContext();
        var interpreter = topContext.Interpreter.ThrowIfNull();
        if (!name.Contains ('.'))
        {
            // это не полное имя, так что попробуем приписать к нему
            // различные пространства имен
            foreach (var ns in topContext.Namespaces.Keys)
            {
                var fullName = ns + "." + name;
                result = Type.GetType (fullName, false);
                if (result is not null)
                {
                    return ConstructType (result, typeArguments);
                }
            }
        }

        if (!name.Contains (','))
        {
            // это не assembly-qualified name, так что попробуем
            // приписать к нему загруженные нами сборки
            foreach (var asm in interpreter.Assemblies.Values)
            {
                var asmName = asm.GetName().Name;
                var fullName = name + ", " + asmName;
                result = Type.GetType (fullName, false);
                if (result is not null)
                {
                    return ConstructType (result, typeArguments);
                }

                if (!name.Contains ('.'))
                {
                    // это не полное имя, так что попробуем приписать к нему
                    // различные пространства имен
                    foreach (var ns in topContext.Namespaces.Keys)
                    {
                        fullName = ns + "." + name + ", " + asmName;
                        result = Type.GetType (fullName, false);
                        if (result is not null)
                        {
                            return ConstructType (result, typeArguments);
                        }
                    }
                }
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

        if (!FindFunction (name, out var result))
        {
            throw new Exception ($"Function {name} not found");
        }

        return result;
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
    /// Загрузка сборки.
    /// </summary>
    public Assembly? LoadAssembly
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        name = name.Trim();
        if (string.IsNullOrEmpty (name))
        {
            throw new BarsikException();
        }

        var interpreter = GetTopContext().Interpreter.ThrowIfNull();
        if (interpreter.Assemblies.ContainsKey (name))
        {
            // уже загружено, пропускаем
            return interpreter.Assemblies [name];
        }

        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var asm in allAssemblies)
        {
            var asmName = asm.GetName().Name;
            if (asmName.SameString (name))
            {
                interpreter.Assemblies.Add (name, asm);
                return asm;
            }
        }

        Assembly? result = null;
        if (File.Exists (name))
        {
            var fullPath = Path.GetFullPath (name);
            result = Assembly.LoadFile (fullPath);
        }
        else
        {
            try
            {
                result = Assembly.Load (name);
            }
            catch
            {
                // просто съедаем исключение
            }
        }

        if (result is not null)
        {
            interpreter.Assemblies.Add (name, result);
            return result;
        }

        var extension = Path.GetExtension (name);
        if (string.IsNullOrEmpty (extension))
        {
            name += ".dll";
        }
        else
        {
            if (extension.ToLowerInvariant () != ".dll")
            {
                name += ".dll";
            }
        }

        foreach (var part in interpreter.Pathes)
        {
            var fullPath = Path.GetFullPath (Path.Combine (part, name));
            if (File.Exists (fullPath))
            {
                result = Assembly.LoadFile (fullPath);
                interpreter.Assemblies.Add (name, result);

                return result;
            }
        }

        return result;
    }

    /// <summary>
    /// Загрузка модуля.
    /// </summary>
    public void LoadModule
        (
            string moduleName
        )
    {
        Sure.NotNullNorEmpty (moduleName);

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var interpreter = GetTopContext().Interpreter.ThrowIfNull();
            var types = assembly.GetTypes()
                .Where (type => type.IsAssignableTo (typeof (IBarsikModule)))
                .ToArray();
            foreach (var type in types)
            {
                var alreadyHave = false;
                foreach (var module in interpreter.Modules)
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
                    AttachModule (instance);
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
        var interpreter = GetTopContext().Interpreter;
        if (interpreter is not null)
        {
            KotikUtility.PrintObject (interpreter.Output, value);
        }
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

        if (Builtins.IsBuiltinFunction (name))
        {
            throw new BarsikException ($"{name} used by builtin function");
        }

        var interpreter = GetTopContext().Interpreter.ThrowIfNull();
        if (interpreter.Defines.ContainsKey (name))
        {
            throw new BarsikException ($"Can't redefine {name}");
        }

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

        var interpreter = GetTopContext().Interpreter.ThrowIfNull();
        if (interpreter.Defines.TryGetValue (name, out value) ||
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

    /// <summary>
    /// Выгрузка модуля.
    /// </summary>
    public bool UnloadModule
        (
            string moduleName
        )
    {
        Sure.NotNullNorEmpty (moduleName);

        var interpreter = GetTopContext().Interpreter.ThrowIfNull();
        IBarsikModule? found = null;
        foreach (var module in interpreter.Modules)
        {
            if (module.GetType().Name.SameString (moduleName))
            {
                found = module;
                break;
            }
        }

        if (found is not null)
        {
            found.DetachModule (interpreter);
            interpreter.Modules.Remove (found);

            return true;
        }

        return false;
    }

    #endregion
}
