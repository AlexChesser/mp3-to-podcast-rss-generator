using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Models;

public class MP3FileLoader {

    private string base_url = "http://192.168.50.112/";
    public Channel LoadFolderAsChannel (string root, string folder) {
        string[] files = Directory.GetFiles(folder);
        Channel c = new Channel();
        c.items = new List<Item>();
        bool first = true;
        foreach(string f in files){
            FileInfo fi = new FileInfo(f);
            if(fi.Name != "folder.jpg" && (fi.Extension == ".jpg" || fi.Extension == ".png")){
                c.itunes_image = f.Replace(root, base_url).Replace("\\", "/");
            }
            if(f.EndsWith("mp3")){
                TagLib.File mp3 = TagLib.File.Create(f);
                TagLib.Tag tag = mp3.Tag; 
                if(first){
                    first = false;
                    c.title = tag.Album;
                    c.language = "English";
                    c.itunes_category = new string [] { "General" };//tag.Genres;
                    c.description = string.Join("|",tag.AlbumArtists);
                }
                c.items.Add(new Item {
                    Title = tag.Title,
                    enclosure = new Enclosure {
                        URL = f.Replace(root, base_url).Replace("\\", "/"),
                        length = fi.Length.ToString(),
                        mime = "audio/mpeg"
                    }
                });
            }
        }
        return c;
    }

    public void ConvertChannelToXML(Channel c){
        using (XmlWriter writer = XmlWriter.Create("C:\\MP3\\Find Your Why\\channel.xml"))  
        {  
            writer.WriteRaw("<rss xmlns:itunes=\"http://www.itunes.com/dtds/podcast-1.0.dtd\" version=\"2.0\">");
            writer.WriteStartElement("channel");
            writer.WriteElementString("title", c.title);
            writer.WriteElementString("description", c.description);
            writer.WriteRaw($"<itunes:image>{c.itunes_image}</itunes:image>");
            writer.WriteRaw($"<itunes:category>{string.Join("|",c.itunes_category)}</itunes:category>");
            writer.WriteRaw("<itunes:explicit>False</itunes:explicit>");
            foreach (Item item in c.items)
            {
                writer.WriteStartElement("item");
                writer.WriteElementString("title", item.Title);
                writer.WriteRaw(@$"<enclosure url=""{item.enclosure.URL}"" length=""{item.enclosure.length}"" type=""audio/mpeg"" />");
                writer.WriteEndElement();
            }
            writer.WriteEndElement(); 
            writer.WriteRaw("</rss>"); 
            writer.Flush();  
        }  
    }


    public string PodCastText(Channel c){
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