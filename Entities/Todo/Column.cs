using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Todo
{
    public class Column
    {
        [JsonPropertyName("data")]
        public string? Name { get; set; }
        public string? Type { get; set; }   //Тип для Handsontable
        public string? DbType { get; set; }   //Тип поля
        public string? Title { get; set; }
        [JsonPropertyName("width1")]
        public int Size { get; set; }   //Размер для Handsontable, становится width
        public int DbSize { get; set; }
        public int Prec { get; set; }
    }
}
