using System;
using System.Collections.Generic;
using System.Text;

namespace FinTracker_1.Models
{
    class Structs
    {
        public class ExpenseByMonth
        {
            public int month { get; set; }
            public int year { get; set; }
            public double amount { get; set; }
            public int periods { get; set; }
            public string userName { get; set; }
        }

        public class ExpenseMonthByCategory
        {
            public int month { get; set; }
            public int year { get; set; }
            public double amount { get; set; }
            public string categoryName { get; set; }
        }

        public class ExpenseMonthByPayment
        {
            public int month { get; set; }
            public int year { get; set; }
            public double amount { get; set; }
            public string paymentName { get; set; }
            public string bankName { get; set; }
        }
    }
}
