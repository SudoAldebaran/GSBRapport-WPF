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
            string hash = this.laSecretaire.getHashTicketMdp();
            string url = this.site + "rapports";
            NameValueCollection parametre = new NameValueCollection();
            parametre.Add("ticket", hash);
            parametre.Add("dateDebut", this.date1.Text);
            parametre.Add("dateFin", this.date2.Text);
            parametre.Add("idVisiteur", ((Visiteur)this.cmbVisiteurs.SelectedItem).id);
            byte[] tab = await wb.UploadValuesTaskAsync(url, "GET", parametre);
            string reponse = UnicodeEncoding.UTF8.GetString(tab);
            string ticket = reponse.Substring(2, reponse.Length - 2);
            this.laSecretaire.ticket = ticket;


            MessageBox.Show("Famille modifiée avec succès !");

            this.Close();
        }


    }
}
