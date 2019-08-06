using MongoDB.Bson;
using System;
using System.Collections;

namespace AOE.Application.Base.Database
{
    public static class BsonValueExtensions
    {
        /// <summary>
        /// Gets the bson value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static BsonValue GetBsonValue<T>(this T value)
        {
            if (value == null) return BsonNull.Value;

            if (Type.GetTypeCode(value.GetType()) != TypeCode.Object) return BsonValue.Create(value);

            if (value is IList list)
            {
                return ToBsonArrayDocument(list);
            }

            return value.ToBsonDocument();
        }

        public static BsonArray ToBsonArrayDocument(this IEnumerable list)
        {
            var array = new BsonArray();

            foreach (var item in list)
            {
                if (Type.GetTypeCode(item.GetType()) != TypeCode.Object)
                {
                    array.Add(BsonValue.Create(item));
                    continue;
                }

                array.Add(item.ToBsonDocument());
            }

            return array;
        }
    }
}
