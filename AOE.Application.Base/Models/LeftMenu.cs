using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AOE.Application.Base.Models
{
    [BsonIgnoreExtraElements]
    public class LeftMenu
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public List<string> Actions { get; set; }
        public List<LeftMenu> Children { get; set; } = new List<LeftMenu>();
        public bool Activated { get; set; }
    }
}
