﻿using Flashcards.Core.Commands.Base;
using Flashcards.Models;
using Flashcards.Repositoties.Abstract;
using Flashcards.ViewModels.Base;
using Flashcards.ViewModels.Windows;
using Flashcards.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;
using System.Windows;

namespace Flashcards.ViewModels.UserControls
{
    public class SelectedSetViewModel : ViewModel
    {
        public SelectedSetViewModel(ObservableCollection<WordEntity> words, bool IsChecked, IWordRepository wordRepository, Func<WordEntity, EditWindowViewModel> viewModel)
        {
            setCommand = KeyCommands;
            _words = words;
            _isChecked = IsChecked;
            _wordRepository = wordRepository;
            _viewModel = viewModel;
            speechSynthesizer = new();
            try { speechSynthesizer.SelectVoice("Microsoft Hazel Desktop"); }
            catch (Exception) { }
            ShowFirstWord();
        }

        #region [ Fields ]

        public static SetCommand? setCommand;
        private int i = 0;
        private readonly bool _isChecked;
        private readonly IWordRepository _wordRepository;
        private readonly Func<WordEntity, EditWindowViewModel> _viewModel;
        private ObservableCollection<WordEntity> _words;
        private string _wordName = string.Empty;
        private string _definitionName = string.Empty;
        private string _imagePath = string.Empty;
        private string _colorFav = "Transporant";
        private string _isVisibleWord = "Hidden";
        private string _isVisibleDef = "Hidden";
        private string _isVisibleImage = "Hidden";
        private string _count = string.Empty;
        private readonly SpeechSynthesizer speechSynthesizer;

        #endregion [ Fields ]

        #region [ Commmands ]

        public RelayCommand MixWordsCommand => new(async execute => await MixWords());
        public RelayCommand EditCommand => new(execute => Edit());
        public RelayCommand DeleteWordCommand => new(async execute => await DeleteWord());

        #endregion [ Commmands ]

        #region [ Properties ]

        public string WordName
        {
            get { return _wordName; }
            set
            {
                _wordName = value;
                OnPropertyChanged();
            }
        }

        public string DefinitionName
        {
            get { return _definitionName; }
            set
            {
                _definitionName = value;
                OnPropertyChanged();
            }
        }

        public string Image
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public string ColorFav
        {
            get { return _colorFav; }
            set
            {
                _colorFav = value;
                OnPropertyChanged();
            }
        }

        public string IsVisibleWord
        {
            get { return _isVisibleWord; }
            set
            {
                _isVisibleWord = value;
                OnPropertyChanged();
            }
        }

        public string IsVisibleDef
        {
            get { return _isVisibleDef; }
            set
            {
                _isVisibleDef = value;
                OnPropertyChanged();
            }
        }

        public string IsVisibleImage
        {
            get { return _isVisibleImage; }
            set
            {
                _isVisibleImage = value;
                OnPropertyChanged();
            }
        }

        public string Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<WordEntity> Words
        {
            get { return _words; }
            set
            {
                _words = value;
                OnPropertyChanged();
            }
        }

        #endregion [ Properties ]

        #region [ Methods ]

        private void SayWordAsync(string word)
        {
            speechSynthesizer.SpeakAsyncCancelAll();
            speechSynthesizer.SpeakAsync(word);
        }

        private void ShowFirstWord()
        {
            var lastWord = _words!.FirstOrDefault(c => c.IsLastWord == true);
            if (lastWord != null)
            {
                i = _words!.IndexOf(lastWord);
            }
            Count = $"{i + 1}" + "/" + $"{_words!.Count}";
            if (_words[i].IsFavorite)
                ColorFav = "Yellow";
            ShowWord();
        }

        private void ShowWord()
        {
            WordName = _words![i].Name;
            DefinitionName = _words![i].Definition;
            Image = _words[i].ImagePath;
            if (_isChecked)
            {
                IsVisibleImage = "Hidden";
                IsVisibleWord = "Hidden";
                IsVisibleDef = "Visible";
            }
            else
            {
                IsVisibleImage = "Hidden";
                IsVisibleDef = "Hidden";
                IsVisibleWord = "Visible";
                SayWordAsync(WordName);
            }
        }

        public async Task KeyCommands(int command)
        {
            switch (command)
            {
                case 0:
                    if (_isChecked)
                    {
                        IsVisibleWord = "Visible";
                        IsVisibleImage = "Visible";
                        SayWordAsync(_words![i].Name);
                    }
                    else
                    {
                        if (IsVisibleDef == "Visible")
                            SayWordAsync(_words![i].Name);
                        IsVisibleDef = "Visible";
                        IsVisibleImage = "Visible";
                    }
                    break;

                case 1:
                    if (i >= 0 && i < _words!.Count - 1)
                    {
                        await LastWordDelete();
                        ColorFav = "Transporant";
                        i++;
                        Count = $"{i + 1}" + "/" + $"{_words!.Count}";
                        if (_words![i].IsFavorite)
                            ColorFav = "Yellow";
                        ShowWord();
                        await LastWordAdd();
                        if (i == _words!.Count - 1)
                            await LastWordDelete();
                    }
                    else if (i == _words!.Count - 1 && _words!.Count > 1)
                    {
                        i = 0;
                        Count = $"{i + 1}" + "/" + $"{_words!.Count}";
                        ColorFav = "Transporant";
                        if (_words![i].IsFavorite)
                            ColorFav = "Yellow";
                        ShowWord();
                    }
                    break;

                case 2:
                    if (i > 0 && i <= _words!.Count - 1)
                    {
                        await LastWordDelete();
                        ColorFav = "Transporant";
                        i--;
                        Count = $"{i + 1}" + "/" + $"{_words!.Count}";
                        if (_words![i].IsFavorite)
                            ColorFav = "Yellow";
                        ShowWord();
                        await LastWordAdd();
                        if (i == 0)
                            await LastWordDelete();
                    }
                    else if (i == 0 && _words!.Count > 1)
                    {
                        i = _words!.Count - 1;
                        Count = $"{i + 1}" + "/" + $"{_words!.Count}";
                        ColorFav = "Transporant";
                        if (_words![i].IsFavorite)
                            ColorFav = "Yellow";
                        ShowWord();
                    }
                    break;

                case 3:
                    if (_words![i].IsFavorite)
                    {
                        ColorFav = "Transporant";
                        await FavoriteWordDelete();
                        break;
                    }
                    ColorFav = "Yellow";
                    await FavoriteWordAdd();
                    break;

                default:
                    break;
            }
        }

        private async Task DeleteWord()
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you wanna delete the word {_words![i].Name}", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                await _wordRepository.DeleteAsync(_words![i].Name);
        }

        private void Edit()
        {
            var editWindow = App.ServiceProvider!.GetRequiredService<EditWindow>();
            editWindow.DataContext = _viewModel(_words![i]);
            editWindow.ShowDialog();
        }

        private async Task MixWords()
        {
            await LastWordDelete();
            i = 0;
            Random rng = new();
            int n = _words!.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (_words![n], _words![k]) = (_words![k], _words![n]);
            }
            Count = $"{i + 1}" + "/" + $"{_words!.Count}";
            ShowWord();
            ColorFav = "Transparent";
            if (_words![0].IsFavorite)
                ColorFav = "Yellow";
        }

        private async Task FavoriteWordAdd()
        {
            _words![i].IsFavorite = true;
            await _wordRepository.UpdateAsync(_words![i]);
        }

        private async Task FavoriteWordDelete()
        {
            _words![i].IsFavorite = false;
            await _wordRepository.UpdateAsync(_words![i]);
        }

        private async Task LastWordAdd()
        {
            _words![i].IsLastWord = true;
            await _wordRepository.UpdateAsync(_words![i]);
        }

        private async Task LastWordDelete()
        {
            _words![i].IsLastWord = false;
            await _wordRepository.UpdateAsync(_words![i]);
        }

        #endregion [ Methods ]
    }
}