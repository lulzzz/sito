using System.ComponentModel.DataAnnotations;

namespace Maddalena.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}