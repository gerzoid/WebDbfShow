using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class Column
    {
        [JsonPropertyName("data")]
        public string? Name { get; set; }
        public string? Type { get; set; }

        public string? Title { get; set; }
        [JsonPropertyName("width")]
        public int Size { get; set; }
        public int Prec { get; set; }
    }
}
