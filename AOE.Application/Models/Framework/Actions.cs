using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AOE.Application.Models.Framework
{
    public static class Actions
    {
        public static Dictionary<string, List<KeyValuePair<string, bool>>> GetValues()
        {
            var result = new Dictionary<string, List<KeyValuePair<string, bool>>>();
            typeof(Actions).GetNestedTypes().ToList().ForEach(m => result.Add(((DescriptionAttribute)m.GetCustomAttributes().First()).Description,
                new List<KeyValuePair<string, bool>>(m.GetFields().Select(x => new KeyValuePair<string, bool>((string)x.GetValue(null), false)))));

            return result;
        }

        [Description("Quản trị hệ thống")]
        public static class Admin
        {
            public const string Config = "Cấu hình";
            public const string UserManager = "Quản lí người dùng";
        }

        [Description("Quản lí file/folder")]
        public static class FileFolder
        {
            public const string List = "Xem danh sách";
            public const string Add = "Thêm file/folder";
            public const string Edit = "Chỉnh sửa file/folder";
        }
    }
}
