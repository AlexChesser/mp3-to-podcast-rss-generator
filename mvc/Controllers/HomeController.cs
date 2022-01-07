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

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        string root = @"C:\Storage\Audiobooks\";
        ViewBag.Host = "http://192.168.50.156/books/";
        List<string> tiles = new();
        foreach (string d in Directory.EnumerateDirectories(root))
        {
            if (!System.IO.File.Exists(@$"{d}\podcast.rss.xml"))
            {
                MP3FileLoader m = new();
                Channel c = m.LoadFolderAsChannel(root, d);
                System.IO.File.WriteAllText(@$"{d}\podcast.rss.xml", m.PodCastText(c));
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
