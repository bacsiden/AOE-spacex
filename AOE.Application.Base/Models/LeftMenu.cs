using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AOE.Application.Base.Models
{
    [BsonIgnoreExtraElements]
    public class LeftMenu
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Title { get; private set; }
        public int Order { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public List<string> Actions { get; set; }
        public List<LeftMenu> Children { get; set; } = new List<LeftMenu>();
        public bool Activated { get; set; }
    }
}
