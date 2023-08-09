using PDSGBD;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineGestionApp
{
    /// <summary>
    /// Contient les définitions (publiques) et implémentations (privées) des modèles de nomenclatures
    /// </summary>
    public static partial class ModelesNomenclatures
    {

        /// <summary>
        /// Événement déclenché avant le changement du code d'une nomenclature
        /// </summary>
        public static event BeforeChange<INomenclature, int> SurChangementCodeNomenclature;

        public interface INomenclature
        {
            /// <summary>
            /// Code numérique de la nomenclature
            /// </summary>
            string Code { get; }

            /// <summary>
            /// Permet de modifier un code d'une nomenclature donnée
            /// </summary>
            /// <param name="code"></param>
            /// <returns>Retourne vrai si le code a été modifié avec succès, sinon faux</returns>
            bool ModifierCodeNomenclature(string code);

            /// <summary>
            /// Permet de vérifier la validité du code de nomenclature selon l'INAMI : https://www.riziv.fgov.be/fr/nomenclature/Pages/default.aspx
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            bool CodeNomenclatureValide(string code);

            /// <summary>
            /// Indique si toutes le numéro de nomenclaure est valide selon l'INAMI
            /// </summary>
            bool EstValide();

            /// <summary>
            /// Identifiant unique d'une nomenclature
            /// </summary>
            int Id { get; }

            /// <summary>
            /// Permet de définir l'identifiant d'une nomenclature si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            bool DefinirIdNomenclature(int id);

            /// <summary>
            /// Événement déclenché avant le changement du code d'une nomenclature
            /// </summary>
            event BeforeChange<INomenclature, int> SurChangementCode;
        }

        /// <summary>
        /// Permet de créer une nouvelle nomenclature dont les données ne sont pas encore définies et valides
        /// </summary>
        /// <returns>Nouvelle entité de type Prescription</returns>
        public static INomenclature CreerNouvelleNomenclature()
        {
            return new Nomenclature(0, null);
        }

        /// <summary>
        /// Constructeur pour un/e patient/e
        /// </summary>
        /// <param name="id">Identifiant de cette prescription</param>
        /// <param name="code">Nombre de séances de cette prescription</param>
        /// <returns>Nouvelle entité de type IPrescription si les paramètres sont valides, sinon null</returns>
        public static INomenclature CreerNomenclature(int id, string code)
        {
            if (id < 1) return null;
            var nouvelleNomenclature = new Nomenclature(id, code);
            if (!nouvelleNomenclature.ModifierCodeNomenclature(code)) return null;
            return nouvelleNomenclature;
        }

        private class Nomenclature : INomenclature, IDisposable
        {
            /// <summary>
            /// Code numérique de la nomenclature
            /// </summary>
            public string Code { get; private set; }

            /// <summary>
            /// Permet de modifier un code d'une nomenclature donnée
            /// </summary>
            /// <param name="code"></param>
            /// <returns>Retourne vrai si le code a été modifié avec succès, sinon faux</returns>
            public bool ModifierCodeNomenclature(string code)
            {
                if (!CodeNomenclatureValide(code)) return false;
                Code = code; return true;
            }

            /// <summary>
            /// Permet de vérifier la validité du code de nomenclature selon l'INAMI : https://www.riziv.fgov.be/fr/nomenclature/Pages/default.aspx https://www.inami.fgov.be/fr/professionnels/sante/kinesitherapeutes/Pages/nomenclature-kinesitherapie-adaptations.aspx
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public bool CodeNomenclatureValide(string code)
            {
                if ((Code.ToString().Length < 6) || ((Code.ToString().Length > 15))) return false;
                return true;
            }

            /// <summary>
            /// Indique si toutes le numéro de nomenclaure est valide selon l'INAMI
            /// </summary>
            public bool EstValide()
            {
                if ((Id >= 1) && (CodeNomenclatureValide(Code))) return true;
                return false;
            }

            /// <summary>
            /// Identifiant unique d'une nomenclature
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// Permet de définir l'identifiant d'une nomenclature si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            public bool DefinirIdNomenclature(int id)
            {
                if ((Id >= 1) || (id < 1)) return false;
                Id = id;
                return true;
            }

            /// <summary>
            /// Événement déclenché avant le changement du code d'une nomenclature
            /// </summary>
            public event BeforeChange<INomenclature, int> SurChangementCode;

            /// <summary>
            /// Constructeur pour une nomenclature
            /// </summary>
            /// <param name="id">Identifiant de cette prescription</param>
            /// <param name="code">Code INAMI de la nomenclature</param>
            public Nomenclature(int id, string code)
            {
                Id = id;
                Code = code;
            }

            /// <summary>
            /// Destructeur
            /// </summary>
            ~Nomenclature()
            {
                Dispose();
            }

            public void Dispose()
            {

            }
        }
    }
}
