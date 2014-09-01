using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace CalculatorServices.WebServices
{
    [WebService(Namespace = "http://localhost/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]

    public class Machine3 : System.Web.Services.WebService
    {
        private double result;

        public Machine3()
        {
            result = 0;
        }

        //Web method for power operation
        [WebMethod]
        public double power(double numberOne, double numberTwo)
        {
            result = Math.Pow(numberOne, numberTwo);

            return result;
                
        }
    }
}
