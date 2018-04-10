using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Multas_tB.Models
{
    public class Agentes{

        public Agentes()
        {
            ListaDeMultas = new HashSet<Multas>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required(ErrorMessage ="O {0} é de preenchimento obrigatório!")]
        [RegularExpression("[A-ZÍÉÂÁ][a-záéíóúàèìòùâêîôûäëïöüçãõ]+(( |'|-| dos | da | de | e | d')[A-ZÍÉÂÁ][a-záéíóúàèìòùâêîôûäëïöüçãõ]+){1,3}", ErrorMessage ="O{0} apenas pode conter letras e espaços em branco.Cada palavra começa em Maiuscula seguida de minuscula")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório!")]
        //[RegularExpression("[A-Z]*[a-záéíóúàèìòùâêîôûäëïöüãõç ]*", ErrorMessage = "O{0} apenas pode conter letras e espaços em branco.Cada palavra começa em Maiuscula seguida de minuscula")]
        public string Esquadra { get; set; }

        public string Fotografia { get; set; }

        // referencia às multas q um Agente 'faz'

        public virtual ICollection<Multas> ListaDeMultas { get; set; }
    }
}