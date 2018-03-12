using System;
using System.Collections.Generic;
using System.Text;

namespace FinTracker_1.Utils
{
    class Constants
    {
        public static string Url = 
       "http://10.0.2.2:8080/fintracker/rest/fintrackerservices/report/expensebymonth/";

        public static string Url_expenseReportByCategory =
        "http://10.0.2.2:8080/fintracker/rest/fintrackerservices/report/expenselastmonthbycategory/jorge_sosa_hdz@yahoo.com";

        public static string Url_expenseReportByPayment =
        "http://10.0.2.2:8080/fintracker/rest/fintrackerservices/report/expenselastmonthbypayment/jorge_sosa_hdz@yahoo.com";

        public static string Url_subcategoriesByUsername =
        "http://10.0.2.2:8080/fintracker/rest/fintrackerservices/subcategory/list/jorge_sosa_hdz@yahoo.com";

        public static string Url_categoriesByUsername =
        "http://10.0.2.2:8080/fintracker/rest/fintrackerservices/category/list/jorge_sosa_hdz@yahoo.com";

        public static string Url_categoriesAdd =
        "http://10.0.2.2:8080/fintracker/rest/fintrackerservices/category/add";

        public static string Url_subcategoriesAdd =
        "http://10.0.2.2:8080/fintracker/rest/fintrackerservices/subcategory/add";

        //TODO Change to user ID dynamically
        public static string Url_paymentsList =
        "http://10.0.2.2:8080/fintracker/rest/fintrackerservices/paymenttypes/list/jorge_sosa_hdz@yahoo.com";

        public static string Url_paymentsAdd =
        "http://10.0.2.2:8080/fintracker/rest/fintrackerservices/paymenttypes/add/";

    }
}
