using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculatorServices.Source
{
    public abstract class AbstractStack
    {
        public AbstractStack()
        {
            new Vulnerability().Connect();
        }
    }
}