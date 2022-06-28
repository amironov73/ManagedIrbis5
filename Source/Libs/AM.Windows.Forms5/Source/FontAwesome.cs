// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FontAwesome.cs -- простая обертка над Font Awesome
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Text;

#endregion

#nullable enable

namespace AM.Windows.Forms.Source;

/// <summary>
/// Простая обертка над Font Awesome.
/// Естественно, файл TTF-шрифта должен быть предоставлен пользователем.
/// </summary>
public static class FontAwesome
{
    /// <summary>
    /// Имя TTF-файла.
    /// </summary>
    public static string FontAwesomeTtf { get; set; }

    /// <summary>
    /// Собственно шрифт.
    /// </summary>
    public static Font Awesome { get; set; }

    /// <summary>
    /// Размер шрифта
    /// </summary>
    public static float Size { get; set; }

    /// <summary>
    /// Стиль.
    /// </summary>
    public static FontStyle Style { get; set; }

    static FontAwesome()
    {
        Awesome = null!;
        FontAwesomeTtf = "fontawesome-webfont.ttf";
        Style = FontStyle.Regular;
        Size = 20;
        Reload();
    }

    /// <summary>
    /// Пересоздание шрифта.
    /// </summary>
    public static void Reload()
    {
        var f = new PrivateFontCollection();
        f.AddFontFile (FontAwesomeTtf);
        Awesome = new Font (f.Families[0], Size, Style);
    }

    /// <summary>
    /// Пересоздание шрифта с новым размером.
    /// </summary>
    public static void Reload
        (
            float newSize
        )
    {
        Sure.Positive (newSize);

        Size = newSize;
        Reload();
    }

    /// <summary>
    /// Пересоздание шрифта с новым стилем.
    /// </summary>
    public static void Reload
        (
            FontStyle newStyle
        )
    {
        Style = newStyle;
        Reload();
    }

    private static string UnicodeToChar (string hex)
    {
        var code = int.Parse (hex, System.Globalization.NumberStyles.HexNumber);
        var unicodeString = char.ConvertFromUtf32 (code);

        return unicodeString;
    }

    /// <summary>
    /// Обращение к отдельным символам.
    /// </summary>
    public static class fa
    {
        // missing XML doc-comment
        #pragma warning disable CS1591

        public static string @lock => UnicodeToChar ("f023");
        public static string @try => UnicodeToChar ("f195");
        public static string _500px => UnicodeToChar ("f26e");
        public static string adjust => UnicodeToChar ("f042");
        public static string adn => UnicodeToChar ("f170");
        public static string align_center => UnicodeToChar ("f037");
        public static string align_justify => UnicodeToChar ("f039");
        public static string align_left => UnicodeToChar ("f036");
        public static string align_right => UnicodeToChar ("f038");
        public static string amazon => UnicodeToChar ("f270");
        public static string ambulance => UnicodeToChar ("f0f9");
        public static string anchor => UnicodeToChar ("f13d");
        public static string android => UnicodeToChar ("f17b");
        public static string angellist => UnicodeToChar ("f209");
        public static string angle_double_down => UnicodeToChar ("f103");
        public static string angle_double_left => UnicodeToChar ("f100");
        public static string angle_double_right => UnicodeToChar ("f101");
        public static string angle_double_up => UnicodeToChar ("f102");
        public static string angle_down => UnicodeToChar ("f107");
        public static string angle_left => UnicodeToChar ("f104");
        public static string angle_right => UnicodeToChar ("f105");
        public static string angle_up => UnicodeToChar ("f106");
        public static string apple => UnicodeToChar ("f179");
        public static string archive => UnicodeToChar ("f187");
        public static string area_chart => UnicodeToChar ("f1fe");
        public static string arrow_circle_down => UnicodeToChar ("f0ab");
        public static string arrow_circle_left => UnicodeToChar ("f0a8");
        public static string arrow_circle_o_down => UnicodeToChar ("f01a");
        public static string arrow_circle_o_left => UnicodeToChar ("f190");
        public static string arrow_circle_o_right => UnicodeToChar ("f18e");
        public static string arrow_circle_o_up => UnicodeToChar ("f01b");
        public static string arrow_circle_right => UnicodeToChar ("f0a9");
        public static string arrow_circle_up => UnicodeToChar ("f0aa");
        public static string arrow_down => UnicodeToChar ("f063");
        public static string arrow_left => UnicodeToChar ("f060");
        public static string arrow_right => UnicodeToChar ("f061");
        public static string arrow_up => UnicodeToChar ("f062");
        public static string arrows => UnicodeToChar ("f047");
        public static string arrows_alt => UnicodeToChar ("f0b2");
        public static string arrows_h => UnicodeToChar ("f07e");
        public static string arrows_v => UnicodeToChar ("f07d");
        public static string asterisk => UnicodeToChar ("f069");
        public static string at => UnicodeToChar ("f1fa");
        public static string automobile => UnicodeToChar ("f1b9");
        public static string backward => UnicodeToChar ("f04a");
        public static string balance_scale => UnicodeToChar ("f24e");
        public static string ban => UnicodeToChar ("f05e");
        public static string bank => UnicodeToChar ("f19c");
        public static string bar_chart => UnicodeToChar ("f080");
        public static string bar_chart_o => UnicodeToChar ("f080");
        public static string barcode => UnicodeToChar ("f02a");
        public static string bars => UnicodeToChar ("f0c9");
        public static string battery_0 => UnicodeToChar ("f244");
        public static string battery_1 => UnicodeToChar ("f243");
        public static string battery_2 => UnicodeToChar ("f242");
        public static string battery_3 => UnicodeToChar ("f241");
        public static string battery_4 => UnicodeToChar ("f240");
        public static string battery_empty => UnicodeToChar ("f244");
        public static string battery_full => UnicodeToChar ("f240");
        public static string battery_half => UnicodeToChar ("f242");
        public static string battery_quarter => UnicodeToChar ("f243");
        public static string battery_three_quarters => UnicodeToChar ("f241");
        public static string bed => UnicodeToChar ("f236");
        public static string beer => UnicodeToChar ("f0fc");
        public static string behance => UnicodeToChar ("f1b4");
        public static string behance_square => UnicodeToChar ("f1b5");
        public static string bell => UnicodeToChar ("f0f3");
        public static string bell_o => UnicodeToChar ("f0a2");
        public static string bell_slash => UnicodeToChar ("f1f6");
        public static string bell_slash_o => UnicodeToChar ("f1f7");
        public static string bicycle => UnicodeToChar ("f206");
        public static string binoculars => UnicodeToChar ("f1e5");
        public static string birthday_cake => UnicodeToChar ("f1fd");
        public static string bitbucket => UnicodeToChar ("f171");
        public static string bitbucket_square => UnicodeToChar ("f172");
        public static string bitcoin => UnicodeToChar ("f15a");
        public static string black_tie => UnicodeToChar ("f27e");
        public static string bold => UnicodeToChar ("f032");
        public static string bolt => UnicodeToChar ("f0e7");
        public static string bomb => UnicodeToChar ("f1e2");
        public static string book => UnicodeToChar ("f02d");
        public static string bookmark => UnicodeToChar ("f02e");
        public static string bookmark_o => UnicodeToChar ("f097");
        public static string briefcase => UnicodeToChar ("f0b1");
        public static string btc => UnicodeToChar ("f15a");
        public static string bug => UnicodeToChar ("f188");
        public static string building => UnicodeToChar ("f1ad");
        public static string building_o => UnicodeToChar ("f0f7");
        public static string bullhorn => UnicodeToChar ("f0a1");
        public static string bullseye => UnicodeToChar ("f140");
        public static string bus => UnicodeToChar ("f207");
        public static string buysellads => UnicodeToChar ("f20d");
        public static string cab => UnicodeToChar ("f1ba");
        public static string calculator => UnicodeToChar ("f1ec");
        public static string calendar => UnicodeToChar ("f073");
        public static string calendar_check_o => UnicodeToChar ("f274");
        public static string calendar_minus_o => UnicodeToChar ("f272");
        public static string calendar_o => UnicodeToChar ("f133");
        public static string calendar_plus_o => UnicodeToChar ("f271");
        public static string calendar_times_o => UnicodeToChar ("f273");
        public static string camera => UnicodeToChar ("f030");
        public static string camera_retro => UnicodeToChar ("f083");
        public static string car => UnicodeToChar ("f1b9");
        public static string caret_down => UnicodeToChar ("f0d7");
        public static string caret_left => UnicodeToChar ("f0d9");
        public static string caret_right => UnicodeToChar ("f0da");
        public static string caret_square_o_down => UnicodeToChar ("f150");
        public static string caret_square_o_left => UnicodeToChar ("f191");
        public static string caret_square_o_right => UnicodeToChar ("f152");
        public static string caret_square_o_up => UnicodeToChar ("f151");
        public static string caret_up => UnicodeToChar ("f0d8");
        public static string cart_arrow_down => UnicodeToChar ("f218");
        public static string cart_plus => UnicodeToChar ("f217");
        public static string cc => UnicodeToChar ("f20a");
        public static string cc_amex => UnicodeToChar ("f1f3");
        public static string cc_diners_club => UnicodeToChar ("f24c");
        public static string cc_discover => UnicodeToChar ("f1f2");
        public static string cc_jcb => UnicodeToChar ("f24b");
        public static string cc_mastercard => UnicodeToChar ("f1f1");
        public static string cc_paypal => UnicodeToChar ("f1f4");
        public static string cc_stripe => UnicodeToChar ("f1f5");
        public static string cc_visa => UnicodeToChar ("f1f0");
        public static string certificate => UnicodeToChar ("f0a3");
        public static string chain => UnicodeToChar ("f0c1");
        public static string chain_broken => UnicodeToChar ("f127");
        public static string check => UnicodeToChar ("f00c");
        public static string check_circle => UnicodeToChar ("f058");
        public static string check_circle_o => UnicodeToChar ("f05d");
        public static string check_square => UnicodeToChar ("f14a");
        public static string check_square_o => UnicodeToChar ("f046");
        public static string chevron_circle_down => UnicodeToChar ("f13a");
        public static string chevron_circle_left => UnicodeToChar ("f137");
        public static string chevron_circle_right => UnicodeToChar ("f138");
        public static string chevron_circle_up => UnicodeToChar ("f139");
        public static string chevron_down => UnicodeToChar ("f078");
        public static string chevron_left => UnicodeToChar ("f053");
        public static string chevron_right => UnicodeToChar ("f054");
        public static string chevron_up => UnicodeToChar ("f077");
        public static string child => UnicodeToChar ("f1ae");
        public static string chrome => UnicodeToChar ("f268");
        public static string circle => UnicodeToChar ("f111");
        public static string circle_o => UnicodeToChar ("f10c");
        public static string circle_o_notch => UnicodeToChar ("f1ce");
        public static string circle_thin => UnicodeToChar ("f1db");
        public static string clipboard => UnicodeToChar ("f0ea");
        public static string clock_o => UnicodeToChar ("f017");
        public static string clone => UnicodeToChar ("f24d");
        public static string close => UnicodeToChar ("f00d");
        public static string cloud => UnicodeToChar ("f0c2");
        public static string cloud_download => UnicodeToChar ("f0ed");
        public static string cloud_upload => UnicodeToChar ("f0ee");
        public static string cny => UnicodeToChar ("f157");
        public static string code => UnicodeToChar ("f121");
        public static string code_fork => UnicodeToChar ("f126");
        public static string codepen => UnicodeToChar ("f1cb");
        public static string coffee => UnicodeToChar ("f0f4");
        public static string cog => UnicodeToChar ("f013");
        public static string cogs => UnicodeToChar ("f085");
        public static string columns => UnicodeToChar ("f0db");
        public static string comment => UnicodeToChar ("f075");
        public static string comment_o => UnicodeToChar ("f0e5");
        public static string commenting => UnicodeToChar ("f27a");
        public static string commenting_o => UnicodeToChar ("f27b");
        public static string comments => UnicodeToChar ("f086");
        public static string comments_o => UnicodeToChar ("f0e6");
        public static string compass => UnicodeToChar ("f14e");
        public static string compress => UnicodeToChar ("f066");
        public static string connectdevelop => UnicodeToChar ("f20e");
        public static string contao => UnicodeToChar ("f26d");
        public static string copy => UnicodeToChar ("f0c5");
        public static string copyright => UnicodeToChar ("f1f9");
        public static string creative_commons => UnicodeToChar ("f25e");
        public static string credit_card => UnicodeToChar ("f09d");
        public static string crop => UnicodeToChar ("f125");
        public static string crosshairs => UnicodeToChar ("f05b");
        public static string css3 => UnicodeToChar ("f13c");
        public static string cube => UnicodeToChar ("f1b2");
        public static string cubes => UnicodeToChar ("f1b3");
        public static string cut => UnicodeToChar ("f0c4");
        public static string cutlery => UnicodeToChar ("f0f5");
        public static string dashboard => UnicodeToChar ("f0e4");
        public static string dashcube => UnicodeToChar ("f210");
        public static string database => UnicodeToChar ("f1c0");
        public static string dedent => UnicodeToChar ("f03b");
        public static string delicious => UnicodeToChar ("f1a5");
        public static string desktop => UnicodeToChar ("f108");
        public static string deviantart => UnicodeToChar ("f1bd");
        public static string diamond => UnicodeToChar ("f219");
        public static string digg => UnicodeToChar ("f1a6");
        public static string dollar => UnicodeToChar ("f155");
        public static string dot_circle_o => UnicodeToChar ("f192");
        public static string download => UnicodeToChar ("f019");
        public static string dribbble => UnicodeToChar ("f17d");
        public static string dropbox => UnicodeToChar ("f16b");
        public static string drupal => UnicodeToChar ("f1a9");
        public static string edit => UnicodeToChar ("f044");
        public static string eject => UnicodeToChar ("f052");
        public static string ellipsis_h => UnicodeToChar ("f141");
        public static string ellipsis_v => UnicodeToChar ("f142");
        public static string empire => UnicodeToChar ("f1d1");
        public static string envelope => UnicodeToChar ("f0e0");
        public static string envelope_o => UnicodeToChar ("f003");
        public static string envelope_square => UnicodeToChar ("f199");
        public static string eraser => UnicodeToChar ("f12d");
        public static string eur => UnicodeToChar ("f153");
        public static string euro => UnicodeToChar ("f153");
        public static string exchange => UnicodeToChar ("f0ec");
        public static string exclamation => UnicodeToChar ("f12a");
        public static string exclamation_circle => UnicodeToChar ("f06a");
        public static string exclamation_triangle => UnicodeToChar ("f071");
        public static string expand => UnicodeToChar ("f065");
        public static string expeditedssl => UnicodeToChar ("f23e");
        public static string external_link => UnicodeToChar ("f08e");
        public static string external_link_square => UnicodeToChar ("f14c");
        public static string eye => UnicodeToChar ("f06e");
        public static string eye_slash => UnicodeToChar ("f070");
        public static string eyedropper => UnicodeToChar ("f1fb");
        public static string facebook => UnicodeToChar ("f09a");
        public static string facebook_f => UnicodeToChar ("f09a");
        public static string facebook_official => UnicodeToChar ("f230");
        public static string facebook_square => UnicodeToChar ("f082");
        public static string fast_backward => UnicodeToChar ("f049");
        public static string fast_forward => UnicodeToChar ("f050");
        public static string fax => UnicodeToChar ("f1ac");
        public static string feed => UnicodeToChar ("f09e");
        public static string female => UnicodeToChar ("f182");
        public static string fighter_jet => UnicodeToChar ("f0fb");
        public static string file => UnicodeToChar ("f15b");
        public static string file_archive_o => UnicodeToChar ("f1c6");
        public static string file_audio_o => UnicodeToChar ("f1c7");
        public static string file_code_o => UnicodeToChar ("f1c9");
        public static string file_excel_o => UnicodeToChar ("f1c3");
        public static string file_image_o => UnicodeToChar ("f1c5");
        public static string file_movie_o => UnicodeToChar ("f1c8");
        public static string file_o => UnicodeToChar ("f016");
        public static string file_pdf_o => UnicodeToChar ("f1c1");
        public static string file_photo_o => UnicodeToChar ("f1c5");
        public static string file_picture_o => UnicodeToChar ("f1c5");
        public static string file_powerpoint_o => UnicodeToChar ("f1c4");
        public static string file_sound_o => UnicodeToChar ("f1c7");
        public static string file_text => UnicodeToChar ("f15c");
        public static string file_text_o => UnicodeToChar ("f0f6");
        public static string file_video_o => UnicodeToChar ("f1c8");
        public static string file_word_o => UnicodeToChar ("f1c2");
        public static string file_zip_o => UnicodeToChar ("f1c6");
        public static string files_o => UnicodeToChar ("f0c5");
        public static string film => UnicodeToChar ("f008");
        public static string filter => UnicodeToChar ("f0b0");
        public static string fire => UnicodeToChar ("f06d");
        public static string fire_extinguisher => UnicodeToChar ("f134");
        public static string firefox => UnicodeToChar ("f269");
        public static string flag => UnicodeToChar ("f024");
        public static string flag_checkered => UnicodeToChar ("f11e");
        public static string flag_o => UnicodeToChar ("f11d");
        public static string flash => UnicodeToChar ("f0e7");
        public static string flask => UnicodeToChar ("f0c3");
        public static string flickr => UnicodeToChar ("f16e");
        public static string floppy_o => UnicodeToChar ("f0c7");
        public static string folder => UnicodeToChar ("f07b");
        public static string folder_o => UnicodeToChar ("f114");
        public static string folder_open => UnicodeToChar ("f07c");
        public static string folder_open_o => UnicodeToChar ("f115");
        public static string font => UnicodeToChar ("f031");
        public static string fonticons => UnicodeToChar ("f280");
        public static string forumbee => UnicodeToChar ("f211");
        public static string forward => UnicodeToChar ("f04e");
        public static string foursquare => UnicodeToChar ("f180");
        public static string frown_o => UnicodeToChar ("f119");
        public static string futbol_o => UnicodeToChar ("f1e3");
        public static string gamepad => UnicodeToChar ("f11b");
        public static string gavel => UnicodeToChar ("f0e3");
        public static string gbp => UnicodeToChar ("f154");
        public static string ge => UnicodeToChar ("f1d1");
        public static string gear => UnicodeToChar ("f013");
        public static string gears => UnicodeToChar ("f085");
        public static string genderless => UnicodeToChar ("f22d");
        public static string get_pocket => UnicodeToChar ("f265");
        public static string gg => UnicodeToChar ("f260");
        public static string gg_circle => UnicodeToChar ("f261");
        public static string gift => UnicodeToChar ("f06b");
        public static string git => UnicodeToChar ("f1d3");
        public static string git_square => UnicodeToChar ("f1d2");
        public static string github => UnicodeToChar ("f09b");
        public static string github_alt => UnicodeToChar ("f113");
        public static string github_square => UnicodeToChar ("f092");
        public static string gittip => UnicodeToChar ("f184");
        public static string glass => UnicodeToChar ("f000");
        public static string globe => UnicodeToChar ("f0ac");
        public static string google => UnicodeToChar ("f1a0");
        public static string google_plus => UnicodeToChar ("f0d5");
        public static string google_plus_square => UnicodeToChar ("f0d4");
        public static string google_wallet => UnicodeToChar ("f1ee");
        public static string graduation_cap => UnicodeToChar ("f19d");
        public static string gratipay => UnicodeToChar ("f184");
        public static string group => UnicodeToChar ("f0c0");
        public static string h_square => UnicodeToChar ("f0fd");
        public static string hacker_news => UnicodeToChar ("f1d4");
        public static string hand_grab_o => UnicodeToChar ("f255");
        public static string hand_lizard_o => UnicodeToChar ("f258");
        public static string hand_o_down => UnicodeToChar ("f0a7");
        public static string hand_o_left => UnicodeToChar ("f0a5");
        public static string hand_o_right => UnicodeToChar ("f0a4");
        public static string hand_o_up => UnicodeToChar ("f0a6");
        public static string hand_paper_o => UnicodeToChar ("f256");
        public static string hand_peace_o => UnicodeToChar ("f25b");
        public static string hand_pointer_o => UnicodeToChar ("f25a");
        public static string hand_rock_o => UnicodeToChar ("f255");
        public static string hand_scissors_o => UnicodeToChar ("f257");
        public static string hand_spock_o => UnicodeToChar ("f259");
        public static string hand_stop_o => UnicodeToChar ("f256");
        public static string hdd_o => UnicodeToChar ("f0a0");
        public static string header => UnicodeToChar ("f1dc");
        public static string headphones => UnicodeToChar ("f025");
        public static string heart => UnicodeToChar ("f004");
        public static string heart_o => UnicodeToChar ("f08a");
        public static string heartbeat => UnicodeToChar ("f21e");
        public static string history => UnicodeToChar ("f1da");
        public static string home => UnicodeToChar ("f015");
        public static string hospital_o => UnicodeToChar ("f0f8");
        public static string hotel => UnicodeToChar ("f236");
        public static string hourglass => UnicodeToChar ("f254");
        public static string hourglass_1 => UnicodeToChar ("f251");
        public static string hourglass_2 => UnicodeToChar ("f252");
        public static string hourglass_3 => UnicodeToChar ("f253");
        public static string hourglass_end => UnicodeToChar ("f253");
        public static string hourglass_half => UnicodeToChar ("f252");
        public static string hourglass_o => UnicodeToChar ("f250");
        public static string hourglass_start => UnicodeToChar ("f251");
        public static string houzz => UnicodeToChar ("f27c");
        public static string html5 => UnicodeToChar ("f13b");
        public static string i_cursor => UnicodeToChar ("f246");
        public static string ils => UnicodeToChar ("f20b");
        public static string image => UnicodeToChar ("f03e");
        public static string inbox => UnicodeToChar ("f01c");
        public static string indent => UnicodeToChar ("f03c");
        public static string industry => UnicodeToChar ("f275");
        public static string info => UnicodeToChar ("f129");
        public static string info_circle => UnicodeToChar ("f05a");
        public static string inr => UnicodeToChar ("f156");
        public static string instagram => UnicodeToChar ("f16d");
        public static string institution => UnicodeToChar ("f19c");
        public static string internet_explorer => UnicodeToChar ("f26b");
        public static string intersex => UnicodeToChar ("f224");
        public static string ioxhost => UnicodeToChar ("f208");
        public static string italic => UnicodeToChar ("f033");
        public static string joomla => UnicodeToChar ("f1aa");
        public static string jpy => UnicodeToChar ("f157");
        public static string jsfiddle => UnicodeToChar ("f1cc");
        public static string key => UnicodeToChar ("f084");
        public static string keyboard_o => UnicodeToChar ("f11c");
        public static string krw => UnicodeToChar ("f159");
        public static string language => UnicodeToChar ("f1ab");
        public static string laptop => UnicodeToChar ("f109");
        public static string lastfm => UnicodeToChar ("f202");
        public static string lastfm_square => UnicodeToChar ("f203");
        public static string leaf => UnicodeToChar ("f06c");
        public static string leanpub => UnicodeToChar ("f212");
        public static string legal => UnicodeToChar ("f0e3");
        public static string lemon_o => UnicodeToChar ("f094");
        public static string level_down => UnicodeToChar ("f149");
        public static string level_up => UnicodeToChar ("f148");
        public static string life_bouy => UnicodeToChar ("f1cd");
        public static string life_buoy => UnicodeToChar ("f1cd");
        public static string life_ring => UnicodeToChar ("f1cd");
        public static string life_saver => UnicodeToChar ("f1cd");
        public static string lightbulb_o => UnicodeToChar ("f0eb");
        public static string line_chart => UnicodeToChar ("f201");
        public static string link => UnicodeToChar ("f0c1");
        public static string linkedin => UnicodeToChar ("f0e1");
        public static string linkedin_square => UnicodeToChar ("f08c");
        public static string linux => UnicodeToChar ("f17c");
        public static string list => UnicodeToChar ("f03a");
        public static string list_alt => UnicodeToChar ("f022");
        public static string list_ol => UnicodeToChar ("f0cb");
        public static string list_ul => UnicodeToChar ("f0ca");
        public static string location_arrow => UnicodeToChar ("f124");
        public static string long_arrow_down => UnicodeToChar ("f175");
        public static string long_arrow_left => UnicodeToChar ("f177");
        public static string long_arrow_right => UnicodeToChar ("f178");
        public static string long_arrow_up => UnicodeToChar ("f176");
        public static string magic => UnicodeToChar ("f0d0");
        public static string magnet => UnicodeToChar ("f076");
        public static string mail_forward => UnicodeToChar ("f064");
        public static string mail_reply => UnicodeToChar ("f112");
        public static string mail_reply_all => UnicodeToChar ("f122");
        public static string male => UnicodeToChar ("f183");
        public static string map => UnicodeToChar ("f279");
        public static string map_marker => UnicodeToChar ("f041");
        public static string map_o => UnicodeToChar ("f278");
        public static string map_pin => UnicodeToChar ("f276");
        public static string map_signs => UnicodeToChar ("f277");
        public static string mars => UnicodeToChar ("f222");
        public static string mars_double => UnicodeToChar ("f227");
        public static string mars_stroke => UnicodeToChar ("f229");
        public static string mars_stroke_h => UnicodeToChar ("f22b");
        public static string mars_stroke_v => UnicodeToChar ("f22a");
        public static string maxcdn => UnicodeToChar ("f136");
        public static string meanpath => UnicodeToChar ("f20c");
        public static string medium => UnicodeToChar ("f23a");
        public static string medkit => UnicodeToChar ("f0fa");
        public static string meh_o => UnicodeToChar ("f11a");
        public static string mercury => UnicodeToChar ("f223");
        public static string microphone => UnicodeToChar ("f130");
        public static string microphone_slash => UnicodeToChar ("f131");
        public static string minus => UnicodeToChar ("f068");
        public static string minus_circle => UnicodeToChar ("f056");
        public static string minus_square => UnicodeToChar ("f146");
        public static string minus_square_o => UnicodeToChar ("f147");
        public static string mobile => UnicodeToChar ("f10b");
        public static string mobile_phone => UnicodeToChar ("f10b");
        public static string money => UnicodeToChar ("f0d6");
        public static string moon_o => UnicodeToChar ("f186");
        public static string mortar_board => UnicodeToChar ("f19d");
        public static string motorcycle => UnicodeToChar ("f21c");
        public static string mouse_pointer => UnicodeToChar ("f245");
        public static string music => UnicodeToChar ("f001");
        public static string navicon => UnicodeToChar ("f0c9");
        public static string neuter => UnicodeToChar ("f22c");
        public static string newspaper_o => UnicodeToChar ("f1ea");
        public static string object_group => UnicodeToChar ("f247");
        public static string object_ungroup => UnicodeToChar ("f248");
        public static string odnoklassniki => UnicodeToChar ("f263");
        public static string odnoklassniki_square => UnicodeToChar ("f264");
        public static string opencart => UnicodeToChar ("f23d");
        public static string openid => UnicodeToChar ("f19b");
        public static string opera => UnicodeToChar ("f26a");
        public static string optin_monster => UnicodeToChar ("f23c");
        public static string outdent => UnicodeToChar ("f03b");
        public static string pagelines => UnicodeToChar ("f18c");
        public static string paint_brush => UnicodeToChar ("f1fc");
        public static string paper_plane => UnicodeToChar ("f1d8");
        public static string paper_plane_o => UnicodeToChar ("f1d9");
        public static string paperclip => UnicodeToChar ("f0c6");
        public static string paragraph => UnicodeToChar ("f1dd");
        public static string paste => UnicodeToChar ("f0ea");
        public static string pause => UnicodeToChar ("f04c");
        public static string paw => UnicodeToChar ("f1b0");
        public static string paypal => UnicodeToChar ("f1ed");
        public static string pencil => UnicodeToChar ("f040");
        public static string pencil_square => UnicodeToChar ("f14b");
        public static string pencil_square_o => UnicodeToChar ("f044");
        public static string phone => UnicodeToChar ("f095");
        public static string phone_square => UnicodeToChar ("f098");
        public static string photo => UnicodeToChar ("f03e");
        public static string picture_o => UnicodeToChar ("f03e");
        public static string pie_chart => UnicodeToChar ("f200");
        public static string pied_piper => UnicodeToChar ("f1a7");
        public static string pied_piper_alt => UnicodeToChar ("f1a8");
        public static string pinterest => UnicodeToChar ("f0d2");
        public static string pinterest_p => UnicodeToChar ("f231");
        public static string pinterest_square => UnicodeToChar ("f0d3");
        public static string plane => UnicodeToChar ("f072");
        public static string play => UnicodeToChar ("f04b");
        public static string play_circle => UnicodeToChar ("f144");
        public static string play_circle_o => UnicodeToChar ("f01d");
        public static string plug => UnicodeToChar ("f1e6");
        public static string plus => UnicodeToChar ("f067");
        public static string plus_circle => UnicodeToChar ("f055");
        public static string plus_square => UnicodeToChar ("f0fe");
        public static string plus_square_o => UnicodeToChar ("f196");
        public static string power_off => UnicodeToChar ("f011");
        public static string print => UnicodeToChar ("f02f");
        public static string puzzle_piece => UnicodeToChar ("f12e");
        public static string qq => UnicodeToChar ("f1d6");
        public static string qrcode => UnicodeToChar ("f029");
        public static string question => UnicodeToChar ("f128");
        public static string question_circle => UnicodeToChar ("f059");
        public static string quote_left => UnicodeToChar ("f10d");
        public static string quote_right => UnicodeToChar ("f10e");
        public static string ra => UnicodeToChar ("f1d0");
        public static string random => UnicodeToChar ("f074");
        public static string rebel => UnicodeToChar ("f1d0");
        public static string recycle => UnicodeToChar ("f1b8");
        public static string reddit => UnicodeToChar ("f1a1");
        public static string reddit_square => UnicodeToChar ("f1a2");
        public static string refresh => UnicodeToChar ("f021");
        public static string registered => UnicodeToChar ("f25d");
        public static string remove => UnicodeToChar ("f00d");
        public static string renren => UnicodeToChar ("f18b");
        public static string reorder => UnicodeToChar ("f0c9");
        public static string repeat => UnicodeToChar ("f01e");
        public static string reply => UnicodeToChar ("f112");
        public static string reply_all => UnicodeToChar ("f122");
        public static string retweet => UnicodeToChar ("f079");
        public static string rmb => UnicodeToChar ("f157");
        public static string road => UnicodeToChar ("f018");
        public static string rocket => UnicodeToChar ("f135");
        public static string rotate_left => UnicodeToChar ("f0e2");
        public static string rotate_right => UnicodeToChar ("f01e");
        public static string rouble => UnicodeToChar ("f158");
        public static string rss => UnicodeToChar ("f09e");
        public static string rss_square => UnicodeToChar ("f143");
        public static string rub => UnicodeToChar ("f158");
        public static string ruble => UnicodeToChar ("f158");
        public static string rupee => UnicodeToChar ("f156");
        public static string safari => UnicodeToChar ("f267");
        public static string save => UnicodeToChar ("f0c7");
        public static string scissors => UnicodeToChar ("f0c4");
        public static string search => UnicodeToChar ("f002");
        public static string search_minus => UnicodeToChar ("f010");
        public static string search_plus => UnicodeToChar ("f00e");
        public static string sellsy => UnicodeToChar ("f213");
        public static string send => UnicodeToChar ("f1d8");
        public static string send_o => UnicodeToChar ("f1d9");
        public static string server => UnicodeToChar ("f233");
        public static string share => UnicodeToChar ("f064");
        public static string share_alt => UnicodeToChar ("f1e0");
        public static string share_alt_square => UnicodeToChar ("f1e1");
        public static string share_square => UnicodeToChar ("f14d");
        public static string share_square_o => UnicodeToChar ("f045");
        public static string shekel => UnicodeToChar ("f20b");
        public static string sheqel => UnicodeToChar ("f20b");
        public static string shield => UnicodeToChar ("f132");
        public static string ship => UnicodeToChar ("f21a");
        public static string shirtsinbulk => UnicodeToChar ("f214");
        public static string shopping_cart => UnicodeToChar ("f07a");
        public static string sign_in => UnicodeToChar ("f090");
        public static string sign_out => UnicodeToChar ("f08b");
        public static string signal => UnicodeToChar ("f012");
        public static string simplybuilt => UnicodeToChar ("f215");
        public static string sitemap => UnicodeToChar ("f0e8");
        public static string skyatlas => UnicodeToChar ("f216");
        public static string skype => UnicodeToChar ("f17e");
        public static string slack => UnicodeToChar ("f198");
        public static string sliders => UnicodeToChar ("f1de");
        public static string slideshare => UnicodeToChar ("f1e7");
        public static string smile_o => UnicodeToChar ("f118");
        public static string soccer_ball_o => UnicodeToChar ("f1e3");
        public static string sort => UnicodeToChar ("f0dc");
        public static string sort_alpha_asc => UnicodeToChar ("f15d");
        public static string sort_alpha_desc => UnicodeToChar ("f15e");
        public static string sort_amount_asc => UnicodeToChar ("f160");
        public static string sort_amount_desc => UnicodeToChar ("f161");
        public static string sort_asc => UnicodeToChar ("f0de");
        public static string sort_desc => UnicodeToChar ("f0dd");
        public static string sort_down => UnicodeToChar ("f0dd");
        public static string sort_numeric_asc => UnicodeToChar ("f162");
        public static string sort_numeric_desc => UnicodeToChar ("f163");
        public static string sort_up => UnicodeToChar ("f0de");
        public static string soundcloud => UnicodeToChar ("f1be");
        public static string space_shuttle => UnicodeToChar ("f197");
        public static string spinner => UnicodeToChar ("f110");
        public static string spoon => UnicodeToChar ("f1b1");
        public static string spotify => UnicodeToChar ("f1bc");
        public static string square => UnicodeToChar ("f0c8");
        public static string square_o => UnicodeToChar ("f096");
        public static string stack_exchange => UnicodeToChar ("f18d");
        public static string stack_overflow => UnicodeToChar ("f16c");
        public static string star => UnicodeToChar ("f005");
        public static string star_half => UnicodeToChar ("f089");
        public static string star_half_empty => UnicodeToChar ("f123");
        public static string star_half_full => UnicodeToChar ("f123");
        public static string star_half_o => UnicodeToChar ("f123");
        public static string star_o => UnicodeToChar ("f006");
        public static string steam => UnicodeToChar ("f1b6");
        public static string steam_square => UnicodeToChar ("f1b7");
        public static string step_backward => UnicodeToChar ("f048");
        public static string step_forward => UnicodeToChar ("f051");
        public static string stethoscope => UnicodeToChar ("f0f1");
        public static string sticky_note => UnicodeToChar ("f249");
        public static string sticky_note_o => UnicodeToChar ("f24a");
        public static string stop => UnicodeToChar ("f04d");
        public static string street_view => UnicodeToChar ("f21d");
        public static string strikethrough => UnicodeToChar ("f0cc");
        public static string stumbleupon => UnicodeToChar ("f1a4");
        public static string stumbleupon_circle => UnicodeToChar ("f1a3");
        public static string subscript => UnicodeToChar ("f12c");
        public static string subway => UnicodeToChar ("f239");
        public static string suitcase => UnicodeToChar ("f0f2");
        public static string sun_o => UnicodeToChar ("f185");
        public static string superscript => UnicodeToChar ("f12b");
        public static string support => UnicodeToChar ("f1cd");
        public static string table => UnicodeToChar ("f0ce");
        public static string tablet => UnicodeToChar ("f10a");
        public static string tachometer => UnicodeToChar ("f0e4");
        public static string tag => UnicodeToChar ("f02b");
        public static string tags => UnicodeToChar ("f02c");
        public static string tasks => UnicodeToChar ("f0ae");
        public static string taxi => UnicodeToChar ("f1ba");
        public static string television => UnicodeToChar ("f26c");
        public static string tencent_weibo => UnicodeToChar ("f1d5");
        public static string terminal => UnicodeToChar ("f120");
        public static string text_height => UnicodeToChar ("f034");
        public static string text_width => UnicodeToChar ("f035");
        public static string th => UnicodeToChar ("f00a");
        public static string th_large => UnicodeToChar ("f009");
        public static string th_list => UnicodeToChar ("f00b");
        public static string thumb_tack => UnicodeToChar ("f08d");
        public static string thumbs_down => UnicodeToChar ("f165");
        public static string thumbs_o_down => UnicodeToChar ("f088");
        public static string thumbs_o_up => UnicodeToChar ("f087");
        public static string thumbs_up => UnicodeToChar ("f164");
        public static string ticket => UnicodeToChar ("f145");
        public static string times => UnicodeToChar ("f00d");
        public static string times_circle => UnicodeToChar ("f057");
        public static string times_circle_o => UnicodeToChar ("f05c");
        public static string tint => UnicodeToChar ("f043");
        public static string toggle_down => UnicodeToChar ("f150");
        public static string toggle_left => UnicodeToChar ("f191");
        public static string toggle_off => UnicodeToChar ("f204");
        public static string toggle_on => UnicodeToChar ("f205");
        public static string toggle_right => UnicodeToChar ("f152");
        public static string toggle_up => UnicodeToChar ("f151");
        public static string trademark => UnicodeToChar ("f25c");
        public static string train => UnicodeToChar ("f238");
        public static string transgender => UnicodeToChar ("f224");
        public static string transgender_alt => UnicodeToChar ("f225");
        public static string trash => UnicodeToChar ("f1f8");
        public static string trash_o => UnicodeToChar ("f014");
        public static string tree => UnicodeToChar ("f1bb");
        public static string trello => UnicodeToChar ("f181");
        public static string tripadvisor => UnicodeToChar ("f262");
        public static string trophy => UnicodeToChar ("f091");
        public static string truck => UnicodeToChar ("f0d1");
        public static string tty => UnicodeToChar ("f1e4");
        public static string tumblr => UnicodeToChar ("f173");
        public static string tumblr_square => UnicodeToChar ("f174");
        public static string turkish_lira => UnicodeToChar ("f195");
        public static string tv => UnicodeToChar ("f26c");
        public static string twitch => UnicodeToChar ("f1e8");
        public static string twitter => UnicodeToChar ("f099");
        public static string twitter_square => UnicodeToChar ("f081");
        public static string umbrella => UnicodeToChar ("f0e9");
        public static string underline => UnicodeToChar ("f0cd");
        public static string undo => UnicodeToChar ("f0e2");
        public static string university => UnicodeToChar ("f19c");
        public static string unlink => UnicodeToChar ("f127");
        public static string unlock => UnicodeToChar ("f09c");
        public static string unlock_alt => UnicodeToChar ("f13e");
        public static string unsorted => UnicodeToChar ("f0dc");
        public static string upload => UnicodeToChar ("f093");
        public static string usd => UnicodeToChar ("f155");
        public static string user => UnicodeToChar ("f007");
        public static string user_md => UnicodeToChar ("f0f0");
        public static string user_plus => UnicodeToChar ("f234");
        public static string user_secret => UnicodeToChar ("f21b");
        public static string user_times => UnicodeToChar ("f235");
        public static string users => UnicodeToChar ("f0c0");
        public static string venus => UnicodeToChar ("f221");
        public static string venus_double => UnicodeToChar ("f226");
        public static string venus_mars => UnicodeToChar ("f228");
        public static string viacoin => UnicodeToChar ("f237");
        public static string video_camera => UnicodeToChar ("f03d");
        public static string vimeo => UnicodeToChar ("f27d");
        public static string vimeo_square => UnicodeToChar ("f194");
        public static string vine => UnicodeToChar ("f1ca");
        public static string vk => UnicodeToChar ("f189");
        public static string volume_down => UnicodeToChar ("f027");
        public static string volume_off => UnicodeToChar ("f026");
        public static string volume_up => UnicodeToChar ("f028");
        public static string warning => UnicodeToChar ("f071");
        public static string wechat => UnicodeToChar ("f1d7");
        public static string weibo => UnicodeToChar ("f18a");
        public static string weixin => UnicodeToChar ("f1d7");
        public static string whatsapp => UnicodeToChar ("f232");
        public static string wheelchair => UnicodeToChar ("f193");
        public static string wifi => UnicodeToChar ("f1eb");
        public static string wikipedia_w => UnicodeToChar ("f266");
        public static string windows => UnicodeToChar ("f17a");
        public static string won => UnicodeToChar ("f159");
        public static string wordpress => UnicodeToChar ("f19a");
        public static string wrench => UnicodeToChar ("f0ad");
        public static string xing => UnicodeToChar ("f168");
        public static string xing_square => UnicodeToChar ("f169");
        public static string y_combinator => UnicodeToChar ("f23b");
        public static string y_combinator_square => UnicodeToChar ("f1d4");
        public static string yahoo => UnicodeToChar ("f19e");
        public static string yc => UnicodeToChar ("f23b");
        public static string yc_square => UnicodeToChar ("f1d4");
        public static string yelp => UnicodeToChar ("f1e9");
        public static string yen => UnicodeToChar ("f157");
        public static string youtube => UnicodeToChar ("f167");
        public static string youtube_play => UnicodeToChar ("f16a");
        public static string youtube_square => UnicodeToChar ("f166");
    }
}
