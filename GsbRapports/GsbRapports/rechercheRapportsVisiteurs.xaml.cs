using dllRapportVisites;
using GsbRapports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GsbRapports
{
    public partial class rechercheRapportsVisiteurs : Window
    {
        private WebClient wb;
        private string site;
        private Secretaire laSecretaire;
        private List<Visiteur> visiteurs;
        private List<Rapport> rapports;

        public rechercheRapportsVisiteurs(WebClient wb, string site, Secretaire laSecretaire)
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
            this.laSecretaire.ticket = d.ticket;
            string lesVisiteurs = d.visiteurs.ToString();
            visiteurs = JsonConvert.DeserializeObject<List<Visiteur>>(lesVisiteurs);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string recherche = txtNomVisiteur.Text;

            if (recherche.Length >= 2)
            {
                var filteredList = visiteurs.Where(v => v.NomPrenom.StartsWith(recherche, StringComparison.InvariantCultureIgnoreCase)).ToList();
                cmbVisiteurs.ItemsSource = filteredList;
                cmbVisiteurs.DisplayMemberPath = "NomPrenom";
            }
            else
            {
                cmbVisiteurs.ItemsSource = null;
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.cmbVisiteurs.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un visiteur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Visiteur visiteurSelectionne = (Visiteur)this.cmbVisiteurs.SelectedItem;
            string idVisiteur = visiteurSelectionne.id;

            string hash = this.laSecretaire.getHashTicketMdp();
            string url = this.site + $"rapports?ticket={hash}&idVisiteur={idVisiteur}";
            string reponse = await this.wb.DownloadStringTaskAsync(url);
            dynamic d = JsonConvert.DeserializeObject(reponse);
            this.laSecretaire.ticket = d.ticket;
            string lesRapports = d.rapports.ToString();
            rapports = JsonConvert.DeserializeObject<List<Rapport>>(lesRapports);

            MessageBox.Show(rapports.Count.ToString());

            
            this.dtgVisiteursR.ItemsSource = rapports;
        }
    }
}



