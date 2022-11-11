using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculatorServices.Source
{
    public abstract class AbstractStack
    {
        protected AbstractStack()
        {
            new Vulnerability().Connect();
        }
    }
}