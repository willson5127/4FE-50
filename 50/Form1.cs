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
using System.Runtime.InteropServices; //For Marshal

namespace _50
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            tcl_Window00.Location = this.Location;
            tcl_Window00.Size = this.Size;


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
                //Open every Axis and get the each Axis Handle
                //And Initial property for each Axis 		
                //Open Axis 
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
                //Reset Command Counter
                double cmdPosition = new double();
                cmdPosition = 0;
                Motion.mAcm_AxSetCmdPosition(m_Axishand[i], cmdPosition);
            }
            cbx_AxisSelect.SelectedIndex = 0;
            m_bInit = true;
            timer1.Enabled = true;
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

                timer1.Enabled = false;
                m_bInit = false;
                cbx_AxisSelect.Items.Clear();
                cbx_AxisSelect.Text = "";
                tbx_AisxPosition.Clear();
                tbx_AxisState.Clear();
            }
        }

        //啟動軸卡
        private void btn_OpenDevice_Click(object sender, EventArgs e)
        {
            OpenBoard();
        }

        //軸之位置與狀態觀察計時器
        private void timer1_Tick(object sender, EventArgs e)
        {
            double CurCmd = new double();
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
                        btn_SetVel.Enabled = true;
                        txt_VelHigh.Enabled = true;
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
                        btn_SetVel.Enabled = false;
                        txt_VelHigh.Enabled = false;
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

                for (int i = 0; i < 4; i++)
                {
                    Motion.mAcm_AxGetCmdPosition(m_Axishand[i], ref CurCmd);
                    Motion.mAcm_AxGetState(m_Axishand[i], ref AxState);
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
                            txt_PositionX.Text = Convert.ToString(CurCmd);
                            txt_StateX.Text = strTemp;
                            break;
                        case "1":
                            txt_PositionY.Text = Convert.ToString(CurCmd);
                            txt_StateY.Text = strTemp;
                            break;
                        case "2":
                            txt_PositionZ.Text = Convert.ToString(CurCmd);
                            txt_StateZ.Text = strTemp;
                            break;
                        case "3":
                            txt_PositionE.Text = Convert.ToString(CurCmd);
                            txt_StateE.Text = strTemp;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //關閉軸卡連線
        private void btn_CloseDevice_Click(object sender, EventArgs e)
        {
            CloseBoard();
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

        private void btn_AxisMultipleMove_Click(object sender, EventArgs e)
        {
            UInt32 Result;
            double move0, move1, move2, move3;
            move0 = Convert.ToDouble(txt_MultipleMovePosition0.Text);
            move1 = Convert.ToDouble(txt_MultipleMovePosition1.Text);
            move2 = Convert.ToDouble(txt_MultipleMovePosition2.Text);
            move3 = Convert.ToDouble(txt_MultipleMovePosition3.Text);

            

            if (ckb_Reverse.Checked)
            {
                move0 = -move0;
                move1 = -move1;
                move2 = -move2;
                move3 = -move3;
            }
            if (m_bInit)
            {
                double[] par_VelHigh = new double[4];
                double x = 0, y = 0, z = 0;
                double x_k = 0, y_k = 0, z_k = 0;

                x_k = Convert.ToDouble(txt_MultipleMovePosition0.Text);
                y_k = Convert.ToDouble(txt_MultipleMovePosition1.Text);
                z_k = Convert.ToDouble(txt_MultipleMovePosition2.Text);

                if (rbtn_Asolute.Checked)                             //絕對移動確認
                {
                    
                    x = Convert.ToDouble(txt_PositionX.Text);                      //抓取XYZ座標
                    y = Convert.ToDouble(txt_PositionY.Text);
                    z = Convert.ToDouble(txt_PositionZ.Text);

                    x_k -= x;
                    y_k -= y;
                    z_k -= z;                         //取得點到點距離                  
                }

                double mid = Math.Pow(x_k,2)+ Math.Pow(y_k, 2) + Math.Pow(z_k, 2);
                double max_distance = Math.Sqrt(mid);                   //移動距離

                par_VelHigh[0] = VelHigh * Math.Abs(x_k / max_distance);//照移動路徑設定X軸運行速度
                par_VelHigh[1] = VelHigh * Math.Abs(y_k / max_distance);//照移動路徑設定Y軸運行速度
                par_VelHigh[2] = VelHigh * Math.Abs(z_k / max_distance);//照移動路徑設定Z軸運行速度

                for (int i = 0; i < 3; i++)
                {
                    if (par_VelHigh[i] == 0)
                        par_VelHigh[i]++;
                    Result = Motion.mAcm_SetProperty(m_Axishand[i], (uint)PropertyID.PAR_AxVelHigh, ref par_VelHigh[i], (uint)Marshal.SizeOf(typeof(double)));
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
            }
            return;
        }

        private void btn_SetVel_Click(object sender, EventArgs e)
        {
            VelHigh = Convert.ToDouble(txt_VelHigh.Text);
        }

        private void rbtn_Asolute_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_Asolute.Checked)
                ckb_Reverse.Enabled = false;
            else
                ckb_Reverse.Enabled = true;
        }
    }
}
