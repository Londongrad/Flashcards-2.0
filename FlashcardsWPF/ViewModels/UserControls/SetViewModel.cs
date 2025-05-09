﻿using Flashcards.Core.Commands.Base;
using Flashcards.Models;
using Flashcards.Repositoties.Abstract;
using Flashcards.Services.Abstracts;
using Flashcards.ViewModels.Base;
using System.Collections.ObjectModel;

namespace Flashcards.ViewModels.UserControls
{
    public class SetViewModel : ViewModel
    {
        public SetViewModel(SetEntity setEntity, INavigationService navigationService, IWordRepository wordRepository, Func<ObservableCollection<WordEntity>, bool, SelectedSetViewModel> selectedSetViewModel)
        {
            _words = [.. setEntity.Words];
            _navigator = navigationService;
            _wordRepository = wordRepository;
            _selectedSetViewModel = selectedSetViewModel;
            _nameOfSet = setEntity.Name;
            _numOfWords = setEntity.Words.Count;
            _numOfFavWords = setEntity.Words.Where(w => w.IsFavorite == true).Count();
            _numOfWordsString = _numOfWords.ToString() + " words";
            _numOfFavWordsString = _numOfFavWords.ToString() + " words";
        }

        #region [ Fields ]

        private readonly Func<ObservableCollection<WordEntity>, bool, SelectedSetViewModel> _selectedSetViewModel;
        private readonly ObservableCollection<WordEntity> _words;
        private int _numOfWords = 0;
        private bool _isChecked = false;
        private int _numOfFavWords = 0;
        private string _nameOfSet = string.Empty;
        private int _rangeStart = 0;
        private int _rangeEnd = 0;
        private string _numOfWordsString;
        private string _numOfFavWordsString;
        private readonly INavigationService _navigator;
        private readonly IWordRepository _wordRepository;

        #endregion [ Fields ]

        #region [ Commands ]

        public RelayCommand SelectSetCommand =>
            new (execute => SelectSet(), canExecute => _words.Count > 0);
        public RelayCommand SelectFavWordsCommand =>
            new (execute => SelectFavWords(), canExecute => _words.Where(w => w.IsFavorite).Any());
        public RelayCommand AddToFavInRangeCommand => 
            new (async execute => await AddToFavInRange(),
                canExecute => RangeStart > 0 && RangeEnd >= RangeStart && RangeStart <= _words.Count && RangeEnd <= _words.Count);
        public RelayCommand DeleteAllFavoriteCommand =>
            new (async execute => await DeleteAllFavorite(), canExecute => _words.Any(w => w.IsFavorite));

        #endregion [ Commands ]

        #region [ Properties ]

        public int NumOfWords
        {
            get { return _numOfWords; }
            set
            {
                _numOfWords = value;
                OnPropertyChanged();
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        public int NumOfFavWords
        {
            get { return _numOfFavWords; }
            set
            {
                _numOfFavWords = value;
                OnPropertyChanged();
            }
        }

        public string NameOfSet
        {
            get { return _nameOfSet; }
            set
            {
                _nameOfSet = value;
                OnPropertyChanged();
            }
        }

        public int RangeStart
        {
            get { return _rangeStart; }
            set
            {
                _rangeStart = value;
                OnPropertyChanged();
            }
        }

        public int RangeEnd
        {
            get { return _rangeEnd; }
            set
            {
                _rangeEnd = value;
                OnPropertyChanged();
            }
        }

        public string NumOfWordsString
        {
            get { return _numOfWordsString; }
            set
            {
                _numOfWordsString = value;
                OnPropertyChanged();
            }
        }

        public string NumOfFavWordsString
        {
            get { return _numOfFavWordsString; }
            set
            {
                _numOfFavWordsString = value;
                OnPropertyChanged();
            }
        }

        #endregion [ Properties ]

        #region [ Methods ]

        private void SelectSet()
        {
            _navigator.NavigateTo(_selectedSetViewModel(_words, IsChecked));
        }

        private void SelectFavWords()
        {
            _navigator.NavigateTo(_selectedSetViewModel([.. _words.Where(words => words.IsFavorite == true).ToList()], IsChecked));
        }

        private async Task AddToFavInRange()
        {
            int i = RangeStart - 1;
            while (i < RangeEnd)
            {
                if (!_words[i].IsFavorite)
                {
                    _words[i].IsFavorite = true;
                    await _wordRepository.UpdateAsync(_words[i]);
                    NumOfFavWords++;
                }
                else
                {
                    _words[i].IsFavorite = false;
                    await _wordRepository.UpdateAsync(_words[i]);
                    NumOfFavWords--;
                }
                i++;
            }
            RangeStart = 0; RangeEnd = 0;
        }

        private async Task DeleteAllFavorite()
        {
            int i = 0;
            while (i < _words.Count)
            {
                if (_words[i].IsFavorite)
                {
                    _words[i].IsFavorite = false;
                    await _wordRepository.UpdateAsync(_words[i]);
                    NumOfFavWords--;
                }
                i++;
            }
        }

        #endregion [ Methods ]
    }
}