using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CFData
{
    [Table("KeyValues")]
    public class KeyValues
    {
        [Key]
        public int Id { get; set; }

        public string KeyValueName { get; set; }

        public string Text { get; set; }

        public string Value { get; set; }

        public int Sort { get; set; }

        public int EnterPriseId { get; set; }
    }
}
