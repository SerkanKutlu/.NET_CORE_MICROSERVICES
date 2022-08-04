using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Extensions;

public static class ProductExtension
{
     public static IFindFluent<Product,Product> Search(this IMongoCollection<Product> products, string searchTerm,string searchArea)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return products.Find(c => true);
            searchArea = char.ToUpper(searchArea[0]) + searchArea.Substring(1);
            var filter = Builders<Product>.Filter.Regex(searchArea,new BsonRegularExpression(new Regex(searchTerm,RegexOptions.IgnoreCase)));
            return products.Find(filter);
        }
        
        
        public static IFindFluent<Product, Product> CustomSort(this IFindFluent<Product, Product> products,
            string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return products;
            var orderParameters = orderBy.Split(',');
            var query = "{";
            var direction = orderParameters[^1].Split(' ')[1].Contains("desc") ? "-1" : "1";
            for (var i = 0; i < orderParameters.Length; i++)
            {
                if (i == orderParameters.Length - 1)
                {
                    var subParam = orderParameters[i].Split(' ')[0];
                    subParam = char.ToUpper(subParam[0]) + subParam.Substring(1);
                    query += $"{subParam}:{direction}}}";
                    continue;
                }
                var param = char.ToUpper(orderParameters[i][0]) + orderParameters[i].Substring(1);
                query += $"{param}:{direction},";
            }
            return products.Sort(query);
        }
}