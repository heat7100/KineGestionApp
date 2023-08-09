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
    public partial class Boite_Modale_Patients : Form
    {
        public Boite_Modale_Patients()
        {
            InitializeComponent();
        }

        private void boutonQuitterModalPatients_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Generale>(sender, this);
        }

        #region Les 2 boutons directionnels
        private void boutonAjouterModalPatients_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Form_Ajouter_Patients>(sender, this);
        }

        private void boutonModifierModalPatients_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Form_Modifier_Patients>(sender, this);
        }
        #endregion
    }
}
