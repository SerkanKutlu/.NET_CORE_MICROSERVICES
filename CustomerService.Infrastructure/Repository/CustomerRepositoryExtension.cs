using System.Text.RegularExpressions;
using CustomerService.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CustomerService.Infrastructure.Repository;

public static class CustomerRepositoryExtension
{
    public static IFindFluent<Customer,Customer> Search(this IMongoCollection<Customer> customers, string searchTerm,string searchArea)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return customers.Find(c => true);
            searchArea = char.ToUpper(searchArea[0]) + searchArea.Substring(1);
            var filter = Builders<Customer>.Filter.Regex(searchArea,new BsonRegularExpression(new Regex(searchTerm,RegexOptions.IgnoreCase)));
            return customers.Find(filter);
           
        }

        public static IFindFluent<Customer, Customer> CustomSort(this IFindFluent<Customer, Customer> customers,
            string orderBy)
        {
            #region Option1

            // if (string.IsNullOrWhiteSpace(orderBy)) return customers;
            // var orderParameters = orderBy.Split(' ');
            // var orderTo = orderParameters[0];
            // orderTo = char.ToUpper(orderTo[0]) + orderTo.Substring(1);
            // var direction = orderParameters.Length == 2 ? orderParameters[1] : "asc";
            // var query = direction.Contains("esc") ? $"{{{orderTo}:-1}}" : $"{{{orderTo}:1}}";
            // return customers.Sort(query);

            #endregion

            #region Option2

            if (string.IsNullOrWhiteSpace(orderBy)) return customers;
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
            return customers.Sort(query);
            #endregion





            // if (string.IsNullOrWhiteSpace(orderBy))
            //     return customers.SortBy(e => e.Name);
            // var customerParams = orderBy.Trim().Split(','); //To order same params
            // var propertyInfos = typeof(Customer).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            // var queryProp = string.Empty;
            // var direction = string.Empty;
            // foreach (var param in customerParams)
            // {
            //     if (string.IsNullOrWhiteSpace(param))
            //         continue;
            //     var propertyFromQueryName = param.Split(" ")[0];
            //     var objectProperty = propertyInfos.FirstOrDefault(pi =>
            //         pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            //     if (objectProperty == null)
            //         continue;
            //     direction = param.EndsWith(" desc") ? "descending" : "ascending";
            //     queryProp += objectProperty.Name+",";
            // }
            //
            // var queryPropList = queryProp.TrimEnd(',', ' ').Split(",");
            // var customerQuery = "{";
            // foreach (var query in queryPropList)
            // {
            //     customerQuery += direction == "ascending" ? $"{query}:1," : $"{query}:-1,";
            // }
            //
            // customerQuery = customerQuery.TrimEnd(',', ' ');
            // customerQuery += "}";
            // if (string.IsNullOrWhiteSpace(customerQuery))
            //     return customers.SortBy(c=>c.Name);
            // return customers.Sort(customerQuery); //{Email:1} or {Name:-1}


            }
}   