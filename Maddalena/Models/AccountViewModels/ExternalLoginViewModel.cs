using System.ComponentModel.DataAnnotations;

namespace Maddalena.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required] public string Username { get; set; }
    }
}