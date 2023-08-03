using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PDSGBD;

namespace KineGestionApp
{
    /// <summary>
    /// Contient les définitions (publiques) et implémentations (privées) des modèles de localités
    /// </summary>
    public partial class ModelesLocalites
    {
        /// <summary>
        /// Événement déclenché avant le changement du nom d'une localité
        /// </summary>
        public static event BeforeChange<ILocalite, string> SurChangementNomLocalite;

        /// <summary>
        /// Événement déclenché avant le changement du code potal d'une localité
        /// </summary>
        public static event BeforeChange<ILocalite, string> SurChangementCodePostalLocalite;

        #region Interface localite
        public interface ILocalite
        {
            /// <summary>
            /// Indique si toutes le numéro de nomenclaure est valide selon l'INAMI
            /// </summary>
            bool EstValide();

            /// <summary>
            /// Identifiant unique d'une localité
            /// </summary>
            int Id { get; }

            /// <summary>
            /// Permet de définir l'identifiant d'une localité si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            bool DefinirIdLocalite(int id);

            /// <summary>
            /// Code postal unique d'une localité
            /// </summary>
            string CodePostal { get; }

            /// <summary>
            /// Permet de modifier un code postal unique d'une localité
            /// </summary>
            /// <param name="codePostal"></param>
            /// <returns>Retourne true si la modification a été réalisée avec succès, sinon faux</returns>
            bool ModifierCodePostal(string codePostal);

            /// <summary>
            /// Permet de valider un code postal belge selon les régles en vigeur 
            /// </summary>
            /// <param name="codePostal"></param>
            /// <returns></returns>
            bool CodePostalValidation(string codePostal);
            
            /// <summary>
            /// Nom unique d'une localité
            /// </summary>
            string NomLocalite { get; }

            /// <summary>
            /// Permet de modifier le nom d'une localité
            /// </summary>
            /// <param name="nom"></param>
            /// <returns>Retourne true si la modification a été réalisée avec succès, sinon faux</returns>
            bool ModifierNomLocalite(string nom);

            /// <summary>
            /// Permet de valider le nom d'une localité de la liste 
            /// </summary>
            /// <param name="codePostal"></param>
            /// <returns></returns>
            bool NomLocaliteValide(string localite);

            /// <summary>
            /// Événement déclenché avant le changement du nom d'une localité
            /// </summary>
            event BeforeChange<ILocalite, string> SurChangementNom;
        }
        #endregion

        /// <summary>
        /// Permet de créer une nouvelle mutuelle dont les données ne sont pas encore définies et valides
        /// </summary>
        /// <returns>Nouvelle entité de type IQuizz</returns>
        public static ILocalite CreerNouvelleLocalite()
        {
            return new Localite(0, string.Empty, string.Empty);
        }


        /// <summary>
        /// Permet de créer un patient dont les caractéristiques sont connues
        /// </summary>
        /// <param name="id">Identifiant de cette localité</param>
        /// <param name="nom">Nom de cette localité</param>
        /// <param name="codePostal">Code_postal de cette localité</param>
        /// <returns>Nouvelle entité de type IPatient si les caractéristiques sont valides, sinon null</returns>

        public static ILocalite CreerLocalite(int id, string nom, string codePostal) 
        {
            if (id < 1) return null;
            var nouvelleLocalite = new Localite(id, nom, codePostal);
            //if (!nouvelleLocalite.ModifierNomLocalite(nom)) return null;
            return nouvelleLocalite;
        }

        private class Localite : ILocalite 
        {
            /// <summary>
            /// Indique si une localité est valide ou pas
            /// </summary>
            public bool EstValide()
            {
                if ((Id >= 1) 
                    && CodePostalValidation(CodePostal)
                    && NomLocaliteValide(NomLocalite))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Identifiant unique d'une nomenclature
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// Permet de définir l'identifiant d'une localité si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            public bool DefinirIdLocalite(int id)
            {
                if ((Id >= 1) || (id < 1)) return false;
                Id = id;
                return true;
            }

            /// <summary>
            /// Code postal unique d'une localité
            /// </summary>
            public string CodePostal { get; private set; }

            /// <summary>
            /// Permet de modifier un code postal unique d'une localité
            /// </summary>
            /// <param name="codePostal"></param>
            /// <returns>Retourne true si la modification a été réalisée avec succès, sinon faux</returns>
            public bool ModifierCodePostal(string codePostal)
            {
                if(!CodePostalValidation(codePostal)) return false;
                CodePostal = codePostal;
                return true;
            }

            /// <summary>
            /// Permet de valider un code postal belge selon les régles en vigeur 
            /// </summary>
            /// <param name="codePostal"></param>
            /// <returns></returns>
            public bool CodePostalValidation(string codePostal)
            {
                if (codePostal == null) return false;
               return Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM localites WHERE localites.Code_postal = {0}", codePostal) >= 1;
            }

            /// <summary>
            /// Nom unique d'une localité
            /// </summary>
            public string NomLocalite { get; private set; }

            /// <summary>
            /// Permet de modifier le nom d'une localité
            /// </summary>
            /// <param name="nom"></param>
            /// <returns>Retourne true si la modification a été réalisée avec succès, sinon faux</returns>
            public bool ModifierNomLocalite(string nom)
            {
                if (NomLocaliteValide(nom)) return false;
                NomLocalite = nom;
                return true;
            }

            /// <summary>
            /// Permet de valider le nom d'une localité de la liste 
            /// </summary>
            /// <param name="codePostal"></param>
            /// <returns></returns>
            public bool NomLocaliteValide(string localite)
            {
                if(localite == null) return false;
                return Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM localites WHERE localites.Localite = {0}", localite) >= 1;

            }

            /// <summary>
            /// Événement déclenché avant le changement du nom d'une localité
            /// </summary>
            public event BeforeChange<ILocalite, string> SurChangementNom;

            /// <summary>
            /// Constructeur pour une localite
            /// </summary>
            /// <param name="id">Identifiant de cette localité</param>
            /// <param name="codePostal">Code postal de la localité</param>
            /// <param name="nom">Nom de cette localité</param>
            public Localite(int id, string nom, string codePostal)
            {
                Id = id;
                NomLocalite = nom;
                CodePostal= codePostal;
            }
        }
    }
}
