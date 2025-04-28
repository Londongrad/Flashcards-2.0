using Flashcards.Core;
using Flashcards.Core.Commands.Base;
using Flashcards.Models;
using Flashcards.Repositoties.Abstract;
using System.Windows;

namespace Flashcards.ViewModels.Windows
{
    public class EditWindowViewModel(WordEntity word, IWordRepository wordRepository) : ObservableObject
    {
        #region [ Commands ]

        public RelayCommand<Window> SaveChangesCommand => new (SaveChanges);

        #endregion [ Commands ]

        #region [ Properties ]

        public string Name
        {
            get { return word.Name; }
            set
            {
                word.Name = value;
                OnPropertyChanged();
            }
        }

        public string Definition
        {
            get { return word.Definition; }
            set
            {
                word.Definition = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get { return word.ImagePath; }
            set
            {
                word.ImagePath = value;
                OnPropertyChanged();
            }
        }

        #endregion [ Properties ]

        #region [ Methods ]

        private async void SaveChanges(Window window)
        {
            if (Name == "" || Definition == "") { return; }
            try
            {
                await wordRepository.UpdateAsync(word);
                window?.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("You already have a word with this name!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion [ Methods ]
    }
}