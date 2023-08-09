using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSGBD;

namespace KineGestionApp
{
    public partial class GestionNomenclatures
    {
        /// <summary>
        /// Définit toute nomenclature
        /// </summary>
        public interface INomenclature
        {
            /// <summary>
            /// Enumère tous les nomenclatures existantes
            /// </summary>
            /// <returns>Énumération des nomenclatures</returns>
            /// <summary>
            /// Enumère tous les localités existantes
            /// </summary>
            /// <returns>Énumération des nomenclatures</returns>
            /// 
            IEnumerable<ModelesNomenclatures.INomenclature> EnumererNomenclatures();

            /// <summary>
            /// Permet de charger une nomenclature selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant de la nomenclature</param>
            /// <returns>Nomenclature chargée si possible, sinon null</returns>
            ModelesNomenclatures.INomenclature ChargerNomenclatures(int id);

            /// <summary>
            /// Permet de retourner une nouvelle entité de type ILocalite
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type ILocalite</returns>
            ModelesNomenclatures.INomenclature CreerNomenclature();

            /// <summary>
            /// Permet de mettre à jour (en DB) la nomenclature spécifiée
            /// </summary>
            /// <param name="nomenclature">Nomenclature à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            bool MettreAJour(ModelesNomenclatures.INomenclature nomenclature);

            /// <summary>
            /// Permet de supprimer définitivement (en DB) la nomenclature spécifiée
            /// </summary>
            /// <param name="nomenclature">Mutuelle à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            bool Supprimer(ModelesNomenclatures.INomenclature nomenclature);
        }

        public static INomenclature CreerNomenclatureEnDB() => new NomenclatureEnDB();

        /// <summary>
        /// Implémente un gestionnaire des localités utilisant uniquement la mémoire comme support d'informations
        /// </summary>
        private class NomenclatureEnDB : INomenclature, IDisposable
        {
            /// <summary>
            /// Créer une nouvelle nomenclature en DB
            /// </summary>
            private static ModelesNomenclatures.INomenclature[] ParDefaut { get; } = new ModelesNomenclatures.INomenclature[]
            {
                ModelesNomenclatures.CreerNouvelleNomenclature()
            };

            private Dictionary<int, ModelesNomenclatures.INomenclature> enDB { get; } = new Dictionary<int, ModelesNomenclatures.INomenclature>();

            /// <summary>
            /// Enumère tous les nomenclatures existantes en mémoire cache
            /// </summary>
            /// <returns>Énumération des nomenclatures</returns>
            public IEnumerable<ModelesNomenclatures.INomenclature> EnumererNomenclaturesEnCache()
            {
                return enDB.Values.OrderBy(nomenclature => nomenclature.Code);
            }

            /// <summary>
            /// Enumère tous les nomenclatures existantes en Base de données
            /// </summary>
            /// <returns>Énumération des nomenclatures</returns>
            public IEnumerable<ModelesNomenclatures.INomenclature> EnumererNomenclatures()
            {
                ModelesNomenclatures.INomenclature codeActuel = null;
                foreach (var enregistrement in Program.Bd.GetRows(
                    "SELECT nomenclatures.ID_Nomenclatures AS id, nomenclatures.Code AS code FROM nomenclatures ORDER BY Code ASC"))
                {
                    var code = ModelesNomenclatures.CreerNomenclature(enregistrement.GetValue<int>("id"), enregistrement.GetValue<string>("code"));
                    if (code == null) continue;
                    if (codeActuel == null)
                    {
                        if (enregistrement.GetValue("id") != null)
                        {
                            codeActuel = code;
                            yield return codeActuel;
                        }
                    }
                    else if (!code.Id.Equals(codeActuel.Id))
                    {
                        codeActuel = code;
                        yield return codeActuel;
                    }
                    else
                    {
                        if (codeActuel != null)
                        {
                            yield return codeActuel;
                        }
                    }
                };
            }

            /// <summary>
            /// Permet de charger une nomenclature selon l'identifiant spécifié en mémoire cache
            /// </summary>
            /// <param name="id">Identifiant de la nomenclature</param>
            /// <returns>Quizz chargé si possible, sinon null</returns>
            public ModelesNomenclatures.INomenclature ChargerNomenclaturesEnCache(int id)
            {
                return enDB.TryGetValue(id, out var localite) ? localite : null;
            }

            /// <summary>
            /// Permet de charger une nomenclature selon l'identifiant spécifié en Base de données
            /// </summary>
            /// <param name="id">Identifiant de la nomenclature</param>
            /// <returns>Quizz chargé si possible, sinon null</returns>
            public ModelesNomenclatures.INomenclature ChargerNomenclatures(int id)
            {
                return enDB.TryGetValue(id, out var localite) ? localite : null;
            }

            /// <summary>
            /// Permet de retourner une nouvelle entité de type INomenclature
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type INomenclature</returns>
            public ModelesNomenclatures.INomenclature CreerNomenclature()
            {
                return ModelesNomenclatures.CreerNouvelleNomenclature();
            }

            /// <summary>
            /// Permet de mettre à jour (en DB) la nomenclature spécifiée
            /// </summary>
            /// <param name="nomenclature">Localité à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            public bool MettreAJour(ModelesNomenclatures.INomenclature nomenclature)    
            {
                if (nomenclature == null) return false;
                if (nomenclature.Id < 1)
                {
                    // Ajout si il est valide
                    nomenclature.DefinirIdNomenclature((enDB.Count == 0) ? 1 : enDB.Keys.Max() + 1);
                    if (nomenclature.EstValide())
                    {
                        enDB.Add(nomenclature.Id, nomenclature);
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
            /// Permet de supprimer définitivement (au sein du support d'informations) la nomenclature spécifiée
            /// </summary>
            /// <param name="nomenclature">Nomenclature à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool Supprimer(ModelesNomenclatures.INomenclature nomenclature)
            {
                if (nomenclature == null) return false;
                return enDB.Remove(nomenclature.Id);
            }

            public NomenclatureEnDB()
            {
                ModelesNomenclatures.SurChangementCodeNomenclature += SurChangementCodeNomenclature;

                enDB.Clear();
                foreach (var enregistrementNomenclature in ParDefaut)
                {
                    var enregistrementNouvelleNomenclature = ModelesNomenclatures.CreerNomenclature(enregistrementNomenclature.Id, enregistrementNomenclature.Code);
                }
            }


            ~NomenclatureEnDB()
            {
                Dispose();
            }

            public void Dispose()
            {
                ModelesNomenclatures.SurChangementCodeNomenclature -= SurChangementCodeNomenclature;
            }

            private void SurChangementCodeNomenclature(ModelesNomenclatures.INomenclature nomenclature, int valeurActuelle, int nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(nomenclatureEnDB => !nomenclature.Id.Equals(nomenclature.Id)
                    && nomenclature.Code.Equals(nouvelleValeur)))
                {
                    annulation.Cancel();
                }
            }
        }
    }
}
