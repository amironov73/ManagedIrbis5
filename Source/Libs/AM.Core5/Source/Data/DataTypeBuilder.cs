// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* DataTypeBuilder.cs -- генерирует типы с данными по описанию
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;
using System.Reflection.Emit;

#endregion

#nullable enable

namespace AM.Data;

/// <summary>
/// Генерирует типы с данными по описанию.
/// </summary>
public sealed class DataTypeBuilder
{
    #region Properties

    /// <summary>
    /// Пространство имен.
    /// </summary>
    public string Namespace { get; set; } = "AM.Data.Generated";

    #endregion

    #region Private members

    private static string Capitalize
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        var firstChar = char.ToUpperInvariant (name.FirstChar());

        return name.Length == 1
            ? firstChar.ToString()
            : firstChar + name.Substring (1);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Генерирует сборку с классами.
    /// </summary>
    public Assembly GenerateAssembly
        (
            DatabaseDescriptor database
        )
    {
        Sure.VerifyNotNull (database);

        var guid = Guid.NewGuid().ToString ("N");
        var assemblyName = new AssemblyName (guid);
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly
            (
                assemblyName,
                AssemblyBuilderAccess.Run
            );
        var moduleBuilder = assemblyBuilder.DefineDynamicModule (guid);
        var tables = database.Tables.ThrowIfNull();
        foreach (var table in tables)
        {
            GenerateClass (moduleBuilder, table);
        }

        return assemblyBuilder;
    }

    /// <summary>
    /// Генерирует класс по описанию.
    /// </summary>
    public void GenerateClass
        (
            ModuleBuilder moduleBuilder,
            TableDescriptor table
        )
    {
        Sure.NotNull (moduleBuilder);
        Sure.VerifyNotNull (table);

        var tableName = Capitalize (table.Name.ThrowIfNullOrEmpty());
        if (!string.IsNullOrEmpty (Namespace))
        {
            tableName = Namespace + "." + tableName;
        }
        var typeBuilder = moduleBuilder.DefineType
            (
                tableName,
                TypeAttributes.Public
                |TypeAttributes.Sealed
                |TypeAttributes.Class
            );

        var fields = table.Fields.ThrowIfNull();
        foreach (var field in fields)
        {
            GenerateProperty (typeBuilder, field);
        }

        typeBuilder.CreateType();
    }

    /// <summary>
    /// Генерирует свойство по описанию.
    /// </summary>
    public void GenerateProperty
        (
            TypeBuilder typeBuilder,
            FieldDescriptor field
        )
    {
        Sure.NotNull (typeBuilder);
        Sure.VerifyNotNull (field);

        Type propertyType;
        switch (field.Type)
        {
            case DataType.Boolean:
                propertyType = typeof (bool);
                break;

            case DataType.Integer:
                switch (field.Length)
                {
                    case 0:
                    case 4:
                        propertyType = typeof (int);
                        break;

                    case 1:
                        propertyType = typeof (byte);
                        break;

                    case 2:
                        propertyType = typeof (short);
                        break;

                    case 8:
                        propertyType = typeof (long);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException (nameof (field.Length));
                }

                break;

            case DataType.Money:
                propertyType = typeof (decimal);
                break;

            case DataType.Text:
                propertyType = typeof (string);
                break;

            case DataType.Date:
                propertyType = typeof (DateTime);
                break;

            default:
                throw new ArgumentOutOfRangeException (nameof (field.Type));
        }

        var fieldName = field.Name.ThrowIfNullOrEmpty();
        var propertyName = Capitalize (fieldName);

        var fieldBuilder = typeBuilder.DefineField
            (
                "_" + fieldName,
                propertyType,
                FieldAttributes.Private
            );
        var propertyBuilder = typeBuilder.DefineProperty
            (
                propertyName,
                PropertyAttributes.None,
                propertyType,
                null
            );
        var getterBuilder = typeBuilder.DefineMethod
            (
                "get_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType,
                Type.EmptyTypes
            );
        var getterGen = getterBuilder.GetILGenerator();
        getterGen.Emit (OpCodes.Ldarg_0);
        getterGen.Emit (OpCodes.Ldfld, fieldBuilder);
        getterGen.Emit (OpCodes.Ret);

        var setterBuilder = typeBuilder.DefineMethod
            (
                "set_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                new[] { propertyType }
            );
        var setterGen = setterBuilder.GetILGenerator();
        setterGen.Emit (OpCodes.Ldarg_0);
        setterGen.Emit (OpCodes.Ldarg_1);
        setterGen.Emit (OpCodes.Stfld, fieldBuilder);
        setterGen.Emit (OpCodes.Ret);

        propertyBuilder.SetGetMethod (getterBuilder);
        propertyBuilder.SetSetMethod (setterBuilder);
    }

    #endregion
}
