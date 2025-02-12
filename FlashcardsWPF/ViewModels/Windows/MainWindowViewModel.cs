using Flashcards.Core.Commands.Base;
using Flashcards.Models;
using Flashcards.Repositoties.Abstract;
using Flashcards.Services.Abstracts;
using Flashcards.ViewModels.Base;
using Flashcards.ViewModels.UserControls;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IO;

namespace Flashcards.ViewModels.Windows
{
    public delegate Task SetCommand(int num);

    public class MainWindowViewModel(INavigationService navigationService) : ViewModel
    {
        #region [ Properties ]

        public INavigationService Navigation
        {
            get => navigationService!;
            private set
            {
                navigationService = value;
                OnPropertyChanged();
            }
        }

        #endregion [ Properties ]

        #region [ Commands ]

        public RelayCommand NavigateToCSViewCommand => new RelayCommand
            (
                execute => Navigation.NavigateTo(App.ServiceProvider!.GetRequiredService<CSViewModel>())
            );

        public RelayCommand NavigateToSetsViewCommand => new RelayCommand
            (
                execute => Navigation.NavigateTo(App.ServiceProvider!.GetRequiredService<SetsViewModel>())
            );

        public RelayCommand SetVisibility => new RelayCommand
            (
            execute => SelectedSetViewModel.setCommand!(0),
            canExecute => Navigation!.CurrentView is SelectedSetViewModel
            );

        public RelayCommand GoForward => new RelayCommand
            (
            execute => SelectedSetViewModel.setCommand!(1),
            canExecute => Navigation!.CurrentView is SelectedSetViewModel
            );

        public RelayCommand GoBack => new RelayCommand
            (
            execute => SelectedSetViewModel.setCommand!(2),
            canExecute => Navigation!.CurrentView is SelectedSetViewModel
            );

        public RelayCommand ToFavorite => new RelayCommand
            (
            execute => SelectedSetViewModel.setCommand!(3),
            canExecute => Navigation!.CurrentView is SelectedSetViewModel
            );

        public RelayCommand Import => new RelayCommand(execute =>
            {
                //var words = new List<WordEntity>();
                //var path = "C:\\Users\\h-b-1\\Desktop\\sets.json";

                //var res = JsonConvert.DeserializeObject<List<SetEntity>>(File.ReadAllText(path))!;
                //foreach (var set in res)
                //{
                //    await setRepository!.AddAsync(set);
                //}
            }
            );

        #endregion [ Commands ]
    }
}