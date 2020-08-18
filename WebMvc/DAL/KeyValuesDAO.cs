using CFData;
using System;
using System.Linq;

namespace DAL
{
    public class KeyValuesDAO : Base<KeyValues>
    {

        public KeyValues Test()
        {
            return _db.KeyValues.FirstOrDefault();
        }
    }
}
