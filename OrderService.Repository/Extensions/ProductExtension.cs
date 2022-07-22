using System.Reflection;
using MongoDB.Driver;
using OrderService.Entity.Models;

namespace OrderService.Repository.Extensions;

public static class ProductExtension
{
     public static IFindFluent<Product,Product> Search(this IMongoCollection<Product> products, string searchTerm)
        {
            var findOptions = new FindOptions()
            {
                Collation = new Collation("en", strength: CollationStrength.Secondary)
            };
            return string.IsNullOrWhiteSpace(searchTerm) ?
                products.Find(p=>true,findOptions) : 
                products.Find(p => p.Name.ToLower()
                    .Contains(searchTerm.Trim().ToLower()),findOptions);
            
        }
        
        
        public static IFindFluent<Product, Product> CustomSort(this IFindFluent<Product, Product> products,
            string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return products.SortBy(p => p.Name);
            var productParams = orderBy.Trim().Split(',');
            var propertyInfos = typeof(Product).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var queryProp = string.Empty;
            var direction = string.Empty;
            foreach (var param in productParams)
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
            var productQuery = "{";
            foreach (var query in queryPropList)
            {
                productQuery += direction == "ascending" ? $"{query}:1," : $"{query}:-1,";
            }

            productQuery = productQuery.TrimEnd(',', ' ');
            productQuery += "}";
            if (string.IsNullOrWhiteSpace(productQuery))
                return products.SortBy(o=>o.Name);
            return products.Sort(productQuery);
        }
}