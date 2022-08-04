using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Extensions;

public static class OrderExtension
{
     public static IFindFluent<Order,Order> Search(this IMongoCollection<Order> orders, string searchTerm,string searchArea, string customerId="")
        {
            
            var builder = Builders<Order>.Filter;
            searchArea = char.ToUpper(searchArea[0]) + searchArea.Substring(1);
            var filter = Builders<Order>.Filter.Regex(searchArea,new BsonRegularExpression(new Regex(searchTerm,RegexOptions.IgnoreCase)));
            if (customerId == "")
            {
                if (string.IsNullOrWhiteSpace(searchTerm)) return orders.Find(c => true);
                return orders.Find(filter);
            }
            if (string.IsNullOrWhiteSpace(searchTerm)) return orders.Find(c => c.CustomerId == customerId);
            var filter2 = Builders<Order>.Filter.Eq("CustomerId", customerId);
            filter &= filter2;
            
            return orders.Find(filter);
            
            
            
        }
        public static IFindFluent<Order, Order> CustomSort(this IFindFluent<Order, Order> orders,
            string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return orders;
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
            return orders.Sort(query);
        }
}