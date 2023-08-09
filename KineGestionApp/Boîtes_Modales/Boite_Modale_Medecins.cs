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
    public partial class Boite_Modale_Medecins : Form
    {
        public Boite_Modale_Medecins()
        {
            InitializeComponent();
        }

        private void boutonQuitterModalMedecin_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Generale>(sender, this);
        }

        private void boutonAjouterModalMedecin_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Form_Ajouter_Medecins>(sender, this);
        }

        private void boutonModifierModalMedecins_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Form_Modifier_Medecins>(sender, this);
        }
    }
}
