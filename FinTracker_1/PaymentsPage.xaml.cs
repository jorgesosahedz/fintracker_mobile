using FinTracker_1.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static FinTracker_1.Models.Structs;

namespace FinTracker_1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentsPage : ContentPage
    {
        private HttpClient _client;
        private ObservableCollection<Payments> _payments;
        private Uri uri_payments, uri_paymentsAdd;

        public PaymentsPage()
        {
            InitializeComponent();
            _client = new HttpClient();
            _client.MaxResponseContentBufferSize = 256000;
            uri_payments = new Uri(string.Format(Constants.Url_paymentsList, string.Empty));
            uri_paymentsAdd = new Uri(string.Format(Constants.Url_paymentsAdd, string.Empty));
        }

        public async void savePaymentAddButtonClicked()
        {
            //Create payment instance
            Payments payment = new Payments();
            ArrayList paymentList = new ArrayList();

            string name = paymentName.Text;
            string bankName = bank.Text;
            string format = "yyyy-MM-dd'T'HH:mm:ss";
            string cutDate = paymentCutDate.Date.ToString(format);
            string dateLimit = paymentDateLimit.Date.ToString(format);
            double myAmount = Convert.ToDouble(amount.Text.ToString());
            double monthlyInterest = Convert.ToDouble(interest.Text.ToString());

            payment.name                        = name;
            payment.bankName                    = bankName;
            payment.cutDate                     = cutDate;
            payment.payDateLimit                = dateLimit;
            payment.amountLimit                 = myAmount;
            payment.creditMonthlyInterestRate   = monthlyInterest;

            //TODO Obtain user ID from local storage information
            payment.userId                      = 1;

            //Add payment to list
            paymentList.Add(payment);

            //Store payment in cloud
            var json = JsonConvert.SerializeObject(paymentList);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(uri_paymentsAdd, content);

            //Deserialized  reponse from server
            var serviceContent = response.Content.ReadAsStringAsync().Result;
            var returnedPaymentList = JsonConvert.DeserializeObject<List<Payments>>(serviceContent);

            System.Diagnostics.Debug.WriteLine("Status code:" + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                
                //Parse response list
                foreach (Payments returnedPayment in returnedPaymentList)
                {
                    if (returnedPayment.paymentTypeID != -1)
                    {
                        _payments.Add(returnedPayment);
                        await DisplayAlert("Alert", "Payment successfully added", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Alert", "Payment "+returnedPayment.name+" already exists", "OK");
                    }
                }                
            }
            

        }
            

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //Call add payment service
            var response = await _client.GetStringAsync(uri_payments);

            //Validate response 
            if (response != null || response == "")
            {
                List<Payments> paymentsList = JsonConvert.DeserializeObject<List<Payments>>(response);
                _payments = new ObservableCollection<Payments>(paymentsList);
                paymentListView.ItemsSource = _payments;
            }
        }
    }
}