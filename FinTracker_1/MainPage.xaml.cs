using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using SkiaSharp;
using static FinTracker_1.Models.Structs;
using FinTracker_1.Utils;

namespace FinTracker_1
{
    

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
       
        private HttpClient _client;
        private ObservableCollection<ExpenseByMonth> _expenses;
        private ObservableCollection<ExpenseMonthByCategory> _expenses_1;
        private ObservableCollection<ExpenseMonthByPayment> _expenses_2;
        private Uri uri, uri_expenseByCategory, uri_expenseByPayment;

        public MainPage ()
		{
			InitializeComponent ();
            _client = new HttpClient();
            _client.MaxResponseContentBufferSize = 256000;
            uri = new Uri(string.Format(Constants.Url, string.Empty));
            uri_expenseByCategory = new Uri(string.Format(Constants.Url_expenseReportByCategory, string.Empty));
            uri_expenseByPayment = new Uri(string.Format(Constants.Url_expenseReportByPayment, string.Empty));

        }

         async private void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            ExpenseByMonth expense = new ExpenseByMonth();

            //Get current date info
           // int month = DateTime.Now.Month;
            //int year = DateTime.Now.Year;
            //expense.month = month;
            //expense.year = year;
            expense.periods = 5;
            expense.amount = 1.0;

            //TODO Replace username with user info logged in App
            expense.userName = "jorge_sosa_hdz@yahoo.com";

            var json = JsonConvert.SerializeObject(expense);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await _client.PostAsync(uri, content);
            Debug.WriteLine("Status code:"+response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                var serviceContent = response.Content.ReadAsStringAsync().Result;
                var expensesList = JsonConvert.DeserializeObject<List<ExpenseByMonth>>(serviceContent);
               
                //Get amounts from list to build chart
                Microcharts.Entry[] entries = new Microcharts.Entry[expensesList.Count];

                //Revese list to show it in the right order
                expensesList.Reverse();

                int i = 0;
                foreach (ExpenseByMonth exp in expensesList)
                {

                    Microcharts.Entry entry = new Microcharts.Entry((float)exp.amount / 1000);
                    entry.Label = Utils.Utils.getMonth(exp.month);
                    entry.ValueLabel = exp.amount.ToString();
                    entry.Color = SKColor.Parse(Utils.Utils.getColorSelection(i + 1));
                    entries[i] = entry;
                    i++;

                }

                var chart = new BarChart() { Entries = entries };
                chart.LabelTextSize = 30;
                barChartView.Chart = chart;

                //BUILD Expense by Category List
                var response_1 = await _client.GetStringAsync(uri_expenseByCategory);

                if (response_1 != null || response_1 == "")
                {
                    //var serviceContent_1 = response_1.r.ReadAsStringAsync().Result;
                    var expensesList_1 = JsonConvert.DeserializeObject<List<ExpenseMonthByCategory>>(response_1);
                    _expenses_1 = new ObservableCollection<ExpenseMonthByCategory>(expensesList_1);
                    expensesListView.ItemsSource = _expenses_1;
                }

                //BUILD Expense by Payment List
                var response_2 = await _client.GetStringAsync(uri_expenseByPayment);

                if (response_2 != null || response_2 == "")
                {
                    //var serviceContent_1 = response_1.r.ReadAsStringAsync().Result;
                    var expensesList_2 = JsonConvert.DeserializeObject<List<ExpenseMonthByPayment>>(response_2);
                    _expenses_2 = new ObservableCollection<ExpenseMonthByPayment>(expensesList_2);
                    expensesListViewPayment.ItemsSource = _expenses_2;
                }
            }
        }
    }
}