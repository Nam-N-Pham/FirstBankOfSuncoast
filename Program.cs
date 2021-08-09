using System;
using System.IO;
using System.Collections.Generic;
using CsvHelper;
using System.Globalization;
using System.Linq;

namespace FirstBankOfSuncoast
{
    class Transaction
    {
        public string Account { get; set; }
        public string Action { get; set; }
        public int Amount { get; set; }

        public void showTransactionIfChecking()
        {
            if (Account == "checking")
            {
                Console.WriteLine($"{Action}ed ${Amount}");
            }
        }

        public void showTransactionIfSavings()
        {
            if (Account == "savings")
            {
                Console.WriteLine($"{Action}ed ${Amount}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Transaction> transactions = new List<Transaction>();

            if (File.Exists("transactions.csv"))
            {
                StreamReader fileReader = new StreamReader("transactions.csv");
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                transactions = csvReader.GetRecords<Transaction>().ToList();
                fileReader.Close();
            }

            // Setting up checking and savings accounts based on transaction input document
            int checkingAccount = 0;
            foreach (Transaction transaction in transactions)
            {
                if (transaction.Account == "checking" && transaction.Action == "deposit")
                {
                    checkingAccount = checkingAccount + transaction.Amount;
                }
                if (transaction.Account == "checking" && transaction.Action == "withdraw")
                {
                    checkingAccount = checkingAccount - transaction.Amount;
                }
            }
            if (checkingAccount < 0)
            {
                Console.WriteLine("Checking account is negative from input file, exiting program.");
                return;
            }

            int savingsAccount = 0;
            foreach (Transaction transaction in transactions)
            {
                if (transaction.Account == "savings" && transaction.Action == "deposit")
                {
                    savingsAccount = savingsAccount + transaction.Amount;
                }
                if (transaction.Account == "savings" && transaction.Action == "withdraw")
                {
                    savingsAccount = savingsAccount - transaction.Amount;
                }
            }
            if (savingsAccount < 0)
            {
                Console.WriteLine("Savings account is negative from input file, exiting program.");
                return;
            }

            Console.WriteLine("Welcome to First Bank of Suncoast!");

            bool runApp = true;
            while (runApp)
            {
                Console.WriteLine("Type the option you would like to do: Show Transactions, Show Balance, Deposit, Withdraw, or Quit.");
                string chosenOption = Console.ReadLine().ToLower();

                switch (chosenOption)
                {
                    case "quit":
                        runApp = false;
                        break;
                    case "show transactions":
                        Console.WriteLine("Which account would you like to see transactions of? Checking or Savings?");
                        string typeOfTransaction = Console.ReadLine().ToLower();
                        if (typeOfTransaction == "checking")
                        {
                            foreach (Transaction transaction in transactions)
                            {
                                transaction.showTransactionIfChecking();
                            }
                        }
                        if (typeOfTransaction == "savings")
                        {
                            foreach (Transaction transaction in transactions)
                            {
                                transaction.showTransactionIfSavings();
                            }
                        }
                        break;
                    case "show balance":
                        Console.WriteLine("Which account would you like to see the balance of? Checking or Savings?");
                        string accountChoice = Console.ReadLine().ToLower();
                        if (accountChoice == "checking")
                        {
                            Console.WriteLine($"The total amount in checking account is ${checkingAccount}");
                        }
                        if (accountChoice == "savings")
                        {
                            Console.WriteLine($"The total amount in savings account is ${savingsAccount}");
                        }
                        break;
                    case "deposit":
                        Console.WriteLine("Which account would you like to deposit to? Checking or Savings?");
                        string depositChoice = Console.ReadLine().ToLower();
                        Console.WriteLine("How much would you like to deposit?");
                        int depositAmount = int.Parse(Console.ReadLine());

                        if (depositAmount < 0)
                        {
                            Console.WriteLine("Invalid amount to deposit.");
                            break;
                        }

                        if (depositChoice == "checking")
                        {
                            checkingAccount = checkingAccount + depositAmount;

                            Transaction depositCheckingTransaction = new Transaction()
                            {
                                Account = "checking",
                                Action = "deposit",
                                Amount = depositAmount
                            };

                            transactions.Add(depositCheckingTransaction);

                            StreamWriter fileWriter = new StreamWriter("transactions.csv");
                            CsvWriter csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
                            csvWriter.WriteRecords(transactions);
                            fileWriter.Close();
                        }

                        if (depositChoice == "savings")
                        {
                            savingsAccount = savingsAccount + depositAmount;

                            Transaction depositSavingsTransaction = new Transaction()
                            {
                                Account = "savings",
                                Action = "deposit",
                                Amount = depositAmount
                            };

                            transactions.Add(depositSavingsTransaction);

                            StreamWriter fileWriter = new StreamWriter("transactions.csv");
                            CsvWriter csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
                            csvWriter.WriteRecords(transactions);
                            fileWriter.Close();
                        }
                        break;
                    case "withdraw":
                        Console.WriteLine("Which account would you like to withdraw from? Checking or Savings?");
                        string withdrawChoice = Console.ReadLine().ToLower();
                        Console.WriteLine("How much would you like to withdraw?");
                        int withdrawAmount = int.Parse(Console.ReadLine());

                        if (withdrawAmount < 0)
                        {
                            Console.WriteLine("Invalid amount to withdraw.");
                            break;
                        }

                        if (withdrawChoice == "checking")
                        {
                            if (checkingAccount - withdrawAmount < 0)
                            {
                                Console.WriteLine("Invalid withdrawl, not enough money");
                                break;
                            }
                            checkingAccount = checkingAccount - withdrawAmount;

                            Transaction withdrawCheckingTransaction = new Transaction()
                            {
                                Account = "checking",
                                Action = "withdraw",
                                Amount = withdrawAmount
                            };

                            transactions.Add(withdrawCheckingTransaction);

                            StreamWriter fileWriter = new StreamWriter("transactions.csv");
                            CsvWriter csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
                            csvWriter.WriteRecords(transactions);
                            fileWriter.Close();
                        }

                        if (withdrawChoice == "savings")
                        {
                            if (savingsAccount - withdrawAmount < 0)
                            {
                                Console.WriteLine("Invalid withdrawl, not enough money");
                                break;
                            }
                            savingsAccount = savingsAccount - withdrawAmount;

                            Transaction withdrawSavingsTransaction = new Transaction()
                            {
                                Account = "savings",
                                Action = "withdraw",
                                Amount = withdrawAmount
                            };

                            transactions.Add(withdrawSavingsTransaction);

                            StreamWriter fileWriter = new StreamWriter("transactions.csv");
                            CsvWriter csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
                            csvWriter.WriteRecords(transactions);
                            fileWriter.Close();
                        }
                        break;
                    default:
                        Console.WriteLine("Only enter the given options spelled correctly.");
                        break;
                }

            }
        }
    }
}
