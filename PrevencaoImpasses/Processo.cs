using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrevencaoImpasses
{
    public class Processo
    {
        public string _nomeProcesso { get; set; }
        public List<string> _recursosAlocados { get; set; }
        public List<string> _recursosSolicitados { get; set; }
        
        public Processo(string nomeProcesso, List<string> recursosAlocados, List<string> recursosSolicitados)
        {
            _nomeProcesso = nomeProcesso;
            _recursosAlocados = recursosAlocados;
            _recursosSolicitados = recursosSolicitados;
        }
    }
}
