// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

using RestfulIrbis.OsmiCards;

var baseUrl = "https://api.osmicards.com/v2";
var apiId = "не помню";
var apiKey = "не знаю";

var client = new OsmiCardsClient
    (
        baseUrl,
        apiId,
        apiKey
    );

// var ping = client.Ping();
// Console.WriteLine (ping);
// Console.WriteLine();

// var templates = client.GetTemplateList();
// foreach (var template in templates)
// {
//     Console.WriteLine (template);
// }

// var chb = client.GetTemplateInfo ("chb");
// Console.WriteLine (chb);

// var passes = client.GetCardList();
// Console.WriteLine (passes.Length);
// foreach (var pass in passes)
// {
//     Console.WriteLine (pass);
// }

var cardData = File.ReadAllLines ("esia-to-es.txt")
    .Select (ChangeData.Parse)
    .ToArray();

var config = DicardsConfiguration.LoadConfiguration (OsmiUtility.DicardsJson());
foreach (var data in cardData)
{
    var number = data.From;
    var card = client.GetRawCard (number);
    if (card is null)
    {
        Console.WriteLine ($"Not found: {data}");
    }
    else
    {
        OsmiUtility.UpdateCardBarcode (card, data.To, config);
        client.UpdateCard (number, card.ToString(), false);
        Console.WriteLine ($"Success: {data}");
    }

    Thread.Sleep (100);
}

Console.WriteLine ("That's all!");

internal record ChangeData (string From, string To, string Name)
{
    public static ChangeData Parse (string line)
    {
        var parts = line.Split ('\t');
        return new ChangeData (parts[0], parts[1], parts[2]);
    }
};
