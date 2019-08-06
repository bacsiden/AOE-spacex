using AspNetCore.Identity.Mongo.Model;

namespace AOE.Application.Base.Models
{
    public class ApplicationRole : MongoRole
    {
        public ApplicationRole() : base()
        {

        }

        public ApplicationRole(string name) : base(name)
        {

        }
    }
}
