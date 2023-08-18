using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectInsurance.Models
{
    public class InsuranceEventModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        public int InsuranceId { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        public int PolicyHolderId { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné\"")]
        [DisplayName("Jméno")]
        public string PolicyHolderName { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné\"")]
        [DisplayName("Příjmení")]
        public string PolicyHolderLastName { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné\"")]
        [DisplayName("Pojištění")]
        public string InsuranceType { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné\"")]
        [DisplayName("Typ pojištěni")]
        public string InsuranceSubject { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné\"")]
        [DataType(DataType.Date)]
        [DisplayName("Datum vzniku škodní události ")]
        public DateTime InsuranceEventTime { get; set; }
    }
}
