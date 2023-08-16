using MimeKit;
using PDSGBD;
using PDSGBD_MySql;
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
    public partial class Form_Modifier_Patients : Form
    {
        private ModelesPatients.IPatient Patient { get; set; }
        private ModelesMutuelles.IMutuelle Mutuelle { get; set; }   
        private ModelesPatients.IPatient PatientConfirmation { get; set; }
        private IEnumerable<ModelesLocalites.ILocalite> listLocalites = Program.Localite.EnumererLocalites();
        private IEnumerable<ModelesMutuelles.IMutuelle> listMutuelles = Program.Mutuelle.EnumererMutuelles();
        int pos = 0;


        public Form_Modifier_Patients()
        {
            InitializeComponent();

            #region Gestion de la récupération de l'ID de la localité

            comboBoxCodePostalModifierPatients.Items.Clear();
            foreach (var loc in listLocalites
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.CodePostal)))
            {
                comboBoxCodePostalModifierPatients.Items.Add(loc);
            }
            comboBoxLocaliteModifierPatients.Items.Clear();
            foreach (var loc in listLocalites
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.NomLocalite)))
            {
                comboBoxLocaliteModifierPatients.Items.Add(loc);
            }
            #endregion

            #region Gestion de la récupération de l'ID de la mutuelle
            listBoxMutuellesModifierPatients.Items.Clear();
            foreach (var mut in listMutuelles
                .Select(mutuelle => new FormattedObject<ModelesMutuelles.IMutuelle>(mutuelle, e => e.NomMutuelle)))
            {
                listBoxMutuellesModifierPatients.Items.Add(mut);
            }
            #endregion

            ShowData(pos);
        }

        private void boutonQuitterModificationPatients_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Patients>(sender, this);
        }

        private void boutonModifierPhotoPatientModifierPatients_Click(object sender, EventArgs e)
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
                                pictureBoxPhotoPatientModifierPatients.Load(openFileDialog1.FileName);
                                if (!Patient.ModifierPhotoPatient(pictureBoxPhotoPatientModifierPatients.Image))
                                {
                                    pictureBoxPhotoPatientModifierPatients.Invalidate();
                                    errorProviderModifierPatients.SetError(boutonModifierPhotoPatientModifierPatients, "Format image invalide\n" +
                                                                                                             "Formats autorisés : png, jpeg");
                                }
                                else
                                {
                                    errorProviderModifierPatients.SetError(boutonModifierPhotoPatientModifierPatients, null);
                                }
                            }
                        }
                        catch (Exception error)
                        {
                            errorProviderModifierPatients.SetError(boutonModifierPhotoPatientModifierPatients, error.Message + "\nVeuillez recommencer l'upload");
                        }
                    }
                    else
                    {
                        errorProviderModifierPatients.SetError(boutonModifierPhotoPatientModifierPatients, "Il ne s'agit pas d'une image valide");
                    }
            }
        }

        private void boutonSupprimerPhotoPatientModifierPatient_Click(object sender, EventArgs e)
        {
            //Supprimer la photo ajoutée précédemment
            pictureBoxPhotoPatientModifierPatients.Image = null;
        }

        private void boutonSauvegarderModificationPatients_Click(object sender, EventArgs e)
        {
            int currentIDPartient = (int.Parse(textBoxCurrentIdPatient.Text));
            if (Patient.EstValide() && (Program.ExistenceTestID(currentIDPartient, "patients", "ID_Patients")))
            {
                string message = "Confirmez vous la modification du patient : \n" + textNomModifierPatients.Text + " " + textPrenomModifierPatients.Text + " ?\n" +
                                 "Voici les modifications : \n" +
                                 MessageBoxConfirmationModif();
                const string caption = "Confirmation modification patient";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                // S'il sélectionne le bouton Yes
                if (result == DialogResult.Yes)
                {
                    if (!Program.Patient.MettreAJour(Patient))
                    {
                        MessageBox.Show("Echec de la modification\n" +
                                        "Veuillez prendre contact avec votre Provider : \n" +
                                        "Samuel Raes : +32473/934591", "Error Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Extensions.OpenAndCloseForm<Boite_Modale_Patients>(sender, this);
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
                string message = "Les données saisies pour l'enregistrement du patient : \n" + textNomModifierPatients.Text + " " + textPrenomModifierPatients.Text + " sont érronées \n" +
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
                    Extensions.OpenAndCloseForm<Boite_Modale_Patients>(sender, this);
                }
            }

        }

        private StringBuilder MessageBoxConfirmationModif()
        {
            StringBuilder message = new StringBuilder();
            if (Patient.NomPatient != PatientConfirmation.NomPatient)
            {
                message.Append("Nom : " + PatientConfirmation.NomPatient + " => " + Patient.NomPatient + "\n");
            }
            if (Patient.PrenomPatient != PatientConfirmation.PrenomPatient)
            {
                message.Append("Prénom : " + PatientConfirmation.PrenomPatient + " => " + Patient.PrenomPatient + "\n");
            }
            if (Patient.AdressePatient != PatientConfirmation.AdressePatient)
            {
                message.Append("Adresse : " + PatientConfirmation.AdressePatient + " => " + Patient.AdressePatient + "\n");
            }
            if (Patient.TelephonePatient != PatientConfirmation.TelephonePatient)
            {
                message.Append("Téléphone : " + PatientConfirmation.TelephonePatient + " => " + Patient.TelephonePatient + "\n");
            }
            if (Patient.EmailPatient != PatientConfirmation.EmailPatient)
            {
                message.Append("Email : " + PatientConfirmation.EmailPatient + " => " + Patient.EmailPatient + "\n");
            }
            if (Patient.Patient_ID_Localite != PatientConfirmation.Patient_ID_Localite)
            {
                string BeforeLocalite = Program.ItemFromEnumerable(listLocalites, PatientConfirmation.Patient_ID_Localite).NomLocalite;
                string AfterLocalite = Program.ItemFromEnumerable(listLocalites, Patient.Patient_ID_Localite).NomLocalite;

                message.Append("Localité : " + BeforeLocalite + " => " + AfterLocalite + "\n");
            }
            if(Patient.PhotoPatient!= PatientConfirmation.PhotoPatient)
            {
                message.Append("La photo a été modifiée");
            }

            return message;
        }

        #region Navigation dans le formulaire

        private bool ShowData(int index)
        {
            DBM.IRow TablePatients = Program.Bd.GetRow(@"SELECT 
                                                                    patients.ID_Patients, 
                                                                    patients.Nom, 
                                                                    patients.Prenom, 
                                                                    patients.Civilite, 
                                                                    patients.Date_de_naissance, 
                                                                    patients.Adresse, 
                                                                    localites.ID_Localite, 
                                                                    patients.Vipo,
                                                                    patients.Email, 
                                                                    patients.Telephone, 
                                                                    patients.Dossier, 
                                                                    patients.Commentaire,
                                                                    patients.NumeroAffiliation,
                                                                    mutualites.ID_Mutualite,
                                                                    patients.Photo
                                                                FROM 

                                                                                patients
                                                                                INNER JOIN localites ON patients.Patient_ID_Localite = localites.ID_Localite
                                                                       
                                                                        INNER JOIN mutualites ON patients.Patients_ID_Mutualite = mutualites.ID_Mutualite

                                                                ORDER BY 
                                                                    patients.Nom, 
                                                                    patients.Prenom
                                                                     LIMIT {0}, 1", index);

            if(TablePatients.Values.Count() > 0)
            {
                int testId = (int)TablePatients.GetValue(6);
                int idMut = (int)TablePatients.GetValue(13);
                string filePath = TablePatients.GetValue(14).ToString();
                Image Img = Extensions.GetImageDirectoryPC(filePath, "images/patients/default.png");

                int id = (int)TablePatients.GetValue(0);
                string nom = TablePatients.GetValue(1).ToString();
                string prenom = TablePatients.GetValue(2).ToString();
                string civilitePatient = TablePatients.GetValue(3).ToString();
                DateTime dateNaissancePatient = (DateTime)TablePatients.GetValue(4);
                string adressePatient = TablePatients.GetValue(5).ToString();
                int Patient_ID_Localite = (int)TablePatients.GetValue(6);
                bool vipoPatient = (bool)TablePatients.GetValue(7);
                string emailPatient = TablePatients.GetValue(8).ToString();
                string telephonePatient = TablePatients.GetValue(9).ToString();
                string dossierPatient = TablePatients.GetValue(10).ToString();
                string commentairePatient = TablePatients.GetValue(11).ToString();
                string numeroAffiliationMutuellePatient = TablePatients.GetValue(12).ToString();
                int patients_ID_Mutualite = (int)TablePatients.GetValue(13);
                Image photoPatient = Img;

                Patient = ModelesPatients.CreerPatient(
                    id,
                    nom,
                    prenom,
                    civilitePatient,
                    dateNaissancePatient,
                    adressePatient,
                    Patient_ID_Localite,
                    vipoPatient,
                    emailPatient,
                    telephonePatient,
                    dossierPatient,
                    commentairePatient,
                    numeroAffiliationMutuellePatient,
                    patients_ID_Mutualite,
                    Img
                    );

                int posLocalite = Program.positionItemFromEnumerable(listLocalites, testId);
                int posMutuelle = Program.positionMutFromEnumerable(listMutuelles, idMut);
                if (posLocalite > -1)
                {
                    textBoxCurrentIdPatient.Text = Patient.Id.ToString();
                    textNomModifierPatients.Text = Patient.NomPatient;
                    textPrenomModifierPatients.Text = Patient.PrenomPatient;
                    textCiviliteModifierPatients.Text = Patient.CivilitePatient;
                    dateTimePickerDateNaissanceModifierPatients.Value = Patient.DateNaissancePatient;
                    textAdresseModifierPatients.Text = Patient.AdressePatient;
                    comboBoxCodePostalModifierPatients.SelectedIndex = posLocalite;
                    comboBoxLocaliteModifierPatients.SelectedIndex = posLocalite;
                    textEmailModifierPatients.Text = Patient.EmailPatient;
                    textTelephoneModifierPatients.Text =Patient.TelephonePatient;
                    textDossierModifierPatients.Text = Patient.DossierPatient;
                    checkBoxVipoModifierPatients.Checked = Patient.VipoPatient;
                    listBoxMutuellesModifierPatients.SelectedIndex = posMutuelle;
                    pictureBoxPhotoPatientModifierPatients.Image = Patient.PhotoPatient;
                    textBoxCommentaireModifierPatients.Text = Patient.CommentairePatient;
                    textBoxNumeroAffiliationMutuelleModifierPatients.Text = Patient.NumeroAffiliationMutuellePatient;

                    PatientConfirmation = ModelesPatients.CreerPatient(id, nom, prenom, civilitePatient, dateNaissancePatient, adressePatient, Patient_ID_Localite, vipoPatient, emailPatient, telephonePatient,
                                                                       dossierPatient, commentairePatient, numeroAffiliationMutuellePatient, patients_ID_Mutualite, photoPatient);
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



        private void boutonPremierEnregistrementModifierPatients_Click(object sender, EventArgs e)
        {
            pos = 0;
            if (!ShowData(pos))
            {
                MessageBox.Show("Aucun patient en mémoire", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void boutonEnregistrementSuivantModifierPatients_Click(object sender, EventArgs e)
        {
            pos++;
            if (!ShowData(pos))
            {
                MessageBox.Show("Dernier enregistrement atteint");
                pos--;
            }
        }
        private void boutonEnregistrementPrecedentModifierPatients_Click(object sender, EventArgs e)
        {
            pos--;
            if (!ShowData(pos))
            {
                MessageBox.Show("Premier enregistrement atteint");
                pos++;
            }
        }

        private void boutonDernierEnregistrementModifierPatients_Click(object sender, EventArgs e)
        {
            pos = (int)Program.NbMaxItemsDB("patients") - 1;
            if (pos < 0)
            {
                MessageBox.Show("Aucun patient en mémoire", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                pos = 0;
            }
            else
            {
                ShowData(pos);
            }
        }
        #endregion



        private void textAdresseModifierPatients_Validating(object sender, CancelEventArgs e)
        {
            if (textAdresseModifierPatients.Text == "")
            {
                errorProviderModifierPatients.SetError(textAdresseModifierPatients, null);
            }
            else if (!Patient.ModifierAdressePatient(textAdresseModifierPatients.Text))
            {
                errorProviderModifierPatients.SetError(textAdresseModifierPatients, "L'adresse ne peut pas comporter de caractères spéciaux\nLe numéro du batiment doit être donné");
            }
            else
            {
                errorProviderModifierPatients.SetError(textAdresseModifierPatients, null);
            }
        }

        private void textEmailModifierPatients_Validating(object sender, CancelEventArgs e)
        {
            if (!(textEmailModifierPatients.Text == Patient.EmailPatient))
            {
                if (!Patient.ModifierEmailPatient(textEmailModifierPatients.Text))
                {
                    errorProviderModifierPatients.SetError(textEmailModifierPatients, "Format email invalide");
                }
                else
                {
                    if (Program.UniquenessInDatabase(textEmailModifierPatients.Text, "patients", "Email") ||
                       (Program.UniquenessInDatabase(textEmailModifierPatients.Text, "medecins", "Email") ||
                       (Program.UniquenessInDatabase(textEmailModifierPatients.Text, "mutualites", "Email"))))
                    {
                        errorProviderModifierPatients.SetError(textEmailModifierPatients, "Cette adresse email est déjà référencée\n" +
                                                                                        "Vous devez en saisir une autre");
                    }
                    else
                    {
                        errorProviderModifierPatients.SetError(textEmailModifierPatients, null);
                    }
                }
            }
            else
            {
                errorProviderModifierPatients.SetError(textEmailModifierPatients, null);
            }
        }

        private void textTelephoneModifierPatients_Validating(object sender, CancelEventArgs e)
        {
            if (!(textTelephoneModifierPatients.Text == Patient.TelephonePatient))
            {
                if (textTelephoneModifierPatients.Text == "")
                {
                    errorProviderModifierPatients.SetError(textTelephoneModifierPatients, null);
                }
                else if (!Patient.ModifierTelephonePatient(textTelephoneModifierPatients.Text))
                {
                    errorProviderModifierPatients.SetError(textTelephoneModifierPatients, "Le numéto de téléphone doit commencer par 0032 ou + 32 et\n" +
                                                                                      "Un nombre total de chiffres entre 9 et 12\n" +
                                                                                      "Les caracètres spéciaux sont interdits (sauf le +)");
                }
                else
                {
                    if ((Program.UniquenessInDatabase(textTelephoneModifierPatients.Text, "patients", "Telephone") ||
                       (Program.UniquenessInDatabase(textTelephoneModifierPatients.Text, "medecins", "Telephone") ||
                       (Program.UniquenessInDatabase(textTelephoneModifierPatients.Text, "mutualites", "Telephone")))))
                    {
                        errorProviderModifierPatients.SetError(textTelephoneModifierPatients, "Ce numéro de téléphone est déjà référencé\n" +
                                                                                            "Vous devez en saisir un autre");
                    }
                    else
                    {
                        errorProviderModifierPatients.SetError(textTelephoneModifierPatients, null);
                    }

                }
            }
            else
            {
                errorProviderModifierPatients.SetError(textTelephoneModifierPatients, null);
            }
        }

        private void textDossierModifierPatients_Validating(object sender, CancelEventArgs e)
        {
            if (!(textTelephoneModifierPatients.Text == Patient.TelephonePatient))
            {
                if (textDossierModifierPatients.Text == "")
                {
                    errorProviderModifierPatients.SetError(textDossierModifierPatients, null);
                }
                else if (!Patient.ModifierDossierPatient(textDossierModifierPatients.Text))
                {
                    errorProviderModifierPatients.SetError(textDossierModifierPatients, "Format nom dossier nom valide\n" +
                                                                                        "Format : Nom_Prenom\n");
                }
                else
                {
                    if (Program.UniquenessInDatabase(textDossierModifierPatients.Text, "patients", "Dossier"))
                    {
                        errorProviderModifierPatients.SetError(textTelephoneModifierPatients, "Ce numéro de dossier est déjà référencé\n" +
                                                                                              "Vous devez en saisir un autre");
                    }
                    else
                    {
                        errorProviderModifierPatients.SetError(textDossierModifierPatients, null);
                    }
                }
            }
            else
            {
                errorProviderModifierPatients.SetError(textTelephoneModifierPatients, null);
            }
        }

        private void checkBoxVipoModifierPatients_CheckedChanged(object sender, EventArgs e)
        {
            //Pas d'ErrorProvider à définir ; si coché TRue, sinon false
            Patient.ModifierVipoPatient(checkBoxVipoModifierPatients.Checked);
        }

        private void textBoxCommentaireModifierPatients_TextChanged(object sender, EventArgs e)
        {
            if (!Patient.ModifierCommentairePatient(textBoxCommentaireModifierPatients.Text))
            {
                errorProviderModifierPatients.SetError(textBoxCommentaireModifierPatients, "La limite textuelle est de 100 caractères");
            }
            else
            {
                errorProviderModifierPatients.SetError(textBoxCommentaireModifierPatients, null);
            }
        }

        private void textBoxNumeroAffiliationMutuelleModifierPatients_TextChanged(object sender, EventArgs e)
        {
            if (!(textBoxNumeroAffiliationMutuelleModifierPatients.Text == Patient.NumeroAffiliationMutuellePatient))
            {
                if (!Patient.ModifierNumeroAffiliationMutuellePatient(textBoxNumeroAffiliationMutuelleModifierPatients.Text))
                {
                    string link = String.Format("<a href=https://www.ocm-cdz.be/fr/particuliers/votre-mutualite-et-vous/votre-affiliation#:~:text=%C3%80%20quel%20organisme%20%C3%AAtes-vous,se%20compose%20de%20trois%20chiffres>Pour plus cliquez</a>").ToString();
                    errorProviderModifierPatients.SetError(textBoxNumeroAffiliationMutuelleModifierPatients, "Format d'affuliation invalide\n" +
                                                                                                              link);
                }
                else
                {
                    if(Program.UniquenessInDatabase(textBoxNumeroAffiliationMutuelleModifierPatients.Text, "patients", "NumeroAffiliation"))
                    {
                        errorProviderModifierPatients.SetError(textBoxNumeroAffiliationMutuelleModifierPatients, "Ce numéro d'affiliation est déjà référencé\n" +
                                                                                                                 "Vous devez en saisir un autre");
                    }
                    else
                    {
                        errorProviderModifierPatients.SetError(textBoxNumeroAffiliationMutuelleModifierPatients, null);
                    }
                }
            }
            else
            {
                errorProviderModifierPatients.SetError(textBoxNumeroAffiliationMutuelleModifierPatients, null);
            }

        }


        private void comboBoxCodePostalModifierPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxLocaliteModifierPatients.SelectedIndex = comboBoxCodePostalModifierPatients.SelectedIndex;
        }

        private void comboBoxLocaliteModifierPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCodePostalModifierPatients.SelectedIndex = comboBoxLocaliteModifierPatients.SelectedIndex;
            Patient.ModifierLocalitePatient((comboBoxLocaliteModifierPatients.SelectedItem as FormattedObject<ModelesLocalites.ILocalite>).Object.Id);
        }

        private void listBoxMutuellesModifierPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mutuelle = Program.Mutuelle.ChargerMutuelles(listBoxMutuellesModifierPatients.SelectedIndex);
            Extensions.ResizeImageAccordingToPictureBox(Mutuelle.LogoMutuelle, pictureBoxLogoMutuelleModifierPatients);
            pictureBoxLogoMutuelleModifierPatients.Image = Mutuelle.LogoMutuelle;
            pictureBoxLogoMutuelleModifierPatients.SizeMode = PictureBoxSizeMode.Zoom;
            //Patient.ModifierMutuellePatient(listBoxMutuellesAjouterPatients.SelectedIndex + 1);
            Patient.ModifierMutuellePatient((listBoxMutuellesModifierPatients.SelectedItem as FormattedObject<ModelesMutuelles.IMutuelle>).Object.Id);
        }

        private void textNomModifierPatients_TextChanged(object sender, EventArgs e)
        {
            if (textNomModifierPatients.Text == "")
            {
                errorProviderModifierPatients.SetError(textNomModifierPatients, null);
            }
            else if (!Patient.ModifierNomPatient(textNomModifierPatients.Text))
            {
                errorProviderModifierPatients.SetError(textNomModifierPatients, "Ce nom n'est pas valide\nLes caractères spéciaux ne sont pas acceptés");
            }
            else
            {
                errorProviderModifierPatients.SetError(textNomModifierPatients, null);
            }
        }

        private void textPrenomModifierPatients_TextChanged(object sender, EventArgs e)
        {
            if (textPrenomModifierPatients.Text == "")
            {
                errorProviderModifierPatients.SetError(textPrenomModifierPatients, null);
            }
            else if (!Patient.ModifierPrenomPatient(textPrenomModifierPatients.Text))
            {
                errorProviderModifierPatients.SetError(textPrenomModifierPatients, "Ce prénom n'est pas valide\nLes caractères spéciaux ne sont pas acceptés");
            }
            else
            {
                errorProviderModifierPatients.SetError(textPrenomModifierPatients, null);
            }
        }
    }
}
