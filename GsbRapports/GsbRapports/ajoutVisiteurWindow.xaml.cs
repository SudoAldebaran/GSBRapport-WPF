using dllRapportVisites;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logique d'interaction pour ajoutVisiteurWindow.xaml
    /// </summary>
    public partial class ajoutVisiteurWindow : Window
    {
        private WebClient wb;
        private string site;
        private Secretaire laSecretaire;
        public ajoutVisiteurWindow(WebClient wb, string site, Secretaire laSecretaire)
        {
            InitializeComponent();
            this.wb = wb;
            this.site = site;
            this.laSecretaire = laSecretaire;
            //this.DataContext = this;
        }


        private async void btnValider_Click(object sender, RoutedEventArgs e)
        {
            // Verif du texte saisi ==> je ne veux ni espace ni vide 

            // ---------------------------------------- ID VISITEUR ------------------------------------------//
            if (this.txtIdVisiteur.Text.Length != 3)
            {
                // msg erreur si champs vide
                MessageBox.Show("Veuillez saisir une adresse contenant 3 lettres", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(this.txtIdVisiteur.Text, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("ID VISITEUR : Veuillez ne saisir que des lettres ou des chiffres");
                return;
            }
            // ---------------------------------------- NOM ------------------------------------------//
            if (this.txtNomVisiteur.Text.Length <= 2 || this.txtNomVisiteur.Text.Length > 30)
            {
                // msg erreur si champs vide
                MessageBox.Show("Veuillez saisir un nom de famille contenant 3 à 30 lettres", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(this.txtNomVisiteur.Text, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("NOM : Veuillez ne saisir que des lettres");
                return;
            }
            // ---------------------------------------- PRENOM ------------------------------------------//
            if (this.txtPrenomVisiteur.Text.Length <= 2 || this.txtPrenomVisiteur.Text.Length > 30)
            {
                // msg erreur si champs vide
                MessageBox.Show("Veuillez saisir un nom de famille contenant 3 à 30 lettres", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(this.txtPrenomVisiteur.Text, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("PRENOM : Veuillez ne saisir que des lettres");
                return;
            }
            // ---------------------------------------- ADRESSE ------------------------------------------//
            if (this.txtAdresseVisiteur.Text.Length <= 2 || this.txtAdresseVisiteur.Text.Length > 30)
            {
                // msg erreur si champs vide
                MessageBox.Show("Veuillez saisir une adresse contenant 3 à 30 lettres", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(this.txtAdresseVisiteur.Text, @"^[a-zA-Z0-9 ]+$"))
            {
                MessageBox.Show("ADRESSE : Veuillez ne saisir que des lettres ou des chiffres");
                return;
            }
            // ---------------------------------------- VILLE ------------------------------------------//
            if (this.txtVilleVisiteur.Text.Length <= 2 || this.txtVilleVisiteur.Text.Length > 30)
            {
                // msg erreur si champs vide
                MessageBox.Show("Veuillez saisir un nom de famille contenant 3 à 30 lettres", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(this.txtVilleVisiteur.Text, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("VILLE : Veuillez ne saisir que des lettres");
                return;
            }
            // ---------------------------------------- CP ------------------------------------------//
            if (this.txtCpVisiteur.Text.Length != 5)
            {
                // msg erreur si champs vide
                MessageBox.Show("Veuillez saisir un code postal de 5 chiffres", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(this.txtCpVisiteur.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("CODE POSTAL : Veuillez ne saisir que des chiffres");
                return;
            }
            // ---------------------------------------- DATE EMBAUCHE ------------------------------------------//
            if (!Regex.IsMatch(this.txtDateVisiteur.Text, @"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$"))
            {
                MessageBox.Show("CODE POSTAL : Veuillez ne saisir que des chiffres, et un format de date adapté sous forme : AAAA-MM-JJ");
                return;
            }

            // sinon si tout est okay : 

            string hash = this.laSecretaire.getHashTicketMdp();
            string url = this.site + "visiteurs";
            NameValueCollection parametre = new NameValueCollection();
            parametre.Add("ticket", hash);
            parametre.Add("idVisiteur", this.txtIdVisiteur.Text);
            parametre.Add("nom", this.txtNomVisiteur.Text);
            parametre.Add("prenom", this.txtPrenomVisiteur.Text);
            parametre.Add("ville", this.txtVilleVisiteur.Text);
            parametre.Add("adresse", this.txtAdresseVisiteur.Text);
            parametre.Add("cp", this.txtCpVisiteur.Text);
            parametre.Add("dateEmbauche", this.txtDateVisiteur.Text);
            byte[] tab = await wb.UploadValuesTaskAsync(url, "POST", parametre);
            string reponse = UnicodeEncoding.UTF8.GetString(tab);
            string ticket = reponse.Substring(2, reponse.Length - 2);
            this.laSecretaire.ticket = ticket;


            MessageBox.Show("Visiteur ajouté avec succès !");

            this.Close();

        }
    }
}
