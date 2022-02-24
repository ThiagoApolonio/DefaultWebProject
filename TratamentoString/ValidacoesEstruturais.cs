using DefaultWebProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAPbobsCOM;
using DefaultWebProject.Conexao.QueryRecordSet;

namespace DefaultWebProject.TratamentoString
{
    public class ValidacoesEstruturais
    {

        public ValidacoesEstruturais() { }
        public List<RetModel> Validacao(Header header, List<Lines> lines, string banco)
        {
            var lst = new List<RetModel>();
            if (string.IsNullOrEmpty(header.DueDate.ToString()))
                lst.Add(new RetModel { Campo = "DueDate", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.Indicator.ToString()))
                lst.Add(new RetModel { Campo = "Indicator", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.Memo.ToString()))
                lst.Add(new RetModel { Campo = "Memo", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.ProjectCode.ToString()))
                lst.Add(new RetModel { Campo = "ProjectCode", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.Reference.ToString()))
                lst.Add(new RetModel { Campo = "Reference", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.Reference2.ToString()))
                lst.Add(new RetModel { Campo = "Reference2", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.ReferenceDate.ToString()))
                lst.Add(new RetModel { Campo = "ReferenceDate", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.Series.ToString()))
                lst.Add(new RetModel { Campo = "Series", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.StornoDate.ToString()))
                lst.Add(new RetModel { Campo = "StornoDate", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.TaxDate.ToString()))
                lst.Add(new RetModel { Campo = "TaxDate", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.TransactionCode.ToString()))
                lst.Add(new RetModel { Campo = "TransactionCode", Valor = "", ValorIncorreto = true });
            if (string.IsNullOrEmpty(header.UseAutoStorno.ToString()))
                lst.Add(new RetModel { Campo = "UseAutoStorno", Valor = "", ValorIncorreto = true });

            foreach (var ln in lines)
            {
                if (string.IsNullOrEmpty(ln.AccountCode))
                    lst.Add(new RetModel { Campo = "AccountCode", Valor = "", ValorIncorreto = false });
                else
                {
                    if (!validarConteudoCampoContabil(ln.AccountCode, banco))
                        lst.Add(new RetModel { Campo = "AccountCode", Valor = ln.AccountCode, ValorIncorreto = true, MsgErro = "Propriedade informada incorretamente.", Credit = ln.Credit, Debit = ln.Debit });
                    else if (string.IsNullOrEmpty(ln.ShortName))
                        lst.Add(new RetModel { Campo = "ShortName", Valor = "", ValorIncorreto = false });
                    else
                    {
                        if (!validarConteudoCampoCodigoPn(ln.ShortName, banco))
                            lst.Add(new RetModel { Campo = "ShortName", Valor = ln.ShortName, ValorIncorreto = true, MsgErro = "Propriedade informada incorretamente.", Credit = ln.Credit, Debit = ln.Debit });
                    }
                }
            }
            return lst;
        }

        private bool validarConteudoCampoContabil(string conteudo, string banco)
        {
            using (var rc = new SboRecordSet(banco))
            {
                var rec = rc.ExecutarQuery(rc.QueryContaContabil(conteudo));
                if (rec.RecordCount > 0)
                    return true;
                else
                    return false;
            }
        }
        private bool validarConteudoCampoCodigoPn(string conteudo, string banco)
        {
            using (var rc = new SboRecordSet(banco))
            {
                var rec = rc.ExecutarQuery(rc.QueryCodigoParceiro(conteudo));
                if (rec.RecordCount > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}