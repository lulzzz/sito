using System.ComponentModel.DataAnnotations;

namespace MatteoFabbri.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}