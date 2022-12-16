// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;

using AM.HtmlTags.Conventions;
using AM.HtmlTags.Conventions.Elements;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

#endregion

#nullable enable

namespace AM.HtmlTags;

public static class ModelMetadataTagExtensions
{
    public static HtmlConventionRegistry ModelMetadata (this HtmlConventionRegistry registry)
    {
        registry.Labels.Modifier<DisplayNameElementModifier>();
        registry.Displays.Modifier<MetadataModelDisplayModifier>();
        registry.Editors.Modifier<MetadataModelEditModifier>();
        registry.Editors.Modifier<PlaceholderElementModifier>();
        registry.Editors.Modifier<ModelStateErrorsModifier>();
        registry.Editors.Modifier<ClientSideValidationModifier>();

        return registry;
    }

    private class DisplayNameElementModifier : IElementModifier
    {
        public bool Matches (ElementRequest token)
            => token.Get<ModelExplorer>()?.Metadata.DisplayName != null;

        public void Modify (ElementRequest request)
            => request.CurrentTag.Text (request.Get<ModelExplorer>().Metadata.DisplayName);
    }

    private static object BuildFormattedModelValue (ElementRequest request,
        Func<ModelMetadata, string> formatStringFinder)
    {
        var modelMetadata = request.Get<ModelExplorer>().Metadata;

        if (modelMetadata.ModelType == typeof (bool))
        {
            return "true";
        }

        var formattedModelValue = request.RawValue;

        if (request.RawValue == null)
        {
            formattedModelValue = modelMetadata.NullDisplayText;
        }

        var formatString = formatStringFinder (modelMetadata);

        if (formatString != null && formattedModelValue != null)
        {
            formattedModelValue = string.Format (CultureInfo.CurrentCulture, formatString, formattedModelValue);
        }

        return formattedModelValue;
    }

    private class MetadataModelDisplayModifier : IElementModifier
    {
        public bool Matches (ElementRequest token)
            => token.Get<ModelExplorer>() != null;

        public void Modify (ElementRequest request)
            => request.CurrentTag.Text (BuildFormattedModelValue (request, m => m.DisplayFormatString)?.ToString());
    }

    private class MetadataModelEditModifier : IElementModifier
    {
        public bool Matches (ElementRequest token)
            => token.Get<ModelExplorer>() != null;

        public void Modify (ElementRequest request)
            => request.CurrentTag.Value (BuildFormattedModelValue (request, m => m.EditFormatString)?.ToString());
    }

    private class PlaceholderElementModifier : IElementModifier
    {
        public bool Matches (ElementRequest token)
            => token.Get<ModelExplorer>()?.Metadata.Placeholder != null;

        public void Modify (ElementRequest request)
            => request.CurrentTag.Attr ("placeholder", request.Get<ModelExplorer>().Metadata.Placeholder);
    }

    private class ModelStateErrorsModifier : IElementModifier
    {
        public bool Matches (ElementRequest token)
            => token.TryGet (out ViewContext viewContext)
               && token.TryGet (out ElementName elementName)
               && viewContext.ViewData.ModelState.TryGetValue (elementName.Value, out var entry)
               && entry.Errors.Count > 0;

        public void Modify (ElementRequest request)
            => request.CurrentTag.AddClass (HtmlHelper.ValidationInputCssClassName);
    }

    private class ClientSideValidationModifier : IElementModifier
    {
        public bool Matches (ElementRequest token)
            => token.TryGet (out ViewContext viewContext)
               && viewContext.ClientValidationEnabled;

        public void Modify (ElementRequest request)
        {
            var validationProvider = request.Get<ValidationHtmlAttributeProvider>();
            var viewContext = request.Get<ViewContext>();
            var modelExplorer = request.Get<ModelExplorer>();
            var attributes = new Dictionary<string, string>();

            validationProvider.AddValidationAttributes (viewContext, modelExplorer, attributes);

            request.CurrentTag.MergeAttributes (attributes);
        }
    }
}
