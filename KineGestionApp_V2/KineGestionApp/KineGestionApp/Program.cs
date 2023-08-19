using Org.BouncyCastle.Utilities;
using PDSGBD_MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace KineGestionApp
{
    internal static class Program
    {
        public static bool ServerOnOrOff()
        {
            var ping = new Ping();
            PingReply reply = ping.Send(Bd.Configuration.ServerAddress, 60 * 1000); 
                                                            
            if(reply.Status != IPStatus.Success) 
            { 
                return false;
            }
            return true;
        }

        #region Définition de l'accès à la base de données
        /// <summary>
        /// Permet de créer la configuration d'accès à la base de données
        /// </summary>
        /// <param name="versionVide">Indique si on veut utiliser la version de débuggage de la base de données qui démarre dans un état "vide" de tout enregistrement</param>
        /// <returns>Configuration de la connexion à la base de données</returns>
        

        private static DBM.IConfiguration CreerAccesBd(bool versionVide = false)
        {
            if (!DBM.CreateConfiguration
            (
                "Sam",
                "mZTbURtCucb92Grf",
                "localhost",
                "kinegestionapp"
                , out var configuration
            )) return null;
            return configuration;
        }
        #endregion

        

        /// <summary>
        /// Objet de connexion/manipulation de la base de données de l'application
        /// </summary>
        public static DBM Bd { get; private set; } = new DBM(CreerAccesBd());

        public static GestionLocalites.ILocalite Localite { get; } = GestionLocalites.CreerLocaliteEnDB();
        public static GestionPatients.IPatient Patient { get; } = GestionPatients.CreerPatientEnDB();
        public static GestionMedecins.IMedecin Medecin { get; } = GestionMedecins.CreerMedecinEnDB();
        public static GestionMutuelles.IMutuelle Mutuelle { get; } = GestionMutuelles.CreerMutuelleEnDB();
        public static GestionPrescriptions.IPrescription Prescription { get; } = GestionPrescriptions.CreerPrescriptiontEnDB();

        public static GestionSeances.ISeance Seance { get; } = GestionSeances.CreerSeanceEnDB();
        public static GestionNomenclatures.INomenclature Nomenclature { get; } = GestionNomenclatures.CreerNomenclatureEnDB();


        /// <summary>
        /// Vérifie une unicité dans la base de données
        /// </summary>
        /// <param name="string">La chaine de caractères à vérifier</param>
        /// <param name="column">La colonne</param>
        /// <param name="column">La ligne</param>
        /// <returns>Vrai si l'unicté est respectée, sinon faux</returns>
        public static bool UniquenessInDatabase(string @string, string table, string column)
        {
          return (Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM " + table + " WHERE " + column + " = {0}", @string) >= 1);
        }

        public static long NbMaxItemsDB(string table)
        {
             return Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM " + table);
        }

        public static bool connectionAttempt()
        {
            return Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM  admin") >= 1;
        }

        public static bool ExistenceTestID(int id, string table, string column)
        {
            if (Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM " + table + " WHERE " + column + " = " + id) == 1) return true;
            return false;
        }

        /// <summary>
        /// Permet de récupérer la position de l'ID de la localité dans les comboBox des localités et des codes postaux
        /// </summary>
        /// <param name="source">Liste des objets localités</param>
        /// <param name="idToFind">ID à récupréer</param>
        /// <returns></returns>
        public static int positionItemFromEnumerable(IEnumerable<ModelesLocalites.ILocalite> source, int idToFind)
            //where T : ModelesMutuelles.IMutuelle, ModelesPatients.IPatient, ModelesLocalites.ILocalite, ModelesMedecins.IMedecin
        {
            int i = 0; //positon de l'index de la comboBox
            foreach (var @object in source)
            {
                if (@object.Id == idToFind) return i;
                i++;
            }
            return -1; //Not found
        }

        /// <summary>
        /// Permet de récupérer l'ID d'un patient sur base d'une position dans un control
        /// </summary>
        /// <param name="source">Liste des objets localités</param>
        /// <param name="posToFind">ID à récupréer</param>
        /// <returns></returns>
        public static int idItemFromEnumerablePatients(IEnumerable<ModelesPatients.IPatient> source, int posToFind)
        {
            int i = 0; //positon de l'index de la comboBox
            foreach (var @object in source)
            {
                if (i == posToFind) return @object.Id;
                i++;
            }
            return -1; //Not found
        }

        /// <summary>
        /// Permet de récupérer l'ID d'un médecin sur base d'une position dans un control
        /// </summary>
        /// <param name="source">Liste des objets localités</param>
        /// <param name="posToFind">ID à récupréer</param>
        /// <returns></returns>
        public static int idItemFromEnumerableMedecins(IEnumerable<ModelesMedecins.IMedecin> source, int posToFind)
        {
            int i = 0; //positon de l'index de la comboBox
            foreach (var @object in source)
            {
                if (i == posToFind) return @object.Id;
                i++;
            }
            return -1; //Not found
        }



        /// <summary>
        /// Permet de récupérer la position de l'ID de la mutuelle dans les comboBox des localités et des codes postaux
        /// </summary>
        /// <param name="source">Liste des objets mutuelles</param>
        /// <param name="idToFind">ID à récupérer</param>
        /// <returns></returns>
        public static int positionMutFromEnumerable(IEnumerable<ModelesMutuelles.IMutuelle> source, int idToFind)
        {
            int i = 0; //positon de l'index de la comboBox
            foreach (var @object in source)
            {
                if (@object.Id == idToFind) return i;
                i++;
            }
            return -1; //Not found
        }

        /// <summary>
        /// Permet de récupérer la position de l'ID de la localité dans les comboBox des localités et des codes postaux
        /// </summary>
        /// <param name="source"></param>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public static ModelesLocalites.ILocalite ItemFromEnumerable(IEnumerable<ModelesLocalites.ILocalite> source, int idToFind)
        {
            foreach (var @object in source)
            {
                if (@object.Id == idToFind) return @object;
            }
            return null; //Not found
        }

        /// <summary>
        /// Permet de récupérer la position de l'ID de la mutuelle dans les comboBox des localités et des codes postaux
        /// </summary>
        /// <param name="source"></param>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public static ModelesMutuelles.IMutuelle ItemMutFromEnumerable(IEnumerable<ModelesMutuelles.IMutuelle> source, int idToFind)
        {
            foreach (var @object in source)
            {
                if (@object.Id == idToFind) return @object;
            }
            return null; //Not found
        }

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!ServerOnOrOff())
            {
                MessageBox.Show("Erreur côté server\n" +
                "Prenez contact avec votre Provider :\n" +
                "Samuël Raes : 0473/934591", "Erreur Serveur", MessageBoxButtons.OK);
            }
            else
            {
                if (!connectionAttempt())
                {
                    MessageBox.Show("Erreur de service MySQL\n" +
                    "Prenez contact avec votre Provider :\n" +
                    "Samuël Raes : 0473/934591", "Erreur service MySQL", MessageBoxButtons.OK);
                }
                else
                {
                    Application.Run(new Boite_Modale_Generale());
                }
                
            }
           
        }
    }
}
