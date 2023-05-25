using System.ComponentModel.DataAnnotations;
using static TestForDotNet.Class.Cartoon;

namespace TestForDotNet.Class;
public class Cartoon
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Info
    {
        public int count { get; set; }
        public int pages { get; set; }
        public string next { get; set; }
        public object prev { get; set; }
    }

    public class Location
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Origin
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Character
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string species { get; set; }
        public string type { get; set; }
        public string gender { get; set; }
        //public Origin origin { get; set; }
        //public Location location { get; set; }
        public string image { get; set; }

        //public List<Episode> episode { get; set; }
        public string url { get; set; }
        public DateTime? created { get; set; }
    }

    public class Root
    {
        public Info info { get; set; }
        public List<Character> results { get; set; }
    }
}

public class Episode
{
    public int id { get; set; }
    public string name { get; set; }
    public string air_date { get; set; }
    public string episode { get; set; }
    public List<Character> characters { get; set; }
    public string url { get; set; }
    public DateTime created { get; set; }
}