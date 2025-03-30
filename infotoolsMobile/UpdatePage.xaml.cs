namespace infotoolsMobile
{
    public partial class UpdatePage : ContentPage
    {
        private List<Client> allClients = new List<Client>();

        public Rdv Rdv { get; set; }

        public UpdatePage(Rdv rdv)
        {
            InitializeComponent();
            this.Rdv = rdv;
            BindingContext = this;
        }

       
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadClients();
            SelectDefaultClient();  
        }

        
        private async Task LoadClients()
        {
            allClients.Clear();
            List<Client> clients = await Core.GetClients();

            if (clients != null && clients.Count > 0)
            {
                allClients.AddRange(clients);
                ClientPicker.ItemsSource = allClients;
                ClientPicker.ItemDisplayBinding = new Binding("name");
            }
        }
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue.ToLower();
            ClientPicker.ItemsSource = string.IsNullOrEmpty(searchText)
                ? allClients
                : allClients.Where(c => c.name.ToLower().Contains(searchText)).ToList();
        }

        // Pré-remplir le client et les informations du rendez-vous
        private async void SelectDefaultClient()
        {
            if (Rdv?.Client != null)
            {
                ClientPicker.SelectedItem = allClients.FirstOrDefault(c => c.name == Rdv.Client); 
            }
            DateRdvPicker.Date = Rdv.DateRdv;
            TimeRdvPicker.Time = Rdv.DateRdv.TimeOfDay;
        }

        private async void OnUpdateRdvClicked(object sender, EventArgs e)
        {
            Client selectedClient = ClientPicker.SelectedItem as Client;

            // Vérifier si un client est sélectionné
            if (selectedClient == null)
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner un client.", "OK");
                return;
            }

            try
            {
                DateTime dateTimeRdv = DateRdvPicker.Date.Add(TimeRdvPicker.Time);

                string formattedDateTime = dateTimeRdv.ToString("yyyy-MM-dd HH:mm:ss");

                bool success = await Core.UpdateRdv(selectedClient, formattedDateTime , Rdv.Id);

                if (success)
                {
                    await DisplayAlert("Succès", "Le rendez-vous a été mis à jour avec succès.", "OK");

                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    await DisplayAlert("Erreur", "Une erreur est survenue lors de la mise à jour du rendez-vous.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Une erreur inattendue s'est produite : {ex.Message}", "OK");
            }
        }

    }
}
