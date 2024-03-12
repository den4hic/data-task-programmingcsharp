using data_task.Models;
using data_task.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace data_task.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PersonController : ControllerBase
	{
		private readonly ILogger<PersonController> _logger;
		private readonly IPersonService _personService;

		public PersonController(ILogger<PersonController> logger, IPersonService personService)
		{
			_logger = logger;
			_personService = personService;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] PersonRequestModel request)
		{
			if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Birthdate.ToString()))
			{
				return BadRequest(new Person() { ErrorMessage = "Bad data" });
			}

			_logger.LogInformation($"Отримано дату народження: {request.Birthdate}");

			var personModel = new PersonModel
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				Birthdate = DateTime.Parse(request.Birthdate)
			};

			var result = await _personService.CalculatePersonInfoAsync(personModel);

			if (!string.IsNullOrEmpty(result.ErrorMessage))
			{
				return BadRequest(result);
			}

			return new JsonResult(result);
		}
	}
}
