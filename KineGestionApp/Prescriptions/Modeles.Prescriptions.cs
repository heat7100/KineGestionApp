using PDSGBD;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KineGestionApp 
{
    /// <summary>
    /// Contient les définitions (publiques) et implémentations (privées) des modèles de prescriptions
    /// </summary>
    public static partial class ModelesPrescriptions
    {
        /// <summary>
        /// Événement déclenché avant le changement du numéro d'une presciption
        /// </summary>
        public static event BeforeChange<IPrescription, string> SurChangementNumeroPrescription;

        /// <summary>
        /// Événement déclenché avant le changement du nombre de séances de la prescription
        /// </summary>
        public static event BeforeChange<IPrescription, int> SurChangementNombrePrescription;

        /// <summary>
        /// Événement déclenché avant le changement du montant de l'acompte de la prescription
        /// </summary>
        public static event BeforeChange<IPrescription, SqlMoney> SurChangementAcomptePrescription;

        /// <summary>
        /// Événement déclenché avant le changement de la date de la prescription
        /// </summary>
        public static event BeforeChange<IPrescription, DateTime> SurChangementDatePrescription;

        /// <summary>
        /// Événement déclenché avant le changement du statut (cloturée) de la prescription
        /// </summary>
        public static event BeforeChange<IPrescription, bool> SurChangementStatutPrescription;

        public interface IPrescription
        {
            /// <summary>
            /// Indique si toutes les caractéristiques de la prescription sont valides
            /// </summary>
            bool EstValide();

            /// <summary>
            /// Identifiant unique d'une prescription
            /// </summary>
            int Id { get; }


            /// <summary>
            /// Permet de définir l'identifiant d'une prescription si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            bool DefinirIdPrescription(int id);


            /// <summary>
            /// Nombre de séances que comporte une presciption 
            /// </summary>
            int NombreSeances { get; }

            /// <summary>
            /// Numéro de la presciption, celui-ci n'est pas l'ID !!
            /// </summary>
            string NumeroPrescription { get; }


            /// <summary>
            /// Permet de vérifier que le numéro de prescription est valide selon la nomenclature
            /// </summary>
            /// <param name="numero"></param>
            /// <returns>Retiurne vrai si valide, son faux</returns>
            bool NumeroPrescriptionValide(string numero);

            /// <summary>
            /// Permet de modifier le numéro d'une presciption définie par celui-ci
            /// </summary>
            /// <param name="id"></param>
            /// <returns>Retourne vrai si la modification a été réalisé, sinon faux</returns>
            bool ModifierNumeroPrescription(string numeroPrescription);

            /// <summary>
            /// Montant de l'acompte payé par le patient
            /// </summary>
            SqlMoney Acompte { get; }

            /// <summary>
            /// Permet de valide le montant de l'acompte
            /// </summary>
            /// <param name="acompte">Montant de l'acompte</param>
            /// <returns>Retourne vrai si le montant est accepté, sinon faux</returns>
            bool AcompteValide(SqlMoney acompte);

            /// <summary>
            /// Permet de modifier le montant de l'acompte
            /// </summary>
            /// <param name="id"></param>
            /// <returns>Retourne vrai si la modification a été réalisé, sinon faux</returns>
            bool ModifierAcompte(SqlMoney acompte);

            /// <summary>
            /// Date de prescription non unique
            /// </summary>
            DateTime DatePrescription { get; }

            /// <summary>
            /// Permet de valider la date d'une prescription
            /// </summary>
            /// <param name="date">Nouvelle date d'une prescription</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool DatePrescriptionValide(DateTime date);


            /// <summary>
            /// Permet de modifier la date d'une presciption
            /// </summary>
            /// <param name="date">Nouvelle date d'une presciption</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierDatePrescription(DateTime date);

            /// <summary>
            /// Défini par Oui/Non si la prescription est cloturée ou pas
            /// </summary>
            /// <param name="id"></param>
            /// <returns>True si oui, sinon false</returns>
            bool Cloturee { get; }

            /// <summary>
            /// Événement déclenché avant le changement du numéro d'une presciption
            /// </summary>
            event BeforeChange<string> SurChangementNumero;

            /// <summary>
            /// Événement déclenché avant le changement du nombre de séances de la prescription
            /// </summary>
            event BeforeChange<IPrescription, int> SurChangementNombre;

            /// <summary>
            /// Événement déclenché avant le changement du nombre de séances de la prescription
            /// </summary>
            event BeforeChange<IPrescription, SqlMoney> SurChangementAcompte;

            /// <summary>
            /// Événement déclenché avant le changement de la date de la prescription
            /// </summary>
            event BeforeChange<IPrescription, DateTime> SurChangementDate;

            /// <summary>
            /// Événement déclenché avant le changement du statut (cloturée) de la prescription
            /// </summary>
            event BeforeChange<IPrescription, bool> SurChangementStatut;
        }

        /// <summary>
        /// Permet de créer une nouvelle prescription dont les données ne sont pas encore définies et valides
        /// </summary>
        /// <returns>Nouvelle entité de type Prescription</returns>
        public static IPrescription CreerNouvellePrescription()
        {
            return new Prescription(0, 0, string.Empty, 0, DateTime.MinValue, false);
        }

        /// <summary>
        /// Constructeur pour un/e patient/e
        /// </summary>
        /// <param name="id">Identifiant de cette prescription</param>
        /// <param name="nombreSeances">Nombre de séances de cette prescription</param>
        /// <param name="numeroPrescription">Numéro de cette prescription</param>
        /// <param name="acompte">Acompe prépayé pour le traitement prescrit</param>
        /// <param name="datePrescription">Date de délivrance de cette prescription</param>
        /// <param name="cloturee">Statut actuel de cette prescription</param>
        /// <returns>Nouvelle entité de type IPrescription si les paramètres sont valides, sinon null</returns>
        public static IPrescription CreerPrescription(int id, int nombreSeances, string numeroPrescription, SqlMoney acompte, DateTime datePrescription, bool cloturee)
        {
            if(id < 1) return null;
            var nouvellePrescription = new Prescription(id, 0, null, SqlMoney.MinValue, DateTime.MinValue, false);
            if(!nouvellePrescription.ModifierNumeroPrescription(numeroPrescription)) return null;
            if(!nouvellePrescription.ModifierAcompte(acompte)) return null;
            return nouvellePrescription;
        }

        private class Prescription : IPrescription
        {
            /// <summary>
            /// Indique si toutes les caractéristiques de la prescription sont valides
            /// </summary>
            public bool EstValide() 
            {
                if ((Id >= 1)
                    && (NombreSeances > 0)
                    && (AcompteValide(Acompte))
                    && (DatePrescriptionValide(DatePrescription))
                    && (NumeroPrescriptionValide(NumeroPrescription)))
                    return true;
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Identifiant unique d'une prescription
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// Permet de définir l'identifiant d'une presciption si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            public bool DefinirIdPrescription(int id)
            {
                if ((Id >= 1) || (id < 1)) return false;
                Id = id;
                return true;
            }

            /// <summary>
            /// Nombre de séances que comporte une presciption 
            /// </summary>
            public int NombreSeances { get; private set; }

            /// <summary>
            /// Numéro de la presciption, celui-ci n'est pas l'ID !!
            /// </summary>
            public string NumeroPrescription { get; private set; }

            /// <summary>
            /// Permet de vérifier que le numéro de prescription est valide selon la nomenclature
            /// </summary>
            /// <param name="numero"></param>
            /// <returns>Retiurne vrai si valide, son faux</returns>
            public bool NumeroPrescriptionValide(string numero)
            {
                numero= numero.Trim();
                if ((!string.IsNullOrEmpty(numero) || (!string.IsNullOrWhiteSpace(numero) || (numero.Length  != 10)))) return false;
                return true;
            }

            /// <summary>
            /// Permet de modifier le numéro d'une presciption définie par son ID
            /// </summary>
            /// <param name="id"></param>
            /// <returns>Retourne vrai si la modification a été réalisé, sinon faux</returns>
            public bool ModifierNumeroPrescription(string numeroPrescription)
            {
                if ((!NumeroPrescriptionValide(numeroPrescription))) return false;
                using (var cancelToken = CancellationToken.GetNew())
                {
                    if (SurChangementNumeroPrescription != null)
                    {
                        SurChangementNumeroPrescription(this, NumeroPrescription, numeroPrescription, cancelToken);
                    }
                    if (SurChangementNumero != null)
                    {
                        SurChangementNumero(NumeroPrescription, numeroPrescription, cancelToken);
                    }
                    if (cancelToken.IsCancelled) return false;
                }
                NumeroPrescription = numeroPrescription;
                return true;
            }


            /// <summary>
            /// Montant de l'acompte payé par le patient
            /// </summary>
            public SqlMoney Acompte { get; private set; }

            /// <summary>
            /// Permet de valide le montant de l'acompte qui doit être de minimum 50€
            /// </summary>
            /// <param name="acompte">Montant de l'acompte</param>
            /// <returns>Retourne vrai si le montant est accepté, sinon faux</returns>
            public bool AcompteValide(SqlMoney acompte)
            {
                if(acompte.Value < 50) return false;
                return true;
            }

            /// <summary>montant de l'acompte perçu par le patient
            /// </summary>
            /// <param name="id"></param>
            /// <returns>Retourne vrai si la modification a été réalisé, sinon faux</returns>
            public bool ModifierAcompte(SqlMoney acompte)
            {
                if(acompte < 1) return false;
                Acompte = acompte;
                return true;
            }

            /// <summary>
            /// Date de prescription non unique
            /// </summary>
            public DateTime DatePrescription { get; private set; }

            /// <summary>
            /// Permet de valider la date d'une prescription
            /// </summary>
            /// <param name="date">Nouvelle date d'une prescription</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool DatePrescriptionValide(DateTime date)
            {
                DateTime dateMin = new DateTime().AddYears(-1);
                DateTime dateMax = new DateTime();
                if ((date > dateMin) || (date < dateMax)) return false;
                DatePrescription = date; return true;
            }

            /// <summary>
            /// Permet de modifier la date d'une presciption
            /// </summary>
            /// <param name="date">Nouvelle date d'une presciption</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
           public bool ModifierDatePrescription(DateTime date)
            {
                DateTime dateMin = new DateTime().AddYears(-1);
                DateTime dateMax = new DateTime();
                if ((date > dateMin) || (date < dateMax)) return false;
                DatePrescription = date; return true;
            }

            /// <summary>
            /// Défini par Oui/Non si la prescription est cloturée ou pas
            /// </summary>
            /// <param name="id"></param>
            /// <returns>True si oui, sinon false</returns>
            public bool Cloturee { get; private set; }

            /// <summary>
            /// Événement déclenché avant le changement du numéro d'une presciption
            /// </summary>
            public event BeforeChange<string> SurChangementNumero;

            /// <summary>
            /// Événement déclenché avant le changement du nombre de séances de la prescription
            /// </summary>
            public event BeforeChange<IPrescription, int> SurChangementNombre;

            /// <summary>
            /// Événement déclenché avant le changement du nombre de séances de la prescription
            /// </summary>
            public event BeforeChange<IPrescription, SqlMoney> SurChangementAcompte;

            /// <summary>
            /// Événement déclenché avant le changement de la date de la prescription
            /// </summary>
            public event BeforeChange<IPrescription, DateTime> SurChangementDate;


            /// <summary>
            /// Événement déclenché avant le changement du statut (cloturée) de la prescription
            /// </summary>
            public event BeforeChange<IPrescription, bool> SurChangementStatut;

            /// <summary>
            /// Constructeur pour un/e patient/e
            /// </summary>
            /// <param name="id">Identifiant de cette prescription</param>
            /// <param name="nombreSeances">Nombre de séances de cette prescription</param>
            /// <param name="numeroPrescription">Numéro de cette prescription</param>
            /// <param name="acompte">Acompe prépayé pour le traitement prescrit</param>
            /// <param name="datePrescription">Date de délivrance de cette prescription</param>
            /// <param name="cloturee">Statut actuel de cette prescription</param>
            public Prescription(int id, int nombreSeances, string numeroPrescription, SqlMoney acompte, DateTime datePrescription, bool cloturee)
            {
                Id = id;
                NombreSeances = nombreSeances;  
                NumeroPrescription = numeroPrescription;
                Acompte = acompte;
                DatePrescription = datePrescription;
                Cloturee = cloturee;
            }

        }

    }
}
