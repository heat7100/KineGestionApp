namespace KineGestionApp
{
    partial class Boite_Modale_Patients
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
            this.tableLayoutPanelModalPatients = new System.Windows.Forms.TableLayoutPanel();
            this.boutonAjouterModalPatients = new System.Windows.Forms.Button();
            this.boutonModifierModalPatients = new System.Windows.Forms.Button();
            this.boutonQuitterModalPatients = new System.Windows.Forms.Button();
            this.tableLayoutPanelModalPatients.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelModalPatients
            // 
            this.tableLayoutPanelModalPatients.ColumnCount = 2;
            this.tableLayoutPanelModalPatients.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalPatients.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalPatients.Controls.Add(this.boutonAjouterModalPatients, 0, 1);
            this.tableLayoutPanelModalPatients.Controls.Add(this.boutonModifierModalPatients, 1, 1);
            this.tableLayoutPanelModalPatients.Controls.Add(this.boutonQuitterModalPatients, 1, 0);
            this.tableLayoutPanelModalPatients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelModalPatients.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelModalPatients.Name = "tableLayoutPanelModalPatients";
            this.tableLayoutPanelModalPatients.RowCount = 2;
            this.tableLayoutPanelModalPatients.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalPatients.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalPatients.Size = new System.Drawing.Size(372, 192);
            this.tableLayoutPanelModalPatients.TabIndex = 0;
            // 
            // boutonAjouterModalPatients
            // 
            this.boutonAjouterModalPatients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonAjouterModalPatients.Location = new System.Drawing.Point(3, 99);
            this.boutonAjouterModalPatients.Name = "boutonAjouterModalPatients";
            this.boutonAjouterModalPatients.Size = new System.Drawing.Size(180, 90);
            this.boutonAjouterModalPatients.TabIndex = 0;
            this.boutonAjouterModalPatients.Text = "Ajouter un patient";
            this.boutonAjouterModalPatients.UseVisualStyleBackColor = true;
            this.boutonAjouterModalPatients.Click += new System.EventHandler(this.boutonAjouterModalPatients_Click);
            // 
            // boutonModifierModalPatients
            // 
            this.boutonModifierModalPatients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonModifierModalPatients.Location = new System.Drawing.Point(189, 99);
            this.boutonModifierModalPatients.Name = "boutonModifierModalPatients";
            this.boutonModifierModalPatients.Size = new System.Drawing.Size(180, 90);
            this.boutonModifierModalPatients.TabIndex = 1;
            this.boutonModifierModalPatients.Text = "Modifier un patient";
            this.boutonModifierModalPatients.UseVisualStyleBackColor = true;
            this.boutonModifierModalPatients.Click += new System.EventHandler(this.boutonModifierModalPatients_Click);
            // 
            // boutonQuitterModalPatients
            // 
            this.boutonQuitterModalPatients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonQuitterModalPatients.Location = new System.Drawing.Point(189, 3);
            this.boutonQuitterModalPatients.Name = "boutonQuitterModalPatients";
            this.boutonQuitterModalPatients.Size = new System.Drawing.Size(180, 90);
            this.boutonQuitterModalPatients.TabIndex = 2;
            this.boutonQuitterModalPatients.Text = "Revenir au menu général";
            this.boutonQuitterModalPatients.UseVisualStyleBackColor = true;
            this.boutonQuitterModalPatients.Click += new System.EventHandler(this.boutonQuitterModalPatients_Click);
            // 
            // Boite_Modale_Patients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 192);
            this.Controls.Add(this.tableLayoutPanelModalPatients);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Boite_Modale_Patients";
            this.Text = "Boite_Modale_Patients";
            this.tableLayoutPanelModalPatients.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelModalPatients;
        private System.Windows.Forms.Button boutonAjouterModalPatients;
        private System.Windows.Forms.Button boutonModifierModalPatients;
        private System.Windows.Forms.Button boutonQuitterModalPatients;
    }
}