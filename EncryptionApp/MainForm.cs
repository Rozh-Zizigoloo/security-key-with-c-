using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptionApp
{
    public partial class MainForm : Form
    {
        private const string publicKeyFilePath = "public.key"; // مسیر فایل کلید عمومی
        private const string privateKeyFilePath = "private.key"; // مسیر فایل کلید خصوصی

        public MainForm()
        {
            InitializeComponent();
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            if (File.Exists("AMG.KEY"))
            {
                statusLabel.Text = "The AMG.KEY file exists. Program will exit.";
                MessageBox.Show("The AMG.KEY file exists. Program will exit.", "File Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CloseAfterDelay();
            }
            else
            {
                statusLabel.Text = "The AMG.KEY file doesn't exist. Program will create it";
                MessageBox.Show("The AMG.KEY file doesn't exist. Program will create it", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                statusLabel.Text = "Creating public.key...";
                GenerateRSAKeys();
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (File.Exists("AMG.KEY"))
            {
                string privateKey = File.ReadAllText(privateKeyFilePath);
                DecryptFile("AMG.KEY", "Decryption AMG.txt", privateKey);
                MessageBox.Show("AMG.KEY decrypted and saved as Decryption AMG.txt.", "Decryption Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("AMG.KEY does not exist. Cannot decrypt.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseAfterDelay()
        {
            System.Threading.Thread.Sleep(5000); // Delay for 5 seconds
            Application.Exit();
        }

        private void GenerateRSAKeys()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Save public key
                string publicKey = rsa.ToXmlString(false);
                File.WriteAllText(publicKeyFilePath, publicKey);
                statusLabel.Text = "public.key created.";
                MessageBox.Show("public.key created.", "Key Created", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Save private key
                string privateKey = rsa.ToXmlString(true);
                File.WriteAllText(privateKeyFilePath, privateKey);
                statusLabel.Text = "private.key created.";
                MessageBox.Show("private.key created.", "Key Created", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Encrypt prvkey.txt and save as AMG.KEY
                EncryptFile("prvkey.txt", "AMG.KEY");
                statusLabel.Text = "prvkey.txt encrypted and saved as AMG.KEY.";
                MessageBox.Show("prvkey.txt encrypted and saved as AMG.KEY.", "Encryption Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        static void EncryptFile(string inputFilePath, string outputFilePath)
        {
            string publicKey = File.ReadAllText(publicKeyFilePath);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                byte[] dataToEncrypt = File.ReadAllBytes(inputFilePath);
                byte[] encryptedData = rsa.Encrypt(dataToEncrypt, false);
                File.WriteAllBytes(outputFilePath, encryptedData);
            }
        }

        static void DecryptFile(string inputFilePath, string outputFilePath, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] dataToDecrypt = File.ReadAllBytes(inputFilePath);
                byte[] decryptedData = rsa.Decrypt(dataToDecrypt, false);
                File.WriteAllBytes(outputFilePath, decryptedData);
            }
        }

        private void decryptionButton_Click(object sender, EventArgs e)
        {
            if (File.Exists("AMG.KEY"))
            {
                string privateKey = File.ReadAllText(privateKeyFilePath);
                DecryptFile("AMG.KEY", "Decryption AMG.txt", privateKey);
                MessageBox.Show("AMG.KEY decrypted and saved as Decryption AMG.txt.", "Decryption Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("AMG.KEY does not exist. Cannot decrypt.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
