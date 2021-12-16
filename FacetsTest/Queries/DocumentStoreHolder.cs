using System;
using FacetsTest.Indexes;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;

namespace FacetsTest.Queries
{
    public class DocumentStoreHolder
    {
        private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

        public static IDocumentStore Store => store.Value;
    
        private static IDocumentStore CreateStore()
        {
            IDocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "FacetsTest"
            }.Initialize();
            
            IndexCreation.CreateIndexes(typeof(Products_Options).Assembly, store);

            return store;
        }
    }
}