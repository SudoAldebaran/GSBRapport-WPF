using dllRapportVisites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Policy;
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
    /// Logique d'interaction pour majFamilleWindow.xaml
    /// </summary>
    public partial class majFamilleWindow : Window
    {
        private WebClient wb;
        private string site;
        private Secretaire laSecretaire;
        public majFamilleWindow(WebClient wb,string  site,Secretaire laSecretaire)
        {
            InitializeComponent();
            this.wb = wb;   
            this.site = site;
            this.laSecretaire = laSecretaire;
            getFamilles();
        }
        private async void getFamilles()
        {
            string hash = this.laSecretaire.getHashTicketMdp();
            string url = this.site + "familles?ticket=" + hash;
            string reponse = await this.wb.DownloadStringTaskAsync(url);  
            dynamic d = JsonConvert.DeserializeObject(reponse);
            this.laSecretaire.ticket = d.ticket; // la secrétaire à jour
            string lesFamilles = d.familles.ToString();
            List<Famille> list = JsonConvert.DeserializeObject<List<Famille>>(lesFamilles);
            this.cmbFamille.ItemsSource = list;
            this.cmbFamille.DisplayMemberPath = "libelle";

        }

        private async void btnValider_Click(object sender, RoutedEventArgs e)
        {
            // Verif du texte saisi ==> je ne veux ni espace ni vide 
            if (this.txtLibFamille.Text.Length <= 2 || this.txtLibFamille.Text.Length > 30)
            {
                // msg erreur si champs vide
                MessageBox.Show("Veuillez saisir un nom de famille contenant 3 à 30 lettres", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); 
                return;
            }
            // Verif de la correspondance de la première lettre de l'ID avec la première lettre du libellé
            if (((Famille)this.cmbFamille.SelectedItem).id[0] != txtLibFamille.Text[0])
            {
                MessageBox.Show("La première lettre de l'ID ne correspond pas à la première lettre du libellé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            // sinon si tout est okay : 

                string hash = this.laSecretaire.getHashTicketMdp();
                string url = this.site + "famille";
                NameValueCollection parametre = new NameValueCollection();
                parametre.Add("ticket", hash);
                parametre.Add("libelle", this.txtLibFamille.Text);
                parametre.Add("idFamille", ((Famille)this.cmbFamille.SelectedItem).id);
                byte[] tab = await wb.UploadValuesTaskAsync(url, "POST", parametre);
                string reponse = UnicodeEncoding.UTF8.GetString(tab);
                string ticket = reponse.Substring(2, reponse.Length - 2);
                this.laSecretaire.ticket = ticket;


                MessageBox.Show("Famille modifiée avec succès !");

                this.Close();

        }
    }
}
