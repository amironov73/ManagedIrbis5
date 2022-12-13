// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* StructuralSemanticsProperty.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public enum StructuralSemanticsProperty
{
    /// <summary>
    ///
    /// </summary>
    COVER = 1,

    /// <summary>
    ///
    /// </summary>
    FRONTMATTER,

    /// <summary>
    ///
    /// </summary>
    BODYMATTER,

    /// <summary>
    ///
    /// </summary>
    BACKMATTER,

    /// <summary>
    ///
    /// </summary>
    VOLUME,

    /// <summary>
    ///
    /// </summary>
    PART,

    /// <summary>
    ///
    /// </summary>
    CHAPTER,

    /// <summary>
    ///
    /// </summary>
    SUBCHAPTER,

    /// <summary>
    ///
    /// </summary>
    DIVISION,

    /// <summary>
    ///
    /// </summary>
    ABSTRACT,

    /// <summary>
    ///
    /// </summary>
    FOREWORD,

    /// <summary>
    ///
    /// </summary>
    PREFACE,

    /// <summary>
    ///
    /// </summary>
    PROLOGUE,

    /// <summary>
    ///
    /// </summary>
    INTRODUCTION,

    /// <summary>
    ///
    /// </summary>
    PREAMBLE,

    /// <summary>
    ///
    /// </summary>
    CONCLUSION,

    /// <summary>
    ///
    /// </summary>
    EPILOGUE,

    /// <summary>
    ///
    /// </summary>
    AFTERWORD,

    /// <summary>
    ///
    /// </summary>
    EPIGRAPH,

    /// <summary>
    ///
    /// </summary>
    TOC,

    /// <summary>
    ///
    /// </summary>
    TOC_BRIEF,

    /// <summary>
    ///
    /// </summary>
    LANDMARKS,

    /// <summary>
    ///
    /// </summary>
    LOA,

    /// <summary>
    ///
    /// </summary>
    LOI,

    /// <summary>
    ///
    /// </summary>
    LOT,

    /// <summary>
    ///
    /// </summary>
    LOV,

    /// <summary>
    ///
    /// </summary>
    APPENDIX,

    /// <summary>
    ///
    /// </summary>
    COLOPHON,

    /// <summary>
    ///
    /// </summary>
    CREDITS,

    /// <summary>
    ///
    /// </summary>
    KEYWORDS,

    /// <summary>
    ///
    /// </summary>
    INDEX,

    /// <summary>
    ///
    /// </summary>
    INDEX_HEADNOTES,

    /// <summary>
    ///
    /// </summary>
    INDEX_LEGEND,

    /// <summary>
    ///
    /// </summary>
    INDEX_GROUP,

    /// <summary>
    ///
    /// </summary>
    INDEX_ENTRY_LIST,

    /// <summary>
    ///
    /// </summary>
    INDEX_ENTRY,

    /// <summary>
    ///
    /// </summary>
    INDEX_TERM,

    /// <summary>
    ///
    /// </summary>
    INDEX_EDITOR_NOTE,

    /// <summary>
    ///
    /// </summary>
    INDEX_LOCATOR,

    /// <summary>
    ///
    /// </summary>
    INDEX_LOCATOR_LIST,

    /// <summary>
    ///
    /// </summary>
    INDEX_LOCATOR_RANGE,

    /// <summary>
    ///
    /// </summary>
    INDEX_XREF_PREFERRED,

    /// <summary>
    ///
    /// </summary>
    INDEX_XREF_RELATED,

    /// <summary>
    ///
    /// </summary>
    INDEX_TERM_CATEGORY,

    /// <summary>
    ///
    /// </summary>
    INDEX_TERM_CATEGORIES,

    /// <summary>
    ///
    /// </summary>
    GLOSSARY,

    /// <summary>
    ///
    /// </summary>
    GLOSSTERM,

    /// <summary>
    ///
    /// </summary>
    GLOSSDEF,

    /// <summary>
    ///
    /// </summary>
    BIBLIOGRAPHY,

    /// <summary>
    ///
    /// </summary>
    BIBLIOENTRY,

    /// <summary>
    ///
    /// </summary>
    TITLEPAGE,

    /// <summary>
    ///
    /// </summary>
    HALFTITLEPAGE,

    /// <summary>
    ///
    /// </summary>
    COPYRIGHT_PAGE,

    /// <summary>
    ///
    /// </summary>
    SERIESPAGE,

    /// <summary>
    ///
    /// </summary>
    ACKNOWLEDGMENTS,

    /// <summary>
    ///
    /// </summary>
    IMPRINT,

    /// <summary>
    ///
    /// </summary>
    IMPRIMATUR,

    /// <summary>
    ///
    /// </summary>
    CONTRIBUTORS,

    /// <summary>
    ///
    /// </summary>
    OTHER_CREDITS,

    /// <summary>
    ///
    /// </summary>
    ERRATA,

    /// <summary>
    ///
    /// </summary>
    DEDICATION,

    /// <summary>
    ///
    /// </summary>
    REVISION_HISTORY,

    /// <summary>
    ///
    /// </summary>
    CASE_STUDY,

    /// <summary>
    ///
    /// </summary>
    HELP,

    /// <summary>
    ///
    /// </summary>
    MARGINALIA,

    /// <summary>
    ///
    /// </summary>
    NOTICE,

    /// <summary>
    ///
    /// </summary>
    PULLQUOTE,

    /// <summary>
    ///
    /// </summary>
    SIDEBAR,

    /// <summary>
    ///
    /// </summary>
    TIP,

    /// <summary>
    ///
    /// </summary>
    WARNING,

    /// <summary>
    ///
    /// </summary>
    HALFTITLE,

    /// <summary>
    ///
    /// </summary>
    FULLTITLE,

    /// <summary>
    ///
    /// </summary>
    COVERTITLE,

    /// <summary>
    ///
    /// </summary>
    TITLE,

    /// <summary>
    ///
    /// </summary>
    SUBTITLE,

    /// <summary>
    ///
    /// </summary>
    LABEL,

    /// <summary>
    ///
    /// </summary>
    ORDINAL,

    /// <summary>
    ///
    /// </summary>
    BRIDGEHEAD,

    /// <summary>
    ///
    /// </summary>
    LEARNING_OBJECTIVE,

    /// <summary>
    ///
    /// </summary>
    LEARNING_OBJECTIVES,

    /// <summary>
    ///
    /// </summary>
    LEARNING_OUTCOME,

    /// <summary>
    ///
    /// </summary>
    LEARNING_OUTCOMES,

    /// <summary>
    ///
    /// </summary>
    LEARNING_RESOURCE,

    /// <summary>
    ///
    /// </summary>
    LEARNING_RESOURCES,

    /// <summary>
    ///
    /// </summary>
    LEARNING_STANDARD,

    /// <summary>
    ///
    /// </summary>
    LEARNING_STANDARDS,

    /// <summary>
    ///
    /// </summary>
    ANSWER,

    /// <summary>
    ///
    /// </summary>
    ANSWERS,

    /// <summary>
    ///
    /// </summary>
    ASSESSMENT,

    /// <summary>
    ///
    /// </summary>
    ASSESSMENTS,

    /// <summary>
    ///
    /// </summary>
    FEEDBACK,

    /// <summary>
    ///
    /// </summary>
    FILL_IN_THE_BLANK_PROBLEM,

    /// <summary>
    ///
    /// </summary>
    GENERAL_PROBLEM,

    /// <summary>
    ///
    /// </summary>
    QNA,

    /// <summary>
    ///
    /// </summary>
    MATCH_PROBLEM,

    /// <summary>
    ///
    /// </summary>
    MULTIPLE_CHOICE_PROBLEM,

    /// <summary>
    ///
    /// </summary>
    PRACTICE,

    /// <summary>
    ///
    /// </summary>
    QUESTION,

    /// <summary>
    ///
    /// </summary>
    PRACTICES,

    /// <summary>
    ///
    /// </summary>
    TRUE_FALSE_PROBLEM,

    /// <summary>
    ///
    /// </summary>
    PANEL,

    /// <summary>
    ///
    /// </summary>
    PANEL_GROUP,

    /// <summary>
    ///
    /// </summary>
    BALLOON,

    /// <summary>
    ///
    /// </summary>
    TEXT_AREA,

    /// <summary>
    ///
    /// </summary>
    SOUND_AREA,

    /// <summary>
    ///
    /// </summary>
    ANNOTATION,

    /// <summary>
    ///
    /// </summary>
    NOTE,

    /// <summary>
    ///
    /// </summary>
    FOOTNOTE,

    /// <summary>
    ///
    /// </summary>
    ENDNOTE,

    /// <summary>
    ///
    /// </summary>
    REARNOTE,

    /// <summary>
    ///
    /// </summary>
    FOOTNOTES,

    /// <summary>
    ///
    /// </summary>
    ENDNOTES,

    /// <summary>
    ///
    /// </summary>
    REARNOTES,

    /// <summary>
    ///
    /// </summary>
    ANNOREF,

    /// <summary>
    ///
    /// </summary>
    BIBLIOREF,

    /// <summary>
    ///
    /// </summary>
    GLOSSREF,

    /// <summary>
    ///
    /// </summary>
    NOTEREF,

    /// <summary>
    ///
    /// </summary>
    BACKLINK,

    /// <summary>
    ///
    /// </summary>
    CREDIT,

    /// <summary>
    ///
    /// </summary>
    KEYWORD,

    /// <summary>
    ///
    /// </summary>
    TOPIC_SENTENCE,

    /// <summary>
    ///
    /// </summary>
    CONCLUDING_SENTENCE,

    /// <summary>
    ///
    /// </summary>
    PAGEBREAK,

    /// <summary>
    ///
    /// </summary>
    PAGE_LIST,

    /// <summary>
    ///
    /// </summary>
    TABLE,

    /// <summary>
    ///
    /// </summary>
    TABLE_ROW,

    /// <summary>
    ///
    /// </summary>
    TABLE_CELL,

    /// <summary>
    ///
    /// </summary>
    LIST,

    /// <summary>
    ///
    /// </summary>
    LIST_ITEM,

    /// <summary>
    ///
    /// </summary>
    FIGURE,

    /// <summary>
    ///
    /// </summary>
    UNKNOWN
}

/// <summary>
///
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name",
    Justification = "Enum and parser need to be close to each other to avoid issues when the enum was changed without changing the parser. The file needs to be named after enum.")]
internal static class StructuralSemanticsPropertyParser
{
    public static StructuralSemanticsProperty Parse(string stringValue)
    {
        return stringValue.ToLowerInvariant() switch
        {
            "cover" => StructuralSemanticsProperty.COVER,
            "frontmatter" => StructuralSemanticsProperty.FRONTMATTER,
            "bodymatter" => StructuralSemanticsProperty.BODYMATTER,
            "backmatter" => StructuralSemanticsProperty.BACKMATTER,
            "volume" => StructuralSemanticsProperty.VOLUME,
            "part" => StructuralSemanticsProperty.PART,
            "chapter" => StructuralSemanticsProperty.CHAPTER,
            "subchapter" => StructuralSemanticsProperty.SUBCHAPTER,
            "division" => StructuralSemanticsProperty.DIVISION,
            "abstract" => StructuralSemanticsProperty.ABSTRACT,
            "foreword" => StructuralSemanticsProperty.FOREWORD,
            "preface" => StructuralSemanticsProperty.PREFACE,
            "prologue" => StructuralSemanticsProperty.PROLOGUE,
            "introduction" => StructuralSemanticsProperty.INTRODUCTION,
            "preamble" => StructuralSemanticsProperty.PREAMBLE,
            "conclusion" => StructuralSemanticsProperty.CONCLUSION,
            "epilogue" => StructuralSemanticsProperty.EPILOGUE,
            "afterword" => StructuralSemanticsProperty.AFTERWORD,
            "epigraph" => StructuralSemanticsProperty.EPIGRAPH,
            "toc" => StructuralSemanticsProperty.TOC,
            "toc-brief" => StructuralSemanticsProperty.TOC_BRIEF,
            "landmarks" => StructuralSemanticsProperty.LANDMARKS,
            "loa" => StructuralSemanticsProperty.LOA,
            "loi" => StructuralSemanticsProperty.LOI,
            "lot" => StructuralSemanticsProperty.LOT,
            "lov" => StructuralSemanticsProperty.LOV,
            "appendix" => StructuralSemanticsProperty.APPENDIX,
            "colophon" => StructuralSemanticsProperty.COLOPHON,
            "credits" => StructuralSemanticsProperty.CREDITS,
            "keywords" => StructuralSemanticsProperty.KEYWORDS,
            "index" => StructuralSemanticsProperty.INDEX,
            "index-headnotes" => StructuralSemanticsProperty.INDEX_HEADNOTES,
            "index-legend" => StructuralSemanticsProperty.INDEX_LEGEND,
            "index-group" => StructuralSemanticsProperty.INDEX_GROUP,
            "index-entry-list" => StructuralSemanticsProperty.INDEX_ENTRY_LIST,
            "index-entry" => StructuralSemanticsProperty.INDEX_ENTRY,
            "index-term" => StructuralSemanticsProperty.INDEX_TERM,
            "index-editor-note" => StructuralSemanticsProperty.INDEX_EDITOR_NOTE,
            "index-locator" => StructuralSemanticsProperty.INDEX_LOCATOR,
            "index-locator-list" => StructuralSemanticsProperty.INDEX_LOCATOR_LIST,
            "index-locator-range" => StructuralSemanticsProperty.INDEX_LOCATOR_RANGE,
            "index-xref-preferred" => StructuralSemanticsProperty.INDEX_XREF_PREFERRED,
            "index-xref-related" => StructuralSemanticsProperty.INDEX_XREF_RELATED,
            "index-term-category" => StructuralSemanticsProperty.INDEX_TERM_CATEGORY,
            "index-term-categories" => StructuralSemanticsProperty.INDEX_TERM_CATEGORIES,
            "glossary" => StructuralSemanticsProperty.GLOSSARY,
            "glossterm" => StructuralSemanticsProperty.GLOSSTERM,
            "glossdef" => StructuralSemanticsProperty.GLOSSDEF,
            "bibliography" => StructuralSemanticsProperty.BIBLIOGRAPHY,
            "biblioentry" => StructuralSemanticsProperty.BIBLIOENTRY,
            "titlepage" => StructuralSemanticsProperty.TITLEPAGE,
            "halftitlepage" => StructuralSemanticsProperty.HALFTITLEPAGE,
            "copyright-page" => StructuralSemanticsProperty.COPYRIGHT_PAGE,
            "seriespage" => StructuralSemanticsProperty.SERIESPAGE,
            "acknowledgments" => StructuralSemanticsProperty.ACKNOWLEDGMENTS,
            "imprint" => StructuralSemanticsProperty.IMPRINT,
            "imprimatur" => StructuralSemanticsProperty.IMPRIMATUR,
            "contributors" => StructuralSemanticsProperty.CONTRIBUTORS,
            "other-credits" => StructuralSemanticsProperty.OTHER_CREDITS,
            "errata" => StructuralSemanticsProperty.ERRATA,
            "dedication" => StructuralSemanticsProperty.DEDICATION,
            "revision-history" => StructuralSemanticsProperty.REVISION_HISTORY,
            "case-study" => StructuralSemanticsProperty.CASE_STUDY,
            "help" => StructuralSemanticsProperty.HELP,
            "marginalia" => StructuralSemanticsProperty.MARGINALIA,
            "notice" => StructuralSemanticsProperty.NOTICE,
            "pullquote" => StructuralSemanticsProperty.PULLQUOTE,
            "sidebar" => StructuralSemanticsProperty.SIDEBAR,
            "tip" => StructuralSemanticsProperty.TIP,
            "warning" => StructuralSemanticsProperty.WARNING,
            "halftitle" => StructuralSemanticsProperty.HALFTITLE,
            "fulltitle" => StructuralSemanticsProperty.FULLTITLE,
            "covertitle" => StructuralSemanticsProperty.COVERTITLE,
            "title" => StructuralSemanticsProperty.TITLE,
            "subtitle" => StructuralSemanticsProperty.SUBTITLE,
            "label" => StructuralSemanticsProperty.LABEL,
            "ordinal" => StructuralSemanticsProperty.ORDINAL,
            "bridgehead" => StructuralSemanticsProperty.BRIDGEHEAD,
            "learning-objective" => StructuralSemanticsProperty.LEARNING_OBJECTIVE,
            "learning-objectives" => StructuralSemanticsProperty.LEARNING_OBJECTIVES,
            "learning-outcome" => StructuralSemanticsProperty.LEARNING_OUTCOME,
            "learning-outcomes" => StructuralSemanticsProperty.LEARNING_OUTCOMES,
            "learning-resource" => StructuralSemanticsProperty.LEARNING_RESOURCE,
            "learning-resources" => StructuralSemanticsProperty.LEARNING_RESOURCES,
            "learning-standard" => StructuralSemanticsProperty.LEARNING_STANDARD,
            "learning-standards" => StructuralSemanticsProperty.LEARNING_STANDARDS,
            "answer" => StructuralSemanticsProperty.ANSWER,
            "answers" => StructuralSemanticsProperty.ANSWERS,
            "assessment" => StructuralSemanticsProperty.ASSESSMENT,
            "assessments" => StructuralSemanticsProperty.ASSESSMENTS,
            "feedback" => StructuralSemanticsProperty.FEEDBACK,
            "fill-in-the-blank-problem" => StructuralSemanticsProperty.FILL_IN_THE_BLANK_PROBLEM,
            "general-problem" => StructuralSemanticsProperty.GENERAL_PROBLEM,
            "qna" => StructuralSemanticsProperty.QNA,
            "match-problem" => StructuralSemanticsProperty.MATCH_PROBLEM,
            "multiple-choice-problem" => StructuralSemanticsProperty.MULTIPLE_CHOICE_PROBLEM,
            "practice" => StructuralSemanticsProperty.PRACTICE,
            "question" => StructuralSemanticsProperty.QUESTION,
            "practices" => StructuralSemanticsProperty.PRACTICES,
            "true-false-problem" => StructuralSemanticsProperty.TRUE_FALSE_PROBLEM,
            "panel" => StructuralSemanticsProperty.PANEL,
            "panel-group" => StructuralSemanticsProperty.PANEL_GROUP,
            "balloon" => StructuralSemanticsProperty.BALLOON,
            "text-area" => StructuralSemanticsProperty.TEXT_AREA,
            "sound-area" => StructuralSemanticsProperty.SOUND_AREA,
            "annotation" => StructuralSemanticsProperty.ANNOTATION,
            "note" => StructuralSemanticsProperty.NOTE,
            "footnote" => StructuralSemanticsProperty.FOOTNOTE,
            "endnote" => StructuralSemanticsProperty.ENDNOTE,
            "rearnote" => StructuralSemanticsProperty.REARNOTE,
            "footnotes" => StructuralSemanticsProperty.FOOTNOTES,
            "endnotes" => StructuralSemanticsProperty.ENDNOTES,
            "rearnotes" => StructuralSemanticsProperty.REARNOTES,
            "annoref" => StructuralSemanticsProperty.ANNOREF,
            "biblioref" => StructuralSemanticsProperty.BIBLIOREF,
            "glossref" => StructuralSemanticsProperty.GLOSSREF,
            "noteref" => StructuralSemanticsProperty.NOTEREF,
            "backlink" => StructuralSemanticsProperty.BACKLINK,
            "credit" => StructuralSemanticsProperty.CREDIT,
            "keyword" => StructuralSemanticsProperty.KEYWORD,
            "topic-sentence" => StructuralSemanticsProperty.TOPIC_SENTENCE,
            "concluding-sentence" => StructuralSemanticsProperty.CONCLUDING_SENTENCE,
            "pagebreak" => StructuralSemanticsProperty.PAGEBREAK,
            "page-list" => StructuralSemanticsProperty.PAGE_LIST,
            "table" => StructuralSemanticsProperty.TABLE,
            "table-row" => StructuralSemanticsProperty.TABLE_ROW,
            "table-cell" => StructuralSemanticsProperty.TABLE_CELL,
            "list" => StructuralSemanticsProperty.LIST,
            "list-item" => StructuralSemanticsProperty.LIST_ITEM,
            "figure" => StructuralSemanticsProperty.FIGURE,
            _ => StructuralSemanticsProperty.UNKNOWN
        };
    }
}
