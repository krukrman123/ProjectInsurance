using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectInsurance.Models
{
    public class PolicyHolderModel
    {
		[Key]
		public int Id { get; set; }

		public string UserId { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné")]
		[RegularExpression(@"^[^\d]+$", ErrorMessage = "Neplatné jméno")]
		[DisplayName("Jméno")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Toto pole je povinné")]
		[RegularExpression(@"^[^\d]+$", ErrorMessage = "Neplatné příjmení")]
		[DisplayName("Příjmení")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "Toto pole je povinné")]
		[RegularExpression(@"^[\w-\.]+@([\w -]+\.)+[\w-]{2,4}$", ErrorMessage = "Neplatný Email")]
		[DisplayName("E-mail")]
		public string EMail { get; set; }
		[Required(ErrorMessage = "Toto pole je povinné")]
		[RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Zadejte devíticiferné telefonní číslo")]
		[DisplayName("Telefonní číslo")]
		public string TelephoneNumber { get; set; }
		[Required(ErrorMessage = "Toto pole je povinné")]
		[DisplayName("Ulice")]
		public string StreetName { get; set; }
		[Required(ErrorMessage = "Toto pole je povinné")]
		[RegularExpression(@"^\d*\/?\d+$", ErrorMessage = "Neplatné č.p.")]
		[DisplayName("Číslo popisné")]
		public string BuildingNumber { get; set; }
		[Required(ErrorMessage = "Toto pole je povinné")]
		[RegularExpression(@"^\D+(\s\d+)?$", ErrorMessage = "Neplatný název")]
		[DisplayName("Město")]
		public string CityName { get; set; }
		[Required(ErrorMessage = "Toto pole je povinné")]
		[RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Zadejte pěticiferné PSČ")]
		[DisplayName("PSČ")]
		public string ZIPCode { get; set; }



	}
}
