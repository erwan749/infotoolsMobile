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
            bool isConfirmed = await DisplayAlert("Confirmation", "�tes-vous s�r de vouloir supprimer ce rendez-vous ?", "Oui", "Non");

            if (isConfirmed)
            {
                try
                {
                    // Appeler la m�thode du Core pour supprimer le rendez-vous
                    bool success = await Core.DeleteRdv(Rdv.Id);

                    if (success)
                    {
                        // Si la suppression est r�ussie, retour � la page pr�c�dente
                        await DisplayAlert("Succ�s", "Rendez-vous supprim� avec succ�s.", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        // Si la suppression �choue
                        await DisplayAlert("Erreur", "�chec de la suppression du rendez-vous.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    // En cas d'erreur inattendue
                    await DisplayAlert("Erreur", "Une erreur s'est produite lors de la suppression du rendez-vous: " + ex.Message, "OK");
                }
            }
        }
    }
}
