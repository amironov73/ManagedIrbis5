// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EnglishNames.cs -- достаточно широко распространенные английские имена и фамилии
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.AOT;

/// <summary>
/// Достаточно широко распространенные английские имена и фамилии.
/// </summary>
public static class EnglishNames
{
    #region Public methods

    /// <summary>
    /// Список английских фамилий.
    /// </summary>
    public static string[] GetSurnames() => new[]
    {
        "Abbott", "Adam", "Adams", "Ahmed", "Akhtar", "Alexander",
        "Ali", "Allan", "Allen", "Anderson", "Andrews", "Appleton",
        "Armitage", "Armstrong", "Arnold", "Ashton", "Atherton",
        "Atkins", "Atkinson", "Austin", "Bailey", "Baird", "Baker",
        "Baldwin", "Ball", "Banks", "Barker", "Barlow", "Barnes",
        "Barnett", "Barrett", "Bartlett", "Barton", "Bates", "Baxter",
        "Beattie", "Begum", "Bell", "Bennett", "Benson", "Bentley",
        "Bernard", "Berry", "Best", "Bird", "Bishop", "Black",
        "Blair", "Blake", "Bloggs", "Bob", "Bolton", "Bond", "Booth",
        "Bourne", "Bowden", "Bowen", "Boyd", "Bradley", "Bradshaw",
        "Brady", "Bray", "Brennan", "Briggs", "Bright", "Broadhurst",
        "Brooks", "Brown", "Browne", "Bryant", "Buckley", "Bull",
        "Burgess", "Burke", "Burns", "Burrows", "Burton", "Bush",
        "Butler", "Byrne", "Cameron", "Campbell", "Carey", "Carpenter",
        "Carr", "Carroll", "Carter", "Cartwright", "Cassidy",
        "Chadwick", "Chambers", "Chapman", "Charles", "Charlton",
        "Christie", "Clark", "Clarke", "Coates", "Cole", "Coleman",
        "Coles", "Collins", "Connolly", "Cook", "Cooke", "Cooper",
        "Cox", "Craig", "Crawford", "Cross", "Crossley", "Cullen",
        "Cunningham", "Curtis", "Dale", "Daniel", "Daniels", "Davey",
        "David", "Davidson", "Davies", "Davis", "Davison", "Dawson",
        "Day", "Dean", "Dennis", "Devlin", "Dickson", "Dixon",
        "Doherty", "Donaldson", "Donnelly", "Douglas", "Doyle",
        "Drew", "Duffy", "Duncan", "Dunn", "Edwards", "Elliott",
        "Ellis", "English", "Evans", "Farrell", "Faulkner",
        "Ferguson", "Fernandez", "Field", "Finch", "Fisher",
        "Fitzgerald", "Fleming", "Fletcher", "Flynn", "Ford",
        "Forrest", "Foster", "Fowler", "Fox", "Francis", "Fraser",
        "Freeman", "French", "Frost", "Fuller", "Gallagher",
        "Garcia", "Gardiner", "Gardner", "Garner", "George",
        "Gibbons", "Gibbs", "Gibson", "Gilbert", "Giles", "Gill",
        "Glover", "Goddard", "Goodwin", "Gordon", "Gough", "Graham",
        "Grant", "Gray", "Green", "Greenwood", "Gregory", "Griffin",
        "Griffiths", "Haines", "Hall", "Hamilton", "Hammond",
        "Hancock", "Harding", "Hardy", "Harper", "Harris",
        "Harrison", "Hart", "Hartley", "Harvey", "Hawkins", "Hayes",
        "Haynes", "Heath", "Henderson", "Henry", "Herbert", "Hewitt",
        "Hicks", "Higgins", "Hill", "Hills", "Hilton", "Hobbs",
        "Hodgson", "Holden", "Holland", "Holmes", "Holt", "Hooper",
        "Hope", "Hopkins", "Horne", "Houghton", "Howard", "Howarth",
        "Howe", "Howell", "Howells", "Hudson", "Hughes", "Humphrey",
        "Humphreys", "Hunt", "Hunter", "Hussain", "Hutchinson",
        "Jackson", "Jacobs", "James", "Jarvis", "Jeffery", "Jenkins",
        "Jennings", "John", "Johns", "Johnson", "Johnston", "Jones",
        "Jordan", "Joseph", "Joyce", "Kaur", "Kavanagh", "Kay",
        "Kaye", "Kelly", "Kemp", "Kennedy", "Kenny", "Kent", "Kerr",
        "Khan", "King", "Kirby", "Kirk", "Knight", "Knowles",
        "Laing", "Lambert", "Lane", "Lawrence", "Lawson", "Leach",
        "Lee", "Leigh", "Lewis", "Lindsay", "Little", "Lloyd", "Long",
        "Lopez", "Lord", "Love", "Lovell", "Lowe", "Lucas", "Lynch",
        "Mac", "Macdonald", "Mackay", "Mackenzie", "Mann", "Marsh",
        "Marshall", "Martin", "Mason", "Matthews", "Maxwell", "May",
        "Mccann", "Mccarthy", "Mcdonald", "Mcfarlane", "Mcgrath",
        "Mcgregor", "Mcintyre", "Mckenna", "Mclean", "Mcleod",
        "Mellor", "Middleton", "Miles", "Millar", "Miller", "Mills",
        "Mitchell", "Mohamed", "Moore", "Moran", "Morgan", "Morley",
        "Morris", "Morrison", "Morton", "Moss", "Murphy", "Murray",
        "Nash", "Naylor", "Nelson", "Newman", "Newton", "Nicholls",
        "Nicholson", "Norman", "O brien", "O connor", "O neill",
        "Obrien", "Oliver", "Oneill", "Osborne", "Owen", "Owens",
        "Page", "Palmer", "Park", "Parker", "Parry", "Parsons",
        "Partridge", "Patel", "Paterson", "Patterson", "Paul",
        "Payne", "Pearce", "Pearson", "Perkins", "Perry", "Peters",
        "Phillips", "Piper", "Pollard", "Poole", "Porter", "Potter",
        "Powell", "Power", "Preston", "Price", "Pritchard", "Quinn",
        "Randall", "Rawlings", "Read", "Reed", "Rees", "Reid",
        "Reilly", "Reynolds", "Rhodes", "Rice", "Richards",
        "Richardson", "Riley", "Roberts", "Robertson", "Robinson",
        "Robson", "Rodgers", "Rogers", "Rooney", "Rose", "Ross",
        "Rossi", "Rowe", "Rowland", "Russell", "Ryan", "Sanders",
        "Sanderson", "Saunders", "Savage", "Sawyer", "Schofield",
        "Scott", "Seymour", "Shah", "Sharma", "Sharp", "Shaw",
        "Shepherd", "Sheppard", "Silva", "Simmons", "Simpson",
        "Sinclair", "Singh", "Singleton", "Slater", "Smart",
        "Smith", "Spence", "Spencer", "Stanton", "Steele", "Stephens",
        "Stephenson", "Stevens", "Stevenson", "Stewart", "Stone",
        "Stuart", "Sullivan", "Summers", "Sutherland", "Sutton",
        "Sweeney", "Swift", "Tait", "Taylor", "Thomas", "Thompson",
        "Thomson", "Thorne", "Thornton", "Todd", "Tucker", "Turnbull",
        "Turner", "Vaughan", "Vincent", "Walker", "Wall", "Wallace",
        "Walsh", "Walters", "Walton", "Ward", "Warren", "Watkins",
        "Watson", "Watts", "Weaver", "Webb", "Webster", "Welch",
        "Wells", "Welsh", "West", "Weston", "Wheeler", "Whelan",
        "White", "Whitehead", "Whitehouse", "Whittaker", "Whittle",
        "Wilkes", "Wilkins", "Wilkinson", "Williams", "Williamson",
        "Willis", "Wills", "Wilson", "Winter", "Wood", "Woods",
        "Woodward", "Wright", "Wyatt", "Yates", "Young"
    };

    #endregion
}
