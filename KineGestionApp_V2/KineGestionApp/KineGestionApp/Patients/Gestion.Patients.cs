using PDSGBD;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KineGestionApp
{
    public static partial class GestionPatients
    {

        /// <summary>
        /// Définit tout gestionnaire de patient
        /// </summary>
        public interface IPatient
        {
            /// <summary>
            /// Enumère tous les patients existants avant un potentiel ajout en DB
            /// </summary>
            /// <returns>Énumération des patients</returns>
            IEnumerable<ModelesPatients.IPatient> EnumererPatients();

            /// <summary>
            /// Permet de charger un patient selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant du patient</param>
            /// <returns>Patient chargé si possible, sinon null</returns>
            ModelesPatients.IPatient ChargerPatients(int id);

            /// <summary>
            /// Permet de retourner une nouvelle entité de type IPatient
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type IQuizz</returns>
            ModelesPatients.IPatient CreerPatients();

            /// <summary>
            /// Permet de mettre à jour (au sein du support d'informations) le patient spécifié
            /// </summary>
            /// <param name="patient">Patient à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            //bool MettreAJour(int idPatient, string nomPatient, string PrenomPatient, string CivilitePatient, DateTime DateNaissancePatient, string AdressePatient, int Patient_ID_Localite, string EmailPatient,
            //                 string TelephonePatient, string DossierPatient, Image photoPatient, int VipoPatient, string CommentairePatient, string NumeroAffiliationMutuellePatient, int Patients_ID_Mutualite);

            bool MettreAJour(ModelesPatients.IPatient patient);

            /// <summary>
            /// Permet de supprimer définitivement (au sein du support d'informations) le patient spécifié
            /// </summary>
            /// <param name="patient">Quizz à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            bool Supprimer(ModelesPatients.IPatient patient);

            /// <summary>
            /// Permet d'ajouter (au sein du support d'informations) un patient
            /// </summary>
            /// <param name="patient">Patient à ajouter</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            //bool Ajouter(string nomPatient, string PrenomPatient, string CivilitePatient, DateTime DateNaissancePatient, string AdressePatient, int Patient_ID_Localite, string EmailPatient,
            //             string TelephonePatient, string DossierPatient, string photoPatient, int VipoPatient, string CommentairePatient, string NumeroAffiliationMutuellePatient, int Patients_ID_Mutualite);
            bool Ajouter(ModelesPatients.IPatient patient);
        }

        public static IPatient CreerPatientEnDB() => new PatientEnDB();

        /// <summary>
        /// Implémente un gestionnaire des patients utilisant uniquement la mémoire comme support d'informations
        /// </summary>
        private class PatientEnDB : IPatient, IDisposable
        {

            /// <summary>
            /// Créer un nouveau patient en DB
            /// </summary>
            private static ModelesPatients.IPatient[] ParDefaut { get; } = new ModelesPatients.IPatient[]
            {
                ModelesPatients.CreerNouveauPatient()
            };
            private Dictionary<int, ModelesPatients.IPatient> enDB { get; } = new Dictionary<int, ModelesPatients.IPatient>();

            /// <summary>
            /// Enumère tous les patients
            /// </summary>
            /// <returns>Énumération des patients</returns>
            public IEnumerable<ModelesPatients.IPatient> EnumererPatientsMemoireCache()
            {
                return enDB.Values.OrderBy(patient => patient.NomPatient[0].ToString().ToUpper() + patient.NomPatient.Substring(1) + " " +
                                                       patient.PrenomPatient[0].ToString().ToUpper() + patient.PrenomPatient.Substring(1));
            }

            public IEnumerable<ModelesPatients.IPatient> EnumererPatients()
            {
                ModelesPatients.IPatient patientActuel = null;
                foreach (var enregistrement in Program.Bd.GetRows(
                    @"SELECT 
                        patients.ID_Patients AS id, 
                        patients.Nom AS nom, 
                        patients.Prenom AS prenom, 
                        patients.Civilite AS civilite, 
                        patients.Date_de_naissance AS date_Naissance, 
                        patients.Adresse AS adresse, 
                        patients.Patient_ID_Localite AS id_Localite,
                        localites.Code_postal AS code_postal, 
                        localites.Localite AS localite, 
                        patients.Email AS email, 
                        patients.Telephone AS telephone, 
                        patients.Dossier AS dossier, 
                        patients.Vipo AS vipo, 
                        patients.Patients_ID_Mutualite AS id_Mutualite,
                        mutualites.Mutualite AS mutuelle, 
                        patients.Photo AS photo,
                        patients.Commentaire AS commentaire,
                        patients.NumeroAffiliation AS numeroAffiliation
                    FROM 
                        (
                            (
                                (
                                    patients
                                    INNER JOIN localites ON patients.Patient_ID_Localite = localites.ID_Localite
                                )
                            )
                            INNER JOIN mutualites ON patients.Patients_ID_Mutualite = mutualites.ID_Mutualite
                        )
                    ORDER BY 
                        patients.Nom ASC, 
                        patients.Prenom ASC"))
                {
                    Image ImgDB = Extensions.GetImageDirectoryPC(enregistrement.GetValue<string>("photo"));
                    var patient = ModelesPatients.CreerPatient
                        (enregistrement.GetValue<int>("id"),
                         enregistrement.GetValue<string>("nom"),
                         enregistrement.GetValue<string>("prenom"),
                         enregistrement.GetValue<string>("civilite"),
                         enregistrement.GetValue<DateTime>("date_naissance"),
                         enregistrement.GetValue<string>("adresse"),
                         enregistrement.GetValue<int>("id_Localite"),
                         enregistrement.GetValue<bool>("vipo"),
                         enregistrement.GetValue<string>("email"),
                         enregistrement.GetValue<string>("telephone"),
                         enregistrement.GetValue<string>("dossier"),
                         enregistrement.GetValue<string>("commentaire"),
                         enregistrement.GetValue<string>("numeroAffiliation"),
                         enregistrement.GetValue<int>("id_Mutualite"),
                         ImgDB
                        );
                    if (patient == null) continue;
                    if(patientActuel == null)
                    {
                        if (enregistrement.GetValue("id_Mutualite") != null)
                        {
                            patientActuel = patient;
                            yield return patientActuel;
                        }
                    }
                    else if (!patient.Id.Equals(patientActuel.Id))
                    {
                        patientActuel = patient;
                        yield return patientActuel;
                    }
                    else
                    {
                        if(patientActuel != null)
                        {
                            yield return patientActuel;
                        }
                    }
                };
            }


            /// <summary>
            /// Permet de charger un patient selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant du patient</param>
            /// <returns>Patient chargé si possible, sinon null</returns>
            public ModelesPatients.IPatient ChargerPatients(int id)
            {
                return enDB.TryGetValue(id, out var patient) ? patient : null;
            }

            /// <summary>
            /// Permet de retourner une nouvelle entité de type IPatient
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type IPatient</returns>
            public ModelesPatients.IPatient CreerPatients()
            {
                return ModelesPatients.CreerNouveauPatient();
            }

            /// <summary>
            /// Permet de mettre à jour en cache le patient spécifié
            /// </summary>
            /// <param name="patient">Patient à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            public bool MettreAJourEnCache(ModelesPatients.IPatient patient)
            {
                if (patient == null) return false;
                if (patient.Id < 1)
                {
                    // Ajout si il est valide
                    patient.DefinirIdPatient((enDB.Count == 0) ? 1 : enDB.Keys.Max() + 1);
                    if (patient.EstValide())
                    {
                        enDB.Add(patient.Id, patient);
                    }
                }
                else
                {
                    // Modification
                    // ici, rien à faire car le chargement d'un patient existant ne fait que retourner la référence d'un objet déjà présent dans le dictionnaire (la BD)
                }
                return true;
            }

            public bool MettreAJour(ModelesPatients.IPatient patient)
            {
                if (patient == null) return false;
                byte[] photo;
                string imgPath;
                if (patient.PhotoPatient != null)
                {
                    photo = Extensions.ImageConversionImageToByte(patient.PhotoPatient);
                    imgPath = Extensions.SaveImageDirectoryPC(patient.PhotoPatient, "patients", patient.Id);
                }
                else
                {
                    photo = null;
                    imgPath = Extensions.SaveImageDirectoryPC(patient.PhotoPatient, "patients", patient.Id); //Image par défaut
                }
                if (patient.Id > 0)
                {
                    if(Program.Bd.Execute(@"UPDATE patients SET patients.Nom = {0}, patients.Prenom = {1}, patients.Civilite = {2}, patients.Date_de_naissance = {3}, 
                                        patients.Adresse = {4}, patients.Patient_ID_Localite = {5}, patients.Email = {6}, patients.Telephone = {7}, 
                                        Patients.Vipo = {8}, patients.Commentaire = {9}, patients.Patients_ID_Mutualite = {10}, patients.Photo = {11}, WHERE patients.ID_Patients = {12}",
                                        patient.NomPatient, patient.PrenomPatient, patient.CivilitePatient, patient.DateNaissancePatient, patient.AdressePatient, patient.Patient_ID_Localite, patient.EmailPatient,
                                        patient.TelephonePatient, patient.VipoPatient, patient.CommentairePatient, patient.Patients_ID_Mutualite, imgPath, patient.Id).RowCount == 1) return true;
                }
                return false;
            }

            /// <summary>
            /// Permet de supprimer définitivement (en DB) le patient spécifié
            /// </summary>
            /// <param name="patient">Patient à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool SupprimerMemoireCache(ModelesPatients.IPatient patient)
            {
                if (patient == null) return false;
                return enDB.Remove(patient.Id);
            }

            /// <summary>
            /// Permet de supprimer définitivement (en DB) le patient spécifié
            /// </summary>
            /// <param name="patient">Patient à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool Supprimer(ModelesPatients.IPatient patient)
            {
                if (patient == null) return false;
                if(Program.Bd.Execute("DELETE FROM patients WHERE patients.ID_Patients={0}", patient.Id).RowCount ==1) return true;
                return false;
            }

            /// <summary>
            /// Permet d'ajouter (au sein du support d'informations) un patient
            /// </summary>
            /// <param name = "patient" > Patient à ajouter</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            //public bool Ajouter(string nomPatient, string PrenomPatient, string CivilitePatient, DateTime DateNaissancePatient, string AdressePatient, int Patient_ID_Localite, string EmailPatient,
            //                                               string TelephonePatient, string DossierPatient, string photoPatient, int VipoPatient, string CommentairePatient, string NumeroAffiliationMutuellePatient, int Patients_ID_Mutualite)
            //{
            //    //ModelesPatients.IPatient patient =  ModelesPatients.CreerPatient(nomPatient, PrenomPatient, CivilitePatient, DateNaissancePatient, AdressePatient, Patient_ID_Localite, EmailPatient,
            //    // TelephonePatient, DossierPatient, photoPatient, VipoPatient, CommentairePatient, NumeroAffiliationMutuellePatient, Patients_ID_Mutualite);
            //    //if(!patient.EstValide()) return false;
            //    Dbm.Execute(@"INSERT INTO Patients ( Nom, Prenom, Civilite, Date_de_naissance, Adresse, Patient_ID_Localite, Email, Telephone, Dossier, Vipo, Commentaire, NumeroAffiliation, Patients_ID_Mutualite )  
            //                  VALUES ('{0}', '{1}', '{2}', #{3}#, '{4}', {5}, '{6}', '{7}', '{8}', {9}, '{10}', '{11}', {12});",
            //                  nomPatient, PrenomPatient, CivilitePatient, DateNaissancePatient, AdressePatient, Patient_ID_Localite, EmailPatient,
            //                  TelephonePatient, DossierPatient, VipoPatient, CommentairePatient, NumeroAffiliationMutuellePatient, Patients_ID_Mutualite);
            //    return true;
            //}


            public bool Ajouter(ModelesPatients.IPatient patient)
            {
                byte[] photo;
                string ImageNameDB;

                //L'exportation en tableau de butes de fonctionne pas avec le format BLOB en MySQL
                //if (patient.PhotoPatient != null)
                //{
                //    photo = Extensions.ImageConversionImageToByte(patient.PhotoPatient);
                //}

                Guid idTemp = Guid.NewGuid();
                if (Program.Bd.Execute(@"INSERT INTO patients ( Nom, Prenom, Civilite, Date_de_naissance, Adresse, Patient_ID_Localite, Email, Telephone, Dossier, Vipo, Commentaire, NumeroAffiliation, Photo, Patients_ID_Mutualite)  
                                         VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13})",
                                        patient.NomPatient, patient.PrenomPatient, patient.CivilitePatient, patient.DateNaissancePatient, patient.AdressePatient, patient.Patient_ID_Localite, patient.EmailPatient,
                                         patient.TelephonePatient, patient.DossierPatient, patient.VipoPatient, patient.CommentairePatient, patient.NumeroAffiliationMutuellePatient, idTemp.ToString(), patient.Patients_ID_Mutualite).RowCount == 1)
                {
                    int lastID = Program.Bd.GetValue<int>("SELECT patients.ID_Patients FROM patients WHERE patients.Photo ={0}", idTemp.ToString());
                    ImageNameDB = Extensions.SaveImageDirectoryPC(patient.PhotoPatient, "patients", (int)lastID);
                    if (Program.Bd.Execute("UPDATE patients SET patients.Photo = {0} WHERE patients.ID_Patients = {1}", ImageNameDB, lastID).RowCount == 1)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("l'Image n'a pas pu être prise en compte");
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            }


            //public PatientEnDB()
            //{
            //    ModelesPatients.SurChangementNomPatient += SurChangementNomPatient;
            //    ModelesPatients.SurChangementPrenomPatient += SurChangementPrenomPatient;
            //    ModelesPatients.SurChangementAdressePatient += SurChangementAdressePatient;
            //    ModelesPatients.SurChangementTelephonePatient += SurChangementTelephonePatient;
            //    ModelesPatients.SurChangementVipoPatient += SurChangementVipoPatient;
            //    ModelesPatients.SurChangementEmailPatient += SurChangementEmailPatient;
            //    ModelesPatients.SurChangementNumeroAffiliationMutuellePatient += SurChangementNumeroAffiliationMutuellePatient;
            //    ModelesPatients.SurChangementDateNaissancePatient += SurChangementDateNaissancePatient;
            //    ModelesPatients.SurChangementDossierPatient += SurChangementDossierPatient;
            //    ModelesPatients.SurChangementCommentairePatient += SurChangementCommentairePatient;
            //    enDB.Clear();
            //    foreach (var enregistrementPatient in ParDefaut)
            //    {
            //        var enregistrementNouveauPatient = ModelesPatients.CreerPatient(enregistrementPatient.Id, enregistrementPatient.NomPatient, enregistrementPatient.PrenomPatient, enregistrementPatient.CivilitePatient, enregistrementPatient.DateNaissancePatient, enregistrementPatient.AdressePatient, enregistrementPatient.Patient_ID_Localite,
            //                                                                        enregistrementPatient.VipoPatient, enregistrementPatient.EmailPatient, enregistrementPatient.TelephonePatient, enregistrementPatient.DossierPatient, enregistrementPatient.PhotoPatient, enregistrementPatient.CommentairePatient, enregistrementPatient.NumeroAffiliationMutuellePatient,
            //                                                                        enregistrementPatient.Patients_ID_Mutualite);
            //        if (enregistrementNouveauPatient != null) enDB.Add(enregistrementPatient.Id, enregistrementNouveauPatient);
            //    }
            //}

            ~PatientEnDB()
            {
                Dispose();
            }

            public void Dispose()
            {
                ModelesPatients.SurChangementNomPatient -= SurChangementNomPatient;
                ModelesPatients.SurChangementPrenomPatient -= SurChangementPrenomPatient;
                ModelesPatients.SurChangementAdressePatient -= SurChangementAdressePatient;
                ModelesPatients.SurChangementTelephonePatient -= SurChangementTelephonePatient;
                ModelesPatients.SurChangementVipoPatient -= SurChangementVipoPatient;
                ModelesPatients.SurChangementEmailPatient -= SurChangementEmailPatient;
                ModelesPatients.SurChangementNumeroAffiliationMutuellePatient -= SurChangementNumeroAffiliationMutuellePatient;
                ModelesPatients.SurChangementDateNaissancePatient -= SurChangementDateNaissancePatient;
                ModelesPatients.SurChangementDossierPatient -= SurChangementDossierPatient;
                ModelesPatients.SurChangementCommentairePatient -= SurChangementCommentairePatient;
            }

            private void SurChangementNomPatient(ModelesPatients.IPatient patient, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(patientEnDB => !patient.Id.Equals(patient.Id)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementPrenomPatient(ModelesPatients.IPatient patient, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(patientEnDB => !patient.Id.Equals(patient.Id)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementAdressePatient(ModelesPatients.IPatient patient, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(patientEnDB => !patient.Id.Equals(patient.Id)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementTelephonePatient(ModelesPatients.IPatient patient, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(patientEnDB => !patient.Id.Equals(patient.Id)
                     && patient.TelephonePatient.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }
            private void SurChangementVipoPatient(ModelesPatients.IPatient patient, bool valeurActuelle, bool nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(patientEnDB => !patient.Id.Equals(patient.Id)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementEmailPatient(ModelesPatients.IPatient patient, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(patientEnDB => !patient.Id.Equals(patient.Id)
                    && patient.EmailPatient.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementNumeroAffiliationMutuellePatient(ModelesPatients.IPatient patient, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(patientEnDB => !patient.Id.Equals(patient.Id)
                    && patient.NumeroAffiliationMutuellePatient.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementDateNaissancePatient(ModelesPatients.IPatient patient, DateTime valeurActuelle, DateTime nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(patientEnDB => !patient.Id.Equals(patient.Id)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementDossierPatient(ModelesPatients.IPatient patient, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(patientEnDB => !patient.Id.Equals(patient.Id)
                    && patient.DossierPatient.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementCommentairePatient(ModelesPatients.IPatient patient, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(patientEnDB => !patient.Id.Equals(patient.Id)))
                {
                    annulation.Cancel();
                }
            }
        }
    }
}
