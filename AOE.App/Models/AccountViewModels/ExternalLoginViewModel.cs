using System.ComponentModel.DataAnnotations;

namespace AOE.App.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
