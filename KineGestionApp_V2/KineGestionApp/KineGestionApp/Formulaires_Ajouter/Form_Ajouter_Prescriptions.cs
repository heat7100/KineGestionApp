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

namespace KineGestionApp
{
    public partial class Form_Ajouter_Prescriptions : Form
    {
        private IEnumerable<ModelesPatients.IPatient> listPatients = Program.Patient.EnumererPatients();
        private IEnumerable<ModelesMedecins.IMedecin> listMedecins = Program.Medecin.EnumererMedecins();

        public Form_Ajouter_Prescriptions()
        {
            InitializeComponent();

            #region Gestion de la récupération des patients

            listBoxPatientAjouterPrescriptions.Items.Clear();
            foreach (var patient in listPatients
                .Select(patient => new FormattedObject<ModelesPatients.IPatient>(patient, p => p.NomPatient + " " + p.PrenomPatient)))
            {
                listBoxPatientAjouterPrescriptions.Items.Add(patient);
            }
            #endregion

            #region Gestion de la récupération des médecins

            listBoxMedecinsAjouterPrescriptions.Items.Clear();
            foreach (var medecin in Program.Medecin
                .EnumererMedecins()
                .Select(medecin => new FormattedObject<ModelesMedecins.IMedecin>(medecin, m => m.NomMedecin + " " + m.PrenomMedecin)))
            {
                listBoxMedecinsAjouterPrescriptions.Items.Add(medecin);
            }
            #endregion

            #region Gestion de la récupération des codes de nomenclature

            comboBoxNomenclaturesAjouterPrescriptions.Items.Clear();
            foreach (var code in Program.Nomenclature
                .EnumererNomenclatures()
                .Select(code => new FormattedObject<ModelesNomenclatures.INomenclature>(code, c => c.Code)))
            {
                comboBoxNomenclaturesAjouterPrescriptions.Items.Add(code);
            }
            #endregion
        }

        private void boutonNumeriserPrescriptionAjouterPrescriptions_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Ouvre l'Explorateur de fichiers Windows pour insérer la photo du patient
                pictureBoxPhotoPrescriptionAjouterPrescriptions.Load(openFileDialog1.FileName);
            }
        }

        private void boutonSupprimerPhotoPrescriptionAjouterPrescriptions_Click(object sender, EventArgs e)
        {
            pictureBoxPhotoPrescriptionAjouterPrescriptions.Image = null;
        }

        private void boutonSauvegarderEnregistrementAjouterPrescription_Click(object sender, EventArgs e)
        {
            string message = "Confirmez vous la sauvegarde de la precription du patient : " + listBoxPatientAjouterPrescriptions.Text + "?";
            const string caption = "Confirmation sauvegarde de prescription";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // S'il sélectionne le bouton Yes
            if (result == DialogResult.No)
            {
               
            }//On supprimer tout ce qui a été sélectionné dans le formulaire
            else
            {
                
            }
        }

        private void boutonAnnulerEnregistrementAjouterPrescription_Click(object sender, EventArgs e)
        {
            Extensions.ClearFormControls(this);
        }

        private void boutonQuitterAjouterPrescription_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Prescriptions>(sender, this);
        }

       DataTable ListeSeances = new DataTable();

        private void boutonAjouterEnregistrementAjouterPrescription_Click(object sender, EventArgs e)
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

        private void listBoxPatientAjouterPrescriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedPatientID = Program.idItemFromEnumerablePatients(listPatients, listBoxPatientAjouterPrescriptions.SelectedIndex);
            ModelesPatients.IPatient SelectedPatient = Program.Patient.ChargerPatients(selectedPatientID);
            ModelesLocalites.ILocalite LocPatient = Program.Localite.ChargerLocalites(SelectedPatient.Patient_ID_Localite);
            ModelesMutuelles.IMutuelle MutPatient = Program.Mutuelle.ChargerMutuelles(SelectedPatient.Patients_ID_Mutualite);

            textBoxAdressePatientAjouterPrescriptions.Text = SelectedPatient.AdressePatient;
            textBoxCodePostalAjouterPrescriptions.Text = LocPatient.CodePostal;
            textBoxLocalitePatientAjouterPrescriptions.Text = LocPatient.NomLocalite;
            checkBoxVipoAjouterPrescriptions.Checked = SelectedPatient.VipoPatient;

            pictureBoxPhotoPatientAjouterPrescriptions.Image = SelectedPatient.PhotoPatient;
            pictureBoxLogoMutuelleAjouterPrescriptions.Image = MutPatient.LogoMutuelle;
        }

        private void listBoxMedecinsAjouterPrescriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedMedecinID = Program.idItemFromEnumerableMedecins(listMedecins, listBoxMedecinsAjouterPrescriptions.SelectedIndex);
            ModelesMedecins.IMedecin SelectedMedecin = Program.Medecin.ChargerMedecins(selectedMedecinID);
            ModelesLocalites.ILocalite LocMedecin = Program.Localite.ChargerLocalites(SelectedMedecin.Medecin_ID_Localite);

            textBoxAdresseMedecinAjouterPrescriptions.Text = SelectedMedecin.AdresseMedecin;
            textBoxCodePostalMedecinAjouterPrescriptions.Text = LocMedecin.CodePostal;
            textBoxLocaliteMedecinAjouterPrescriptions.Text = LocMedecin.NomLocalite;
        }
    }
}
