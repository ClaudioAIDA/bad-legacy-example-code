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

    public class Machine2 : System.Web.Services.WebService
    {
        private double result;

        public Machine2()
        {
            result = 0;
        }

        //Web method for multiplication operation
        [WebMethod]
        public double multiply(double numberOne, double numberTwo)
        {
            result = numberOne * numberTwo;

            return result;
        }

        //Web method for division operation
        [WebMethod]
        public double divide(double numberOne, double numberTwo)
        {
            double result = numberOne / numberTwo;

            return result;
        }
    }
}
