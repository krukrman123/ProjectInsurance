using System.ComponentModel;

namespace ProjectInsurance.Models
{
    public class PolicyHolderDetailModel
    {
		public int PolicyHolderId { get; set; }

		[DisplayName("Jméno")]
		public string Name { get; set; }

		[DisplayName("Příjmení")]
		public string LastName { get; set; }

		[DisplayName("E-mail")]
		public string EMail { get; set; }

		[DisplayName("Telefonní číslo")]
		public string TelephoneNumber { get; set; }

		[DisplayName("Ulice")]
		public string StreetName { get; set; }

		[DisplayName("Číslo popisné")]
		public string BuildingNumber { get; set; }
		[DisplayName("Město")]
		public string CityName { get; set; }

		[DisplayName("PSČ")]
		public string ZIPCode { get; set; }

		public List<InsuranceModel> Insurances { get; set; }
	}
}
