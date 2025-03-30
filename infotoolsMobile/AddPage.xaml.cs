namespace infotoolsMobile;

public partial class AddPage : ContentPage
{
    private List<Client> allClients = new List<Client>();

    public AddPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadClients();
    }

    private async Task LoadClients()
    {
        allClients.Clear();
        List<Client> clients = await Core.GetClients();

        if (clients != null && clients.Count > 0)
        {
            allClients.AddRange(clients);
            ClientPicker.ItemsSource = allClients;
            ClientPicker.ItemDisplayBinding = new Binding("name"); // Afficher le nom des clients
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue.ToLower();
        ClientPicker.ItemsSource = string.IsNullOrEmpty(searchText)
            ? allClients
            : allClients.Where(c => c.name.ToLower().Contains(searchText)).ToList();
    }

    private async void OnAddRdvClicked(object sender, EventArgs e)
    {
        Client selectedClient = ClientPicker.SelectedItem as Client;

        // Vérifier si un client est sélectionné
        if (selectedClient == null)
        {
            await DisplayAlert("Erreur", "Veuillez sélectionner un client.", "OK"); 
        }
        else
        {
            try
            {
                // Récupérer la date et l'heure du rendez-vous
                DateTime dateTimeRdv = DateRdvPicker.Date.Add(TimeRdvPicker.Time);

                // Formater la date et l'heure dans le format "yyyy-MM-dd HH:mm:ss"
                string formattedDateTime = dateTimeRdv.ToString("yyyy-MM-dd HH:mm:ss");

                // Appeler la méthode pour ajouter le rendez-vous, en passant le client et la date formatée
                bool success = await Core.AddRdv(selectedClient, formattedDateTime);

                // Gérer la réponse de la méthode AddRdv
                if (success)
                {
                    // Code pour indiquer que le rendez-vous a été ajouté avec succès, par exemple :
                    await DisplayAlert("Succès", "Le rendez-vous a été ajouté avec succès.", "OK");
                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    // Gérer le cas où l'ajout du rendez-vous a échoué
                    await DisplayAlert("Erreur", "Une erreur est survenue lors de l'ajout du rendez-vous.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Gérer les exceptions en cas de problème lors de l'ajout du rendez-vous
                await DisplayAlert("Erreur", $"Une erreur inattendue s'est produite : {ex.Message}", "OK");
            }
        }
    }


}
