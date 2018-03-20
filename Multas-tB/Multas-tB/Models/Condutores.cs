﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas_tB.Models
{
    public class Condutores{

        public Condutores()
        {
            ListaDeMultas = new HashSet<Multas>();
        }
        [Key]
        public int ID { get; set; }

        public string Nome { get; set; }

        public string BI { get; set; }

        public string Telemovel { get; set; }

        public DateTime DataNascimento { get; set; }

        public string NumCartaConducao { get; set; }

        public string LocaEmissao { get; set; }

        public DateTime DataValidadeCarta { get; set; }

        // referencia às multas q um condutor 'recebe'

        public virtual ICollection<Multas> ListaDeMultas { get; set; }


    }
}