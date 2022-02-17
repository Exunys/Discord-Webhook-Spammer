using System;
using System.Drawing;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Webhook_Spammer
{
    public partial class Form1 : Form
    {
        public HttpClient httpClient = new HttpClient();
        bool Spamming = false;
        int Requests;
        int TotalRequests;

        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.UseSystemPasswordChar = false;
            }
            else if (checkBox1.Checked == false)
            {
                textBox1.UseSystemPasswordChar = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox5.Text != "")
            {
                if (Spamming == false)
                {
                    Spamming = true;
                    button1.Text = "Stop";
                    button1.ForeColor = Color.FromArgb(255, 0, 0);
                }
                else if (Spamming == true)
                {
                    Spamming = false;
                    button1.Text = "Start";
                    button1.ForeColor = Color.FromArgb(0, 255, 0);
                    Requests = 0;
                }
            }
            else
            {
                Spamming = false;
                button1.Text = "Start";
                button1.ForeColor = Color.FromArgb(0, 255, 0);
                Requests = 0;
            }

            SpamWebhook();
        }

        private void RateLimited(bool Value)
        {
            if (Value == true)
            {
                this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate  () {  label8.Text = "You are being rate limited!";  });
            }
            else if (Value == false)
            {
                this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate  () {  label8.Text = "";  });
            }
        }

        private void Post(string Url, HttpContent Pairs)
        {
            Thread WebhookSendThread = new Thread(async  () =>
            {
                try
                {
                    await httpClient.PostAsync(Url, Pairs);
                    RateLimited(false);
                }
                catch
                {
                    RateLimited(true);
                    await httpClient.PostAsync(Url, Pairs);
                }
            });

            WebhookSendThread.Start();
        }


        private void PostWebhook(string Url, string Message, string Username, string AvatarURL)
        {
            
            string data = JsonConvert.SerializeObject(new { content = Message, username = Username, avatar_url = AvatarURL });

            Post(Url, new StringContent(data, System.Text.Encoding.UTF8, "application/json"));
        }

        private async void SpamWebhook()
        {
            while (true)
            {
                if (Spamming == true)
                {
                    for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
                    {
                        PostWebhook(textBox1.Text, textBox5.Text, textBox4.Text, textBox6.Text);
                        Requests = Requests + 1;
                        TotalRequests = TotalRequests + 1;
                        label9.Text = Convert.ToString(TotalRequests);
                        label5.Text = Convert.ToString(Requests);
                    }
                }
                else if (Spamming == false)
                {
                    Requests = 0;
                    label5.Text = Convert.ToString(Requests);
                    break;
                }

                await Task.Delay((int)Convert.ToDouble(textBox3.Text));
            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Spamming = false;
            Requests = 0;
            label5.Text = Convert.ToString(Requests);
            button1.Text = "Start";
            button1.ForeColor = Color.FromArgb(0, 255, 0);
            textBox1.Text = "";
            checkBox1.Checked = false;
            textBox3.Text = "500";
            textBox2.Text = "1";
            textBox5.Text = "";
            textBox4.Text = "";
            textBox6.Text = "";
        }
    }
}

// Exunys (Aleksandar)