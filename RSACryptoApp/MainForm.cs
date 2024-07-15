namespace RSACryptoApp;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RSACryptoApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnGenerateKeys_Click(object sender, EventArgs e)
        {
            GenerateKeys();
            txtMessages.Text = "Keys generated successfully.";
        }

        private void btnEncryptFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                EncryptFile(openFileDialog.FileName, "encryptedFile.dat", "publicKey.xml");
                txtMessages.Text = "File encrypted successfully.";
            }
        }

        private void btnDecryptFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DecryptFile(openFileDialog.FileName, "decryptedFile.txt", "privateKey.xml");
                txtMessages.Text = "File decrypted successfully.";
            }
        }

        private void GenerateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                string publicKey = rsa.ToXmlString(false);
                string privateKey = rsa.ToXmlString(true);
                File.WriteAllText("publicKey.xml", publicKey);
                File.WriteAllText("privateKey.xml", privateKey);
            }
        }

        private void EncryptFile(string inputFile, string outputFile, string publicKeyFile)
        {
            string publicKey = File.ReadAllText(publicKeyFile);
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(publicKey);
                byte[] dataToEncrypt = File.ReadAllBytes(inputFile);
                byte[] encryptedData = rsa.Encrypt(dataToEncrypt, true);
                File.WriteAllBytes(outputFile, encryptedData);
            }
        }

        private void DecryptFile(string inputFile, string outputFile, string privateKeyFile)
        {
            string privateKey = File.ReadAllText(privateKeyFile);
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(privateKey);
                byte[] dataToDecrypt = File.ReadAllBytes(inputFile);
                byte[] decryptedData = rsa.Decrypt(dataToDecrypt, true);
                File.WriteAllBytes(outputFile, decryptedData);
            }
        }
    }
}
