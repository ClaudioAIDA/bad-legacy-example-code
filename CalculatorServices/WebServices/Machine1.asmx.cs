using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;

namespace CalculatorServices.WebServices
{
    [WebService(Namespace = "http://localhost/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]

    public class Machine1 : System.Web.Services.WebService
    {
        //Declare machine2 and machine3
        public Machine2 machine2;
        public Machine3 machine3;

        //Declare output list that will be returned to client once calculation is complete
        private List<string> outputList;

        public Machine1()
        {
            //Initialize machine2 and machine3
            machine2 = new Machine2();
            machine3 = new Machine3();

            //Initialize output list
            outputList = new List<string>();
        }

        //Web method that transforms expression into postfix form and parses expression accordingly
        [WebMethod]
        public List<string> handleExpression(string expressionInput)
        {
            if(expressionInput == "")
            {
                MessageBox.Show("Expression input must not be empty!", "Invalid expression", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }

            //Add to output list
            outputList.Add("Machine 1 transforms expression into postfix....");

            //Transform expression into postfix form
            List<string> postfixExpression = transformToPostfix(expressionInput);

            //Calculation list for calculating postfix expression
            List<string> calculationList = new List<string>();            

            //Fill calculation list with postfix expression
            for (int i = 0; i < postfixExpression.Count; i++)
                calculationList.Add(postfixExpression[i].ToString());

            //Parse calculation list and calculate accordingly
            return parse(calculationList);           

        }

        //Method for transforming expression into postfix form
        public List<string> transformToPostfix(string expressionInput)
        {
            string currentNumber = "";

            //Initialize stacks
            Source.ExpressionStack postFixStack = new Source.ExpressionStack();
            Source.ExpressionStack tempStack = new Source.ExpressionStack();

            //Priority table for operators
            string[,] priorityTable = new string[3, 7];

            //Operators
            priorityTable[0, 0] = "+";
            priorityTable[0, 1] = "-";
            priorityTable[0, 2] = "*";
            priorityTable[0, 3] = "/";
            priorityTable[0, 4] = "^";
            priorityTable[0, 5] = "(";
            priorityTable[0, 6] = ")";

            //Operator priorities for temporary stack 
            priorityTable[1, 0] = "1";
            priorityTable[1, 1] = "1";
            priorityTable[1, 2] = "2";
            priorityTable[1, 3] = "2";
            priorityTable[1, 4] = "3";
            priorityTable[1, 5] = "0";
            priorityTable[1, 6] = "4";

            //Operator priorities for incoming operator while parsing expression
            priorityTable[2, 0] = "1";
            priorityTable[2, 1] = "1";
            priorityTable[2, 2] = "2";
            priorityTable[2, 3] = "2";
            priorityTable[2, 4] = "3";
            priorityTable[2, 5] = "4";
            priorityTable[2, 6] = "5";

            bool negativeNumberException = false;

            //Parse expression
            for (int currentIndex = 0; currentIndex < expressionInput.Length; currentIndex++)
            {
                if (negativeNumberException)
                {
                    currentNumber = expressionInput[currentIndex].ToString();
                    negativeNumberException = false;
                }
                else
                {

                    if (currentIndex == 0 && expressionInput[currentIndex].ToString() == "-")
                    {
                        currentNumber = expressionInput[currentIndex].ToString();
                    }
                    else
                    {
                        //Expression element is a number
                        if (expressionInput[currentIndex].ToString() != "+" && expressionInput[currentIndex].ToString() != "-" && expressionInput[currentIndex].ToString() != "*" && expressionInput[currentIndex].ToString() != "/" && expressionInput[currentIndex].ToString() != "^" && expressionInput[currentIndex].ToString() != "(" && expressionInput[currentIndex].ToString() != ")")
                            currentNumber += expressionInput[currentIndex];

                        //Expression element is an operator
                        else
                        {
                            if (currentIndex + 1 != expressionInput.Length)
                            {
                                if (expressionInput[currentIndex + 1].ToString() == "-" || (expressionInput[currentIndex].ToString() == "(" && expressionInput[currentIndex + 1].ToString() == "-"))
                                {
                                    negativeNumberException = true;
                                }
                            }
                            if (expressionInput[currentIndex].ToString() != "+" && expressionInput[currentIndex].ToString() != "-" && expressionInput[currentIndex].ToString() != "*" && expressionInput[currentIndex].ToString() != "/" && expressionInput[currentIndex].ToString() != "^" && expressionInput[currentIndex].ToString() != "(" && expressionInput[currentIndex].ToString() != ")")
                            {
                                //expression error, break
                                break;
                            }
                            if (currentNumber != "")
                            {
                                //Push number into postfix stack and clear current number value
                                postFixStack.push(currentNumber);
                                currentNumber = "";
                            }

                            //If temporary stack is empty, then push current operator into it
                            if (tempStack.isEmpty())
                                tempStack.push(expressionInput[currentIndex].ToString());

                            //If temporary stack is not empty, call recursive method that compares operator priorities and pushes/pops accordingly
                            else
                            {
                                if (compareAndPushPop(tempStack, currentIndex, expressionInput, postFixStack, priorityTable))
                                    tempStack.push(expressionInput[currentIndex].ToString());
                            }

                        }
                    }
                }
            }


            //If current number is not empty, then push it into postfix stack
            if (!currentNumber.Equals(""))
                postFixStack.push(currentNumber);

            //While temporary stack is not empty, pop everything from temporary stack and push into postfix stack
            while (!tempStack.isEmpty())
                postFixStack.push(tempStack.pop());

            //Return completed postfix stack
            return postFixStack.getStack();
	    }

        //Recursive method that helps in transforming expression into postfix form
        public bool compareAndPushPop(Source.ExpressionStack tempStack, int currentIndex, string expressionInput, Source.ExpressionStack postFixStack, string[,] priorityTable)
	    {
            //Operator compare strings
            string compareOperandOne = tempStack.pop();
            string compareOperandTwo = expressionInput[currentIndex].ToString();

            //Flag for fruther recursive testing on expression
            bool furtherTesting = false;
            int priorityOne = 0, priorityTwo = 0;

            //Set priorities
            for (int i = 0; i < 7; i++)
            {
                if (compareOperandOne.Equals(priorityTable[0, i]))
                    priorityOne = Convert.ToInt32(priorityTable[1, i]);

                if (compareOperandTwo.Equals(priorityTable[0, i]))
                    priorityTwo = Convert.ToInt32(priorityTable[2, i]);
            }
            if (compareOperandOne == "(" && compareOperandTwo == ")")
                return furtherTesting;

            //Priority checks
            if (priorityOne >= priorityTwo)
            {
                postFixStack.push(compareOperandOne);

                furtherTesting = true;
            }
            else
            {
                if (compareOperandTwo.Equals(")"))
                {
                    //Push into stack
                    postFixStack.push(compareOperandOne);

                    //Flag that helps computing operators which are inside the parenthesis
                    bool operatorsInsideParenthesis = true;

                    string tempString = "";

                    while (operatorsInsideParenthesis)
                    {
                        tempString = tempStack.pop();

                        //Push the operators that are inside parenthesis into postfix stack
                        if (!tempString.Equals("("))
                            postFixStack.push(tempString);

                        else
                            operatorsInsideParenthesis = false;
                    }

                    furtherTesting = false;
                }

                else
                {
                    tempStack.push(compareOperandOne);
                    tempStack.push(compareOperandTwo);
                    furtherTesting = false;
                }
            }

            //If temporary stack is not empty and we are still testing operator priorities, re-call method (recursion)
            if (!tempStack.isEmpty() && furtherTesting)
            {
                compareAndPushPop(tempStack,currentIndex, expressionInput, postFixStack, priorityTable);
                //furtherTesting = false;
            }
            return furtherTesting;
	    }

        //Method for parsing and calculating postfix expression
        public List<string> parse(List<string> calculationList)
        {
            if (calculationList.Count == 1)
            {
                MessageBox.Show("Invalid expression input! Please check expression and try again.", "Invalid expression", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }

            //Temporary double values for expression calculation (used for subtituting)
            double temp = 0;
            double temp2 = 0;

            try
            {
                int n = 0;

                //Traverse and calculate the calculation list
                while (calculationList.Count != 0)
                {
                    //Calculation finished
                    if (calculationList.Count == 1)
                        break;

                    if (calculationList[n].ToString().Equals("+"))
                    {
                        //Add previous two numbers
                        temp = add(double.Parse(calculationList[n - 2], CultureInfo.InvariantCulture), double.Parse(calculationList[n - 1], CultureInfo.InvariantCulture));

                        //Remove from calculation list
                        calculationList.RemoveAt(n - 1);
                        calculationList.RemoveAt(n - 1);

                        //Decrement n
                        n--;

                        //We have reached the end of the list
                        if (n == 1)
                        {
                            //Update position in calculation list with new value
                            temp2 = double.Parse(calculationList[n - 1], CultureInfo.InvariantCulture);
                            temp2 = temp;
                            calculationList[n - 1] = temp2.ToString();
                        }

                        else
                        {
                            //Update position in calculation list with new value
                            temp2 = double.Parse(calculationList[n - 2], CultureInfo.InvariantCulture);
                            temp2 = temp;
                            calculationList[n - 1] = temp2.ToString();
                        }

                        continue;
                    }

                    if (calculationList[n].ToString().Equals("-"))
                    {
                        //Subtract previous two numbers
                        temp = subtract(double.Parse(calculationList[n - 2], CultureInfo.InvariantCulture), double.Parse(calculationList[n - 1], CultureInfo.InvariantCulture));

                        //Remove from calculation list
                        calculationList.RemoveAt(n - 1);
                        calculationList.RemoveAt(n - 1);

                        //Decrement n
                        n--;

                        //We have reached the end of the list
                        if (n == 1)
                        {
                            //Update position in calculation list with new value
                            temp2 = double.Parse(calculationList[n - 1], CultureInfo.InvariantCulture);
                            temp2 = temp;
                            calculationList[n - 1] = temp2.ToString();
                        }

                        else
                        {
                            //Update position in calculation list with new value
                            temp2 = double.Parse(calculationList[n - 2], CultureInfo.InvariantCulture);
                            temp2 = temp;
                            calculationList[n - 1] = temp2.ToString();
                        }

                        continue;
                    }

                    if (calculationList[n].ToString().Equals("*"))
                    {
                        //Call Machine2 to multiply previous two numbers
                        temp = machine2.multiply(double.Parse(calculationList[n - 2], CultureInfo.InvariantCulture), double.Parse(calculationList[n - 1], CultureInfo.InvariantCulture));
                        
                        //Add to output list
                        outputList.Add("Machine 1 distributes work to Machine 2.");

                        //Remove from calculation list
                        calculationList.RemoveAt(n - 1);
                        calculationList.RemoveAt(n - 1);

                        //Decrement n
                        n--;

                        //We have reached the end of the list
                        if (n == 1)
                        {
                            //Update position in calculation list with new value
                            temp2 = double.Parse(calculationList[n - 1], CultureInfo.InvariantCulture);
                            temp2 = temp;
                            calculationList[n - 1] = temp2.ToString();
                        }

                        else
                        {
                            //Update position in calculation list with new value
                            temp2 = double.Parse(calculationList[n - 2], CultureInfo.InvariantCulture);
                            temp2 = temp;
                            calculationList[n - 1] = temp2.ToString();
                        }

                        continue;
                    }

                    if (calculationList[n].ToString().Equals("/"))
                    {
                        //Call Machine2 to divide previous two numbers
                        temp = machine2.divide(double.Parse(calculationList[n - 2], CultureInfo.InvariantCulture), double.Parse(calculationList[n - 1], CultureInfo.InvariantCulture));

                        //Add to output list
                        outputList.Add("Machine 1 distributes work to Machine 2.");

                        //Remove from calculation list
                        calculationList.RemoveAt(n - 1);
                        calculationList.RemoveAt(n - 1);

                        //Decrement n
                        n--;

                        //We have reached the end of the list
                        if (n == 1)
                        {
                            //Update position in calculation list with new value
                            temp2 = double.Parse(calculationList[n - 1], CultureInfo.InvariantCulture);
                            temp2 = temp;
                            calculationList[n - 1] = temp2.ToString();
                        }

                        else
                        {
                            //Update position in calculation list with new value
                            temp2 = double.Parse(calculationList[n - 2], CultureInfo.InvariantCulture);
                            temp2 = temp;
                            calculationList[n - 1] = temp2.ToString();
                        }

                        continue;
                    }

                    if (calculationList[n].ToString().Equals("^"))
                    {
                        //Call Machine3 to power previous two numbers
                        temp = machine3.power(double.Parse(calculationList[n - 2], CultureInfo.InvariantCulture), double.Parse(calculationList[n - 1], CultureInfo.InvariantCulture));

                        //Add to output list
                        outputList.Add("Machine 1 distributes work to Machine 3.");

                        //Remove from calculation list
                        calculationList.RemoveAt(n - 1);
                        calculationList.RemoveAt(n - 1);

                        //Decrement n
                        n--;

                        //We have reached the end of the list
                        if (n == 1)
                        {
                            //Update position in calculation list with new value
                            temp2 = double.Parse(calculationList[n - 1], CultureInfo.InvariantCulture);
                            temp2 = temp;
                            calculationList[n - 1] = temp2.ToString();
                        }

                        else
                        {
                            //Update position in calculation list with new value
                            temp2 = double.Parse(calculationList[n - 2], CultureInfo.InvariantCulture);
                            temp2 = temp;
                            calculationList[n - 1] = temp2.ToString();
                        }

                        continue;
                    }

                    n++;
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Invalid expression input! Please check expression and try again.", "Invalid expression", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }

            //Add output result to output list
            outputList.Add(calculationList[0].ToString());         

            return outputList;
        }

        //Method for addition operation
        public double add(double numberOne, double numberTwo)
        {
            double result = 0;

            result = numberOne + numberTwo;

            return result;
        }

        //Method for substraction operation
        public double subtract(double numberOne, double numberTwo)
        {
            double result = 0;

            result = numberOne - numberTwo;

            return result;
        }
    }
}
