using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.X509;
using System.Windows.Forms;
using PDSGBD;
using PDSGBD_MySql;

namespace KineGestionApp
{
    public static partial class GestionMutuelles
    {
        /// <summary>
        /// Définit tout gestionnaire de mutuelles
        /// </summary>
        public interface IMutuelle
        {
            /// <summary>
            /// Enumère tous les mutuelles existantes
            /// </summary>
            /// <returns>Énumération des mutuelles</returns>
            IEnumerable<ModelesMutuelles.IMutuelle> EnumererMutuelles();

            /// <summary>
            /// Permet de charger une mutuelle selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant de la mutuelle</param>
            /// <returns>Mutuelle chargée si possible, sinon null</returns>
            ModelesMutuelles.IMutuelle ChargerMutuelles(int id);

            /// <summary>
            /// Permet de retourner une nouvelle entité de type IMutuelle
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type IMutuelle</returns>
            ModelesMutuelles.IMutuelle CreerMutuelles();

            /// <summary>
            /// Permet de mettre à jour (en DB) la mutuelle spécifiée
            /// </summary>
            /// <param name="mutuelle">Mutuelle à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            bool MettreAJour(ModelesMutuelles.IMutuelle mutuelle);

            /// <summary>
            /// Permet de supprimer définitivement (au sein du support d'informations) la mutuelle spécifiée
            /// </summary>
            /// <param name="mutuelle">Mutuelle à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            bool Supprimer(ModelesMutuelles.IMutuelle mutuelle);

            /// <summary>
            /// Permet d'ajouter (au sein du support d'informations) la mutuelle spécifiée
            /// </summary>
            /// <param name="mutuelle">Mutuelle à ajouter</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            bool Ajouter(ModelesMutuelles.IMutuelle mutuelle);
        }

        public static IMutuelle CreerMutuelleEnDB() => new MutuelleEnDB();

        /// <summary>
        /// Implémente un gestionnaire de mutuelle  utilisant uniquement la mémoire comme support d'informations
        /// </summary>
        private class MutuelleEnDB :IMutuelle, IDisposable
        {
            /// <summary>
            /// Créer une nouvelle mutuelle en DB
            /// </summary>
            private static ModelesMutuelles.IMutuelle[] ParDefaut { get; } = new ModelesMutuelles.IMutuelle[]
            {
                ModelesMutuelles.CreerNouvelleMutuelle()
            };

            private Dictionary<int, ModelesMutuelles.IMutuelle> enDB { get; } = new Dictionary<int, ModelesMutuelles.IMutuelle>();

            /// <summary>
            /// Enumère tous les mutuelles existantes en mémoire cache
            /// </summary>
            /// <returns>Énumération des mutuelles</returns>
            public IEnumerable<ModelesMutuelles.IMutuelle> EnumererMutullesEnMemoireCache()
            {
                return enDB.Values.OrderBy(mutuelle => mutuelle.NomMutuelle.ToUpper() + mutuelle.NomMutuelle.Substring(1));
            }

            /// <summary>
            /// Enumère tous les mutuelles existantes en DB
            /// </summary>
            /// <returns>Énumération des mutuelles</returns>
            public IEnumerable<ModelesMutuelles.IMutuelle> EnumererMutuelles()
            {
                ModelesMutuelles.IMutuelle mutuelleActuelle = null;
                foreach (var enregistrement in Program.Bd.GetRows(@"SELECT mutualites.ID_Mutualite AS id, mutualites.Mutualite AS mutuelle, 
                                                                    mutualites.Adresse AS adresse, mutualites.Mutualite_ID_Localite AS mutualite_ID_Localite, 
                                                                    mutualites.Email AS email, mutualites.Telephone AS telephone, mutualites.Logo AS logo FROM mutualites"))
                {
                    Image ImgDB = Extensions.GetImageDirectoryPC(enregistrement.GetValue<string>("logo"));
                    var mutuelle = ModelesMutuelles.CreerMutuelle(enregistrement.GetValue<int>("id"), enregistrement.GetValue<string>("mutuelle"), enregistrement.GetValue<string>("adresse"), enregistrement.GetValue<string>("email"), enregistrement.GetValue<string>("telephone"), enregistrement.GetValue<int>("mutualite_ID_Localite"), ImgDB);
                    if (mutuelle == null) continue;
                    if (enregistrement.GetValue("id") != null)
                    {
                        //if (mutuelleActuelle == null)
                        //{
                            mutuelleActuelle = mutuelle;
                            yield return mutuelleActuelle;
                        //}
                    }
                    else if (!mutuelle.Id.Equals(mutuelleActuelle.Id))
                    {
                        mutuelleActuelle = mutuelle;
                        yield return mutuelleActuelle;
                    }
                    else
                    {
                        if (mutuelleActuelle != null)
                        {
                            yield return mutuelleActuelle;
                        }
                    }
                }
            }


            /// <summary>
            /// Permet de charger une mutuelle selon l'identifiant spécifié en mémoire cache
            /// </summary>
            /// <param name="id">Identifiant de la mutuelle</param>
            /// <returns>Mutuelle chargé si possible, sinon null</returns>
            public ModelesMutuelles.IMutuelle ChargerMutuellesEnCache(int id)
            {
                return enDB.TryGetValue(id, out var mutuelle) ? mutuelle : null;
            }

            /// <summary>
            /// Permet de charger une mutuelle selon l'identifiant spécifié en base de données
            /// </summary>
            /// <param name="id">Identifiant de la mutuelle</param>
            /// <returns>Mutuelle chargé si possible, sinon null</returns>
            public ModelesMutuelles.IMutuelle ChargerMutuelles(int id)
            {
                //Pour le moment, il est question de récupérer le logo pour pictureBox dans les formulaires
                ModelesMutuelles.IMutuelle mutuelle = null;
                var enregistrement = Program.Bd.GetRow(@"SELECT mutualites.ID_Mutualite AS id, mutualites.Mutualite AS mutuelle, 
                                                                    mutualites.Adresse AS adresse, mutualites.Mutualite_ID_Localite AS mutualite_ID_Localite, 
                                                                    mutualites.Email AS email, mutualites.Telephone AS telephone, mutualites.Logo AS logo FROM mutualites WHERE mutualites.ID_Mutualite = {0}", id + 1);
                Image ImgDB = Extensions.GetImageDirectoryPC(enregistrement.GetValue<string>("logo"));
                mutuelle = ModelesMutuelles.CreerMutuelle(enregistrement.GetValue<int>("id"), enregistrement.GetValue<string>("mutuelle"), enregistrement.GetValue<string>("adresse"), enregistrement.GetValue<string>("email"), enregistrement.GetValue<string>("telephone"), enregistrement.GetValue<int>("mutualite_ID_Localite"), ImgDB);
                    return mutuelle;
            }

            /// <summary>
            /// Permet de retourner une nouvelle entité de type IMutuelle
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type IMutuelle</returns>
            public ModelesMutuelles.IMutuelle CreerMutuelles()
            {
                return ModelesMutuelles.CreerNouvelleMutuelle();
            }

            /// <summary>
            /// Permet de mettre à jour (en cache) la mutuelle spécifiée
            /// </summary>
            /// <param name="mutuelle">Mutuelle à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            public bool MettreAJourEnCache(ModelesMutuelles.IMutuelle mutuelle)
            {
                if (mutuelle == null) return false;
                if (mutuelle.Id > 1)
                {
                    // Ajout si il est valide
                    mutuelle.DefinirIdMutuelle((enDB.Count == 0) ? 1 : enDB.Keys.Max() + 1);
                    if (mutuelle.EstValide())
                    {
                       enDB.Add(mutuelle.Id, mutuelle);
                    }
                }
                else
                {
                    // Modification
                    // ici, rien à faire car le chargement d'un quizz existant ne fait que retourner la référence d'un objet déjà présent dans le dictionnaire (le support d'informations)
                }
                return true;
            }

            /// <summary>
            /// Permet de mettre à jour (en DB) la mutuelle spécifiée
            /// </summary>
            /// <param name="mutuelle">Mutuelle à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            public bool MettreAJour(ModelesMutuelles.IMutuelle mutuelle)
            {
                if (mutuelle == null) return false;
                byte[] logo;
                string imgPath;
                if (mutuelle.LogoMutuelle != null)
                {
                    logo = Extensions.ImageConversionImageToByte(mutuelle.LogoMutuelle);

                    imgPath = Extensions.SaveImageDirectoryPC(mutuelle.LogoMutuelle, "mutuelles", mutuelle.Id);
                }
                else
                {
                    logo = null;
                    imgPath = Extensions.SaveImageDirectoryPC(mutuelle.LogoMutuelle, "mutuelles", mutuelle.Id); //Image par défaut
                }
                if (mutuelle.Id > 0)
                {
                    if(Program.Bd.Execute(@"UPDATE mutualites SET mutualites.Mutualite = {0}, mutualites.Adresse = {1}, mutualites.Mutualite_ID_Localite = {2}, 
                                         mutualites.Telephone = {3}, mutualites.Email = {4}, mutualites.Logo = {5} WHERE mutualites.ID_Mutualite ={6}",
                                  mutuelle.NomMutuelle, mutuelle.AdresseMutuelle, mutuelle.Mutuelle_ID_Localite, mutuelle.TelephoneMutuelle, mutuelle.EmailMutuelle, imgPath, mutuelle.Id).RowCount == 1) return true;
                }
                return false;
            }


            /// <summary>
            /// Permet de supprimer définitivement (au sein du support d'informations) la mutuelle spécifiée
            /// </summary>
            /// <param name="mutuelle">Mutuelle à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool SupprimerEnCache(ModelesMutuelles.IMutuelle mutuelle)
            {
                if(mutuelle == null) return false;
                return enDB.Remove(mutuelle.Id);
            }

            /// <summary>
            /// Permet de supprimer définitivement (au sein du support d'informations) la mutuelle spécifiée
            /// </summary>
            /// <param name="mutuelle">Mutuelle à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool Supprimer(ModelesMutuelles.IMutuelle mutuelle)
            {
                if (mutuelle == null) return false;
                if(Program.Bd.Execute("DELETE FROM mutualites WHERE mutualites.ID_Mutualite = {0}", mutuelle.Id).RowCount == 1) return true;
                return false;
            }

            /// <summary>
            /// Permet d'ajouter (au sein du support d'informations) la mutuelle spécifiée
            /// </summary>
            /// <param name="mutuelle">Mutuelle à ajouter</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool Ajouter(ModelesMutuelles.IMutuelle mutuelle)
            {
                byte[] logo;
                string ImageNameDB;

                //SqlParameter parameter = new SqlParameter("@ImageData", System.Data.SqlDbType.VarBinary, logo.Length);
                //parameter.Value = logo;
                Guid idTemp = Guid.NewGuid();
                if (Program.Bd.Execute(@"INSERT INTO mutualites ( Mutualite, Adresse, Mutualite_ID_Localite, Telephone, Email, Logo)  
                                        VALUES ({0}, {1}, {2}, {3}, {4}, {5})", mutuelle.NomMutuelle, mutuelle.AdresseMutuelle,
                             mutuelle.Mutuelle_ID_Localite, mutuelle.TelephoneMutuelle, mutuelle.EmailMutuelle, idTemp.ToString()).RowCount == 1)
                {
                    int lastID = Program.Bd.GetValue<int>("SELECT mutualites.ID_Mutualite FROM mutualites WHERE mutualites.Logo ={0}", idTemp.ToString());
                    //logo = Extensions.ImageConversionImageToByte(mutuelle.LogoMutuelle);
                    ImageNameDB = Extensions.SaveImageDirectoryPC(mutuelle.LogoMutuelle, "mutuelles", (int)lastID);
                    if (Program.Bd.Execute("UPDATE mutualites SET mutualites.Logo = {0} WHERE mutualites.ID_Mutualite = {1}", ImageNameDB, lastID).RowCount == 1)
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


                //SqlConnection connection = new SqlConnection("user=Sam;password=mZTbURtCucb92Grf;server=localhost;database=kinegestionapp");
                //connection.Open();
                //SqlCommand cmd = connection.CreateCommand();
                //cmd.CommandText = @"INSERT INTO mutualites ( Mutualite, Adresse, Mutualite_ID_Localite, Telephone, Email, Logo)  
                //                      VALUES (@Mutualite, @Adresse, @Mutualite_ID_Localite, @Telephone, @Email, @Logo)";

                //parameter.Value = logo;
                //cmd.Parameters.AddWithValue("Mutualite", mutuelle.NomMutuelle);
                //cmd.Parameters.AddWithValue("Adresse", mutuelle.AdresseMutuelle);
                //cmd.Parameters.AddWithValue("Mutualite_ID_Localite", mutuelle.Mutuelle_ID_Localite);
                //cmd.Parameters.AddWithValue("Telephone", mutuelle.TelephoneMutuelle);
                //cmd.Parameters.AddWithValue("Email", mutuelle.EmailMutuelle);
                //cmd.Parameters.AddWithValue("Logo", parameter);
                //cmd.ExecuteNonQuery();
                //connection.Close();
                return false;
            }

            ~MutuelleEnDB()
            {
                Dispose();
            }

            public void Dispose()
            {
                ModelesMutuelles.SurChangementNomMutuelle -= SurChangementNomMutuelle;
                ModelesMutuelles.SurChangementAdresseMutuelle-= SurChangementAdresseMutuelle;
                ModelesMutuelles.SurChangementEmailMutuelle -= SurChangementEmailMutuelle;
                ModelesMutuelles.SurChangementTelephoneMutuelle -= SurChangementTelephoneMutuelle;
                ModelesMutuelles.SurChangementLogoMutuelle -= SurChangementLogoMutuelle;
            }


            //public MutuelleEnDB()
            //{
            //    ModelesMutuelles.SurChangementNomMutuelle += SurChangementNomMutuelle;
            //    ModelesMutuelles.SurChangementAdresseMutuelle += SurChangementAdresseMutuelle;
            //    ModelesMutuelles.SurChangementEmailMutuelle += SurChangementEmailMutuelle;
            //    ModelesMutuelles.SurChangementTelephoneMutuelle += SurChangementTelephoneMutuelle;
            //    ModelesMutuelles.SurChangementLogoMutuelle += SurChangementLogoMutuelle;
            //    enDB.Clear();
            //    foreach (var enregistrementMutuelle in ParDefaut)
            //    {
            //        var enregistrementNouvelleMutuelle = ModelesMutuelles.CreerMutuelle(enregistrementMutuelle.Id, enregistrementMutuelle.NomMutuelle, enregistrementMutuelle.AdresseMutuelle, enregistrementMutuelle.EmailMutuelle, enregistrementMutuelle.TelephoneMutuelle, enregistrementMutuelle.Mutuelle_ID_Localite, ImageConversionImageToByte(enregistrementMutuelle.LogoMutuelle));
            //        if (enregistrementNouvelleMutuelle != null) enDB.Add(enregistrementMutuelle.Id, enregistrementNouvelleMutuelle);
            //    }
            //}

            private void SurChangementNomMutuelle(ModelesMutuelles.IMutuelle mutuelle, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(mutuelleEnDB => !mutuelle.Id.Equals(mutuelle.Id)
                    && mutuelle.NomMutuelle.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementAdresseMutuelle(ModelesMutuelles.IMutuelle mutuelle, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(mutuelleEnDB => !mutuelle.Id.Equals(mutuelle.Id)
                    && mutuelle.AdresseMutuelle.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementEmailMutuelle(ModelesMutuelles.IMutuelle mutuelle, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(mutuelleEnDB => !mutuelle.Id.Equals(mutuelle.Id)
                    && mutuelle.EmailMutuelle.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementTelephoneMutuelle(ModelesMutuelles.IMutuelle mutuelle, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(mutuelleEnDB => !mutuelle.Id.Equals(mutuelle.Id)
                    && mutuelle.TelephoneMutuelle.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementLogoMutuelle(ModelesMutuelles.IMutuelle mutuelle, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(mutuelleEnDB => !mutuelle.Id.Equals(mutuelle.Id)
                    && mutuelle.LogoMutuelle.Equals(mutuelleEnDB.LogoMutuelle)));
                {
                    annulation.Cancel();
                }
            }
        }
    }
}
