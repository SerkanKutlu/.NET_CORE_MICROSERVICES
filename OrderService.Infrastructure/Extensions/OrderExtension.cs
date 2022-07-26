using System.Reflection;
using MongoDB.Driver;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Extensions;

public static class OrderExtension
{
     public static IFindFluent<Order,Order> Search(this IMongoCollection<Order> orders, string searchTerm, string customerId="")
        {
            var findOptions = new FindOptions()
            {
                Collation = new Collation("en", strength: CollationStrength.Secondary)
            };
            if(customerId == "")
                return string.IsNullOrWhiteSpace(searchTerm) ?
                    orders.Find(o=>true,findOptions) : 
                    orders.Find(o => o.Address.City.ToLower()
                        .Contains(searchTerm.Trim().ToLower()),findOptions);
            return string.IsNullOrWhiteSpace(searchTerm) ?
                orders.Find(o=>o.CustomerId == customerId,findOptions) : 
                orders.Find(o => o.Address.City.ToLower()
                    .Contains(searchTerm.Trim().ToLower()) && o.CustomerId == customerId,findOptions);
            
            
        }
        public static IFindFluent<Order, Order> CustomSort(this IFindFluent<Order, Order> orders,
            string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return orders.SortBy(o => o.CreatedAt);
            var orderParams = orderBy.Trim().Split(',');
            var propertyInfos = typeof(Order).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var queryProp = string.Empty;
            var direction = string.Empty;
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                    pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                direction = param.EndsWith(" desc") ? "descending" : "ascending";
                queryProp += objectProperty.Name+",";
                
            }

            var queryPropList = queryProp.TrimEnd(',', ' ').Split(",");
            var orderQuery = "{";
            foreach (var query in queryPropList)
            {
                orderQuery += direction == "ascending" ? $"{query}:1," : $"{query}:-1,";
            }

            orderQuery = orderQuery.TrimEnd(',', ' ');
            orderQuery += "}";
            if (string.IsNullOrWhiteSpace(orderQuery))
                return orders.SortBy(o=>o.CreatedAt);
            return orders.Sort(orderQuery);
        }
}