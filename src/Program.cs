using System;
using System.IO;
using System.Xml;
using Models;

namespace src
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string root = @"C:\MP3\";
            foreach(var d in Directory.EnumerateDirectories(root)){
                MP3FileLoader m = new();
                Channel c = m.LoadFolderAsChannel(root, d);
                //m.ConvertChannelToXML(c);
                Console.WriteLine(@$"{d}\podcast.rss.xml");
                File.WriteAllText(@$"{d}\podcast.rss.xml", m.PodCastText(c));
            }
        }
    }
}
