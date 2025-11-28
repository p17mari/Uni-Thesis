using TeacherMAUI.Models;


namespace TeacherMAUI
{
    public partial class AdditionEfhmeriaPage : ContentPage
    {
        public AdditionEfhmeriaPage()
        {
            InitializeComponent();
        }

        async void OnChaperonClicked(object sender, EventArgs e)//does the commands in the brackets when onlessonclicked button got clicked 
        {
            if (!string.IsNullOrWhiteSpace(locationEntry.Text) && !(startchaperonEntry == null) && !(endchaperonEntry == null) && !(dayEntry == null))
            {
                TimeSpan timeEnds = endchaperonEntry.Time;//saving endchaperonentry in timeEnds
                TimeSpan timeStarts = startchaperonEntry.Time;//saving startchaperonentry in timeStarts
                string selectedDay = (string)dayEntry.SelectedItem;//saving dayEntry in selectedDay

                await App.Database.SaveEfhmeriaAsync(new Efhmeria  //calling SaveEfhmeria for insertion of codebinded input
                {
                    Location = locationEntry.Text,//inserting the location where the chaperoning takes place
                    Starts = new DateTime(1, 1, 1, timeStarts.Hours, timeStarts.Minutes, timeStarts.Seconds),//inserting the starting time the chaperoning starts
                    Ends = new DateTime(1, 1, 1, timeEnds.Hours, timeEnds.Minutes, timeEnds.Seconds),//inserting the closing time the chaperoning ends
                    Day = selectedDay//inserting the day the chaperoning ends
                });
                // After successful submission, reset the form
                ResetForm();

                // Dispaly alert
                await DisplayAlert("Success", "Chaperon saved", "OK");

            }
        }

        private void ResetForm()//reset form after adding items
        {
            // Clear the location entry
            locationEntry.Text = string.Empty;

            // Reset the day picker
            dayEntry.SelectedIndex = -1;

            // Reset the time pickers
            startchaperonEntry.Time = new TimeSpan(0, 0, 0);
            endchaperonEntry.Time = new TimeSpan(0, 0, 0);

            // Reset scroll position
            formScrollView.ScrollToAsync(0, 0, true);
        }
    }

}
