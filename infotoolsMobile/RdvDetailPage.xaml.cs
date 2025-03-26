using Microsoft.Maui.Controls;

namespace infotoolsMobile
{
    public partial class RdvDetailPage : ContentPage
    {
        public Rdv Rdv { get; set; }

        public RdvDetailPage(Rdv rdv)
        {
            InitializeComponent();
            Rdv = rdv;
            BindingContext = this;
        }
        private async void OnModifyClicked(object sender, EventArgs e)
        {
            // Vous pouvez naviguer vers une page de modification, par exemple.
            await DisplayAlert("Modifier", "Page de modification � impl�menter.", "OK");
        }

        // Gestion de l'�v�nement pour le bouton Supprimer
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var confirmDelete = await DisplayAlert("Supprimer", "�tes-vous s�r de vouloir supprimer ce rendez-vous ?", "Oui", "Non");
            if (confirmDelete)
            {
                // Impl�mentez la logique de suppression ici
                await DisplayAlert("Supprimer", "suppresion � impl�menter.", "OK");
            }
        }
    }
}
