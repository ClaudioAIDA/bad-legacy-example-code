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
    public class ExpressionStack
    {
        List<string> stackList;       

	    public ExpressionStack()
	    {
		    stackList = new List<string>();
	    }
    	
	    public void push(string pushVar)
	    {
		    stackList.Add(pushVar);
	    }

	    public string pop()
	    {
            string temp = stackList[stackList.Count - 1];
            stackList.Remove(stackList[stackList.Count - 1]);
		    return temp;
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
		    return stackList;
	    }

    }
}
