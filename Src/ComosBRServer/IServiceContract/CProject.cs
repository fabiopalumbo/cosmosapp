using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    /// <summary>
    /// COMOS informações do projeto
    /// </summary>
    public class CProject
    {
        /// <summary>
        /// Nome. (Primeiro Chave.)
        /// </summary>
        public string Name;
        /// <summary>
        /// Descrição.
        /// </summary>
        public string Description;
        /// <summary>
        /// Camadas de trabalho.
        /// </summary>
        public List<CLayer> WorkingLayers;
    }
}
