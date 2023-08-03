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
using PDSGBD_MySql;

namespace KineGestionApp
{
    public partial class Form_Modifier_Medecins : Form
    {
        private ModelesMedecins.IMedecin Medecin { get; set; }
        private ModelesMedecins.IMedecin MedecinConfirmation { get; set; }
        private IEnumerable<ModelesLocalites.ILocalite> listLocalites = Program.Localite.EnumererLocalites();
        int pos = 0;
        
        public Form_Modifier_Medecins()
        {
            InitializeComponent();
            #region Gestion de la récupération de l'ID de la localité

            /*IEnumerable<ModelesLocalites.ILocalite> */listLocalites = Program.Localite.EnumererLocalites();
            comboBoxCodePostalModifierMedecins.Items.Clear();
            foreach (var loc in listLocalites
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.CodePostal)))
            {
                comboBoxCodePostalModifierMedecins.Items.Add(loc);
            }
            comboBoxLocaliteModifierMedecins.Items.Clear();
            foreach (var loc in listLocalites
                //.EnumererLocalites()
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.NomLocalite)))
            {
                comboBoxLocaliteModifierMedecins.Items.Add(loc);
            }
            #endregion
            ShowData(pos);
        }

        private void boutonQuitterModifierMedecins_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Medecins>(sender, this);
        }

        private void boutonSauvegarderModifierMedecins_Click(object sender, EventArgs e)
        {
            int currentIDMedecin = (int.Parse(textBoxCurrentIDMedecin.Text));
            if (/*Medecin.EstValide() && */(Program.ExistenceTestID(currentIDMedecin, "medecins", "ID_Medecin")))
            {
                string message = "Confirmez vous la modification du médecin : \n" + textBoxNomModifierMedecins.Text + " " + textBoxPrenomModifierMedecins.Text + " ?\n" +
                                 "Voici les modifications : \n"
                                  + MessageBoxConfirmationModif();
                const string caption = "Confirmation modification patient";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                // S'il sélectionne le bouton Yes
                if (result == DialogResult.Yes)
                {
                    if (!Program.Medecin.MettreAJour(Medecin))
                    {
                        MessageBox.Show("Echec de la modification\n" +
                                        "Veuillez prendre contact avec votre Provider : \n" +
                                        "Samuel Raes : +32473/934591", "Error Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Extensions.OpenAndCloseForm<Boite_Modale_Medecins>(sender, this);
                    }
                    else
                    {
                        MessageBox.Show("Succès de la modification",
                                        "L'enregistrement a bien été traité", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ShowData(pos);
                    }
                }
                else
                {
                    Extensions.ClearFormControls(this);
                }
            }
            else
            {
                string message = "Les données saisies pour l'enregistrement du médecin : \n" + textBoxNomModifierMedecins.Text + " " + textBoxPrenomModifierMedecins.Text + " sont érronées \n" +
                                "Souhaitez-vous recommencer la modification de l'enregistrement ?";
                string caption = "Données érronées";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    ShowData(pos);
                }
                else
                {
                    Extensions.OpenAndCloseForm<Boite_Modale_Medecins>(sender, this);
                }
            }
        }

        private StringBuilder MessageBoxConfirmationModif()
        {
            StringBuilder message = new StringBuilder();    
            if(Medecin.NomMedecin != MedecinConfirmation.NomMedecin) 
            {
                message.Append("Nom : " + MedecinConfirmation.NomMedecin + " => " + Medecin.NomMedecin+"\n");
            }
            if(Medecin.PrenomMedecin != MedecinConfirmation.PrenomMedecin)
            {
                message.Append("Prénom : " + MedecinConfirmation.PrenomMedecin + " => " + Medecin.PrenomMedecin + "\n");
            }
            if(Medecin.NumeroInami != MedecinConfirmation.NumeroInami)
            {
                message.Append("Numéro INAMI : " + MedecinConfirmation.NumeroInami + " => " + Medecin.NumeroInami + "\n");
            }
            if(Medecin.AdresseMedecin != MedecinConfirmation.AdresseMedecin) 
            {
                message.Append("Adresse : " + MedecinConfirmation.AdresseMedecin + " => " + Medecin.AdresseMedecin + "\n");
            }
            if (Medecin.TelephoneMedecin != MedecinConfirmation.TelephoneMedecin)
            {
                message.Append("Téléphone : " + MedecinConfirmation.TelephoneMedecin + " => " + Medecin.TelephoneMedecin + "\n");
            }
            if(Medecin.EmailMedecin != MedecinConfirmation.EmailMedecin)
            {
                message.Append("Email : " + MedecinConfirmation.EmailMedecin + " => " + Medecin.EmailMedecin + "\n");
            }
            if(Medecin.Medecin_ID_Localite != MedecinConfirmation.Medecin_ID_Localite) 
            {
               string BeforeLocalite = Program.ItemFromEnumerable(listLocalites, MedecinConfirmation.Medecin_ID_Localite).NomLocalite;
               string AfterLocalite = Program.ItemFromEnumerable(listLocalites, Medecin.Medecin_ID_Localite).NomLocalite;

                message.Append("Localité : " + BeforeLocalite + " => " + AfterLocalite + "\n");
            }

            return message;
        }

        #region Navigation dans le formulaire
        private bool ShowData(int index)
        {
            //textBoxCurrentIDMedecin.Text = index.ToString();
            DBM.IRow TableMedecins = Program.Bd.GetRow(@"SELECT medecins.ID_Medecin, 
                                                                medecins.Nom, 
                                                                medecins.Prenom, 
                                                                medecins.Civilite, 
                                                                medecins.Adresse, 
                                                                medecins.Numero_INAMI, 
                                                                medecins.Email, 
                                                                medecins.Telephone, 
                                                                localites.ID_Localite, 
                                                                localites.Code_postal, 
                                                                localites.Localite 
                                                                FROM localites
                                                                INNER JOIN medecins ON localites.ID_Localite = medecins.Medecin_ID_Localite 
                                                                ORDER BY medecins.Nom, medecins.Prenom LIMIT {0}, 1", index);

          
            if (TableMedecins.Values.Count() > 0)
            {
                int testId = (int)TableMedecins.GetValue(8);
                Medecin = ModelesMedecins.CreerMedecin(
                (int)TableMedecins.GetValue(0),
                TableMedecins.GetValue(1).ToString(),
                TableMedecins.GetValue(2).ToString(),
                TableMedecins.GetValue(3).ToString(),
                TableMedecins.GetValue(4).ToString(),
                TableMedecins.GetValue(5).ToString(),
                TableMedecins.GetValue(6).ToString(),
                TableMedecins.GetValue(7).ToString(),
                (int)TableMedecins.GetValue(8)
                );

                //IEnumerable<ModelesLocalites.ILocalite> listLocalite = Program.Localite.EnumererLocalites();
                int posLocalite = Program.positionItemFromEnumerable(listLocalites, testId);
                if(posLocalite > -1) 
                {
                    textBoxCurrentIDMedecin.Text = Medecin.Id.ToString();
                    textBoxPrenomModifierMedecins.Text = Medecin.PrenomMedecin;
                    textBoxNomModifierMedecins.Text = Medecin.NomMedecin;
                    textBoxCiviliteModifierMedecins.Text = Medecin.CiviliteMedecin;
                    textBoxAdresseModifierMedecins.Text = Medecin.AdresseMedecin;
                    textBoxNumeroINAMIModifierMedecins.Text = Medecin.NumeroInami;
                    comboBoxCodePostalModifierMedecins.SelectedIndex = posLocalite;
                    textBoxTelephoneModifierMedecins.Text = Medecin.TelephoneMedecin;
                    comboBoxLocaliteModifierMedecins.SelectedIndex = posLocalite;
                    textBoxEmailModifierMedecins.Text = Medecin.EmailMedecin;


                    MedecinConfirmation = ModelesMedecins.CreerMedecin(Medecin.Id, Medecin.NomMedecin, Medecin.PrenomMedecin, Medecin.CiviliteMedecin, Medecin.AdresseMedecin, Medecin.NumeroInami, Medecin.EmailMedecin, Medecin.TelephoneMedecin, Medecin.Medecin_ID_Localite);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
            
        }

        #endregion


        private void boutonPremierEnregistrementModifierMedecins_Click(object sender, EventArgs e)
        {
            pos = 0;
            if (!ShowData(pos))
            {
                MessageBox.Show("Aucun médecin en mémoire", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);           
            }
        }

        private void boutonEnregistrementSuivantModifierMedecins_Click(object sender, EventArgs e)
        {
            pos++;
            if (!ShowData(pos))
            {
                MessageBox.Show("Dernier enregistrement atteint");
                pos--;
            }
        }

        private void boutonEnregistrementPrecedentModifierMedecins_Click(object sender, EventArgs e)
        {
            pos--;       
            if(!ShowData(pos))
            {
                MessageBox.Show("Premier enregistrement atteint");
                pos++;
            }
        }

        private void boutonDernierEnregistrementModifierMedecins_Click(object sender, EventArgs e)
        {
            pos = (int)Program.NbMaxItemsDB("medecins") - 1;
            if(pos <0)
            {
                    MessageBox.Show("Aucun médecin en mémoire", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                pos = 0;
            }
            else
            {
                ShowData(pos);
            }   
        }

        private void textBoxTelephoneModifierMedecins_Validating(object sender, CancelEventArgs e)
        {
            if (!(textBoxTelephoneModifierMedecins.Text == Medecin.TelephoneMedecin))
            {
                if (textBoxTelephoneModifierMedecins.Text == "")
                {
                    errorProviderModifierMedecins.SetError(textBoxTelephoneModifierMedecins, null);
                }
                else if (!Medecin.ModifierTelephoneMedecin(textBoxTelephoneModifierMedecins.Text))
                {
                    errorProviderModifierMedecins.SetError(textBoxTelephoneModifierMedecins, "Le numéto de téléphone doit commencer par 0032 ou + 32 et\n" +
                                                                                      "Un nombre total de chiffres entre 9 et 12\n" +
                                                                                      "Les caracètres spéciaux sont interdits (sauf le +)");
                }
                else
                {
                    if ((Program.UniquenessInDatabase(textBoxTelephoneModifierMedecins.Text, "patients", "Telephone") ||
                       (Program.UniquenessInDatabase(textBoxTelephoneModifierMedecins.Text, "medecins", "Telephone") ||
                       (Program.UniquenessInDatabase(textBoxTelephoneModifierMedecins.Text, "mutualites", "Telephone")))))
                    {
                        errorProviderModifierMedecins.SetError(textBoxTelephoneModifierMedecins, "Ce numéro de téléphone est déjà référencé\n" +
                                                                                            "Vous devez en saisir un autre");
                    }
                    else
                    {
                        errorProviderModifierMedecins.SetError(textBoxTelephoneModifierMedecins, null);
                    }

                }
            }
            else
            {
                errorProviderModifierMedecins.SetError(textBoxTelephoneModifierMedecins, null);
            }
        }

        private void textBoxAdresseModifierMedecins_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxAdresseModifierMedecins.Text == "")
            {
                errorProviderModifierMedecins.SetError(textBoxAdresseModifierMedecins, null);
            }
            else if (!Medecin.ModifierAdresseMedecin(textBoxAdresseModifierMedecins.Text))
            {
                errorProviderModifierMedecins.SetError(textBoxAdresseModifierMedecins, "L'adresse ne peut pas comporter de caractères spéciaux\nLe numéro du batiment doit être donné");
            }
            else
            {
                errorProviderModifierMedecins.SetError(textBoxAdresseModifierMedecins, null);
            }
        }

        private void comboBoxCodePostalModifierMedecins_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxLocaliteModifierMedecins.SelectedIndex = comboBoxLocaliteModifierMedecins.SelectedIndex;
        }

        private void comboBoxLocaliteModifierMedecins_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCodePostalModifierMedecins.SelectedIndex = comboBoxLocaliteModifierMedecins.SelectedIndex;
            Medecin.ModifierLocaliteMedecin((comboBoxLocaliteModifierMedecins.SelectedItem as FormattedObject<ModelesLocalites.ILocalite>).Object.Id);
        }

        private void textBoxNumeroINAMIModifierMedecins_TextChanged(object sender, EventArgs e)
        {
            if(!(textBoxNumeroINAMIModifierMedecins.Text == Medecin.NumeroInami))
            {
                if (!Medecin.ModifierNumeroInamiMedecin(textBoxNumeroINAMIModifierMedecins.Text))
                {
                    errorProviderModifierMedecins.SetError(textBoxNumeroINAMIModifierMedecins, "Numéro INAMI invalide\n" +
                                                                                             "Prenez contact avec le médecin");
                }
                else
                {
                    if (Program.UniquenessInDatabase(textBoxNumeroINAMIModifierMedecins.Text, "medecins", "Numero_INAMI"))
                    {
                        errorProviderModifierMedecins.SetError(textBoxNumeroINAMIModifierMedecins, "Ce numéro INAMI est déjà référencé\n" +
                                                                                                 "Vous devez en saisir un autre");
                    }
                    else
                    {
                        errorProviderModifierMedecins.SetError(textBoxNumeroINAMIModifierMedecins, null);
                    }
                }
            }
            else
            {
                errorProviderModifierMedecins.SetError(textBoxNumeroINAMIModifierMedecins, null);
            }
        }

        private void textBoxNomModifierMedecins_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNomModifierMedecins.Text == "")
            {
                errorProviderModifierMedecins.SetError(textBoxNomModifierMedecins, null);
            }
            else if (!Medecin.ModifierNomMedecin(textBoxNomModifierMedecins.Text))
            {
                errorProviderModifierMedecins.SetError(textBoxNomModifierMedecins, "Ce nom n'est pas valide\nLes caractères spéciaux ne sont pas acceptés");
            }
            else
            {
                errorProviderModifierMedecins.SetError(textBoxNomModifierMedecins, null);
            }
        }

        private void textBoxPrenomModifierMedecins_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPrenomModifierMedecins.Text == "")
            {
                errorProviderModifierMedecins.SetError(textBoxPrenomModifierMedecins, null);
            }
            else if (!Medecin.ModifierPrenomMedecin(textBoxPrenomModifierMedecins.Text))
            {
                errorProviderModifierMedecins.SetError(textBoxPrenomModifierMedecins, "Ce prénom n'est pas valide\nLes caractères spéciaux ne sont pas acceptés");
            }
            else
            {
                errorProviderModifierMedecins.SetError(textBoxPrenomModifierMedecins, null);
            }
        }

        private void textBoxEmailModifierMedecins_Validating(object sender, CancelEventArgs e)
        {
            if (!(textBoxEmailModifierMedecins.Text == Medecin.EmailMedecin))
            {
                if(textBoxEmailModifierMedecins.Text == "")
                {
                    errorProviderModifierMedecins.SetError(textBoxEmailModifierMedecins, null);
                }
                else if (!Medecin.ModifierEmailMedecin(textBoxEmailModifierMedecins.Text))
                {
                    errorProviderModifierMedecins.SetError(textBoxEmailModifierMedecins, "Format email invalide");
                }
                else
                {
                    if (Program.UniquenessInDatabase(textBoxEmailModifierMedecins.Text, "patients", "Email") ||
                       (Program.UniquenessInDatabase(textBoxEmailModifierMedecins.Text, "medecins", "Email") ||
                       (Program.UniquenessInDatabase(textBoxEmailModifierMedecins.Text, "mutualites", "Email"))))
                    {
                        errorProviderModifierMedecins.SetError(textBoxEmailModifierMedecins, "Cette adresse email est déjà référencée\n" +
                                                                                        "Vous devez en saisir une autre");
                    }
                    else
                    {
                        errorProviderModifierMedecins.SetError(textBoxEmailModifierMedecins, null);
                    }
                }
            }
            else
            {
                errorProviderModifierMedecins.SetError(textBoxEmailModifierMedecins, null);
            }
        }
    }
}
