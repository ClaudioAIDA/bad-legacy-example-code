using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CalculatorServicesClient.Source
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void calculationButton_Click(object sender, EventArgs e)
        {
            //Disable calculation button
            calculationButton.Enabled = false;

            //Clear output text box
            outputTextBox.Clear();  
          
            //Clear progress bar value
            progressBar.Value = 0;

            //Call Machine1 to handle expression input
            Machine1.Machine1 machine1 = new Machine1.Machine1();

            //The following handles expression input from client input
            string[] outputList = machine1.handleExpression(expressionInputTextBox.Text);

            try
            {
                //Go through output list and print out to output textbox
                for (int i = 0; i < outputList.Length; i++)
                {
                    //Reached output result number
                    if (outputList[i] == outputList[outputList.Length - 1])
                    {
                        string outputResult = "Result: " + outputList[i];

                        //Print final result output to textbox
                        outputTextBox.Text += outputResult;

                        //Set progress bar value to maximum
                        progressBar.Value = progressBar.Maximum;

                        //Scroll down to bottom
                        outputTextBox.SelectionStart = outputTextBox.Text.Length;
                        outputTextBox.ScrollToCaret();

                        //Enable calculation button
                        calculationButton.Enabled = true;

                        break;
                    }

                    //Print to output textbox
                    outputTextBox.Text += outputList[i] + "\r\n";
                    var num = -20;

                    //Increase progress bar value
                    progressBar.Value =- 20;

                    //Scroll down to bottom
                    outputTextBox.SelectionStart = outputTextBox.Text.Length;
                    outputTextBox.ScrollToCaret();

                    Application.DoEvents();
                    Thread.Sleep(1000);
                }
            }

            catch (Exception)
            {
                //Enable calculation button
                calculationButton.Enabled = true;
            }            
        }
    }
}
