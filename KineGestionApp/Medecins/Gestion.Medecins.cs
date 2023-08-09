using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSGBD;

namespace KineGestionApp
{
    public static partial class GestionMedecins
    {
        /// <summary>
        /// Définit tout gestionnaire de medecin
        /// </summary>
        public interface IMedecin
        {
            /// <summary>
            /// Enumère tous les médecins existants
            /// </summary>
            /// <returns>Énumération des médecins</returns>
            IEnumerable<ModelesMedecins.IMedecin> EnumererMedecins();

            /// <summary>
            /// Permet de charger un médecin selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant du médecin</param>
            /// <returns>Médecin chargé si possible, sinon null</returns>
            ModelesMedecins.IMedecin ChargerMedecins(int id);

            /// <summary>
            /// Permet de retourner une nouvelle entité de type IMedecin
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type IMedecin</returns>
            ModelesMedecins.IMedecin CreerMedecins();

            /// <summary>
            /// Permet de mettre à jour (au sein du support d'informations) le médecin spécifié
            /// </summary>
            /// <param name="medecin">Medecin à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            bool MettreAJour(ModelesMedecins.IMedecin medecin);

            /// <summary>
            /// Permet de supprimer définitivement (au sein du support d'informations) le médecin spécifié
            /// </summary>
            /// <param name="medecin">Medecin à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            bool Supprimer(ModelesMedecins.IMedecin medecin);

            /// <summary>
            /// Permet d'ajouter (au sein du support d'informations) un medecin
            /// </summary>
            /// <param name="medecin">Medecin à ajouter</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            bool Ajouter(ModelesMedecins.IMedecin medecin);
        }

        public static IMedecin CreerMedecinEnDB() => new MedecinEnDB();


        /// <summary>
        /// Implémente un gestionnaire des médecins utilisant uniquement la mémoire comme support d'informations
        /// </summary>
        private class MedecinEnDB : IMedecin, IDisposable
        {
            /// <summary>
            /// Créer un nouveau médecin en DB
            /// </summary>
            private static ModelesMedecins.IMedecin[] ParDefaut { get; } = new ModelesMedecins.IMedecin[]
            {
                ModelesMedecins.CreerNouveauMedecin()
            };

            /// <summary>
            /// Mémoire cache (non utilisée pour le moment
            /// </summary>
            private Dictionary<int, ModelesMedecins.IMedecin> enDB { get; } = new Dictionary<int, ModelesMedecins.IMedecin>();

            /// <summary>
            /// Enumère tous les médecins en mémoire cache
            /// </summary>
            /// <returns>Énumération des médecins</returns>
            public IEnumerable<ModelesMedecins.IMedecin> EnumererMedecinsEnCache()
            {
                return enDB.Values.OrderBy(medecin => medecin.NomMedecin[0].ToString().ToUpper() + medecin.NomMedecin.Substring(1));
            }

            /// <summary>
            /// Enumère tous les médecins
            /// </summary>
            /// <returns>Énumération des médecins</returns>
            public IEnumerable<ModelesMedecins.IMedecin> EnumererMedecins()
            {
                ModelesMedecins.IMedecin medecinActuel = null;
                foreach (var enregistrement in Program.Bd.GetRows(
                    @"SELECT
                            medecins.ID_Medecin AS id,
                            medecins.Nom AS nom,
                            medecins.Prenom AS prenom,
                            medecins.Civilite AS civilite,
                            medecins.Numero_INAMI AS numeroINAMI,
                            medecins.Adresse AS adresse,
                            localites.Localite AS localite,
                            localites.Code_postal AS code_postal,
                            medecins.Email AS email,
                            medecins.Medecin_ID_Localite AS id_Localite,
                            medecins.Telephone AS telephone
                        FROM
                            medecins
                            INNER JOIN localites ON medecins.Medecin_ID_Localite = localites.ID_Localite
                        ORDER BY
                            medecins.Nom ASC,
                            medecins.Prenom ASC"))
                {
                    var medecin = ModelesMedecins.CreerMedecin
                        (enregistrement.GetValue<int>("id"),
                         enregistrement.GetValue<string>("nom"),
                         enregistrement.GetValue<string>("prenom"),
                         enregistrement.GetValue<string>("civilite"),
                         enregistrement.GetValue<string>("adresse"),
                         enregistrement.GetValue<string>("numeroINAMI"),
                         enregistrement.GetValue<string>("email"),
                         enregistrement.GetValue<string>("telephone"),
                         enregistrement.GetValue<int>("id_Localite")
                        );
                    if (medecin == null) continue;
                    if (medecinActuel == null)
                    {
                        if (enregistrement.GetValue("id_Mutualite") != null)
                        {
                            medecinActuel = medecin;
                        }
                    }
                    else if (!medecin.Id.Equals(medecinActuel.Id))
                    {
                        yield return medecinActuel;
                        medecinActuel = medecin;
                    }
                    else
                    {
                        if (medecinActuel != null)
                        {
                            yield return medecinActuel;
                        }
                    }
                };
            }

            /// <summary>
            /// Permet de charger un médecin selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant du quizz</param>
            /// <returns>Médecin chargé si possible, sinon null</returns>
            public ModelesMedecins.IMedecin ChargerMedecins(int id)
            {
                return enDB.TryGetValue(id, out var medecin) ? medecin : null;
            }

            /// <summary>
            /// Permet de retourner une nouvelle entité de type IMedecin
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type IMedecin</returns>
            public ModelesMedecins.IMedecin CreerMedecins()
            {
                return ModelesMedecins.CreerNouveauMedecin();
            }

            /// <summary>
            /// Permet de mettre à jour (en DB) le patient spécifié
            /// </summary>
            /// <param name="medecin">Quizz à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            public bool MettreAJourEnCache(ModelesMedecins.IMedecin medecin)
            {
                if (medecin == null) return false;
                if (medecin.Id < 1)
                {
                    // Ajout si il est valide
                    medecin.DefinirIdMedecin((enDB.Count == 0) ? 1 : enDB.Keys.Max() + 1);
                    if (medecin.EstValide())
                    {
                        enDB.Add(medecin.Id, medecin);
                    }
                }
                else
                {
                    // Modification
                    // ici, rien à faire car le chargement d'un patient existant ne fait que retourner la référence d'un objet déjà présent dans le dictionnaire (la BD)
                }
                return true;
            }

            /// <summary>
            /// Permet de mettre à jour (en DB) le patient spécifié
            /// </summary>
            /// <param name="medecin">Quizz à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            public bool MettreAJour(ModelesMedecins.IMedecin medecin)
            {
                if (medecin == null) return false;
                if (medecin.Id >0)
                {
                    if(Program.Bd.Execute(@"UPDATE medecins SET medecins.Nom = {0}, medecins.Prenom = {1}, medecins.Civilite = {2}, 
                                  medecins.Numero_INAMI = {3}, medecins.Adresse = {4}, medecins.Email = {5}, 
                                  medecins.Medecin_ID_Localite = {6}, medecins.Telephone = {7} WHERE medecins.ID_Medecin = {8}", 
                                  medecin.NomMedecin, medecin.PrenomMedecin, medecin.CiviliteMedecin, medecin.NumeroInami, medecin.AdresseMedecin,
                                  medecin.EmailMedecin, medecin.Medecin_ID_Localite, medecin.TelephoneMedecin, medecin.Id).RowCount == 1) return true;
                }
                return false;
            }

            public bool Ajouter(ModelesMedecins.IMedecin medecin)
            {
                if (!medecin.EstValide()) return false;
                if (Program.Bd.Execute(@"INSERT INTO medecins ( Nom, Prenom, Civilite, Numero_INAMI, Adresse, Email, Medecin_ID_Localite, Telephone) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", medecin.NomMedecin, medecin.PrenomMedecin,
                                             medecin.CiviliteMedecin, medecin.NumeroInami, medecin.AdresseMedecin, medecin.EmailMedecin, medecin.Medecin_ID_Localite,
                                             medecin.TelephoneMedecin).RowCount == 1) return true;
                return false;
            }

            /// <summary>
            /// Permet de supprimer définitivement (en mémoire cache) le médecin spécifié
            /// </summary>
            /// <param name="patient">Quizz à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool SupprimerEnCache(ModelesMedecins.IMedecin medecin)
            {
                if (medecin == null) return false;
                return enDB.Remove(medecin.Id);
            }

            /// <summary>
            /// Permet de supprimer définitivement (en mémoire cache) le médecin spécifié
            /// </summary>
            /// <param name="patient">Quizz à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool Supprimer(ModelesMedecins.IMedecin medecin)
            {
                if (medecin == null) return false;
                if (Program.Bd.Execute("DELETE FROM medecins WHERE medecins.ID_Medecin = {0}", medecin.Id).RowCount == 1) return true;
                return false;
            }

            public MedecinEnDB()
            {
                ModelesMedecins.SurChangementNomMedecin += SurChangementNomMedecin;
                ModelesMedecins.SurChangementPrenomMedecin += SurChangementPrenomMedecin;
                ModelesMedecins.SurChangementAdresseMedecin += SurChangementAdresseMedecin;
                ModelesMedecins.SurChangementNumeroInamiMedecin += SurChangementNumeroInamiMedecin;
                ModelesMedecins.SurChangementTelephoneMedecin += SurChangementTelephoneMedecin;
                ModelesMedecins.SurChangementEmailMedecin += SurChangementEmailMedecin;

                enDB.Clear();
                foreach (var enregistrementMedecin in ParDefaut)
                {
                    var enregistrementNouveauMedecin = ModelesMedecins.CreerMedecin(enregistrementMedecin.Id, enregistrementMedecin.NomMedecin, enregistrementMedecin.PrenomMedecin, enregistrementMedecin.CiviliteMedecin, enregistrementMedecin.AdresseMedecin,
                                                                                    enregistrementMedecin.NumeroInami, enregistrementMedecin.EmailMedecin, enregistrementMedecin.TelephoneMedecin, enregistrementMedecin.Medecin_ID_Localite);
                    if (enregistrementNouveauMedecin != null) enDB.Add(enregistrementMedecin.Id, enregistrementNouveauMedecin);
                }
            }

            ~MedecinEnDB()
            {
                Dispose();
            }

            public void Dispose()
            {
                ModelesMedecins.SurChangementNomMedecin -= SurChangementNomMedecin;
                ModelesMedecins.SurChangementPrenomMedecin-= SurChangementPrenomMedecin;
                ModelesMedecins.SurChangementAdresseMedecin -= SurChangementAdresseMedecin;
                ModelesMedecins.SurChangementNumeroInamiMedecin -= SurChangementNumeroInamiMedecin;
                ModelesMedecins.SurChangementTelephoneMedecin -= SurChangementTelephoneMedecin;
                ModelesMedecins.SurChangementEmailMedecin -= SurChangementEmailMedecin;
            }

            private void SurChangementNomMedecin(ModelesMedecins.IMedecin medecin, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(medecinEnDB => !medecin.Id.Equals(medecin.Id)
                    && medecin.NomMedecin.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementPrenomMedecin(ModelesMedecins.IMedecin medecin, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(medecinEnDB => !medecin.Id.Equals(medecin.Id)
                    && medecin.PrenomMedecin.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementAdresseMedecin(ModelesMedecins.IMedecin medecin, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(medecinEnDB => !medecin.Id.Equals(medecin.Id)
                    && medecin.AdresseMedecin.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementNumeroInamiMedecin(ModelesMedecins.IMedecin medecin, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(medecinEnDB => !medecin.Id.Equals(medecin.Id)
                    && medecin.NumeroInami.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementTelephoneMedecin(ModelesMedecins.IMedecin medecin, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(medecinEnDB => !medecin.Id.Equals(medecin.Id)
                    && medecin.TelephoneMedecin.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementEmailMedecin(ModelesMedecins.IMedecin medecin, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(medecinEnDB => !medecin.Id.Equals(medecin.Id)
                    && medecin.EmailMedecin.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }
        }
    }
}
