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
    public partial class Boite_Modale_Mutuelles : Form
    {
        public Boite_Modale_Mutuelles()
        {
            InitializeComponent();
        }

        private void buttonQuitterModalMutuelles_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Boite_Modale_Generale>(sender, this);
        }

        private void buttonAjouterModalMutuelles_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Form_Ajouter_Mutuelles>(sender, this);
        }

        private void buttonModifierModalMutuelles_Click(object sender, EventArgs e)
        {
            Extensions.OpenAndCloseForm<Form_Modifier_Mutuelles>(sender, this);
        }
    }
}
