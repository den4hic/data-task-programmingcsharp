using data_task.Models;

namespace data_task.Services
{
	public interface IPersonService
	{
		Task<Person> CalculatePersonInfoAsync(PersonModel model);
	}
}
