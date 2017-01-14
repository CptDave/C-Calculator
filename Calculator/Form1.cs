using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Calculator
{
    public enum Operator { PLUS, MINUS, DIVIDE, MULTIPLY };

    public partial class Form1 : Form
    {
        private double total = 0;

        public Form1()
        {
            InitializeComponent();
        }

        #region Modifier
        public void setTotal(double total)
        {
            this.total = total;
        }
        #endregion

        #region Button Listeners
        private void btnClear_Click(object sender, EventArgs e)
        {
            total = 0;
            tbDisplay.Clear();
            tbDisplay.Focus();
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            //tbDisplay.Clear();
            //tbDisplay.Text = total.ToString();
            //calcDisplay(tbDisplay.Text);
            perfomCalculation(calcDisplay(tbDisplay.Text), tbDisplay.Text);
            tbDisplay.Focus();
        }
        #endregion

        #region Calculation Methods
        /*
        *Find where the operations are in the string, and which operation they are, store them in a List 
        */
        private List<OperationPoint> calcDisplay(String str)
        {
            //read numbers, find +-/*, calculate, display
            List<OperationPoint> operatorLocation = new List<OperationPoint>();

            //read string and assign location to thier list
            for (int x = 0; x < str.Length; x++)
            {
                //find operators
                switch (str[x])
                {
                    case '+':
                        operatorLocation.Add(new OperationPoint(x, Operator.PLUS));
                        break;
                    case '-':
                        operatorLocation.Add(new OperationPoint(x, Operator.MINUS));
                        break;
                    case '*':
                        operatorLocation.Add(new OperationPoint(x, Operator.MULTIPLY));
                        break;
                    case '/':
                        operatorLocation.Add(new OperationPoint(x, Operator.DIVIDE));
                        break;
                    default:
                        break;
                }
            }

            //sort the list
            operatorLocation.Sort((x, y) => x.getPoint().CompareTo(y.getPoint()));

            //display list to console
            for (int x = 0; x < operatorLocation.Count; x++)
            {
                Console.WriteLine("Operation " + operatorLocation[x].getOperation() + " at point  " + operatorLocation[x].getPoint());
            }

            return operatorLocation;
        }

        /*
         * Main calculation method. Use list of operations and their locations to parse the string,
         * do the operation and store the result in total. This method finds the numbers within the string,
         * does the operation between the numbers, store the result then continue along with the next number
         * */
        public void perfomCalculation(List<OperationPoint> op, String str)
        {
            double numberHoldOne = 0, numberHoldTwo = 0;
            Boolean firstCalc = true;

            //the number of operation are the number of objects in op
            for (int x=0; x < op.Count; x++)
            {
                Console.WriteLine("Operation {0}", x+1);
                if (firstCalc)
                {
                    numberHoldOne = double.Parse(str.Substring(0, op[x].getPoint()));
                    firstCalc = false;
                    Console.WriteLine("Got first number " + numberHoldOne.ToString());
                }
                else
                {
                    numberHoldOne = total;
                    Console.WriteLine("Got first number " + numberHoldOne.ToString());
                }

                if (op.Count > (x + 1))//there are more operation to do
                {
                    /*
                    * SubString arguments:
                    * First = Grab index of first operation, + 1 to grab the beginning index of the second number
                    * Second = Grab index of second operation, - index of first operation, - 1 to grab length of second number 
                    */
                    numberHoldTwo = double.Parse(str.Substring(op[x].getPoint() + 1, op[x + 1].getPoint() - op[x].getPoint() - 1));
                    Console.WriteLine("Got second number " + numberHoldTwo.ToString());
                }
                else //there are no more points left
                {
                    numberHoldTwo = double.Parse(str.Substring(op[x].getPoint() + 1, ((str.Length - 1) - op[x].getPoint())));
                    Console.WriteLine("Got second number " + numberHoldTwo.ToString());
                }

                setTotal(doOperation(numberHoldOne, numberHoldTwo, op[x].getOperation()));
                Console.WriteLine("Total so far is {0}", total);

            }

            Console.WriteLine("Total is {0}", total);
            tbResult.Text = total.ToString();
        }
        #endregion

        #region Calculation Helper
        /*
         * Helper method for performCalculation. Takes two numbers and the operation,
         * performs is and returns it.
         * */
        public double doOperation(double x, double y, Operator op)
        {
            double t = 0;

            switch (op)
            {
                case Operator.PLUS:
                    t = x + y;
                    break;
                case Operator.MINUS:
                    t = x - y;
                    break;
                case Operator.MULTIPLY:
                    t = x * y;
                    break;
                case Operator.DIVIDE:
                    t = x / y;
                    break;
                default:
                    break;
            }

            return t;
        }
    }
    #endregion

        #region OperationPoint Class
    public class OperationPoint
    {
        private int point;
        private Operator operation;

        public OperationPoint(int x, Operator op)
        {
            setPoint(x);
            setOperation(op);
        }

        public void setPoint(int x)
        {
            point = x;
        }

        public void setOperation(Operator op)
        {
            operation = op;
        }

        public int getPoint()
        {
            return point;
        }

        public Operator getOperation()
        {
            return operation;
        }
    }
    #endregion
}
