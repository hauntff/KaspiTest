using KaspiTest.ApplicationContext;
using KaspiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace KaspiTest.Repository
{
    public class NewsRepository : INewsRespository
    {
        private readonly ApplicationDbContext _context;

        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNews(List<News> news)
        {
            foreach(News item in news.ToList())
            {
                var existingNews = _context.News.Where(x=>x.Title == item.Title && x.Time == item.Time && x.Details == item.Details).FirstOrDefault();
                if (existingNews != null)
                {
                    news.RemoveAll(x=>x.Title == item.Title && x.Time == item.Time && x.Details == item.Details);
                }
            }
            _context.News.AddRange(news);
            _context.SaveChanges();
        }
		public async Task<List<News>> GetNews(DateTime from, DateTime to)
        {
            return await _context.News.Where(x=>x.Time >= from && x.Time <= to).ToListAsync();
        }
		public List<News> Get()
        {
            return _context.News.ToList();
        }

        public async Task<List<News>> GetByText(string text)
        {
            return await _context.News.Where(x=>x.Details.Contains(text)).ToListAsync();
        }
    }
}
