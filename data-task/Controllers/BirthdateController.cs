using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace data_task.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BirthdateController : ControllerBase
	{
		private readonly ILogger<BirthdateController> _logger;

		public BirthdateController(ILogger<BirthdateController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public IActionResult Post([FromBody] string birthdate)
		{
			return new JsonResult(new { Message = $"Отримано дату народження: {birthdate}" });
		}
	}
}
