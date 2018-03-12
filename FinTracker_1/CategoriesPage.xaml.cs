using Android.Widget;
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
    public partial class CategoriesPage : ContentPage
    {
        private HttpClient _client;
        private Uri uri_subcategories, uri_categories, uri_categoryAdd, uri_subcategoryAdd;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Subcategory> _subcategories;
        private Category selectedCategory;
        private Subcategory selectedSubcategory;
       

        public CategoriesPage()
        {
            InitializeComponent();
            _client = new HttpClient();
            _client.MaxResponseContentBufferSize = 256000;
            uri_categories = new Uri(string.Format(Constants.Url_categoriesByUsername, string.Empty));
            uri_subcategories = new Uri(string.Format(Constants.Url_subcategoriesByUsername, string.Empty));
            uri_categoryAdd = new Uri(string.Format(Constants.Url_categoriesAdd, string.Empty));
            uri_subcategoryAdd = new Uri(string.Format(Constants.Url_subcategoriesAdd, string.Empty));
            selectedCategory    = null;
            selectedSubcategory = null;

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //Retrive categories information
            var response = await _client.GetStringAsync(uri_categories);


            if (response != null & response != string.Empty)
            {
                var categories = JsonConvert.DeserializeObject<List<Category>>(response);
                _categories = new ObservableCollection<Category>(categories);
                categoryListView.ItemsSource = _categories;
            }

            //Retrieve subcategories information
            var response_1 = await _client.GetStringAsync(uri_subcategories);


            if (response_1 != null & response_1 != string.Empty)
            {
                var subcategories = JsonConvert.DeserializeObject<List<Subcategory>>(response_1);
                _subcategories = new ObservableCollection<Subcategory>(subcategories);

            }

        }
       
        private async void saveCategoryButtonClicked()
        {
            //Retrieve value from entry cell
            String categoryName = categoryNameEntry.Text;
            System.Diagnostics.Debug.WriteLine("Add button Clicked:" + categoryName);

            //Validate entry
            if (categoryName == null || categoryName.Trim() == "")
            {
                await DisplayAlert("Alert", "Please enter category name", "OK");
                return;
            }

            //Create category instance
            var category = new Category
            {
                //TODO to update when login is implemented
                userId = 1,
                categoryId = 0,
                name = categoryName
            };

            //Create ArrayList instance
            ArrayList newCategoryList = newCategoryList = new ArrayList();
            newCategoryList.Add(category);

            //Store category in cloud thru web services
            var json = JsonConvert.SerializeObject(newCategoryList);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(uri_categoryAdd, content);

            //Deserialized  reponse from server
            var serviceContent = response.Content.ReadAsStringAsync().Result;
            var categoryList = JsonConvert.DeserializeObject<List<Category>>(serviceContent);
           

            System.Diagnostics.Debug.WriteLine("Status code:" + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                //Parse response list
                foreach (Category returnedCategory in categoryList)
                {
                    if (returnedCategory.categoryId != -1)
                    {
                        _categories.Add(returnedCategory);
                        await DisplayAlert("Alert", "Categories successfully added", "OK");
                    }
                    else if(returnedCategory.categoryId == -1)
                    {
                        await DisplayAlert("Alert", "Category already exists:"+returnedCategory.name, "OK");
                    }

                }              
            }
        }

       

        private async void saveSubcategoryButtonClicked()
        {
            //Get information from entry field
            String subcategoryName = subcategoryNameEntry.Text;
            
            //Validate user entry
            if (subcategoryName == null || subcategoryName.Trim() == "")
            {
                await DisplayAlert("Alert", "Please enter subcategory name", "OK");
                return;
            }

            if (selectedCategory != null)
            {     
                Subcategory subcategory = new Subcategory
                {
                    categoryId = selectedCategory.categoryId,
                    name = subcategoryName,
                    //TODO substitute userID with actual ID when login implemented
                    userId = 1
                };

                //Add subcategory instance to list to post it to web service
                ArrayList subcategoryList = new ArrayList();
                subcategoryList.Add(subcategory);

                // Store category in cloud thru web services
                var json = JsonConvert.SerializeObject(subcategoryList);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(uri_subcategoryAdd, content);

                //Deserialized  reponse from server
                var serviceContent = response.Content.ReadAsStringAsync().Result;
                var returnSubcategoryList = JsonConvert.DeserializeObject<List<Subcategory>>(serviceContent);

                System.Diagnostics.Debug.WriteLine("Status code:" + response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    //Clear observable collection
                    _subcategories.Clear();

                    //Parse response list
                    foreach (Subcategory returnedSubcategory in returnSubcategoryList)
                    {
                        if (returnedSubcategory.subcategoryId != -1)
                        {
                            _subcategories.Add(returnedSubcategory);
                            await DisplayAlert("Alert", "Subcategories successfully added", "OK");
                        }
                        else if (returnedSubcategory.subcategoryId == -1)
                        {
                            await DisplayAlert("Alert", "Subcategory already exists:" + returnedSubcategory.name, "OK");
                        }

                    }
                    //Add subcategory to list view
                    subcategoryListView.ItemsSource = _subcategories; 
                }

            }
            else
            {
                await DisplayAlert("Alert", "Please select a category", "OK");
            }
        }

        private void categorySelected()
        {
            selectedCategory = (Category)categoryListView.SelectedItem;
            int categoryId = selectedCategory.categoryId;
            ObservableCollection<Subcategory> _subcategoriesInCategory = new ObservableCollection<Subcategory>();

            //Loop thru observable collection of sub categories
            foreach (Subcategory subcategory in _subcategories)
            {
                if (categoryId == subcategory.categoryId)
                    _subcategoriesInCategory.Add(subcategory);
            }
            subcategoryListView.ItemsSource = _subcategoriesInCategory;
        }

        private void subcategorySelected()
        {
            //Get information from selected items
            selectedCategory     = (Category)categoryListView.SelectedItem;
            selectedSubcategory  = (Subcategory)subcategoryListView.SelectedItem;

            System.Diagnostics.Debug.WriteLine("Category selected:"+selectedCategory.name);
            System.Diagnostics.Debug.WriteLine("Subcategory selected:" + selectedSubcategory.name);
        }


    }
}