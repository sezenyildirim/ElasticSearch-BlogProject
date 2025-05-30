using Elastic.Clients.Elasticsearch;
using ElastichSearch_BlogProject.Models;

namespace ElastichSearch_BlogProject.Repositories
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "flowers2";

        public BlogRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<List<Flowers?>> SearchAsync(string searchText)
        {
            searchText = searchText.ToLower(); // Wildcard duyarlılığı için normalize et
            var response = await _client.SearchAsync<Flowers>(s => s
                    .Index(indexName)
                    .Size(1000)
                    .Query(q =>
                        q.Bool(b => b.Should(
                            // Match (fuzziness ile, yazım hatası vs.)
                            s => s.Match(m => m.Field("name").Query(searchText).Fuzziness(new Fuzziness(1))),
                            s => s.Match(m => m.Field("latinName").Query(searchText).Fuzziness(new Fuzziness(1))),
                            s => s.Match(m => m.Field("description").Query(searchText).Fuzziness(new Fuzziness(1))),
                            s => s.Match(m => m.Field("category").Query(searchText).Fuzziness(new Fuzziness(1))), //max değer

                            // Wildcard (prefix eşleşmeleri için)
                            s => s.Wildcard(w => w.Field("name").Value($"{searchText}*")),
                            s => s.Wildcard(w => w.Field("latinName").Value($"{searchText}*")),
                            s => s.Wildcard(w => w.Field("description").Value($"{searchText}*")),
                            s => s.Wildcard(w => w.Field("category").Value($"{searchText}*")),

                                //MatchPhrasePrefix
                                s => s.MatchPhrasePrefix(m => m.Field("category").Query(searchText))
                        ))
                    )
                );

            if (!response.IsValidResponse || response.Documents is null)
                return new List<Flowers?>();

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToList();

        }

        public async Task<List<Flowers?>> ListAsync()
        {
            var result = await _client.SearchAsync<Flowers>(s => s
         .Index(indexName)
         .Size(100)
         .Query(q => q.MatchAll())
     );
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;

            }

            return result.Documents.ToList();
        }

        public async Task<Flowers> GetByIdAsync(string id)
        {
            var result = await _client.GetAsync<Flowers>(id, x => x.Index(indexName));

            if (!result.IsSuccess())
            {
                return null;
            }
            return result.Source;

        }
    }
}
