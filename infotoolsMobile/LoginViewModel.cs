using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace infotoolsMobile
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _errorMessage;
        private bool _hasError;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged(nameof(HasError));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login());
        }

        private async Task Login()
        {
            HasError = false;
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                HasError = true;
                ErrorMessage = "Veuillez remplir tous les champs.";
                return;
            }

            // Appelle l'API pour récupérer l'utilisateur
            user loggedUser = await Core.GetUser(Email, Password);

            if (loggedUser != null && !string.IsNullOrEmpty(loggedUser.token))
            {
                // Stocke le token dans SecureStorage
                await SecureStorage.SetAsync("UserToken", loggedUser.token);

                // Stocke le nom de l'utilisateur si nécessaire
                await SecureStorage.SetAsync("UserName", loggedUser.name);

                await SecureStorage.SetAsync("IdUser", loggedUser.Id.ToString());

                // Rediriger vers la page d'accueil
                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                HasError = true;
                ErrorMessage = "Connexion échouée. Vérifiez vos informations.";
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
