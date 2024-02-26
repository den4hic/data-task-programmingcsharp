using data_task.Models;

namespace data_task.Services
{
	public interface IBirthdayService
	{
		BirthdayResult CalculateBirthdayInfo(BirthdayModel model);
	}
}
