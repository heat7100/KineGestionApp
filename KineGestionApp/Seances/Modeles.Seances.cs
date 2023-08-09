using PDSGBD;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineGestionApp
{
    public static partial class ModelesSeances
    {

        /// <summary>
        /// Événement déclenché avant le changement du numéro d'une presciption
        /// </summary>
        public static event BeforeChange<ISeance, DateTime> SurChangementDateSeance;

        /// <summary>
        /// Événement déclenché avant le changement du numéro d'une presciption
        /// </summary>
        public static event BeforeChange<ISeance, string> SurChangementCommentaireSeance;

        /// <summary>
        /// Événement déclenché avant le changement du numéro d'une presciption
        /// </summary>
        public static event BeforeChange<ISeance, SqlMoney> SurChangementPrixSeance;

        public interface ISeance
        {
            /// <summary>
            /// Indique si toutes les caractéristiques de la séances sont valides
            /// </summary>
            bool EstValide();

            /// <summary>
            /// Identifiant unique de cette séance
            /// </summary>
            int Id { get; }

            /// <summary>
            /// Permet de définir l'identifiant d'une séance si elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            bool DefinirId(int id);

            /// <summary>
            /// Définit la date de la séance
            /// </summary>
            DateTime DateSeance { get; }

            /// <summary>
            /// Permet de valider la date d'une séance
            /// </summary>
            /// <param name="date">Nouvelle date d'une prescription</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool DateSeanceValide(DateTime date);

            /// <summary>
            /// Permet de modifier la date d'une séance
            /// </summary>
            /// <param name="date">Nouvelle date d'une presciption</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierDateSeance(DateTime date);

            /// <summary>
            /// Commentaire pouvant être facultatif à une séance
            /// </summary>
            string CommentaireSeance { get; }

            /// <summary>
            /// Permet de modifier un commentaire établi, mais aussi de le supprimer en ne comprotant aucun caractères
            /// </summary>
            /// <param name="commentaire"></param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierCommentaireSeance(string commentaire);

            /// <summary>
            /// Prix établi de la séance
            /// </summary>
            SqlMoney PrixSeance { get; }


            /// <summary>
            /// Permet de valider le prix de la séance
            /// </summary>
            /// <param name="prix"></param>
            /// <returns>Vrai si valide, sinon faux</returns>
            bool PrixSeanceValide(SqlMoney prix);

            /// <summary>
            /// Permet de modifier le prix établi d'une séance
            /// </summary>
            /// <param name="id"></param>
            /// <returns>Retourne vrai si la modification a été réalisé, sinon faux</returns>
            bool ModifierPrixSeance(SqlMoney prix);


            /// <summary>
            /// Événement déclenché avant le changement du numéro d'une presciption
            /// </summary>
            event BeforeChange<ISeance, DateTime> SurChangementDate;

            /// <summary>
            /// Événement déclenché avant le changement du numéro d'une presciption
            /// </summary>
            event BeforeChange<ISeance, string> SurChangementCommentaire;

            /// <summary>
            /// Événement déclenché avant le changement du numéro d'une presciption
            /// </summary>
            event BeforeChange<ISeance, SqlMoney> SurChangementPrix;
        }

        /// <summary>
        /// Permet de créer une nouvelle prescription dont les données ne sont pas encore définies et valides
        /// </summary>
        /// <returns>Nouvelle entité de type Prescription</returns>
        public static ISeance CreerNouvelleSeance()
        {
            return new Seance(0, DateTime.MinValue, string.Empty, SqlMoney.MinValue);
        }

        /// <summary>
        /// Constructeur pour un/e patient/e
        /// </summary>
        /// <param name="id">Identifiant de cette prescription</param>
        /// <param name="date">Date de cette seance</param>
        /// <param name="prix">Prix de cette séance</param>
        /// <param name="commentaire">Commenatire sur cette séance</param>
        /// <returns>Nouvelle entité de type IPrescription si les paramètres sont valides, sinon null</returns>
        public static ISeance CreerSeance(int id, DateTime date, string commentaire, SqlMoney prix)
        {
            if (id < 1) return null;
            var nouvelleSeance = new Seance(id, date, commentaire, prix);
            if (!nouvelleSeance.ModifierPrixSeance(prix)) return null;
            if (!nouvelleSeance.ModifierDateSeance(date)) return null;
            return nouvelleSeance;
        }

        private class Seance : ISeance
        {

            /// <summary>
            /// Indique si toutes les caractéristiques de la séances sont valides
            /// </summary>
            public bool EstValide()
            {
                if ((Id >= 1)
                    && (DateSeanceValide(DateSeance))
                    && (PrixSeanceValide(PrixSeance.Value)))
                    return false;
                else
                {
                    return false;
                }
            }
            /// <summary>
            /// Identifiant unique de cette séance
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// Permet de définir l'identifiant d'une séance si elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            public bool DefinirId(int id)
            {
                if ((Id >= 1) || (id < 1)) return false;
                Id = id;
                return true;
            }

            /// <summary>
            /// Définit la date de la séance
            /// </summary>
            public DateTime DateSeance { get; private set; }

            /// <summary>
            /// Permet de valider la date d'une séance
            /// </summary>
            /// <param name="date">Nouvelle date d'une prescription</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool DateSeanceValide(DateTime date)
            {
                DateTime dateMin = new DateTime().AddYears(-1);
                DateTime dateMax = new DateTime();
                if ((date > dateMin) || (date < dateMax)) return false;
                DateSeance = date; return true;
            }

            /// <summary>
            /// Permet de modifier la date d'une séance
            /// </summary>
            /// <param name="date">Nouvelle date d'une presciption</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierDateSeance(DateTime date)
            {
                DateTime dateMin = new DateTime().AddYears(-1);
                DateTime dateMax = new DateTime();
                if ((date > dateMin) || (date < dateMax)) return false;
                DateSeance = date; return true;
            }

            /// <summary>
            /// Commentaire pouvant être facultatif à une séance
            /// </summary>
            public string CommentaireSeance { get; private set; }

            /// <summary>
            /// Permet de modifier un commentaire établi, mais aussi de le supprimer en ne comprotant aucun caractères
            /// </summary>
            /// <param name="commentaire"></param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierCommentaireSeance(string commentaire)
            {
                commentaire = commentaire.TrimStart();
                commentaire = commentaire.TrimEnd();
                if(string.IsNullOrEmpty(commentaire)) return false;
                CommentaireSeance = commentaire; return true;
            }

            /// <summary>
            /// Prix établi de la séance
            /// </summary>
            public SqlMoney PrixSeance { get; private set; }


            /// <summary>
            /// Permet de valider le prix de la séance
            /// </summary>
            /// <param name="prix"></param>
            /// <returns>Vrai si valide, sinon faux</returns>
            public bool PrixSeanceValide(SqlMoney prix)
            {
                if(prix.Value < 25) return false; return true;
            }

            /// <summary>
            /// Permet de modifier le prix établi d'une séance
            /// </summary>
            /// <param name="id"></param>
            /// <returns>Retourne vrai si la modification a été réalisé, sinon faux</returns>
            public bool ModifierPrixSeance(SqlMoney prix)
            {
                if(prix.Value < 25) return false;
                PrixSeance = prix; return true;
            }

            /// <summary>
            /// Événement déclenché avant le changement du numéro d'une presciption
            /// </summary>
            public event BeforeChange<ISeance, DateTime> SurChangementDate;

            /// <summary>
            /// Événement déclenché avant le changement du numéro d'une presciption
            /// </summary>
            public event BeforeChange<ISeance, string> SurChangementCommentaire;

            /// <summary>
            /// Événement déclenché avant le changement du numéro d'une presciption
            /// </summary>
            public event BeforeChange<ISeance, SqlMoney> SurChangementPrix;

            /// <summary>
            /// Constructeur pour un/e patient/e
            /// </summary>
            /// <param name="id">Identifiant de cette séance</param>
            /// <param name="date">Date de cette seance</param>
            /// <param name="prix">Acompe prépayé pour le traitement prescrit</param>
            /// <param name="commentaire">Commenatire sur cette séance</param>
            public Seance(int id, DateTime date, string commentaire, SqlMoney prix)
            {
                Id = id;
                DateSeance = date;
                CommentaireSeance = commentaire;
                PrixSeance= prix;
            }
        }
    }
}
