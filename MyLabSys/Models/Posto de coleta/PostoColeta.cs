﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLabSys.Models {
    [Table("PostosColetas")]
    public class PostoColeta {
        public int Id { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        [StringLength(255)]
        public string Endereco { get; set; }
    }
}