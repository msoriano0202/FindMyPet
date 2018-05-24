using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace FindMyPet.MVC.IdentityExtentions
{
    //Micky
    public class CustomizePasswordValidation : IIdentityValidator<string>
    {
        public int LengthRequired { get; set; }

        public CustomizePasswordValidation(int length)
        {
            LengthRequired = length;
        }

        public Task<IdentityResult> ValidateAsync(string Item)
        {

            if (String.IsNullOrEmpty(Item) || Item.Length < LengthRequired)
            {
                return Task.FromResult(IdentityResult.Failed(String.Format("Minimum Password Length Required is:{0}", LengthRequired)));
            }

            string PasswordPattern = @"^(?=.*[0-9])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*0-9]{10,}$";
            if (!Regex.IsMatch(Item, PasswordPattern))
            {
                return Task.FromResult(IdentityResult.Failed(String.Format("The Password must have at least one numeric and one special character")));
            }

            return Task.FromResult(IdentityResult.Success);

        }
    }
}