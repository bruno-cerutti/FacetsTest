using System.Linq;
using FacetsTest.Models;
using Raven.Client.Documents.Indexes;

namespace FacetsTest.Indexes
{
    public class Products_Options_V2 : AbstractIndexCreationTask<Product>
    {
        public Products_Options_V2()
        {
            Map = products => from doc in products
                select new
                {
                    doc.Title,
                    doc.Brand,
                    Option_Available = doc.Options.Select(option => option.Available),
                    _ = doc.Options.Select(option => option.Attributes.Select(attribute => CreateField(attribute.Name, attribute.Value)))
                };
        }
    }
}