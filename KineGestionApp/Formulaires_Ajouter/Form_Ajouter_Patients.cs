using PDSGBD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MimeKit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Org.BouncyCastle.Tls;
using System.Text.RegularExpressions;

namespace KineGestionApp
{
    public partial class Form_Ajouter_Patients : Form
    {
        private ModelesPatients.IPatient Patient { get; set; }
        private ModelesMutuelles.IMutuelle Mutuelle { get; set; }

        public Form_Ajouter_Patients()
        {
            InitializeComponent();
            Patient = ModelesPatients.CreerNouveauPatient();
            #region Gestion de la récupération des codes postaux et des localités

            comboBoxCodePostalAjouterPatients.Items.Clear();
            foreach (var loc in Program.Localite
                .EnumererLocalites()
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.CodePostal)))
            {
                comboBoxCodePostalAjouterPatients.Items.Add(loc);
            }
            comboBoxLocaliteAjouterPatients.Items.Clear();
            foreach (var loc in Program.Localite
                .EnumererLocalites()
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.NomLocalite)))
            {
                comboBoxLocaliteAjouterPatients.Items.Add(loc);
            }
            #endregion

            #region Gestion de la récupération des mutuelles
            listBoxMutuellesAjouterPatients.Items.Clear();
            foreach (var mut in Program.Mutuelle
                .EnumererMutuelles()
                .Select(mutuelle => new FormattedObject<ModelesMutuelles.IMutuelle>(mutuelle, e => e.NomMutuelle)))
            {
                listBoxMutuellesAjouterPatients.Items.Add(mut);
            }
            #endregion
        }

        //private string PrendreCodePostal(ModelesLocalites.ILocalite localite) 
        //{
        //    return localite.CodePostal;
        //}

        //private string PrendreNom(ModelesLocalites.ILocalite localite)
        //{
        //    return localite.NomLocalite;
        //}

        private void boutonChargerPhotoAjouterPatients_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
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
                            Extensions.ResizeImageAccordingToPictureBox(image, pictureBoxAjouterPatients);
                            pictureBoxAjouterPatients.Load(openFileDialog1.FileName);
                            if (!Patient.ModifierPhotoPatient(pictureBoxAjouterPatients.Image))
                            {
                                pictureBoxAjouterPatients.Invalidate();

                                errorProviderPhotoAjouterPatients.SetError(boutonSupprimerPhotoAjouterPatients, "Format image invalide\n" +
                                                                                                         "Formats autorisés : png, jpeg");
                            }
                            else
                            {
                                errorProviderPhotoAjouterPatients.SetError(boutonChargerPhotoAjouterPatients, null);
                            }

                        }
                    }
                    catch (Exception error)
                    {
                        errorProviderPhotoAjouterPatients.SetError(boutonChargerPhotoAjouterPatients, error.Message + "\nVeuillez recommencer l'upload");
                    }
                }
                else
                {
                    errorProviderPhotoAjouterPatients.SetError(boutonChargerPhotoAjouterPatients, "Il ne s'agit pas d'une image valide");
                }
            }
        }


        private void boutonSupprimerPhotoAjouterPatients_Click(object sender, EventArgs e)
        {
            //Supprimer la photo ajoutée précédemment
            pictureBoxAjouterPatients.Image = null;
        }

        private void boutonQuitterAjouterPatient_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Patients>(sender, this);
        }

        private void boutonSauvegarderEnregistrementAjouterPatients_Click(object sender, EventArgs e)
        {
            if (Patient.EstValide())
            {
                //Extensions.ErrorProviderFields(this, "Aucun champ ne peut être vide et  0 <> 20! ", errorProviderAjouterPatients, 20);
                string message = "Confirmez vous l'enregistrement du patient : \n" + textNomAjouterPatients.Text + " " + textPrenomAjouterPatients.Text + " ?\n" +
                      "Vérifiez bien les champs saisis du formulaire";
                string caption = "Confirmation enregistrement nouveau patient";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                // S'il sélectionne le bouton Yes
                if (result == DialogResult.Yes)
                {
                    if (!Program.Patient.Ajouter(Patient))
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
                string message = "Les données saisies pour l'enregistrement du patient : \n" + textNomAjouterPatients.Text + " " + textPrenomAjouterPatients.Text + " sont érronées \n" +
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

        private void boutonAnnulerEnregistrementAjouterPatients_Click(object sender, EventArgs e)
        {
            Extensions.ClearFormControls(this);
        }

        private void boutonAjouterEnregistrementAjouterPatients_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Etes-vous sûr d'abandonner l'enregistrement en cours ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Extensions.ClearFormControls(this);
                }
        }

        private void textNomAjouterPatients_TextChanged(object sender, EventArgs e)
        {
            if(textNomAjouterPatients.Text == "")
            {
                errorProviderAjouterPatients.SetError(textNomAjouterPatients, null);
            }
            else if (!Patient.ModifierNomPatient(textNomAjouterPatients.Text))
            {
                errorProviderAjouterPatients.SetError(textNomAjouterPatients, "Ce nom n'est pas valide\nLes caractères spéciaux ne sont pas acceptés");
            }
            else
            {
                errorProviderAjouterPatients.SetError(textNomAjouterPatients, null);
            }
        }

        private void textPrenomAjouterPatients_TextChanged(object sender, EventArgs e)
        {
            if(textPrenomAjouterPatients.Text == "")
            {
                errorProviderAjouterPatients.SetError(textPrenomAjouterPatients, null);
            }
            else if (!Patient.ModifierPrenomPatient(textPrenomAjouterPatients.Text))
            {
                errorProviderAjouterPatients.SetError(textPrenomAjouterPatients, "Ce prénom n'est pas valide\nLes caractères spéciaux ne sont pas acceptés");
            }
            else
            {
                errorProviderAjouterPatients.SetError(textPrenomAjouterPatients, null);
            }
        }

        private void textAdresseAjouterPatients_Validating(object sender, CancelEventArgs e)
        {
            if (textAdresseAjouterPatients.Text == "")
            {
                errorProviderAjouterPatients.SetError(textAdresseAjouterPatients, null);
            }
            else if (!Patient.ModifierAdressePatient(textAdresseAjouterPatients.Text))
            {
                errorProviderAjouterPatients.SetError(textAdresseAjouterPatients, "L'adresse ne peut pas comporter de caractères spéciaux\nLe numéro du batiment doit être donné");
            }
            else
            {
                errorProviderAjouterPatients.SetError(textAdresseAjouterPatients, null);
            }
        }


        private void textEmailAjouterPatients_Validating(object sender, CancelEventArgs e)
        {
            if(textEmailAjouterPatients.Text == "")
            {
                errorProviderAjouterPatients.SetError(textEmailAjouterPatients, null);
            }
            else if (!Patient.ModifierEmailPatient(textEmailAjouterPatients.Text))
            {
                errorProviderAjouterPatients.SetError(textEmailAjouterPatients, "Format email non valide");
            }
            else
            {
                if (Program.UniquenessInDatabase(textEmailAjouterPatients.Text, "patients", "Email") ||
                  (Program.UniquenessInDatabase(textEmailAjouterPatients.Text, "medecins", "Email") ||
                  (Program.UniquenessInDatabase(textEmailAjouterPatients.Text, "mutualites", "Email"))))
                {
                    errorProviderAjouterPatients.SetError(textEmailAjouterPatients, "Cette adresse email est déjà référencée\n" +
                                                                                    "Vous devez en saisir une autre");
                }
                else
                {
                    errorProviderAjouterPatients.SetError(textEmailAjouterPatients, null);
                }
            }
        }

        private void textTelephoneAjouterPatients_Validating(object sender, CancelEventArgs e)
        {
            if(textTelephoneAjouterPatients.Text == "")
            {
                errorProviderAjouterPatients.SetError(textTelephoneAjouterPatients, null);
            }
            else if (!Patient.ModifierTelephonePatient(textTelephoneAjouterPatients.Text))
            {
                errorProviderAjouterPatients.SetError(textTelephoneAjouterPatients, "Le numéto de téléphone doit commencer par 0032 ou +32 et\n" +
                                                                                  "Un nombre total de chiffres entre 9 et 12\n" +
                                                                                  "Les caracètres spéciaux sont interdits (sauf le +)");
            }
            else
            {
                if ((Program.UniquenessInDatabase(textTelephoneAjouterPatients.Text, "patients", "Telephone") ||
                    (Program.UniquenessInDatabase(textTelephoneAjouterPatients.Text, "medecins", "Telephone") ||
                    (Program.UniquenessInDatabase(textTelephoneAjouterPatients.Text, "mutualites", "Telephone")))))
                {
                    errorProviderAjouterPatients.SetError(textTelephoneAjouterPatients, "Ce numéro de téléphone est déjà référencé\n" +
                                                                                        "Vous devez en saisir un autre");
                }
                else
                {
                    errorProviderAjouterPatients.SetError(textTelephoneAjouterPatients, null);
                }
            }
        }


        private void dateTimePickerDateNaissanceAjouterPatients_ValueChanged(object sender, CancelEventArgs e)
        {

        }

        private void textDossierAjouterPatients_Validating(object sender, CancelEventArgs e)
        {
            if(textDossierAjouterPatients.Text == "")
            {
                errorProviderAjouterPatients.SetError(textDossierAjouterPatients, null);
            }
            else if (!Patient.ModifierDossierPatient(textDossierAjouterPatients.Text))
            {
                errorProviderAjouterPatients.SetError(textDossierAjouterPatients, "Format nom dossier nom valide\n" +
                                                                                  "Format : Prenom_Nom\n" +
                                                                                  "Le dossier doit être unique");
            }
            else
            {
                errorProviderAjouterPatients.SetError(textDossierAjouterPatients, null);
            }
        }

        private void checkBoxVipoAjouterPatient_CheckedChanged(object sender, EventArgs e)
        {
            //Pas d'ErrorProvider à définir ; si coché TRue, sinon false
            Patient.ModifierVipoPatient(checkBoxVipoAjouterPatient.Checked);
        }

        private void textBoxCommentaireAjouterPatients_TextChanged(object sender, EventArgs e)
        {
            if(textBoxCommentaireAjouterPatients.Text == "")
            {
                errorProviderAjouterPatients.SetError(textBoxCommentaireAjouterPatients, null);
            }
            else if (!Patient.ModifierCommentairePatient(textBoxCommentaireAjouterPatients.Text))
            {
                errorProviderAjouterPatients.SetError(textBoxCommentaireAjouterPatients, "La limite textuelle est de 500 caractères");
            }
            else
            {
                errorProviderAjouterPatients.SetError(textBoxCommentaireAjouterPatients, null);
            }
        }

        private void textNumeroAffilationMutuelleAjouterPatients_Validating(object sender, CancelEventArgs e)
        {
            if (textNumeroAffilationMutuelleAjouterPatients.Text == "")
            {
                errorProviderAjouterPatients.SetError(textNumeroAffilationMutuelleAjouterPatients, null);
            }
            else if (!Patient.ModifierNumeroAffiliationMutuellePatient(textNumeroAffilationMutuelleAjouterPatients.Text))
            {
                string link = String.Format("<a href=https://www.ocm-cdz.be/fr/particuliers/votre-mutualite-et-vous/votre-affiliation#:~:text=%C3%80%20quel%20organisme%20%C3%AAtes-vous,se%20compose%20de%20trois%20chiffres>Pour plus cliquez</a>").ToString();
                errorProviderAjouterPatients.SetError(textNumeroAffilationMutuelleAjouterPatients, "Format d'affiliation invalide\n" +
                                                                                                    link);
            }
            else
            {
                if (Program.UniquenessInDatabase(textNumeroAffilationMutuelleAjouterPatients.Text, "patients", "NumeroAffiliation"))
                {

                    errorProviderAjouterPatients.SetError(textNumeroAffilationMutuelleAjouterPatients, "Ce numéro d'affiliation existe déjà\n" +
                                                                                        "Vous devez en saisir un autre");
                }
                else
                {
                    errorProviderAjouterPatients.SetError(textNumeroAffilationMutuelleAjouterPatients, null);
                }
            }
        }

        private void comboBoxCodePostalAjouterPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxLocaliteAjouterPatients.SelectedIndex = comboBoxCodePostalAjouterPatients.SelectedIndex;
        }

        private void comboBoxLocaliteAjouterPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCodePostalAjouterPatients.SelectedIndex = comboBoxLocaliteAjouterPatients.SelectedIndex;
            Patient.ModifierLocalitePatient((comboBoxLocaliteAjouterPatients.SelectedItem as FormattedObject<ModelesLocalites.ILocalite>).Object.Id);
        }

        private void listBoxMutuellesAjouterPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mutuelle = Program.Mutuelle.ChargerMutuelles(listBoxMutuellesAjouterPatients.SelectedIndex);
            pictureBoxLogoMutuelleAjouterPatients.Image = Mutuelle.LogoMutuelle;
            Extensions.ResizeImageAccordingToPictureBox(pictureBoxLogoMutuelleAjouterPatients.Image, pictureBoxLogoMutuelleAjouterPatients);
            pictureBoxLogoMutuelleAjouterPatients.SizeMode = PictureBoxSizeMode.Zoom;
            //Patient.ModifierMutuellePatient(listBoxMutuellesAjouterPatients.SelectedIndex + 1);
            Patient.ModifierMutuellePatient((listBoxMutuellesAjouterPatients.SelectedItem as FormattedObject<ModelesMutuelles.IMutuelle>).Object.Id);
        }

        private void dateTimePickerDateNaissanceAjouterPatients_ValueChanged(object sender, EventArgs e)
        {
            Patient.ModifierDateNaissancePatient(dateTimePickerDateNaissanceAjouterPatients.Value);
        }

        private void comboBoxCiviliteAjouterPatient_SelectedValueChanged(object sender, EventArgs e)
        {
            Patient.ModifierCivilitePatient(comboBoxCiviliteAjouterPatient.SelectedItem.ToString());
        }
    }
}

