using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AOE.Application.Base.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    [BsonIgnoreExtraElements]
    public class ApplicationUser : MongoUser
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }

        public bool Locked { get; set; }
    }
}
