using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Advantech.Motion;
using Advantech.MotionComponent;    //研華
using Automation;              //USB-4716

using HalconDotNet; //hlacon

using System.Runtime.InteropServices; //For Marshal
using System.IO; // For commend "File"
using System.Text.RegularExpressions; // For commend "Regex"

namespace _50
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //表示USB-4716是否連線成功
            lbl_USBState.Text = instantDoCtrl1.Initialized.ToString();           //表示USB-4716是否連線成功

            tcl_Window00.Location = this.Location;
            tcl_Window00.Size = this.Size;

            gbx_MoveWay.Enabled = false;
            gbx_SingleAxisControl.Enabled = false;
            gbx_AxisState.Enabled = false;
            gbx_MultipleAxisState.Enabled = false;
            gbx_MultipleAxisControl.Enabled = false;
            gbx_VelocitySet.Enabled = false;
            gbx_Gcode.Enabled = false;

            btn_LoadConfig.Enabled = false;
            btn_ServoControl.Enabled = false;
            

            
        }

        void KeyPosition()
        {
            for(int i = 0; i < m_ulAxisCount - 1; i++)
            {
                switch (i)
                {
                    case 0:
                        try
                        {
                            Key[i] = Convert.ToDouble(nud_MultipleMovePosition0.Text);
                        }
                        catch
                        {
                            nud_MultipleMovePosition0.Value = 0;
                            Key[i] = Convert.ToDouble(nud_MultipleMovePosition0.Value);
                        }
                        break;
                    case 1:
                        try
                        {
                            Key[i] = Convert.ToDouble(nud_MultipleMovePosition1.Text);
                        }
                        catch
                        {
                            nud_MultipleMovePosition1.Value = 0;
                            Key[i] = Convert.ToDouble(nud_MultipleMovePosition1.Value);
                        }
                        break;
                    case 2:
                        try
                        {
                            Key[i] = Convert.ToDouble(nud_MultipleMovePosition2.Text);
                        }
                        catch
                        {
                            nud_MultipleMovePosition2.Value = 0;
                            Key[i] = Convert.ToDouble(nud_MultipleMovePosition2.Value);
                        }
                        break;
                }
            }

            try
            {

                E = Convert.ToDouble(nud_MultipleMovePosition3.Text);
            }
            catch
            {
                nud_MultipleMovePosition3.Value = 0;
                E = Convert.ToDouble(nud_MultipleMovePosition3.Value);
            }

            if (ckb_Reverse.Checked)
            {
                Key[0] = -Key[0];
                Key[1] = -Key[1];
                Key[2] = -Key[2];
                E = -E;
            }

            if (rbtn_Asolute.Checked)
            {                
                Key[0] = Math.Abs(Key[0]);
                Key[1] = Math.Abs(Key[1]);
                Key[2] = Math.Abs(Key[2]);
                E = Math.Abs(E);
            }
        }

        void GroupMove()
        {
            uint Result;
            uint AxisNum = new uint();
            UInt16 State = new UInt16();
            if (m_bInit != true)
            {
                return;
            }

            Result = Motion.mAcm_GpGetState(m_GpHand, ref State);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Gp Get State Failed", "Line", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (State != (UInt16)GroupState.STA_Gp_Ready)
            {
                MessageBox.Show("Gp's State Error", "Line", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double par_GpVelHigh = Vel;
            double par_GpVelLow = VelLow;
            double par_GpAcc = Acc;
            double par_GpDec = Dec;

            Result = Motion.mAcm_SetProperty(m_GpHand, (uint)PropertyID.PAR_GpVelHigh, ref par_GpVelHigh, (uint)Marshal.SizeOf(typeof(double)));
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Line", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Result = Motion.mAcm_SetProperty(m_GpHand, (uint)PropertyID.PAR_GpVelLow, ref par_GpVelLow, (uint)Marshal.SizeOf(typeof(double)));
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Result = Motion.mAcm_SetProperty(m_GpHand, (uint)PropertyID.PAR_GpAcc, ref par_GpAcc, (uint)Marshal.SizeOf(typeof(double)));
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Result = Motion.mAcm_SetProperty(m_GpHand, (uint)PropertyID.PAR_GpDec, ref par_GpDec, (uint)Marshal.SizeOf(typeof(double)));
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AxisNum = AxCountInGp;
            if (rbtn_Relatively.Checked)
            {
                Result = Motion.mAcm_GpMoveLinearRel(m_GpHand, Key, ref AxisNum);
            }
            else
                Result = Motion.mAcm_GpMoveLinearAbs(m_GpHand, Key, ref AxisNum);

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Line", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double par_VelHigh = VelHighE;
            double par_VelLow = VelHighE;
            double par_Acc = VelHighE;
            double par_Dec = VelHighE;


            Result = Motion.mAcm_SetProperty(m_Axishand[3], (uint)PropertyID.PAR_AxVelHigh, ref par_VelHigh, (uint)Marshal.SizeOf(typeof(double)));
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Result = Motion.mAcm_SetProperty(m_Axishand[3], (uint)PropertyID.PAR_AxVelLow, ref par_VelLow, (uint)Marshal.SizeOf(typeof(double)));
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Result = Motion.mAcm_SetProperty(m_Axishand[3], (uint)PropertyID.PAR_AxAcc, ref par_Acc, (uint)Marshal.SizeOf(typeof(double)));
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Result = Motion.mAcm_SetProperty(m_Axishand[3], (uint)PropertyID.PAR_AxDec, ref par_Dec, (uint)Marshal.SizeOf(typeof(double)));
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Etrigger)
                E = 0;

            if (rbtn_Relatively.Checked)
            {
                Result = Motion.mAcm_AxMoveRel(m_Axishand[3], E);
            }
            else
            {
                Result = Motion.mAcm_AxMoveAbs(m_Axishand[3], E);
            }

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("PTP Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            return;
        }

        void MultipleMove(double move0, double move1, double move2, double move3)
        {
            UInt32 Result;
            if (m_bInit)
            {
                //速度差補
                double[] par_VelHigh = new double[4];
                double[] par_VelLow = new double[4];
                double[] par_Acc = new double[4];
                double[] par_Dec = new double[4];
                double x = 0, y = 0, z = 0;
                double x_k = 0, y_k = 0, z_k = 0;

                x_k = move0;
                y_k = move1;
                z_k = move2;

                if (rbtn_Asolute.Checked)                             //絕對移動確認
                {

                    x = Convert.ToDouble(txt_CmdPositionX.Text);                      //抓取XYZ座標
                    y = Convert.ToDouble(txt_CmdPositionY.Text);
                    z = Convert.ToDouble(txt_CmdPositionZ.Text);

                    x_k -= x;
                    y_k -= y;
                    z_k -= z;                         //取得點到點距離                  
                }

                double mid = Math.Pow(x_k, 2) + Math.Pow(y_k, 2) + Math.Pow(z_k, 2);
                double max_distance = Math.Sqrt(mid);                   //移動距離

                par_VelHigh[0] = Vel * Math.Abs(x_k / max_distance);//照移動路徑設定X軸運行速度
                par_VelHigh[1] = Vel * Math.Abs(y_k / max_distance);//照移動路徑設定Y軸運行速度
                par_VelHigh[2] = Vel * Math.Abs(z_k / max_distance);//照移動路徑設定Z軸運行速度
                par_VelHigh[3] = VelHighE;                               //照移動路徑設定E軸運行速度

                par_VelLow[0] = VelLow;
                par_VelLow[1] = VelLow;
                par_VelLow[2] = VelLow;
                par_VelLow[3] = VelLowE;

                par_Acc[0] = Acc;
                par_Acc[1] = Acc;
                par_Acc[2] = Acc;
                par_Acc[3] = AccE;

                par_Dec[0] = Dec;
                par_Dec[1] = Dec;
                par_Dec[2] = Dec;
                par_Dec[3] = DecE;

                for (int i = 0; i < 4; i++)
                {
                    if (par_VelHigh[i] == 0)
                        par_VelHigh[i]++;
                    Result = Motion.mAcm_SetProperty(m_Axishand[i], (uint)PropertyID.PAR_AxVelHigh, ref par_VelHigh[i], (uint)Marshal.SizeOf(typeof(double)));
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Result = Motion.mAcm_SetProperty(m_Axishand[i], (uint)PropertyID.PAR_AxVelLow, ref par_VelLow[i], (uint)Marshal.SizeOf(typeof(double)));
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Result = Motion.mAcm_SetProperty(m_Axishand[i], (uint)PropertyID.PAR_AxAcc, ref par_Acc[i], (uint)Marshal.SizeOf(typeof(double)));
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Result = Motion.mAcm_SetProperty(m_Axishand[i], (uint)PropertyID.PAR_AxDec, ref par_Dec[i], (uint)Marshal.SizeOf(typeof(double)));
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //0
                if (rbtn_Relatively.Checked)
                {
                    Result = Motion.mAcm_AxMoveRel(m_Axishand[0], move0);
                }
                else
                {
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[0], move0);
                }

                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("PTP Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //1
                if (rbtn_Relatively.Checked)
                {
                    Result = Motion.mAcm_AxMoveRel(m_Axishand[1], move1);
                }
                else
                {
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[1], move1);
                }

                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("PTP Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //2
                if (rbtn_Relatively.Checked)
                {
                    Result = Motion.mAcm_AxMoveRel(m_Axishand[2], move2);
                }
                else
                {
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[2], move2);
                }

                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("PTP Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //3
                if (rbtn_Relatively.Checked)
                {
                    Result = Motion.mAcm_AxMoveRel(m_Axishand[3], move3);
                }
                else
                {
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[3], move3);
                }

                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("PTP Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                /*par_VelHigh[0] = VelHigh;       //重置X軸運行速度
                par_VelHigh[1] = VelHigh;       //重置Y軸運行速度
                par_VelHigh[2] = VelHigh;       //重置Z軸運行速度

                for (int i = 0; i < 3; i++)
                {
                    Result = Motion.mAcm_SetProperty(m_Axishand[i], (uint)PropertyID.PAR_AxVelHigh, ref par_VelHigh[i], (uint)Marshal.SizeOf(typeof(double)));
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        MessageBox.Show("Set Property Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Change_V", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }*/
            }
            return;
        }

        //啟動軸卡函式
        void OpenBoard()
        {
            uint Result;
            uint i = 0;
            uint[] slaveDevs = new uint[16];
            uint AxesPerDev = new uint();
            uint AxisNumber;
            uint buffLen = 0;
            //開啟設備並給予其代號
            Result = Motion.mAcm_DevOpen(DeviceNum, ref m_DeviceHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Can Not Open Device", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            buffLen = 4;
            //取得到其設備每一軸狀態值
            Result = Motion.mAcm_GetProperty(m_DeviceHandle, (uint)PropertyID.FT_DevAxesCount, ref AxesPerDev, ref buffLen);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Get Property Error", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            AxisNumber = AxesPerDev;
            buffLen = 64;
            //取得到其設備每一軸狀態值
            Result = Motion.mAcm_GetProperty(m_DeviceHandle, (uint)PropertyID.CFG_DevSlaveDevs, slaveDevs, ref buffLen);
            if (Result == (uint)ErrorCode.SUCCESS)
            {
                i = 0;
                while (slaveDevs[i] != 0)
                {
                    AxisNumber += AxesPerDev;
                    i++;
                }
            }
            m_ulAxisCount = AxisNumber;
            cbx_AxisSelect.Items.Clear();
            for (i = 0; i < m_ulAxisCount; i++)
            {
                //匯入所有軸並給予每一隻軸代號
                Result = Motion.mAcm_AxOpen(m_DeviceHandle, (UInt16)i, ref m_Axishand[i]);
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("Open Axis Failed", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cbx_AxisSelect.Items.Add(String.Format("{0:d}-Axis", i));

                //多軸同動
                switch (i.ToString())
                {
                    case "0":
                        cbx_AxisOpen0.Text = String.Format("{0:d}-Axis", i);
                        break;
                    case "1":
                        cbx_AxisOpen1.Text = String.Format("{0:d}-Axis", i);
                        break;
                    case "2":
                        cbx_AxisOpen2.Text = String.Format("{0:d}-Axis", i);
                        break;
                    case "3":
                        cbx_AxisOpen3.Text = String.Format("{0:d}-Axis", i);
                        break;
                    default:
                        break;
                }
                //將每一軸設定理論位置到0
                double cmdPosition = new double();
                cmdPosition = 0;
                Motion.mAcm_AxSetCmdPosition(m_Axishand[i], cmdPosition);
            }
            cbx_AxisSelect.SelectedIndex = 0;
            m_bInit = true;
            time_State.Enabled = true;
        }

        //關閉軸卡函式
        void CloseBoard()
        {
            UInt16[] usAxisState = new UInt16[32];
            uint AxisNum;
            //Stop Every Axes
            if (m_bInit == true)
            {
                for (AxisNum = 0; AxisNum < m_ulAxisCount; AxisNum++)
                {
                    Motion.mAcm_AxGetState(m_Axishand[AxisNum], ref usAxisState[AxisNum]);
                    //檢查是否軸出錯
                    if (usAxisState[AxisNum] == (uint)AxisState.STA_AX_ERROR_STOP)
                    {
                        Motion.mAcm_AxResetError(m_Axishand[AxisNum]);
                    }
                    //命令軸減速停止
                    Motion.mAcm_AxStopDec(m_Axishand[AxisNum]);
                }
                //洗白軸群組
                Motion.mAcm_GpClose(ref m_GpHand);
                m_GpHand = IntPtr.Zero;
                //Close Axes
                //關閉軸
                for (AxisNum = 0; AxisNum < m_ulAxisCount; AxisNum++)
                {
                    Motion.mAcm_AxClose(ref m_Axishand[AxisNum]);
                }
                m_ulAxisCount = 0;
                //Close Device
                //關閉設備
                Motion.mAcm_DevClose(ref m_DeviceHandle);
                m_DeviceHandle = IntPtr.Zero;

                time_State.Enabled = false;
                m_bInit = false;
                cbx_AxisSelect.Items.Clear();
                cbx_AxisSelect.Text = "";
                tbx_AisxPosition.Clear();
                tbx_AxisState.Clear();
                cbx_AxisOpen0.Clear();
                txt_CmdPositionX.Clear();
                txt_ActPositionX.Clear();
                txt_StateX.Clear();
                cbx_AxisOpen1.Clear();
                txt_CmdPositionY.Clear();
                txt_ActPositionY.Clear();
                txt_StateY.Clear();
                cbx_AxisOpen2.Clear();
                txt_CmdPositionZ.Clear();
                txt_ActPositionZ.Clear();
                txt_StateZ.Clear();
                cbx_AxisOpen3.Clear();
                txt_CmdPositionE.Clear();
                txt_ActPositionE.Clear();
                txt_StateE.Clear();
            }
        }

        //啟動軸卡
        private void btn_OpenDevice_Click(object sender, EventArgs e)
        {
            //UInt32 result;
            //uint uDeviceType = 0;           //軸卡型號代號

            OpenBoard();

            uint Result;
            if (m_bInit != true)
            {
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                AxCountInGp++;
                Result = Motion.mAcm_GpAddAxis(ref m_GpHand, m_Axishand[i]);
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("Add Axis To Group Failed", "Line", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            KeyPosition();

            btn_LoadConfig.Enabled = true;
            btn_ServoControl.Enabled = true;

            /*//E
            uDeviceType = DeviceNum >> 24 & 0xff;               //轉換代碼成型號代號
            BoardID = (DeviceNum >> 12) & (0x00000fff);         //代碼轉換成ID


            mD_ForAxisE.DeviceNumber = uDeviceType;
            mD_ForAxisE.DeviceType = ((DevTypeID)uDeviceType).ToString();              //輸入型號名稱
            mD_ForAxisE.BoardID = BoardID;                    //輸入ID
            a_E.PhyID = (AxID)(3);      //E軸設定
            result = a_E.Open();                              //開啟E軸
            if (result != (uint)ErrorCode.SUCCESS)              //讀取是否成功?
            {
                MessageBox.Show("Error Code[0x" + Convert.ToString(result, 16) + "]:" + (ErrorCode)result, "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }*/
        }

    

           //軸之位置與狀態觀察計時器
        private void timer1_Tick(object sender, EventArgs e)
        {
            double CurCmd = new double();
            double CurAct = new double();
            UInt16 AxState = new UInt16();
            string strTemp = "";
            if (m_bInit)
            {
                lbl_AxisState.Text = cbx_AxisSelect.Text;

                //取得軸之理論位置
                Motion.mAcm_AxGetCmdPosition(m_Axishand[cbx_AxisSelect.SelectedIndex], ref CurCmd);
                tbx_AisxPosition.Text = Convert.ToString(CurCmd);

                //取得軸狀態
                Motion.mAcm_AxGetState(m_Axishand[cbx_AxisSelect.SelectedIndex], ref AxState);
                switch (AxState)
                {
                    case 0:
                        strTemp = "STA_AX_DISABLE";
                        break;
                    case 1:
                        strTemp = "STA_AX_READY";

                        break;
                    case 2:
                        strTemp = "STA_AX_STOPPING";
                        break;
                    case 3:
                        strTemp = "STA_AX_ERROR_STOP";
                        break;
                    case 4:
                        strTemp = "STA_AX_HOMING";
                        break;
                    case 5:
                        strTemp = "STA_AX_PTP_MOT";
                        break;
                    case 6:
                        strTemp = "STA_AX_CONTI_MOT";
                        break;
                    case 7:
                        strTemp = "STA_AX_SYNC_MOT";
                        break;
                    default:
                        break;
                }
                tbx_AxisState.Text = strTemp;

                //多軸
                for (int i = 0; i < m_ulAxisCount; i++)
                {
                    Motion.mAcm_AxGetCmdPosition(m_Axishand[i], ref CurCmd);
                    Motion.mAcm_AxGetActualPosition(m_Axishand[i], ref CurAct);
                    Motion.mAcm_AxGetState(m_Axishand[i], ref AxState);
                    sp[i] = AxState;
                    switch (AxState)
                    {
                        case 0:
                            strTemp = "STA_AX_DISABLE";
                            break;
                        case 1:
                            strTemp = "STA_AX_READY";
                            break;
                        case 2:
                            strTemp = "STA_AX_STOPPING";
                            break;
                        case 3:
                            strTemp = "STA_AX_ERROR_STOP";
                            break;
                        case 4:
                            strTemp = "STA_AX_HOMING";
                            break;
                        case 5:
                            strTemp = "STA_AX_PTP_MOT";
                            break;
                        case 6:
                            strTemp = "STA_AX_CONTI_MOT";
                            break;
                        case 7:
                            strTemp = "STA_AX_SYNC_MOT";
                            break;
                        default:
                            break;
                    }
                    switch (i.ToString())
                    {
                        case "0":
                            txt_CmdPositionX.Text = Convert.ToString(CurCmd);
                            txt_ActPositionX.Text = Convert.ToString(CurAct);
                            txt_StateX.Text = strTemp;
                            break;
                        case "1":
                            txt_CmdPositionY.Text = Convert.ToString(CurCmd);
                            txt_ActPositionY.Text = Convert.ToString(CurAct);
                            txt_StateY.Text = strTemp;
                            break;
                        case "2":
                            txt_CmdPositionZ.Text = Convert.ToString(CurCmd);
                            txt_ActPositionZ.Text = Convert.ToString(CurAct);
                            txt_StateZ.Text = strTemp;
                            break;
                        case "3":
                            txt_CmdPositionE.Text = Convert.ToString(CurCmd);
                            txt_ActPositionE.Text = Convert.ToString(CurAct);
                            txt_StateE.Text = strTemp;
                            break;
                        default:
                            break;
                    }

                    if (sp[0] == 1 && sp[1] == 1 && sp[2] == 1 && sp[3] == 1)
                    {
                        btn_SetVel.Enabled = true;
                        txt_VelHigh.Enabled = true;
                        txt_VelLow.Enabled = true;
                        txt_Acc.Enabled = true;
                        txt_Dec.Enabled = true;
                        txt_EVelHigh.Enabled = true;
                        txt_EVelLow.Enabled = true;
                        txt_EAcc.Enabled = true;
                        txt_EDec.Enabled = true;

                        btn_AxisMove.Enabled = true;
                        btn_AxisMultipleMove.Enabled = true;
                        btn_GoHome.Enabled = true;
                        btn_StartPrint.Enabled = true;
                        btn_SetCmd.Enabled = true;
                    }
                    else
                    {
                        btn_SetVel.Enabled = false;
                        txt_VelHigh.Enabled = false;
                        txt_VelLow.Enabled = false;
                        txt_Acc.Enabled = false;
                        txt_Dec.Enabled = false;
                        txt_EVelHigh.Enabled = false;
                        txt_EVelLow.Enabled = false;
                        txt_EAcc.Enabled = false;
                        txt_EDec.Enabled = false;

                        btn_AxisMove.Enabled = false;
                        btn_AxisMultipleMove.Enabled = false;
                        btn_GoHome.Enabled = false;
                        btn_StartPrint.Enabled = false;
                        btn_SetCmd.Enabled = false;


                    }

                    UInt32 Result;
                    UInt32 IOStatus = new UInt32();
                    Result = Motion.mAcm_AxGetMotionIO(m_Axishand[i], ref IOStatus);
                    if (Result == (uint)ErrorCode.SUCCESS)
                    {
                        switch (i)
                        {
                            case 0:
                                if ((IOStatus & 0x200) > 0)//EZ 
                                {
                                    pbx_EZX.BackColor = Color.Red;
                                }
                                else
                                {
                                    pbx_EZX.BackColor = Color.Gray;
                                }

                                if ((IOStatus & 0x4) > 0)//+EL
                                {
                                    pbx_PosHELX.BackColor = Color.Red;
                                }
                                else
                                {
                                    pbx_PosHELX.BackColor = Color.Gray;
                                }

                                if ((IOStatus & 0x8) > 0)//-EL
                                {
                                    pbx_NegHELX.BackColor = Color.Red;
                                }
                                else
                                {
                                    pbx_NegHELX.BackColor = Color.Gray;
                                }
                                break;
                            case 1:
                                if ((IOStatus & 0x200) > 0)//EZ 
                                {
                                    pbx_EZY.BackColor = Color.Red;
                                }
                                else
                                {
                                    pbx_EZY.BackColor = Color.Gray;
                                }

                                if ((IOStatus & 0x4) > 0)//+EL
                                {
                                    pbx_PosHELY.BackColor = Color.Red;
                                }
                                else
                                {
                                    pbx_PosHELY.BackColor = Color.Gray;
                                }

                                if ((IOStatus & 0x8) > 0)//-EL
                                {
                                    pbx_NegHELY.BackColor = Color.Red;
                                }
                                else
                                {
                                    pbx_NegHELY.BackColor = Color.Gray;
                                }
                                break;
                            case 2:

                                if ((IOStatus & 0x4) > 0)//+EL
                                {
                                    pbx_PosHELZ.BackColor = Color.Red;
                                }
                                else
                                {
                                    pbx_PosHELZ.BackColor = Color.Gray;
                                }

                                if ((IOStatus & 0x8) > 0)//-EL
                                {
                                    pbx_NegHELZ.BackColor = Color.Red;
                                }
                                else
                                {
                                    pbx_NegHELZ.BackColor = Color.Gray;
                                }
                                break;
                        }

                    }


                    /*a_E.GetState(ref Es);                           //取得E軸狀態
                    txt_StateE.Text = ((AxisState)Es).ToString();   //顯示E軸狀態
                    a_E.GetCmdPosition(ref fe);                     //取得E軸座標
                    txt_PositionE.Text = Convert.ToString(fe);            //顯示E軸座標*/
                }
            }
        }

        //關閉軸卡連線
        private void btn_CloseDevice_Click(object sender, EventArgs e)
        {
            CloseBoard();


            gbx_MoveWay.Enabled = false;
            gbx_SingleAxisControl.Enabled = false;
            gbx_AxisState.Enabled = false;
            gbx_MultipleAxisState.Enabled = false;
            gbx_MultipleAxisControl.Enabled = false;
            gbx_VelocitySet.Enabled = false;
            gbx_Gcode.Enabled = false;

            btn_LoadConfig.Enabled = false;
            btn_ServoControl.Enabled = false;
        }

        //匯入軸設定
        private void btn_LoadConfig_Click(object sender, EventArgs e)
        {
            UInt32 Result;
            if (m_bInit != true)
            {
                return;
            }
            this.ofd_LoadConfigFile.FileName = ".cfg";
            if (ofd_LoadConfigFile.ShowDialog() != DialogResult.OK)
                return;
            //讀取軸設定值
            Result = Motion.mAcm_DevLoadConfig(m_DeviceHandle, ofd_LoadConfigFile.FileName);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Load File Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //馬達軸卡啟動開關
        private void btn_ServoOn_Click(object sender, EventArgs e)
        {
            UInt32 AxisNum;
            UInt32 Result;
            //Check the servoOno flag to decide if turn on or turn off the ServoOn output.
            //確認軸卡是否準備就緒
            if (m_bInit != true)
            {
                return;
            }
            if (m_bServoOn == false)
            {
                for (AxisNum = 0; AxisNum < m_ulAxisCount; AxisNum++)
                {
                    //連線每一軸之伺服馬達
                    Result = Motion.mAcm_AxSetSvOn(m_Axishand[AxisNum], 1);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        MessageBox.Show("Servo On Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    m_bServoOn = true;
                    btn_ServoControl.Text = "Servo Off";

                    gbx_MoveWay.Enabled = true;
                    gbx_SingleAxisControl.Enabled = true;
                    gbx_AxisState.Enabled = true;
                    gbx_MultipleAxisState.Enabled = true;
                    gbx_MultipleAxisControl.Enabled = true;
                    gbx_VelocitySet.Enabled = true;
                    gbx_Gcode.Enabled = true;

                    cbx_DeviceSelect.Enabled = false;
                    btn_OpenDevice.Enabled = false;
                    btn_CloseDevice.Enabled = false;
                }
            }
            else
            {
                for (AxisNum = 0; AxisNum < m_ulAxisCount; AxisNum++)
                {
                    //斷線每一軸之伺服馬達
                    Result = Motion.mAcm_AxSetSvOn(m_Axishand[AxisNum], 0);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        MessageBox.Show("Servo Off Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    m_bServoOn = false;
                    btn_ServoControl.Text = "Servo On";

                    gbx_MoveWay.Enabled = false;
                    gbx_SingleAxisControl.Enabled = false;
                    gbx_AxisState.Enabled = false;
                    gbx_MultipleAxisState.Enabled = false;
                    gbx_MultipleAxisControl.Enabled = false;
                    gbx_VelocitySet.Enabled = false;
                    gbx_Gcode.Enabled = false;

                    cbx_DeviceSelect.Enabled = true;
                    btn_OpenDevice.Enabled = true;
                    btn_CloseDevice.Enabled = true;
                }
            }



        }

        private void btn_AxisMove_Click(object sender, EventArgs e)
        {
            UInt32 Result;
            if (m_bInit)
            {
                if (rbtn_Relatively.Checked)
                {
                    Result = Motion.mAcm_AxMoveRel(m_Axishand[cbx_AxisSelect.SelectedIndex], Convert.ToDouble(tbx_MovePosition.Text));
                }
                else
                {
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[cbx_AxisSelect.SelectedIndex], Convert.ToDouble(tbx_MovePosition.Text));
                }

                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("PTP Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            return;
        }

        private void btn_AxisStop_Click(object sender, EventArgs e)
        {
            UInt16 AxState = new UInt16();
            if (m_bInit)
            {
                //if axis is in error state , reset it first. then Stop Axes
                Motion.mAcm_AxGetState(m_Axishand[cbx_AxisSelect.SelectedIndex], ref AxState);
                if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                { Motion.mAcm_AxResetError(m_Axishand[cbx_AxisSelect.SelectedIndex]); }

                Motion.mAcm_AxStopDec(m_Axishand[cbx_AxisSelect.SelectedIndex]);
            }
            return;
        }

        //當按右上的叉叉時的反應或程式強制關閉時
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //關掉加熱器
            timer_USB.Enabled = false;
            DO_port = 0;
            instantDoCtrl1.Write(0, DO_port);

            OpenBoard();

            UInt16[] usAxisState = new UInt16[32];
            UInt32 AxisNum;
            UInt32 Result;
            for (AxisNum = 0; AxisNum < m_ulAxisCount; AxisNum++)
            {
                Motion.mAcm_AxGetState(m_Axishand[AxisNum], ref usAxisState[AxisNum]);
                //檢查是否軸出錯
                if (usAxisState[AxisNum] == (uint)AxisState.STA_AX_ERROR_STOP)
                {
                    Motion.mAcm_AxResetError(m_Axishand[AxisNum]);
                }
                //命令軸減速停止
                Motion.mAcm_AxStopDec(m_Axishand[AxisNum]);
                //斷線每一軸之伺服馬達
                Result = Motion.mAcm_AxSetSvOn(m_Axishand[AxisNum], 0);
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("Servo Off Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m_bServoOn = false;
                btn_ServoControl.Text = "Servo On";
            }

            CloseBoard();
        }

        //多軸移動
        private void btn_AxisMultipleMove_Click(object sender, EventArgs e)
        {
            KeyPosition();
            GroupMove();

            /*
            double move0, move1, move2, move3;
            try
            {
                move0 = Convert.ToDouble(txt_MultipleMovePosition0.Text);
            }
            catch
            {
                txt_MultipleMovePosition0.Text = "0";
                move0 = Convert.ToDouble(txt_MultipleMovePosition0.Text);
            }
            try
            {
                move1 = Convert.ToDouble(txt_MultipleMovePosition1.Text);
            }
            catch
            {
                txt_MultipleMovePosition1.Text = "0";
                move1 = Convert.ToDouble(txt_MultipleMovePosition1.Text);
            }
            try
            {
                move2 = Convert.ToDouble(txt_MultipleMovePosition2.Text);
            }
            catch
            {
                txt_MultipleMovePosition2.Text = "0";
                move2 = Convert.ToDouble(txt_MultipleMovePosition2.Text);
            }
            try
            {
                move3 = Convert.ToDouble(txt_MultipleMovePosition3.Text);
            }
            catch
            {
                txt_MultipleMovePosition3.Text = "0";
                move3 = Convert.ToDouble(txt_MultipleMovePosition3.Text);
            }

            if (ckb_Reverse.Checked)
            {
                move0 = -move0;
                move1 = -move1;
                move2 = -move2;
                move3 = -move3;
            }

            MultipleMove(move0, move1, move2, move3);*/
        }

        private void btn_AxisMultipleStop_Click(object sender, EventArgs e)
        {
            UInt16 AxState = new UInt16();
            if (m_bInit)
            {
                for (int i = 0; i < m_ulAxisCount; i++)
                {
                    //if axis is in error state , reset it first. then Stop Axes
                    Motion.mAcm_AxGetState(m_Axishand[i], ref AxState);
                    if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                    { Motion.mAcm_AxResetError(m_Axishand[i]); }

                    Motion.mAcm_AxStopDec(m_Axishand[i]);
                }
                Motion.mAcm_GpGetState(m_GpHand, ref AxState);
                if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                { Motion.mAcm_GpResetError(m_GpHand); }

                Motion.mAcm_GpStopDec(m_GpHand);
            }
            return;
        }

        private void btn_SetVel_Click(object sender, EventArgs e)
        {
            Vel = Convert.ToDouble(txt_VelHigh.Text);
            VelLow = Convert.ToDouble(txt_VelLow.Text);
            Acc = Convert.ToDouble(txt_Acc.Text);
            Dec = Convert.ToDouble(txt_Dec.Text);

            VelHighE = Convert.ToDouble(txt_EVelHigh.Text);
            VelLowE = Convert.ToDouble(txt_EVelLow.Text);
            AccE = Convert.ToDouble(txt_EAcc.Text);
            DecE = Convert.ToDouble(txt_EDec.Text);
        }

        private void rbtn_Asolute_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_Asolute.Checked)
            {
                ckb_Reverse.Enabled = false;
            }
            else
                ckb_Reverse.Enabled = true;
        }

        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
            ofd_Gcode.Filter = "Gcode file(*.gcode)|*.gcode|Text file(*.txt)|*.txt|All file(*.*)|*.*";//選擇副檔名

            if (ofd_Gcode.ShowDialog() == DialogResult.OK)// 選擇的副檔名符合
            {
                dgv_Gcode.ColumnCount = 0;                 //全部舊的程式碼清除掉
                dgv_Gcode.ColumnCount = 1;
                dgv_Gcode.Columns[0].Name = "Code";

                string name = ofd_Gcode.FileName;//預設檔名

                string[] text = File.ReadAllLines(ofd_Gcode.FileName.ToString());//把存的文件丟到text陣列
                //string data_col;    //= null;//引用類型變量的默認值，無法給 Null 為初始值，在自己經驗中都給予 MinValue 來做初始值，而.NET 2.0 以後，可以透過 Nullable 的方式來做初始值給值方式及檢查


                foreach (string text_line in text)//foreach 和 for 具有相同的目標：在一個區塊內反覆執行。 主要的不同點在於 foreach 不需要處理結束回圈的條件，此外，foreach 主要是設計於找尋全部的的資料
                // foreach 陳述式是用來逐一查看集合，以取得所需的資訊，但是不能用來加入或移除來源集合的項目，以避免無法預期的副作用(不能出現++或--)。如果您必須加入或移除來源集合的項目，請使用 for迴圈。
                {
                    //data_col = text_line.Split(';');//分隔

                    dgv_Gcode.Rows.Add(text_line);
                }
                dgv_Gcode.Columns[0].Width = 1000;

                dgv_Gcode.CurrentCell = dgv_Gcode.Rows[0].Cells[0];//選擇第一行
            }

        }

        private void btn_StartPrint_Click(object sender, EventArgs e)
        {
            if (m_bInit)
            {
                dgv_Gcode.CurrentCell = dgv_Gcode.Rows[0].Cells[0];//選擇第一行

                currentGcode = 0;
                /*axis4.VelHigh = 3;
                axis4.MoveVel(1);

                textBox4.Clear();           //數值淨空
                textBox8.Clear();
                textBox9.Clear();
                textBox10.Clear();
                axis4.SetCmdPosition(0);           //重置相對E軸位置
                axis4.SetActualPosition(0);        //重置絕對E軸位置*/
                time_GcodeRead.Enabled = true;      //列印計時器啟動

            }
        }

        private void time_GcodeRead_Tick(object sender, EventArgs e)
        {
            if (currentGcode != dgv_Gcode.RowCount - 1)
            {
                UInt16 GpState = new UInt16();
                UInt32 result;                                                          //宣告讀取碼對照組
                                                                                        //以區分大小寫形式宣告Gcode讀寫規則
                Regex Gcode = new Regex("[xyzg]-?[0-9]*.?([0-9]­*[0-9]­*[0-9]­*|[0-9]­*[0-9]­*|[0-9]­*)", RegexOptions.IgnoreCase);
                //使用'Gcode'規則讀'dataGridView1'的程式碼到特定規則讀寫字串'm'
                MatchCollection m = Gcode.Matches(dgv_Gcode.CurrentCell.Value.ToString());

                foreach (Match n in m)
                {
                    string G;                              //宣告G變數
                    if (n.Value.StartsWith("G"))                        //判斷是否是G數據
                    {
                        G = n.Value;                                    //確定G數值
                        G = Convert.ToString(n.Value.Remove(0, 1));     //去除數值開頭取距離
                        txt_G.Text = G.ToString();                   //G值輸入到文字欄
                                                                     //  textBox4.Text = Regex.Replace(X, "^[-+]?/d+(/./d+)?$", " ");
                    }
                    string X;                                //宣告X變數
                    if (n.Value.StartsWith("X"))                        //判斷是否是X數據
                    {
                        X = n.Value;                                    //確定X數值
                        X = Convert.ToString(n.Value.Remove(0, 1));     //去除數值開頭取距離
                        txt_X.Text = X.ToString();                   //X值輸入到文字欄
                                                                     //  textBox8.Text = Regex.Replace(X, "^[-+]?/d+(/./d+)?$", " ");
                    }
                    string Y;                               //宣告Y變數
                    if (n.Value.StartsWith("Y"))                        //判斷是否是Y數據
                    {
                        Y = n.Value;                                    //確定Y數值
                        Y = Convert.ToString(n.Value.Remove(0, 1));     //去除數值開頭取距離
                                                                        // textBox9.Text = Regex.Replace(Y, "[^0-9]*\\.[^0-9]­*\\[^0-9]", "");
                        txt_Y.Text = Y.ToString();                   //Y值輸入到文字欄
                    }
                    string Z;                                //宣告Z變數
                    if (n.Value.StartsWith("Z"))                        //判斷是否是Z數據
                    {
                        Z = n.Value;                                    //確定Z數值
                        Z = Convert.ToString(n.Value.Remove(0, 1));     //去除數值開頭取距離
                                                                        // textBox10.Text = Regex.Replace(Z, "[^0-9]*\\.[^0-9]­*\\[^0-9]", "");
                        txt_Z.Text = Z.ToString();                  //Z值輸入到文字欄
                    }
                }


                //move3 = Convert.ToDouble(txt_E.Text);

                /*if (txt_G.Text == "1")
                    result = axis4.MoveVel(1);               //啟動E軸
                else if (txt_G.Text == "0")
                {
                    result = axis4.StopDec();                //E軸停止
                }*/

                if (m_bInit)
                {
                    if (txt_X.Text != "" && txt_Y.Text != "" && txt_Z.Text != "")   //判定有無效
                    {

                        //輸入位置                        
                        Key[0] = Convert.ToDouble(txt_X.Text);
                        Key[1] = Convert.ToDouble(txt_Y.Text);
                        Key[2] = Convert.ToDouble(txt_Z.Text);

                        rbtn_Relatively.Checked = false;
                        rbtn_Asolute.Checked = true;
                        E = 0;

                        result = Motion.mAcm_GpGetState(m_GpHand, ref GpState);
                        if (result != (uint)ErrorCode.SUCCESS)
                        {
                            MessageBox.Show("Gp Get State Failed", "Line", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                       
                        if (GpState == (UInt16)GroupState.STA_Gp_Ready)
                        {
                            if (currentGcode != dgv_Gcode.RowCount - 1)
                            {
                                //移動軸
                                GroupMove();
                                currentGcode++;
                                dgv_Gcode.CurrentCell = dgv_Gcode.Rows[currentGcode].Cells[0];         //程式碼表選取該應選的行號
                            }

                        }
                    }
                }
                else
                {
                    time_GcodeRead.Enabled = false;         //Gcode計時器關閉
                }

                /*//Gcode最後一行判定
                if (currentGcode != dgv_Gcode.RowCount - 1)
                {
                    currentGcode++;
                    dgv_Gcode.CurrentCell = dgv_Gcode.Rows[currentGcode].Cells[0];         //程式碼表選取該應選的行號
                    string str = dgv_Gcode.CurrentCell.Value.ToString();                   //取得該行號之程式碼

                    //G1判斷!!
                    bool skip = str.StartsWith("G1 ");              //宣告判斷布林
                    if (skip == false)                              //判斷是否G1開頭
                    {
                        dgv_Gcode.CurrentCell = dgv_Gcode.Rows[currentGcode + 1].Cells[0];               //跳行
                    }

                    // serialPort1.Write("a");
                    //time_GcodeSkip.Enabled = true;          //跳行計時器啟動
                }
                else
                {
                    time_GcodeSkip.Enabled = false;         //跳行計時器關閉
                    time_GcodeRead.Enabled = false;         //Gcode計時器關閉
                                                            timer_USB.Enabled = false;      //加熱器停止
                                                            DO_port = 0;                    //加熱器停止
                                                            instantDoCtrl1.Write(0, DO_port);//加熱器停止
                                                            axis4.StopDec();                //E軸停止
                                                            //MultipleMove(0, 0, 10000, 0);           //相對上升Z 10000
                }*/
            }
        }

        //halcon測試
        int HA = 0;
        private HWindow window_A;
        private HFramegrabber framegrabber_A;
        private HImage image_A;

        private void button1_Click(object sender, EventArgs e)
        {
            if (HA == 0)
            {
                window_A = hWindowControl1.HalconWindow;

                framegrabber_A = new HFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "default", -1,
        "default", -1, "false", "default", "0030531de0c2_Basler_acA192050gm", 0, -1);

                framegrabber_A.GrabImageStart(-1);
                image_A = framegrabber_A.GrabImage();
                image_A.DispObj(window_A);
                //pictureBox1.Image = framegrabber_A.GrabImage();
                HA = 1;
            }
            image_A = framegrabber_A.GrabImage();
            image_A.DispObj(window_A);

        }

        //USB-4716


        //加熱測試鈕 關閉程式時記得關閉加熱器!
        public const int AI_count = 16;
        double[] AI_Data = new double[AI_count];            //類比輸入空間矩陣
        byte DO_port = 0;                                   //數位輸入出之代號

        private void btn_HeatingTrigger_Click(object sender, EventArgs e)
        {
            if (!timer_USB.Enabled)                         //加熱計時器啟動
            {
                timer_USB.Enabled = true;
                Etrigger = true;
            }
            else if (timer_USB.Enabled)                     //加熱計時器關閉並關閉加熱
            {
                timer_USB.Enabled = false;
                Etrigger = false;
                DO_port = 0;
                instantDoCtrl1.Write(0, DO_port);
            }
        }

        private void timer_USB_Tick(object sender, EventArgs e)
        {
            if (Convert.ToDouble(AI_Data[0]) < 2.9)                     //當小於2.9V時啟動加熱器
            {
                DO_port = 1;                        //數位輸出腳位0號
                instantDoCtrl1.Write(0, DO_port);
            }
            else if (Convert.ToDouble(AI_Data[0]) >= 2.9)               //當大於等於2.9V時關閉加熱器
            {
                DO_port = 0;
                instantDoCtrl1.Write(0, DO_port);
            }
            //double temperature;
            if (lb_USBHeating.Items.Count >= 15)                          //電壓檢測表寫入與限制行數
            {
                for (int i = 1; i < lb_USBHeating.Items.Count; i++)
                {
                    lb_USBHeating.Items[i - 1] = lb_USBHeating.Items[i];
                }
                instantAiCtrl1.Read(0, AI_count, AI_Data);

                lb_USBHeating.Items[lb_USBHeating.Items.Count - 1] = AI_Data[0].ToString("0.000");
            }
            else                                                        //初始加入行數
            {
                instantAiCtrl1.Read(0, AI_count, AI_Data);
                lb_USBHeating.Items.Add(AI_Data[0].ToString("0.000"));
            }
            lbl_USBState2.Text = AI_Data[0].ToString("0.000");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //自動輸入與找尋軸卡程式

            //宣告(全域宣告在Form1.Designer.cs之末端)
            int Result;

            //程式本體
            //掃描設備數量
            Result = Motion.mAcm_GetAvailableDevs(CurAvailableDevs, Motion.MAX_DEVICES, ref deviceCount);
            if (Result != (int)ErrorCode.SUCCESS)
            {
                MessageBox.Show("Can Not Get Available Device", "PTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cbx_DeviceSelect.Items.Clear();
            for (int i = 0; i < deviceCount; i++)
            {
                cbx_DeviceSelect.Items.Add(CurAvailableDevs[i].DeviceName);
            }
            if (deviceCount > 0)
            {
                cbx_DeviceSelect.SelectedIndex = 0;
                DeviceNum = CurAvailableDevs[0].DeviceNum;
            }
            //結束
        }

        private void nud_MultipleMovePosition0_ValueChanged(object sender, EventArgs e)
        {
            KeyPosition();
        }

        private void nud_MultipleMovePosition1_ValueChanged(object sender, EventArgs e)
        {
            KeyPosition();
        }

        private void nud_MultipleMovePosition2_ValueChanged(object sender, EventArgs e)
        {
            KeyPosition();
        }

        private void nud_MultipleMovePosition3_ValueChanged(object sender, EventArgs e)
        {
            KeyPosition();
        }

        private void ckb_Reverse_CheckedChanged(object sender, EventArgs e)
        {
            KeyPosition();
        }

        private void btn_GoHome_Click(object sender, EventArgs e)
        {
            UInt32 Result;
            UInt32 PropertyVal = new UInt32();
            double Vel = new double();
            double CrossDistance = new double();
            if (!m_bInit)
            {
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                Vel = 4000;
                Result = Motion.mAcm_SetProperty(m_Axishand[i], (uint)PropertyID.PAR_AxVelLow, ref Vel, (uint)Marshal.SizeOf(typeof(double)));
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("Set Property-PAR_AxVelLow Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Home", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Vel = 12000;
                Result = Motion.mAcm_SetProperty(m_Axishand[i], (uint)PropertyID.PAR_AxVelHigh, ref Vel, (uint)Marshal.SizeOf(typeof(double)));
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("Set Property-PAR_AxVelHigh Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Home", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                PropertyVal = 1;
                Result = Motion.mAcm_SetProperty(m_Axishand[i], (uint)PropertyID.PAR_AxHomeExSwitchMode, ref PropertyVal, (uint)Marshal.SizeOf(typeof(UInt32)));
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("Set Property-PAR_AxHomeExSwitchMode Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Home", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CrossDistance = 0;
                Result = Motion.mAcm_SetProperty(m_Axishand[i], (uint)PropertyID.PAR_AxHomeCrossDistance, ref CrossDistance, (uint)Marshal.SizeOf(typeof(double)));
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("Set Property-AxHomeCrossDistance Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Home", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                UInt32 dir = 0;
                switch (i)
                {
                    case 0:
                        dir = 0;
                        break;
                    case 1:
                        dir = 0;
                        break;
                    case 2:
                        dir = 0;
                        break;
                }
                Result = Motion.mAcm_AxHome(m_Axishand[i], (UInt32)1, dir);
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    MessageBox.Show("AxHome Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]", "Home", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            HomeCome = true;
            timer_HomeWait.Enabled = true;
            return;
        }

        bool HomeCome = true;
        private void timer_HomeWait_Tick(object sender, EventArgs e)
        {
            if (HomeCome)
            {
                if (sp[0] == (UInt16)AxisState.STA_AX_READY && sp[1] == (UInt16)AxisState.STA_AX_READY && sp[2] == (UInt16)AxisState.STA_AX_READY)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                Key[i] = -300000;
                                break;
                            case 1:
                                Key[i] = -200000;
                                break;
                            case 2:
                                Key[i] = -100000;
                                break;
                        }
                        E = 0;
                    }
                    Vel = 16000;
                    VelLow = 4000;
                    Acc = 8000;
                    Dec = 8000;
                    GroupMove();
                    HomeCome = false;
                }
            }
            else
            {
                if (sp[0] == (UInt16)AxisState.STA_AX_READY && sp[1] == (UInt16)AxisState.STA_AX_READY && sp[2] == (UInt16)AxisState.STA_AX_READY)
                {
                    //將每一軸設定理論位置到0
                    for (int i = 0; i < 4; i++)
                    {
                        double cmdPosition = new double();
                        cmdPosition = 0;
                        Motion.mAcm_AxSetCmdPosition(m_Axishand[i], cmdPosition);
                        Motion.mAcm_AxSetActualPosition(m_Axishand[i], cmdPosition);
                    }
                    HomeCome = true;
                    timer_HomeWait.Enabled = false;
                }
            }
        }

        private void btn_SetCmd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                Motion.mAcm_AxSetCmdPosition(m_Axishand[i], 0);
            }
        }
    }
}
