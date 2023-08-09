using PDSGBD;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KineGestionApp
{

    /// <summary>
    /// Contient les définitions (publiques) et implémentations (privées) des modèles de patients
    /// </summary>
    public static partial class ModelesPatients
    {
        /// <summary>
        /// Événement déclenché avant le changement du nom d'un patient
        /// </summary>
        public static event BeforeChange<IPatient, string> SurChangementNomPatient;

        /// <summary>
        /// Événement déclenché avant le changement du prénom d'un patient
        /// </summary>
        public static event BeforeChange<IPatient, string> SurChangementPrenomPatient;


        /// <summary>
        /// Événement déclenché avant le changement d'adresse d'un patient
        /// </summary>
        public static event BeforeChange<IPatient, string> SurChangementAdressePatient;

        /// <summary>
        /// Événement déclenché avant le changement de téléphone d'un patient
        /// </summary>
        public static event BeforeChange<IPatient, string> SurChangementTelephonePatient;

        /// <summary>
        /// Événement déclenché avant le changement de statut Vipo d'un patient
        /// </summary>
        public static event BeforeChange<IPatient, bool> SurChangementVipoPatient;

        /// <summary>
        /// Événement déclenché avant le changement de téléphone d'un patient
        /// </summary>
        public static event BeforeChange<IPatient, string> SurChangementEmailPatient;

        /// <summary>
        /// Événement déclenché avant le changement de téléphone d'un patient
        /// </summary>
        public static event BeforeChange<IPatient, string> SurChangementNumeroAffiliationMutuellePatient;

        /// <summary>
        /// Événement déclenché avant le changement de téléphone d'un patient
        /// </summary>
        public static event BeforeChange<IPatient, DateTime> SurChangementDateNaissancePatient;

        /// <summary>
        /// Événement déclenché avant le changement de téléphone d'un patient
        /// </summary>
        public static event BeforeChange<IPatient, string> SurChangementCommentairePatient;

        /// <summary>
        /// Événement déclenché avant le changement de téléphone d'un patient
        /// </summary>
        public static event BeforeChange<IPatient, string> SurChangementDossierPatient;

        #region Interface patient
        /// <summary>
        /// Définit tout patient
        /// <para>Expose publiquement des informations et des fonctionnalités</para>
        /// </summary>
        public interface IPatient
        {
            /// <summary>
            /// Indique si toutes les caractéristiques du patient sont valides
            /// </summary>
            bool EstValide();

            /// <summary>
            /// Identifiant unique d'un patient
            /// </summary>
            int Id { get; }

            /// <summary>
            /// Permet de définir l'identifiant d'un patient si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            bool DefinirIdPatient(int id);

            /// <summary>
            /// Nom non unique d'un patient
            /// </summary>
            string NomPatient { get;}

            /// <summary>
            /// Permet de modifier le nom d'un patient
            /// </summary>
            /// <param name="nom">Nouveau nom d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierNomPatient(string nom);

            /// <summary>
            /// Permet de valider le nom d'un patient selon les critères définis
            /// </summary>
            /// <param name="nom">Nom non unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool NomPatientValidation(string nom);

            /// <summary>
            /// Prenom non unique d'un patient
            /// </summary>
            string PrenomPatient { get;}

            /// <summary>
            /// Permet de modifier le prénom d'un patien
            /// </summary>
            /// <param name="prenom">Nouveau prénom d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierPrenomPatient(string prenom);

            /// <summary>
            /// Permet de valider le prénom d'un patient selon les critères définis
            /// </summary>
            /// <param name="prenom">Prénom non unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool PrenomPatientValidation(string prenom);

            /// <summary>
            /// Civilité non unique d'un patient
            /// </summary>
            string CivilitePatient { get; }

            /// <summary>
            /// Permet de modifier la civilité d'un patient
            /// </summary>
            /// <param name="civilite">Nouvelle civilité d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierCivilitePatient(string civilite);

            /// <summary>
            /// Pemret de la valider le respect de la civilité d'un/e patient/e
            /// </summary>
            /// <param name="civilite"></param>
            /// <returns></returns>
            bool CivilitePatientValide(string civilite);

            /// <summary>
            /// Date de naissance non unique d'un patient
            /// </summary>
            DateTime DateNaissancePatient { get; }

            /// <summary>
            /// Permet de valider la date de naissance d'un patient
            /// </summary>
            /// <param name="date">Nouvelle date de naissance d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            //bool DateNaissancePatientValide(DateTime date);


            /// <summary>
            /// Permet de modifier la date de naissance d'un patient
            /// </summary>
            /// <param name="date">Nouvelle date de naissance d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            void ModifierDateNaissancePatient(DateTime date);

            /// <summary>
            /// Adresse unique d'un patient
            /// </summary>
            string AdressePatient { get; }


            /// <summary>
            /// Permet de modifier l'adresse d'un patient
            /// </summary>
            /// <param name="adresse">Nouvelle date de naissance d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierAdressePatient(string adresse);

            /// <summary>
            /// Permet de valider l'adresse d'un patient
            /// </summary>
            /// <param name="adresse">Adresse non unique d'un patient => plusieurs patients peuvent habiter à la même adresse</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool AdressePatientValidation(string adresse);

            ///<summary>
            ///Définir la localité du patient selon l'Id pour la clé étrangère
            ///</summary>
             int Patient_ID_Localite { get; }

            /// <summary>
            /// Permet de modifier la clé étrangère de la table des localités dans celle des patients
            /// Ceci est utilisé dans la cadre d'un changement d'adresse d'un patient
            /// </summary>
            /// <param name="ID_Localite">Clé étrangère</param>
            /// <returns>Vrai si le changement est accepté, sinon faux</returns>
            bool ModifierLocalitePatient(int ID_Localite);

            /// <summary>
            /// Permet de valider une localité si elle est contenue dans la table des localités
            /// </summary>
            /// <param name="patient_ID_Localite">Clé étrangère</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool LocalitePatientValidation(int patient_ID_Localite);

            /// <summary>
            /// Permet de définir si un patient a le status Vipo ou pas
            /// https://support.microsoft.com/fr-fr/office/afficher-les-valeurs-oui-non-en-utilisant-des-cases-%C3%A0-cocher-et-d-autres-contr%C3%B4les-4fa55fff-b3a0-4d03-a7a6-a2cfe4d03d4c
            /// </summary>
            /// <param name="VipoPatient">Statut Vrai ou faux</param>
            /// <returns>Vrai si status Vipo, sinon faux</returns>
            bool VipoPatient { get; }

            /// <summary>
            /// Permet de modifier le status Vipo d'un patient selon le principe d'Access
            /// Source : https://support.microsoft.com/fr-fr/office/afficher-les-valeurs-oui-non-en-utilisant-des-cases-%C3%A0-cocher-et-d-autres-contr%C3%B4les-4fa55fff-b3a0-4d03-a7a6-a2cfe4d03d4c
            /// </summary>
            /// <param name="vipo">ID unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            void ModifierVipoPatient(bool vipo);

            /// <summary>
            /// Permet de valider le statut ou pas vipo d'un patient
            /// </summary>
            /// <param name="vipo">Statut ou pas vipo d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            //bool VipoPatientValidation(bool vipo);

            ///<summary>
            ///ID de la mutualité actuelle du patient
            ///</summary>
            int Patients_ID_Mutualite { get; }

            /// <summary>
            /// Permet de modifier la mutuelle d'un patient si valide
            /// Ceci est utilisé dans la cadre d'un changement d'adresse d'un patient
            /// </summary>
            /// <param name="ID_Mutuelle">ID de la mutuelle</param>
            /// <returns>Vrai si le changement est accepté, sinon faux</returns>
            bool ModifierMutuellePatient(int ID_Mutuelle);

            /// <summary>
            /// Permet de valider une mutuellesi elle est contenue dans la table des mutualités
            /// </summary>
            /// <param name="ID_Mutuelle">Mutuelle non unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool MutuellePatientValidation(int ID_Mutuelle);


            /// <summary>
            /// Adresse courriel unique d'un patient qui sera dans une LinkArea
            /// </summary>
            string EmailPatient { get; }

            /// <summary>
            /// Permet de valider l'adresse courielle d'un patient
            /// </summary>
            /// <param name="email">Email unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool EmailPatientValide(string email);

            /// <summary>
            /// Permet de modifier l'adresse courielle d'un patient
            /// </summary>
            /// <param name="email">Email unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierEmailPatient(string email);


            /// <summary>
            /// Numéro de téléphone unique d'un patient
            /// </summary>
            string TelephonePatient { get; }

            /// <summary>
            /// Permet de modifier le numéro de téléphone d'un patient
            /// </summary>
            /// <param name="telephone">Numéro de téléphone unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierTelephonePatient(string telephone);

            /// <summary>
            /// Permet de valider le format téléphonique du numéro d'un patient
            /// </summary>
            /// <param name="telephonel">Numéro de téléphone unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool TelephonePatientValidation(string telephone);


            /// <summary>
            /// Lien unique du dossier d'un patient qui sera dans un LinkArea
            /// </summary>
            string DossierPatient { get; }

            /// <summary>
            /// Permet de modifier le lien du dossier d'un patient
            /// </summary>
            /// <param name="dossier">Lien unique du dossier d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierDossierPatient(string dossier);

            /// <summary>
            /// Permet de valider le lien du dossier d'un patient
            /// </summary>
            /// <param name="dossier">Lien du dossier en Windows</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool DossierPatientValidation(string dossier);

            /// <summary>
            /// Photo unique d'un patient qui sera dans une PictureBox
            /// </summary>
            Image PhotoPatient { get; }

            /// <summary>
            /// Permet de modifier l'image uploadée 
            /// </summary>
            /// <param name="photo">Image de la photo du patiznt</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierPhotoPatient(Image photo);

            /// <summary>
            /// Permet de valider le lien de la photo du patient et son format
            /// </summary>
            /// <param name="photo">Image de la photo du patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool PhotoPatientValidation(Image photo);

            /// <summary>
            /// Zone de commentaire unique d'un patient qui sera dans une TextBox
            /// </summary>
            string CommentairePatient { get; }

            /// <summary>
            /// Permet de modifier le text commentaire sur un patient
            /// </summary>
            /// <param name="commentaire">Lien unique de la photo d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierCommentairePatient(string commentaire);

            /// <summary>
            /// Permet de valider le commentaire d'un patient selon les régles définies
            /// </summary>
            /// <param name="commentaire">Commentaire universel sur un patient</param>
            /// <returns>Vrai si ce changement a été accepté (respect du format INAMI et unicité en DB), sinon faux</returns>
            bool CommentairePatientValidation(string commentaire);

            /// <summary>
            /// Numéro non unique d'affiliation mutuelle d'un patient
            /// Deux patients pourraient potentiellement avoir le même numéro dans des mutuelles différentes
            /// </summary>
            string NumeroAffiliationMutuellePatient { get; }

            /// <summary>
            /// Permet de modifier le text commentaire sur un patient
            /// </summary>
            /// <param name="numero">Numéro non unique d'affiliation mutuelle d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierNumeroAffiliationMutuellePatient(string numero);

            /// <summary>
            /// Permet de valider le numéro d'affiliation mutuelle d'un patient
            /// </summary>
            /// <param name="numero">Numéro mutuelle unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté (respect du format INAMI et unicité en DB), sinon faux</returns>
            bool NumeroAffiliationMutuellePatientValidation(string numero);

            /// <summary>
            /// Événement déclenché avant le changement de statut Vipo d'un patient
            /// </summary>
            event BeforeChange<IPatient, bool> SurChangementVipo;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            event BeforeChange<IPatient, string> SurChangementEmail;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            event BeforeChange<IPatient, string> SurChangementNumeroAffiliationMutuelle;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            event BeforeChange<IPatient, DateTime> SurChangementDateNaissance;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            event BeforeChange<IPatient, string> SurChangementCommentaire;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            event BeforeChange<IPatient, string> SurChangementDossier;

        }
        #endregion

        /// <summary>
        /// Permet de créer un nouveau patient dont les données ne sont pas encore définies et valides
        /// </summary>
        /// <returns>Nouvelle entité de type IQuizz</returns>
        public static IPatient CreerNouveauPatient()
        {
            return new Patient(0, string.Empty, string.Empty, "", DateTime.MinValue, string.Empty,0,false, 
                                string.Empty, string.Empty, string.Empty, null, string.Empty, string.Empty, 0);
        }


        /// <summary>
        /// Permet de créer un patient dont les caractéristiques sont connues
        /// </summary>
        /// <param name="id">Identifiant de ce nouveau patient</param>
        /// <param name="nom">Nom de ce nouveau patient</param>
        /// <param name="prenom">Nom de la partie gauche dans ce nouveau patient</param>
        /// <param name="civilitePatient">Civilité de ce patient</param>
        /// <param name="dateNaissancePatient">Date de naissance de ce patient</param>
        /// <param name="adressePatient">Adresse de ce patient </param>
        /// <param name="Patient_ID_Localite">ID de la localité du patient </param>
        /// <param name="emailPatient">Adresse courrielle de ce patient</param>
        /// <param name="telephonePatient">Numéro de téléphone de ce patient</param>
        /// <param name="dossierPatient">Lien du dossier de ce patient</param>
        /// <param name="photoPatient">Photo de ce patient</param>
        /// <param name="commentairePatient">Commentaire sur ce patient</param>
        /// <param name="numeroAffiliationMutuellePatient">Numéro d'affliation mutuelle ce patient</param>
        /// <param name="patients_ID_Mutualite">ID de la mutualité actuelle du patient</param>
        /// <returns>Nouvelle entité de type IPatient si les caractéristiques sont valides, sinon null</returns>

        public static IPatient CreerPatient(int id, string nom, string prenom, string civilitePatient, DateTime dateNaissancePatient,
                string adressePatient, int Patient_ID_Localite, bool vipoPatient, string emailPatient, string telephonePatient,
                string dossierPatient, string commentairePatient, string numeroAffiliationMutuellePatient, int patients_ID_Mutualite, Image photoPatient = null)
        {
            if (id < 1) return null;
            var nouveauPatient = new Patient(id, nom, prenom, civilitePatient, dateNaissancePatient, adressePatient, Patient_ID_Localite,
                vipoPatient, emailPatient, telephonePatient, dossierPatient, photoPatient, commentairePatient, numeroAffiliationMutuellePatient, patients_ID_Mutualite);
            if (!nouveauPatient.ModifierNomPatient(nom)) return null;
            return nouveauPatient;
        }

        private class Patient : IPatient
        {

            /// <summary>
            /// Indique si toutes les caractéristiques du patient sont valides
            /// </summary>
            ///   DateTime dateMin = new DateTime(1920, 1, 1);
            public bool EstValide()
            {
                bool testNom = NomPatientValidation(NomPatient);
                bool testPrenom = PrenomPatientValidation(PrenomPatient);
                bool testAdresse = AdressePatientValidation(AdressePatient);
                bool testLocalite = LocalitePatientValidation(Patient_ID_Localite);
                bool testEmail = EmailPatientValide(EmailPatient);
                bool testCivilite = CivilitePatientValide(CivilitePatient);
                bool testMutuelle = MutuellePatientValidation(Patients_ID_Mutualite);
                bool testAffiliation = NumeroAffiliationMutuellePatientValidation(NumeroAffiliationMutuellePatient);
                bool testTelephone = TelephonePatientValidation(TelephonePatient);
                bool testDossier = DossierPatientValidation(DossierPatient);
                bool testComment = CommentairePatientValidation(CommentairePatient);
                bool testImage = PhotoPatientValidation(PhotoPatient);
                if (
                    testNom
                    //(NomPatientValidation(NomPatient))
                    && (PrenomPatientValidation(PrenomPatient))
                    && (AdressePatientValidation(AdressePatient))
                    && (LocalitePatientValidation(Patient_ID_Localite))
                    && (EmailPatientValide(EmailPatient))
                    && (CivilitePatientValide(CivilitePatient))
                    &&(MutuellePatientValidation(Patients_ID_Mutualite))
                    && (NumeroAffiliationMutuellePatientValidation(NumeroAffiliationMutuellePatient))
                    && (TelephonePatientValidation(TelephonePatient))
                    && (DossierPatientValidation(DossierPatient))
                    //&& (VipoPatientValidation((VipoPatient))
                    && (CommentairePatientValidation(CommentairePatient))
                    &&(PhotoPatientValidation(PhotoPatient)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            /// <summary>
            /// Nom non unique d'un patient
            /// </summary>
            public string NomPatient { get; private set; }

            /// <summary>
            /// Permet de modifier le nom d'un patient
            /// </summary>
            /// <param name="nom">Nouveau nom d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierNomPatient(string nom)
            {
                nom = nom.Trim().Replace(" ", "");
                if (!NomPatientValidation(nom)) return false;
                NomPatient = nom;
                return true;
            }

            /// <summary>
            /// Permet de valider le nom d'un patient selon les critères définis
            /// </summary>
            /// <param name="nom">Nom non unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool NomPatientValidation(string nom)
            {
                //Supprime tous les espaces entres les mots, mais en laissant un espace blanc
                //string.Join(" ", nom.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                if (string.IsNullOrEmpty(nom) || (nom.Any(char.IsDigit) || (PDSGBD.Extensions.InvalideChars(nom) || (nom.Length > 25))))
                {
                    return false;
                }
                //using (var cancelToken = CancellationToken.GetNew())
                //{
                //    if (SurChangementNomPatient != null)
                //    {
                //        SurChangementNomPatient(this, NomPatient, nom, cancelToken);
                //    }
                //    if (SurChangementNom != null)
                //    {
                //        SurChangementNom(NomPatient, nom, cancelToken);
                //    }
                //    if (cancelToken.IsCancelled) return false;
                //}
                return true;
            }

            /// <summary>
            /// Prenom non unique d'un patient
            /// </summary>
            public string PrenomPatient { get; private set; }

            /// <summary>
            /// Permet de modifier le prénom d'un patient
            /// </summary>
            /// <param name="prenom">Nouveau nom de ce quizz</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierPrenomPatient(string prenom)
            {
                PrenomPatient = prenom.Trim().Replace(" ", ""); 
                if(!PrenomPatientValidation(PrenomPatient)) return false;
                return true;
            }

            /// <summary>
            /// Permet de valider le prénom d'un patient selon les critères définis
            /// </summary>
            /// <param name="prenom">Prénom non unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool PrenomPatientValidation(string prenom)
            {
                //Supprime tous les espaces entres les mots, mais en laissant un espace blanc
                //string.Join(" ", prenom.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                if (string.IsNullOrEmpty(prenom) || (prenom.Any(char.IsDigit) || (PDSGBD.Extensions.InvalideChars(prenom) || (prenom.Length > 25)))) {
                    return false;
                }
                //using (var cancelToken = CancellationToken.GetNew())
                //{
                //    if (SurChangementPrenomPatient != null)
                //    {
                //        SurChangementPrenomPatient(this, NomPatient, prenom, cancelToken);
                //    }
                //    if (SurChangementPrenomPatient != null)
                //    {
                //        SurChangementPrenom(PrenomPatient, prenom, cancelToken);
                //    }
                //    if (cancelToken.IsCancelled) return false;
                //}
                return true;
            }

            /// <summary>
            /// Identifiant unique d'un patient
            /// </summary>
            public int Id { get; private set; }


            /// <summary>
            /// Permet de définir l'identifiant d'un patient si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            public bool DefinirIdPatient(int id)
            {
                if ((Id >= 1) || (id < 1)) return false;
                Id = id;
                return true;
            }

            /// <summary>
            /// Civilité non unique d'un patient
            /// </summary>
            public string CivilitePatient{ get; private set; }

            /// <summary>
            /// Permet de modifier la civilité d'un patient
            /// </summary>
            /// <param name="civilite">Choix civilité d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierCivilitePatient(string civilite)
            {
                if (!CivilitePatientValide(civilite)) return false; 
                CivilitePatient = civilite;
                return true;
            }

            /// <summary>
            /// Pemret de la valider le respect de la civilité d'un/e patient/e
            /// </summary>
            /// <param name="civilite"></param>
            /// <returns>Vrai si ok, sinon faux</returns>
            public bool CivilitePatientValide(string civilite)
            {
                if (civilite == null) return false;
                if (civilite.Equals("Monsieur") || (civilite.Equals("Madame")))
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Date de naissance non unique d'un patient
            /// </summary>
            public DateTime DateNaissancePatient { get; private set; }


            /// <summary>
            /// Permet de valider la date de naissance d'un patient
            /// </summary>
            // <param name="date">Nouvelle date de naissance d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            //public bool DateNaissancePatientValide(DateTime date)
            //{
            //    DateTime dateMin = new DateTime(1940, 1, 1);
            //    DateTime dateMax = new DateTime(2020, 12, 31);
            //    if ((date < dateMin) || (date > dateMax))
            //    {
            //        return false;
            //    }
            //    return true;
            //}


            /// <summary>
            /// Permet de modifier la date de naissance d'un patient
            /// </summary>
            // <param name="date">Nouvelle date de naissance d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public void ModifierDateNaissancePatient(DateTime date)
            {
                //if (!DateNaissancePatientValide(date))
                //{
                //    return false;
                //}
                DateNaissancePatient= date;
                //return true;
            }

            /// <summary>
            /// Adresse unique d'un patient
            /// </summary>
            public string AdressePatient { get; private set; }


            /// <summary>
            /// Permet de modifier l'adresse d'un patient
            /// </summary>
            /// <param name="adresse">Nouvelle date de naissance d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierAdressePatient(string adresse)
            {
                adresse.Trim();
                string.Join(" ", adresse.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                if (!AdressePatientValidation(adresse)) return false;
                AdressePatient = adresse;
                return true;
            }

            /// <summary>
            /// Permet de valider l'adresse d'un patient
            /// </summary>
            /// <param name="adresse">Adresse non unique d'un patient => plusieurs patients peuvent habiter à la même adresse</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool AdressePatientValidation(string adresse)
            {
                //Supprime tous les espaces entres les mots, mais en laissant un espace blanc
                if (((string.IsNullOrEmpty(adresse) || adresse.Length > 100) || (PDSGBD.Extensions.InvalideChars(adresse)))) return false;
                bool containsInt = adresse.Any(char.IsDigit); //Un numéro doit être spécifié
                if (!containsInt) return false;
                return true;
            }

            ///<summary>
            ///Définir la localité du patient selon l'Id pour la clé étrangère
            ///</summary>
            /// <param name="Patient_ID_Localite">ID de la localité</param>
            public int Patient_ID_Localite { get; private set; }

            /// <summary>
            /// Permet de modifier la clé étrangère de la table des localités dans celle des patients.
            /// Ceci est utilisé dans la cadre d'un changement d'adresse d'un patient
            /// </summary>
            /// <param name="ID_Localite"></param>
            /// <returns>Vrai si le changement est accepté, sinon faux</returns>
            public bool ModifierLocalitePatient(int ID_Localite)
            {
                if (!LocalitePatientValidation(ID_Localite)) return false;
                Patient_ID_Localite = ID_Localite;
                return true;
            }

            /// <summary>
            /// Permet de valider une localité si elle est contenue dans la table des localités
            /// </summary>
            /// <param name="patient_ID_Localite">Adresse non unique d'un patient => plusieurs patients peuvent habiter à la même adresse</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool LocalitePatientValidation(int ID_Localite)
            {
                if (ID_Localite < 0) return false;
                if (Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM localites WHERE localites.ID_Localite = {0}", ID_Localite) >=1) return true;
                return false;
            }

            /// <summary>
            /// Permet de définir si un patient a le status Vipo ou pas
            /// </summary>
            /// <param name="vipo">Statut vipo d'un patient</param>
            /// <returns>Vrai si status Vipo, sinon faux</returns>
            public bool VipoPatient { get; private set; }

            /// <summary>
            /// Permet de modifier le status Vipo d'un patient
            /// </summary>
            /// <param name="vipo">ID unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public void ModifierVipoPatient(bool vipo)
            {
                //if (VipoPatientValidation(vipo)) return false;
                VipoPatient = vipo;
                //return true;
            }

            /// <summary>
            /// Permet de valider le statut ou pas vipo d'un patient
            /// </summary>
            /// <param name="vipo">Statut ou pas vipo d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            //public bool VipoPatientValidation(bool vipo)
            //{
            //    if (vipo)
            //    {
            //        VipoPatient = false;
            //        return true;
            //    }
            //    else if (!vipo)
            //    {
            //        VipoPatient = true;
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }

            //}

            ///<summary>
            ///ID de la mutualité actuelle du patient
            ///</summary>
            public int Patients_ID_Mutualite { get; private set; }

            /// <summary>
            /// Permet de modifier la clé étrangère de la table des localités dans celle des patients.
            /// Ceci est utilisé dans la cadre d'un changement d'adresse d'un patient
            /// </summary>
            /// <param name="ID_Mutuelle"></param>
            /// <returns>Vrai si le changement est accepté, sinon faux</returns>
            public bool ModifierMutuellePatient(int ID_Mutuelle)
            {
                if (!MutuellePatientValidation(ID_Mutuelle)) return false;
                Patients_ID_Mutualite= ID_Mutuelle;
                return true;
            }

            /// <summary>
            /// Permet de valider la mutuelle si elle est contenue dans la table des mutuelles
            /// </summary>
            /// <param name="ID_Mutuelle">Adresse non unique d'un patient => plusieurs patients peuvent habiter à la même adresse</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool MutuellePatientValidation(int ID_Mutuelle)
            {
                if(Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM mutualites WHERE mutualites.ID_Mutualite = {0}", ID_Mutuelle) >= 1) return true;
                return false;
            }


            /// <summary>
            /// Adresse courriel unique d'un patient qui sera dans une LinkArea
            /// </summary>
            public string EmailPatient { get; private set; }

            /// <summary>
            /// Permet de valider l'adresse courielle d'un patient si respect du format et unicité en DB
            /// </summary>
            /// <param name="email">Email unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux<returns>
            public bool EmailPatientValide(string email)
            {
                if (!PDSGBD.Extensions.IsEmailValide(email)) return false;
                //if(Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM patients WHERE patients.Email = {0}", email) >= 1) return false;

                return true;
            }

            /// <summary>
            /// Permet de modifier l'adresse courielle d'un patient
            /// </summary>
            /// <param name="email">Email unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierEmailPatient(string email)
            {
                email.Trim().Replace(" ", "");
                if (!EmailPatientValide(email))
                {
                    return false;
                }
                EmailPatient = email;
                return true;
            }

            /// <summary>
            /// Numéro de téléphone unique d'un patient
            /// </summary>
            public string TelephonePatient { get; private set; }

            /// <summary>
            /// Permet de modifier le numéro de téléphone d'un patient
            /// </summary>
            /// <param name="telephone">Numéro de téléphone unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierTelephonePatient(string telephone)
            {
                if(!TelephonePatientValidation(telephone)) return false;
                TelephonePatient = telephone;
                return true;
            }

            /// <summary>
            /// Permet de valider le format téléphonique du numéro d'un patient si respect du format et de l'unicité en DB
            /// </summary>
            /// <param name="telephone">Numéro de téléphone d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool TelephonePatientValidation(string telephone)
            {
                if(!PDSGBD.Extensions.TelephoneValidation(telephone)) return false;
                //if (Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM Patients WHERE Telephone LIKE {0}", telephone) >= 1) return false;

                return true;
            }

            /// <summary>
            /// Lien unique du dossier d'un patient qui sera dans un LinkArea
            /// </summary>
            public string DossierPatient { get; private set; }

            /// <summary>
            /// Permet de modifier le lien du dossier d'un patient
            /// </summary>
            /// <param name="dossier">Lien unique du dossier d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierDossierPatient(string dossier)
            {
                dossier = dossier.Trim().Replace(" ", "");
                if (!DossierPatientValidation(dossier)) return false;
                DossierPatient = dossier;
                return true;
            }

            /// <summary>
            /// Permet de valider le lien du dossier d'un patient
            /// </summary>
            /// <param name="dossier">Lien du dossier en Windows</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool DossierPatientValidation(string dossier)
            {
                if (!(dossier.Equals(PrenomPatient + "_" + NomPatient)))
                {
                    return false;
                }
                dossier = "C:\\Users\\heat7\\Documents\\Technicien_bureautique\\" + dossier + "_" + Id;
                return true;
            }

            /// <summary>
            /// Photo unique d'un patient qui sera dans une PictureBox
            /// </summary>
            public Image PhotoPatient { get; private set; }

            /// <summary>
            /// Permet de modifier le lien de la photo d'un patient
            /// </summary>
            /// <param name="photo">Lien unique de la photo d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierPhotoPatient(Image photo)
            {
                if(!PhotoPatientValidation(photo)) return false;
                PhotoPatient = photo;
                return true;
            }

            /// <summary>
            /// Permet de valider le lien de la photo du patient et son format
            /// </summary>
            /// <param name="photo">Lien du dossier en Windows</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool PhotoPatientValidation(Image photo)
            {
                if (photo == null) return true;
                ImageFormat imageFormat = photo.RawFormat;
                //La photo peut être facultative
                if (!ImageFormat.Png.Equals(imageFormat) && !ImageFormat.Jpeg.Equals(imageFormat) && !ImageFormat.Gif.Equals(imageFormat))
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Zone de commentaire unique d'un patient qui sera dans une TextBox
            /// </summary>
            public string CommentairePatient { get; private set; }

            /// <summary>
            /// Permet de modifier le text commentaire sur un patient
            /// </summary>
            /// <param name="commentaire">Lien unique de la photo d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierCommentairePatient(string commentaire)
            {
                commentaire = commentaire.Trim();
                commentaire = string.Join(" ", commentaire.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                if (!CommentairePatientValidation(commentaire)) return false;
                CommentairePatient = commentaire;
                return true;
            }

            /// <summary>
            /// Permet de valider le commentaire d'un patient selon les régles définies
            /// </summary>
            /// <param name="commentaire">Commentaire universel sur un patient</param>
            /// <returns>Vrai si ce changement a été accepté (respect du format INAMI et unicité en DB), sinon faux</returns>
            public bool CommentairePatientValidation(string commentaire)
            {
                if (commentaire.Count() > 500) return false;
                return true;
            }

            /// <summary>
            /// Numéro non unique d'affiliation mutuelle d'un patient
            /// Deux patients pourraient potentiellement avoir le même numéro dans des mutuelles différentes
            /// </summary>
            public string NumeroAffiliationMutuellePatient { get; private set; }

            /// <summary>
            /// Permet de modifier le text commentaire sur un patient
            /// </summary>
            /// <param name="numero">Numéro non unique d'affuliation mutuelle d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierNumeroAffiliationMutuellePatient(string numero)
            {
                if(!NumeroAffiliationMutuellePatientValidation(numero)) return false;
                NumeroAffiliationMutuellePatient= numero;
                return true;
            }

            /// <summary>
            /// Permet de valider le numéro d'affiliation mutuelle d'un patient si respect du format et de l'unicité en DB
            ///                 //Source : https://hellosafe.be/mutuelle/vignette#:~:text=Pour%20les%20r%C3%A9sidents%20belges%2C%20ce,de%20naissance%20de%20l'affili%C3%A9.
            /// </summary>
            /// <param name="numero">Numéro mutuelle unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté (respect du format INAMI et unicité en DB), sinon faux</returns>
            public bool NumeroAffiliationMutuellePatientValidation(string numero)
            {
                numero.Trim().Replace(" ", ""); ;
                if (numero.Count() != 11) return false;
                string mois;
                string jour;
                string annee = DateNaissancePatient.Year.ToString().Substring(2);
                if(DateNaissancePatient.Month < 10)
                {
                    mois = '0' + DateNaissancePatient.Month.ToString();
                }
                else
                {
                    mois = DateNaissancePatient.Month.ToString();
                }
                if (DateNaissancePatient.Day < 10)
                {
                    jour = '0' + DateNaissancePatient.Day.ToString();
                }
                else
                {
                    jour = DateNaissancePatient.Day.ToString();
                }

                if (!numero.StartsWith(annee + mois + jour)) return false;
                //if (Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM patients WHERE patients.NumeroAffiliation LIKE {0}", numero) >= 1) return false;
                return true;
            }

            /// <summary>
            /// Événement déclenché avant le changement du nom de ce quizz
            /// </summary>
            public event BeforeChange<string> SurChangementNom;


            /// <summary>
            /// Événement déclenché avant le changement du nom de ce quizz
            /// </summary>
            public event BeforeChange<string> SurChangementPrenom;

            /// <summary>
            /// Événement déclenché avant le changement d'adresse d'un patient
            /// </summary>
            public static event BeforeChange<IPatient, string> SurChangementAdresse;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            public static event BeforeChange<IPatient, string> SurChangementTelephone;


            /// <summary>
            /// Événement déclenché avant le changement de statut Vipo d'un patient
            /// </summary>
            public event BeforeChange<IPatient, bool> SurChangementVipo;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            public event BeforeChange<IPatient, string> SurChangementEmail;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            public event BeforeChange<IPatient, string> SurChangementNumeroAffiliationMutuelle;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            public event BeforeChange<IPatient, DateTime> SurChangementDateNaissance;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            public event BeforeChange<IPatient, string> SurChangementCommentaire;

            /// <summary>
            /// Événement déclenché avant le changement de téléphone d'un patient
            /// </summary>
            public event BeforeChange<IPatient, string> SurChangementDossier;

            /// <summary>
            /// Constructeur pour un/e patient/e
            /// </summary>
            /// <param name="id">Identifiant de ce patient</param>
            /// <param name="nom">Nom de ce patient</param>
            /// <param name="prenom">Prenom de ce patient</param>
            /// <param name="civilitePatient">Civilité de ce patient</param>
            /// <param name="dateNaissancePatient">Date de naissance de ce patient</param>
            /// <param name="adressePatient">Adresse de ce patient </param>
            /// <param name = "patient_ID_Localite"> ID de la localité</param>
            /// <param name="emailPatient">Adresse courrielle de ce patient</param>
            /// <param name="telephonePatient">Numéro de téléphone de ce patient</param>
            /// <param name="dossierPatient">Lien du dossier de ce patient</param>
            /// <param name="photoPatient">Photo de ce patient</param>
            /// <param name="commentairePatient">Commentaire sur ce patient</param>
            /// <param name="numeroAffiliationMutuellePatient">Numéro d'affliation mutuelle ce patient</param>
            /// <param name="patients_ID_Mutualite">Numéro d'affliation mutuelle ce patient</param>
            public Patient(int id, string nom, string prenom, string civilitePatient, DateTime dateNaissancePatient,
                string adressePatient, int patient_ID_Localite, bool vipoPatient, string emailPatient, string telephonePatient,
                string dossierPatient, Image photoPatient, string commentairePatient, string numeroAffiliationMutuellePatient, int patients_ID_Mutualite)
            {
                Id = id;
                NomPatient = nom;
                PrenomPatient = prenom;
                CivilitePatient = civilitePatient;
                DateNaissancePatient= dateNaissancePatient;
                AdressePatient = adressePatient;
                Patient_ID_Localite = patient_ID_Localite;
                VipoPatient = vipoPatient;
                EmailPatient = emailPatient;
                TelephonePatient = telephonePatient;
                DossierPatient= dossierPatient;
                PhotoPatient = photoPatient;
                CommentairePatient = commentairePatient;
                NumeroAffiliationMutuellePatient = numeroAffiliationMutuellePatient;
                Patients_ID_Mutualite = patients_ID_Mutualite;
            }
        }

    }
}
