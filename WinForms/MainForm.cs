using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaiduVoiceAPI.demo;
using Common;
using NAudio.Wave;

namespace WinForms
{
    public partial class MainForm : Form
    {
        public int status = 0;//录音状态标识，0-未录音 1-录音中
        private byte[] tempBytes;
        Speech newSpeech = new Speech();
        private IWaveIn waveIn;
        private WaveFileWriter writer;
        public MainForm()
        {
            InitializeComponent();
            
        }

        //开始录音
        private void startrecord_Click(object sender, EventArgs e)
        {
            switch (status)
            {
                case 0:
                    this.startrecord.Text = "录音中";
                    StartRecording();
                    status = 1;
                    break;
                case 1:
                    this.startrecord.Text = "识别中";
                    StopRecording();
                    status = 0;
                    ReadVoice();
                    this.startrecord.Text = "再次识别";
                    break;
            }
        }


        private async void ReadVoice()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var result = newSpeech.AsrData("test.wav");
                        var temp = JsonHelper.DeserializeStringToDictionary<string, object>(result);
                        if (temp["err_no"].ToString() == "0")
                        {
                            var trimResult = temp["result"].ToString().Replace("[", "").Replace("]", "")
                                .Replace("，", " ").Replace("\"", "");
                            //MessageBox.Show("识别成功:" + trimResult);
                            this.BeginInvoke(new Action(() => { WebView.OpenBaidu(trimResult); }));
                            break;
                        }
                        else
                        {
                            MessageBox.Show("识别错误" + result);
                            break;
                        }
                    }
                    catch(Exception ex)
                    {
                        LogHelper.Error(ex.ToString());
                        // ignored
                    }
                }

            });
        }


        #region NAudio封装部分

        /// <summary>
        /// 开始录音
        /// </summary>
        private void StartRecording()
        {
            if (waveIn != null) return;
            waveIn = new WaveIn { WaveFormat = new WaveFormat(8000, 1) };//设置码率
            writer = new WaveFileWriter("test.wav", waveIn.WaveFormat);
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.RecordingStopped += OnRecordingStopped;
            waveIn.StartRecording();
        }
        /// <summary>
        /// 停止录音
        /// </summary>
        private void StopRecording()
        {
            waveIn.StopRecording();
            waveIn.Dispose();
        }
        /// <summary>
        /// 录音中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<WaveInEventArgs>(waveIn_DataAvailable), sender, e);
            }
            else
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
                int secondsRecorded = (int)(writer.Length / writer.WaveFormat.AverageBytesPerSecond);//录音时间获取
                if (secondsRecorded >= 30)
                {
                    StopRecording();
                }
            }
        }
        /// <summary>
        /// 停止录音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<StoppedEventArgs>(OnRecordingStopped), sender, e);
            }
            else
            {
                if (waveIn != null) // 关闭录音对象
                {
                    waveIn.Dispose();
                    waveIn = null;
                }
                if (writer != null)//关闭文件流
                {
                    writer.Close();
                    writer = null;
                }
                if (e.Exception != null)
                {
                    MessageBox.Show(String.Format("出现问题 {0}",
                        e.Exception.Message));
                }
            }
        }

        #endregion

    }
}
