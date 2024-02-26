namespace data_task.Models
{
	public class BirthdayResult
	{
		public int Age { get; set; }
		public bool IsBirthdayToday { get; set; }
		public string WesternZodiacSign { get; set; }
		public string ChineseZodiacSign { get; set; }
        public string ErrorMessage { get; set; }
    }

}
