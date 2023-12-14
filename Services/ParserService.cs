using KaspiTest.Helpers;
using KaspiTest.Models;
using KaspiTest.Repository;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;

namespace KaspiTest.Services
{
	using KaspiTest.Models;
	using System.Linq;

	public class ParserService : IParserService
	{
		private readonly INewsRespository _newsRespository;
		private readonly string _baseUrl;
		private readonly string _newsController;


		public ParserService(INewsRespository newsRespository, IConfiguration configuration)
		{
			_newsRespository = newsRespository;
			_baseUrl = configuration["TengriNews:Url"];
			_newsController = configuration["TengriNews:News"];
		}

		public bool ParseNews()
		{
			List<News> news = new List<News>();
			var client = new WebClient();

			for (int page = 1; page <= 10; page++)
			{
				string pageUrl = page == 1 ? "" : $"page/{page}/";

				var html = client.DownloadString($"{_baseUrl}{_newsController}{pageUrl}");

				// parse multiple text between <time> and </time> from html and add to times list and parse multiple text between tn-main-news-title"> and < from html and add to titles list
				var times = Regex.Matches(html, @"<time>(.*?)</time>");
				var titles = Regex.Matches(html, @"tn-article-title"">(.*?)<");
				var links = ParsingHelper.GetLinks(html);

				for (int i = 0; i < links.Count; i++)
				{
					if (links[i][0] == '/')
					{
						links[i] = _baseUrl + links[i];
					}
				}

				// loop through times and titles and add to news list
				for (int i = 0; i < times.Count; i++)
				{
					var detailsHtml = client.DownloadString(links[i]);

					var details = Regex.Match(detailsHtml, @"<p>(.*?)</p>");

					// remove all html tags from details
					var detailsText = Regex.Replace(details.Groups[1].Value, "<.*?>", string.Empty);

					var time = ParsingHelper.ParseDate(times[i].Groups[1].Value);
					news.Add(new News
					{
						Time = time,//times[i].Groups[1].Value,
						Title = titles[i].Groups[1].Value,
						Link = links[i],
						Details = detailsText
					});


				}
			}

			_newsRespository.AddNews(news);

			return true;
		}

		public async Task<Dictionary<string, WordInfo>> GetTopWords()
		{
			List<News> news = _newsRespository.Get();

			Dictionary<string, WordInfo> wordCounter = new Dictionary<string, WordInfo>();

			foreach (var item in news)
			{
				// seperate all the words in detailsText by space, and remove commas and dots and then count the words and add to wordCount map with their count and link
				var words = item.Details.Split(' ');
				foreach (var word in words)
				{
					// remove all the symbols exept letters
					var wordWithoutSymbols = Regex.Replace(word, @"[^a-zA-Zа-яА-Я]", string.Empty).ToLower();

					if (wordCounter.ContainsKey(wordWithoutSymbols))
					{
						wordCounter[wordWithoutSymbols].Count++;
					}
					else
					{
						wordCounter.Add(wordWithoutSymbols, new WordInfo
						{
							Count = 1,
							Link = item.Link
						});
					}
				}
			}


			foreach (var word in wordCounter)
			{
				if (word.Key.Length < 3)
				{
					wordCounter.Remove(word.Key);
				}
			}
			wordCounter = wordCounter.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
			var topTenWords = wordCounter.Take(10).ToDictionary(x => x.Key, x => x.Value);
			return topTenWords;
		}
	}
}
