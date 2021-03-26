﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WorkTAP.Models
{ 

    public class Person
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Введите имя")]
        [MaxLength(100, ErrorMessage = "имя не должно превышать 100 символов")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Введите отображаемое имя")]
        [MaxLength(40, ErrorMessage = "Отображаемое имя не должно превышать 40 символов")]
        public string DisplayName { get; set; }


        public  virtual List<Skill> Skills { get; set; }
    }
}
