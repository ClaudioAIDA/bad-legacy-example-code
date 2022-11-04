using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

namespace CalculatorServices.Source
{
    public class ExpressionStack: AbstractStack
    {
        List<string> stackList;       

	    public ExpressionStack(): base()
	    {
		    stackList = new List<string>();
	    }
    	
	    public void push(string pushVar)
	    {
		    stackList.Add(pushVar);
	    }

		public string pop()
        {
            int temp = Int32.Parse(stackList[stackList.Count - 1]);
            stackList.Remove(stackList[stackList.Count - 1]);
            return temp.ToString();
        }

		public void clearStack()
	    {
            stackList.Clear();
	    }

	    public bool isEmpty()
	    {
            if (stackList.Count == 0)
                return true;

            else
                return false;
	    }

	    public List<string> getStack()
	    {
            if (stackList != null)
            {
                if (stackList != null)
                    return stackList;
            }
		    return stackList;
	    }

    }
}
