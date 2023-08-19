using PDSGBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineGestionApp
{
    /// <summary>
    /// Définit tout gestionnaire de localité
    /// </summary>
    public static partial class GestionLocalites
    {

        public interface ILocalite
        {
            /// <summary>
            /// Enumère tous les localités existantes
            /// </summary>
            /// <returns>Énumération des localités</returns>
            /// 
            IEnumerable<ModelesLocalites.ILocalite> EnumererLocalites();

            /// <summary>
            /// Permet de charger une localité selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant de la localité</param>
            /// <returns>Localité chargé si possible, sinon null</returns>
            ModelesLocalites.ILocalite ChargerLocalites(int id);

            /// <summary>
            /// Permet de retourner une nouvelle entité de type ILocalite
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type ILocalite</returns>
            ModelesLocalites.ILocalite CreerLocalites();

            /// <summary>
            /// Permet de mettre à jour (en DB) la localité spécifiée
            /// </summary>
            /// <param name="localite">Mutuelle à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            bool MettreAJour(ModelesLocalites.ILocalite localite);

            /// <summary>
            /// Permet de supprimer définitivement (au sein du support d'informations) la localité spécifiée
            /// </summary>
            /// <param name="localite">Mutuelle à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            bool Supprimer(ModelesLocalites.ILocalite localite);
        }

        public static ILocalite CreerLocaliteEnDB() => new LocaliteEnDB();


        /// <summary>
        /// Implémente un gestionnaire des localités utilisant uniquement la mémoire comme support d'informations
        /// </summary>
        private class LocaliteEnDB : ILocalite, IDisposable
        {
            /// <summary>
            /// Créer une nouvelle localité en DB
            /// </summary>
            private static ModelesLocalites.ILocalite[] ParDefaut { get; } = new ModelesLocalites.ILocalite[]
            {
                ModelesLocalites.CreerNouvelleLocalite()
            };

            private Dictionary<int, ModelesLocalites.ILocalite> enDB { get; } = new Dictionary<int, ModelesLocalites.ILocalite>();

            /// <summary>
            /// Enumère tous les localités existantes en mémoire cache
            /// </summary>
            /// <returns>Énumération des localités</returns>
            public IEnumerable<ModelesLocalites.ILocalite> EnumererLocalitesMemoireCache()
            {
                return enDB.Values.OrderBy(localite => localite.NomLocalite.ToUpper() + localite.NomLocalite.Substring(1));
            }

            /// <summary>
            /// Enumère tous les localités existantes 
            /// </summary>
            /// <returns>Énumération des localités</returns>
            public IEnumerable<ModelesLocalites.ILocalite> EnumererLocalites()
            {
                
                ModelesLocalites.ILocalite localiteActuelle = null;
                foreach (var enregistrement in Program.Bd.GetRows("SELECT localites.ID_Localite AS id, localites.Code_postal AS code_postal, localites.Localite AS localite FROM localites"))
                {
                    var localite = ModelesLocalites.CreerLocalite(enregistrement.GetValue<int>("id"), enregistrement.GetValue<string>("localite"), enregistrement.GetValue<string>("code_postal"));
                    localiteActuelle = localite;
                    yield return localiteActuelle;
                    //if (localite == null) continue;
                    //if (enregistrement != null)
                    //{
                    //    //if(localiteActuelle == null)
                    //    //{
                    //        localiteActuelle = localite;
                    //        yield return localiteActuelle;
                    //    //}
                    //}
                    //else if (!localite.Id.Equals(localiteActuelle.Id))
                    //{
                    //    yield return localiteActuelle;
                    //    localiteActuelle = localite;

                    //}
                    //else
                    //{
                    //    if (localiteActuelle != null)
                    //    {
                    //        yield return localiteActuelle;
                    //    }
                    //}
                }
            }

            /// <summary>
            /// Permet de charger une localité selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant de la mutuelle</param>
            /// <returns>Quizz chargé si possible, sinon null</returns>
            public ModelesLocalites.ILocalite ChargerLocalitesEnCache(int id)
            {
                return enDB.TryGetValue(id, out var localite) ? localite : null;
            }

            /// <summary>
            /// Permet de charger une localité selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant de la mutuelle</param>
            /// <returns>Quizz chargé si possible, sinon null</returns>
            public ModelesLocalites.ILocalite ChargerLocalites(int id)
            {
                //Pour le moment, il est question de récupérer le logo pour pictureBox dans les formulaires
                ModelesLocalites.ILocalite localite = null;
                var enregistrement = Program.Bd.GetRow(@"SELECT localites.ID_Localite AS id, localites.Code_postal AS code_postal, localites.Localite AS localite
                                                         FROM localites WHERE localites.ID_Localite = {0}", id);
                localite = ModelesLocalites.CreerLocalite(enregistrement.GetValue<int>("id"), enregistrement.GetValue<string>("localite"), enregistrement.GetValue<string>("code_postal"));
                return localite;
            }

            /// <summary>
            /// Permet de retourner une nouvelle entité de type ILocalite
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type ILocalite</returns>
            public ModelesLocalites.ILocalite CreerLocalites()
            {
                return ModelesLocalites.CreerNouvelleLocalite();
            }

            /// <summary>
            /// Permet de mettre à jour (en DB) la localité spécifiée
            /// </summary>
            /// <param name="localite">Localité à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            public bool MettreAJourEnCache(ModelesLocalites.ILocalite localite)
            {
                if (localite == null) return false;
                if (localite.Id < 1)
                {
                    // Ajout si il est valide
                    localite.DefinirIdLocalite((enDB.Count == 0) ? 1 : enDB.Keys.Max() + 1);
                    if (localite.EstValide())
                    {
                        enDB.Add(localite.Id, localite);
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
            /// Permet de mettre à jour (en DB) la localité spécifiée
            /// </summary>
            /// <param name="localite">Localité à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            public bool MettreAJour(ModelesLocalites.ILocalite localite)
            {
                if (localite == null) return false;
                if (localite.Id < 1)
                {
                    // Ajout si il est valide
                    localite.DefinirIdLocalite((enDB.Count == 0) ? 1 : enDB.Keys.Max() + 1);
                    if (localite.EstValide())
                    {
                        enDB.Add(localite.Id, localite);
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
            /// Permet de supprimer définitivement (en cache) la localité spécifiée
            /// </summary>
            /// <param name="localite">Localité à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool SupprimerEnCache(ModelesLocalites.ILocalite localite)
            {
                if (localite == null) return false;
                return enDB.Remove(localite.Id);
            }

            /// <summary>
            /// Permet de supprimer définitivement (en cache) la localité spécifiée
            /// </summary>
            /// <param name="localite">Localité à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool Supprimer(ModelesLocalites.ILocalite localite)
            {
                if (localite == null) return false;
                if (Program.Bd.Execute("DELETE FROM localites WHERE localites.ID_Localite = {0}", localite.Id).RowCount == 1) return true; 
                return false;
            }

            public LocaliteEnDB()
            {
                ModelesLocalites.SurChangementNomLocalite += SurChangementNomLocalite;
                ModelesLocalites.SurChangementCodePostalLocalite += SurChangementCodePostalLocalite;

                enDB.Clear();
                foreach (var enregistrementLocalite in ParDefaut)
                {
                    var enregistrementNouvelleLocalite = ModelesLocalites.CreerLocalite(enregistrementLocalite.Id, enregistrementLocalite.NomLocalite, enregistrementLocalite.CodePostal);
                }
            }


            ~LocaliteEnDB()
            {
                Dispose();
            }

            public void Dispose()
            {
                ModelesLocalites.SurChangementNomLocalite -= SurChangementNomLocalite;
                ModelesLocalites.SurChangementCodePostalLocalite -= SurChangementCodePostalLocalite;
            }

            private void SurChangementNomLocalite(ModelesLocalites.ILocalite localite, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(localiteEnDB => !localite.Id.Equals(localite.Id)
                    && localite.NomLocalite.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementCodePostalLocalite(ModelesLocalites.ILocalite localite, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(localiteEnDB => !localite.Id.Equals(localite.Id)
                    && localite.CodePostal.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }
        }
    }
}
