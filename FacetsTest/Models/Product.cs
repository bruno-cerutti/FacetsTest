using System.Collections.Generic;
using Raven.Client.Documents.Operations.OngoingTasks;

namespace FacetsTest.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public IList<ProductOption> Options { get; set; }
    }
}