using PDSGBD;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KineGestionApp
{
    public static partial class ModelesMutuelles
    {
        /// <summary>
        /// Événement déclenché avant le changement du nom d'une mutuelle
        /// </summary>
        public static event BeforeChange<IMutuelle, string> SurChangementNomMutuelle;

        /// <summary>
        /// Événement déclenché avant le changement de l'adresse d'une mutuelle
        /// </summary>
        public static event BeforeChange<IMutuelle, string> SurChangementAdresseMutuelle;

        /// <summary>
        /// Événement déclenché avant le changement du téléphone d'une mutuelle
        /// </summary>
        public static event BeforeChange<IMutuelle, string> SurChangementTelephoneMutuelle;

        /// <summary>
        /// Événement déclenché avant le changement du courriel  d'une mutuelle
        /// </summary>
        public static event BeforeChange<IMutuelle, string> SurChangementEmailMutuelle;

        /// <summary>
        /// Événement déclenché avant le changement du logo d'une mutuelle
        /// </summary>
        public static event BeforeChange<IMutuelle, string> SurChangementLogoMutuelle;

        #region Interface mutuelle
        /// <summary>
        /// Définit toute mutuelle
        /// <para>Expose publiquement des informations et des fonctionnalités</para>
        /// </summary>
        public interface IMutuelle
        {
            /// <summary>
            /// Indique si toutes les caractéristiques d'une mutuelle sont valides
            /// </summary>
            bool EstValide();

            /// <summary>
            /// Identifiant unique d'une mutuelle
            /// </summary>
            int Id { get; }

            /// <summary>
            /// Permet de définir l'identifiant d'une mutuelle si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            bool DefinirIdMutuelle(int id);

            /// <summary>
            /// Nom "unique" d'une mutuelle
            /// </summary>
            string NomMutuelle { get; }

            /// <summary>
            /// Permet de modifier le nom unique d'une mutuelle
            /// </summary>
            /// <param name="nom"></param>
            /// <returns>Retourne vrai si la modification a été faite avec succès, sinon faux</returns>
            bool ModifierNomMutuelle(string nom);

            /// <summary>
            /// Permet de valider le nom d'une mutuelle selon les critères définis
            /// </summary>
            /// <param name="nom">Nom non unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool NomMutuelleValidation(string nom);


            /// <summary>
            /// Adresse unique d'une mutuelle
            /// </summary>
            string AdresseMutuelle { get; }

            ///<summary>
            ///Définir la localité de la mutuelle selon l'Id pour la clé étrangère
            ///</summary>
            int Mutuelle_ID_Localite { get; }

            /// <summary>
            /// Permet de modifier la clé étrangère de la table des localités dans celle des mutuelles.
            /// Ceci est utilisé dans la cadre d'un changement d'adresse d'une mutuelle
            /// </summary>
            /// <param name="ID_Localite">Clé de la localité ciblée</param>
            /// <returns>Vrai si le changement est accepté, sinon faux</returns>
            bool ModifierLocaliteMutuelle(int ID_Localite);

            /// <summary>
            /// Permet de valider une localité si elle est contenue dans la table des localités
            /// </summary>
            /// <param name="ID_Localite">Clé de la localité ciblée</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool LocaliteMutuelleValidation(int ID_Localite);

            /// <summary>
            /// Permet de modifier l'adresse d'un médecin
            /// </summary>
            /// <param name="adresse">Nouvelle adresse d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierAdresseMutuelle(string adresse);

            /// <summary>
            /// Permet de valider l'adresse d'une mutuelle
            /// </summary>
            /// <param name="adresse">Adresse non unique d'un patient => plusieurs patients peuvent habiter à la même adresse</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool AdresseMutuelleValidation(string adresse);

            /// <summary>
            /// Téléphone unique d'une mutuelle
            /// </summary>
            string TelephoneMutuelle { get;}

            /// <summary>
            /// Permet de modifier le numéro unique d'une mutuelle
            /// </summary>
            /// <param name="numero"></param>
            /// <returns>Retourne vrai si la modification a été faite avec succès, sinon faux</returns>
            bool ModifierTelephoneMutuelle(string telephone);

            /// <summary>
            /// Permet de valider le format téléphonique du numéro d'une mutuelle
            /// </summary>
            /// <param name="telephonel">Numéro de téléphone unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool TelephoneMutuelleValidation(string telephone);

            /// <summary>
            /// Adresse courriel unique d'un patient qui sera dans une LinkArea
            /// </summary>
            string EmailMutuelle { get; }

            /// <summary>
            /// Permet de valider l'adresse unique courielle d'une mutuelle
            /// </summary>
            /// <param name="email">Email unique d'une mutuelle</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool EmailMutuelleValide(string email);

            /// <summary>
            /// Permet de modifier l'adresse courielle d'une mutuelle
            /// </summary>
            /// <param name="email">Email unique d'une mutuelle</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierEmailMutuelle(string email);

            /// <summary>
            /// Logo unique d'une mutuelle qui sera dans une PictureBox
            /// </summary>
            Image LogoMutuelle { get; }
            
            /// <summary>
            /// Permet de modifier le lien de la photo du logo d'une mutuelle
            /// </summary>
            /// <param name="logo">Lien unique de la photo du logo d'une mutuelle</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool ModifierLogoMutuelle(Image logo);

            /// <summary>
            /// Permet de valider le lien de la photo du patient et son format
            /// </summary>
            /// <param name="logo">Lien du dossier en Windows</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            bool LogoMutuelleValidation(Image logo);

            /// <summary>
            /// Événement déclenché avant le changement du nom d'une mutuelle
            /// </summary>
            event BeforeChange<IMutuelle, string> SurChangementNomMutuelle;

            /// <summary>
            /// Événement déclenché avant le changement de l'adresse d'une mutuelle
            /// </summary>
            event BeforeChange<IMutuelle, string> SurChangementAdresseMutuelle;

            /// <summary>
            /// Événement déclenché avant le changement du téléphone d'une mutuelle
            /// </summary>
            event BeforeChange<IMutuelle, string> SurChangementTelephone;

            /// <summary>
            /// Événement déclenché avant le changement du courriel  d'une mutuelle
            /// </summary>
            event BeforeChange<IMutuelle, string> SurChangementEmail;

            /// <summary>
            /// Événement déclenché avant le changement du logo d'une mutuelle
            /// </summary>
            event BeforeChange<IMutuelle, string> SurChangementLogo;
        }
        #endregion

        /// <summary>
        /// Permet de créer une nouvelle mutuelle dont les données ne sont pas encore définies et valides
        /// </summary>
        /// <returns>Nouvelle entité de type IQuizz</returns>
        public static IMutuelle CreerNouvelleMutuelle()
        {
            return new Mutuelle(0, string.Empty, string.Empty, string.Empty, string.Empty, null, 0);
        }


        /// <summary>
        /// Permet de créer un patient dont les caractéristiques sont connues
        /// </summary>
        /// <param name="id">Identifiant de ce nouveau patient</param>
        /// <param name="nom">Nom de ce nouveau patient</param>
        /// <param name="adresseMutuelle">Adresse de ce patient </param>
        /// <param name="emailMutuelle">Adresse courrielle de ce patient</param>
        /// <param name="telephoneMutuelle">Numéro de téléphone de ce patient</param>
        /// <param name="logoMutuelle">Lien de la photo de ce patient</param>
        /// <param name="Mutuelle_ID_Localite">ID de la localité de la mutuelle</param>
        /// <returns>Nouvelle entité de type IPatient si les caractéristiques sont valides, sinon null</returns>


        public static IMutuelle CreerMutuelle(int id, string nom, string adresseMutuelle, string emailMutuelle, string telephoneMutuelle, int Mutuelle_ID_Localite, Image logoMutuelle = null) 
        {
            if (id < 1) return null;
            var nouveauMutuelle = new Mutuelle(id, nom, adresseMutuelle, telephoneMutuelle, emailMutuelle, logoMutuelle, Mutuelle_ID_Localite);
               
            return nouveauMutuelle;
        }

        private class Mutuelle : IMutuelle
        {
            /// <summary>
            /// Indique si toutes les caractéristiques d'une mutuelle sont valides
            /// </summary>
            public bool EstValide()
            {
                if ((NomMutuelleValidation(NomMutuelle))
                    && (AdresseMutuelleValidation(AdresseMutuelle))
                    && (TelephoneMutuelleValidation(TelephoneMutuelle))
                    && (LocaliteMutuelleValidation(Mutuelle_ID_Localite))
                    && (LogoMutuelleValidation(LogoMutuelle)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Identifiant unique d'une mutuelle
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// Permet de définir l'identifiant d'une mutuelle si il/elle n'en avait pas déjà
            /// </summary>
            /// <param name="id">Identifiant</param>
            /// <returns>Vrai si la définition d'identifiant a été réalisée, sinon faux</returns>
            public bool DefinirIdMutuelle(int id)
            {
                if ((Id >= 1) || (id < 1)) return false;
                Id = id;
                return true;
            }

            /// <summary>
            /// Nom "unique" d'une mutuelle
            /// </summary>
            public string NomMutuelle { get; private set; }

            /// <summary>
            /// Permet de valider le nom d'un patient selon les critères définis
            /// </summary>
            /// <param name="nom">Nom non unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool NomMutuelleValidation(string nom)
            {
                nom = nom.Trim().Replace(" ", "");
                string.Join(" ", nom.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                if (string.IsNullOrEmpty(nom) || (nom.Any(char.IsDigit) || (PDSGBD.Extensions.InvalideChars(nom) || (nom.Length > 25)))) return false;
                return true;
            }

            /// <summary>
            /// Permet de modifier le nom unique d'une mutuelle
            /// </summary>
            /// <param name="nom"></param>
            /// <returns>Retourne vrai si la modification a été faite avec succès, sinon faux</returns>
            public bool ModifierNomMutuelle(string nom)
            {
                if (!NomMutuelleValidation(nom))
                {
                    return false;
                }
                NomMutuelle = nom;
                return true;
            }



            /// <summary>
            /// Adresse unique d'une mutuelle
            /// </summary>
            public string AdresseMutuelle { get; private set; }


            /// <summary>
            /// Permet de modifier l'adresse d'un médecin
            /// </summary>
            /// <param name="adresse">Nouvelle adresse d'un médecin</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierAdresseMutuelle(string adresse)
            {
                if(!AdresseMutuelleValidation(adresse)) return false;
                AdresseMutuelle= adresse;
                return true;
            }

            /// <summary>
            /// Permet de valider l'adresse d'une mutuelle
            /// </summary>
            /// <param name="adresse">Adresse non unique d'un patient => plusieurs patients peuvent habiter à la même adresse</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool AdresseMutuelleValidation(string adresse)
            {
                adresse.Trim();
                string.Join(" ", adresse.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                if (((string.IsNullOrEmpty(adresse) || adresse.Length > 100) || (Extensions.InvalideChars(adresse)))) return false;
                bool containsInt = adresse.Any(char.IsDigit); //Un numéro doit être spécifié
                if (!containsInt) return false;
                return true;
            }

            ///<summary>
            ///Définir la localité du patient selon l'Id pour la clé étrangère
            ///</summary>
            public int Mutuelle_ID_Localite { get; private set; }

            /// <summary>
            /// Permet de modifier la clé étrangère de la table des localités dans celle des patients.
            /// Ceci est utilisé dans la cadre d'un changement d'adresse d'un patient
            /// </summary>
            /// <param name="ID_Localite"></param>
            /// <returns>Vrai si le changement est accepté, sinon faux</returns>
            public bool ModifierLocaliteMutuelle(int ID_Localite)
            {
                if (!LocaliteMutuelleValidation(ID_Localite)) return false;
                Mutuelle_ID_Localite = ID_Localite;
                return true;

            }

            /// <summary>
            /// Permet de valider une localité si elle est contenue dans la table des localités
            /// </summary>
            /// <param name="mutuelle_ID_Localite">Adresse non unique d'un patient => plusieurs patients peuvent habiter à la même adresse</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool LocaliteMutuelleValidation(int ID_Localite)
            {
                if (ID_Localite < 0) return false;
                if (Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM localites WHERE localites.ID_Localite = {0}", ID_Localite) >= 1) return true;
                return false;
            }

            /// <summary>
            /// Téléphone unique d'une mutuelle
            /// </summary>
            public string TelephoneMutuelle { get; private set; }

            /// <summary>
            /// Permet de modifier le numéro unique d'une mutuelle
            /// </summary>
            /// <param name="numero"></param>
            /// <returns>Retourne vrai si la modification a été faite avec succès, sinon faux</returns>
            public bool ModifierTelephoneMutuelle(string telephone)
            {
                if(!TelephoneMutuelleValidation(telephone)) return false;
                TelephoneMutuelle = telephone;
                return true;
            }

            /// <summary>
            /// Permet de valider le format téléphonique du numéro d'une mutuelle
            /// </summary>
            /// <param name="telephonel">Numéro de téléphone unique d'un patient</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool TelephoneMutuelleValidation(string telephone)
            {
                if (!Extensions.TelephoneValidation(telephone)) return false;
                return true;
            }

            /// <summary>
            /// Adresse courriel unique d'un patient qui sera dans une LinkArea
            /// </summary>
            public string EmailMutuelle { get; private set; }

            /// <summary>
            /// Permet de valider l'adresse unique courielle d'une mutuelle
            /// </summary>
            /// <param name="email">Email unique d'une mutuelle</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool EmailMutuelleValide(string email)
            {
                email.Trim();
                if (!Extensions.IsEmailValide(email)) return false;
                //if(Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM mutualites WHERE mutualites.Email LIKE {0}", email) >= 1) return false;
                return true;
            }

            /// <summary>
            /// Permet de modifier l'adresse courielle d'une mutuelle
            /// </summary>
            /// <param name="email">Email unique d'une mutuelle</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierEmailMutuelle(string email)
            {
                if (!EmailMutuelleValide(email))
                {
                    return false;
                }
                EmailMutuelle = email;
                return true;
            }

            /// <summary>
            /// Logo unique d'une mutuelle qui sera dans une PictureBox
            /// </summary>
            public Image LogoMutuelle { get; private set; }

            /// <summary>
            /// Permet de modifier le lien de la photo du logo d'une mutuelle
            /// </summary>
            /// <param name="photo">Lien unique de la photo du logo d'une mutuelle</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool ModifierLogoMutuelle(Image logo)
            {
                if (!LogoMutuelleValidation(logo)) return false;
                LogoMutuelle = logo;
                return true;
            }


            /// <summary>
            /// Permet de valider le lien de la photo du patient et son format
            /// </summary>
            /// <param name="logo">Lien du dossier en Windows</param>
            /// <returns>Vrai si ce changement a été accepté, sinon faux</returns>
            public bool LogoMutuelleValidation(Image logo)
            {
                if (logo == null) return true;
                ImageFormat imageFormat = logo.RawFormat;
                //La photo peut être facultative
                if ((!ImageFormat.Png.Equals(imageFormat) && !ImageFormat.Jpeg.Equals(imageFormat) && !ImageFormat.Gif.Equals(imageFormat))) return false;

                return true;
            }

            /// <summary>
            /// Événement déclenché avant le changement du nom d'une mutuelle
            /// </summary>
            public event BeforeChange<IMutuelle, string> SurChangementNomMutuelle;

            /// <summary>
            /// Événement déclenché avant le changement de l'adresse d'une mutuelle
            /// </summary>
            public event BeforeChange<IMutuelle, string> SurChangementAdresseMutuelle;

            /// <summary>
            /// Événement déclenché avant le changement du téléphone d'une mutuelle
            /// </summary>
            public event BeforeChange<IMutuelle, string> SurChangementTelephone;


            /// <summary>
            /// Événement déclenché avant le changement du courriel  d'une mutuelle
            /// </summary>
            public event BeforeChange<IMutuelle, string> SurChangementEmail;

            /// <summary>
            /// Événement déclenché avant le changement du logo d'une mutuelle
            /// </summary>
            public event BeforeChange<IMutuelle, string> SurChangementLogo;

            /// <summary>
            /// Constructeur pour une mutuelle
            /// </summary>
            /// <param name="id">Identifiant de cette mutuelle</param>
            /// <param name="nom">Nom de cette mutuelle</param>
            /// <param name="adresse">Prenom de ce patient</param>
            /// <param name="telephone">Prenom de ce patient</param>
            /// <param name="email">Prenom de ce patient</param>
            /// <param name="logo">Prenom de ce patient</param>
            /// <param name="mutuelle_ID_Localite">ID de la localité de la mutuelle</param>
            public Mutuelle(int id, string nom, string adresse, string telephone, string email, Image logo, int mutuelle_ID_Localite)
            {
                Id = id;
                NomMutuelle= nom;
                AdresseMutuelle = adresse;
                EmailMutuelle = email;
                TelephoneMutuelle= telephone;
                LogoMutuelle= logo;
                Mutuelle_ID_Localite = mutuelle_ID_Localite;
            }
        }
    }
}
