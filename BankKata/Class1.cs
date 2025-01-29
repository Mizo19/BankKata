using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankKata
{
    public interface AccountService
    {
        void Deposit(int amount);
        void Withdraw(int amount);
        void PrintStatement();
    }

    public class Account : AccountService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private List<Transaction> _transactions = new List<Transaction>();
        private int _currentBalance = 0; // Pour Balance
                                         // Constructor now takes IDateTimeProvider
        public Account(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;

        }
        public void Deposit(int amount)
        {
            var transactionDate = _dateTimeProvider.Now; // Use Clock for current date
            _currentBalance += amount;
            _transactions.Add(new Transaction(amount, _currentBalance, transactionDate));
        }

        public void Withdraw(int amount)
        {
            var transactionDate = _dateTimeProvider.Now;  //Clock pour la date actuel 
            _currentBalance -= amount;
            _transactions.Add(new Transaction(-amount, _currentBalance, transactionDate));
        }

        public void PrintStatement()
        {
            // Headers 
            Console.WriteLine("Date       || Amount || Balance");

            // Print all transactions in reverse order (latest transaction first)
            foreach (var transaction in _transactions)
            {
                // Print each transaction's date, amount, and the balance at that time
                Console.WriteLine($"{transaction.Date:dd/MM/yyyy} || {transaction.Amount,7} || {transaction.Balance,7}");
            }
        }
    }
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
 
    // The Transaction class
    public class Transaction
    {
        public int Amount { get; }
        public int Balance { get; }
        public DateTime Date { get; }

        //Mofification de la date pour le Test unitaire 
        public Transaction(int amount, int balance, DateTime date)
        {
            Amount = amount;
            Balance = balance;
            Date = date ; // Si aucun date specifier Utiliser la date actuel 
        }
    }

}
