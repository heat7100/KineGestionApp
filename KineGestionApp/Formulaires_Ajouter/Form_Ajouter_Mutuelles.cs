using MimeKit;
using PDSGBD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KineGestionApp
{
    public partial class Form_Ajouter_Mutuelles : Form
    {
        private ModelesMutuelles.IMutuelle Mutuelle { get; set; }
        public Form_Ajouter_Mutuelles()
        {
            InitializeComponent();

            Mutuelle = ModelesMutuelles.CreerNouvelleMutuelle();

            #region Gestion de la récupération de l'ID de la localité

            comboBoxCodePostalAjouterMutuelles.Items.Clear();
            foreach (var loc in Program.Localite
                .EnumererLocalites()
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.CodePostal)))
            {
                comboBoxCodePostalAjouterMutuelles.Items.Add(loc);
            }
            comboBoxLocaliteAjouterMutuelles.Items.Clear();
            foreach (var loc in Program.Localite
                .EnumererLocalites()
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.NomLocalite)))
            {
                comboBoxLocaliteAjouterMutuelles.Items.Add(loc);
            }
            #endregion

        }

        private void boutonQuitterAjouterMutuelles_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Mutuelles>(sender, this);
        }

        private void boutonAjouterPhotoAjouterMutuelles_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                errorProviderAjouterMutuelles.RightToLeft = false;
                string uploadedFileName = openFileDialog1.FileName;
                string extension = Path.GetExtension(uploadedFileName).ToLower();
                string mimeType = MimeTypes.GetMimeType(extension);

                if (mimeType.StartsWith("image/"))
                {
                    //Ouvre l'Explorateur de fichiers Windows pour insérer la photo du patient
                    try
                    {
                        using (var image = Image.FromFile(uploadedFileName))
                        {
                            //Ouvre l'Explorateur de fichiers Windows pour insérer la photo du patient
                            pictureBoxAjouterMutuelles.Load(openFileDialog1.FileName);
                            if (!Mutuelle.ModifierLogoMutuelle(pictureBoxAjouterMutuelles.Image))
                            {
                                pictureBoxAjouterMutuelles.Invalidate();
                                errorProviderAjouterMutuelles.SetError(boutonAjouterPhotoAjouterMutuelles, errorProviderAjouterMutuelles.GetError(boutonAjouterPhotoAjouterMutuelles) + "Format image invalide\n" +
                                                                                                         "Formats autorisés : png, jpeg");
                            }
                            else
                            {
                                errorProviderAjouterMutuelles.SetError(boutonAjouterPhotoAjouterMutuelles, null);
                                errorProviderAjouterMutuelles.RightToLeft = true;
                            }

                        }
                    }
                    catch (Exception error)
                    {
                        errorProviderAjouterMutuelles.SetError(boutonAjouterPhotoAjouterMutuelles, error.Message + "\nVeuillez recommencer l'upload");
                    }
                }
                else
                {
                    errorProviderAjouterMutuelles.SetError(boutonAjouterPhotoAjouterMutuelles, "Il ne s'agit pas d'une image valide");
                }
            }
        }

        private void boutonSupprimerPhotoAjouterMutuelles_Click(object sender, EventArgs e)
        {
            //Supprimer la photo ajoutée précédemment
            pictureBoxAjouterMutuelles.Image = null;
        }

        private void boutonSauvegarderEnregistrementAjouterMutuelles_Click(object sender, EventArgs e)
        { 
            if (Mutuelle.EstValide())
            {
                //Extensions.ErrorProviderFields(this, "Aucun champ ne peut être vide et  doit comporter 0 <> 20 caractères ! ", errorProviderAjouterMutuelles, 20);
                string message = "Confirmez vous l'enregistrement du médecin : \n" + textBoxMutuelleAjouterMutuelles.Text + " ?\n" +
                      "Vérifiez bien les champs saisis du formulaire";
                const string caption = "Confirmation enregistrement nouvelle mutuelle";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                // S'il sélectionne le bouton Yes
                if (result == DialogResult.Yes)
                {
                    if (!Program.Mutuelle.Ajouter(Mutuelle))
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

                }//On supprimer tout ce qui a été sélectionné dans le formulaire
                else
                {
                    Extensions.ClearFormControls(this);
                }
            }
            else
            {
                string message = "Les données saisies pour l'enregistrement de la mutuelle : \n" + textBoxMutuelleAjouterMutuelles.Text + " " + "sont érronées \n" +
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
                    Extensions.OpenAndCloseForm<Boite_Modale_Mutuelles>(sender, this);
                }
            }

        }

        private void boutonAnnulerEnregistrementAjouterMutuelles_Click(object sender, EventArgs e)
        {
            Extensions.ClearFormControls(this);
        }

        private void boutonAjouterEnregistrementAjouterMutuelles_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.Text.Any();
                if (MessageBox.Show("Etes-vous sûr d'abandonner l'enregistrement en cours ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Extensions.ClearFormControls(this);
                }
            }
        }

        private void textBoxMutuelleAjouterMutuelles_TextChanged(object sender, EventArgs e)
        {
            if(textBoxMutuelleAjouterMutuelles.Text == "")
            {
                errorProviderAjouterMutuelles.SetError(textBoxMutuelleAjouterMutuelles, null);
            }
            else if (!Mutuelle.ModifierNomMutuelle(textBoxMutuelleAjouterMutuelles.Text)) 
            {
                errorProviderAjouterMutuelles.SetError(textBoxMutuelleAjouterMutuelles, "Ce nom n'est pas valide\nLes caractères spéciaux ne sont pas acceptés");
            }
            else
            {
                if(Program.UniquenessInDatabase(textBoxMutuelleAjouterMutuelles.Text, "mutualites", "Mutualite"))
                {
                    errorProviderAjouterMutuelles.SetError(textBoxMutuelleAjouterMutuelles, "Ce nom de mutuelle est déjà référencé\n" +
                                                                                          "Vous devez saisir un autre nom");
                }
                else
                {
                    errorProviderAjouterMutuelles.SetError(textBoxMutuelleAjouterMutuelles, null);
                }
                
            }
        }

        private void textBoxAdresseAjouterMutuelles_Validating(object sender, CancelEventArgs e)
        {
            if(textBoxAdresseAjouterMutuelles.Text == "")
            {
                errorProviderAjouterMutuelles.SetError(textBoxAdresseAjouterMutuelles, null);
            }
            else if (!Mutuelle.ModifierAdresseMutuelle(textBoxAdresseAjouterMutuelles.Text))
            {
                errorProviderAjouterMutuelles.SetError(textBoxAdresseAjouterMutuelles, "L'adresse ne peut pas comporter de caractères spéciaux\nLe numéro du batiment doit être donné");
            }
            else
            {
                errorProviderAjouterMutuelles.SetError(textBoxAdresseAjouterMutuelles, null);
            }
        }

        private void textBoxTelephoneAjouterMutuelles_Validating(object sender, CancelEventArgs e)
        {
            if(textBoxTelephoneAjouterMutuelles.Text == "")
            {
                errorProviderAjouterMutuelles.SetError(textBoxTelephoneAjouterMutuelles, null);
            }
            else if (!Mutuelle.ModifierTelephoneMutuelle(textBoxTelephoneAjouterMutuelles.Text))
            {
                errorProviderAjouterMutuelles.SetError(textBoxTelephoneAjouterMutuelles, "Le numéto de téléphone doit commencer par 0032 ou +32 et\n" +
                                                                                  "Un nombre total de chiffres entre 9 et 12\n" +
                                                                                  "Les caracètres spéciaux sont interdits (sauf le +)");
            }
            else
            {
                if ((Program.UniquenessInDatabase(textBoxTelephoneAjouterMutuelles.Text, "patients", "Telephone") ||
                (Program.UniquenessInDatabase(textBoxTelephoneAjouterMutuelles.Text, "medecins", "Telephone") ||
                (Program.UniquenessInDatabase(textBoxTelephoneAjouterMutuelles.Text, "mutualites", "Telephone")))))
                {
                    errorProviderAjouterMutuelles.SetError(textBoxTelephoneAjouterMutuelles, "Ce numéro de téléphone est déjà référencé\n" +
                                                                                                "Vous devez en saisir un autre");
                }
                else
                {
                    errorProviderAjouterMutuelles.SetError(textBoxTelephoneAjouterMutuelles, null);
                }
            }
        }
        private void textBoxEmailAjouterMutuelles_Validating(object sender, CancelEventArgs e)
        {
            if(textBoxEmailAjouterMutuelles.Text == "")
            {
                errorProviderAjouterMutuelles.SetError(textBoxEmailAjouterMutuelles, null);
            }
            else if (!Mutuelle.ModifierEmailMutuelle(textBoxEmailAjouterMutuelles.Text))
            {
                errorProviderAjouterMutuelles.SetError(textBoxEmailAjouterMutuelles, "Format email non valide");
            }
            else
            {
                if (Program.UniquenessInDatabase(textBoxEmailAjouterMutuelles.Text, "patients", "Email") ||
                   (Program.UniquenessInDatabase(textBoxEmailAjouterMutuelles.Text, "medecins", "Email") ||
                   (Program.UniquenessInDatabase(textBoxEmailAjouterMutuelles.Text, "mutualites", "Email"))))
                {
                    errorProviderAjouterMutuelles.SetError(textBoxEmailAjouterMutuelles, "Cette adresse email est déjà référencée\n" +
                                                                                    "Vous devez en saisir une autre");
                }
                else
                {
                    errorProviderAjouterMutuelles.SetError(textBoxEmailAjouterMutuelles, null);
                }
            }
        }

        private void comboBoxCodePostalAjouterMutuelles_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxLocaliteAjouterMutuelles.SelectedIndex = comboBoxCodePostalAjouterMutuelles.SelectedIndex;
        }

        private void comboBoxLocaliteAjouterMutuelles_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCodePostalAjouterMutuelles.SelectedIndex = comboBoxLocaliteAjouterMutuelles.SelectedIndex;
            Mutuelle.ModifierLocaliteMutuelle((comboBoxLocaliteAjouterMutuelles.SelectedItem as FormattedObject<ModelesLocalites.ILocalite>).Object.Id);
        }
    }
}
