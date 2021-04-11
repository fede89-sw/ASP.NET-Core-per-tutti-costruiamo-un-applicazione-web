using System;
using MyCourse.Models.Enums;

namespace MyCourse.Models.ValueTypes
{
    public class Money
    {
        // classe con 2 proprietà: Amount e Currency
        public Money() : this(Currency.EUR, 0.00m)
        {
        }
        public Money(Currency currency, decimal amount)
        {
            Amount = amount;
            Currency = currency;
        }
        private decimal amount = 0;
        public decimal Amount
        { 
            // metodo per il valore di vendita del corso (usa decimal quando devi trattare prezzi )
            get
            {
                return amount;
            }
            set
            {
                if (value < 0) {
                    throw new InvalidOperationException("The amount cannot be negative");
                }
                amount = value;
            }
        }
        public Currency Currency
        {
            // rappresenta la valuta. Il tipo Currency da la possibilità di scegliere solo tra
            // le opzioni di quella classe. Se avessi usato una 'string' si poteva mettere ciò che si voleva
            get; set;
        }

        public override bool Equals(object obj)
        {
            var money = obj as Money;
            return money != null &&
                   Amount == money.Amount &&
                   Currency == money.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
        
        public override string ToString()
        {
            return $"{Currency} {Amount:#.00}";
        }
    }
}