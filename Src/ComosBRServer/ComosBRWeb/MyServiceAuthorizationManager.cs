using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ComosBRWeb
{
    class MyServiceAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {

            HttpResponseMessageProperty prop = new HttpResponseMessageProperty();
            //prop.Headers.Add("Access-Control-Allow-Origin", "*");
            prop.Headers.Add("Cache-Control", "no-cache");
            prop.Headers.Add("Access-Control-Allow-Methods", "GET, POST");
            prop.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
            prop.Headers.Add("Access-Control-Max-Age", "1728000");
            

            operationContext.OutgoingMessageProperties.Add(HttpResponseMessageProperty.Name, prop);

            return true;
        }
    }
}