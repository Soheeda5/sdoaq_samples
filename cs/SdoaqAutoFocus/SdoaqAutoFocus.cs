using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using SDOAQ;
using SDOAQCSharp.Tool;
using SDOAQCSharp;
using SDOAQCSharp.Component;

namespace SdoaqAutoFocus
{
    public partial class SdoaqAutoFocus : Form
    {
        private StringBuilder _logBuffer = new StringBuilder();
        private object _lockLog = new object();
        private Dictionary<int, MySdoaq> _sdoaqObjList = null;

        private SdoaqImageViewr _imgViewer;


        public SdoaqAutoFocus()
        {
            InitializeComponent();

            _imgViewer = new SdoaqImageViewr(false);
            _imgViewer.Dock = DockStyle.Fill;

            pnl_Viewer.Controls.Add(_imgViewer);
            _sdoaqObjList = MySdoaq.LoadScript();
            _imgViewer.Set_SdoaqObj(GetSdoaqObj());

            MySdoaq.LogReceived += Sdoaq_LogDataReceived;            
        }

        private void SdoaqAutoFocus_Load(object sender, EventArgs e)
        {
            tmr_LogUpdate.Start();
            Init();
        }
        
        private void Init()
        {
            bool isInit = MySdoaq.Initialize();
            if (isInit == false) //Init Fail
            {
                gpb_Controls.Enabled = false;
            }
            else
            {
                gpb_Controls.Enabled = true;
            }
        }

        private void btn_SetROI_Click(object sender, EventArgs e)
        {
            // Set ROI
            GetSdoaqObj()?.SetRoi_AF(txt_ROI.Text);
       }

        private void btn_SingleShotAF_Click(object sender, EventArgs e)
        {
            // Single Shot Auto Focus
            var task = GetSdoaqObj()?.Acquisition_AfAsync();
        }

        private void btn_PlayAF_Click(object sender, EventArgs e)
        {
            // Starts an Auto Focus preview
            GetSdoaqObj()?.AcquisitionContinuous_Af();

            btn_SingleShotAF.Enabled = false;
        }

        private void btn_StopAF_Click(object sender, EventArgs e)
        {
            //  Requests a stop of the running continuous AF acquisition.
            GetSdoaqObj()?.AcquisitionStop_Af();

            btn_SingleShotAF.Enabled = true;
        }

        private void btn_SharpnessMethod_Click(object sender, EventArgs e)
        {
            // Focus measure (sharpness measure) method (0: Modified Laplacian, 1: Gradient(Sobel), 2: Graylevel local variance)
            SDOAQ_SetIntParameterValue(SDOAQ_API.eParameterId.pi_af_sharpness_measure_method, txt_SharpnessMethod.Text, 0, 2);            
        }

        private void btn_ResamplingMethod_Click(object sender, EventArgs e)
        {
            // Image processing resolution(0: full, 1: half, 2: quarter)
            SDOAQ_SetIntParameterValue(SDOAQ_API.eParameterId.pi_af_resampling_method, txt_ResamplingMethod.Text, 0, 2);
        }

        private void btn_StabilityMethod_Click(object sender, EventArgs e)
        {
            // 1: None, 2: stability-fullstep, 3: stability-halfstep, 4: stability-onestep
            SDOAQ_SetIntParameterValue(SDOAQ_API.eParameterId.pi_af_stability_method, txt_StabilityMethod.Text, 1, 4);
        }

        #region [Event]
        private void btn_DebounceCount_Click(object sender, EventArgs e)
        {
            SDOAQ_SetIntParameterValue(SDOAQ_API.eParameterId.pi_af_stability_debounce_count, txt_DebounceCount.Text, 0, 10);
        }


        private MySdoaq GetSdoaqObj()
        {
            return _sdoaqObjList[0];
        }

        private void SDOAQ_SetIntParameterValue(SDOAQ_API.eParameterId id, string value, int startRange, int endRange)
        {
            if (Int32.TryParse(value, out int valueNum) == false)
            {
                Write_Log($"Value is not a number. Value is out of range[{startRange} ~ {endRange}] ");
                return;
            }
            else
            {
                if (valueNum >= startRange && valueNum <= endRange)
                {
                    var rv = SDOAQ_API.SDOAQ_SetIntParameterValue(id, valueNum);
                }
                else
                {
                    Write_Log($"The value range is wrong. Value is out of range[{startRange} ~ {endRange}]");
                }
            }
        }
        
        private void tmr_LogUpdate_Tick(object sender, EventArgs e)
        {
            if (_logBuffer.Length == 0)
            {
                return;
            }

            lock (_lockLog)
            {
                txt_Log.AppendText(_logBuffer.ToString());
                txt_Log.ScrollToCaret();
                _logBuffer.Clear();
            }
        }

        private void Sdoaq_LogDataReceived(object sender, LoggerEventArgs e)
        {
            lock (_lockLog)
            {
                _logBuffer.Append(e.Data);
            }
        }

        private void Write_Log(string str)
        {
            Sdoaq_LogDataReceived(null, new LoggerEventArgs(str));
        }        

        private void btn_Init_Click(object sender, EventArgs e)
        {
            Init();
        }
        #endregion
    }
}
