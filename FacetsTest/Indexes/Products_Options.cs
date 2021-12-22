using System.Linq;
using FacetsTest.Models;
using Raven.Client.Documents.Indexes;

namespace FacetsTest.Indexes
{
    public class Products_Options : AbstractIndexCreationTask<Product>
    {
        public Products_Options()
        {
            Map = products => from doc in products
                from option in doc.Options
                select new
                {
                    doc.Title,
                    doc.Brand,
                    Option_Available = option.Available,
                    _ = option.Attributes.Select(attribute => CreateField(attribute.Name, attribute.Value))
                };
        }
    }
}