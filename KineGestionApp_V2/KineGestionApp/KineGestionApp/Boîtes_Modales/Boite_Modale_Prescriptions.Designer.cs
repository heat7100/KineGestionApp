namespace KineGestionApp
{
    partial class Boite_Modale_Prescriptions
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.boutonCreerPescriptionModalPrescriptions = new System.Windows.Forms.Button();
            this.boutonConsulterPrescriptionModalPrescriptions = new System.Windows.Forms.Button();
            this.boutonPrescriptionsEnCoursModalPrescriptions = new System.Windows.Forms.Button();
            this.boutonQuitterModalPrescriptions = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.boutonCreerPescriptionModalPrescriptions, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.boutonConsulterPrescriptionModalPrescriptions, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.boutonPrescriptionsEnCoursModalPrescriptions, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.boutonQuitterModalPrescriptions, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(539, 192);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // boutonCreerPescriptionModalPrescriptions
            // 
            this.boutonCreerPescriptionModalPrescriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonCreerPescriptionModalPrescriptions.Location = new System.Drawing.Point(3, 99);
            this.boutonCreerPescriptionModalPrescriptions.Name = "boutonCreerPescriptionModalPrescriptions";
            this.boutonCreerPescriptionModalPrescriptions.Size = new System.Drawing.Size(263, 90);
            this.boutonCreerPescriptionModalPrescriptions.TabIndex = 2;
            this.boutonCreerPescriptionModalPrescriptions.Text = "Créer une prescription";
            this.boutonCreerPescriptionModalPrescriptions.UseVisualStyleBackColor = true;
            this.boutonCreerPescriptionModalPrescriptions.Click += new System.EventHandler(this.boutonCreerPescriptionModalPrescriptions_Click);
            // 
            // boutonConsulterPrescriptionModalPrescriptions
            // 
            this.boutonConsulterPrescriptionModalPrescriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonConsulterPrescriptionModalPrescriptions.Location = new System.Drawing.Point(272, 99);
            this.boutonConsulterPrescriptionModalPrescriptions.Name = "boutonConsulterPrescriptionModalPrescriptions";
            this.boutonConsulterPrescriptionModalPrescriptions.Size = new System.Drawing.Size(264, 90);
            this.boutonConsulterPrescriptionModalPrescriptions.TabIndex = 3;
            this.boutonConsulterPrescriptionModalPrescriptions.Text = "Consulter une prescription";
            this.boutonConsulterPrescriptionModalPrescriptions.UseVisualStyleBackColor = true;
            this.boutonConsulterPrescriptionModalPrescriptions.Click += new System.EventHandler(this.boutonConsulterPrescriptionModalPrescriptions_Click);
            // 
            // boutonPrescriptionsEnCoursModalPrescriptions
            // 
            this.boutonPrescriptionsEnCoursModalPrescriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonPrescriptionsEnCoursModalPrescriptions.Location = new System.Drawing.Point(272, 3);
            this.boutonPrescriptionsEnCoursModalPrescriptions.Name = "boutonPrescriptionsEnCoursModalPrescriptions";
            this.boutonPrescriptionsEnCoursModalPrescriptions.Size = new System.Drawing.Size(264, 90);
            this.boutonPrescriptionsEnCoursModalPrescriptions.TabIndex = 1;
            this.boutonPrescriptionsEnCoursModalPrescriptions.Text = "Prescriptions en cours";
            this.boutonPrescriptionsEnCoursModalPrescriptions.UseVisualStyleBackColor = true;
            this.boutonPrescriptionsEnCoursModalPrescriptions.Click += new System.EventHandler(this.boutonPrescriptionsEnCoursModalPrescriptions_Click);
            // 
            // boutonQuitterModalPrescriptions
            // 
            this.boutonQuitterModalPrescriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boutonQuitterModalPrescriptions.Location = new System.Drawing.Point(3, 3);
            this.boutonQuitterModalPrescriptions.Name = "boutonQuitterModalPrescriptions";
            this.boutonQuitterModalPrescriptions.Size = new System.Drawing.Size(263, 90);
            this.boutonQuitterModalPrescriptions.TabIndex = 0;
            this.boutonQuitterModalPrescriptions.Text = "Revenir au menu général";
            this.boutonQuitterModalPrescriptions.UseVisualStyleBackColor = true;
            this.boutonQuitterModalPrescriptions.Click += new System.EventHandler(this.boutonQuitterModalPrescriptions_Click);
            // 
            // Boite_Modale_Prescriptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 192);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Boite_Modale_Prescriptions";
            this.Text = "Boite_Modale_Prescriptions";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button boutonCreerPescriptionModalPrescriptions;
        private System.Windows.Forms.Button boutonConsulterPrescriptionModalPrescriptions;
        private System.Windows.Forms.Button boutonPrescriptionsEnCoursModalPrescriptions;
        private System.Windows.Forms.Button boutonQuitterModalPrescriptions;
    }
}