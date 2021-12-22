using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacetsTest.Queries;
using NUnit.Framework;

namespace FacetsTest.TestCases
{
    public class ProductQueriesTests
    {
        private const string OriginalIndex = "Products/Options";
        private readonly ProductsQueries _sut = new ProductsQueries();

        [OneTimeSetUp]
        public async Task Setup()
        {
            //Save products to DB
            await _sut.InitializeCollection();
        }

        [TestCase(OriginalIndex)]
        public async Task FacetsWithoutFiltering_ShouldReturn2BrandsWithCount1(string indexName)
        {
            var facets = await _sut.GetProductsFacets(indexName, false, new Dictionary<string, IList<string>>());
            var brandFacetValues = facets["Brand"].Values.OrderBy(v => v.Range).ToList();

            Assert.AreEqual(2, brandFacetValues.Count);
            Assert.AreEqual("adidas", brandFacetValues[0].Range);
            Assert.AreEqual(2, brandFacetValues[0].Count, "Expected two unavailable products of brand Adidas");
            Assert.AreEqual("nike", brandFacetValues[1].Range);
            Assert.AreEqual(4, brandFacetValues[1].Count, "Expected only one product with brand Nike");
        }
        
        [Test(Description = "Only one product has at least an option available with Size 9, so I expect to have as result a single brand (Adidas) with count 1")]
        [TestCase(OriginalIndex)]
        public async Task FacetsFilteringBySize9AndAvailabilityTrue_ShouldReturnOnlyProductsWithAtLeastOneOptionAvailableWithTheSpecifiedSize(string indexName)
        {
            var facets = await _sut.GetProductsFacets(indexName, true, new Dictionary<string, IList<string>>{{"Size", new List<string>{"9"}}});
            var brandFacetValues = facets["Brand"].Values.OrderBy(v => v.Range).ToList();

            Assert.AreEqual(1, brandFacetValues.Count, "Expected only one brand, because only Adidas shoes has an option available with size 9");
            Assert.AreEqual("adidas", brandFacetValues[0].Range);
            Assert.AreEqual(2, brandFacetValues[0].Count, "Expected two products with brand Adidas");
        }

        [Test(Description = "No product has an option available with Size 11, so I expect to have as result 0 brand")]
        [TestCase(OriginalIndex)]
        public async Task FacetsFilteringBySize11AndAvailabilityTrue_ShouldNotReturnAnyProduct(string indexName)
        {
            var facets = await _sut.GetProductsFacets(indexName, true, new Dictionary<string, IList<string>>{{"Size", new List<string>{"11"}}});
            var brandFacetValues = facets["Brand"].Values.OrderBy(v => v.Range).ToList();

            Assert.AreEqual(0, brandFacetValues.Count, "Expected 0 brand, because no product has an option available with size 11");
        }
    }
}
