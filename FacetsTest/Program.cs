using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacetsTest.Queries;
using Raven.Client.Documents.Queries.Facets;

namespace FacetsTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var productsQueries = new ProductsQueries();
            
            //Run InitializeCollection the first time to save two sample products into the DB
            //await productsQueries.InitializeCollection();

            Console.WriteLine("Results without filtering");
            var products = await productsQueries.GetProducts(null, new Dictionary<string, IList<string>>());
            Console.WriteLine("Number of products found: {0}", products.Count);
            var facets = await productsQueries.GetProductsFacets(null, new Dictionary<string, IList<string>>());
            PrintFacetResult(facets);

            Console.WriteLine("Results filtering by brand='Nike'");
            var productsByBrand = await productsQueries.GetProducts(null, new Dictionary<string, IList<string>>{{"Brand", new List<string>{"Nike"}}});
            Console.WriteLine("Number of products found: {0}", productsByBrand.Count);
            var facets2 = await productsQueries.GetProductsFacets(null, new Dictionary<string, IList<string>>{{"Brand", new List<string>{"Nike"}}});
            PrintFacetResult(facets2);

            Console.WriteLine("Results filtering by availability=true and Size='9'");
            var productsAvailable = await productsQueries.GetProducts(true, new Dictionary<string, IList<string>>{{"Size", new List<string>{"9"}}});
            Console.WriteLine("Number of products found: {0}", productsAvailable.Count);
            var facets3 = await productsQueries.GetProductsFacets(true, new Dictionary<string, IList<string>>{{"Size", new List<string>{"9"}}});
            PrintFacetResult(facets3);
        }

        private static void PrintFacetResult(IDictionary<string, FacetResult> facets)
        {
            foreach (var facetResult in facets)
            {
                Console.WriteLine("FacetResult {0} -> FacetValues: {1}", facetResult.Key,
                    string.Join(',', facetResult.Value.Values.Select(v => $"{v.Range} ({v.Count})")));
            }
        }
    }
}
