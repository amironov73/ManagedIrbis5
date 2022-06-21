// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertToLocalFunction
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BindingExtensions.cs -- методы расширения для байндинга
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для байндинга.
/// </summary>
public static class BindingExtensions
{
    #region Public methods

    /// <summary>
    /// Связывание свойства с источником данных.
    /// </summary>
    public static TControl Binding<TSource, TSourceProp, TControl, TTargetProp>
        (
            this TControl control,
            TSource source,
            Expression<Func<TSource, TSourceProp>> sourceProp,
            Expression<Func<TControl, TTargetProp>> targetProp,
            Func<TSourceProp, TTargetProp>? convert = null,
            Func<TTargetProp, TSourceProp>? convertBack = null
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (sourceProp);
        Sure.NotNull (targetProp);

        var propertyName = ((MemberExpression) targetProp.Body).Member.Name;
        var sourceChain = sourceProp.Body.ToString();
        sourceChain = sourceChain.Substring
            (
                sourceChain.IndexOf (".", StringComparison.Ordinal) + 1
            );
        var b = new Binding
            (
                propertyName,
                source,
                sourceChain,
                false,
                DataSourceUpdateMode.OnPropertyChanged
            );
        if (convert != null)
        {
            ConvertEventHandler doConvert = (_, args) => args.Value
                = convert (((TSourceProp)args.Value!));
            b.Format += doConvert;
        }

        if (convertBack != null)
        {
            ConvertEventHandler doConvert = (_, args) => args.Value
                = convertBack (((TTargetProp)args.Value!));
            b.Parse += doConvert;
        }

        control.DataBindings.Add (b);

        return control;
    }

    /// <summary>
    /// Связывание свойства с источником данных.
    /// </summary>
    public static TControl Binding<TSource, TSourceProp, TControl>
        (
            this TControl control,
            TSource source,
            Expression<Func<TSource, TSourceProp>> sourceProp
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (sourceProp);

        var sourceChain = sourceProp.Body.ToString();
        sourceChain = sourceChain.Substring
            (
                sourceChain.IndexOf (".", StringComparison.Ordinal) + 1
            );
        var binding = new Binding
            (
                "Text",
                source,
                sourceChain,
                false,
                DataSourceUpdateMode.OnPropertyChanged
            );

        control.DataBindings.Add (binding);

        return control;
    }

    /// <summary>
    /// Связывание свойства с источником данных.
    /// </summary>
    public static TControl Binding<TSource, TSourceProp, TControl, TTargetProp>
        (
            this TControl control,
            TSource source,
            string sourceProp,
            string targetProp = "Text",
            Func<TSourceProp, TTargetProp>? convert = null,
            Func<TTargetProp, TSourceProp>? convertBack = null
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNullNorEmpty (sourceProp);
        Sure.NotNullNorEmpty (targetProp);

        var b = new Binding
            (
                targetProp,
                source, sourceProp,
                false,
                DataSourceUpdateMode.OnPropertyChanged
            );
        if (convert is not null)
        {
            ConvertEventHandler doConvert = (_, args) => args.Value =
                convert (((TSourceProp) args.Value!));
            b.Format += doConvert;
        }

        if (convertBack is not null)
        {
            ConvertEventHandler doConvert = (_, args) => args.Value =
                convertBack (((TTargetProp) args.Value!));
            b.Parse += doConvert;
        }

        control.DataBindings.Add (b);

        return control;
    }

    #endregion
}
