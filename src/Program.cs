using System;
using System.IO;
using System.Xml;
using PodcastRSSGenerator.Models;

namespace PodcastRSSGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string root = @"C:\Storage\Audiobooks\";
            string base_url = @"https://192.168.50.156:5001/books/";
            foreach(var d in Directory.EnumerateDirectories(root)){
                MP3FileLoader m = new(root, base_url);
                //if (!File.Exists(@$"{d}\podcast.rss.xml"))
                //{
                Channel c = m.LoadFolderAsChannel(d);
                //m.ConvertChannelToXML(c);
                Console.WriteLine(@$"{d}\podcast.rss.xml");
                File.WriteAllText(@$"{d}\podcast.rss.xml", m.PodCastText(c));
                //}
            }
        }
    }
}
