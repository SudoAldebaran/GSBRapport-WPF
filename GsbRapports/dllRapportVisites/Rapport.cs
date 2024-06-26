﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dllRapportVisites
{
    public class Rapport
    {
        public int id { get; }
        public string motif { get; set; }
        public string bilan { get; set; }
        public DateTime date { get; set; }
        public int idMedecin { get; set; }
        public string idVisiteur { get; set; }
        public string nomVisiteur { get; set; }
        public string prenomVisiteur { get; set; }
        public string nomMedecin { get; set; }
        public string prenomMedecin { get; set; }

        
        public Rapport()
        {
            
        }

        public Rapport(int id /*modif*/, string motif, string bilan, DateTime date, string nomVisiteur, string prenomVisiteur, string nomMedecin, string prenomMedecin)
        {
            this.id = id; // modif
            this.motif = motif;
            this.bilan = bilan;
            this.date = date;
            this.nomMedecin = nomMedecin;
            this.prenomMedecin = prenomMedecin;
            this.prenomVisiteur = prenomVisiteur;
            this.nomVisiteur = nomVisiteur;
        }
    }
}
