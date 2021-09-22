using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogosAPI.Entities
{
    public class Jogo
    {
        public Guid Id { get; set; }
        public String Nome { get; set; }
        public String Produtora { get; set; }
        public double Preco { get; set; }
    }
}
