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
            foreach(var d in Directory.EnumerateDirectories(root)){
                MP3FileLoader m = new();
                //if (!File.Exists(@$"{d}\podcast.rss.xml"))
                //{
                Channel c = m.LoadFolderAsChannel(root, d);
                //m.ConvertChannelToXML(c);
                Console.WriteLine(@$"{d}\podcast.rss.xml");
                File.WriteAllText(@$"{d}\podcast.rss.xml", m.PodCastText(c));
                //}
            }
        }
    }
}
