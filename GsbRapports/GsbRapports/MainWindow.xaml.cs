using System;
using System.Configuration;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using dllRapportVisites;
using Newtonsoft.Json;

namespace GsbRapports
{
    public partial class MainWindow : Window
    {
        private WebClient wb;
        private string site;
        private string ticket;
        private Secretaire laSecretaire;

        public MainWindow()
        {
            InitializeComponent();
            this.wb = new WebClient();
            this.site = ConfigurationManager.AppSettings.Get("srvLocal");
            this.laSecretaire = new Secretaire();
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string mdp = this.txtMdp.Password;
                string login = this.txtLogin.Text;
                string reponse;
                string url = this.site + "login?login=" + login;
                reponse = this.wb.DownloadString(url);
                this.ticket = (string)JsonConvert.DeserializeObject(reponse);

                if (this.ticket == null)
                {
                    MessageBox.Show("Erreur de Login", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.txtLogin.Clear();
                }
                else
                {
                    this.laSecretaire.ticket = this.ticket;
                    this.laSecretaire.mdp = mdp;
                    string hash = this.laSecretaire.getHashTicketMdp();
                    url = this.site + "connexion?login=" + login + "&mdp=" + hash;
                    reponse = this.wb.DownloadString(url);
                    Secretaire s = JsonConvert.DeserializeObject<Secretaire>(reponse);

                    if (s == null)
                    {
                        MessageBox.Show("Erreur de mot de passe", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        this.laSecretaire.nom = s.nom;
                        this.laSecretaire.prenom = s.prenom;
                        this.laSecretaire.mdp = this.txtMdp.Password;
                        this.laSecretaire.ticket = s.ticket;
                        this.txtBonjour.Text = "Bonjour " + this.laSecretaire.prenom + " " + this.laSecretaire.nom;
                        this.txtBonjour.Visibility = Visibility.Visible;
                        this.DckMenu.Visibility = Visibility.Visible;
                        this.imgLogo.Visibility = Visibility.Visible;
                        this.loginPanel.Visibility = Visibility.Collapsed;
                        this.txtConnexion.Visibility = Visibility.Collapsed; // Masque le texte de connexion
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    MessageBox.Show(response.StatusCode.ToString(), "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MenuItem_Quitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            VoirFamillesWindow w = new VoirFamillesWindow(wb, site, laSecretaire);
            w.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            majFamilleWindow w = new majFamilleWindow(wb, site, laSecretaire);
            w.Show();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            ajoutFamilleWindow w = new ajoutFamilleWindow(wb, site, laSecretaire);
            w.Show();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            voirVisiteursWindow w = new voirVisiteursWindow(wb, site, laSecretaire);
            w.Show();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            majVisiteurWindow w = new majVisiteurWindow(wb, site, laSecretaire);
            w.Show();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            ajoutVisiteurWindow w = new ajoutVisiteurWindow(wb, site, laSecretaire);
            w.Show();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            rapportsVisiteurs w = new rapportsVisiteurs(wb, site, laSecretaire);
            w.Show();
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            rechercheRapportsVisiteurs w = new rechercheRapportsVisiteurs(wb, site, laSecretaire);
            w.Show();
        }
    }
}
