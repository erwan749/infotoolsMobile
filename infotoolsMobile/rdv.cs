using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infotoolsMobile
{
    public class Rdv
    {
        // Propriétés
        public int Id { get; set; }  // ID du rendez-vous
        public string Client { get; set; }
        public string NameCom { get; set; }
        public DateTime DateRdv { get; set; }  // Date et heure du rendez-vous
    }

}
