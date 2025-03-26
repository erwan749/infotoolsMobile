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
            await DisplayAlert("Modifier", "Page de modification à implémenter.", "OK");
        }

        // Gestion de l'événement pour le bouton Supprimer
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool isConfirmed = await DisplayAlert("Confirmation", "Êtes-vous sûr de vouloir supprimer ce rendez-vous ?", "Oui", "Non");

            if (isConfirmed)
            {
                try
                {
                    // Appeler la méthode du Core pour supprimer le rendez-vous
                    bool success = await Core.DeleteRdv(Rdv.Id);

                    if (success)
                    {
                        // Si la suppression est réussie, retour à la page précédente
                        await DisplayAlert("Succès", "Rendez-vous supprimé avec succès.", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        // Si la suppression échoue
                        await DisplayAlert("Erreur", "Échec de la suppression du rendez-vous.", "OK");
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
