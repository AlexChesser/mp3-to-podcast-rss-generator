using System;
using System.Collections.Generic;

namespace PodcastRSSGenerator.Models
{
    public class Channel {
        public string title { get; set; }
        public string description { get; set; }
        public string itunes_image { get; set; }
        public string language { get; set; }
        public string[] itunes_category { get; set; }
        public string itunes_explicit { get; set; }
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string Title { get; set; }
        public Enclosure enclosure { get; set; }

        public string ToXML(){
             return 
@$"<item>
    <title>{this.Title}</title>
    <description>{this.Title}</description>
    <pubDate>{DateTime.Now}</pubDate>
    <enclosure url=""{this.enclosure.URL}""
               type=""audio/mpeg"" length=""{this.enclosure.length}""/>
</item>";
        }
    }

    public class Enclosure {
        public string URL {get; set;}
        public string mime {get; set;}
        public string length {get; set;}
        
    }
}
