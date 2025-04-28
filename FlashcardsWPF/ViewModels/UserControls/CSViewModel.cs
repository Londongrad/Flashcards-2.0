using Flashcards.Core.Commands.Base;
using Flashcards.Models;
using Flashcards.Repositoties.Abstract;
using Flashcards.ViewModels.Base;
using Microsoft.Win32;
using System.Windows;

namespace Flashcards.ViewModels.UserControls
{
    public class CSViewModel(ISetRepository setRepository, IWordRepository wordRepository) : ViewModel
    {
        #region [ Fields ]

        private string _set = string.Empty;
        private string _wordName = string.Empty;
        private string _definition = string.Empty;
        private string _imagePath = string.Empty;
        private string _nameDeleteSet = string.Empty;
        private string _nameDeleteWord = string.Empty;
        private string _thumbUpVis = "Hidden";
        private string _exVis = "Hidden";

        #endregion [ Fields ]

        #region [ Commands ]

        public RelayCommand CreateSetCommand =>
            new (async execute => await CreateSet(), canExecute => Set != "");
        public RelayCommand DeleteSetCommand =>
            new (async execute => await DeleteSet(), canExecute => NameDeleteSet != "");
        public RelayCommand FindImageCommand =>
            new (execute => FindImage(), canExecute => WordName != "");
        public RelayCommand DeleteWordCommand =>
            new (async execute => await DeleteWord(), canExecute => NameDeleteWord != "");
        public RelayCommand AddWordCommand =>
            new (async execute => await AddWord(), 
                canExecute => Set != "" && WordName != "" && Definition != "" && ExVis != "Visible");

        #endregion [ Commands ]

        #region [ Properties ]

        public string Set
        {
            get { return _set; }
            set
            {
                _set = value;
                OnPropertyChanged();
            }
        }

        public string WordName
        {
            get { return _wordName; }
            set
            {
                Task.Run(() =>
                {
                    if (wordRepository.CheckWordIfExistsAsync(value.ToLower()).Result)
                        ExVis = "Visible";
                    else
                        ExVis = "Hidden";
                });
                _wordName = value;
                OnPropertyChanged();
            }
        }

        public string Definition
        {
            get { return _definition; }
            set
            {
                _definition = value;
                OnPropertyChanged();
            }
        }

        public string NameDeleteSet
        {
            get { return _nameDeleteSet; }
            set
            {
                _nameDeleteSet = value;
                OnPropertyChanged();
            }
        }

        public string NameDeleteWord
        {
            get { return _nameDeleteWord; }
            set
            {
                _nameDeleteWord = value;
                OnPropertyChanged();
            }
        }

        public string ThumbUpVis
        {
            get { return _thumbUpVis; }
            set
            {
                _thumbUpVis = value;
                OnPropertyChanged();
            }
        }

        public string ExVis
        {
            get { return _exVis; }
            set
            {
                _exVis = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (value != null)
                    _imagePath = value;
                OnPropertyChanged();
            }
        }

        #endregion [ Properties ]

        #region [ Methods ]

        private async Task<SetEntity> CreateSet()
        {
            var sets = await setRepository.GetAllAsync();
            var set = sets.FirstOrDefault(set => set.Name == Set)
                      ??
                      await setRepository.AddAsync(new SetEntity() { Name = Set });
            return set;
        }

        private async Task DeleteSet()
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the set {NameDeleteSet} ?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                await setRepository.DeleteAsync(NameDeleteSet);
            NameDeleteSet = "";
        }

        private async Task DeleteWord()
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the word {NameDeleteWord} ?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                await wordRepository.DeleteAsync(NameDeleteWord);
            NameDeleteWord = "";
        }

        private void FindImage()
        {
            ThumbUpVis = "Hidden";

            OpenFileDialog dlg = new()
            {
                DefaultExt = ".png",
                Filter = "All Pictures (*.emf;*.wmf;*.jpg;*.jpeg;*.jfif;*.jpe;*.png;*.bmp;*.dib;*.rle;*.gif;*.emz;*.wmz;*.tif;*.tiff;*.svg;*.ico;*.webp)" +
                "|*.emf;*.wmf;*.jpg;*.jpeg;*.jfif;*.jpe;*.png;*.bmp;*.dib;*.rle;*.gif;*.emz;*.wmz;*.tif;*.tiff;*.svg;*.ico;*.webp"
            };

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                ImagePath = dlg.FileName;
                ThumbUpVis = "Visible";
            }

            // You can use this code, if you're lazy to select and image. Just predefine the path and name you image just like a word. And it will choose it automatically by pressing a button.

            //ThumbUpVis = "Hidden";
            //if (WordName != "")
            //    ImagePath = Directory.GetFiles("D:\\Download\\Images", WordName + ".*", SearchOption.AllDirectories).FirstOrDefault()!;
            //if (ImagePath != "" && ImagePath != null)
            //    ThumbUpVis = "Visible";
        }

        private async Task AddWord()
        {
            ExVis = "Hidden";
            ThumbUpVis = "Hidden";
            var set = await CreateSet();
            var word = new WordEntity()
            {
                Name = WordName,
                Definition = Definition,
                ImagePath = ImagePath,
                SetId = set.Id
            };
            await wordRepository.AddAsync(word);
            WordName = "";
            Definition = "";
        }

        #endregion [ Methods ]
    }
}