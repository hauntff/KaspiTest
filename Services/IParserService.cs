using KaspiTest.Models;

namespace KaspiTest.Services
{
    public interface IParserService
    {
        Task<Dictionary<string, WordInfo>> GetTopWords();
		public bool ParseNews();
    }
}
