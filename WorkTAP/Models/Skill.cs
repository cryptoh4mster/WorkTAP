using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WorkTAP.Models
{

    public class Skill
    {

        [Required(ErrorMessage = "Укажите название навыка")]
        [MaxLength(50,ErrorMessage ="Название навыка не должно превышать 50 символов")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Укажите уровень навыка")]
        [Range(1, 10, ErrorMessage = "Уровень навыка может быть в диапазоне от 1 до 10")]
        public byte Level { get; set; }
    }
}
