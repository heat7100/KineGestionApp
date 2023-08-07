namespace KineGestionApp
{
    partial class Boite_Modale_Generale
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanelModalGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.boutonModalPatients = new System.Windows.Forms.Button();
            this.boutonModalMedecins = new System.Windows.Forms.Button();
            this.boutonModalMutuelles = new System.Windows.Forms.Button();
            this.boutonModalPrescriptions = new System.Windows.Forms.Button();
            this.boutonQuitterModalGeneral = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.zoneTexteMotDePasse = new System.Windows.Forms.TextBox();
            this.errorProviderMenuGeneral = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanelModalGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderMenuGeneral)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelModalGeneral
            // 
            this.tableLayoutPanelModalGeneral.ColumnCount = 3;
            this.tableLayoutPanelModalGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelModalGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelModalGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelModalGeneral.Controls.Add(this.boutonModalPatients, 1, 2);
            this.tableLayoutPanelModalGeneral.Controls.Add(this.boutonModalMedecins, 0, 3);
            this.tableLayoutPanelModalGeneral.Controls.Add(this.boutonModalMutuelles, 0, 3);
            this.tableLayoutPanelModalGeneral.Controls.Add(this.boutonModalPrescriptions, 0, 2);
            this.tableLayoutPanelModalGeneral.Controls.Add(this.boutonQuitterModalGeneral, 2, 0);
            this.tableLayoutPanelModalGeneral.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelModalGeneral.Controls.Add(this.zoneTexteMotDePasse, 0, 1);
            this.tableLayoutPanelModalGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelModalGeneral.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelModalGeneral.Name = "tableLayoutPanelModalGeneral";
            this.tableLayoutPanelModalGeneral.RowCount = 4;
            this.tableLayoutPanelModalGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelModalGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelModalGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelModalGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelModalGeneral.Size = new System.Drawing.Size(546, 254);
            this.tableLayoutPanelModalGeneral.TabIndex = 0;
            // 
            // boutonModalPatients
            // 
            this.boutonModalPatients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonModalPatients.Location = new System.Drawing.Point(236, 86);
            this.boutonModalPatients.Name = "boutonModalPatients";
            this.boutonModalPatients.Size = new System.Drawing.Size(220, 78);
            this.boutonModalPatients.TabIndex = 2;
            this.boutonModalPatients.Text = "Patients";
            this.boutonModalPatients.UseVisualStyleBackColor = true;
            this.boutonModalPatients.Click += new System.EventHandler(this.boutonModalPatients_Click);
            // 
            // boutonModalMedecins
            // 
            this.boutonModalMedecins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonModalMedecins.Location = new System.Drawing.Point(236, 170);
            this.boutonModalMedecins.Name = "boutonModalMedecins";
            this.boutonModalMedecins.Size = new System.Drawing.Size(220, 81);
            this.boutonModalMedecins.TabIndex = 4;
            this.boutonModalMedecins.Text = "Médecins";
            this.boutonModalMedecins.UseVisualStyleBackColor = true;
            this.boutonModalMedecins.Click += new System.EventHandler(this.boutonModalMedecins_Click);
            // 
            // boutonModalMutuelles
            // 
            this.boutonModalMutuelles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonModalMutuelles.Location = new System.Drawing.Point(3, 170);
            this.boutonModalMutuelles.Name = "boutonModalMutuelles";
            this.boutonModalMutuelles.Size = new System.Drawing.Size(227, 81);
            this.boutonModalMutuelles.TabIndex = 3;
            this.boutonModalMutuelles.Text = "Mutuelles";
            this.boutonModalMutuelles.UseVisualStyleBackColor = true;
            this.boutonModalMutuelles.Click += new System.EventHandler(this.boutonModalMutuelles_Click);
            // 
            // boutonModalPrescriptions
            // 
            this.boutonModalPrescriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonModalPrescriptions.Location = new System.Drawing.Point(3, 86);
            this.boutonModalPrescriptions.Name = "boutonModalPrescriptions";
            this.boutonModalPrescriptions.Size = new System.Drawing.Size(227, 78);
            this.boutonModalPrescriptions.TabIndex = 1;
            this.boutonModalPrescriptions.Text = "Prescriptions";
            this.boutonModalPrescriptions.UseVisualStyleBackColor = true;
            this.boutonModalPrescriptions.Click += new System.EventHandler(this.boutonModalPrescriptions_Click);
            // 
            // boutonQuitterModalGeneral
            // 
            this.boutonQuitterModalGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonQuitterModalGeneral.Location = new System.Drawing.Point(462, 3);
            this.boutonQuitterModalGeneral.Name = "boutonQuitterModalGeneral";
            this.boutonQuitterModalGeneral.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.boutonQuitterModalGeneral.Size = new System.Drawing.Size(81, 49);
            this.boutonQuitterModalGeneral.TabIndex = 5;
            this.boutonQuitterModalGeneral.Text = "Quitter";
            this.boutonQuitterModalGeneral.UseVisualStyleBackColor = true;
            this.boutonQuitterModalGeneral.Click += new System.EventHandler(this.boutonQuitterModalGeneral_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mot de passe d\'administration";
            // 
            // zoneTexteMotDePasse
            // 
            this.zoneTexteMotDePasse.Location = new System.Drawing.Point(3, 58);
            this.zoneTexteMotDePasse.Name = "zoneTexteMotDePasse";
            this.zoneTexteMotDePasse.PasswordChar = '*';
            this.zoneTexteMotDePasse.Size = new System.Drawing.Size(227, 22);
            this.zoneTexteMotDePasse.TabIndex = 0;
            this.zoneTexteMotDePasse.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SurValidationMotDePasse);
            // 
            // errorProviderMenuGeneral
            // 
            this.errorProviderMenuGeneral.ContainerControl = this;
            // 
            // Boite_Modale_Generale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 254);
            this.Controls.Add(this.tableLayoutPanelModalGeneral);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Boite_Modale_Generale";
            this.Text = "Menu général";
            this.tableLayoutPanelModalGeneral.ResumeLayout(false);
            this.tableLayoutPanelModalGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderMenuGeneral)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelModalGeneral;
        private System.Windows.Forms.Button boutonModalPrescriptions;
        private System.Windows.Forms.Button boutonQuitterModalGeneral;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button boutonModalMedecins;
        private System.Windows.Forms.Button boutonModalMutuelles;
        private System.Windows.Forms.Button boutonModalPatients;
        private System.Windows.Forms.TextBox zoneTexteMotDePasse;
        private System.Windows.Forms.ErrorProvider errorProviderMenuGeneral;
    }
}

