using AOE.Application.Base.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AOE.Application.Base.Database
{
    public class LeftMenuRepository : BaseRepository<LeftMenu>, ILeftMenuRepository
    {
        public LeftMenuRepository(IMongoDatabase db) : base(db, "left-menus") { }
    }
}
