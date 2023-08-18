using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectInsurance.Models
{
    public class InsuranceModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        public int InsuranceHolderId { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Pojištění")]
        public string InsuranceType { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Neplatná částka")]
        [DisplayName("Částka")]
        public string InsuranceAmount { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Předmět pojištění")]
        public string InsuranceSubject { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DataType(DataType.Date)]
        [DisplayName("Platnost od")]
        public DateTime InsuranceValidFrom { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DataType(DataType.Date)]
        [DisplayName("Platnost do")]
        public DateTime InsuranceValidUntil { get; set; }


        
    }
}
