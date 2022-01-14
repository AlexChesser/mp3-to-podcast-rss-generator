using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using System.IO;
using PodcastRSSGenerator;
using PodcastRSSGenerator.Models;

namespace mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment Host;
    private readonly IConfiguration Configuration;


    public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IWebHostEnvironment host)
    {
        _logger = logger;
        Configuration = configuration;
        Host = host;
    }

    private void GenerateRSSFile(string d, string host, string root)
    {
        Console.WriteLine($"Attempting to build RSS feed: {d}\\podcast.rss.xml");
        try
        {
            MP3FileLoader m = new(host);
            Channel c = m.LoadFolderAsChannel(root, d);
            System.IO.File.WriteAllText(@$"{d}\podcast.rss.xml", m.PodCastText(c));
        }
        catch (Exception)
        {
            Console.WriteLine($"{d} failed  to create an RSS feed");
        }
    }

    public IActionResult Index()
    {
        string root = Configuration["PODCAST_FILE_PATH"];
        ViewBag.Host = $"{Request.Scheme}://{Request.Host.Value}/books/";
        List<string> tiles = new();
        foreach (string d in Directory.EnumerateDirectories(root))
        {
            string rss = @$"{d}\podcast.rss.xml";
            if (!System.IO.File.Exists(rss) || Request.Query.ContainsKey("reindex"))
            {
                GenerateRSSFile(d, ViewBag.Host, root);
            }
            tiles.Add(d.Replace(root, ""));
        }
        ViewBag.Tiles = tiles;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
