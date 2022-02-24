using DefaultWebProject.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Tokken
{
    public class ValidarToken
    {

        public static List<TokenUtilizado> TokenUtilizado { get; set; } = new List<TokenUtilizado>();
        public (bool, Sapcredentials) CredencialValida(PersonalAuthentication credencial)
        {

            string user = credencial.Username;
            string pass = credencial.Password;
        
            List<Sapcredentials> empresas = new Sapcredentials(true,XmlOrConfigEnum.Xml).Empresas;
            var empresa = empresas.Where(x => x.Usuario_sap == user && x.Senha_sap == pass).FirstOrDefault();

            if (empresa != null)
                return (true, empresa);
            else
                return (false, null);

        }
        

   

        public (bool, Sapcredentials) ValidarTokenUtilizadoJournalVouchers(string Token)
        {


            var tokenAtivo = JournalVouchersController.TokenUtilizado.Where(x => x.Token == Token).FirstOrDefault();
            if (tokenAtivo != null)
            {
                var textoToken = CipherClass.Decrypt(tokenAtivo.Token, tokenAtivo.Empresa.ChavePrivada);
                var partesToken = textoToken.Split('_');
                string senha = partesToken[0].ToString();

                string format = "yyyymmddhhmmssffff";
                string dateTime = partesToken[1].ToString();
                DateTime data = DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);

                string strdataAtual = DateTime.Now.ToString("yyyymmddhhmmssffff");
                DateTime dataAtual = DateTime.ParseExact(strdataAtual, format, CultureInfo.InvariantCulture);
                var validade = (dataAtual - data).TotalSeconds;
                if (validade > 600)//Alterado if (validade > 60)
                    return (false, null);
                else
                {
                    tokenAtivo.Utilizado = true;
                    return (true, tokenAtivo.Empresa);
                }

            }
            else
            {
                return (false, null);
            }
        }
        public (bool, Sapcredentials) ValidarTokenUtilizadoBP(string Token)
        {
            var tokenAtivo = BusinessPartnerController.TokenUtilizado.Where(x => x.Token == Token).FirstOrDefault();
            if (tokenAtivo != null)
            {
                var textoToken = CipherClass.Decrypt(tokenAtivo.Token, tokenAtivo.Empresa.ChavePrivada);
                var partesToken = textoToken.Split('_');
                string senha = partesToken[0].ToString();

                string format = "yyyymmddhhmmssffff";
                string dateTime = partesToken[1].ToString();
                DateTime data = DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);

                string strdataAtual = DateTime.Now.ToString("yyyymmddhhmmssffff");
                DateTime dataAtual = DateTime.ParseExact(strdataAtual, format, CultureInfo.InvariantCulture);
                var validade = (dataAtual - data).TotalSeconds;
                if (validade > 600)//Alterado if (validade > 60)
                    return (false, null);
                else
                {
                    tokenAtivo.Utilizado = true;
                    return (true, tokenAtivo.Empresa);
                }

            }
            else
            {
                return (false, null);
            }
        }
        //public (bool, Sapcredentials) ValidarTokenUtilizadoItem(string Token)
        //{
        //    var tokenAtivo = ItemController.TokenUtilizado.Where(x => x.Token == Token).FirstOrDefault();
        //    if (tokenAtivo != null)
        //    {
        //        var textoToken = CipherClass.Decrypt(tokenAtivo.Token, tokenAtivo.Empresa.ChavePrivada);
        //        var partesToken = textoToken.Split('_');
        //        string senha = partesToken[0].ToString();

        //        string format = "yyyymmddhhmmssffff";
        //        string dateTime = partesToken[1].ToString();
        //        DateTime data = DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);

        //        string strdataAtual = DateTime.Now.ToString("yyyymmddhhmmssffff");
        //        DateTime dataAtual = DateTime.ParseExact(strdataAtual, format, CultureInfo.InvariantCulture);
        //        var validade = (dataAtual - data).TotalSeconds;
        //        if (validade > 600)//Alterado if (validade > 60)
        //            return (false, null);
        //        else
        //        {
        //            tokenAtivo.Utilizado = true;
        //            return (true, tokenAtivo.Empresa);
        //        }

        //    }
        //    else
        //    {
        //        return (false, null);
        //    }
        //}
       

        public string GerarToken(Sapcredentials empresa)
        {
            string textoToken = string.Format(@"{0}_{1}", empresa.Senha_sap, DateTime.Now.ToString("yyyymmddhhmmssffff"));
            return CipherClass.Encrypt(textoToken, empresa.ChavePrivada);
        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        //Method using to Decode, you can use internal, public, private...
        public string Base64Decode(string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return "";
            }
        }
    }
}