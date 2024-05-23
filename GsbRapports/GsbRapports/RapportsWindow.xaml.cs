using dllRapportVisites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Xml.Serialization;

namespace GsbRapports
{
    public partial class RapportsWindow : Window
    {
        private List<Rapport> rapports;

        public RapportsWindow(List<Rapport> rapports, string visiteurNomPrenom)
        {
            InitializeComponent();
            this.Title = $"Rapports de {visiteurNomPrenom}";
            this.rapports = rapports;
            this.dtgRapports.ItemsSource = rapports;
        }

        private void ExportToXML_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Rapports"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                SaveReportsToXML(filename);
            }
        }

        private void SaveReportsToXML(string filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Rapport>));
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    serializer.Serialize(writer, this.rapports);
                }
                MessageBox.Show("Les rapports ont été exportés avec succès en XML.", "Exportation réussie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'exportation des rapports : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}