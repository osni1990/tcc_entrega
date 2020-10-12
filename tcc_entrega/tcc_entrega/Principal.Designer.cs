namespace tcc_entrega
{
    partial class Principal
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
            this.lblPiso = new System.Windows.Forms.Label();
            this.lblTeto = new System.Windows.Forms.Label();
            this.txtValorPiso = new System.Windows.Forms.TextBox();
            this.txtValorTeto = new System.Windows.Forms.TextBox();
            this.dgvParametros = new System.Windows.Forms.DataGridView();
            this.lblCodigoPapel = new System.Windows.Forms.Label();
            this.cbxCodigoPapel = new System.Windows.Forms.ComboBox();
            this.btnAdicionaParametro = new System.Windows.Forms.Button();
            this.picBoxLampada = new System.Windows.Forms.PictureBox();
            this.lstBoxLogEvento = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametros)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLampada)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPiso
            // 
            this.lblPiso.AutoSize = true;
            this.lblPiso.Location = new System.Drawing.Point(52, 34);
            this.lblPiso.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPiso.Name = "lblPiso";
            this.lblPiso.Size = new System.Drawing.Size(32, 13);
            this.lblPiso.TabIndex = 0;
            this.lblPiso.Text = "PISO";
            // 
            // lblTeto
            // 
            this.lblTeto.AutoSize = true;
            this.lblTeto.Location = new System.Drawing.Point(48, 58);
            this.lblTeto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTeto.Name = "lblTeto";
            this.lblTeto.Size = new System.Drawing.Size(36, 13);
            this.lblTeto.TabIndex = 1;
            this.lblTeto.Text = "TETO";
            // 
            // txtValorPiso
            // 
            this.txtValorPiso.Location = new System.Drawing.Point(88, 31);
            this.txtValorPiso.Margin = new System.Windows.Forms.Padding(2);
            this.txtValorPiso.Name = "txtValorPiso";
            this.txtValorPiso.Size = new System.Drawing.Size(68, 20);
            this.txtValorPiso.TabIndex = 2;
            // 
            // txtValorTeto
            // 
            this.txtValorTeto.Location = new System.Drawing.Point(88, 55);
            this.txtValorTeto.Margin = new System.Windows.Forms.Padding(2);
            this.txtValorTeto.Name = "txtValorTeto";
            this.txtValorTeto.Size = new System.Drawing.Size(68, 20);
            this.txtValorTeto.TabIndex = 3;
            // 
            // dgvParametros
            // 
            this.dgvParametros.AllowUserToAddRows = false;
            this.dgvParametros.AllowUserToDeleteRows = false;
            this.dgvParametros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParametros.Location = new System.Drawing.Point(234, 6);
            this.dgvParametros.Margin = new System.Windows.Forms.Padding(2);
            this.dgvParametros.Name = "dgvParametros";
            this.dgvParametros.ReadOnly = true;
            this.dgvParametros.RowHeadersWidth = 62;
            this.dgvParametros.RowTemplate.Height = 28;
            this.dgvParametros.Size = new System.Drawing.Size(421, 223);
            this.dgvParametros.TabIndex = 4;
            // 
            // lblCodigoPapel
            // 
            this.lblCodigoPapel.AutoSize = true;
            this.lblCodigoPapel.Location = new System.Drawing.Point(11, 9);
            this.lblCodigoPapel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCodigoPapel.Name = "lblCodigoPapel";
            this.lblCodigoPapel.Size = new System.Drawing.Size(73, 13);
            this.lblCodigoPapel.TabIndex = 6;
            this.lblCodigoPapel.Text = "Código Papel:";
            // 
            // cbxCodigoPapel
            // 
            this.cbxCodigoPapel.FormattingEnabled = true;
            this.cbxCodigoPapel.Location = new System.Drawing.Point(88, 6);
            this.cbxCodigoPapel.Margin = new System.Windows.Forms.Padding(2);
            this.cbxCodigoPapel.Name = "cbxCodigoPapel";
            this.cbxCodigoPapel.Size = new System.Drawing.Size(68, 21);
            this.cbxCodigoPapel.TabIndex = 7;
            // 
            // btnAdicionaParametro
            // 
            this.btnAdicionaParametro.Location = new System.Drawing.Point(160, 6);
            this.btnAdicionaParametro.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdicionaParametro.Name = "btnAdicionaParametro";
            this.btnAdicionaParametro.Size = new System.Drawing.Size(70, 69);
            this.btnAdicionaParametro.TabIndex = 8;
            this.btnAdicionaParametro.Text = "Adiciona Registro";
            this.btnAdicionaParametro.UseVisualStyleBackColor = true;
            this.btnAdicionaParametro.Click += new System.EventHandler(this.btnAdicionaParametro_Click);
            // 
            // picBoxLampada
            // 
            this.picBoxLampada.Image = global::tcc_entrega.Properties.Resources.lampada;
            this.picBoxLampada.Location = new System.Drawing.Point(37, 79);
            this.picBoxLampada.Margin = new System.Windows.Forms.Padding(2);
            this.picBoxLampada.Name = "picBoxLampada";
            this.picBoxLampada.Size = new System.Drawing.Size(150, 150);
            this.picBoxLampada.TabIndex = 5;
            this.picBoxLampada.TabStop = false;
            this.picBoxLampada.Visible = false;
            // 
            // lstBoxLogEvento
            // 
            this.lstBoxLogEvento.FormattingEnabled = true;
            this.lstBoxLogEvento.Location = new System.Drawing.Point(660, 6);
            this.lstBoxLogEvento.Name = "lstBoxLogEvento";
            this.lstBoxLogEvento.Size = new System.Drawing.Size(373, 225);
            this.lstBoxLogEvento.TabIndex = 9;
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 238);
            this.Controls.Add(this.lstBoxLogEvento);
            this.Controls.Add(this.btnAdicionaParametro);
            this.Controls.Add(this.cbxCodigoPapel);
            this.Controls.Add(this.lblCodigoPapel);
            this.Controls.Add(this.picBoxLampada);
            this.Controls.Add(this.dgvParametros);
            this.Controls.Add(this.txtValorTeto);
            this.Controls.Add(this.txtValorPiso);
            this.Controls.Add(this.lblTeto);
            this.Controls.Add(this.lblPiso);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Principal";
            this.Text = "Principal";
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametros)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLampada)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPiso;
        private System.Windows.Forms.Label lblTeto;
        private System.Windows.Forms.TextBox txtValorPiso;
        private System.Windows.Forms.TextBox txtValorTeto;
        private System.Windows.Forms.DataGridView dgvParametros;
        private System.Windows.Forms.PictureBox picBoxLampada;
        private System.Windows.Forms.Label lblCodigoPapel;
        private System.Windows.Forms.ComboBox cbxCodigoPapel;
        private System.Windows.Forms.Button btnAdicionaParametro;
        private System.Windows.Forms.ListBox lstBoxLogEvento;
    }
}

