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

        // V�rifier si un client est s�lectionn�
        if (selectedClient == null)
        {
            await DisplayAlert("Erreur", "Veuillez s�lectionner un client.", "OK"); 
        }
        else
        {
            try
            {
                // R�cup�rer la date et l'heure du rendez-vous
                DateTime dateTimeRdv = DateRdvPicker.Date.Add(TimeRdvPicker.Time);

                // Formater la date et l'heure dans le format "yyyy-MM-dd HH:mm:ss"
                string formattedDateTime = dateTimeRdv.ToString("yyyy-MM-dd HH:mm:ss");

                // Appeler la m�thode pour ajouter le rendez-vous, en passant le client et la date format�e
                bool success = await Core.AddRdv(selectedClient, formattedDateTime);

                // G�rer la r�ponse de la m�thode AddRdv
                if (success)
                {
                    // Code pour indiquer que le rendez-vous a �t� ajout� avec succ�s, par exemple :
                    await DisplayAlert("Succ�s", "Le rendez-vous a �t� ajout� avec succ�s.", "OK");
                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    // G�rer le cas o� l'ajout du rendez-vous a �chou�
                    await DisplayAlert("Erreur", "Une erreur est survenue lors de l'ajout du rendez-vous.", "OK");
                }
            }
            catch (Exception ex)
            {
                // G�rer les exceptions en cas de probl�me lors de l'ajout du rendez-vous
                await DisplayAlert("Erreur", $"Une erreur inattendue s'est produite : {ex.Message}", "OK");
            }
        }
    }


}
