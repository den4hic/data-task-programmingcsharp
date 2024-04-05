using data_task.Models;

namespace data_task.Services
{
	public interface IPersonService
	{
		Task<Person> CalculatePersonInfoAsync(PersonModel model);
		List<Person> MakeSort(List<Person> users, string sortField, string sortDirection);
		Task<List<Person>> ProcessUsersFromFileAsync(string filePath);
		List<Person> SearchUsers(List<Person> filteredUsers, string? searchTerm);
		List<Person> SearchDateRange(List<Person> cachedUsers, string? startDate, string? endDate);
		List<Person> SearchSunSign(List<Person> filteredUsers, string searchSunSign);
		List<Person> SearchChineseSign(List<Person> filteredUsers, string searchChineseSign);
		List<Person> SearchEmail(List<Person> filteredUsers, string searchEmail);
		void SerializeUsersToFile(List<Person> users, string filePath);
		List<Person> FilterAdult(List<Person> filteredUsers, string? filterIsAdult);
		List<Person> FilterBirthday(List<Person> filteredUsers, string? filterIsAdult);
	}
}
