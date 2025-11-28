using TeacherMAUI.Services;

namespace TeacherMAUI
{
    public partial class App : Application
    {
        public static DatabaseHelper Database { get; private set; }
        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "items.db3");
            Database = new DatabaseHelper(dbPath);


            Routing.RegisterRoute(nameof(ExeiEditPage), typeof(ExeiEditPage));

            MainPage = new AppShell();
        }
    }
}
