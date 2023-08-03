using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using PDSGBD;
using PDSGBD_MySql;
using MimeKit;
using System.IO;

namespace KineGestionApp
{
    public partial class Form_Modifier_Mutuelles : Form
    {
        private ModelesMutuelles.IMutuelle Mutuelle { get; set; }
        private ModelesMutuelles.IMutuelle MutuelleConfirmation { get; set; }
        private IEnumerable<ModelesLocalites.ILocalite> listLocalite = Program.Localite.EnumererLocalites();

        int pos;
        public Form_Modifier_Mutuelles()
        {
            InitializeComponent();

            #region Gestion de la récupération de l'ID de la localité

            comboBoxCodePostalModifierMutuelles.Items.Clear();
            foreach (var loc in listLocalite
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.CodePostal)))
            {
                comboBoxCodePostalModifierMutuelles.Items.Add(loc);
            }
            comboBoxLocaliteModifierMutuelles.Items.Clear();
            foreach (var loc in listLocalite
                .Select(localite => new FormattedObject<ModelesLocalites.ILocalite>(localite, e => e.NomLocalite)))
            {
                comboBoxLocaliteModifierMutuelles.Items.Add(loc);
            }
            #endregion

            ShowData(pos);
        }

        private void boutonModifierPhotoModifierMutuelles_Click(object sender, EventArgs e)
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
                            //Ouvre l'Explorateur de fichiers Windows pour insérer la photo du patient
                            pictureBoxModifierMutuelles.Load(openFileDialog1.FileName);
                            if (!Mutuelle.ModifierLogoMutuelle(pictureBoxModifierMutuelles.Image))
                            {
                                pictureBoxModifierMutuelles.Invalidate();
                                errorProviderModifierMutuelles.SetError(boutonModifierPhotoAjouterMutuelles, "Format image invalide\n" +
                                                                                                         "Formats autorisés : png, jpeg");
                            }
                            else
                            {
                                errorProviderModifierMutuelles.SetError(boutonModifierPhotoAjouterMutuelles, null);
                            }

                        }
                    }
                    catch (Exception error)
                    {
                        errorProviderModifierMutuelles.SetError(boutonModifierPhotoAjouterMutuelles, error.Message + "\nVeuillez recommencer l'upload");
                    }
                }
                else
                {
                    errorProviderModifierMutuelles.SetError(boutonModifierPhotoAjouterMutuelles, "Il ne s'agit pas d'une image valide");
                }
            }
        }

        private void boutonSupprimerPhotoModifierMutuelles_Click(object sender, EventArgs e)
        {
            //Supprimer la photo ajoutée précédemment
            pictureBoxModifierMutuelles.Image = null;
            Mutuelle.ModifierLogoMutuelle(pictureBoxModifierMutuelles.Image);
        }

        private void boutonSauvegarderModificationMutuelles_Click(object sender, EventArgs e)
        {
            int currentIDMutuelle = (int.Parse(textBoxCurrentIdMutuelle.Text));
            if (Mutuelle.EstValide() && (Program.ExistenceTestID(currentIDMutuelle, "mutualites", "ID_Mutualite")))
            {
                string message = "Confirmez vous la modification de la mutuelle : /n" + textBoxMutuelleModifierMutuelles.Text + " ?/n" +
                                 "Voici les modifications : \n"
                                 + MessageBoxConfirmationModif();
                const string caption = "Confirmation modification mutuelle";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                // S'il sélectionne le bouton Yes
                if (result == DialogResult.Yes)
                {
                    if (!Program.Mutuelle.MettreAJour(Mutuelle))
                    {
                        MessageBox.Show("Echec de l'ajout\n" +
                                        "Veuillez prendre contact avec votre Provider : \n" +
                                        "Samuel Raes : +32473/934591", "Error Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Extensions.OpenAndCloseForm<Boite_Modale_Mutuelles>(sender, this);
                    }
                    else
                    {
                        MessageBox.Show("Succès de la modification",
                                        "L'enregistrement a bien été traité", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ShowData(pos);
                    }

                }//On supprimer tout ce qui a été sélectionné dans le formulaire
                else
                {
                    Extensions.ClearFormControls(this);
                }
            }
            else
            {
                string message = "Les données saisies pour l'enregistrement de la mutuelle : \n" + textBoxMutuelleModifierMutuelles.Text + " " + "sont érronées \n" +
                            "Souhaitez-vous recommencer l'enregistrement ?";
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
                    Extensions.OpenAndCloseForm<Boite_Modale_Mutuelles>(sender, this);
                }
            }

        }

        private StringBuilder MessageBoxConfirmationModif()
        {
            StringBuilder message = new StringBuilder();
            if (Mutuelle.NomMutuelle != MutuelleConfirmation.NomMutuelle)
            {
                message.Append("Nom : " + MutuelleConfirmation.NomMutuelle + " => " + Mutuelle.NomMutuelle + "\n");
            }
            if (Mutuelle.AdresseMutuelle != MutuelleConfirmation.AdresseMutuelle)
            {
                message.Append("Adresse : " + MutuelleConfirmation.AdresseMutuelle + " => " + Mutuelle.AdresseMutuelle + "\n");
            }
            if (Mutuelle.TelephoneMutuelle != MutuelleConfirmation.TelephoneMutuelle)
            {
                message.Append("Téléphone : " + MutuelleConfirmation.TelephoneMutuelle + " => " + Mutuelle.TelephoneMutuelle + "\n");
            }
            if (Mutuelle.EmailMutuelle != MutuelleConfirmation.EmailMutuelle)
            {
                message.Append("Email : " + MutuelleConfirmation.EmailMutuelle + " => " + Mutuelle.EmailMutuelle + "\n");
            }
            if (Mutuelle.Mutuelle_ID_Localite != MutuelleConfirmation.Mutuelle_ID_Localite)
            {
                string BeforeLocalite = Program.ItemFromEnumerable(listLocalite, MutuelleConfirmation.Mutuelle_ID_Localite).NomLocalite;
                string AfterLocalite = Program.ItemFromEnumerable(listLocalite, Mutuelle.Mutuelle_ID_Localite).NomLocalite;

                message.Append("Localité : " + BeforeLocalite + " => " + AfterLocalite + "\n");
            }
            if(Mutuelle.LogoMutuelle != MutuelleConfirmation.LogoMutuelle)
            {
                message.Append("Le logo a été modifié");
            }

            return message;
        }

        private void boutonQuitterModificationMutuelles_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Mutuelles>(sender, this);
        }

        #region Navigation dans le formulaire

        private bool ShowData(int index)
        {
            DBM.IRow TableMutuelles = Program.Bd.GetRow(@"SELECT mutualites.ID_Mutualite, 
                                                          mutualites.Mutualite, 
                                                          mutualites.Adresse, 
                                                          mutualites.Email, 
                                                          mutualites.Telephone, 
                                                          localites.ID_Localite, 
                                                          mutualites.Logo, 
                                                          localites.Code_postal, 
                                                          localites.Localite 
                                                          FROM localites INNER JOIN mutualites ON localites.ID_Localite = mutualites.Mutualite_ID_Localite  
                                                          ORDER BY mutualites.Mutualite LIMIT {0}, 1", index);

            if (TableMutuelles.Values.Count() > 0)
            {
                int testId = (int)TableMutuelles.GetValue(5);
                string filePath = TableMutuelles.GetValue(6).ToString();
                Image Img = Extensions.GetImageDirectoryPC(filePath);
                Mutuelle = ModelesMutuelles.CreerMutuelle(
                    (int)TableMutuelles.GetValue(0),
                    TableMutuelles.GetValue(1).ToString(),
                    TableMutuelles.GetValue(2).ToString(),
                    TableMutuelles.GetValue(3).ToString(),
                    TableMutuelles.GetValue(4).ToString(),
                    (int)TableMutuelles.GetValue(5),
                    Img
                    );


                int posLocalite = Program.positionItemFromEnumerable(listLocalite, testId);
                textBoxCurrentIdMutuelle.Text = Mutuelle.Id.ToString();
                textBoxMutuelleModifierMutuelles.Text = Mutuelle.NomMutuelle;
                textBoxAdresseModifierMutuelles.Text = Mutuelle.AdresseMutuelle;
                textBoxEmailModifierMutuelles.Text = Mutuelle.EmailMutuelle;
                textBoxTelephoneModifierMutuelles.Text = Mutuelle.TelephoneMutuelle;
                comboBoxLocaliteModifierMutuelles.SelectedIndex = posLocalite;
                comboBoxCodePostalModifierMutuelles.SelectedIndex = posLocalite;
                pictureBoxModifierMutuelles.Image = Mutuelle.LogoMutuelle;

                MutuelleConfirmation = ModelesMutuelles.CreerMutuelle(Mutuelle.Id, Mutuelle.NomMutuelle, Mutuelle.AdresseMutuelle, Mutuelle.EmailMutuelle, Mutuelle.TelephoneMutuelle, Mutuelle.Mutuelle_ID_Localite, Mutuelle.LogoMutuelle);
                return true;
            }
            else
            {
                return false;
            }
        }


        private void boutonPremierEnregistrementModifierMutuelles_Click(object sender, EventArgs e)
        {
            pos = 0;
            if (!ShowData(pos))
            {
                MessageBox.Show("Aucune mutuelle en mémoire", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void boutonEnregistrementSuivantModifierMutuelles_Click(object sender, EventArgs e)
        {
            pos++;
            if (!ShowData(pos))
            {
                MessageBox.Show("Dernier enregistrement atteint");
                pos--;
            }
        }
        private void boutonEnregistrementPrecedentModifierMutuelles_Click(object sender, EventArgs e)
        {
            pos--;
            if (!ShowData(pos))
            {
                MessageBox.Show("Premier enregistrement atteint");
                pos++;
            }
        }

        private void boutonDernierEnregistrementModifierMutuelles_Click(object sender, EventArgs e)
        {
            pos = (int)Program.NbMaxItemsDB("mutualites") - 1;
            if (pos < 0)
            {
                MessageBox.Show("Aucune mutuelle en mémoire", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                pos = 0;
            }
            else
            {
                ShowData(pos);
            }
        }
        #endregion

        private void textBoxMutuelleModifierMutuelles_TextChanged(object sender, EventArgs e)
        {
            if(textBoxMutuelleModifierMutuelles.Text == Mutuelle.NomMutuelle)
            {
                errorProviderModifierMutuelles.SetError(textBoxMutuelleModifierMutuelles, null);
            }
            else
            {
                if (textBoxMutuelleModifierMutuelles.Text == "")
                {
                    errorProviderModifierMutuelles.SetError(textBoxMutuelleModifierMutuelles, null);
                }
                else if (!Mutuelle.ModifierNomMutuelle(textBoxMutuelleModifierMutuelles.Text))
                {
                    errorProviderModifierMutuelles.SetError(textBoxMutuelleModifierMutuelles, "Ce nom n'est pas valide\nLes caractères spéciaux ne sont pas acceptés");
                }

                else
                {
                    if (Program.UniquenessInDatabase(textBoxMutuelleModifierMutuelles.Text, "mutualites", "Mutualite"))
                    {
                        errorProviderModifierMutuelles.SetError(textBoxMutuelleModifierMutuelles, "Ce nom de mutuelle est déjà référencé\n" +
                                                                                              "Vous devez saisir un autre nom");
                    }
                    else
                    {
                        errorProviderModifierMutuelles.SetError(textBoxMutuelleModifierMutuelles, null);
                    }
                }
            }
        }

        private void textBoxAdresseModifierMutuelles_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxAdresseModifierMutuelles.Text == "")
            {
                errorProviderModifierMutuelles.SetError(textBoxAdresseModifierMutuelles, null);
            }
            else if (!Mutuelle.ModifierAdresseMutuelle(textBoxAdresseModifierMutuelles.Text))
            {
                errorProviderModifierMutuelles.SetError(textBoxAdresseModifierMutuelles, "L'adresse ne peut pas comporter de caractères spéciaux\nLe numéro du batiment doit être donné");
            }
            else
            {
                errorProviderModifierMutuelles.SetError(textBoxAdresseModifierMutuelles, null);
            }
        }

        private void textBoxTelephoneModifierMutuelles_Validating(object sender, CancelEventArgs e)
        {
            if (!(textBoxTelephoneModifierMutuelles.Text == Mutuelle.TelephoneMutuelle))
            {
                if(textBoxTelephoneModifierMutuelles.Text == "")
                {
                    errorProviderModifierMutuelles.SetError(textBoxTelephoneModifierMutuelles, null);
                }
                else if (!Mutuelle.ModifierTelephoneMutuelle(textBoxTelephoneModifierMutuelles.Text))
                {
                    errorProviderModifierMutuelles.SetError(textBoxTelephoneModifierMutuelles, "Le numéto de téléphone doit commencer par 0032 ou + 32 et\n" +
                                                                                      "Un nombre total de chiffres entre 9 et 12\n" +
                                                                                      "Les caracètres spéciaux sont interdits (sauf le +)");
                }
                else
                {
                    if ((Program.UniquenessInDatabase(textBoxTelephoneModifierMutuelles.Text, "patients", "Telephone") ||
                        (Program.UniquenessInDatabase(textBoxTelephoneModifierMutuelles.Text, "medecins", "Telephone") ||
                        (Program.UniquenessInDatabase(textBoxTelephoneModifierMutuelles.Text, "mutualites", "Telephone")))))
                    {
                        errorProviderModifierMutuelles.SetError(textBoxTelephoneModifierMutuelles, "Ce numéro de téléphone est déjà référencé\n" +
                                                                                                    "Vous devez en saisir un autre");
                    }
                    else
                    {
                        errorProviderModifierMutuelles.SetError(textBoxTelephoneModifierMutuelles, null);
                    }
                    
                }
            }
            else
            {
                errorProviderModifierMutuelles.SetError(textBoxTelephoneModifierMutuelles, null);
            }
        }

        private void textBoxEmailModifierMutuelles_Validating(object sender, CancelEventArgs e)
        {
            if (!(textBoxEmailModifierMutuelles.Text == Mutuelle.EmailMutuelle))
            {
                if (!Mutuelle.ModifierEmailMutuelle(textBoxEmailModifierMutuelles.Text))
                {
                    errorProviderModifierMutuelles.SetError(textBoxEmailModifierMutuelles, "Format email invalide");
                }
                else
                {
                    if (Program.UniquenessInDatabase(textBoxEmailModifierMutuelles.Text, "patients", "Email") ||
                       (Program.UniquenessInDatabase(textBoxEmailModifierMutuelles.Text, "medecins", "Email") ||
                       (Program.UniquenessInDatabase(textBoxEmailModifierMutuelles.Text, "mutualites", "Email"))))
                    {
                        errorProviderModifierMutuelles.SetError(textBoxEmailModifierMutuelles, "Cette adresse email est déjà référencée\n" +
                                                                                        "Vous devez en saisir une autre");
                    }
                    errorProviderModifierMutuelles.SetError(textBoxEmailModifierMutuelles, null);
                }
            }
            else
            {
                errorProviderModifierMutuelles.SetError(textBoxEmailModifierMutuelles, null);
            }
        }

        private void comboBoxCodePostalModifierMutuelles_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxLocaliteModifierMutuelles.SelectedIndex = comboBoxCodePostalModifierMutuelles.SelectedIndex;
        }

        private void comboBoxLocaliteModifierMutuelles_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCodePostalModifierMutuelles.SelectedIndex = comboBoxLocaliteModifierMutuelles.SelectedIndex;
        }
    }
}
