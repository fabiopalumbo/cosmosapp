using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    public class CLayer
    {
        /// <summary>
        /// Nome.
        /// </summary>
        public string Name;
        /// <summary>
        /// Descrição.
        /// </summary>
        public string Description;
        /// <summary>
        /// Identificação de um Camadas de trabalho. (Primeiro Chave)
        /// </summary>
        public int ID;
        /// <summary>
        /// Camadas de trabalho.
        /// </summary>
        public List<CLayer> WorkingLayers;
    }
}
