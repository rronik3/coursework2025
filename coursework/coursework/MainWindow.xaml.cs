using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace coursework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RecipeContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new RecipeContext();
            _context.Database.EnsureCreated();

            LoadRecipes();
        }

        private void LoadRecipes()
        {
            int currentUserId = (int)Application.Current.Properties["CurrentUserId"];
            RecipesDataGrid.ItemsSource = _context.Recipes
                .Where(r => r.UserId == currentUserId)
                .ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditRecipeWindow(_context);
            if (window.ShowDialog() == true)
            {
                LoadRecipes();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesDataGrid.SelectedItem is Recipe selected)
            {
                var window = new AddEditRecipeWindow(_context, selected);
                if (window.ShowDialog() == true)
                {
                    LoadRecipes();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesDataGrid.SelectedItem is Recipe selected)
            {
                var result = MessageBox.Show($"Ви впевнені, що хочете видалити рецепт \"{selected.Title}\"?",
                                             "Підтвердження видалення",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Recipes.Remove(selected);
                    _context.SaveChanges();
                    LoadRecipes();
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            RecipesDataGrid.ItemsSource = string.IsNullOrEmpty(query)
                ? _context.Recipes.ToList()
                : _context.Recipes.Where(r => r.Title.ToLower().Contains(query)).ToList();
        }

        private void RecipesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RecipesDataGrid.SelectedItem is Recipe selectedRecipe)
            {
                var detailsWindow = new RecipeDetailsWindow(selectedRecipe);
                detailsWindow.Show();
                this.Close(); 
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
