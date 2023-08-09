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
    public partial class Form_Ajouter_Medecins : Form
    {
        private ModelesMedecins.IMedecin Medecin  { get ; set ;}
        public Form_Ajouter_Medecins()
        {
            InitializeComponent();
            Medecin = ModelesMedecins.CreerNouveauMedecin();

            #region Gestion de la récupération de l'ID de la localité

            comboBoxCodePostalAjouterMedecins.Items.Clear();
            foreach (var loc in Program.Localite
                .EnumererLocalites()
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.CodePostal)))
            {
                comboBoxCodePostalAjouterMedecins.Items.Add(loc);
            }
            comboBoxLocaliteAjouterMedecins.Items.Clear();
            foreach (var loc in Program.Localite
                .EnumererLocalites()
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.NomLocalite)))
            {
                comboBoxLocaliteAjouterMedecins.Items.Add(loc);
            }
            #endregion
        }

        private void boutonSauvegarderEnregistrementAjouterMedecins_Click(object sender, EventArgs e)
        {
            if (Medecin.EstValide())
            {
                //Extensions.ErrorProviderFields(this, " Aucun champ ne peut être vide et doit comporter 0 <> 20 caractètres ! ", errorProviderAjouterMedecins, 20);
                string message = "Confirmez vous l'enregistrement du médecin : \n" + textBoxNomAjouterMedecins.Text + " " + textBoxPrenomAjouterMedecins.Text + " ?\n" +
                                 "Vérifiez bien les champs saisis du formulaire";
                const string caption = "Confirmation enregistrement nouveau patient";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                // S'il sélectionne le bouton Yes
                if (result == DialogResult.Yes)
                {
                    if (!Program.Medecin.Ajouter(Medecin))
                    {
                        MessageBox.Show("Echec de l'ajout\n" +
                                        "Veuillez prendre contact avec votre Provider : \n" +
                                        "Samuel Raes : +32473/934591", "Error Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Extensions.ClearFormControls(this);
                    }
                    else
                    {
                        Extensions.ClearFormControls(this);
                        MessageBox.Show("Succès de l'ajout",
                                        "L'enregistrement a bien été traité", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    Extensions.ClearFormControls(this);
                }
            }
            else
            {
                string message = "Les données saisies pour l'enregistrement du médecin : \n" + textBoxNomAjouterMedecins.Text + " " + textBoxPrenomAjouterMedecins.Text + " sont érronées \n" +
                                 "Souhaitez-vous recommencer l'enregistrement ?";
                string caption = "Données érronées";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    Extensions.ClearFormControls(this);
                }
                else
                {
                    Extensions.OpenAndCloseForm<Boite_Modale_Patients>(sender, this);
                }
            }
        }

        private void boutonAnnulerEnregistrementAjouterMedecins_Click(object sender, EventArgs e)
        {
            Extensions.ClearFormControls(this);
        }

        private void textBoxNumeroINAMIAjouterMedecins_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxNumeroINAMIAjouterMedecins.Text == "")
            {
                errorProviderAjouterMedecins.SetError(textBoxNumeroINAMIAjouterMedecins, null);
            }
            else if (!Medecin.ModifierNumeroInamiMedecin(textBoxNumeroINAMIAjouterMedecins.Text))
            {
                errorProviderAjouterMedecins.SetError(textBoxNumeroINAMIAjouterMedecins, "Numéro INAMI invalide\n" +
                                                                                         "Prenez contact avec le médecin");
            }
            else
            {
                if (Program.UniquenessInDatabase(textBoxNumeroINAMIAjouterMedecins.Text, "medecins", "Numero_INAMI"))
                {
                    errorProviderAjouterMedecins.SetError(textBoxNumeroINAMIAjouterMedecins, "Ce numéro INAMI est déjà référencé\n" +
                                                                                             "Prenez contact avec le médecin");
                }
                else
                {
                    errorProviderAjouterMedecins.SetError(textBoxNumeroINAMIAjouterMedecins, null);
                }
                
            }
        }

        private void textBoxTelephoneAjouterMedecins_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxTelephoneAjouterMedecins.Text == "")
            {
                errorProviderAjouterMedecins.SetError(textBoxTelephoneAjouterMedecins, null);
            }
            else if (!Medecin.ModifierTelephoneMedecin(textBoxTelephoneAjouterMedecins.Text))
            {
                errorProviderAjouterMedecins.SetError(textBoxTelephoneAjouterMedecins, "Le numéto de téléphone doit commencer par 0032 ou + 32 et\n" +
                                                                                  "Un nombre total de chiffres entre 9 et 12\n" +
                                                                                  "Les caracètres spéciaux sont interdits (sauf le +)");
            }
            else
            {
                if ((Program.UniquenessInDatabase(textBoxTelephoneAjouterMedecins.Text, "patients", "Telephone") ||
                   (Program.UniquenessInDatabase(textBoxTelephoneAjouterMedecins.Text, "medecins", "Telephone") ||
                   (Program.UniquenessInDatabase(textBoxTelephoneAjouterMedecins.Text, "mutualites", "Telephone")))))
                {
                    errorProviderAjouterMedecins.SetError(textBoxTelephoneAjouterMedecins, "Ce numéro de téléphone est déjà référencé\n" +
                                                                                        "Vous devez en saisir un autre");
                }
                else
                {
                    errorProviderAjouterMedecins.SetError(textBoxTelephoneAjouterMedecins, null);
                }

            }
        }
        private void textBoxEmailAjouterMedecins_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxEmailAjouterMedecins.Text == "")
            {
                errorProviderAjouterMedecins.SetError(textBoxEmailAjouterMedecins, null);
            }
            else if (!Medecin.ModifierEmailMedecin(textBoxEmailAjouterMedecins.Text))
            {
                errorProviderAjouterMedecins.SetError(textBoxEmailAjouterMedecins, "Format email invalide");
            }
            else
            {
                if (Program.UniquenessInDatabase(textBoxEmailAjouterMedecins.Text, "patients", "Email") ||
                   (Program.UniquenessInDatabase(textBoxEmailAjouterMedecins.Text, "medecins", "Email") ||
                   (Program.UniquenessInDatabase(textBoxEmailAjouterMedecins.Text, "mutualites", "Email"))))
                {
                    errorProviderAjouterMedecins.SetError(textBoxEmailAjouterMedecins, "Cette adresse email est déjà référencée\n" +
                                                                                    "Vous devez en saisir une autre");
                }
                else
                {
                    errorProviderAjouterMedecins.SetError(textBoxEmailAjouterMedecins, null);
                }

            }
        }

        private void textBoxNomAjouterMedecins_TextChanged(object sender, EventArgs e)
        {
            if(textBoxNomAjouterMedecins.Text == "")
            {
                errorProviderAjouterMedecins.SetError(textBoxNomAjouterMedecins, null);
            }
            else if (!Medecin.ModifierNomMedecin(textBoxNomAjouterMedecins.Text))
            {
                errorProviderAjouterMedecins.SetError(textBoxNomAjouterMedecins, "Ce nom n'est pas valide\nLes caractères spéciaux ne sont pas acceptés");
            }
            else
            {
                errorProviderAjouterMedecins.SetError(textBoxNomAjouterMedecins, null);
            }
        }

        private void textBoxPrenomAjouterMedecins_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPrenomAjouterMedecins.Text == "")
            {
                errorProviderAjouterMedecins.SetError(textBoxPrenomAjouterMedecins, null);
            }
            else if (!Medecin.ModifierPrenomMedecin(textBoxPrenomAjouterMedecins.Text))
            {
                errorProviderAjouterMedecins.SetError(textBoxPrenomAjouterMedecins, "Ce prénom n'est pas valide\nLes caractères spéciaux ne sont pas acceptés");
            }
            else
            {
                errorProviderAjouterMedecins.SetError(textBoxPrenomAjouterMedecins, null);
            }
        }

        private void boutonQuitterAjouterMedecins_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Medecins>(sender, this);
        }

        private void comboBoxCodePostalAjouterMedecins_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxLocaliteAjouterMedecins.SelectedIndex = comboBoxCodePostalAjouterMedecins.SelectedIndex;
        }

        private void comboBoxLocaliteAjouterMedecins_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCodePostalAjouterMedecins.SelectedIndex = comboBoxLocaliteAjouterMedecins.SelectedIndex;
            Medecin.ModifierLocaliteMedecin((comboBoxLocaliteAjouterMedecins.SelectedItem as FormattedObject<ModelesLocalites.ILocalite>).Object.Id);
        }


        private void textBoxAdresseAjouterMedecins_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxAdresseAjouterMedecins.Text == "")
            {
                errorProviderAjouterMedecins.SetError(textBoxAdresseAjouterMedecins, null);
            }
            else if (!Medecin.ModifierAdresseMedecin(textBoxAdresseAjouterMedecins.Text))
            {
                errorProviderAjouterMedecins.SetError(textBoxAdresseAjouterMedecins, "L'adresse ne peut pas comporter de caractères spéciaux\nLe numéro du batiment doit être donné");
            }
            else
            {
                errorProviderAjouterMedecins.SetError(textBoxAdresseAjouterMedecins, null);
            }
        }

        private void comboBoxCiviliteAjouterMedecins_SelectedIndexChanged(object sender, EventArgs e)
        {
            Medecin.ModifierCiviliteMedecin(comboBoxCiviliteAjouterMedecins.SelectedItem.ToString());
        }
    }
}
