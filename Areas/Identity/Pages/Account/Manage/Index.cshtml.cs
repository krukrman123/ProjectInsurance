// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectInsurance.Areas.Identity.Data;

namespace ProjectInsurance.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
           /* [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }*/

           
            [Display(Name = "Jméno")]
            public string Name { get; set; }
            
            [Display(Name = "Příjmení")]
            public string LastName { get; set; }
           
            [Display(Name = "Email/Uživatelské jméno")]
            public string Email { get; set; }

            [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Zadejte devíticiferné telefonní číslo")]
            [Display(Name = "Telefonní číslo")]
            public string TelephoneNumber { get; set; }
          
            [Display(Name = "Ulice")]
            public string StreetName { get; set; }

            [RegularExpression(@"^\d*\/?\d+$", ErrorMessage = "Neplatné č.p.")]
            [Display(Name = "Číslo popisné")]
            public string BuildingNumber { get; set; }

            [RegularExpression(@"^\D+(\s\d+)?$", ErrorMessage = "Neplatný název")]
            [Display(Name = "Město")]
            public string CityName { get; set; }

            [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Zadejte pěticiferné PSČ")]
            [Display(Name = "PSČ")]
            public string ZipCode { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            /*var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;*/
           

            Input = new InputModel
            {
                //PhoneNumber = phoneNumber
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                TelephoneNumber = user.TelephoneNumber,
                StreetName = user.StreetName,
                BuildingNumber = user.BuildingNumber,
                CityName = user.CityName,
                ZipCode = user.ZipCode,
                
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var name = user.Name;
            var lastName = user.LastName;
            var email = user.Email;
            var telephoneNumber = user.TelephoneNumber;
            var streetName = user.StreetName;
            var buildingNumber = user.BuildingNumber;
            var cityName = user.CityName;
            var zipCode = user.ZipCode;
            /*if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }*/
            if (Input.Name != name)
            {
                user.Name = Input.Name;
                await _userManager.UpdateAsync(user);
            }
            
            if (Input.LastName != lastName)
            {
                user.LastName = Input.LastName;
                await _userManager.UpdateAsync(user);
            }
           
            
            if (Input.Email != email)
            {
                user.Email = Input.Email;
                await _userManager.UpdateAsync(user);
            }
            
            if (Input.TelephoneNumber != telephoneNumber)
            {
                user.TelephoneNumber = Input.TelephoneNumber;
                await _userManager.UpdateAsync(user);
            }
            
            if (Input.StreetName != streetName)
            {
                user.StreetName = Input.StreetName;
                await _userManager.UpdateAsync(user);
            }
            
            if (Input.BuildingNumber != buildingNumber)
            {
                user.BuildingNumber = Input.BuildingNumber;
                await _userManager.UpdateAsync(user);
            }
           
            if (Input.CityName != cityName)
            {
                user.CityName = Input.CityName;
                await _userManager.UpdateAsync(user);
            }
           
            if (Input.ZipCode != zipCode)
            {
                user.ZipCode = Input.ZipCode;
                await _userManager.UpdateAsync(user);
            }
           

            await _signInManager.RefreshSignInAsync(user);
            TempData["success"] = "Profil aktualizován.";
            //StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
