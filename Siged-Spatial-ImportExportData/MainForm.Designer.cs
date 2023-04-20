namespace Siged_Spatial_ImportExportData
{
    partial class MainForm
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
            this.btnExportarImportar = new System.Windows.Forms.Button();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.cbDivision = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbTransfParticularesA = new System.Windows.Forms.CheckBox();
            this.chbReguladores = new System.Windows.Forms.CheckBox();
            this.chbAlimentadores = new System.Windows.Forms.CheckBox();
            this.chbFusibles = new System.Windows.Forms.CheckBox();
            this.chbRestauradores = new System.Windows.Forms.CheckBox();
            this.chbDesconectadores = new System.Windows.Forms.CheckBox();
            this.chbCapacitores = new System.Windows.Forms.CheckBox();
            this.chbTransformadores = new System.Windows.Forms.CheckBox();
            this.chbPostes = new System.Windows.Forms.CheckBox();
            this.chbLineaPrimaria = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbSeccionadores = new System.Windows.Forms.CheckBox();
            this.chbAcometidaMT = new System.Windows.Forms.CheckBox();
            this.chbTransicionesMT = new System.Windows.Forms.CheckBox();
            this.chbRegistrosBT = new System.Windows.Forms.CheckBox();
            this.chbTransicionesBT = new System.Windows.Forms.CheckBox();
            this.chbRegistrosMt = new System.Windows.Forms.CheckBox();
            this.chbTransfCfeSubt = new System.Windows.Forms.CheckBox();
            this.chbLinea_mts = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chbLineasAT = new System.Windows.Forms.CheckBox();
            this.chbTorres = new System.Windows.Forms.CheckBox();
            this.btnExportarImportarMadis = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.chbLineasATSubt = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chbRegistrosSubt = new System.Windows.Forms.CheckBox();
            this.chbCuchillaSubt = new System.Windows.Forms.CheckBox();
            this.chbTransicionesSubt = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExportarImportar
            // 
            this.btnExportarImportar.Location = new System.Drawing.Point(23, 364);
            this.btnExportarImportar.Name = "btnExportarImportar";
            this.btnExportarImportar.Size = new System.Drawing.Size(362, 48);
            this.btnExportarImportar.TabIndex = 0;
            this.btnExportarImportar.Text = "Ejecutar SIGED";
            this.btnExportarImportar.UseVisualStyleBackColor = true;
            this.btnExportarImportar.Click += new System.EventHandler(this.btnExportarImportar_Click);
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Location = new System.Drawing.Point(20, 432);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(47, 13);
            this.lblMensaje.TabIndex = 5;
            this.lblMensaje.Text = "Mensaje";
            // 
            // cbDivision
            // 
            this.cbDivision.FormattingEnabled = true;
            this.cbDivision.Location = new System.Drawing.Point(74, 12);
            this.cbDivision.Name = "cbDivision";
            this.cbDivision.Size = new System.Drawing.Size(255, 21);
            this.cbDivision.TabIndex = 6;
            this.cbDivision.SelectedIndexChanged += new System.EventHandler(this.cbDivision_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "DIV:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbTransfParticularesA);
            this.groupBox1.Controls.Add(this.chbReguladores);
            this.groupBox1.Controls.Add(this.chbAlimentadores);
            this.groupBox1.Controls.Add(this.chbFusibles);
            this.groupBox1.Controls.Add(this.chbRestauradores);
            this.groupBox1.Controls.Add(this.chbDesconectadores);
            this.groupBox1.Controls.Add(this.chbCapacitores);
            this.groupBox1.Controls.Add(this.chbTransformadores);
            this.groupBox1.Controls.Add(this.chbPostes);
            this.groupBox1.Controls.Add(this.chbLineaPrimaria);
            this.groupBox1.Location = new System.Drawing.Point(23, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 272);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Aereo";
            // 
            // chbTransfParticularesA
            // 
            this.chbTransfParticularesA.AutoSize = true;
            this.chbTransfParticularesA.Location = new System.Drawing.Point(19, 226);
            this.chbTransfParticularesA.Name = "chbTransfParticularesA";
            this.chbTransfParticularesA.Size = new System.Drawing.Size(114, 17);
            this.chbTransfParticularesA.TabIndex = 10;
            this.chbTransfParticularesA.Text = "Transf Particulares";
            this.chbTransfParticularesA.UseVisualStyleBackColor = true;
            // 
            // chbReguladores
            // 
            this.chbReguladores.AutoSize = true;
            this.chbReguladores.Location = new System.Drawing.Point(19, 203);
            this.chbReguladores.Name = "chbReguladores";
            this.chbReguladores.Size = new System.Drawing.Size(86, 17);
            this.chbReguladores.TabIndex = 9;
            this.chbReguladores.Text = "Reguladores";
            this.chbReguladores.UseVisualStyleBackColor = true;
            // 
            // chbAlimentadores
            // 
            this.chbAlimentadores.AutoSize = true;
            this.chbAlimentadores.Location = new System.Drawing.Point(19, 19);
            this.chbAlimentadores.Name = "chbAlimentadores";
            this.chbAlimentadores.Size = new System.Drawing.Size(92, 17);
            this.chbAlimentadores.TabIndex = 8;
            this.chbAlimentadores.Text = "Alimentadores";
            this.chbAlimentadores.UseVisualStyleBackColor = true;
            // 
            // chbFusibles
            // 
            this.chbFusibles.AutoSize = true;
            this.chbFusibles.Location = new System.Drawing.Point(19, 180);
            this.chbFusibles.Name = "chbFusibles";
            this.chbFusibles.Size = new System.Drawing.Size(64, 17);
            this.chbFusibles.TabIndex = 7;
            this.chbFusibles.Text = "Fusibles";
            this.chbFusibles.UseVisualStyleBackColor = true;
            // 
            // chbRestauradores
            // 
            this.chbRestauradores.AutoSize = true;
            this.chbRestauradores.Location = new System.Drawing.Point(19, 157);
            this.chbRestauradores.Name = "chbRestauradores";
            this.chbRestauradores.Size = new System.Drawing.Size(95, 17);
            this.chbRestauradores.TabIndex = 6;
            this.chbRestauradores.Text = "Restauradores";
            this.chbRestauradores.UseVisualStyleBackColor = true;
            // 
            // chbDesconectadores
            // 
            this.chbDesconectadores.AutoSize = true;
            this.chbDesconectadores.Location = new System.Drawing.Point(19, 134);
            this.chbDesconectadores.Name = "chbDesconectadores";
            this.chbDesconectadores.Size = new System.Drawing.Size(110, 17);
            this.chbDesconectadores.TabIndex = 5;
            this.chbDesconectadores.Text = "Desconectadores";
            this.chbDesconectadores.UseVisualStyleBackColor = true;
            // 
            // chbCapacitores
            // 
            this.chbCapacitores.AutoSize = true;
            this.chbCapacitores.Location = new System.Drawing.Point(19, 111);
            this.chbCapacitores.Name = "chbCapacitores";
            this.chbCapacitores.Size = new System.Drawing.Size(82, 17);
            this.chbCapacitores.TabIndex = 4;
            this.chbCapacitores.Text = "Capacitores";
            this.chbCapacitores.UseVisualStyleBackColor = true;
            // 
            // chbTransformadores
            // 
            this.chbTransformadores.AutoSize = true;
            this.chbTransformadores.Location = new System.Drawing.Point(19, 88);
            this.chbTransformadores.Name = "chbTransformadores";
            this.chbTransformadores.Size = new System.Drawing.Size(105, 17);
            this.chbTransformadores.TabIndex = 3;
            this.chbTransformadores.Text = "Transformadores";
            this.chbTransformadores.UseVisualStyleBackColor = true;
            // 
            // chbPostes
            // 
            this.chbPostes.AutoSize = true;
            this.chbPostes.Location = new System.Drawing.Point(19, 65);
            this.chbPostes.Name = "chbPostes";
            this.chbPostes.Size = new System.Drawing.Size(58, 17);
            this.chbPostes.TabIndex = 2;
            this.chbPostes.Text = "Postes";
            this.chbPostes.UseVisualStyleBackColor = true;
            // 
            // chbLineaPrimaria
            // 
            this.chbLineaPrimaria.AutoSize = true;
            this.chbLineaPrimaria.Location = new System.Drawing.Point(19, 42);
            this.chbLineaPrimaria.Name = "chbLineaPrimaria";
            this.chbLineaPrimaria.Size = new System.Drawing.Size(89, 17);
            this.chbLineaPrimaria.TabIndex = 0;
            this.chbLineaPrimaria.Text = "LineaPrimaria";
            this.chbLineaPrimaria.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbSeccionadores);
            this.groupBox2.Controls.Add(this.chbAcometidaMT);
            this.groupBox2.Controls.Add(this.chbTransicionesMT);
            this.groupBox2.Controls.Add(this.chbRegistrosBT);
            this.groupBox2.Controls.Add(this.chbTransicionesBT);
            this.groupBox2.Controls.Add(this.chbRegistrosMt);
            this.groupBox2.Controls.Add(this.chbTransfCfeSubt);
            this.groupBox2.Controls.Add(this.chbLinea_mts);
            this.groupBox2.Location = new System.Drawing.Point(211, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(182, 272);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Subterraneo";
            // 
            // chbSeccionadores
            // 
            this.chbSeccionadores.AutoSize = true;
            this.chbSeccionadores.Location = new System.Drawing.Point(21, 180);
            this.chbSeccionadores.Name = "chbSeccionadores";
            this.chbSeccionadores.Size = new System.Drawing.Size(97, 17);
            this.chbSeccionadores.TabIndex = 7;
            this.chbSeccionadores.Text = "Seccionadores";
            this.chbSeccionadores.UseVisualStyleBackColor = true;
            this.chbSeccionadores.CheckedChanged += new System.EventHandler(this.chbSeccionadores_CheckedChanged);
            // 
            // chbAcometidaMT
            // 
            this.chbAcometidaMT.AutoSize = true;
            this.chbAcometidaMT.Location = new System.Drawing.Point(21, 157);
            this.chbAcometidaMT.Name = "chbAcometidaMT";
            this.chbAcometidaMT.Size = new System.Drawing.Size(95, 17);
            this.chbAcometidaMT.TabIndex = 6;
            this.chbAcometidaMT.Text = "Acometida MT";
            this.chbAcometidaMT.UseVisualStyleBackColor = true;
            // 
            // chbTransicionesMT
            // 
            this.chbTransicionesMT.AutoSize = true;
            this.chbTransicionesMT.Location = new System.Drawing.Point(23, 88);
            this.chbTransicionesMT.Name = "chbTransicionesMT";
            this.chbTransicionesMT.Size = new System.Drawing.Size(105, 17);
            this.chbTransicionesMT.TabIndex = 5;
            this.chbTransicionesMT.Text = "Transiciones MT";
            this.chbTransicionesMT.UseVisualStyleBackColor = true;
            // 
            // chbRegistrosBT
            // 
            this.chbRegistrosBT.AutoSize = true;
            this.chbRegistrosBT.Location = new System.Drawing.Point(21, 111);
            this.chbRegistrosBT.Name = "chbRegistrosBT";
            this.chbRegistrosBT.Size = new System.Drawing.Size(84, 17);
            this.chbRegistrosBT.TabIndex = 4;
            this.chbRegistrosBT.Text = "RegistrosBT";
            this.chbRegistrosBT.UseVisualStyleBackColor = true;
            // 
            // chbTransicionesBT
            // 
            this.chbTransicionesBT.AutoSize = true;
            this.chbTransicionesBT.Location = new System.Drawing.Point(21, 134);
            this.chbTransicionesBT.Name = "chbTransicionesBT";
            this.chbTransicionesBT.Size = new System.Drawing.Size(103, 17);
            this.chbTransicionesBT.TabIndex = 3;
            this.chbTransicionesBT.Text = "Transiciones BT";
            this.chbTransicionesBT.UseVisualStyleBackColor = true;
            // 
            // chbRegistrosMt
            // 
            this.chbRegistrosMt.AutoSize = true;
            this.chbRegistrosMt.Location = new System.Drawing.Point(21, 19);
            this.chbRegistrosMt.Name = "chbRegistrosMt";
            this.chbRegistrosMt.Size = new System.Drawing.Size(89, 17);
            this.chbRegistrosMt.TabIndex = 2;
            this.chbRegistrosMt.Text = "Registros MT";
            this.chbRegistrosMt.UseVisualStyleBackColor = true;
            // 
            // chbTransfCfeSubt
            // 
            this.chbTransfCfeSubt.AutoSize = true;
            this.chbTransfCfeSubt.Location = new System.Drawing.Point(21, 65);
            this.chbTransfCfeSubt.Name = "chbTransfCfeSubt";
            this.chbTransfCfeSubt.Size = new System.Drawing.Size(105, 17);
            this.chbTransfCfeSubt.TabIndex = 1;
            this.chbTransfCfeSubt.Text = "Transformadores";
            this.chbTransfCfeSubt.UseVisualStyleBackColor = true;
            // 
            // chbLinea_mts
            // 
            this.chbLinea_mts.AutoSize = true;
            this.chbLinea_mts.Location = new System.Drawing.Point(21, 42);
            this.chbLinea_mts.Name = "chbLinea_mts";
            this.chbLinea_mts.Size = new System.Drawing.Size(125, 17);
            this.chbLinea_mts.TabIndex = 0;
            this.chbLinea_mts.Text = "Linea Media Tensión";
            this.chbLinea_mts.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(180, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "SIGED";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(558, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "MADIS";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chbLineasAT);
            this.groupBox3.Controls.Add(this.chbTorres);
            this.groupBox3.Location = new System.Drawing.Point(417, 71);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(182, 272);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Aereo";
            // 
            // chbLineasAT
            // 
            this.chbLineasAT.AutoSize = true;
            this.chbLineasAT.Location = new System.Drawing.Point(19, 42);
            this.chbLineasAT.Name = "chbLineasAT";
            this.chbLineasAT.Size = new System.Drawing.Size(69, 17);
            this.chbLineasAT.TabIndex = 9;
            this.chbLineasAT.Text = "Linea AT";
            this.chbLineasAT.UseVisualStyleBackColor = true;
            // 
            // chbTorres
            // 
            this.chbTorres.AutoSize = true;
            this.chbTorres.Location = new System.Drawing.Point(19, 19);
            this.chbTorres.Name = "chbTorres";
            this.chbTorres.Size = new System.Drawing.Size(56, 17);
            this.chbTorres.TabIndex = 8;
            this.chbTorres.Text = "Torres";
            this.chbTorres.UseVisualStyleBackColor = true;
            // 
            // btnExportarImportarMadis
            // 
            this.btnExportarImportarMadis.Location = new System.Drawing.Point(417, 364);
            this.btnExportarImportarMadis.Name = "btnExportarImportarMadis";
            this.btnExportarImportarMadis.Size = new System.Drawing.Size(362, 48);
            this.btnExportarImportarMadis.TabIndex = 13;
            this.btnExportarImportarMadis.Text = "Ejecutar MADIS";
            this.btnExportarImportarMadis.UseVisualStyleBackColor = true;
            this.btnExportarImportarMadis.Click += new System.EventHandler(this.btnExportarImportarMadis_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 415);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(710, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "La importación en MADIS  se hace por Division , hago una consulta a la tabla area" +
    "sres y por cada zona importo los elementos seleccionados";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label4.UseCompatibleTextRendering = true;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // chbLineasATSubt
            // 
            this.chbLineasATSubt.AutoSize = true;
            this.chbLineasATSubt.Location = new System.Drawing.Point(6, 20);
            this.chbLineasATSubt.Name = "chbLineasATSubt";
            this.chbLineasATSubt.Size = new System.Drawing.Size(74, 17);
            this.chbLineasATSubt.TabIndex = 10;
            this.chbLineasATSubt.Text = "Lineas AT";
            this.chbLineasATSubt.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chbTransicionesSubt);
            this.groupBox4.Controls.Add(this.chbRegistrosSubt);
            this.groupBox4.Controls.Add(this.chbCuchillaSubt);
            this.groupBox4.Controls.Add(this.chbLineasATSubt);
            this.groupBox4.Location = new System.Drawing.Point(608, 70);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(182, 272);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Subterraneo";
            // 
            // chbRegistrosSubt
            // 
            this.chbRegistrosSubt.AutoSize = true;
            this.chbRegistrosSubt.Location = new System.Drawing.Point(6, 66);
            this.chbRegistrosSubt.Name = "chbRegistrosSubt";
            this.chbRegistrosSubt.Size = new System.Drawing.Size(70, 17);
            this.chbRegistrosSubt.TabIndex = 12;
            this.chbRegistrosSubt.Text = "Registros";
            this.chbRegistrosSubt.UseVisualStyleBackColor = true;
            // 
            // chbCuchillaSubt
            // 
            this.chbCuchillaSubt.AutoSize = true;
            this.chbCuchillaSubt.Location = new System.Drawing.Point(6, 43);
            this.chbCuchillaSubt.Name = "chbCuchillaSubt";
            this.chbCuchillaSubt.Size = new System.Drawing.Size(68, 17);
            this.chbCuchillaSubt.TabIndex = 11;
            this.chbCuchillaSubt.Text = "Cuchillas";
            this.chbCuchillaSubt.UseVisualStyleBackColor = true;
            // 
            // chbTransicionesSubt
            // 
            this.chbTransicionesSubt.AutoSize = true;
            this.chbTransicionesSubt.Location = new System.Drawing.Point(6, 89);
            this.chbTransicionesSubt.Name = "chbTransicionesSubt";
            this.chbTransicionesSubt.Size = new System.Drawing.Size(86, 17);
            this.chbTransicionesSubt.TabIndex = 13;
            this.chbTransicionesSubt.Text = "Transiciones";
            this.chbTransicionesSubt.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 466);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnExportarImportarMadis);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbDivision);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.btnExportarImportar);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExportarImportar;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.ComboBox cbDivision;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbTransformadores;
        private System.Windows.Forms.CheckBox chbPostes;
        private System.Windows.Forms.CheckBox chbLineaPrimaria;
        private System.Windows.Forms.CheckBox chbCapacitores;
        private System.Windows.Forms.CheckBox chbDesconectadores;
        private System.Windows.Forms.CheckBox chbRestauradores;
        private System.Windows.Forms.CheckBox chbFusibles;

        private System.Windows.Forms.CheckBox chbAlimentadores;
        private System.Windows.Forms.CheckBox chbReguladores;
        private System.Windows.Forms.CheckBox chbTransfParticularesA;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbTransfCfeSubt;
        private System.Windows.Forms.CheckBox chbLinea_mts;
        private System.Windows.Forms.CheckBox chbRegistrosMt;
        private System.Windows.Forms.CheckBox chbTransicionesBT;
        private System.Windows.Forms.CheckBox chbRegistrosBT;
        private System.Windows.Forms.CheckBox chbTransicionesMT;
        private System.Windows.Forms.CheckBox chbAcometidaMT;
        private System.Windows.Forms.CheckBox chbSeccionadores;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chbTorres;
        private System.Windows.Forms.Button btnExportarImportarMadis;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chbLineasAT;
        private System.Windows.Forms.CheckBox chbLineasATSubt;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chbCuchillaSubt;
        private System.Windows.Forms.CheckBox chbRegistrosSubt;
        private System.Windows.Forms.CheckBox chbTransicionesSubt;
    }
}

