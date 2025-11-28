using TeacherMAUI.Models;
using TeacherMAUI.Services;


namespace TeacherMAUI
{
    public partial class AdditionExeiPage : ContentPage
    {
          public AdditionExeiPage()
        {
            InitializeComponent();
        }

        async void OnLessonClicked(object sender, EventArgs e)//does the commands in the brackets when onlessonclicked button got clicked
        {
            if (!string.IsNullOrWhiteSpace(tmimaEntry.Text) && !(startchaperonEntry == null) && !(endchaperonEntry == null) && !(dayEntry == null))//does the code in the brackets if the input is not null
            {
                TimeSpan timeEnds = endchaperonEntry.Time;//saving endclessonentry in timeEnds
                TimeSpan timeStarts = startchaperonEntry.Time;//saving startlessonentry in timeStarts
                string selectedDay = (string)dayEntry.SelectedItem;//saving dayEntry in selectedDay

                await App.Database.SaveExeiAsync(new Exei //calling SaveExei for insertion of codebinded input
                {
                    Tmima = tmimaEntry.Text,//inserting the Tmima the class is being taught
                    Lesson = lessonEntry.Text,//inserting the Lesson the class is being taught
                    Starts = new DateTime(1, 1, 1, timeStarts.Hours, timeStarts.Minutes, timeStarts.Seconds),//inserting the starting time the class is being taught
                    Ends = new DateTime(1, 1, 1, timeEnds.Hours, timeEnds.Minutes, timeEnds.Seconds),//inserting the closing time the class is being taught
                    Day = selectedDay//inserting the day of the week the class is being taught

                });

                // After successful submission, reset the form
                ResetForm();

                // Display alert
                await DisplayAlert("Success", "Lesson saved", "OK");
            }
        }

        private void ResetForm()//reset form after adding item
        {
            // Clear the location entry
            lessonEntry.Text = string.Empty;
            tmimaEntry.Text = string.Empty;

            // Reset the day picker
            dayEntry.SelectedIndex = -1;

            // Reset the time pickers
            startchaperonEntry.Time = new TimeSpan(0, 0, 0);
            endchaperonEntry.Time = new TimeSpan(0, 0, 0);

            // Reset scroll position
            ExeiformScrollView.ScrollToAsync(0, 0, true);
        }
    }

}
