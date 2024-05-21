using dllRapportVisites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GsbRapports
{
    /// <summary>
    /// Logique d'interaction pour majVisiteurWindow.xaml
    /// </summary>
    public partial class majVisiteurWindow : Window
    {
        private WebClient wb;
        private string site;
        private Secretaire laSecretaire;
        public majVisiteurWindow(WebClient wb, string site, Secretaire laSecretaire)
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

        private async void btnValider_Click(object sender, RoutedEventArgs e)
        {
            // Verif du texte saisi ==> je ne veux ni espace ni vide 
            if (this.txtVilleVisiteur.Text.Length <= 2 || this.txtVilleVisiteur.Text.Length > 30)
            {
                // msg erreur si champs vide
                MessageBox.Show("Veuillez saisir une ville contenant 3 à 30 lettres", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (this.txtAdrVisiteur.Text.Length <= 2 || this.txtAdrVisiteur.Text.Length > 30)
            {
                // msg erreur si champs vide
                MessageBox.Show("Veuillez saisir une adresse contenant 3 à 30 lettres", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (this.txtCpVisiteur.Text.Length != 5) // CODE POSTAL DOIT FAIRE 5 CHIFFRES
            {
                // msg erreur si champs vide
                MessageBox.Show("Veuillez saisir un code postal à 5 chiffres", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            // sinon si tout est okay : 

            string hash = this.laSecretaire.getHashTicketMdp();
            string url = this.site + "visiteur";
            NameValueCollection parametre = new NameValueCollection();
            parametre.Add("ticket", hash);
            parametre.Add("ville", this.txtVilleVisiteur.Text);
            parametre.Add("adresse", this.txtAdrVisiteur.Text);
            parametre.Add("cp", this.txtCpVisiteur.Text);
            parametre.Add("idVisiteur", ((Visiteur)this.cmbVisiteurs.SelectedItem).id);
            byte[] tab = await wb.UploadValuesTaskAsync(url, "POST", parametre);
            string reponse = UnicodeEncoding.UTF8.GetString(tab);
            string ticket = reponse.Substring(2, reponse.Length - 2);
            this.laSecretaire.ticket = ticket;


            MessageBox.Show("Visiteur modifié avec succès !");

            this.Close();

        }
    }

}
