namespace KineGestionApp
{
    partial class Boite_Modale_Mutuelles
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
            this.tableLayoutPanelModalMutuelles = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAjouterModalMutuelles = new System.Windows.Forms.Button();
            this.buttonModifierModalMutuelles = new System.Windows.Forms.Button();
            this.buttonQuitterModalMutuelles = new System.Windows.Forms.Button();
            this.tableLayoutPanelModalMutuelles.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelModalMutuelles
            // 
            this.tableLayoutPanelModalMutuelles.ColumnCount = 2;
            this.tableLayoutPanelModalMutuelles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalMutuelles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalMutuelles.Controls.Add(this.buttonAjouterModalMutuelles, 0, 1);
            this.tableLayoutPanelModalMutuelles.Controls.Add(this.buttonModifierModalMutuelles, 1, 1);
            this.tableLayoutPanelModalMutuelles.Controls.Add(this.buttonQuitterModalMutuelles, 1, 0);
            this.tableLayoutPanelModalMutuelles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelModalMutuelles.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelModalMutuelles.Name = "tableLayoutPanelModalMutuelles";
            this.tableLayoutPanelModalMutuelles.RowCount = 2;
            this.tableLayoutPanelModalMutuelles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalMutuelles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelModalMutuelles.Size = new System.Drawing.Size(346, 172);
            this.tableLayoutPanelModalMutuelles.TabIndex = 0;
            // 
            // buttonAjouterModalMutuelles
            // 
            this.buttonAjouterModalMutuelles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAjouterModalMutuelles.Location = new System.Drawing.Point(3, 89);
            this.buttonAjouterModalMutuelles.Name = "buttonAjouterModalMutuelles";
            this.buttonAjouterModalMutuelles.Size = new System.Drawing.Size(167, 80);
            this.buttonAjouterModalMutuelles.TabIndex = 1;
            this.buttonAjouterModalMutuelles.Text = "Ajouter une mutuelle";
            this.buttonAjouterModalMutuelles.UseVisualStyleBackColor = true;
            this.buttonAjouterModalMutuelles.Click += new System.EventHandler(this.buttonAjouterModalMutuelles_Click);
            // 
            // buttonModifierModalMutuelles
            // 
            this.buttonModifierModalMutuelles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonModifierModalMutuelles.Location = new System.Drawing.Point(176, 89);
            this.buttonModifierModalMutuelles.Name = "buttonModifierModalMutuelles";
            this.buttonModifierModalMutuelles.Size = new System.Drawing.Size(167, 80);
            this.buttonModifierModalMutuelles.TabIndex = 2;
            this.buttonModifierModalMutuelles.Text = "Modifier une mutuelle";
            this.buttonModifierModalMutuelles.UseVisualStyleBackColor = true;
            this.buttonModifierModalMutuelles.Click += new System.EventHandler(this.buttonModifierModalMutuelles_Click);
            // 
            // buttonQuitterModalMutuelles
            // 
            this.buttonQuitterModalMutuelles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonQuitterModalMutuelles.Location = new System.Drawing.Point(176, 3);
            this.buttonQuitterModalMutuelles.Name = "buttonQuitterModalMutuelles";
            this.buttonQuitterModalMutuelles.Size = new System.Drawing.Size(167, 80);
            this.buttonQuitterModalMutuelles.TabIndex = 0;
            this.buttonQuitterModalMutuelles.Text = "Revenir au menu général";
            this.buttonQuitterModalMutuelles.UseVisualStyleBackColor = true;
            this.buttonQuitterModalMutuelles.Click += new System.EventHandler(this.buttonQuitterModalMutuelles_Click);
            // 
            // Boite_Modale_Mutuelles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 172);
            this.Controls.Add(this.tableLayoutPanelModalMutuelles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Boite_Modale_Mutuelles";
            this.Text = "Boite_Modale_Mutuelle";
            this.tableLayoutPanelModalMutuelles.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelModalMutuelles;
        private System.Windows.Forms.Button buttonAjouterModalMutuelles;
        private System.Windows.Forms.Button buttonModifierModalMutuelles;
        private System.Windows.Forms.Button buttonQuitterModalMutuelles;
    }
}