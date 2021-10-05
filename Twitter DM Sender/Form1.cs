using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace Twitter_DM_Sender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            CheckForIllegalCrossThreadCalls = false;
            StreamReader sr = new StreamReader(@"accounts.txt");
            try
            {
                var fp = File.ReadAllLines(@"accounts.txt");
                foreach (var item in fp)
                {
                    comboBox1.Items.Add(item);
                }
            }
            catch { }
            comboBox1.SelectedIndex = 0;
        }

        int counter;
        int failed;
        int success;
        
        private void button1_Click(object sender, EventArgs e)
        {
            counter = 0;
            failed = 0;
            success = 0;
            Thread thread = new Thread(Gorev);
            thread.Start();
        }

        void Gorev()
        {
            var account = comboBox1.Text.Split(':');
            var username = account[0];
            var password = account[1];

            CookieContainer c = new CookieContainer();
            var token = Login.Go(c, password, username);

            while (counter < textBox1.Lines.Length)
            {
                try
                {
                    var target_user = textBox1.Lines[counter];
                    bool Sent = DMSender.Go(c, token, textBox2.Text, target_user, username, csrf:token);
                    counter++;
                    label4.Text = counter.ToString();
                    if (Sent)
                    {
                        success++;
                        label5.Text = success.ToString();
                    }
                    else
                    {
                        failed++;
                        label6.Text = failed.ToString();
                    }
                }
                catch
                {
                    counter++;
                    label4.Text = counter.ToString();
                    failed++;
                    label6.Text = failed.ToString();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Spinner.Spin(textBox2.Text));
        }

        private void visitOfficialWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://twittermoneybot.com");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://twittermoneybot.com/contact");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int i = textBox1.Lines.Length;
            label8.Text = textBox1.Lines.Length.ToString();
            if (i > 0 && textBox1.Lines[i - 1] == "")
            {
                i--;
                label8.Text = i.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog od = new OpenFileDialog();
                od.Title = "Import";
                od.Filter = "Text Files(*.txt)|*.txt";
                DialogResult dr = od.ShowDialog();
                StreamReader sr = new StreamReader(od.FileName);
                if (dr == DialogResult.OK)
                {
                    textBox1.Text = sr.ReadToEnd();
                }
                sr.Close();
            }
            catch { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }
    }
}
