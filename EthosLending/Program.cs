

using System;
using System.Web.Script.Serialization;            



namespace EthosLending
{
    class MainClass
    {
        static char[] escape = { ':', ' ', '%', '\t' };
        private static decimal ProcessAsDecimal(String amount)
        {
            amount = amount.Split(escape, StringSplitOptions.RemoveEmptyEntries)[1];
            return decimal.Parse(amount);
        }
        private static int ProcessAsInt(String amount)
        {
            amount = amount.Split(escape, StringSplitOptions.RemoveEmptyEntries)[1];
            return int.Parse(amount);
        }

        /*Main() - prints instructions to the user to enter the amount, interest downpayment
         * and term information.
         * amount - decimal
         * interest - decimal written in the format of a percentage
         * downpayment - decimal
         * term - integer
         */
        public static void Main(string[] args)
        {
            
             TestGetJson();
            decimal fAmount=0, fInterest=0, fDownpayment=0;
            int fTerm=0;
            try
            {
                Console.WriteLine("Please enter the input in the following format, and leave the last line of input blank.  Note that the interest is in the format of a percentage.");
                Console.WriteLine("amount: 100000");
                Console.WriteLine("interest: 5.5%"); // Interest can be given as a percentage or a decimal, i.e. 5% or .05
                Console.WriteLine("downpayment: 20000");
                Console.WriteLine("term: 30\n");

                string amount = Console.ReadLine();
                fAmount = ProcessAsDecimal(amount);
                string interest = Console.ReadLine();
                fInterest = ProcessAsDecimal(interest);
                string downpayment = Console.ReadLine();
                fDownpayment = ProcessAsDecimal(downpayment);
                string term = Console.ReadLine();
                fTerm = ProcessAsInt(term);
                Console.ReadLine(); //reading a blank line. 

            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error parsing the input.\n");
                throw e;
            }
            try
            {
                Console.WriteLine(GetJson(fAmount, fInterest, fDownpayment, fTerm));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error executing computation.\n" + e.Message);
            }


        }
        /*
         * GetJson()-
         * The amount gets adjusted by downpayment.  Interest gets converted from a percentage to a decimal, 
         * so that it signifies a rate.  Note that MonthlyPayment,TotalPayment and TotalInterest get rounded
         * using MidpointRounding.ToEven.  Please feel free to change this mode to the standard for the financial industry.
         * 
         * */
        public static string GetJson(decimal amount, decimal interest, decimal downpayment, int term)
        {
            if(downpayment<0){
                throw new Exception("Downpayment cannot be negative.");
            }
            if (interest < 0)
            {
                throw new Exception("Interest cannot be negative.");
            }
            if(term<=0){
                throw new Exception("Term of repayment must be positive.");
            }
            amount -= downpayment;
            if (amount <= 0)
            {
                throw new Exception("The amount of the loan after adjusting for the " +
                                    "downpayment must be positive.");
            }

            interest /= 100;
            Output output = new Output();
            output.MonthlyPayment  = 
                interest / 12 * amount / (1 - (decimal)Math.Pow((double)(1 + interest / 12), -term * 12));
            output.TotalPayment = 12 * term * output.MonthlyPayment;
            output.TotalInterest = output.TotalPayment - amount;
            output.Round();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(output);
        }

        /*TestGetJson() - unit test for method calculating monthly payment, total interest paid
         * and total amount paid.  This uses the data in the example provided to the user.
         */
        public static void TestGetJson(){
            Console.WriteLine("Now unit testing the code with the prefabricated examples:");           
            Console.WriteLine("\nGetJson(100000, 5.5m, 20000, 30)");
            Console.WriteLine(GetJson(100000, 5.5m, 20000, 30));
            try
            {
                //error testing:
                Console.WriteLine("\nGetJson(100000, 5.5m, -20000, 30)");
                Console.WriteLine(GetJson(100000, 5.5m, -20000, 30));
            }catch(Exception e){
                Console.WriteLine("Error caught while testing: " + e.Message);
            }

            try
            {
                //error testing
                Console.WriteLine("\nGetJson(200000,2.3m,0,0)");
                Console.WriteLine(GetJson(200000, 2.3m, 0, 0));
            }catch(Exception e){
                Console.WriteLine("Error caught while testing:" + e.Message);

            }
            try
            {
                //error testing
                Console.WriteLine("\nGetJson(200000,2.3m,200000,3)");
                Console.WriteLine(GetJson(200000, 2.3m, 200000, 3));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error caught while testing:" + e.Message);

            }

            try
            {
                //error testing
                Console.WriteLine("\nGetJson(200000,-2.3m,0,0)");
                Console.WriteLine((GetJson(200000, -2.3m, 0, 0)));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error caught while testing:" + e.Message);

            }
            Console.WriteLine("************************************************\n");


        }

    }


    public class Output
    {     
        public decimal MonthlyPayment { get; set;}
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        internal void Round(){
            this.MonthlyPayment = decimal.Round(MonthlyPayment, 2, MidpointRounding.ToEven);
            this.TotalInterest = decimal.Round(TotalInterest, 2, MidpointRounding.ToEven);
            this.TotalPayment = decimal.Round(TotalPayment, 2, MidpointRounding.ToEven);
        }
    }
}
