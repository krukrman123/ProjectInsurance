using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectInsurance.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [DisplayName("Jméno")]
    public string Name { get; set; }
    [DisplayName("Příjmení")]
    public string LastName { get; set; }
    [DisplayName("Ulice")]
    public string StreetName { get; set; }
    [DisplayName("Číslo popisné")]
    public string BuildingNumber { get; set; }
    [DisplayName("Město")]
    public string CityName { get; set; }
    [DisplayName("PSČ")]
    public string ZipCode { get; set; }
    [DisplayName("E-mail")]
    public string Email { get; set; }
    [DisplayName("Telefonní číslo")]
    public string TelephoneNumber { get; set; }
}

