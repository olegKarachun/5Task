using System.ComponentModel.DataAnnotations;

namespace _5Task.ViewModels
{
    public class NewGameModel
    {
        [Required(ErrorMessage = "Не указано название игры")]
        public string Name { get; set; }

        public string Tags { get; set; }
    }
}
