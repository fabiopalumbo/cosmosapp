using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    /// <summary>
    /// COMOS informações de um usario
    /// </summary>
    public class CUserInfo
    {
        /// <summary>
        /// Nome de usario.
        /// </summary>
        public string Name;

        /// <summary>
        /// System UID de usario. 
        /// </summary>
        public string UserUID;

        /// <summary>
        /// System full name de empregador object de usario (Usario e Companhia objeto) no COMOS USERS projeto. 
        /// </summary>
        public string SystemFullName;

        /// <summary>
        /// Id da sessão atual do usuário. 
        /// </summary>
        public string SessionID;

        /// <summary>
        /// Indica se o usuário deve mudar a senha
        /// </summary>
        public bool ChangePassword;

    }
}
