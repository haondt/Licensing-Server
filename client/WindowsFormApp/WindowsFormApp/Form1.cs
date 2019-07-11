using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;

namespace WindowsFormApp
{
    public partial class Form1 : Form
    {
        string publicKey;
        string signature;

        public Form1()
        {
            InitializeComponent();
            this.HWIDVerificationLabel.Text = "";
            this.SignatureVerificationLabel.Text = "";
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private string getHWID()
        {
            return new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_OperatingSystem").Get().Cast<ManagementObject>().First()["SerialNumber"].ToString();
        }

        private void HWIDButton_Click(object sender, EventArgs e)
        {
            this.HWIDTextBox.Text = this.getHWID();
        }

        private void LicenseKeyButton_Click(object sender, EventArgs e)
        {
            if (this.signature == null)
            {
                this.HWIDVerificationLabel.Text = "No license loaded!";
                return;
            }

            string HWID = this.HWIDTextBox.Text;
            this.publicKey = this.getPublicKey();

            // Verify HWID matches license key
            if (this.verifyHWID(HWID, this.LicenseKeyTextBox.Text))
            {
                this.HWIDVerificationLabel.Text = "HWID matches license key";
                this.HWIDVerificationLabel.ForeColor = Color.Green;
            }
            else
            {
                this.HWIDVerificationLabel.Text = "HWID does not match license key";
                this.HWIDVerificationLabel.ForeColor = Color.Red;
            }


            // Verify signature
            if (this.verifyLicenseSignature(this.LicenseKeyTextBox.Text, this.publicKey, this.signature))
            {
                this.SignatureVerificationLabel.Text = "Signature matches license key";
                this.SignatureVerificationLabel.ForeColor = Color.Green;
            }
            else
            {

                this.SignatureVerificationLabel.Text = "Signature does not match license key";
                this.SignatureVerificationLabel.ForeColor = Color.Red;
            }
    


        }

        private string getPublicKey()
        {
            return File.ReadAllText(@"C:\Users\Noah Burghardt\Desktop\license_server\pyserver\pbKey.txt");
        }

        private bool verifyHWID(string HWID, string licenseKey)
        {
            string HWIDHash;
            using(System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(HWID);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                HWIDHash = hashBytes.Aggregate(new StringBuilder(), (SB, hb) => SB.Append(hb.ToString("x2"))).ToString();
            }
            return HWIDHash.CompareTo(licenseKey) == 0;
        }

        private void LoadLicenseFileButton_Click(object sender, EventArgs e)
        {
            // License key that is valid, matches computer HWID and has matching signature
            string licenseJson = this.getLicenseKeyJson(@"C:\Users\Noah Burghardt\Desktop\license_server\pyserver\good_license.txt");

            // License key that was created for a different computer, and they just manually changed the HWID in the license file
            //string licenseJson = this.getLicenseKeyJson(@"C:\Users\Noah Burghardt\Desktop\license_server\pyserver\tampered_license.txt");

            // License key that is valid but was generated for a different computer
            //string licenseJson = this.getLicenseKeyJson(@"C:\Users\Noah Burghardt\Desktop\license_server\pyserver\HWID_license.txt");

            // License key that matches computer HWID and has matching signature but was invalidated by the server
            //string licenseJson = this.getLicenseKeyJson(@"C:\Users\Noah Burghardt\Desktop\license_server\pyserver\invalid_license.txt");

            JObject license = JObject.Parse(licenseJson);
            this.LicenseKeyTextBox.Text = license["license_key"].ToString();
            this.signature = license["signature"].ToString();
        }

        private string getLicenseKeyJson(string location)
        {
            return File.ReadAllText(location);
        }

        private bool verifyLicenseSignature(string licenseKey, string publicKey, string signature)
        {
            byte[] encodedPublicKey = Encoding.UTF8.GetBytes(publicKey);
            byte[] encodedLicenseKey = Encoding.UTF8.GetBytes(licenseKey);
            byte[] decodedSignature = Convert.FromBase64String(signature);



            SHA256 sha256 = new SHA256Managed();
            PemReader pr = new PemReader(new StringReader(publicKey));
            AsymmetricKeyParameter publicKeyParameter = (AsymmetricKeyParameter)pr.ReadObject();
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKeyParameter);

            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParams);

            return csp.VerifyData(encodedLicenseKey, CryptoConfig.MapNameToOID("SHA256"), decodedSignature);

            



        }
    }
}
