using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using IOPath = System.IO.Path;

namespace coursework
{
    /// <summary>
    /// Логика взаимодействия для AddEditRecipeWindow.xaml
    /// </summary>
    public partial class AddEditRecipeWindow : Window
    {
        private RecipeContext _context;
        private Recipe? _recipe;

        public AddEditRecipeWindow(RecipeContext context, Recipe? recipe = null)
        {
            InitializeComponent();
            _context = context;
            _recipe = recipe;

            if (_recipe != null) // редагування
            {
                TitleBox.Text = _recipe.Title;
                CategoryBox.Text = _recipe.Category;
                CookingTimeBox.Text = _recipe.CookingTime;
                IngredientsBox.Text = _recipe.Ingredients;
                DescriptionBox.Text = _recipe.Description;
                ImagePathBox.Text = _recipe.ImagePath;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_recipe == null) // новий рецепт
            {
                _recipe = new Recipe
                {
                    UserId = (int)Application.Current.Properties["CurrentUserId"] // Прив'язка до користувача
                };
                _context.Recipes.Add(_recipe);
            }

            _recipe.Title = TitleBox.Text;
            _recipe.Category = CategoryBox.Text;
            _recipe.CookingTime = CookingTimeBox.Text;
            _recipe.Ingredients = IngredientsBox.Text;
            _recipe.Description = DescriptionBox.Text;
            _recipe.ImagePath = ImagePathBox.Text;

            _context.SaveChanges();
            this.DialogResult = true;
            this.Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            if (openFileDialog.ShowDialog() == true)
            {
                string sourcePath = openFileDialog.FileName;
                string appFolder = IOPath.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Images");

                if (!Directory.Exists(appFolder))
                    Directory.CreateDirectory(appFolder);

                string fileName = IOPath.GetFileName(sourcePath);
                string destPath = IOPath.Combine(appFolder, fileName);

                File.Copy(sourcePath, destPath, true);

                ImagePathBox.Text = destPath;
            }
        }
    }
}
