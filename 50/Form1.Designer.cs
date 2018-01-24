//匯入DLL檔案
using System;

using Advantech.Motion;


namespace _50
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //全域變數宣告
        DEV_LIST[] CurAvailableDevs = new DEV_LIST[Motion.MAX_DEVICES];

        IntPtr m_DeviceHandle = IntPtr.Zero;
        IntPtr m_GpHand = IntPtr.Zero;
        IntPtr[] m_Axishand = new IntPtr[32];

        ushort[] sp = new ushort[4] ;
        double[] EndArray = new double[32];
        double[] Key = new double[4];
        double E;
        double Vel = 1000;
        double VelLow = 0;
        double Acc = 50;
        double Dec = 50;
        double VelHighE = 50;
        double VelLowE = 0;
        double AccE = 5;
        double DecE = 5;
        uint deviceCount = 0;
        uint AxCountInGp = 0;
        uint DeviceNum = 0;
        uint EndPointNum = 0;
        uint BoardID = 0;       //E軸卡ID
        uint m_ulAxisCount = 0;
        bool m_bInit = false;
        bool m_bServoOn = false;

        double fe;
        ushort Es;
        int currentGcode;

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tcl_Window00 = new System.Windows.Forms.TabControl();
            this.tp1_Test = new System.Windows.Forms.TabPage();
            this.gbx_Gcode = new System.Windows.Forms.GroupBox();
            this.txt_Z = new System.Windows.Forms.TextBox();
            this.txt_Y = new System.Windows.Forms.TextBox();
            this.txt_X = new System.Windows.Forms.TextBox();
            this.txt_G = new System.Windows.Forms.TextBox();
            this.btn_StartPrint = new System.Windows.Forms.Button();
            this.btn_OpenFile = new System.Windows.Forms.Button();
            this.dgv_Gcode = new System.Windows.Forms.DataGridView();
            this.gbx_VelocitySet = new System.Windows.Forms.GroupBox();
            this.txt_Dec = new System.Windows.Forms.TextBox();
            this.txt_EDec = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Acc = new System.Windows.Forms.TextBox();
            this.txt_EAcc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_SetVel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.txt_VelLow = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_EVelLow = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.txt_VelHigh = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_EVelHigh = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.gbx_MultipleAxisControl = new System.Windows.Forms.GroupBox();
            this.nud_MultipleMovePosition3 = new System.Windows.Forms.NumericUpDown();
            this.nud_MultipleMovePosition2 = new System.Windows.Forms.NumericUpDown();
            this.nud_MultipleMovePosition1 = new System.Windows.Forms.NumericUpDown();
            this.nud_MultipleMovePosition0 = new System.Windows.Forms.NumericUpDown();
            this.textBoxEnd = new System.Windows.Forms.TextBox();
            this.ckb_Reverse = new System.Windows.Forms.CheckBox();
            this.lbl_CtrlE = new System.Windows.Forms.Label();
            this.lbl_CtrlZ = new System.Windows.Forms.Label();
            this.lbl_CtrlY = new System.Windows.Forms.Label();
            this.lbl_CtrlX = new System.Windows.Forms.Label();
            this.btn_AxisMultipleStop = new System.Windows.Forms.Button();
            this.cbx_AxisOpen3 = new System.Windows.Forms.TextBox();
            this.cbx_AxisOpen2 = new System.Windows.Forms.TextBox();
            this.cbx_AxisOpen1 = new System.Windows.Forms.TextBox();
            this.cbx_AxisOpen0 = new System.Windows.Forms.TextBox();
            this.btn_AxisMultipleMove = new System.Windows.Forms.Button();
            this.gbx_MoveWay = new System.Windows.Forms.GroupBox();
            this.rbtn_Relatively = new System.Windows.Forms.RadioButton();
            this.rbtn_Asolute = new System.Windows.Forms.RadioButton();
            this.gbx_MultipleAxisState = new System.Windows.Forms.GroupBox();
            this.lbl_StateE = new System.Windows.Forms.Label();
            this.txt_StateE = new System.Windows.Forms.TextBox();
            this.lbl_StateZ = new System.Windows.Forms.Label();
            this.txt_PositionE = new System.Windows.Forms.TextBox();
            this.lbl_StateY = new System.Windows.Forms.Label();
            this.txt_StateZ = new System.Windows.Forms.TextBox();
            this.lbl_StateX = new System.Windows.Forms.Label();
            this.txt_PositionZ = new System.Windows.Forms.TextBox();
            this.txt_StateY = new System.Windows.Forms.TextBox();
            this.txt_PositionY = new System.Windows.Forms.TextBox();
            this.txt_StateX = new System.Windows.Forms.TextBox();
            this.txt_PositionX = new System.Windows.Forms.TextBox();
            this.gbx_AxisState = new System.Windows.Forms.GroupBox();
            this.tbx_AxisState = new System.Windows.Forms.TextBox();
            this.tbx_AisxPosition = new System.Windows.Forms.TextBox();
            this.lbl_AxisState = new System.Windows.Forms.Label();
            this.gbx_SingleAxisControl = new System.Windows.Forms.GroupBox();
            this.btn_AxisStop = new System.Windows.Forms.Button();
            this.tbx_MovePosition = new System.Windows.Forms.TextBox();
            this.btn_AxisMove = new System.Windows.Forms.Button();
            this.cbx_AxisSelect = new System.Windows.Forms.ComboBox();
            this.gbx_DeviceConnect = new System.Windows.Forms.GroupBox();
            this.btn_ServoControl = new System.Windows.Forms.Button();
            this.btn_LoadConfig = new System.Windows.Forms.Button();
            this.btn_CloseDevice = new System.Windows.Forms.Button();
            this.btn_OpenDevice = new System.Windows.Forms.Button();
            this.cbx_DeviceSelect = new System.Windows.Forms.ComboBox();
            this.tp1_Heating = new System.Windows.Forms.TabPage();
            this.lbl_USBState2 = new System.Windows.Forms.Label();
            this.lbl_USBState = new System.Windows.Forms.Label();
            this.lb_USBHeating = new System.Windows.Forms.ListBox();
            this.btn_HeatingTrigger = new System.Windows.Forms.Button();
            this.tp1_HlaconTest = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.time_State = new System.Windows.Forms.Timer(this.components);
            this.ofd_LoadConfigFile = new System.Windows.Forms.OpenFileDialog();
            this.ofd_Gcode = new System.Windows.Forms.OpenFileDialog();
            this.time_GcodeRead = new System.Windows.Forms.Timer(this.components);
            this.instantDoCtrl1 = new Automation.BDaq.InstantDoCtrl(this.components);
            this.instantAiCtrl1 = new Automation.BDaq.InstantAiCtrl(this.components);
            this.timer_USB = new System.Windows.Forms.Timer(this.components);
            this.tcl_Window00.SuspendLayout();
            this.tp1_Test.SuspendLayout();
            this.gbx_Gcode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Gcode)).BeginInit();
            this.gbx_VelocitySet.SuspendLayout();
            this.gbx_MultipleAxisControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MultipleMovePosition3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MultipleMovePosition2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MultipleMovePosition1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MultipleMovePosition0)).BeginInit();
            this.gbx_MoveWay.SuspendLayout();
            this.gbx_MultipleAxisState.SuspendLayout();
            this.gbx_AxisState.SuspendLayout();
            this.gbx_SingleAxisControl.SuspendLayout();
            this.gbx_DeviceConnect.SuspendLayout();
            this.tp1_Heating.SuspendLayout();
            this.tp1_HlaconTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcl_Window00
            // 
            this.tcl_Window00.Controls.Add(this.tp1_Test);
            this.tcl_Window00.Controls.Add(this.tp1_Heating);
            this.tcl_Window00.Controls.Add(this.tp1_HlaconTest);
            this.tcl_Window00.Location = new System.Drawing.Point(0, 0);
            this.tcl_Window00.Name = "tcl_Window00";
            this.tcl_Window00.SelectedIndex = 0;
            this.tcl_Window00.Size = new System.Drawing.Size(887, 715);
            this.tcl_Window00.TabIndex = 0;
            // 
            // tp1_Test
            // 
            this.tp1_Test.Controls.Add(this.gbx_Gcode);
            this.tp1_Test.Controls.Add(this.gbx_VelocitySet);
            this.tp1_Test.Controls.Add(this.gbx_MultipleAxisControl);
            this.tp1_Test.Controls.Add(this.gbx_MoveWay);
            this.tp1_Test.Controls.Add(this.gbx_MultipleAxisState);
            this.tp1_Test.Controls.Add(this.gbx_AxisState);
            this.tp1_Test.Controls.Add(this.gbx_SingleAxisControl);
            this.tp1_Test.Controls.Add(this.gbx_DeviceConnect);
            this.tp1_Test.Location = new System.Drawing.Point(4, 22);
            this.tp1_Test.Name = "tp1_Test";
            this.tp1_Test.Size = new System.Drawing.Size(879, 689);
            this.tp1_Test.TabIndex = 0;
            this.tp1_Test.Text = "Test";
            this.tp1_Test.UseVisualStyleBackColor = true;
            // 
            // gbx_Gcode
            // 
            this.gbx_Gcode.Controls.Add(this.txt_Z);
            this.gbx_Gcode.Controls.Add(this.txt_Y);
            this.gbx_Gcode.Controls.Add(this.txt_X);
            this.gbx_Gcode.Controls.Add(this.txt_G);
            this.gbx_Gcode.Controls.Add(this.btn_StartPrint);
            this.gbx_Gcode.Controls.Add(this.btn_OpenFile);
            this.gbx_Gcode.Controls.Add(this.dgv_Gcode);
            this.gbx_Gcode.Location = new System.Drawing.Point(8, 321);
            this.gbx_Gcode.Name = "gbx_Gcode";
            this.gbx_Gcode.Size = new System.Drawing.Size(868, 362);
            this.gbx_Gcode.TabIndex = 7;
            this.gbx_Gcode.TabStop = false;
            this.gbx_Gcode.Text = "Gcode";
            // 
            // txt_Z
            // 
            this.txt_Z.Location = new System.Drawing.Point(6, 152);
            this.txt_Z.Name = "txt_Z";
            this.txt_Z.ReadOnly = true;
            this.txt_Z.Size = new System.Drawing.Size(100, 22);
            this.txt_Z.TabIndex = 10;
            // 
            // txt_Y
            // 
            this.txt_Y.Location = new System.Drawing.Point(6, 124);
            this.txt_Y.Name = "txt_Y";
            this.txt_Y.ReadOnly = true;
            this.txt_Y.Size = new System.Drawing.Size(100, 22);
            this.txt_Y.TabIndex = 10;
            // 
            // txt_X
            // 
            this.txt_X.Location = new System.Drawing.Point(6, 96);
            this.txt_X.Name = "txt_X";
            this.txt_X.ReadOnly = true;
            this.txt_X.Size = new System.Drawing.Size(100, 22);
            this.txt_X.TabIndex = 10;
            // 
            // txt_G
            // 
            this.txt_G.Location = new System.Drawing.Point(6, 68);
            this.txt_G.Name = "txt_G";
            this.txt_G.ReadOnly = true;
            this.txt_G.Size = new System.Drawing.Size(100, 22);
            this.txt_G.TabIndex = 10;
            // 
            // btn_StartPrint
            // 
            this.btn_StartPrint.Location = new System.Drawing.Point(3, 286);
            this.btn_StartPrint.Name = "btn_StartPrint";
            this.btn_StartPrint.Size = new System.Drawing.Size(197, 70);
            this.btn_StartPrint.TabIndex = 9;
            this.btn_StartPrint.Text = "StartPrint";
            this.btn_StartPrint.UseVisualStyleBackColor = true;
            this.btn_StartPrint.Click += new System.EventHandler(this.btn_StartPrint_Click);
            // 
            // btn_OpenFile
            // 
            this.btn_OpenFile.Location = new System.Drawing.Point(3, 18);
            this.btn_OpenFile.Name = "btn_OpenFile";
            this.btn_OpenFile.Size = new System.Drawing.Size(197, 44);
            this.btn_OpenFile.TabIndex = 9;
            this.btn_OpenFile.Text = "OpenFile";
            this.btn_OpenFile.UseVisualStyleBackColor = true;
            this.btn_OpenFile.Click += new System.EventHandler(this.btn_OpenFile_Click);
            // 
            // dgv_Gcode
            // 
            this.dgv_Gcode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Gcode.Location = new System.Drawing.Point(206, 21);
            this.dgv_Gcode.Name = "dgv_Gcode";
            this.dgv_Gcode.RowTemplate.Height = 24;
            this.dgv_Gcode.Size = new System.Drawing.Size(656, 335);
            this.dgv_Gcode.TabIndex = 8;
            // 
            // gbx_VelocitySet
            // 
            this.gbx_VelocitySet.Controls.Add(this.txt_Dec);
            this.gbx_VelocitySet.Controls.Add(this.txt_EDec);
            this.gbx_VelocitySet.Controls.Add(this.label1);
            this.gbx_VelocitySet.Controls.Add(this.txt_Acc);
            this.gbx_VelocitySet.Controls.Add(this.txt_EAcc);
            this.gbx_VelocitySet.Controls.Add(this.label2);
            this.gbx_VelocitySet.Controls.Add(this.btn_SetVel);
            this.gbx_VelocitySet.Controls.Add(this.label6);
            this.gbx_VelocitySet.Controls.Add(this.label38);
            this.gbx_VelocitySet.Controls.Add(this.txt_VelLow);
            this.gbx_VelocitySet.Controls.Add(this.label5);
            this.gbx_VelocitySet.Controls.Add(this.txt_EVelLow);
            this.gbx_VelocitySet.Controls.Add(this.label4);
            this.gbx_VelocitySet.Controls.Add(this.label37);
            this.gbx_VelocitySet.Controls.Add(this.txt_VelHigh);
            this.gbx_VelocitySet.Controls.Add(this.label9);
            this.gbx_VelocitySet.Controls.Add(this.label3);
            this.gbx_VelocitySet.Controls.Add(this.txt_EVelHigh);
            this.gbx_VelocitySet.Controls.Add(this.label36);
            this.gbx_VelocitySet.Location = new System.Drawing.Point(444, 109);
            this.gbx_VelocitySet.Name = "gbx_VelocitySet";
            this.gbx_VelocitySet.Size = new System.Drawing.Size(224, 200);
            this.gbx_VelocitySet.TabIndex = 6;
            this.gbx_VelocitySet.TabStop = false;
            this.gbx_VelocitySet.Text = "VelocitySet";
            // 
            // txt_Dec
            // 
            this.txt_Dec.Location = new System.Drawing.Point(145, 63);
            this.txt_Dec.Name = "txt_Dec";
            this.txt_Dec.Size = new System.Drawing.Size(53, 22);
            this.txt_Dec.TabIndex = 33;
            this.txt_Dec.Text = "180000";
            // 
            // txt_EDec
            // 
            this.txt_EDec.Location = new System.Drawing.Point(145, 131);
            this.txt_EDec.Name = "txt_EDec";
            this.txt_EDec.Size = new System.Drawing.Size(53, 22);
            this.txt_EDec.TabIndex = 33;
            this.txt_EDec.Text = "500";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "E";
            // 
            // txt_Acc
            // 
            this.txt_Acc.Location = new System.Drawing.Point(48, 63);
            this.txt_Acc.Name = "txt_Acc";
            this.txt_Acc.Size = new System.Drawing.Size(57, 22);
            this.txt_Acc.TabIndex = 32;
            this.txt_Acc.Text = "160000";
            // 
            // txt_EAcc
            // 
            this.txt_EAcc.Location = new System.Drawing.Point(48, 131);
            this.txt_EAcc.Name = "txt_EAcc";
            this.txt_EAcc.Size = new System.Drawing.Size(57, 22);
            this.txt_EAcc.TabIndex = 32;
            this.txt_EAcc.Text = "500";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "XYZ";
            // 
            // btn_SetVel
            // 
            this.btn_SetVel.Location = new System.Drawing.Point(6, 160);
            this.btn_SetVel.Name = "btn_SetVel";
            this.btn_SetVel.Size = new System.Drawing.Size(203, 32);
            this.btn_SetVel.TabIndex = 4;
            this.btn_SetVel.Text = "SET";
            this.btn_SetVel.UseVisualStyleBackColor = true;
            this.btn_SetVel.Click += new System.EventHandler(this.btn_SetVel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(110, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 12);
            this.label6.TabIndex = 31;
            this.label6.Text = "Dec:";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(110, 134);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(26, 12);
            this.label38.TabIndex = 31;
            this.label38.Text = "Dec:";
            // 
            // txt_VelLow
            // 
            this.txt_VelLow.Location = new System.Drawing.Point(145, 38);
            this.txt_VelLow.Name = "txt_VelLow";
            this.txt_VelLow.Size = new System.Drawing.Size(53, 22);
            this.txt_VelLow.TabIndex = 29;
            this.txt_VelLow.Text = "1000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 12);
            this.label5.TabIndex = 30;
            this.label5.Text = "Acc:";
            // 
            // txt_EVelLow
            // 
            this.txt_EVelLow.Location = new System.Drawing.Point(145, 106);
            this.txt_EVelLow.Name = "txt_EVelLow";
            this.txt_EVelLow.Size = new System.Drawing.Size(53, 22);
            this.txt_EVelLow.TabIndex = 29;
            this.txt_EVelLow.Text = "500";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(6, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "VelH:";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(14, 134);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(26, 12);
            this.label37.TabIndex = 30;
            this.label37.Text = "Acc:";
            // 
            // txt_VelHigh
            // 
            this.txt_VelHigh.Location = new System.Drawing.Point(48, 38);
            this.txt_VelHigh.Name = "txt_VelHigh";
            this.txt_VelHigh.Size = new System.Drawing.Size(57, 22);
            this.txt_VelHigh.TabIndex = 27;
            this.txt_VelHigh.Text = "18000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(6, 111);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 12);
            this.label9.TabIndex = 26;
            this.label9.Text = "VelH:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(110, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 12);
            this.label3.TabIndex = 28;
            this.label3.Text = "VelL:";
            // 
            // txt_EVelHigh
            // 
            this.txt_EVelHigh.Location = new System.Drawing.Point(48, 106);
            this.txt_EVelHigh.Name = "txt_EVelHigh";
            this.txt_EVelHigh.Size = new System.Drawing.Size(57, 22);
            this.txt_EVelHigh.TabIndex = 27;
            this.txt_EVelHigh.Text = "500";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(110, 111);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(31, 12);
            this.label36.TabIndex = 28;
            this.label36.Text = "VelL:";
            // 
            // gbx_MultipleAxisControl
            // 
            this.gbx_MultipleAxisControl.Controls.Add(this.nud_MultipleMovePosition3);
            this.gbx_MultipleAxisControl.Controls.Add(this.nud_MultipleMovePosition2);
            this.gbx_MultipleAxisControl.Controls.Add(this.nud_MultipleMovePosition1);
            this.gbx_MultipleAxisControl.Controls.Add(this.nud_MultipleMovePosition0);
            this.gbx_MultipleAxisControl.Controls.Add(this.textBoxEnd);
            this.gbx_MultipleAxisControl.Controls.Add(this.ckb_Reverse);
            this.gbx_MultipleAxisControl.Controls.Add(this.lbl_CtrlE);
            this.gbx_MultipleAxisControl.Controls.Add(this.lbl_CtrlZ);
            this.gbx_MultipleAxisControl.Controls.Add(this.lbl_CtrlY);
            this.gbx_MultipleAxisControl.Controls.Add(this.lbl_CtrlX);
            this.gbx_MultipleAxisControl.Controls.Add(this.btn_AxisMultipleStop);
            this.gbx_MultipleAxisControl.Controls.Add(this.cbx_AxisOpen3);
            this.gbx_MultipleAxisControl.Controls.Add(this.cbx_AxisOpen2);
            this.gbx_MultipleAxisControl.Controls.Add(this.cbx_AxisOpen1);
            this.gbx_MultipleAxisControl.Controls.Add(this.cbx_AxisOpen0);
            this.gbx_MultipleAxisControl.Controls.Add(this.btn_AxisMultipleMove);
            this.gbx_MultipleAxisControl.Location = new System.Drawing.Point(214, 109);
            this.gbx_MultipleAxisControl.Name = "gbx_MultipleAxisControl";
            this.gbx_MultipleAxisControl.Size = new System.Drawing.Size(224, 206);
            this.gbx_MultipleAxisControl.TabIndex = 6;
            this.gbx_MultipleAxisControl.TabStop = false;
            this.gbx_MultipleAxisControl.Text = "MultipleAxisControl";
            // 
            // nud_MultipleMovePosition3
            // 
            this.nud_MultipleMovePosition3.Location = new System.Drawing.Point(6, 99);
            this.nud_MultipleMovePosition3.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_MultipleMovePosition3.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_MultipleMovePosition3.Name = "nud_MultipleMovePosition3";
            this.nud_MultipleMovePosition3.Size = new System.Drawing.Size(100, 22);
            this.nud_MultipleMovePosition3.TabIndex = 7;
            this.nud_MultipleMovePosition3.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // nud_MultipleMovePosition2
            // 
            this.nud_MultipleMovePosition2.Location = new System.Drawing.Point(6, 73);
            this.nud_MultipleMovePosition2.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_MultipleMovePosition2.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_MultipleMovePosition2.Name = "nud_MultipleMovePosition2";
            this.nud_MultipleMovePosition2.Size = new System.Drawing.Size(100, 22);
            this.nud_MultipleMovePosition2.TabIndex = 7;
            this.nud_MultipleMovePosition2.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // nud_MultipleMovePosition1
            // 
            this.nud_MultipleMovePosition1.Location = new System.Drawing.Point(6, 47);
            this.nud_MultipleMovePosition1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_MultipleMovePosition1.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_MultipleMovePosition1.Name = "nud_MultipleMovePosition1";
            this.nud_MultipleMovePosition1.Size = new System.Drawing.Size(100, 22);
            this.nud_MultipleMovePosition1.TabIndex = 7;
            this.nud_MultipleMovePosition1.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // nud_MultipleMovePosition0
            // 
            this.nud_MultipleMovePosition0.Location = new System.Drawing.Point(6, 22);
            this.nud_MultipleMovePosition0.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_MultipleMovePosition0.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_MultipleMovePosition0.Name = "nud_MultipleMovePosition0";
            this.nud_MultipleMovePosition0.Size = new System.Drawing.Size(100, 22);
            this.nud_MultipleMovePosition0.TabIndex = 7;
            this.nud_MultipleMovePosition0.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // textBoxEnd
            // 
            this.textBoxEnd.Location = new System.Drawing.Point(177, 131);
            this.textBoxEnd.Name = "textBoxEnd";
            this.textBoxEnd.Size = new System.Drawing.Size(41, 22);
            this.textBoxEnd.TabIndex = 34;
            this.textBoxEnd.Text = "10000";
            // 
            // ckb_Reverse
            // 
            this.ckb_Reverse.AutoSize = true;
            this.ckb_Reverse.Location = new System.Drawing.Point(6, 129);
            this.ckb_Reverse.Name = "ckb_Reverse";
            this.ckb_Reverse.Size = new System.Drawing.Size(61, 16);
            this.ckb_Reverse.TabIndex = 7;
            this.ckb_Reverse.Text = "Reverse";
            this.ckb_Reverse.UseVisualStyleBackColor = true;
            // 
            // lbl_CtrlE
            // 
            this.lbl_CtrlE.AutoSize = true;
            this.lbl_CtrlE.Location = new System.Drawing.Point(200, 102);
            this.lbl_CtrlE.Name = "lbl_CtrlE";
            this.lbl_CtrlE.Size = new System.Drawing.Size(12, 12);
            this.lbl_CtrlE.TabIndex = 7;
            this.lbl_CtrlE.Text = "E";
            // 
            // lbl_CtrlZ
            // 
            this.lbl_CtrlZ.AutoSize = true;
            this.lbl_CtrlZ.Location = new System.Drawing.Point(200, 76);
            this.lbl_CtrlZ.Name = "lbl_CtrlZ";
            this.lbl_CtrlZ.Size = new System.Drawing.Size(12, 12);
            this.lbl_CtrlZ.TabIndex = 7;
            this.lbl_CtrlZ.Text = "Z";
            // 
            // lbl_CtrlY
            // 
            this.lbl_CtrlY.AutoSize = true;
            this.lbl_CtrlY.Location = new System.Drawing.Point(200, 50);
            this.lbl_CtrlY.Name = "lbl_CtrlY";
            this.lbl_CtrlY.Size = new System.Drawing.Size(13, 12);
            this.lbl_CtrlY.TabIndex = 7;
            this.lbl_CtrlY.Text = "Y";
            // 
            // lbl_CtrlX
            // 
            this.lbl_CtrlX.AutoSize = true;
            this.lbl_CtrlX.Location = new System.Drawing.Point(200, 24);
            this.lbl_CtrlX.Name = "lbl_CtrlX";
            this.lbl_CtrlX.Size = new System.Drawing.Size(13, 12);
            this.lbl_CtrlX.TabIndex = 7;
            this.lbl_CtrlX.Text = "X";
            // 
            // btn_AxisMultipleStop
            // 
            this.btn_AxisMultipleStop.Location = new System.Drawing.Point(103, 153);
            this.btn_AxisMultipleStop.Name = "btn_AxisMultipleStop";
            this.btn_AxisMultipleStop.Size = new System.Drawing.Size(115, 47);
            this.btn_AxisMultipleStop.TabIndex = 5;
            this.btn_AxisMultipleStop.Text = "Stop";
            this.btn_AxisMultipleStop.UseVisualStyleBackColor = true;
            this.btn_AxisMultipleStop.Click += new System.EventHandler(this.btn_AxisMultipleStop_Click);
            // 
            // cbx_AxisOpen3
            // 
            this.cbx_AxisOpen3.Location = new System.Drawing.Point(112, 99);
            this.cbx_AxisOpen3.Name = "cbx_AxisOpen3";
            this.cbx_AxisOpen3.ReadOnly = true;
            this.cbx_AxisOpen3.Size = new System.Drawing.Size(82, 22);
            this.cbx_AxisOpen3.TabIndex = 3;
            // 
            // cbx_AxisOpen2
            // 
            this.cbx_AxisOpen2.Location = new System.Drawing.Point(112, 73);
            this.cbx_AxisOpen2.Name = "cbx_AxisOpen2";
            this.cbx_AxisOpen2.ReadOnly = true;
            this.cbx_AxisOpen2.Size = new System.Drawing.Size(82, 22);
            this.cbx_AxisOpen2.TabIndex = 3;
            // 
            // cbx_AxisOpen1
            // 
            this.cbx_AxisOpen1.Location = new System.Drawing.Point(112, 47);
            this.cbx_AxisOpen1.Name = "cbx_AxisOpen1";
            this.cbx_AxisOpen1.ReadOnly = true;
            this.cbx_AxisOpen1.Size = new System.Drawing.Size(82, 22);
            this.cbx_AxisOpen1.TabIndex = 3;
            // 
            // cbx_AxisOpen0
            // 
            this.cbx_AxisOpen0.Location = new System.Drawing.Point(112, 21);
            this.cbx_AxisOpen0.Name = "cbx_AxisOpen0";
            this.cbx_AxisOpen0.ReadOnly = true;
            this.cbx_AxisOpen0.Size = new System.Drawing.Size(82, 22);
            this.cbx_AxisOpen0.TabIndex = 3;
            // 
            // btn_AxisMultipleMove
            // 
            this.btn_AxisMultipleMove.Location = new System.Drawing.Point(6, 153);
            this.btn_AxisMultipleMove.Name = "btn_AxisMultipleMove";
            this.btn_AxisMultipleMove.Size = new System.Drawing.Size(91, 47);
            this.btn_AxisMultipleMove.TabIndex = 4;
            this.btn_AxisMultipleMove.Text = "Move";
            this.btn_AxisMultipleMove.UseVisualStyleBackColor = true;
            this.btn_AxisMultipleMove.Click += new System.EventHandler(this.btn_AxisMultipleMove_Click);
            // 
            // gbx_MoveWay
            // 
            this.gbx_MoveWay.Controls.Add(this.rbtn_Relatively);
            this.gbx_MoveWay.Controls.Add(this.rbtn_Asolute);
            this.gbx_MoveWay.Location = new System.Drawing.Point(8, 109);
            this.gbx_MoveWay.Name = "gbx_MoveWay";
            this.gbx_MoveWay.Size = new System.Drawing.Size(200, 100);
            this.gbx_MoveWay.TabIndex = 5;
            this.gbx_MoveWay.TabStop = false;
            this.gbx_MoveWay.Text = "Move The Way";
            // 
            // rbtn_Relatively
            // 
            this.rbtn_Relatively.AutoSize = true;
            this.rbtn_Relatively.Checked = true;
            this.rbtn_Relatively.Location = new System.Drawing.Point(6, 43);
            this.rbtn_Relatively.Name = "rbtn_Relatively";
            this.rbtn_Relatively.Size = new System.Drawing.Size(100, 16);
            this.rbtn_Relatively.TabIndex = 6;
            this.rbtn_Relatively.TabStop = true;
            this.rbtn_Relatively.Text = "Relatively Move";
            this.rbtn_Relatively.UseVisualStyleBackColor = true;
            // 
            // rbtn_Asolute
            // 
            this.rbtn_Asolute.AutoSize = true;
            this.rbtn_Asolute.Location = new System.Drawing.Point(6, 21);
            this.rbtn_Asolute.Name = "rbtn_Asolute";
            this.rbtn_Asolute.Size = new System.Drawing.Size(88, 16);
            this.rbtn_Asolute.TabIndex = 6;
            this.rbtn_Asolute.Text = "Asolute Move";
            this.rbtn_Asolute.UseVisualStyleBackColor = true;
            this.rbtn_Asolute.CheckedChanged += new System.EventHandler(this.rbtn_Asolute_CheckedChanged);
            // 
            // gbx_MultipleAxisState
            // 
            this.gbx_MultipleAxisState.Controls.Add(this.lbl_StateE);
            this.gbx_MultipleAxisState.Controls.Add(this.txt_StateE);
            this.gbx_MultipleAxisState.Controls.Add(this.lbl_StateZ);
            this.gbx_MultipleAxisState.Controls.Add(this.txt_PositionE);
            this.gbx_MultipleAxisState.Controls.Add(this.lbl_StateY);
            this.gbx_MultipleAxisState.Controls.Add(this.txt_StateZ);
            this.gbx_MultipleAxisState.Controls.Add(this.lbl_StateX);
            this.gbx_MultipleAxisState.Controls.Add(this.txt_PositionZ);
            this.gbx_MultipleAxisState.Controls.Add(this.txt_StateY);
            this.gbx_MultipleAxisState.Controls.Add(this.txt_PositionY);
            this.gbx_MultipleAxisState.Controls.Add(this.txt_StateX);
            this.gbx_MultipleAxisState.Controls.Add(this.txt_PositionX);
            this.gbx_MultipleAxisState.Location = new System.Drawing.Point(332, 3);
            this.gbx_MultipleAxisState.Name = "gbx_MultipleAxisState";
            this.gbx_MultipleAxisState.Size = new System.Drawing.Size(436, 100);
            this.gbx_MultipleAxisState.TabIndex = 1;
            this.gbx_MultipleAxisState.TabStop = false;
            this.gbx_MultipleAxisState.Text = "AxisState";
            // 
            // lbl_StateE
            // 
            this.lbl_StateE.AutoSize = true;
            this.lbl_StateE.Location = new System.Drawing.Point(370, 24);
            this.lbl_StateE.Name = "lbl_StateE";
            this.lbl_StateE.Size = new System.Drawing.Size(12, 12);
            this.lbl_StateE.TabIndex = 7;
            this.lbl_StateE.Text = "E";
            // 
            // txt_StateE
            // 
            this.txt_StateE.Location = new System.Drawing.Point(327, 70);
            this.txt_StateE.Name = "txt_StateE";
            this.txt_StateE.ReadOnly = true;
            this.txt_StateE.Size = new System.Drawing.Size(100, 22);
            this.txt_StateE.TabIndex = 3;
            // 
            // lbl_StateZ
            // 
            this.lbl_StateZ.AutoSize = true;
            this.lbl_StateZ.Location = new System.Drawing.Point(263, 24);
            this.lbl_StateZ.Name = "lbl_StateZ";
            this.lbl_StateZ.Size = new System.Drawing.Size(12, 12);
            this.lbl_StateZ.TabIndex = 7;
            this.lbl_StateZ.Text = "Z";
            // 
            // txt_PositionE
            // 
            this.txt_PositionE.Location = new System.Drawing.Point(327, 42);
            this.txt_PositionE.Name = "txt_PositionE";
            this.txt_PositionE.ReadOnly = true;
            this.txt_PositionE.Size = new System.Drawing.Size(100, 22);
            this.txt_PositionE.TabIndex = 3;
            // 
            // lbl_StateY
            // 
            this.lbl_StateY.AutoSize = true;
            this.lbl_StateY.Location = new System.Drawing.Point(158, 24);
            this.lbl_StateY.Name = "lbl_StateY";
            this.lbl_StateY.Size = new System.Drawing.Size(13, 12);
            this.lbl_StateY.TabIndex = 7;
            this.lbl_StateY.Text = "Y";
            // 
            // txt_StateZ
            // 
            this.txt_StateZ.Location = new System.Drawing.Point(221, 70);
            this.txt_StateZ.Name = "txt_StateZ";
            this.txt_StateZ.ReadOnly = true;
            this.txt_StateZ.Size = new System.Drawing.Size(100, 22);
            this.txt_StateZ.TabIndex = 3;
            // 
            // lbl_StateX
            // 
            this.lbl_StateX.AutoSize = true;
            this.lbl_StateX.Location = new System.Drawing.Point(50, 24);
            this.lbl_StateX.Name = "lbl_StateX";
            this.lbl_StateX.Size = new System.Drawing.Size(13, 12);
            this.lbl_StateX.TabIndex = 7;
            this.lbl_StateX.Text = "X";
            // 
            // txt_PositionZ
            // 
            this.txt_PositionZ.Location = new System.Drawing.Point(221, 42);
            this.txt_PositionZ.Name = "txt_PositionZ";
            this.txt_PositionZ.ReadOnly = true;
            this.txt_PositionZ.Size = new System.Drawing.Size(100, 22);
            this.txt_PositionZ.TabIndex = 3;
            // 
            // txt_StateY
            // 
            this.txt_StateY.Location = new System.Drawing.Point(115, 70);
            this.txt_StateY.Name = "txt_StateY";
            this.txt_StateY.ReadOnly = true;
            this.txt_StateY.Size = new System.Drawing.Size(100, 22);
            this.txt_StateY.TabIndex = 3;
            // 
            // txt_PositionY
            // 
            this.txt_PositionY.Location = new System.Drawing.Point(115, 42);
            this.txt_PositionY.Name = "txt_PositionY";
            this.txt_PositionY.ReadOnly = true;
            this.txt_PositionY.Size = new System.Drawing.Size(100, 22);
            this.txt_PositionY.TabIndex = 3;
            // 
            // txt_StateX
            // 
            this.txt_StateX.Location = new System.Drawing.Point(9, 70);
            this.txt_StateX.Name = "txt_StateX";
            this.txt_StateX.ReadOnly = true;
            this.txt_StateX.Size = new System.Drawing.Size(100, 22);
            this.txt_StateX.TabIndex = 3;
            // 
            // txt_PositionX
            // 
            this.txt_PositionX.Location = new System.Drawing.Point(9, 42);
            this.txt_PositionX.Name = "txt_PositionX";
            this.txt_PositionX.ReadOnly = true;
            this.txt_PositionX.Size = new System.Drawing.Size(100, 22);
            this.txt_PositionX.TabIndex = 3;
            // 
            // gbx_AxisState
            // 
            this.gbx_AxisState.Controls.Add(this.tbx_AxisState);
            this.gbx_AxisState.Controls.Add(this.tbx_AisxPosition);
            this.gbx_AxisState.Controls.Add(this.lbl_AxisState);
            this.gbx_AxisState.Location = new System.Drawing.Point(214, 3);
            this.gbx_AxisState.Name = "gbx_AxisState";
            this.gbx_AxisState.Size = new System.Drawing.Size(112, 100);
            this.gbx_AxisState.TabIndex = 1;
            this.gbx_AxisState.TabStop = false;
            this.gbx_AxisState.Text = "AxisState";
            // 
            // tbx_AxisState
            // 
            this.tbx_AxisState.Location = new System.Drawing.Point(6, 70);
            this.tbx_AxisState.Name = "tbx_AxisState";
            this.tbx_AxisState.Size = new System.Drawing.Size(100, 22);
            this.tbx_AxisState.TabIndex = 3;
            // 
            // tbx_AisxPosition
            // 
            this.tbx_AisxPosition.Location = new System.Drawing.Point(6, 42);
            this.tbx_AisxPosition.Name = "tbx_AisxPosition";
            this.tbx_AisxPosition.Size = new System.Drawing.Size(100, 22);
            this.tbx_AisxPosition.TabIndex = 3;
            // 
            // lbl_AxisState
            // 
            this.lbl_AxisState.AutoSize = true;
            this.lbl_AxisState.Location = new System.Drawing.Point(45, 24);
            this.lbl_AxisState.Name = "lbl_AxisState";
            this.lbl_AxisState.Size = new System.Drawing.Size(26, 12);
            this.lbl_AxisState.TabIndex = 7;
            this.lbl_AxisState.Text = "Axis";
            // 
            // gbx_SingleAxisControl
            // 
            this.gbx_SingleAxisControl.Controls.Add(this.btn_AxisStop);
            this.gbx_SingleAxisControl.Controls.Add(this.tbx_MovePosition);
            this.gbx_SingleAxisControl.Controls.Add(this.btn_AxisMove);
            this.gbx_SingleAxisControl.Controls.Add(this.cbx_AxisSelect);
            this.gbx_SingleAxisControl.Location = new System.Drawing.Point(8, 215);
            this.gbx_SingleAxisControl.Name = "gbx_SingleAxisControl";
            this.gbx_SingleAxisControl.Size = new System.Drawing.Size(200, 100);
            this.gbx_SingleAxisControl.TabIndex = 2;
            this.gbx_SingleAxisControl.TabStop = false;
            this.gbx_SingleAxisControl.Text = "SingleAxisControl";
            // 
            // btn_AxisStop
            // 
            this.btn_AxisStop.Location = new System.Drawing.Point(87, 47);
            this.btn_AxisStop.Name = "btn_AxisStop";
            this.btn_AxisStop.Size = new System.Drawing.Size(75, 23);
            this.btn_AxisStop.TabIndex = 5;
            this.btn_AxisStop.Text = "AxisStop";
            this.btn_AxisStop.UseVisualStyleBackColor = true;
            this.btn_AxisStop.Click += new System.EventHandler(this.btn_AxisStop_Click);
            // 
            // tbx_MovePosition
            // 
            this.tbx_MovePosition.Location = new System.Drawing.Point(6, 21);
            this.tbx_MovePosition.Name = "tbx_MovePosition";
            this.tbx_MovePosition.Size = new System.Drawing.Size(100, 22);
            this.tbx_MovePosition.TabIndex = 3;
            this.tbx_MovePosition.Text = "10000";
            // 
            // btn_AxisMove
            // 
            this.btn_AxisMove.Location = new System.Drawing.Point(6, 47);
            this.btn_AxisMove.Name = "btn_AxisMove";
            this.btn_AxisMove.Size = new System.Drawing.Size(75, 23);
            this.btn_AxisMove.TabIndex = 4;
            this.btn_AxisMove.Text = "AxisMove";
            this.btn_AxisMove.UseVisualStyleBackColor = true;
            this.btn_AxisMove.Click += new System.EventHandler(this.btn_AxisMove_Click);
            // 
            // cbx_AxisSelect
            // 
            this.cbx_AxisSelect.FormattingEnabled = true;
            this.cbx_AxisSelect.Location = new System.Drawing.Point(112, 21);
            this.cbx_AxisSelect.Name = "cbx_AxisSelect";
            this.cbx_AxisSelect.Size = new System.Drawing.Size(82, 20);
            this.cbx_AxisSelect.TabIndex = 3;
            // 
            // gbx_DeviceConnect
            // 
            this.gbx_DeviceConnect.Controls.Add(this.btn_ServoControl);
            this.gbx_DeviceConnect.Controls.Add(this.btn_LoadConfig);
            this.gbx_DeviceConnect.Controls.Add(this.btn_CloseDevice);
            this.gbx_DeviceConnect.Controls.Add(this.btn_OpenDevice);
            this.gbx_DeviceConnect.Controls.Add(this.cbx_DeviceSelect);
            this.gbx_DeviceConnect.Location = new System.Drawing.Point(8, 3);
            this.gbx_DeviceConnect.Name = "gbx_DeviceConnect";
            this.gbx_DeviceConnect.Size = new System.Drawing.Size(200, 100);
            this.gbx_DeviceConnect.TabIndex = 1;
            this.gbx_DeviceConnect.TabStop = false;
            this.gbx_DeviceConnect.Text = "Connection";
            // 
            // btn_ServoControl
            // 
            this.btn_ServoControl.Location = new System.Drawing.Point(87, 71);
            this.btn_ServoControl.Name = "btn_ServoControl";
            this.btn_ServoControl.Size = new System.Drawing.Size(75, 23);
            this.btn_ServoControl.TabIndex = 1;
            this.btn_ServoControl.Text = "Servo On";
            this.btn_ServoControl.UseVisualStyleBackColor = true;
            this.btn_ServoControl.Click += new System.EventHandler(this.btn_ServoOn_Click);
            // 
            // btn_LoadConfig
            // 
            this.btn_LoadConfig.Location = new System.Drawing.Point(6, 71);
            this.btn_LoadConfig.Name = "btn_LoadConfig";
            this.btn_LoadConfig.Size = new System.Drawing.Size(75, 23);
            this.btn_LoadConfig.TabIndex = 4;
            this.btn_LoadConfig.Text = "LoadConfig";
            this.btn_LoadConfig.UseVisualStyleBackColor = true;
            this.btn_LoadConfig.Click += new System.EventHandler(this.btn_LoadConfig_Click);
            // 
            // btn_CloseDevice
            // 
            this.btn_CloseDevice.Location = new System.Drawing.Point(87, 47);
            this.btn_CloseDevice.Name = "btn_CloseDevice";
            this.btn_CloseDevice.Size = new System.Drawing.Size(75, 23);
            this.btn_CloseDevice.TabIndex = 3;
            this.btn_CloseDevice.Text = "CloseDevice";
            this.btn_CloseDevice.UseVisualStyleBackColor = true;
            this.btn_CloseDevice.Click += new System.EventHandler(this.btn_CloseDevice_Click);
            // 
            // btn_OpenDevice
            // 
            this.btn_OpenDevice.Location = new System.Drawing.Point(6, 47);
            this.btn_OpenDevice.Name = "btn_OpenDevice";
            this.btn_OpenDevice.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenDevice.TabIndex = 2;
            this.btn_OpenDevice.Text = "OpenDevice";
            this.btn_OpenDevice.UseVisualStyleBackColor = true;
            this.btn_OpenDevice.Click += new System.EventHandler(this.btn_OpenDevice_Click);
            // 
            // cbx_DeviceSelect
            // 
            this.cbx_DeviceSelect.FormattingEnabled = true;
            this.cbx_DeviceSelect.Location = new System.Drawing.Point(73, 21);
            this.cbx_DeviceSelect.Name = "cbx_DeviceSelect";
            this.cbx_DeviceSelect.Size = new System.Drawing.Size(121, 20);
            this.cbx_DeviceSelect.TabIndex = 0;
            // 
            // tp1_Heating
            // 
            this.tp1_Heating.Controls.Add(this.lbl_USBState2);
            this.tp1_Heating.Controls.Add(this.lbl_USBState);
            this.tp1_Heating.Controls.Add(this.lb_USBHeating);
            this.tp1_Heating.Controls.Add(this.btn_HeatingTrigger);
            this.tp1_Heating.Location = new System.Drawing.Point(4, 22);
            this.tp1_Heating.Name = "tp1_Heating";
            this.tp1_Heating.Size = new System.Drawing.Size(879, 689);
            this.tp1_Heating.TabIndex = 2;
            this.tp1_Heating.Text = "Heating";
            this.tp1_Heating.UseVisualStyleBackColor = true;
            // 
            // lbl_USBState2
            // 
            this.lbl_USBState2.AutoSize = true;
            this.lbl_USBState2.Location = new System.Drawing.Point(234, 48);
            this.lbl_USBState2.Name = "lbl_USBState2";
            this.lbl_USBState2.Size = new System.Drawing.Size(39, 12);
            this.lbl_USBState2.TabIndex = 4;
            this.lbl_USBState2.Text = "label35";
            // 
            // lbl_USBState
            // 
            this.lbl_USBState.AutoSize = true;
            this.lbl_USBState.Location = new System.Drawing.Point(234, 18);
            this.lbl_USBState.Name = "lbl_USBState";
            this.lbl_USBState.Size = new System.Drawing.Size(39, 12);
            this.lbl_USBState.TabIndex = 4;
            this.lbl_USBState.Text = "label35";
            // 
            // lb_USBHeating
            // 
            this.lb_USBHeating.FormattingEnabled = true;
            this.lb_USBHeating.ItemHeight = 12;
            this.lb_USBHeating.Location = new System.Drawing.Point(3, 3);
            this.lb_USBHeating.Name = "lb_USBHeating";
            this.lb_USBHeating.Size = new System.Drawing.Size(214, 376);
            this.lb_USBHeating.TabIndex = 1;
            // 
            // btn_HeatingTrigger
            // 
            this.btn_HeatingTrigger.Location = new System.Drawing.Point(236, 101);
            this.btn_HeatingTrigger.Name = "btn_HeatingTrigger";
            this.btn_HeatingTrigger.Size = new System.Drawing.Size(145, 47);
            this.btn_HeatingTrigger.TabIndex = 0;
            this.btn_HeatingTrigger.Text = "HeatingTrigger";
            this.btn_HeatingTrigger.UseVisualStyleBackColor = true;
            this.btn_HeatingTrigger.Click += new System.EventHandler(this.btn_HeatingTrigger_Click);
            // 
            // tp1_HlaconTest
            // 
            this.tp1_HlaconTest.Controls.Add(this.button1);
            this.tp1_HlaconTest.Controls.Add(this.hWindowControl1);
            this.tp1_HlaconTest.Location = new System.Drawing.Point(4, 22);
            this.tp1_HlaconTest.Name = "tp1_HlaconTest";
            this.tp1_HlaconTest.Size = new System.Drawing.Size(879, 689);
            this.tp1_HlaconTest.TabIndex = 1;
            this.tp1_HlaconTest.Text = "Hlacon-Test";
            this.tp1_HlaconTest.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(394, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(336, 85);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(8, 3);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(380, 291);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(380, 291);
            // 
            // time_State
            // 
            this.time_State.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ofd_LoadConfigFile
            // 
            this.ofd_LoadConfigFile.FileName = "Config1";
            // 
            // ofd_Gcode
            // 
            this.ofd_Gcode.FileName = "Config1";
            // 
            // time_GcodeRead
            // 
            this.time_GcodeRead.Tick += new System.EventHandler(this.time_GcodeRead_Tick);
            // 
            // instantDoCtrl1
            // 
            this.instantDoCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("instantDoCtrl1._StateStream")));
            // 
            // instantAiCtrl1
            // 
            this.instantAiCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("instantAiCtrl1._StateStream")));
            // 
            // timer_USB
            // 
            this.timer_USB.Interval = 500;
            this.timer_USB.Tick += new System.EventHandler(this.timer_USB_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 717);
            this.Controls.Add(this.tcl_Window00);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tcl_Window00.ResumeLayout(false);
            this.tp1_Test.ResumeLayout(false);
            this.gbx_Gcode.ResumeLayout(false);
            this.gbx_Gcode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Gcode)).EndInit();
            this.gbx_VelocitySet.ResumeLayout(false);
            this.gbx_VelocitySet.PerformLayout();
            this.gbx_MultipleAxisControl.ResumeLayout(false);
            this.gbx_MultipleAxisControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MultipleMovePosition3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MultipleMovePosition2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MultipleMovePosition1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MultipleMovePosition0)).EndInit();
            this.gbx_MoveWay.ResumeLayout(false);
            this.gbx_MoveWay.PerformLayout();
            this.gbx_MultipleAxisState.ResumeLayout(false);
            this.gbx_MultipleAxisState.PerformLayout();
            this.gbx_AxisState.ResumeLayout(false);
            this.gbx_AxisState.PerformLayout();
            this.gbx_SingleAxisControl.ResumeLayout(false);
            this.gbx_SingleAxisControl.PerformLayout();
            this.gbx_DeviceConnect.ResumeLayout(false);
            this.tp1_Heating.ResumeLayout(false);
            this.tp1_Heating.PerformLayout();
            this.tp1_HlaconTest.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcl_Window00;
        private System.Windows.Forms.TabPage tp1_Test;
        private System.Windows.Forms.GroupBox gbx_DeviceConnect;
        private System.Windows.Forms.ComboBox cbx_DeviceSelect;
        private System.Windows.Forms.Button btn_OpenDevice;
        private System.Windows.Forms.GroupBox gbx_SingleAxisControl;
        private System.Windows.Forms.ComboBox cbx_AxisSelect;
        private System.Windows.Forms.Timer time_State;
        private System.Windows.Forms.GroupBox gbx_AxisState;
        private System.Windows.Forms.TextBox tbx_AxisState;
        private System.Windows.Forms.TextBox tbx_AisxPosition;
        private System.Windows.Forms.Button btn_CloseDevice;
        private System.Windows.Forms.Button btn_LoadConfig;
        private System.Windows.Forms.OpenFileDialog ofd_LoadConfigFile;
        private System.Windows.Forms.Button btn_ServoControl;
        private System.Windows.Forms.Button btn_AxisMove;
        private System.Windows.Forms.GroupBox gbx_MoveWay;
        private System.Windows.Forms.Button btn_AxisStop;
        private System.Windows.Forms.RadioButton rbtn_Relatively;
        private System.Windows.Forms.RadioButton rbtn_Asolute;
        private System.Windows.Forms.TextBox tbx_MovePosition;
        private System.Windows.Forms.GroupBox gbx_MultipleAxisControl;
        private System.Windows.Forms.Button btn_AxisMultipleStop;
        private System.Windows.Forms.Button btn_AxisMultipleMove;
        private System.Windows.Forms.TextBox cbx_AxisOpen3;
        private System.Windows.Forms.TextBox cbx_AxisOpen2;
        private System.Windows.Forms.TextBox cbx_AxisOpen1;
        private System.Windows.Forms.TextBox cbx_AxisOpen0;
        private System.Windows.Forms.GroupBox gbx_MultipleAxisState;
        private System.Windows.Forms.TextBox txt_StateE;
        private System.Windows.Forms.TextBox txt_PositionE;
        private System.Windows.Forms.TextBox txt_StateZ;
        private System.Windows.Forms.TextBox txt_PositionZ;
        private System.Windows.Forms.TextBox txt_StateY;
        private System.Windows.Forms.TextBox txt_PositionY;
        private System.Windows.Forms.TextBox txt_StateX;
        private System.Windows.Forms.TextBox txt_PositionX;
        private System.Windows.Forms.Label lbl_CtrlE;
        private System.Windows.Forms.Label lbl_CtrlZ;
        private System.Windows.Forms.Label lbl_CtrlY;
        private System.Windows.Forms.Label lbl_CtrlX;
        private System.Windows.Forms.Label lbl_StateE;
        private System.Windows.Forms.Label lbl_StateZ;
        private System.Windows.Forms.Label lbl_StateY;
        private System.Windows.Forms.Label lbl_StateX;
        private System.Windows.Forms.Label lbl_AxisState;
        private System.Windows.Forms.CheckBox ckb_Reverse;
        private System.Windows.Forms.GroupBox gbx_VelocitySet;
        private System.Windows.Forms.Button btn_SetVel;
        private System.Windows.Forms.GroupBox gbx_Gcode;
        private System.Windows.Forms.DataGridView dgv_Gcode;
        private System.Windows.Forms.TextBox txt_Z;
        private System.Windows.Forms.TextBox txt_Y;
        private System.Windows.Forms.TextBox txt_X;
        private System.Windows.Forms.TextBox txt_G;
        private System.Windows.Forms.Button btn_StartPrint;
        private System.Windows.Forms.Button btn_OpenFile;
        private System.Windows.Forms.OpenFileDialog ofd_Gcode;
        private System.Windows.Forms.Timer time_GcodeRead;
        private System.Windows.Forms.TabPage tp1_HlaconTest;
        private System.Windows.Forms.Button button1;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.TabPage tp1_Heating;
        private System.Windows.Forms.Label lbl_USBState;
        private System.Windows.Forms.ListBox lb_USBHeating;
        private System.Windows.Forms.Button btn_HeatingTrigger;
        private Automation.BDaq.InstantDoCtrl instantDoCtrl1;
        private Automation.BDaq.InstantAiCtrl instantAiCtrl1;
        private System.Windows.Forms.Timer timer_USB;
        private System.Windows.Forms.Label lbl_USBState2;
        private System.Windows.Forms.TextBox txt_Dec;
        private System.Windows.Forms.TextBox txt_EDec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_Acc;
        private System.Windows.Forms.TextBox txt_EAcc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox txt_VelLow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_EVelLow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txt_VelHigh;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_EVelHigh;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox textBoxEnd;
        private System.Windows.Forms.NumericUpDown nud_MultipleMovePosition3;
        private System.Windows.Forms.NumericUpDown nud_MultipleMovePosition2;
        private System.Windows.Forms.NumericUpDown nud_MultipleMovePosition1;
        private System.Windows.Forms.NumericUpDown nud_MultipleMovePosition0;
    }
}

