namespace data_task.Models
{
	public class BirthdayModel
	{
		public DateTime Birthdate { get; set; }

		public override string ToString()
		{
			return $"Дата народження: {Birthdate}";
		}
	}
}
