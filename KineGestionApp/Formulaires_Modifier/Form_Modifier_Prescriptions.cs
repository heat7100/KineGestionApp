using PDSGBD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KineGestionApp
{
    public partial class Form_Modifier_Prescriptions : Form
    {
        public Form_Modifier_Prescriptions()
        {
            InitializeComponent();
        }

        private void boutonNumeriserPrescriptionAjouterPrescriptions_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Ouvre l'Explorateur de fichiers Windows pour insérer la photo du patient
                pictureBoxPhotoPrescriptionModifierPrescriptions.Load(openFileDialog1.FileName);
            }
        }

        private void boutonSupprimerPhotoPrescriptionAjouterPrescriptions_Click(object sender, EventArgs e)
        {
            pictureBoxPhotoPrescriptionModifierPrescriptions.Image = null;
        }


        private void buttonQuitterModifierPrescriptions_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Prescriptions>(sender, this);
        }

        private void boutonSauvegarderModifierEnregistrement_Click(object sender, EventArgs e)
        {
            string message = "Confirmez vous la modification de la prescription numéro : " + textBoxNumeroPrescriptionModifierPrescription.Text + " ?/n" +
                             "Vérifiez bien les champs saisis du formulaire";
            const string caption = "Confirmation modification prescription";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // S'il sélectionne le bouton Yes
            if (result == DialogResult.Yes)
            {
                //TODO : appel de la méthode avec la réquête SQL depuis fichier gestion de prescription

            }//On supprimer tout ce qui a été sélectionné dans le formulaire
            else
            {

            }
        }

        private void Form_Modifier_Prescriptions_Load(object sender, EventArgs e)
        {
            // TODO: cette ligne de code charge les données dans la table '_Kiné_Sam_2491_2___Copie_pour_TFE_Sauvegarde_SauvegardeDataSet.Seances'. Vous pouvez la déplacer ou la supprimer selon les besoins.
            //this.seancesTableAdapter.Fill(this._Kiné_Sam_2491_2___Copie_pour_TFE_Sauvegarde_SauvegardeDataSet.Seances);

        }



        int pos;
        //public DataTable ShowData(int index)
        //{
        //    textBoxCurrentIdPrescription.Text = index.ToString();
        //    DataTable TableSeances = DBM.Access_Data_Navigation_Load(@"SELECT Seances.Date, Seances.Numero_seance, Seances.Prix_seance, Seances.Payé, Medecins.Nom, Patients.Nom, Patients.Prenom
        //                                                                FROM (Patients INNER JOIN (Medecins INNER JOIN Prescriptions ON Medecins.ID_Medecin = Prescriptions.Prescriptions_ID_Medecin) ON Patients.ID_Patients = Prescriptions.Prescriptions_ID_Patients) INNER JOIN Seances ON Prescriptions.ID_Prescriptions = Seances.Seances_ID_Prescriptions
        //                                                                GROUP BY Prescriptions.ID_Prescriptions, Patients.ID_Patients, Medecins.ID_Medecin, Seances.Date, Seances.Numero_seance, Seances.Prix_seance, Seances.Payé, Medecins.Nom, Patients.Nom, Patients.Prenom
        //                                                                HAVING (((Prescriptions.ID_Prescriptions)=2));", this.textBoxCurrentIdPrescription.Text);
        //    dataGridViewListeSeancesModifierPrescriptions.DataSource = TableSeances;

        //    DataTable TablePrescriptions = DBM.Access_Data_Navigation_Load(@"SELECT Prescriptions.ID_Prescriptions, Patients.Nom, Patient.Prenom, Patients.Vipo, Medecins.Nom, Medecins.Prenom, Medecins.Numero_INAMI, Prescriptions.Numero, Nomenclatures.Code, Prescriptions.[Nombre_de-seances], Prescriptions.Date_de_prescription, Prescriptions.Acompte, Prescriptions.Cloturée
        //                                                                    FROM Nomenclatures INNER JOIN (Patients INNER JOIN (Medecins INNER JOIN Prescriptions ON Medecins.ID_Medecin = Prescriptions.Prescriptions_ID_Medecin) ON Patients.ID_Patients = Prescriptions.Prescriptions_ID_Patients) ON Nomenclatures.ID_Nomenclatures = Prescriptions.Prescriptions_ID_Nomenclatures
        //                                                                    WHERE (((Prescriptions.ID_Prescriptions)={0}));", textBoxCurrentIdPrescription.Text);
        //    TO DO : Finding the way to display Code postal and Localite of both(Medecin / Patient)

        //    textBoxNomPatientModifierPrescriptions.Text = TablePrescriptions.Rows[index][1].ToString() + " " + TablePrescriptions.Rows[index][2].ToString();
        //    checkBoxVipoModifierPrescriptions.Checked = (int)TablePrescriptions.Rows[index][3] == -1 ? true : false;
        //    textBoxMedecinModifierPrescriptions.Text = TablePrescriptions.Rows[index][4].ToString() + " " + TablePrescriptions.Rows[index][4].ToString();
        //    textBoxNumeroINAMIMedecinModifierPrescriptions.Text = TablePrescriptions.Rows[index][5].ToString();
        //    textBoxNumeroPrescriptionModifierPrescription.Text = TablePrescriptions.Rows[index][6].ToString();
        //    textBoxNomenclatureModifierPrescriptions.Text = TablePrescriptions.Rows[index][7].ToString();
        //    textBoxAcompteModifierPrescriptions.Text = TablePrescriptions.Rows[index][8].ToString();
        //    checkBoxClotureeModifierPrescriptions.Checked = (int)TablePrescriptions.Rows[index][3] == -1 ? true : false;

        //    return TablePrescriptions;

        //}

        private void boutonPremierEnregistrementAjouterPrescription_Click(object sender, EventArgs e)
        {
            pos = 0;
            //ShowData(pos);
        }

        private void boutonPrecedentEnregistrementModifierPrescription_Click(object sender, EventArgs e)
        {
            pos--;
            if (pos >= 0)
            {
                //ShowData(pos);
            }
            else
            {
                MessageBox.Show("Dernier enregistrement");
            }
        }

        private void boutonSuivantEnregistrementModifierPrescription_Click(object sender, EventArgs e)
        {
            pos++;
            //DataTable TablePrescriptions = ShowData(pos);
            //if (pos < TablePrescriptions.Rows.Count)
            //{
            //    //ShowData(pos);
            //}
            //else
            //{
            //    MessageBox.Show("Dernier enregistrement");
            //    pos = TablePrescriptions.Rows.Count - 1;
            //}
        }

        private void boutonDernierEnregistrementModifierPrescription_Click(object sender, EventArgs e)
        {

        }

        private void boutonCloturerModifierPrescriptions_Click(object sender, EventArgs e)
        {
           if(this.checkBoxClotureeModifierPrescriptions.Checked == false) 
            {
                errorProviderModifierPrescriptions.SetError(checkBoxClotureeModifierPrescriptions, "Veuillez cocher la case \"Cloturée\"");
            }
            Extensions.OpenAndCloseForm<Boite_Modale_Prescriptions>(sender, this);
            this.Close();
        }
    }
}

