// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable InconsistentlySynchronizedField
// ReSharper disable UnusedMember.Global

/* HtmlBuildDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Configuration that describes how to build HTML from a tree
/// </summary>
/// <typeparam name="TNode"></typeparam>
public class HtmlBuildDefinition<TNode>
    where TNode : TreeNode<TNode>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly IDictionary<string, string> EmptyAttributes = new Dictionary<string, string>();

    /// <summary>
    ///
    /// </summary>
    public HtmlBuildDefinition()
    {
        RootElementName = "ul";
        GetRootAttributes = _ => EmptyAttributes;
        GetRootPreHtml = _ => string.Empty;
        GetRootPostHtml = _ => string.Empty;

        ItemElementName = "li";
        GetItemAttributes = _ => EmptyAttributes;
        GetItemPreHtml = n => n.HierarchyId.ToString ("/");
        GetItemPostHtml = _ => string.Empty;

        ContainerElementName = "ul";
        GetContainerAttributes = _ => EmptyAttributes;
        GetContainerPreHtml = _ => string.Empty;
        GetContainerPostHtml = _ => string.Empty;
    }

    /// <summary>
    /// Name of HTML element to use for root. Default : "ul".
    /// </summary>
    public string RootElementName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Func<TNode, IDictionary<string, string>> GetRootAttributes { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Func<TNode, string> GetRootPreHtml { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Func<TNode, string> GetRootPostHtml { get; set; }


    /// <summary>
    /// Name of HTML element to use for individual item (node).  Default: "li".
    /// </summary>
    public string ItemElementName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Func<TNode, IDictionary<string, string>> GetItemAttributes { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Func<TNode, string> GetItemPreHtml { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Func<TNode, string> GetItemPostHtml { get; set; }


    /// <summary>
    /// Name of HTML element to use for nested item (node) container. Default: "ul".
    /// </summary>
    public string ContainerElementName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Func<TNode, IDictionary<string, string>> GetContainerAttributes { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Func<TNode, string> GetContainerPreHtml { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Func<TNode, string> GetContainerPostHtml { get; set; }
}
