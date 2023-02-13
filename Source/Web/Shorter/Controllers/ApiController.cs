using AM.Web.Shortening;

using Microsoft.AspNetCore.Mvc;

namespace Shorter.Controllers;

[Route ("api")]
[ApiController]
public class ApiController
    : ControllerBase
{
    public ApiController
        (
            ILinkShortener shortener,
            ILogger<ApiController> logger
        )
    {
        _shortener = shortener;
        _logger = logger;
    }

    private readonly ILinkShortener _shortener;
    private readonly ILogger _logger;

    public ActionResult Index()
    {
        return Ok ("Nothing to see here");
    }
    
    [HttpPost]
    [Route ("add")]
    public ActionResult Add
        (
            [FromBody] LinkData? linkData
        )
    {
        _logger.LogInformation ("Add {LinkData}", linkData);

        if (linkData is null)
        {
            return NoContent();
        }

        var fullLink = linkData.FullLink;
        if (string.IsNullOrWhiteSpace (fullLink))
        {
            return NoContent();
        }

        var description = linkData.Description;
        var shortLink = _shortener.ShortenLink (fullLink, description);
        
        return new JsonResult (shortLink);
    }
    
    [Route ("get/{shortLink}/{count:bool?}")]
    public ActionResult Get
        (
            string? shortLink,
            bool count = true
        )
    {
        _logger.LogInformation ("Go {ShortLink}, {Count}", shortLink, count);
        
        if (string.IsNullOrWhiteSpace (shortLink))
        {
            return NoContent();
        }

        var fullLink = _shortener.GetFullLink (shortLink, count);

        return new JsonResult (fullLink);
    }
    
    [Route ("list/{shortLink?}")]
    public ActionResult List
        (
            string? shortLink
        )
    {
        _logger.LogInformation ("List {ShortLink}", shortLink);
        
        if (string.IsNullOrWhiteSpace (shortLink))
        {
            var all = _shortener.GetAllLinks();
            return new JsonResult (all);
        }

        var fullLink = _shortener.GetLinkData (shortLink);
        return new JsonResult (fullLink);
    }
}
