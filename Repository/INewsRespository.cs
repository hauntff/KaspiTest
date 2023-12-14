using KaspiTest.Models;

namespace KaspiTest.Repository
{
    public interface INewsRespository
    {
        Task AddNews(List<News> news);
        Task<List<News>> GetNews(DateTime from, DateTime to);
        Task<List<News>> GetByText(string text);
		List<News> Get();
    }
}
