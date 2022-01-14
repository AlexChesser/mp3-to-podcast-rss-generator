using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using PodcastRSSGenerator.Models;

namespace PodcastRSSGenerator
{



    public class MP3FileLoader
    {
        private string base_url;
        private string root;
        private Channel c;

        public MP3FileLoader(string audiobook_storage_root, string podcast_host_url)
        {
            base_url = podcast_host_url;
            root = audiobook_storage_root;
        }

        private void HandleMP3(string f, FileInfo fi)
        {
            TagLib.File mp3 = TagLib.File.Create(f);
            TagLib.Tag tag = mp3.Tag;
            if (!c.HasFirst)
            {
                c.title = tag.Album;
                c.language = "English";
                c.itunes_category = new string[] { "General" };//tag.Genres;
                c.description = string.Join("|", tag.AlbumArtists);
            }
            c.items.Add(new Item
            {
                Title = tag.Title != "" ? tag.Title : fi.Name,
                enclosure = new Enclosure
                {
                    URL = f.Replace(root, base_url).Replace("\\", "/"),
                    length = fi.Length.ToString(),
                    mime = "audio/mpeg"
                }
            });
        }

        private void HandleImage(string folder, string f, FileInfo fi)
        {
            string tile_name = $"{folder}\\tile.jpg";
            if (c.itunes_image == null)
            {
                File.Copy(fi.FullName, tile_name, true);
                c.itunes_image = f.Replace(root, base_url).Replace("\\", "/");
            }
            else if (!c.itunes_image.ToLower().Contains("cover"))
            {
                File.Copy(fi.FullName, tile_name, true);
                c.itunes_image = f.Replace(root, base_url).Replace("\\", "/");
            }
        }

        public Channel LoadFolderAsChannel(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            c = new();
            c.items = new List<Item>();
            foreach (string f in files)
            {
                FileInfo fi = new FileInfo(f);
                if (fi.Name != "folder.jpg" && (fi.Extension == ".jpg" || fi.Extension == ".png"))
                {
                    HandleImage(folder, f, fi);
                }
                if (f.EndsWith("mp3"))
                {
                    HandleMP3(f, fi);
                }
            }
            return c;
        }

        public string PodCastText(Channel c)
        {
            string[] items = c.items.Select(i => i.ToXML()).ToArray();
            return
    @$"<?xml version=""1.0"" encoding=""UTF-8""?>
<rss version=""2.0""
    xmlns:itunes=""http://www.itunes.com/dtds/podcast-1.0.dtd"">
  <channel>
    <title>{c.title}</title>
    <itunes:owner>
        <itunes:email>home@example.com</itunes:email>
    </itunes:owner>
    <itunes:author>Personal</itunes:author>
    <description>{c.description}</description>
    <itunes:image href=""{c.itunes_image}""/>
    <language>en-us</language>
    <link>{base_url}</link>
{string.Join("", items)}
  </channel>
</rss>";

        }
    }

}

