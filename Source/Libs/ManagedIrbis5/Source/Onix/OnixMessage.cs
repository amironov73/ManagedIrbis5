// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* OnixMessage.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    //
    // https://en.wikipedia.org/wiki/ONIX_for_Books
    //
    // ONIX for Books is an XML format for sharing bibliographic data
    // pertaining to both traditional books and eBooks. It is the oldest
    // of the three ONIX standards, and is widely implemented in the book
    // trade in North America, Europe and increasingly in the Asia-Pacific
    // region. It allows book and ebook publishers to create and manage
    // a corpus of rich metadata about their products, and to exchange
    // it with their customers (distributors and retailers) in a coherent,
    // unambiguous, and largely automated manner.
    //
    // The ONIX for Books standard provides a free-to-use format for passing
    // descriptive metadata about books between publishers, data aggregators,
    // book retailers and other interested parties in the publishing industry.
    // Metadata concerning one or more book titles can be stored in a suitably
    // formatted XML file known as an 'ONIX message' ready for dissemination.
    // Whereas other data standards exist for storing the contents of
    // a book - the text, layout and graphics - the ONIX for Books standard
    // holds information about the book, similar to, but more extensive than,
    // the information one would typically find on the cover or title page of
    // a printed book or in a library catalog. The ONIX for Books standard
    // provides a way to communicate information about a book's author,
    // publisher, price, publication date, physical dimensions, synopsis
    // and many other details besides. The standard is quite extensive and most
    // publishers currently provide only a few dozen of the many hundreds
    // of pieces of information that the standard is designed to carry.
    //
    // ONIX for Books Release 1.0 was published in 2000. Revisions were made
    // in releases 1.1, 1.2 and 1.2.1.
    //
    // Release 2.0 was issued in 2001. A backwards-compatible version,
    // Release 2.1, arrived in June 2003. Three minor revisions intended for
    // general use have been made since then, the most recent in January 2006.
    // A further revision intended solely for use in Japan was issued in 2010.
    //
    // Release 3.0 was published in April 2009 with some corrections in 2010,
    // and the first minor revision (labelled 3.0.1) was issued in January 2012.
    // A second minor revision (3.0.2) was published in January 2014 and a third
    // in April 2016. The latest version is 3.0.7, released in October 2019,
    // and the standard continues to evolve to meet new business requirements
    // as they emerge. This 3.0 release has not yet completely replaced 2.1,
    // though implementation of 3.0 is widespread and continuing to grow.
    // There is also an Acknowledgement message format (published 2015) which
    // recipients of ONIX data files may send to confirm receipt of ONIX messages.
    //
    // The authors have stated that any new revisions will be based on, and
    // backwards-compatible with, Release 3.0.[1] The international steering
    // committee announced in January 2012 that support for version 2.1 would
    // be reduced at the end of December 2014.
    //
    // Releases 2.1 and 3.0 share a set of 'Codelists' or controlled vocabularies,
    // that are extended regularly to allow new types of information to be carried
    // without having to revise the main specifications. From Issue 37 of the
    // controlled vocabularies, additions are applicable only to ONIX 3.0,
    // and ONIX 2.1 is limited to Issue 36 or earlier.
    //
    // The ONIX for Books standard can be used to communicate a great deal more
    // information than most publishers currently choose to provide. There are
    // a number of reasons for this. Firstly, the standard is designed for use
    // with many different types of book and no single publication is expected
    // to use all of them. The standard also provides for the inclusion of sales
    // and pricing information which a publisher may not wish to freely distribute
    // outside their organization. And while the ONIX for Books standard has been
    // around since 2000, many publishers are still getting to grips with producing
    // ONIX messages; the task is made easier if the amount of information provided
    // for each title is kept to a minimum. However, studies have found that
    // a richer selection of metadata is associated with enhanced sales
    // of books and eBooks.
    //

    //
    // ГОСТ Р 7.0.92-2015
    //
    // Формат электронного обмена данными в книжном деле ONIX XML
    //
    // Дата введения: "1" июля 2016 г.
    //
    // Настоящий стандарт определяет основные поля и подполя коммуникативного
    // формата ONIX (версия 3.0, 2014 г.) для обмена информацией о книгах
    // в российском книжном бизнесе.
    //
    // Стандарт предназначен для издателей, книготорговых организаций, библиотек,
    // центров государственной библиографии и органов научно-технической информации.
    //
    // Запись в формате ONIX соответствует требованиям к документам формата XML
    // и состоит из пяти частей:
    //
    // - начало сообщения;
    // - заголовок сообщения;
    // - описание издания;
    // - сведения о поставщиках, другая коммерческая информация;
    // - конец сообщения.
    //
    // Начало сообщения состоит из двух строк: указание версии XML и указание версии ONIX:
    //
    // <?xml version="1.0"?>
    // <ONIXMessage release="3.0">
    //
    // Конец сообщения обозначают строкой
    //
    // </ONIXmessage>
    //

    /// <summary>
    ///
    /// </summary>
    public sealed class OnixMessage
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public Header Header { get; } = new ();

        /// <summary>
        ///
        /// </summary>
        public List<Product> Products { get; } = new ();

        #endregion
    }
}
