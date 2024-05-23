using dllRapportVisites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace GsbRapports
{
    /// <summary>
    /// Logique d'interaction pour rapportsVisiteurs.xaml
    /// </summary>
    public partial class rapportsVisiteurs : Window
    {
        private WebClient wb;
        private string site;
        private Secretaire laSecretaire;

        public rapportsVisiteurs(WebClient wb, string site, Secretaire laSecretaire)
        {
            InitializeComponent();
            this.wb = wb;
            this.site = site;
            this.laSecretaire = laSecretaire;
            getVisiteurs();
        }

        private async void getVisiteurs()
        {
            string hash = this.laSecretaire.getHashTicketMdp();
            string url = this.site + "visiteurs?ticket=" + hash;
            string reponse = await this.wb.DownloadStringTaskAsync(url);
            dynamic d = JsonConvert.DeserializeObject(reponse);
            this.laSecretaire.ticket = d.ticket; // la secrétaire à jour
            string lesVisiteurs = d.visiteurs.ToString();
            List<Visiteur> list = JsonConvert.DeserializeObject<List<Visiteur>>(lesVisiteurs);
            this.cmbVisiteurs.ItemsSource = list;
            this.cmbVisiteurs.DisplayMemberPath = "NomPrenom";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier que les dates et le visiteur sont sélectionnés
            if (this.cmbVisiteurs.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un visiteur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(this.date1.Text) || string.IsNullOrWhiteSpace(this.date2.Text))
            {
                MessageBox.Show("Veuillez saisir les deux dates.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime dateDebut, dateFin;
            if (!DateTime.TryParse(this.date1.Text, out dateDebut) || !DateTime.TryParse(this.date2.Text, out dateFin))
            {
                MessageBox.Show("Veuillez saisir des dates valides.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Récupérer l'ID du visiteur sélectionné
            Visiteur visiteurSelectionne = (Visiteur)this.cmbVisiteurs.SelectedItem;
            string idVisiteur = visiteurSelectionne.id;

            // Faire la requête pour obtenir les rapports
            string hash = this.laSecretaire.getHashTicketMdp();
            string url = this.site + $"rapports?ticket={hash}&idVisiteur={idVisiteur}&dateDebut={dateDebut:yyyy-MM-dd}&dateFin={dateFin:yyyy-MM-dd}";
            string reponse = await this.wb.DownloadStringTaskAsync(url);
            dynamic d = JsonConvert.DeserializeObject(reponse);
            this.laSecretaire.ticket = d.ticket;
            string lesRapports = d.rapports.ToString();
            List<Rapport> rapports = JsonConvert.DeserializeObject<List<Rapport>>(lesRapports);

            // Ouvrir la nouvelle fenêtre avec les rapports
            string visiteurNomPrenom = ((Visiteur)this.cmbVisiteurs.SelectedItem).NomPrenom;
            RapportsWindow rapportsWindow = new RapportsWindow(rapports, visiteurNomPrenom);
            rapportsWindow.Show();

            // Facultatif : Fermer la fenêtre actuelle
            // this.Close();
        }
    }
}
