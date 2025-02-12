using Flashcards.Models;
using Flashcards.Repositoties.Abstract;
using Flashcards.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Flashcards.ViewModels.UserControls
{
    public class SetsViewModel : ViewModel
    {
        public SetsViewModel(ISetRepository setRepository, Func<SetEntity, SetViewModel> setView)
        {
            Sets = [];
            _setRepository = setRepository;
            _setView = setView;
            LoadSets();
            BirdsCollectionView = CollectionViewSource.GetDefaultView(Sets);
            BirdsCollectionView.SortDescriptions.Add(new SortDescription(nameof(SetViewModel.NameOfSet), ListSortDirection.Descending));
        }

        #region [ Fields ]
        private ObservableCollection<SetViewModel>? _sets;
        private readonly ISetRepository _setRepository;
        private readonly Func<SetEntity, SetViewModel> _setView;
        #endregion

        #region [ Properties ]
        /// <summary>
        /// Коллекция для сортировки
        /// </summary>
        public ICollectionView BirdsCollectionView { get; }
        public ObservableCollection<SetViewModel> Sets
        {
            get { return _sets!; }
            set
            {
                _sets = value;
            }
        }
        #endregion

        #region [ Methods ]
        private async Task LoadSets()
        {
            var sets = await _setRepository.GetAllAsync();
            foreach (var set in sets)
                Sets.Add(_setView(set));
        }
        #endregion
    }
}
