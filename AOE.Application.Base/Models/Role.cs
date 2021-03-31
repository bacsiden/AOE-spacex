using System;
using System.Collections.Generic;
using System.Text;

namespace AOE.Application.Base.Models
{
    public class Role : BaseEntity
    {
        [SetCopy]
        public string Title { get; set; }

        public List<string> Actions { get; set; } = new List<string>();

        public List<UserMeta> Users { get; set; } = new List<UserMeta>();
    }
}
