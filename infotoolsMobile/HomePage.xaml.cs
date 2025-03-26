using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace infotoolsMobile
{
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<Rdv> RdvList { get; set; }

        public HomePage()
        {
            InitializeComponent();

            RdvList = new ObservableCollection<Rdv>();

            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            GetRdvData();
        }
        private async void GetRdvData()
        {
            RdvList.Clear();
            List<Rdv> rdvData = await Core.getRdv();

            if (rdvData != null)
            {
                foreach (var rdv in rdvData)
                {
                    RdvList.Add(rdv);
                }
            }
            else
            {
                Console.WriteLine("Aucun rendez-vous trouvé.");
            }
        }
        private async void OnRdvTapped(object sender, EventArgs e)
        {
            Rdv tappedRdv = (Rdv)((TappedEventArgs)e).Parameter;  // Récupère l'objet de rendez-vous
            if (tappedRdv != null)
            {
                // Naviguer vers la page de détails du rendez-vous et passer l'objet de rendez-vous
                await Navigation.PushAsync(new RdvDetailPage(tappedRdv));
            }
        }

        // Exemple de fonction pour ajouter un rendez-vous
        private async void OnAddRdvClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Ajouter", "Page d&ajout à implémenter.", "OK");
        }
    }
}
