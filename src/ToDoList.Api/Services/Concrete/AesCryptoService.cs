using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Api.Helpers;

namespace ToDoList.Api.Services.Concrete
{
	public class AesCryptoService : IAesCryptoService
    {
        private readonly IOptionsMonitor<OptionManager> optionManager;

        public AesCryptoService(IOptionsMonitor<OptionManager> optionManager)
		{
            this.optionManager = optionManager;
        }

        public string EncryptString(string text)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(optionManager.CurrentValue.AppSettings.AesKey, Encoding.Unicode.GetBytes(optionManager.CurrentValue.AppSettings.AlgorithmIV));
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    text = Convert.ToBase64String(ms.ToArray());
                }
            }

            return text;
        }

        public string DecryptString(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(optionManager.CurrentValue.AppSettings.AesKey, Encoding.Unicode.GetBytes(optionManager.CurrentValue.AppSettings.AlgorithmIV));
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }
    }
}
