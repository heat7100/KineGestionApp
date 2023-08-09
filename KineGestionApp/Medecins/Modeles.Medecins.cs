using PDSGBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineGestionApp
{
    /// <summary>
    /// Contient les définitions (publiques) et implémentations (privées) des modèles de médecins
    /// </summary>
    public static partial class ModelesMedecins
    {
        /// <summary>
        /// Événement déclenché avant le changement du nom d'un médecin
        /// </summary>
        public static event BeforeChange<IMedecin, string> SurChangementNomMedecin;

        /// <summary>
        /// Événement déclenché avant le changement du nom d'un médecin
        /// </summary>
        public static event BeforeChange<IMedecin, string> SurChangementPrenomMedecin;

        /// <summary>
        /// Événement déclenché avant le changement du nom d'un médecin
        /// </summary>
        public static event BeforeChange<IMedecin, string> SurChangementAdresseMedecin;

        /// <summary>
        /// Événement déclenché avant le changement du nom d'un médecin
        /// </summary>
        public static event BeforeChange<IMedecin, string> SurChangementNumeroInamiMedecin;

        /// <summary>
        /// Événement déclenché avant le changement du nom d'un médecin
        /// </summary>
        public static event BeforeChange<IMedecin, string> SurChangementEmailMedecin;

        /// <summary>
        /// Événement déclenché avant le changement du nom d'un médecin
        /// </summary>
        public static event BeforeChange<IMedecin, string> SurChangementTelephoneMedecin;

        #region Interface medecin
        /// <summary>
        /// Définit tout patient
        /// <para>Expose publiquement des informations et des fonctionnalités</para>
        /// </summary>
        public interface IMedecin
        {
            /// <summary>
            /// Indique si toutes les caractéristiques du medecin sont valides
            /// </summary>
            bool EstValide();

            /// <summary>
            /// Identifiant unique d'un médecin
            /// </summary>
            int Id { get; }

            /// <summary>
            /// Permet de définir l'identifiant d'un medecin si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            bool DefinirIdMedecin(int id);

            /// <summary>
            /// Nom non unique d'un médecin
            /// </summary>
            string NomMedecin { get; }

            /// <summary>
            /// Permet de modifier le nom d'un médecin
            /// </summary>
            /// <param name="nom">Nouveau nom d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierNomMedecin(string nom);


            /// <summary>
            /// Permet de valider le nom d'un médecin selon les critères définis
            /// </summary>
            /// <param name="nom">Nom non unique d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool NomMedecinValidation(string nom);

            /// <summary>
            /// Prenom non unique d'un médecin
            /// </summary>
            string PrenomMedecin { get; }

            /// <summary>
            /// Permet de modifier le prénom d'un médecin
            /// </summary>
            /// <param name="prenom">Nouveau prénom d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierPrenomMedecin(string prenom);


            /// <summary>
            /// Permet de valider le prénom d'un médecin selon les critères définis
            /// </summary>
            /// <param name="prenom">Prénom non unique d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool PrenomMedecinValidation(string prenom);

            /// <summary>
            /// Civilité non unique d'un patient
            /// </summary>
            string CiviliteMedecin { get; }

            /// <summary>
            /// Permet de modifier la civilité d'un patient
            /// </summary>
            /// <param name="civilite">Nouvelle civilité d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierCiviliteMedecin(string civilite);

            /// <summary>
            /// Adresse unique d'un médecin
            /// </summary>
            string AdresseMedecin { get; }

            ///<summary>
            ///Définir la localité du patient selon l'Id pour la clé étrangère
            ///</summary>
            int Medecin_ID_Localite { get;}

            /// <summary>
            /// Permet de modifier la clé étrangère de la table des localités dans celle des patients.
            /// Ceci est utilisé dans la cadre d'un changement d'adresse d'un patient
            /// </summary>
            /// <param name="medecin_ID_Localite">Clé de la localité sélectionnée</param>
            /// <returns>Vrai si le changement est accepté, sinon faux</returns>
            bool ModifierLocaliteMedecin(int ID_Localite);

            /// <summary>
            /// Permet de valider une localité si elle est contenue dans la table des localités
            /// </summary>
            /// <param name="ID_Localite">Clé de la localité sélectionnée</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool LocaliteMedecinValidation(int ID_Localite);


            /// <summary>
            /// Permet de modifier l'adresse d'un médecin
            /// </summary>
            /// <param name="adresse">Nouvelle adresse d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierAdresseMedecin(string adresse);

            /// <summary>
            /// Permet de valider l'adresse d'un patient
            /// </summary>
            /// <param name="adresse">Adresse non unique d'un patient => plusieurs patients peuvent habiter à la même adresse</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool AdresseMedecinValidation(string adresse);

            /// <summary>
            /// Numéro INAMI unique d'un médecin
            /// </summary>
            string NumeroInami { get; }

            /// <summary>
            /// Permet de vérifier si le numéro INAMI respectant le formatage gouvernemental
            /// </summary>
            bool numeroInamiValide(string numeroInami);

            /// <summary>
            /// Permet de modifier le numéro INAMI d'un médecin en respectant le formatage gouvernemental
            /// </summary>
            /// <param name="adresse">Nouvelle date de naissance d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierNumeroInamiMedecin(string numeroInami);


            /// <summary>
            /// Adresse courriel unique d'un patient qui sera dans une LinkArea
            /// </summary>
            string EmailMedecin { get; }

            /// <summary>
            /// Permet de valider l'adresse courielle d'un médecin
            /// </summary>
            /// <param name="email">Email unique d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool EmailMedecinValide(string email);

            /// <summary>
            /// Permet de modifier l'adresse courielle d'un médecin
            /// </summary>
            /// <param name="email">Email unique d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierEmailMedecin(string email);

            /// <summary>
            /// Numéro de téléphone unique d'un médecin
            /// </summary>
            string TelephoneMedecin { get; }

            /// <summary>
            /// Permet de modifier le numéro de téléphone d'un médecin
            /// </summary>
            /// <param name="telephone">Numéro de téléphone unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierTelephoneMedecin(string telephone);

            /// <summary>
            /// Permet de valider le numéro de téléphone d'un médecin
            /// </summary>
            /// <param name="telephone">Email unique d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool TelephoneMedecinValidation(string telephone);


            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
            event BeforeChange<string> SurChangementPrenom;

            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
            event BeforeChange<IMedecin, string> SurChangementAdresse;

            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
           event BeforeChange<IMedecin, bool> SurChangementNumeroInami;

            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
            event BeforeChange<IMedecin, string> SurChangementEmail;

            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
           event BeforeChange<IMedecin, string> SurChangementTelephone;

        }
        #endregion

        /// <summary>
        /// Permet de créer un nouveau quizz dont les caractéristiques ne sont pas encore définies et valides
        /// </summary>
        /// <returns>Nouvelle entité de type IQuizz</returns>
        public static IMedecin CreerNouveauMedecin()
        {
            return new Medecin(0, string.Empty, string.Empty, "", string.Empty, string.Empty, string.Empty, string.Empty, 0);
        }

        /// <summary>
        /// Permet de créer un médecin dont les caractéristiques sont connues
        /// </summary>
        /// <param name="id">Identifiant de ce nouveau médecin</param>
        /// <param name="nom">Nom de ce nouveau médecin</param>
        /// <param name="prenom">Nom de la partie gauche dans ce nouveau médecin</param>
        /// <param name="civiliteMedecin">Civilité de ce medecin</param>
        /// <param name="adresseMedecin">Adresse de ce médecin </param>
        /// <param name="numeroInami">Adresse de ce médecin </param>
        /// <param name="emailMedecin">Adresse courrielle de ce médecin</param>
        /// <param name="telephoneMedecin">Numéro de téléphone de ce patient</param>
        /// <returns>Nouvelle entité de type IPatient si les caractéristiques sont valides, sinon null</returns>

        public static IMedecin CreerMedecin(int id, string nom, string prenom, string civiliteMedecin, string adresseMedecin, string numeroInami, string emailMedecin, string telephoneMedecin, int medecin_ID_Localite)
        {
            if (id < 1) return null;
            var nouveauPatient = new Medecin(id, nom, prenom, civiliteMedecin, adresseMedecin, numeroInami, emailMedecin, telephoneMedecin, medecin_ID_Localite);
            return nouveauPatient;
        }

        private class Medecin : IMedecin
        {
            public bool EstValide()
            {
                if (
                    (NomMedecinValidation(NomMedecin))
                    && (PrenomMedecinValidation(PrenomMedecin))
                    && (CiviliteMedecintValide(CiviliteMedecin))
                    && (EmailMedecinValide(EmailMedecin))
                    &&(numeroInamiValide(NumeroInami))
                    &&(AdresseMedecinValidation(AdresseMedecin))
                    &&(LocaliteMedecinValidation(Medecin_ID_Localite))
                    && (TelephoneMedecinValidation(TelephoneMedecin)))
                    return true;
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Identifiant unique d'un médecin
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// Permet de définir l'identifiant d'un medecin si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            public bool DefinirIdMedecin(int id)
            {
                if ((Id >= 1) || (id < 1)) return false;
                Id = id;
                return true;
            }

            /// <summary>
            /// Nom non unique d'un médecin
            /// </summary>
            public string NomMedecin { get; private set; }

            /// <summary>
            /// Permet de modifier le nom d'un médecin
            /// </summary>
            /// <param name="nom">Nouveau nom d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierNomMedecin(string nom)
            {
                nom = nom.Trim();
                string.Join(" ", nom.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                if (!NomMedecinValidation(nom)) return false;
                NomMedecin = nom;
                return true;
            }

            public bool NomMedecinValidation(string nom)
            {
                if (string.IsNullOrEmpty(nom) || (nom.Any(char.IsDigit) || (PDSGBD.Extensions.InvalideChars(nom) || (nom.Length > 25))))
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Prenom non unique d'un médecin
            /// </summary>
            public string PrenomMedecin { get; private set; }

            /// <summary>
            /// Permet de modifier le prénom d'un médecin
            /// </summary>
            /// <param name="prenom">Nouveau prénom d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierPrenomMedecin(string prenom)
            {
                prenom = prenom.Trim();
                string.Join(" ", prenom.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                if(!PrenomMedecinValidation(prenom)) return false;
                PrenomMedecin = prenom;
                return true;
            }

            public bool PrenomMedecinValidation(string prenom)
            {
                if (string.IsNullOrEmpty(prenom) || (prenom.Any(char.IsDigit) || (PDSGBD.Extensions.InvalideChars(prenom) || (prenom.Length > 25))))
                {
                    return false;
                }
                return true;
            }


            /// <summary>
            /// Civilité non unique d'un patient
            /// </summary>
            public string CiviliteMedecin { get; private set; }

            /// <summary>
            /// Pemret de la valider le respect de la civilité d'un médecin
            /// </summary>
            /// <param name="civilite"></param>
            /// <returns>vrai si ok, sinon faux</returns>
            public bool CiviliteMedecintValide(string civilite)
            {
                if (civilite == null) return false;
                if (civilite.Equals("Monsieur") || (civilite.Equals("Madame"))) return true;

                return false;
            }

            /// <summary>
            /// Permet de modifier la civilité d'un patient
            /// </summary>
            /// <param name="civilite">Nouvelle civilité d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierCiviliteMedecin(string civilite)
            {
                if (!CiviliteMedecintValide(civilite))
                {
                    return false;
                }
                CiviliteMedecin= civilite;
                return true;
            }

            /// <summary>
            /// Adresse unique d'un médecin
            /// </summary>
            public string AdresseMedecin { get; private set; }


            /// <summary>
            /// Permet de modifier l'adresse d'un médecin
            /// </summary>
            /// <param name="adresse">Nouvelle date de naissance d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierAdresseMedecin(string adresse)
            {
                adresse.Trim();
                string.Join(" ", adresse.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                if (!AdresseMedecinValidation(adresse)) return false;
                AdresseMedecin= adresse; return true;
            }

            /// <summary>
            /// Permet de valider l'adresse d'un patient
            /// </summary>
            /// <param name="adresse">Adresse non unique d'un patient => plusieurs patients peuvent habiter à la même adresse</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool AdresseMedecinValidation(string adresse)
            {
                if (((string.IsNullOrEmpty(adresse) || adresse.Length > 100) || (PDSGBD.Extensions.InvalideChars(adresse)))) return false;
                bool containsInt = adresse.Any(char.IsDigit); //Un numéro doit être spécifié
                if (!containsInt) return false;
                return true;
            }

            ///<summary>
            ///Définir la localité du patient selon l'Id pour la clé étrangère
            ///</summary>
            public int Medecin_ID_Localite { get; private set; }

            /// <summary>
            /// Permet de modifier la clé étrangère de la table des localités dans celle des patients.
            /// Ceci est utilisé dans la cadre d'un changement d'adresse d'un patient
            /// </summary>
            /// <param name="ID_Localite">Clée de la localité sélectionnée</param>
            /// <returns>Vrai si le changement est accepté, sinon faux</returns>
            public bool ModifierLocaliteMedecin(int ID_Localite)
            {
                if(!LocaliteMedecinValidation(ID_Localite)) return false;
                Medecin_ID_Localite = ID_Localite;
                return true;
                
            }

            /// <summary>
            /// Permet de valider une localité si elle est contenue dans la table des localités
            /// </summary>
            /// <param name="ID_Localite">Adresse non unique d'un patient => plusieurs patients peuvent habiter à la même adresse</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool LocaliteMedecinValidation(int ID_Localite)
            {
                if(ID_Localite < 0) return false;
                if (Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM localites WHERE localites.ID_Localite = {0}", ID_Localite) >= 1) return true; 
                return false;
            }

            /// <summary>
            /// Numéro INAMI unique d'un médecin
            /// </summary>
            public string NumeroInami { get; private set; }

            /// <summary>
            /// Permet de modifier le numéro INAMI d'un médecin en respectant le formatage gouvernemental
            /// </summary>
            /// <param name="numeroInami">Nouveeau nuémro INAMI d'un médecin, quoi que normalement, celui-ci ne sera jamais modifié jusqu'à terme de sa carrière</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool numeroInamiValide(string numeroInami)
            {
                //Source : https://www.inami.fgov.be/fr/professionnels/autres/fournisseurs-logiciels/Pages/default.aspx#:~:text=Le%20num%C3%A9ro%20INAMI%20est%20une,unique%20qui%20distingue%20chaque%20dispensateur.
                numeroInami = numeroInami.Trim().Replace(" ", "");
                if (numeroInami.Count() != 8) return false;
                int n = int.Parse(numeroInami.Substring(0, 6));
                int[] tab = new int[] { 97, 89, 83, 79 };
                Random rdn = new Random();
                int index = rdn.Next(tab.Count());
                int m = tab[index];
                int c = m - (n % m);

                if((c < 1) || (c > (n % m)))
                {
                    return false;
                }
                else
                {
                    //    return Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM medecins WHERE medecins.Numero_INAMI Like {0}", numeroInami) >= 1;
                    return true;
                }
            }

            public bool ModifierNumeroInamiMedecin(string numeroInami)
            {
                if (!numeroInamiValide(numeroInami))
                {
                    return false;
                }
                NumeroInami = numeroInami;
                return true;
            }

            /// <summary>
            /// Adresse courriel unique d'un patient qui sera dans une LinkArea
            /// </summary>
            public string EmailMedecin { get; private set; }

            /// <summary>
            /// Permet de valider l'adresse courielle d'un médecin
            /// </summary>
            /// <param name="email">Email unique d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool EmailMedecinValide(string email)
            {
                if (!Extensions.IsEmailValide(email))
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Permet de modifier l'adresse courielle d'un médecin
            /// </summary>
            /// <param name="email">Email unique d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierEmailMedecin(string email) 
            {
                if (!EmailMedecinValide(email))
                {
                    return false;
                }
                EmailMedecin= email;
                return true;
            }

            /// <summary>
            /// Numéro de téléphone unique d'un médecin
            /// </summary>
            public string TelephoneMedecin { get; private set; }

            /// <summary>
            /// Permet de modifier le numéro de téléphone d'un médecin
            /// </summary>
            /// <param name="telephone">Numéro de téléphone unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierTelephoneMedecin(string telephone)
            {
                if (!TelephoneMedecinValidation(telephone)) return false;
                TelephoneMedecin = telephone; return true;
            }

            /// <summary>
            /// Permet de valider le numéro de téléphone d'un médecin
            /// </summary>
            /// <param name="telephone">Email unique d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool TelephoneMedecinValidation(string telephone)
            {
                if (!Extensions.TelephoneValidation(telephone)) return false;
                return true;
            }

            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
            public event BeforeChange<string> SurChangementNom;


            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
            public event BeforeChange<string> SurChangementPrenom;

            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
            public event BeforeChange<IMedecin, string> SurChangementAdresse;

            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
            public event BeforeChange<IMedecin, bool> SurChangementNumeroInami;

            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
            public event BeforeChange<IMedecin, string> SurChangementEmail;

            /// <summary>
            /// Événement déclenché avant le changement du nom d'un médecin
            /// </summary>
            public event BeforeChange<IMedecin, string> SurChangementTelephone;

            /// <summary>
            /// Constructeur pour un/e patient/e
            /// </summary>
            /// <param name="id">Identifiant de ce patient</param>
            /// <param name="nom">Nom de ce patient</param>
            /// <param name="prenom">Prenom de ce patient</param>
            /// <param name="civiliteMedecin">Civilité de ce patient</param>
            /// <param name="adresseMedecin">Adresse de ce patient </param>
            /// <param name="numeroInami">Numéro INAMI du médecin</param>
            /// <param name="emailMedecin">Adresse courrielle de ce patient</param>
            /// <param name="telephoneMedecin">Numéro de téléphone de ce patient</param>
            /// <param name="medecin_ID_Localite">Clé étrangère de la localité</param>
            public Medecin(int id, string nom, string prenom, string civiliteMedecin, string adresseMedecin, string numeroInami, string emailMedecin, string telephoneMedecin, int medecin_ID_Localite)
            {
                Id = id;
                NomMedecin= nom;
                PrenomMedecin= prenom;
                CiviliteMedecin= civiliteMedecin;
                AdresseMedecin = adresseMedecin;
                NumeroInami= numeroInami;
                EmailMedecin= emailMedecin;
                TelephoneMedecin= telephoneMedecin;
                Medecin_ID_Localite = medecin_ID_Localite;
            }
        }
    }
}
