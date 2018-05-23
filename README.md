# EthosLending
Uses amount of loan, interest, downpayment and term to compute monthy payment, total interest, and total payment.

This program prints output from several boundary test conditions to the screen.  After that
the user has the option to input their own data in the following format:
amount: 1200<br />
interest: 3.4%<br />
downpayment: 200<br />
term: 3<br />

Note that the last line is to remain blank for the input.  Amount is parsed as a decimal.
Interest is parsed as a decimal.  Downpayment is parsed as a decimal.  Term is parsed as
an integer.  So please provide input acccordingly.  The user can enter the interest as a
percent or as decimal, i.e. as 3.4% or as 3.4.  Do NOT enter the interest as a rate, such
as .034 instead.  This will be treated by the program as .034%.  Please note that you can 
use different capitalization (upper/lower case) for the words.

After the user enters the input, the program will execute and output
the JSON representing the MonthlyPayment, TotalInterest and TotalPayment.  The JSON is 
generated from the serialized object Output.  Note that before printing the JSON, the
TotalPayment, TotalInterest and MonthlyPayment values are rounded to two decimal places
using the MidpointRounding.ToEven mode.  
