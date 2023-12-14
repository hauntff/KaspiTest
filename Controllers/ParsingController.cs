using KaspiTest.Repository;
using KaspiTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KaspiTest.Controllers
{
    [ApiController]
    [Route("api/")]
    [Authorize]
    public class ParsingController : ControllerBase
    {
        private readonly IParserService _newsService;
        private readonly INewsRespository _newsRepository;

		public ParsingController(IParserService newsService, INewsRespository newsRepository)
		{
			_newsService = newsService;
			_newsRepository = newsRepository;
		}


        [HttpGet]
        [Route("posts")]
        public async Task<IActionResult> Posts(DateTime from, DateTime to) => Ok(await _newsRepository.GetNews(from, to));

        [HttpGet]
        [Route("topten")]
        public async Task<IActionResult> GetTopWords()
        {
			return Ok(await _newsService.GetTopWords());
        }

		[HttpGet]
		[Route("search")]
		public async Task<IActionResult> GetByText(string text)
		{
			return Ok(await _newsRepository.GetByText(text));
		}

	}
}
