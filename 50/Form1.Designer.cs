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
        IntPtr[] m_Axishand = new IntPtr[32];

        double VelHigh = 1000;
        uint deviceCount = 0;
        uint DeviceNum = 0;
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
            this.txt_VelHigh = new System.Windows.Forms.TextBox();
            this.btn_SetVel = new System.Windows.Forms.Button();
            this.gbx_MultipleAxisControl = new System.Windows.Forms.GroupBox();
            this.ckb_Reverse = new System.Windows.Forms.CheckBox();
            this.lbl_CtrlE = new System.Windows.Forms.Label();
            this.lbl_CtrlZ = new System.Windows.Forms.Label();
            this.lbl_CtrlY = new System.Windows.Forms.Label();
            this.lbl_CtrlX = new System.Windows.Forms.Label();
            this.btn_AxisMultipleStop = new System.Windows.Forms.Button();
            this.txt_MultipleMovePosition3 = new System.Windows.Forms.TextBox();
            this.txt_MultipleMovePosition2 = new System.Windows.Forms.TextBox();
            this.txt_MultipleMovePosition1 = new System.Windows.Forms.TextBox();
            this.cbx_AxisOpen3 = new System.Windows.Forms.TextBox();
            this.cbx_AxisOpen2 = new System.Windows.Forms.TextBox();
            this.cbx_AxisOpen1 = new System.Windows.Forms.TextBox();
            this.cbx_AxisOpen0 = new System.Windows.Forms.TextBox();
            this.txt_MultipleMovePosition0 = new System.Windows.Forms.TextBox();
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
            this.time_State = new System.Windows.Forms.Timer(this.components);
            this.ofd_LoadConfigFile = new System.Windows.Forms.OpenFileDialog();
            this.ofd_Gcode = new System.Windows.Forms.OpenFileDialog();
            this.time_GcodeRead = new System.Windows.Forms.Timer(this.components);
            this.time_GcodeSkip = new System.Windows.Forms.Timer(this.components);
            this.tp1_HlaconTest = new System.Windows.Forms.TabPage();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.button1 = new System.Windows.Forms.Button();
            this.tcl_Window00.SuspendLayout();
            this.tp1_Test.SuspendLayout();
            this.gbx_Gcode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Gcode)).BeginInit();
            this.gbx_VelocitySet.SuspendLayout();
            this.gbx_MultipleAxisControl.SuspendLayout();
            this.gbx_MoveWay.SuspendLayout();
            this.gbx_MultipleAxisState.SuspendLayout();
            this.gbx_AxisState.SuspendLayout();
            this.gbx_SingleAxisControl.SuspendLayout();
            this.gbx_DeviceConnect.SuspendLayout();
            this.tp1_HlaconTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcl_Window00
            // 
            this.tcl_Window00.Controls.Add(this.tp1_Test);
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
            this.txt_Z.Size = new System.Drawing.Size(100, 22);
            this.txt_Z.TabIndex = 10;
            // 
            // txt_Y
            // 
            this.txt_Y.Location = new System.Drawing.Point(6, 124);
            this.txt_Y.Name = "txt_Y";
            this.txt_Y.Size = new System.Drawing.Size(100, 22);
            this.txt_Y.TabIndex = 10;
            // 
            // txt_X
            // 
            this.txt_X.Location = new System.Drawing.Point(6, 96);
            this.txt_X.Name = "txt_X";
            this.txt_X.Size = new System.Drawing.Size(100, 22);
            this.txt_X.TabIndex = 10;
            // 
            // txt_G
            // 
            this.txt_G.Location = new System.Drawing.Point(6, 68);
            this.txt_G.Name = "txt_G";
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
            this.gbx_VelocitySet.Controls.Add(this.txt_VelHigh);
            this.gbx_VelocitySet.Controls.Add(this.btn_SetVel);
            this.gbx_VelocitySet.Location = new System.Drawing.Point(444, 109);
            this.gbx_VelocitySet.Name = "gbx_VelocitySet";
            this.gbx_VelocitySet.Size = new System.Drawing.Size(224, 121);
            this.gbx_VelocitySet.TabIndex = 6;
            this.gbx_VelocitySet.TabStop = false;
            this.gbx_VelocitySet.Text = "VelocitySet";
            // 
            // txt_VelHigh
            // 
            this.txt_VelHigh.Location = new System.Drawing.Point(6, 37);
            this.txt_VelHigh.Name = "txt_VelHigh";
            this.txt_VelHigh.Size = new System.Drawing.Size(203, 22);
            this.txt_VelHigh.TabIndex = 3;
            this.txt_VelHigh.Text = "10000";
            // 
            // btn_SetVel
            // 
            this.btn_SetVel.Location = new System.Drawing.Point(6, 82);
            this.btn_SetVel.Name = "btn_SetVel";
            this.btn_SetVel.Size = new System.Drawing.Size(203, 32);
            this.btn_SetVel.TabIndex = 4;
            this.btn_SetVel.Text = "SET";
            this.btn_SetVel.UseVisualStyleBackColor = true;
            this.btn_SetVel.Click += new System.EventHandler(this.btn_SetVel_Click);
            // 
            // gbx_MultipleAxisControl
            // 
            this.gbx_MultipleAxisControl.Controls.Add(this.ckb_Reverse);
            this.gbx_MultipleAxisControl.Controls.Add(this.lbl_CtrlE);
            this.gbx_MultipleAxisControl.Controls.Add(this.lbl_CtrlZ);
            this.gbx_MultipleAxisControl.Controls.Add(this.lbl_CtrlY);
            this.gbx_MultipleAxisControl.Controls.Add(this.lbl_CtrlX);
            this.gbx_MultipleAxisControl.Controls.Add(this.btn_AxisMultipleStop);
            this.gbx_MultipleAxisControl.Controls.Add(this.txt_MultipleMovePosition3);
            this.gbx_MultipleAxisControl.Controls.Add(this.txt_MultipleMovePosition2);
            this.gbx_MultipleAxisControl.Controls.Add(this.txt_MultipleMovePosition1);
            this.gbx_MultipleAxisControl.Controls.Add(this.cbx_AxisOpen3);
            this.gbx_MultipleAxisControl.Controls.Add(this.cbx_AxisOpen2);
            this.gbx_MultipleAxisControl.Controls.Add(this.cbx_AxisOpen1);
            this.gbx_MultipleAxisControl.Controls.Add(this.cbx_AxisOpen0);
            this.gbx_MultipleAxisControl.Controls.Add(this.txt_MultipleMovePosition0);
            this.gbx_MultipleAxisControl.Controls.Add(this.btn_AxisMultipleMove);
            this.gbx_MultipleAxisControl.Location = new System.Drawing.Point(214, 109);
            this.gbx_MultipleAxisControl.Name = "gbx_MultipleAxisControl";
            this.gbx_MultipleAxisControl.Size = new System.Drawing.Size(224, 206);
            this.gbx_MultipleAxisControl.TabIndex = 6;
            this.gbx_MultipleAxisControl.TabStop = false;
            this.gbx_MultipleAxisControl.Text = "MultipleAxisControl";
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
            this.btn_AxisMultipleStop.Location = new System.Drawing.Point(87, 153);
            this.btn_AxisMultipleStop.Name = "btn_AxisMultipleStop";
            this.btn_AxisMultipleStop.Size = new System.Drawing.Size(107, 47);
            this.btn_AxisMultipleStop.TabIndex = 5;
            this.btn_AxisMultipleStop.Text = "Stop";
            this.btn_AxisMultipleStop.UseVisualStyleBackColor = true;
            this.btn_AxisMultipleStop.Click += new System.EventHandler(this.btn_AxisMultipleStop_Click);
            // 
            // txt_MultipleMovePosition3
            // 
            this.txt_MultipleMovePosition3.Location = new System.Drawing.Point(6, 99);
            this.txt_MultipleMovePosition3.Name = "txt_MultipleMovePosition3";
            this.txt_MultipleMovePosition3.Size = new System.Drawing.Size(100, 22);
            this.txt_MultipleMovePosition3.TabIndex = 3;
            this.txt_MultipleMovePosition3.Text = "10000";
            // 
            // txt_MultipleMovePosition2
            // 
            this.txt_MultipleMovePosition2.Location = new System.Drawing.Point(6, 73);
            this.txt_MultipleMovePosition2.Name = "txt_MultipleMovePosition2";
            this.txt_MultipleMovePosition2.Size = new System.Drawing.Size(100, 22);
            this.txt_MultipleMovePosition2.TabIndex = 3;
            this.txt_MultipleMovePosition2.Text = "10000";
            // 
            // txt_MultipleMovePosition1
            // 
            this.txt_MultipleMovePosition1.Location = new System.Drawing.Point(6, 47);
            this.txt_MultipleMovePosition1.Name = "txt_MultipleMovePosition1";
            this.txt_MultipleMovePosition1.Size = new System.Drawing.Size(100, 22);
            this.txt_MultipleMovePosition1.TabIndex = 3;
            this.txt_MultipleMovePosition1.Text = "10000";
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
            // txt_MultipleMovePosition0
            // 
            this.txt_MultipleMovePosition0.Location = new System.Drawing.Point(6, 21);
            this.txt_MultipleMovePosition0.Name = "txt_MultipleMovePosition0";
            this.txt_MultipleMovePosition0.Size = new System.Drawing.Size(100, 22);
            this.txt_MultipleMovePosition0.TabIndex = 3;
            this.txt_MultipleMovePosition0.Text = "10000";
            // 
            // btn_AxisMultipleMove
            // 
            this.btn_AxisMultipleMove.Location = new System.Drawing.Point(6, 153);
            this.btn_AxisMultipleMove.Name = "btn_AxisMultipleMove";
            this.btn_AxisMultipleMove.Size = new System.Drawing.Size(75, 47);
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
            // time_GcodeSkip
            // 
            this.time_GcodeSkip.Tick += new System.EventHandler(this.time_GcodeSkip_Tick);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 717);
            this.Controls.Add(this.tcl_Window00);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tcl_Window00.ResumeLayout(false);
            this.tp1_Test.ResumeLayout(false);
            this.gbx_Gcode.ResumeLayout(false);
            this.gbx_Gcode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Gcode)).EndInit();
            this.gbx_VelocitySet.ResumeLayout(false);
            this.gbx_VelocitySet.PerformLayout();
            this.gbx_MultipleAxisControl.ResumeLayout(false);
            this.gbx_MultipleAxisControl.PerformLayout();
            this.gbx_MoveWay.ResumeLayout(false);
            this.gbx_MoveWay.PerformLayout();
            this.gbx_MultipleAxisState.ResumeLayout(false);
            this.gbx_MultipleAxisState.PerformLayout();
            this.gbx_AxisState.ResumeLayout(false);
            this.gbx_AxisState.PerformLayout();
            this.gbx_SingleAxisControl.ResumeLayout(false);
            this.gbx_SingleAxisControl.PerformLayout();
            this.gbx_DeviceConnect.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox txt_MultipleMovePosition0;
        private System.Windows.Forms.Button btn_AxisMultipleMove;
        private System.Windows.Forms.TextBox txt_MultipleMovePosition3;
        private System.Windows.Forms.TextBox txt_MultipleMovePosition2;
        private System.Windows.Forms.TextBox txt_MultipleMovePosition1;
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
        private System.Windows.Forms.TextBox txt_VelHigh;
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
        private System.Windows.Forms.Timer time_GcodeSkip;
        private System.Windows.Forms.TabPage tp1_HlaconTest;
        private System.Windows.Forms.Button button1;
        private HalconDotNet.HWindowControl hWindowControl1;
    }
}

