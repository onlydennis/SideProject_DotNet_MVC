using CFData;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModel;

namespace DAL
{
    public class KeyValuesDAO : Base<KeyValues>
    {
        public List<KeyValues> QueryAll(KeyValuesViewModel query)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("select                ");
            stringBuilder.Append("	K.Id,               ");
            stringBuilder.Append("	K.KeyValueName,     ");
            stringBuilder.Append("	K.Text,             ");
            stringBuilder.Append("	K.Value,            ");
            stringBuilder.Append("	K.Sort,             ");
            stringBuilder.Append("	K.EnterPriseId      ");
            stringBuilder.Append("from                  ");
            stringBuilder.Append("	KeyValues K         ");
            stringBuilder.Append("where                 ");
            stringBuilder.Append("	1 = 1               ");

            Dapper.DynamicParameters parameterList = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(query.KeyValueName))
            {
                stringBuilder.AppendLine(@" and K.KeyValueName LIKE @keyVlaueName ");
                parameterList.Add("@keyVlaueName", $"%{query.KeyValueName}%");
            }

            if (!string.IsNullOrWhiteSpace(query.Text))
            {
                stringBuilder.AppendLine(@" and K.Text LIKE @text ");
                parameterList.Add("@text", $"%{query.Text}%");
            }

            stringBuilder.AppendLine(" order by K.Sort, K.Id desc ");

            var res = _db.Database.Connection.Query<KeyValues>(stringBuilder.ToString(), parameterList).ToList();

            return res;
        }
    }
}
