using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSGBD;

namespace KineGestionApp
{
    /// <summary>
    /// Définit tout gestionnaire de séances
    /// </summary>
    public partial class GestionSeances
    {
        public interface ISeance
        {
            /// <summary>
            /// Enumère tous les séances existantes
            /// </summary>
            /// <returns>Énumération des séances</returns>
            IEnumerable<ModelesSeances.ISeance> EnumererSeances();

            /// <summary>
            /// Permet de charger une séance selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant de la séance</param>
            /// <returns>Médecin chargé si possible, sinon null</returns>
            ModelesSeances.ISeance ChargerSeances(int id);

            /// <summary>
            /// Permet de retourner une nouvelle entité de type ISeance
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type ISeance</returns>
            ModelesSeances.ISeance CreerSeances();

            /// <summary>
            /// Permet de mettre à jour (au sein du support d'informations) la prescription spécifiée
            /// </summary>
            /// <param name="seance">Séance à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            bool MettreAJour(ModelesSeances.ISeance seance);

            /// <summary>
            /// Permet de supprimer définitivement (au sein du support d'informations) la séance spécifiée
            /// </summary>
            /// <param name="seance">Medecin à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            bool Supprimer(ModelesSeances.ISeance seance);
        }

        public static ISeance CreerSeanceEnDB() => new SeanceEnDB();

        /// <summary>
        /// Implémente un gestionnaire des patients utilisant uniquement la mémoire comme support d'informations
        /// </summary>
        private class SeanceEnDB : ISeance, IDisposable
        {

            /// <summary>
            /// Créer un nouvelle séance en DB
            /// </summary>
            private static ModelesSeances.ISeance[] ParDefaut { get; } = new ModelesSeances.ISeance[]
            {
                ModelesSeances.CreerNouvelleSeance()
            };

            private Dictionary<int, ModelesSeances.ISeance> enDB { get; } = new Dictionary<int, ModelesSeances.ISeance>();

            /// <summary>
            /// Enumère tous les séances
            /// </summary>
            /// <returns>Énumération des séances</returns>
            public IEnumerable<ModelesSeances.ISeance> EnumererSeances()
            {
                return enDB.Values.OrderBy(seance => seance.DateSeance);
            }

            /// <summary>
            /// Permet de charger une séance selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant de cette sance</param>
            /// <returns>Prescription chargée si possible, sinon null</returns>
            public ModelesSeances.ISeance ChargerSeances(int id)
            {
                return enDB.TryGetValue(id, out var seance) ? seance : null;
            }

            /// <summary>
            /// Permet de retourner une nouvelle entité de type ISeance
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type ISeance</returns>
            public ModelesSeances.ISeance CreerSeances()
            {
                return ModelesSeances.CreerNouvelleSeance();
            }

            /// <summary>
            /// Permet de mettre à jour (en DB) la séance spécifiée
            /// </summary>
            /// <param name="seance">Prescriptionz à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            public bool MettreAJour(ModelesSeances.ISeance seance)
            {
                if (seance == null) return false;
                if (seance.Id < 1)
                {
                    // Ajout si il est valide
                    seance.DefinirId((enDB.Count == 0) ? 1 : enDB.Keys.Max() + 1);
                    if (seance.EstValide())
                    {
                        enDB.Add(seance.Id, seance);
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
            /// Permet de supprimer définitivement (en DB) la prescription spécifiée
            /// </summary>
            /// <param name="seance">Prescription à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool Supprimer(ModelesSeances.ISeance seance)
            {
                if (seance == null) return false;
                return enDB.Remove(seance.Id);
            }

            public SeanceEnDB()
            {
                ModelesSeances.SurChangementDateSeance += SurChangementDateSeance;
                ModelesSeances.SurChangementPrixSeance += SurChangementPrixSeance;
                ModelesSeances.SurChangementCommentaireSeance += SurChangementCommentaireSeance;
                enDB.Clear();
                foreach (var enregistrementSeance in ParDefaut)
                {
                    var enregistrementNouvelleSeance = ModelesSeances.CreerSeance(enregistrementSeance.Id, enregistrementSeance.DateSeance, enregistrementSeance.CommentaireSeance, enregistrementSeance.PrixSeance);
                }
            }

            ~SeanceEnDB()
            {
                Dispose();
            }

            public void Dispose()
            {
                ModelesSeances.SurChangementDateSeance += SurChangementDateSeance;
                ModelesSeances.SurChangementPrixSeance += SurChangementPrixSeance;
                ModelesSeances.SurChangementCommentaireSeance += SurChangementCommentaireSeance;
            }


            private void SurChangementDateSeance(ModelesSeances.ISeance seance, DateTime valeurActuelle, DateTime nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(prescriptionEnDB => !seance.Id.Equals(seance.Id)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementPrixSeance(ModelesSeances.ISeance seance, SqlMoney valeurActuelle, SqlMoney nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(prescriptionEnDB => !seance.Id.Equals(seance.Id)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementCommentaireSeance(ModelesSeances.ISeance seance, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(prescriptionEnDB => !seance.Id.Equals(seance.Id)))
                {
                    annulation.Cancel();
                }
            }
        }
    }
}
