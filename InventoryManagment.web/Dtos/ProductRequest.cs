
using InventoryManagment.web.Converter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace InventoryManagment.web.Dtos
{
    public class ProductRequest
    {
        public string Name { get; set; }
        [JsonConverter(typeof(converter))]
        public string Price { get; set; }
        [JsonConverter(typeof(converter))]
        public string StockQuantity { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }

    }
    public class ProductResponse
    {
        [JsonConverter(typeof(converter))]
        public string? Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(converter))]
        public string Price { get; set; }
        [JsonConverter(typeof(converter))]
        public string StockQuantity { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
    }
    public class OnlyId {
        [JsonConverter(typeof(converter))]
        public string Id { get; set; }
    }

    public class stockqty {
        [JsonConverter(typeof(converter))]
        public string? StockQuantity { get; set; }

    }
}
