using data_task.Models;
using data_task.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace data_task.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BirthdayController : ControllerBase
	{
		private readonly ILogger<BirthdayController> _logger;
		private readonly IBirthdayService _birthdayService;

		public BirthdayController(ILogger<BirthdayController> logger, IBirthdayService birthdayService)
		{
			_logger = logger;
			_birthdayService = birthdayService;
		}

		[HttpPost]
		public IActionResult Post([FromBody] string birthdate)
		{
			var birthdayModel = new BirthdayModel { Birthdate = DateTime.Parse(birthdate) };

			_logger.LogInformation($"Отримано дату народження: {birthdayModel}");

			var result = _birthdayService.CalculateBirthdayInfo(birthdayModel);

			if (!string.IsNullOrEmpty(result.ErrorMessage))
			{
				return BadRequest(result);
			}

			return new JsonResult(result);
		}
	}
}
