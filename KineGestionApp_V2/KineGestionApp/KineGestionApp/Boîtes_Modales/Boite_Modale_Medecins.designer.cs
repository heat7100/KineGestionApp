namespace KineGestionApp
{
    partial class Boite_Modale_Medecins
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelModalMedecins = new System.Windows.Forms.TableLayoutPanel();
            this.boutonAjouterModalMedecin = new System.Windows.Forms.Button();
            this.boutonModifierModalMedecins = new System.Windows.Forms.Button();
            this.boutonQuitterModalMedecin = new System.Windows.Forms.Button();
            this.tableLayoutPanelModalMedecins.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelModalMedecins
            // 
            this.tableLayoutPanelModalMedecins.ColumnCount = 2;
            this.tableLayoutPanelModalMedecins.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalMedecins.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalMedecins.Controls.Add(this.boutonAjouterModalMedecin, 0, 1);
            this.tableLayoutPanelModalMedecins.Controls.Add(this.boutonModifierModalMedecins, 1, 1);
            this.tableLayoutPanelModalMedecins.Controls.Add(this.boutonQuitterModalMedecin, 1, 0);
            this.tableLayoutPanelModalMedecins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelModalMedecins.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelModalMedecins.Name = "tableLayoutPanelModalMedecins";
            this.tableLayoutPanelModalMedecins.RowCount = 2;
            this.tableLayoutPanelModalMedecins.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalMedecins.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalMedecins.Size = new System.Drawing.Size(383, 200);
            this.tableLayoutPanelModalMedecins.TabIndex = 0;
            // 
            // boutonAjouterModalMedecin
            // 
            this.boutonAjouterModalMedecin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonAjouterModalMedecin.Location = new System.Drawing.Point(3, 103);
            this.boutonAjouterModalMedecin.Name = "boutonAjouterModalMedecin";
            this.boutonAjouterModalMedecin.Size = new System.Drawing.Size(185, 94);
            this.boutonAjouterModalMedecin.TabIndex = 1;
            this.boutonAjouterModalMedecin.Text = "Ajouter un médecin";
            this.boutonAjouterModalMedecin.UseVisualStyleBackColor = true;
            this.boutonAjouterModalMedecin.Click += new System.EventHandler(this.boutonAjouterModalMedecin_Click);
            // 
            // boutonModifierModalMedecins
            // 
            this.boutonModifierModalMedecins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonModifierModalMedecins.Location = new System.Drawing.Point(194, 103);
            this.boutonModifierModalMedecins.Name = "boutonModifierModalMedecins";
            this.boutonModifierModalMedecins.Size = new System.Drawing.Size(186, 94);
            this.boutonModifierModalMedecins.TabIndex = 2;
            this.boutonModifierModalMedecins.Text = "Modifier un médecin";
            this.boutonModifierModalMedecins.UseVisualStyleBackColor = true;
            this.boutonModifierModalMedecins.Click += new System.EventHandler(this.boutonModifierModalMedecins_Click);
            // 
            // boutonQuitterModalMedecin
            // 
            this.boutonQuitterModalMedecin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonQuitterModalMedecin.Location = new System.Drawing.Point(194, 3);
            this.boutonQuitterModalMedecin.Name = "boutonQuitterModalMedecin";
            this.boutonQuitterModalMedecin.Size = new System.Drawing.Size(186, 94);
            this.boutonQuitterModalMedecin.TabIndex = 0;
            this.boutonQuitterModalMedecin.Text = "Revenir au menu général";
            this.boutonQuitterModalMedecin.UseVisualStyleBackColor = true;
            this.boutonQuitterModalMedecin.Click += new System.EventHandler(this.boutonQuitterModalMedecin_Click);
            // 
            // Boite_Modale_Medecins
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 200);
            this.Controls.Add(this.tableLayoutPanelModalMedecins);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Boite_Modale_Medecins";
            this.Text = "Boite_Dialogue_Medecins";
            this.tableLayoutPanelModalMedecins.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelModalMedecins;
        private System.Windows.Forms.Button boutonAjouterModalMedecin;
        private System.Windows.Forms.Button boutonModifierModalMedecins;
        private System.Windows.Forms.Button boutonQuitterModalMedecin;
    }
}