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
    public partial class Boite_Modale_Prescriptions : Form
    {
        public Boite_Modale_Prescriptions()
        {
            InitializeComponent();
        }

        private void boutonQuitterModalPrescriptions_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                var bouton = sender as Button;
            }
            this.Hide();
            this.DialogResult = DialogResult.OK;
            Boite_Modale_Generale boite_Modale_Generale = new Boite_Modale_Generale();
            boite_Modale_Generale.ShowDialog();
        }

        private void boutonPrescriptionsEnCoursModalPrescriptions_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Form_Modifier_Prescriptions>(sender, this);
        }

        private void boutonCreerPescriptionModalPrescriptions_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Form_Ajouter_Prescriptions>(sender, this);
        }

        private void boutonConsulterPrescriptionModalPrescriptions_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Form_Modifier_Prescriptions>(sender, this);
        }
    }
}
