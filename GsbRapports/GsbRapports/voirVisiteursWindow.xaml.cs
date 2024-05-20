using dllRapportVisites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour voirVisiteursWindow.xaml
    /// </summary>
    public partial class voirVisiteursWindow : Window
    {
        private WebClient wb;
        private string site;
        private string ticket;
        private Secretaire laSecretaire;
        public voirVisiteursWindow(WebClient wb, string site, Secretaire laSecretaire)
        {
            InitializeComponent();
            this.wb = wb;
            this.site = site;
            this.laSecretaire = laSecretaire;
            getVisiteurs();

        }
        private async void getVisiteurs()
        {
            try
            {
                string hash = this.laSecretaire.getHashTicketMdp();
                string url = this.site + "visiteurs?ticket=" + hash;
                string reponse = await this.wb.DownloadStringTaskAsync(url);
                dynamic d = JsonConvert.DeserializeObject(reponse);
                this.laSecretaire.ticket = d.ticket;
                string lesVisiteurs = d.visiteurs.ToString();
                List<Visiteur> visiteurs = JsonConvert.DeserializeObject<List<Visiteur>>(lesVisiteurs);
                this.dtgVisiteurs.ItemsSource = visiteurs;
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                    MessageBox.Show(((HttpWebResponse)ex.Response).StatusCode.ToString());

            }
        }
    }
}
