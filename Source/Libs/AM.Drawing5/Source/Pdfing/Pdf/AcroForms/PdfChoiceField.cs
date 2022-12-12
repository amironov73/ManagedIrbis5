// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfChoiceField.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.AcroForms;

/// <summary>
/// Represents the base class for all choice field dictionaries.
/// </summary>
public abstract class PdfChoiceField
    : PdfAcroField
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfChoiceField"/> class.
    /// </summary>
    protected PdfChoiceField (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfChoiceField"/> class.
    /// </summary>
    protected PdfChoiceField (PdfDictionary dict)
        : base (dict)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Gets the index of the specified string in the /Opt array or -1, if no such string exists.
    /// </summary>
    protected int IndexInOptArray (string value)
    {
        var opt = Elements.GetArray (Keys.Opt);

#if DEBUG // Check with //R080317 implemention
        PdfArray? opt2 = null;
        if (Elements[Keys.Opt] is PdfArray)
        {
            opt2 = Elements[Keys.Opt] as PdfArray;
        }
        else if (Elements[Keys.Opt] is Advanced.PdfReference)
        {
            //falls das Array nicht direkt am Element h�ngt,
            //das Array aus dem referenzierten Element holen
            opt2 = ((Advanced.PdfReference)Elements[Keys.Opt]!).Value as PdfArray;
        }

        Debug.Assert (ReferenceEquals (opt, opt2));
#endif

        if (opt != null)
        {
            var count = opt.Elements.Count;
            for (var idx = 0; idx < count; idx++)
            {
                var item = opt.Elements[idx];
                if (item is PdfString)
                {
                    if (item.ToString() == value)
                    {
                        return idx;
                    }
                }
                else if (item is PdfArray array)
                {
                    if (array.Elements.Count != 0)
                    {
                        if (array.Elements[0].ToString() == value)
                        {
                            return idx;
                        }
                    }
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Gets the value from the index in the /Opt array.
    /// </summary>
    protected string ValueInOptArray (int index)
    {
        var opt = Elements.GetArray (Keys.Opt);
        if (opt != null)
        {
            var count = opt.Elements.Count;
            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException (nameof (index));
            }

            var item = opt.Elements[index];
            if (item is PdfString)
            {
                return item.ToString()!;
            }

            if (item is PdfArray)
            {
                var array = (PdfArray)item;
                if (array.Elements.Count != 0)
                {
                    return array.Elements[0].ToString()!;
                }
            }
        }

        return "";
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// The description comes from PDF 1.4 Reference.
    /// </summary>
    public new class Keys : PdfAcroField.Keys
    {
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// (Required; inheritable) An array of options to be presented to the user. Each element of
        /// the array is either a text string representing one of the available options or a two-element
        /// array consisting of a text string together with a default appearance string for constructing
        /// the item�s appearance dynamically at viewing time.
        /// </summary>
        [KeyInfo (KeyType.Array | KeyType.Optional)]
        public const string Opt = "/Opt";

        /// <summary>
        /// (Optional; inheritable) For scrollable list boxes, the top index (the index in the Opt array
        /// of the first option visible in the list).
        /// </summary>
        [KeyInfo (KeyType.Integer | KeyType.Optional)]
        public const string TI = "/TI";

        /// <summary>
        /// (Sometimes required, otherwise optional; inheritable; PDF 1.4) For choice fields that allow
        /// multiple selection (MultiSelect flag set), an array of integers, sorted in ascending order,
        /// representing the zero-based indices in the Opt array of the currently selected option
        /// items. This entry is required when two or more elements in the Opt array have different
        /// names but the same export value, or when the value of the choice field is an array; in
        /// other cases, it is permitted but not required. If the items identified by this entry differ
        /// from those in the V entry of the field dictionary (see below), the V entry takes precedence.
        /// </summary>
        [KeyInfo (KeyType.Array | KeyType.Optional)]
        public const string I = "/I";

        /// <summary>
        /// Gets the KeysMeta for these keys.
        /// </summary>
        internal static DictionaryMeta Meta => _meta ??= CreateMeta (typeof (Keys));

        static DictionaryMeta? _meta;

        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta
    {
        get { return Keys.Meta; }
    }
}
