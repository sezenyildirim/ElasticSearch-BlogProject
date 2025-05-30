using ElastichSearch_BlogProject.Models;
using ElastichSearch_BlogProject.Repositories;

namespace ElastichSearch_BlogProject.Services
{
    public class BlogService
    {
        private readonly BlogRepository _repository;

        public BlogService(BlogRepository repository)
        {
            _repository = repository;

        }

        public async Task<List<Flowers>> SearchAsync(string searchText)
        {
            try
            {

                return await _repository.SearchAsync(searchText);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Flowers>> ListAsync()
        {
            try
            {
                return await _repository.ListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Flowers> GetByIdAsync(string id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return null;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
