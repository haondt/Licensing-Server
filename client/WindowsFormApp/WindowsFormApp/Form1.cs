﻿using System;
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
using System.Net;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;
using System.Numerics;

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
			this.ExpiryLabel.Text = "";
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

			// Get time stamp
			this.ExpiryLabel.Text = "License will expire on: " + this.getExpiryDate(this.LicenseKeyTextBox.Text);
    


        }

        private string getPublicKey()
		{
			return @"-----BEGIN PUBLIC KEY-----
				MIIBYjANBgkqhkiG9w0BAQEFAAOCAU8AMIIBSgKCAUEA2NNNpjIHbh4Nj6rwoCDs
				kLqapFwErz266pfGs05570mXwB1z2oGSfRzKeYFswQa+SMNLkLTDiylfL/XGdoI9
				olTBcScsLTkB+/qnThLRZ1cZsjtAZ5QN68T3tfR6iSzNX6YPuxw+wGah4uuDuZ5C
				Jail7lhMQcKZ4WuNJ17MrTs6OwSPWd3bxuqGcq9tUHeHDGC23rFPMNH0NJZx6+tr
				8131vac2tDEa26Dq62IoiY79NbhKSUnz1B135jATBeZkIFevAuZT05kwNP7klTKL
				4iDMjIJwq7IQBtXUJk1VaPA+svCht7/tXDk3yr0wTi5z6hm8RjUEQ6MnflXUOhmi
				hLvoMJT0Mke6QcYnwHVciI5M2FiXRXYaMTMuShbQPwB/H+WPJ29yGEx4Na2XoeLo
				qoPeYnUY216vTVGq2CiJ0/cCAwEAAQ==
				-----END PUBLIC KEY-----";
		}
		private string getPublickeyFromServer()
        {
			string uri = "http://10.48.32.119:8080/signature_key";
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
        }

		private string getExpiryDate(string licenseKey)
		{
			// Seperate last 8 digits as date string
			string dateHex = licenseKey.Split('-')[2] + licenseKey.Split('-')[3];

			// Convert to int
			BigInteger timestamp = BigInteger.Parse(dateHex, System.Globalization.NumberStyles.HexNumber);

			// convert to date
			System.DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			var expiry = epoch.AddSeconds((double) timestamp).ToLocalTime();
			return expiry.ToLongDateString();
		}

        private bool verifyHWID(string HWID, string licenseKey)
        {
			// Seperate first 8 digits as HWID
			string KeyHWIDHash = licenseKey.Split('-')[0] + licenseKey.Split('-')[1];

			// Get hash of hwid
            string HWIDHash;
            using(System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(HWID);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                HWIDHash = hashBytes.Aggregate(new StringBuilder(), (SB, hb) => SB.Append(hb.ToString("x2"))).ToString();
            }

			// convert hash to integer
			BigInteger HWIDHashInt = BigInteger.Parse(HWIDHash, System.Globalization.NumberStyles.HexNumber);

			// mod to 8 hex digits
			HWIDHashInt = (BigInteger) (HWIDHashInt % (BigInteger)(Math.Pow(2, 4 * 8)));

			// convert back to hex
			HWIDHash = HWIDHashInt.ToString("X");

			// trim leading characters
			while(HWIDHash.Length > 8)
			{
				HWIDHash = HWIDHash.Substring(1);
			}

			Console.WriteLine(HWIDHash);
			Console.WriteLine(KeyHWIDHash);


            return HWIDHash.CompareTo(KeyHWIDHash) == 0;
        }

        private void LoadLicenseFileButton_Click(object sender, EventArgs e)
        {
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string licenseJson = this.getLicenseKeyJson(openFileDialog1.FileName);

				JObject license = JObject.Parse(licenseJson);
				this.LicenseKeyTextBox.Text = license["license_key"].ToString();
				this.signature = license["signature"].ToString();
			}

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
