using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeacherMAUI.Models;

namespace TeacherMAUI.ViewModels
{
    public class TestTabViewModel : BindableObject
    {
        private ObservableCollection<GroupedScheduleItems> _groupedScheduleItems;
        public ObservableCollection<GroupedScheduleItems> GroupedScheduleItems 
        {
            get => _groupedScheduleItems; //the result of the loadscheduleitems task below is the sorted 
            set
            {
                _groupedScheduleItems = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditItemCommand { get; private set; } //command that will load edit pages
        public ICommand DeleteItemCommand { get; private set; } //command that will delete items depending on type

        public TestTabViewModel() //establishing viewmodel for testtab
        {
            EditItemCommand = new Command<CombinedScheduleItem>(EditItem);//defining edit command instance for combined schedule item
            DeleteItemCommand = new Command<CombinedScheduleItem>(DeleteItem);//defining delete command instance for combined schedule item
        }

        public async Task LoadScheduleItems()
        {
            // Use Task.WhenAll to fetch data in parallel
            var efhmeriaTask = App.Database.GetEfhmeriasAsync();//connection to databasehelper for getefhmerias
            var exeiTask = App.Database.GetExeisAsync();//connection to databasehelper for getexeis

            await Task.WhenAll(efhmeriaTask, exeiTask);// whenall is used to update when items have been added to the list

            var efhmeriaItems = efhmeriaTask.Result;//efhmeriatask is the result of the efhmeria items in combinedschedgule items
            var exeiItems = exeiTask.Result;//exeitask is the result of the exei items in combinedschedgule items

            var orderedDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };//defining the order that days of the week are sorted

            var combinedItems = new List<CombinedScheduleItem>();//defining combineditems as a list

            combinedItems.AddRange(efhmeriaItems.Select(efhmeria => new CombinedScheduleItem//adding efhmerias to combineditems
            {
                Day = efhmeria.Day,
                Starts = efhmeria.Starts,
                Ends = efhmeria.Ends,
                Type = "Efhmeria",
                Details = efhmeria.Location,
                OriginalItem = efhmeria
            }));

            combinedItems.AddRange(exeiItems.Select(exei => new CombinedScheduleItem //adding exei to combineditems
            {
                Day = exei.Day,
                Starts = exei.Starts,
                Ends = exei.Ends,
                Type = "Exei",
                Details = $"{exei.Lesson} - {exei.Tmima}",
                OriginalItem = exei
            }));

            var groupedItems = combinedItems  //defining the grouped items as sorted combined items
                .GroupBy(item => item.Day)
                .Select(g => new GroupedScheduleItems(g.Key, g.OrderBy(item => item.Starts).ToList()))
                .OrderBy(g => Array.IndexOf(orderedDays, g.DayOfWeek))
                .ToList();

            GroupedScheduleItems = new ObservableCollection<GroupedScheduleItems>(groupedItems); //the observable collection is filled in with the sorted combined items

        }

     
        private async void EditItem(CombinedScheduleItem item) // edit item command is defined
        {
            Page editPage;
            if (item.Type == "Efhmeria")// if the type is an efhmeria the correct form opens up
            {
                editPage = new EfhmeriaEditPage((Efhmeria)item.OriginalItem);
            }
            else //if the type is Exei the lesson form opens up instead
            {
                editPage = new ExeiEditPage((Exei)item.OriginalItem);
            }

            await Application.Current.MainPage.Navigation.PushAsync(editPage);

            // Refresh the list after returning from the edit page
            await LoadScheduleItems();
        }

        private async void DeleteItem(CombinedScheduleItem item)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm Delete", $"Are you sure you want to delete this {item.Type} item?", "Yes", "No");//warning message
            if (confirm)//if the user chose to delete
            {
                try
                {
                    // Delete from database based on type
                    if (item.Type == "Efhmeria")//if the type is efhmeria the delete efhmeria task from database helper is called
                    {
                        await App.Database.DeleteEfhmeriaAsync((Efhmeria)item.OriginalItem);
                    }
                    else if (item.Type == "Exei")//if the type is exei the delete exei task from database helper is called
                    {
                        await App.Database.DeleteExeiAsync((Exei)item.OriginalItem);
                    }

             
                    await LoadScheduleItems();// Refresh the list after successful deletion

                    await Application.Current.MainPage.DisplayAlert("Delete", $"{item.Type} item deleted", "OK");//confirmed deletion popup
                }
                catch (Exception ex)//in case of error
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete item: {ex.Message}", "OK");//error popup failure of deletion
                }

              
            }
        }
    }
}
