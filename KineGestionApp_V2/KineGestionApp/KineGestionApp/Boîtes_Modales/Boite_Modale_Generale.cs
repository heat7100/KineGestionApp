using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDSGBD;


namespace KineGestionApp
{
    /// <summary>
    /// Boîte modale principale du programme qui dirige l'utilisateur vers les différents sous-menus
    /// </summary>
    public partial class Boite_Modale_Generale : Form
    {
        /// <summary>
        /// État de fonctionnement du formulaire principal
        /// </summary>
        public enum Etat
        {
            /// <summary>
            /// Fonctionnalités non disponibles pour les utilisateurs non administrateurs
            /// <para>Ce type d'utilisateur est le type par défaut au lancement de l'application</para>
            /// <para>Ce type d'utilisateur ne peut pas consulter et manipuler les données</para>
            /// </summary>
            NonAdministrateur,
            /// <summary>
            /// Fonctionnalités disponibles que pour un administrateur
            /// <para>Ce type d'utilisateur a réussi à s'identifier en tant que tel via le mot de passe d'administration</para>
            /// <para>Ce type d'utilisateur a tous les droits</para>
            /// </summary>
            Administrateur
        }

        /// <summary>
        /// État actuel du fonctionnement de la boîte modale générale
        /// </summary>
        public Etat EtatActuel { get; set; }
        public Boite_Modale_Generale()
        {
            InitializeComponent();
            EtatActuel = Etat.NonAdministrateur;
            MettreAJourEtat();

        }

        /// <summary>
        /// Permet de passer en état de fonctionnement administrateur
        /// </summary>
        /// <param name="motDePasse">Mot de passe servant à l'authentification en tant qu'administrateur</param>
        /// <returns>Vrai si on a pu passer en mode d'administration, sinon faux</returns>
        private bool DevenirAdministrateur(string passWord)
        {
            if ((EtatActuel == Etat.Administrateur) || (!Admin_Access(passWord))) return false;
            EtatActuel = Etat.Administrateur;
            UserSession.EtatSessionAdmin = true;
            MettreAJourEtat();
            return true;
        }

        /// <summary>
        /// Met à jour l'état des contrôles du formulaire en fonction de l'état actuel de fonctionnement
        /// </summary>
        public void MettreAJourEtat()
        {
            if (EtatActuel == Etat.Administrateur) zoneTexteMotDePasse.Enabled = false;
            boutonModalPrescriptions.Enabled = (EtatActuel == Etat.Administrateur);
            boutonModalMedecins.Enabled = (EtatActuel == Etat.Administrateur);
            boutonModalPatients.Enabled = (EtatActuel == Etat.Administrateur);
            boutonModalMutuelles.Enabled = (EtatActuel == Etat.Administrateur);
        }

        private void SurValidationMotDePasse(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;

                if ((!string.IsNullOrEmpty(zoneTexteMotDePasse.Text)))
                {
                    if (!DevenirAdministrateur(zoneTexteMotDePasse.Text))
                    {
                        MessageBox.Show("Ce n'est pas le mot de passe d'administration !\n" +
                            "Il vous reste tentatives", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        zoneTexteMotDePasse.Text = "";
                    }
                    else
                    {
                        MettreAJourEtat();
                    }
                }
                else
                {
                    errorProviderMenuGeneral.SetError(zoneTexteMotDePasse, "Veuillez saisir le mot de passe administrateur");
                }
                errorProviderMenuGeneral.SetError(zoneTexteMotDePasse, null);
            }
        }


        #region Bouton "Quitter" de la la boite de modale générale avec le timing
        private void boutonQuitterModalGeneral_Click(object sender, EventArgs e)
        {
            TimerBoiteModaleGenerale();
            Application.Exit();
        }

        public void TimerBoiteModaleGenerale()
        {
            Timer timer = new Timer();
            timer.Interval = 1500;
            timer.Tick += new EventHandler(TimerEnd);
        }

        public void TimerEnd(object sender, EventArgs e)
        {
            MessageBox.Show("Au revoir !");
            this.Hide();
        }
        #endregion

        #region Les 4 boutons directionnels
        private void boutonModalPatients_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Patients>(sender, this);
        }

        private void boutonModalMedecins_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Medecins>(sender, this);
        }

        private void boutonModalMutuelles_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Mutuelles>(sender, this);
        }

        private void boutonModalPrescriptions_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Prescriptions>(sender, this);
        }
        #endregion

        #region Mot de passe hébergé dans la table Admin en Access
        public static bool Admin_Access(string passWord)
        {
            if (Program.ServerOnOrOff())
            {
                long nb = Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM admin WHERE admin.PassWord = {0}", passWord);
                if (Program.Bd.GetValueWithDefault<long>(-1, "SELECT COUNT(*) FROM admin WHERE admin.PassWord = {0}", passWord) >= 1)
                {
                    return true;
                }
                return false;
            }
            else
            {
                MessageBox.Show("Erreur côté server\n" +
                                "Prenez contact avec votre Provider :\n" +
                                "Samuël Raes : 0473/934591", "Erreur Serveur", MessageBoxButtons.OK);
                return false;
            }

        }
        #endregion

        /// <summary>
        /// Fonction appelée à l'affichage du formulaire pour établir son état en admin
        /// </summary>
        /// <param name="sender">Le formulaire</param>
        /// <param name="e">L'évènement de chargement</param>
        private void Boite_Modale_Generale_Shown(object sender, EventArgs e)
        {
            if (UserSession.EtatSessionAdmin)
            {
                EtatActuel = (UserSession.EtatSessionAdmin == true) ? Etat.Administrateur : Etat.NonAdministrateur;
                MettreAJourEtat();
            }
            else
            {
                EtatActuel = Etat.NonAdministrateur;
                MettreAJourEtat();
            }
            
        }
    }

    /// <summary>
    /// Class globale à dessein de conserver le boite modale générale en administrateur
    /// </summary>
    public static class UserSession
        {
            public static bool EtatSessionAdmin { get; set; }
        }

}
