using DefaultWebProject.Conexao;
using DefaultWebProject.Log;
using DefaultWebProject.Models;
using DefaultWebProject.Tokken;
using DefaultWebProject.TratamentoString;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml;
using Newtonsoft;
using System.Web.Http.OData;
using DefaultWebProject.Context;

namespace DefaultWebProject.Controllers
{
    public class JournalVouchersController : ApiController
    {
        public static string LogFile = "";    
        public static List<TokenUtilizado> TokenUtilizado { get; set; } = new List<TokenUtilizado>();

        private SBO_TesteContext db = new SBO_TesteContext();


        public IHttpActionResult GetToken()
        {
            PersonalAuthentication credential = new PersonalAuthentication();
            LogFile = new GravarLog(LogFile).Escrever("#####################################################################");
            LogFile = new GravarLog(LogFile).Escrever("RECUPERANDO CREDENCIAIS");

            var xml = new ConfigXmlDocument();
            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();

            if (credential != null)
            {
                var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
                if (validacao)
                {
                    string tokenGerado = new ValidarToken().GerarToken(empresa);
                    List<TokenUtilizado> TokenUtilizad = new List<TokenUtilizado>();
                    TokenUtilizado = TokenUtilizad;
                    TokenUtilizado.Add(new TokenUtilizado()
                    {
                        Token = tokenGerado,
                        Utilizado = false,
                        Empresa = empresa
                    });
                    LogFile = new GravarLog(LogFile).Escrever("SUCESSO: TOKEN - " + tokenGerado);
                    return Ok(tokenGerado);
                }
                else
                {
                    LogFile = new GravarLog(LogFile).Escrever("CREDENCIAL INVALIDA - DADOS REPASSADOS: Usuario: " + credential.Username + " - Senha: " + credential.Password);
                    return BadRequest("Credencial Invalida:::: DADOS REPASSADOS: Usuario: " + credential.Username + " - Senha: " + credential.Password);
                }
            }
            else
            {
                return BadRequest("CredencialInvalida - OBJETO CREDENCIAL NULO!");
            }
        }

        /// <summary>
        /// Primeiro Gere Seu Token Após Faça Login Aqui 
        /// </summary> 
        /// <returns></returns> 
        [System.Web.Http.Route("Login")]
        [ResponseType(typeof(PersonalAuthentication))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult Login()
        {

            GetToken();
            LogFile = new GravarLog(LogFile).Escrever("#####################################################################");
            LogFile = new GravarLog(LogFile).Escrever("VALIDAÇÃO DE LOGIN POR TOKEN");
            var listatoken = TokenUtilizado;
            foreach (var item in TokenUtilizado)
            {


                if (item.Token != null)
                {
                    LogFile = new GravarLog(LogFile).Escrever("INICIA VALIDAÇÃO DE TOKEN");
                    var (tokenValido, empresa) = new ValidarToken().ValidarTokenUtilizadoJournalVouchers(item.Token);
                    if (tokenValido)
                    {
                        CompanyList.ConexaoInicial(empresa);
                        StringBuilder empresas = new StringBuilder();
                        foreach (var comp in CompanyList.SAPCompanies.companyLists)
                        {
                            if (comp.Company.Connected)
                                empresas.Append(string.Format("Empresa {0} conectada com sucesso!{1}", comp.Company.CompanyName + " ----- ", Environment.NewLine));
                            else
                                empresas.Append(string.Format("Empresa {0} não conectada! - Erro: {1} {2}", comp.DataBase + " ----- ", comp.Mensagem, Environment.NewLine));
                        }
                        LogFile = new GravarLog(LogFile).Escrever("VALIDAÇÃO REALIZADA COM SUCESSO!");
                        LogFile = new GravarLog(LogFile).Escrever("EMPRESAS LOGADAS: " + empresa.ToString());
                        return Ok(empresas.ToString());
                    }
                    else
                    {
                        LogFile = new GravarLog(LogFile).Escrever("TOKEN INVALIDO: " + item.Token);
                        return BadRequest("Token Invalido!");
                    }

                }
                else
                {
                    LogFile = new GravarLog(LogFile).Escrever("TOKEN INVALIDO: O OBJETO TOKEN NULO");
                    return BadRequest("Token Invalido!");
                }

            }
            return BadRequest("Token Invalido!");
        }


        /// <summary>
        /// GetOJDTList
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetOJDTList")]
        [ResponseType(typeof(JornalVouchersModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetOJDTList()
        {
            IQueryable<JornalVouchersModel> query = QueryOJDT();
            return Ok(query);
        }
        /// <summary>
        /// GetOJDT(ID)
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetOJDT(ID)")]
        [ResponseType(typeof(JornalVouchersModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetOJDT(int ID)
        {
            IQueryable<JornalVouchersModel> query = QueryOJDT();
            return Ok(query.Where(x=>x.TransId == ID));
        }
        // Metodo Auxiliar 
   
        public IQueryable<JornalVouchersModel> QueryOJDT()
        {
            try
            {
                return db.jornalVouchers;
            }
            catch (Exception ex)
            {

                return db.jornalVouchers;
            }
        }

        /// <summary>
        /// Faça o Lançamento Por Aqui Com Base No XML Enviado
        /// </summary> 
        /// <returns></returns>
        [System.Web.Http.Route("AddLcmXml")]
        [ResponseType(typeof(AddLcmModel))]
        [System.Web.Http.HttpPost]
        public IHttpActionResult AddLcmXml(String XMlDocument)        {
            String BANCO;
            String ID_FILIAL;
            String HEADER;
            String LINES;
            LogFile = new GravarLog(LogFile).Escrever("#####################################################################");
            LogFile = new GravarLog(LogFile).Escrever("INICIADO PROCESSO DE PRÉ LANÇAMENTO CONTABIL!");
            LogFile = new GravarLog(LogFile).Escrever("INICIADA VALIDAÇAO DE TOKEN!");
            XmlDocument document = new XmlDocument();
            document.LoadXml(XMlDocument);   
            AddLcmModel lcmModel = new AddLcmModel();
            BANCO = document.GetElementsByTagName("sap:BANCO").Item(0).InnerXml.ToString();
            ID_FILIAL = document.GetElementsByTagName("sap:ID_FILIAL").Item(0).InnerXml.ToString();
            HEADER = document.GetElementsByTagName("sap:HEADER").Item(0).InnerXml.ToString();
            LINES = document.GetElementsByTagName("sap:LINES").Item(0).InnerXml.ToString();
       
            foreach (var TokenGerado in TokenUtilizado)
            {


                if (TokenGerado.Token != null)
                {
                    var (tokenValido, empresa) = new ValidarToken().ValidarTokenUtilizadoJournalVouchers(TokenGerado.Token);
                    if (tokenValido)
                    {
                        if (empresa.Banco_Dados != BANCO)
                        {
                            LogFile = new GravarLog(LogFile).Escrever("BANCO DE DADOS INFORMADO INCORRETAMENTE!!");
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(string.Format(@"<root>Banco de dados informado incorretamente.</root>"));
                            TokenUtilizado = null;
                            return BadRequest(xmlDoc.ToString());
                        }


                        LogFile = new GravarLog(LogFile).Escrever("VALIDANDO CONEXÃO COM SAP!");
                        CompanyList.ConexaoInicial(empresa);
                        if (string.IsNullOrEmpty(LogFile))
                            LogFile = new GravarLog().Escrever("#################################");
                        else
                            LogFile = new GravarLog(LogFile).Escrever("#################################");

                        LogFile = new GravarLog(LogFile).Escrever("INICIANDO VALIDAÇÃO DE ESTRUTURA DE DADOS");
                        var (sucesso, xml) = ValidarEstrutura(BANCO, ID_FILIAL, HEADER, LINES);
                        if (!sucesso)
                        {
                            TokenUtilizado = null;
                            return BadRequest(xml.ToString());
                        }

                        LogFile = new GravarLog(LogFile).Escrever("ESTRUTURA VALIDADA COM SUCESSO");
                        LogFile = new GravarLog(LogFile).Escrever("Xml Header orginal:::>>>" + HEADER);
                        LogFile = new GravarLog(LogFile).Escrever("Xml Lines original:::>>>" + LINES);

                        HEADER = HEADER.Replace("<![CDATA[", "").Replace("]]>", "");
                        LINES = LINES.Replace("<![CDATA[", "").Replace("]]>", "");

                        LogFile = new GravarLog(LogFile).Escrever("Xml Header ajustado:::>>>" + HEADER);
                        LogFile = new GravarLog(LogFile).Escrever("Xml Lines ajustado:::>>>" + LINES);

                        Header header;
                        try
                        {
                            LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO HEADER");
                            header = new Header().GetHeader(HEADER);
                            LogFile = new GravarLog(LogFile).Escrever("FINALIZADO MAPEAMENTO HEADER");
                        }
                        catch
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(string.Format(@"<root>XML de header do lançamento contabil fora do pardão especificado. Processo finalizado!</root>"));
                            TokenUtilizado = null;
                            return BadRequest(xmlDoc.ToString());
                        }

                        List<Lines> lines;
                        try
                        {
                            LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO LINES");
                            lines = new Lines().GetLines(LINES);


                            //Verificando dados das linhas alterado Beto
                            ConnectionWithXml consulta = new ConnectionWithXml();
                            List<DadosErroModel> dadosErro = consulta.ConsultarPN(lines);
                            if (dadosErro.Count > 0)
                            {
                                var reg = new GravarLog(true, "ERRO").Escrever("#################################");
                                foreach (var item in dadosErro)
                                {
                                    new GravarLog(reg).Escrever("Linha:");
                                    new GravarLog(reg).Escrever("Campo: " + item.Campo);
                                    new GravarLog(reg).Escrever("valor: " + item.conteudo);
                                    new GravarLog(reg).Escrever("Erro: " + item.mgsErro);
                                }

                            }


                            if (lines.Count == 0)
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(string.Format(@"<root>XML de linhas do lançamento contabil fora do pardão especificado. Processo finalizado!</root>"));
                                TokenUtilizado = null;
                                return BadRequest(xmlDoc.ToString());
                            }
                            else
                            {
                                var ln = lines.Where(x => x.AccountCode == "").FirstOrDefault();
                                if (ln != null)
                                    if (string.IsNullOrEmpty(ln.AccountCode))
                                    {
                                        XmlDocument xmlDoc = new XmlDocument();
                                        xmlDoc.LoadXml(string.Format(@"<root>XML de linhas do lançamento contabil fora do pardão especificado, Existem linhas sem conta contabil. Referencia da linha incorreta "" {0} "". Processo finalizado!</root>", ln.Reference1));
                                        TokenUtilizado = null;
                                        return BadRequest(xmlDoc.ToString());
                                    }
                            }
                            LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO LINES FINALIZADO");
                        }
                        catch
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(string.Format(@"<root>XML de linhas do lançamento contabil fora do pardão especificado. Processo finalizado!</root>"));
                            TokenUtilizado = null;
                            return BadRequest(xmlDoc.ToString());
                        }

                        try
                        {

                            LogFile = new GravarLog(LogFile).Escrever("Xml Cabecalho - " + HEADER);
                            LogFile = new GravarLog(LogFile).Escrever("#################################");
                            LogFile = new GravarLog(LogFile).Escrever("Xml Linhas - " + LINES);
                            LogFile = new GravarLog(LogFile).Escrever("#################################");
                        }
                        catch (Exception ex)
                        {
                            LogFile = new GravarLog(LogFile).Escrever("#################################");
                            LogFile = new GravarLog(LogFile).Escrever("ERRO APRESENTADO>>>" + ex.Message);
                            LogFile = new GravarLog(LogFile).Escrever("#################################");
                        }

                        LogFile = new GravarLog(LogFile).Escrever("RECUPERA EMPRESA CONECTADA");
                        LogFile = new GravarLog(LogFile).Escrever("VERIFICA SE SAPCOMPANIES É NULA");
                        if (CompanyList.SAPCompanies == null)
                        {
                            LogFile = new GravarLog(LogFile).Escrever("SAPCOMPANIES É NULA, TENTANDO RECONECTAR");
                            CompanyList.ConexaoInicial();
                        }

                        LogFile = new GravarLog(LogFile).Escrever("VERIFICA SE COMPANYLIST É NULA");
                        if (CompanyList.SAPCompanies.companyLists == null)
                            LogFile = new GravarLog(LogFile).Escrever("RECUPERA EMPRESA CONECTADA 1.");

                        LogFile = new GravarLog(LogFile).Escrever("TENTANDO RECUPERAR BANCO DA LISTA DE EMPRESAS CONECTADAS");
                        var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == BANCO).FirstOrDefault();

                        if (comp == null || comp.Company == null)
                            LogFile = new GravarLog(LogFile).Escrever("EMPRESA NÃO ESTA CONECTADA 2.");

                        if (comp.Company.Connected)
                            LogFile = new GravarLog(LogFile).Escrever("EMPRESA CONECTADA " + comp.Company.CompanyName);
                        else
                            LogFile = new GravarLog(LogFile).Escrever("EMPRESA NÃO ESTA CONECTADA 3.");

                        if (comp.Company.Connected)
                        {
                            LogFile = new GravarLog(LogFile).Escrever("INICIADO MAPEAMENTO DE LANÇAMENTO CONTABIL");
                            LogFile = new GravarLog(LogFile).Escrever("VALIDANDO DATAS:");
                            LogFile = new GravarLog(LogFile).Escrever("DATA LANÇAMENTO:" + header.ReferenceDate);
                            LogFile = new GravarLog(LogFile).Escrever("DATA VENCIMENTO:" + header.DueDate);
                            LogFile = new GravarLog(LogFile).Escrever("DATA DOCUMENTO:" + header.TaxDate);
                            string linhamapeada = "";
                            string linhaDetalhada = "";
                            try
                            {
                                LogFile = new GravarLog(LogFile).Escrever("INICIADO MAPEAMENTO DE LANÇAMENTO");
                                using (var doc = new InstanciaSap(comp.Company))
                                {
                                    LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO DE CABEÇALHO DO PRE-LANÇAMENTO");
                                    doc.JournalVouchers.JournalEntries.ReferenceDate = header.ReferenceDate; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO REFERENCEDATE");
                                    doc.JournalVouchers.JournalEntries.Reference = header.Reference; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO REFERENCE");
                                    doc.JournalVouchers.JournalEntries.Reference2 = header.Reference2; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO REFERENCE2");
                                    doc.JournalVouchers.JournalEntries.DueDate = header.DueDate; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO DUEDATE");
                                    doc.JournalVouchers.JournalEntries.TaxDate = header.TaxDate; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO TAXDATE");
                                    doc.JournalVouchers.JournalEntries.StornoDate = header.StornoDate; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO STORNODATE");
                                    doc.JournalVouchers.JournalEntries.Memo = header.Memo; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO MEMO");
                                    switch (header.UseAutoStorno)
                                    {
                                        case "tYES":
                                            doc.JournalVouchers.JournalEntries.UseAutoStorno = SAPbobsCOM.BoYesNoEnum.tYES;
                                            break;
                                        default:
                                            doc.JournalVouchers.JournalEntries.UseAutoStorno = SAPbobsCOM.BoYesNoEnum.tNO;
                                            break;
                                    }
                                    LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO AUTOSTORNO");

                                    LogFile = new GravarLog(LogFile).Escrever("INICIADO MAPEAMENTO DE LINHAS DO PRE-LANÇAMENTO");
                                    int index = 0;

                                    foreach (var ln in lines)
                                    {
                                        linhaDetalhada = string.Format(@"Linha: {10} - Conta contabil: {0} - CodPn: {1} - Filial: {2} - Debito: {3} - Credito: {4} - Reference1: {5} - CentroCusto: {6} - Projeto: {7} - Memo: {8} - Data: {9}",
                                            ln.AccountCode, ln.ShortName, ln.Filial, ln.Debit, ln.Credit, ln.Reference1, ln.CostingCode, ln.ProjectCode, ln.Memo, header.ReferenceDate, index.ToString());
                                        linhamapeada = linhamapeada + " - " + index.ToString();
                                        if (index > 0)
                                            doc.JournalVouchers.JournalEntries.Lines.Add();

                                        if (!string.IsNullOrEmpty(ln.AccountCode))
                                            doc.JournalVouchers.JournalEntries.Lines.AccountCode = ln.AccountCode;
                                        else
                                            doc.JournalVouchers.JournalEntries.Lines.ShortName = ln.ShortName;

                                        doc.JournalVouchers.JournalEntries.Lines.BPLID = int.Parse(ln.Filial);
                                        doc.JournalVouchers.JournalEntries.Lines.Debit = double.Parse(ln.Debit, CultureInfo.InvariantCulture);
                                        doc.JournalVouchers.JournalEntries.Lines.Credit = double.Parse(ln.Credit, CultureInfo.InvariantCulture);
                                        doc.JournalVouchers.JournalEntries.Lines.Reference1 = ln.Reference1;
                                        doc.JournalVouchers.JournalEntries.Lines.CostingCode2 = ln.CostingCode;
                                        doc.JournalVouchers.JournalEntries.Lines.ProjectCode = ln.ProjectCode;
                                        doc.JournalVouchers.JournalEntries.Lines.LineMemo = ln.Memo;
                                        doc.JournalVouchers.JournalEntries.Lines.ReferenceDate1 = header.ReferenceDate;
                                        doc.JournalVouchers.JournalEntries.Lines.TaxDate = header.TaxDate;
                                        doc.JournalVouchers.JournalEntries.Lines.DueDate = header.DueDate;

                                        index++;
                                    }
                                    LogFile = new GravarLog(LogFile).Escrever("LINHAS MAPEADAS >>>" + linhamapeada);

                                    LogFile = new GravarLog(LogFile).Escrever("ADICIONANDO LANÇAMENTO CONTABIL");
                                    int ret = doc.JournalVouchers.JournalEntries.Add();
                                    if (ret != 0)
                                    {
                                        comp.Company.GetLastError(out ret, out string msg);
                                        LogFile = new GravarLog(LogFile).Escrever("ERRO AO ADICIONAR LANÇAMENTO: " + msg);
                                        XmlDocument xmlDoc = new XmlDocument();
                                        xmlDoc.LoadXml(string.Format(@"<root>Erro em geracao de documento:{0}</root>", msg));
                                        TokenUtilizado = null;
                                        return BadRequest(xmlDoc.ToString());
                                    }
                                    else
                                    {
                                        ret = doc.JournalVouchers.Add();
                                        if (ret != 0)
                                        {
                                            comp.Company.GetLastError(out ret, out string msg);
                                            LogFile = new GravarLog(LogFile).Escrever("ERRO AO ADICIONAR LANÇAMENTO: " + msg);
                                            XmlDocument xmlDoc = new XmlDocument();
                                            xmlDoc.LoadXml(string.Format(@"<root>Erro em geracao de documento. Verifique Log para saber o motivo do erro. Retorno SAP :{0}</root>", msg));
                                            var lst = new ValidacoesEstruturais().Validacao(header, lines, BANCO);
                                            var reg = new GravarLog(true, "ERRO").Escrever("#################################");
                                            new GravarLog(reg).Escrever("ERRO AO ADICIONAR LANÇAMENTO: " + msg);
                                            //Consulta consulta = new Consulta();
                                            //consulta.ConsultarPN("L10002");

                                            foreach (var err in lst)
                                            {
                                                LogFile = new GravarLog(LogFile).Escrever(string.Format(@"Campo {0} Valor Campo{1}  Mensagem {2} Credito {3} - Debito: {4}", err.Campo, err.Valor, err.MsgErro, err.Credit, err.Debit));
                                            }
                                            reg = new GravarLog(reg).Escrever("#################################");
                                            TokenUtilizado = null;
                                            return BadRequest(xmlDoc.ToString());
                                        }
                                        else
                                        {
                                            string msg = comp.Company.GetNewObjectKey();
                                            LogFile = new GravarLog(LogFile).Escrever("LANÇAMENTO ADICIONADO Nº: " + msg);
                                            var strxml = string.Format(@"<AddObjectResponse CommandID=""Add Object"" Xmlsns=""{1}""><RetKey>{0}</RetKey><RetType>28</RetType></AddObjectResponse>", msg,"RamoBh");
                                            XmlDocument xmlDoc = new XmlDocument();
                                            xmlDoc.LoadXml(strxml);
                                            TokenUtilizado = null;
                                            return Ok(xmlDoc);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogFile = new GravarLog(LogFile).Escrever("ERRO APRESENTADO AO GERAR LANÇAMENTO:::>>>" + ex.Message);
                                LogFile = new GravarLog(LogFile).Escrever("LINHA COM ERRO" + linhaDetalhada);
                                LogFile = new GravarLog(LogFile).Escrever("LINHAS PROCESSADAS" + linhamapeada);

                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(string.Format("<root>ERRO :::>>> " + ex.Message + "</root>"));
                                TokenUtilizado = null;
                                return BadRequest(xmlDoc.ToString());
                            }
                        }
                        else
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(string.Format(@"<root>Token Invalido!</root>"));
                            TokenUtilizado = null;
                            return BadRequest(xmlDoc.ToString());
                        }
                    }
                    else
                    {
                        LogFile = new GravarLog(LogFile).Escrever("TOKEN INVALIDO! - TOKEN::" + TokenGerado.Token);
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(string.Format(@"<root>Token Invalido!</root>"));
                        TokenUtilizado = null;
                        return BadRequest(xmlDoc.ToString());
                    }
                }
                else
                {
                    LogFile = new GravarLog(LogFile).Escrever("ERRO AO AUTENTICAR: OBJETO TOKEN NULO!");
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(string.Format(@"<root>ERRO AO AUTENTICAR: OBJETO TOKEN NULO!</root>"));
                    TokenUtilizado = null;
                    return BadRequest(xmlDoc.ToString());
                }
            }
            TokenUtilizado = null;
            return BadRequest();
        }

        /// <summary>
        /// Faça o Lançamento Por Aqui Com Base No Json Enviado
        /// </summary> 
        /// <returns></returns>
        [System.Web.Http.Route("AddLcmJson")]
        [ResponseType(typeof(BodyModel))]
        [System.Web.Http.HttpPost]
        public IHttpActionResult AddLcmJson(BodyModel folhaModels)
        {
            LogFile = new GravarLog(LogFile).Escrever("#####################################################################");
            LogFile = new GravarLog(LogFile).Escrever("INICIADO PROCESSO DE PRÉ LANÇAMENTO CONTABIL!");
            LogFile = new GravarLog(LogFile).Escrever("INICIADA VALIDAÇAO DE TOKEN!");

            foreach (var itemToken in TokenUtilizado)
            {
                foreach (var itemFolha in folhaModels.Body.AddLcm)
                {
                
                    if (itemToken.Token != null)
                    {
                        var (tokenValido, empresa) = new ValidarToken().ValidarTokenUtilizadoJournalVouchers(itemToken.Token);
                        if (tokenValido)
                        {
                            if (empresa.Banco_Dados != itemFolha.BANCO)
                            {
                                LogFile = new GravarLog(LogFile).Escrever("BANCO DE DADOS INFORMADO INCORRETAMENTE!!");
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(string.Format(@"<root>Banco de dados informado incorretamente.</root>"));
                                TokenUtilizado = null;
                                return BadRequest(xmlDoc.ToString());
                            }


                            LogFile = new GravarLog(LogFile).Escrever("VALIDANDO CONEXÃO COM SAP!");
                            CompanyList.ConexaoInicial(empresa);
                            if (string.IsNullOrEmpty(LogFile))
                                LogFile = new GravarLog().Escrever("#################################");
                            else
                                LogFile = new GravarLog(LogFile).Escrever("#################################");

                            LogFile = new GravarLog(LogFile).Escrever("INICIANDO VALIDAÇÃO DE ESTRUTURA DE DADOS");
                            var (sucesso, xml) = ValidarEstrutura(itemFolha.BANCO, itemFolha.ID_FILIAL, itemFolha.HEADER, itemFolha.LINES);
                            if (!sucesso)
                            {
                                TokenUtilizado = null;
                                return BadRequest(xml.ToString());
                            }
                            LogFile = new GravarLog(LogFile).Escrever("ESTRUTURA VALIDADA COM SUCESSO");
                            LogFile = new GravarLog(LogFile).Escrever("Xml Header orginal:::>>>" + itemFolha.HEADER);
                            LogFile = new GravarLog(LogFile).Escrever("Xml Lines original:::>>>" + itemFolha.LINES);

                            itemFolha.HEADER = itemFolha.HEADER.Replace("<![CDATA[", "").Replace("]]>", "");
                            itemFolha.LINES = itemFolha.LINES.Replace("<![CDATA[", "").Replace("]]>", "");

                            LogFile = new GravarLog(LogFile).Escrever("Xml Header ajustado:::>>>" + itemFolha.HEADER);
                            LogFile = new GravarLog(LogFile).Escrever("Xml Lines ajustado:::>>>" + itemFolha.LINES);

                            Header header;
                            try
                            {
                                LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO HEADER");
                                header = new Header().GetHeader(itemFolha.HEADER);
                                LogFile = new GravarLog(LogFile).Escrever("FINALIZADO MAPEAMENTO HEADER");
                            }
                            catch
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(string.Format(@"<root>XML de header do lançamento contabil fora do pardão especificado. Processo finalizado!</root>"));
                                TokenUtilizado = null;
                                return BadRequest(xmlDoc.ToString());
                            }

                            List<Lines> lines;
                            try
                            {
                                LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO LINES");
                                lines = new Lines().GetLines(itemFolha.LINES);


                                //Verificando dados das linhas alterado Beto
                                ConnectionWithXml consulta = new ConnectionWithXml();
                                List<DadosErroModel> dadosErro = consulta.ConsultarPN(lines);
                                if (dadosErro.Count > 0)
                                {
                                    var reg = new GravarLog(true, "ERRO").Escrever("#################################");
                                    foreach (var item in dadosErro)
                                    {
                                        new GravarLog(reg).Escrever("Linha:");
                                        new GravarLog(reg).Escrever("Campo: " + item.Campo);
                                        new GravarLog(reg).Escrever("valor: " + item.conteudo);
                                        new GravarLog(reg).Escrever("Erro: " + item.mgsErro);
                                    }

                                }


                                if (lines.Count == 0)
                                {
                                    XmlDocument xmlDoc = new XmlDocument();
                                    xmlDoc.LoadXml(string.Format(@"<root>XML de linhas do lançamento contabil fora do pardão especificado. Processo finalizado!</root>"));
                                    TokenUtilizado = null;
                                    return BadRequest(xmlDoc.ToString());
                                }
                                else
                                {
                                    var ln = lines.Where(x => x.AccountCode == "").FirstOrDefault();
                                    if (ln != null)
                                        if (string.IsNullOrEmpty(ln.AccountCode))
                                        {
                                            XmlDocument xmlDoc = new XmlDocument();
                                            xmlDoc.LoadXml(string.Format(@"<root>XML de linhas do lançamento contabil fora do pardão especificado, Existem linhas sem conta contabil. Referencia da linha incorreta "" {0} "". Processo finalizado!</root>", ln.Reference1));
                                            TokenUtilizado = null;
                                            return BadRequest(xmlDoc.ToString());
                                        }
                                }
                                LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO LINES FINALIZADO");
                            }
                            catch
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(string.Format(@"<root>XML de linhas do lançamento contabil fora do pardão especificado. Processo finalizado!</root>"));
                                TokenUtilizado = null;
                                return BadRequest(xmlDoc.ToString());
                            }

                            try
                            {

                                LogFile = new GravarLog(LogFile).Escrever("Xml Cabecalho - " + itemFolha.HEADER);
                                LogFile = new GravarLog(LogFile).Escrever("#################################");
                                LogFile = new GravarLog(LogFile).Escrever("Xml Linhas - " + itemFolha.LINES);
                                LogFile = new GravarLog(LogFile).Escrever("#################################");
                            }
                            catch (Exception ex)
                            {
                                LogFile = new GravarLog(LogFile).Escrever("#################################");
                                LogFile = new GravarLog(LogFile).Escrever("ERRO APRESENTADO>>>" + ex.Message);
                                LogFile = new GravarLog(LogFile).Escrever("#################################");
                            }

                            LogFile = new GravarLog(LogFile).Escrever("RECUPERA EMPRESA CONECTADA");
                            LogFile = new GravarLog(LogFile).Escrever("VERIFICA SE SAPCOMPANIES É NULA");
                            if (CompanyList.SAPCompanies == null)
                            {
                                LogFile = new GravarLog(LogFile).Escrever("SAPCOMPANIES É NULA, TENTANDO RECONECTAR");
                                CompanyList.ConexaoInicial();
                            }

                            LogFile = new GravarLog(LogFile).Escrever("VERIFICA SE COMPANYLIST É NULA");
                            if (CompanyList.SAPCompanies.companyLists == null)
                                LogFile = new GravarLog(LogFile).Escrever("RECUPERA EMPRESA CONECTADA 1.");

                            LogFile = new GravarLog(LogFile).Escrever("TENTANDO RECUPERAR BANCO DA LISTA DE EMPRESAS CONECTADAS");
                            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == itemFolha.BANCO).FirstOrDefault();

                            if (comp == null || comp.Company == null)
                                LogFile = new GravarLog(LogFile).Escrever("EMPRESA NÃO ESTA CONECTADA 2.");

                            if (comp.Company.Connected)
                                LogFile = new GravarLog(LogFile).Escrever("EMPRESA CONECTADA " + comp.Company.CompanyName);
                            else
                                LogFile = new GravarLog(LogFile).Escrever("EMPRESA NÃO ESTA CONECTADA 3.");

                            if (comp.Company.Connected)
                            {
                                LogFile = new GravarLog(LogFile).Escrever("INICIADO MAPEAMENTO DE LANÇAMENTO CONTABIL");
                                LogFile = new GravarLog(LogFile).Escrever("VALIDANDO DATAS:");
                                LogFile = new GravarLog(LogFile).Escrever("DATA LANÇAMENTO:" + header.ReferenceDate);
                                LogFile = new GravarLog(LogFile).Escrever("DATA VENCIMENTO:" + header.DueDate);
                                LogFile = new GravarLog(LogFile).Escrever("DATA DOCUMENTO:" + header.TaxDate);
                                string linhamapeada = "";
                                string linhaDetalhada = "";
                                try
                                {
                                    LogFile = new GravarLog(LogFile).Escrever("INICIADO MAPEAMENTO DE LANÇAMENTO");
                                    using (InstanciaSap doc = new InstanciaSap(comp.Company))
                                    {
                                        LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO DE CABEÇALHO DO PRE-LANÇAMENTO");
                                        doc.JournalVouchers.JournalEntries.ReferenceDate = header.ReferenceDate; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO REFERENCEDATE");
                                        doc.JournalVouchers.JournalEntries.Reference = header.Reference; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO REFERENCE");
                                        doc.JournalVouchers.JournalEntries.Reference2 = header.Reference2; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO REFERENCE2");
                                        doc.JournalVouchers.JournalEntries.DueDate = header.DueDate; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO DUEDATE");
                                        doc.JournalVouchers.JournalEntries.TaxDate = header.TaxDate; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO TAXDATE");
                                        doc.JournalVouchers.JournalEntries.StornoDate = header.StornoDate; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO STORNODATE");
                                        doc.JournalVouchers.JournalEntries.Memo = header.Memo; LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO MEMO");
                                        switch (header.UseAutoStorno)
                                        {
                                            case "tYES":
                                                doc.JournalVouchers.JournalEntries.UseAutoStorno = SAPbobsCOM.BoYesNoEnum.tYES;
                                                break;
                                            default:
                                                doc.JournalVouchers.JournalEntries.UseAutoStorno = SAPbobsCOM.BoYesNoEnum.tNO;
                                                break;
                                        }
                                        LogFile = new GravarLog(LogFile).Escrever("MAPEAMENTO AUTOSTORNO");

                                        LogFile = new GravarLog(LogFile).Escrever("INICIADO MAPEAMENTO DE LINHAS DO PRE-LANÇAMENTO");
                                        int index = 0;

                                        foreach (var ln in lines)
                                        {
                                            linhaDetalhada = string.Format(@"Linha: {10} - Conta contabil: {0} - CodPn: {1} - Filial: {2} - Debito: {3} - Credito: {4} - Reference1: {5} - CentroCusto: {6} - Projeto: {7} - Memo: {8} - Data: {9}",
                                                ln.AccountCode, ln.ShortName, ln.Filial, ln.Debit, ln.Credit, ln.Reference1, ln.CostingCode, ln.ProjectCode, ln.Memo, header.ReferenceDate, index.ToString());
                                            linhamapeada = linhamapeada + " - " + index.ToString();
                                            if (index > 0)
                                                doc.JournalVouchers.JournalEntries.Lines.Add();

                                            if (!string.IsNullOrEmpty(ln.AccountCode))
                                                doc.JournalVouchers.JournalEntries.Lines.AccountCode = ln.AccountCode;
                                            else
                                                doc.JournalVouchers.JournalEntries.Lines.ShortName = ln.ShortName;

                                            doc.JournalVouchers.JournalEntries.Lines.BPLID = int.Parse(ln.Filial);
                                            doc.JournalVouchers.JournalEntries.Lines.Debit = double.Parse(ln.Debit, CultureInfo.InvariantCulture);
                                            doc.JournalVouchers.JournalEntries.Lines.Credit = double.Parse(ln.Credit, CultureInfo.InvariantCulture);
                                            doc.JournalVouchers.JournalEntries.Lines.Reference1 = ln.Reference1;
                                            doc.JournalVouchers.JournalEntries.Lines.CostingCode2 = ln.CostingCode;
                                            doc.JournalVouchers.JournalEntries.Lines.ProjectCode = ln.ProjectCode;
                                            doc.JournalVouchers.JournalEntries.Lines.LineMemo = ln.Memo;
                                            doc.JournalVouchers.JournalEntries.Lines.ReferenceDate1 = header.ReferenceDate;
                                            doc.JournalVouchers.JournalEntries.Lines.TaxDate = header.TaxDate;
                                            doc.JournalVouchers.JournalEntries.Lines.DueDate = header.DueDate;

                                            index++;
                                        }
                                        LogFile = new GravarLog(LogFile).Escrever("LINHAS MAPEADAS >>>" + linhamapeada);

                                        LogFile = new GravarLog(LogFile).Escrever("ADICIONANDO LANÇAMENTO CONTABIL");
                                        int ret = doc.JournalVouchers.JournalEntries.Add();
                                        if (ret != 0)
                                        {
                                            comp.Company.GetLastError(out ret, out string msg);
                                            LogFile = new GravarLog(LogFile).Escrever("ERRO AO ADICIONAR LANÇAMENTO: " + msg);
                                            XmlDocument xmlDoc = new XmlDocument();
                                            xmlDoc.LoadXml(string.Format(@"<root>Erro em geracao de documento:{0}</root>", msg));
                                            TokenUtilizado = null;
                                            return BadRequest(xmlDoc.ToString());
                                        }
                                        else
                                        {
                                            doc.JournalVouchers.JournalEntries.Lines.Add();
                                            ret = doc.JournalVouchers.Add();

                                            if (ret != 0)
                                            {
                                                comp.Company.GetLastError(out ret, out string msg);
                                                LogFile = new GravarLog(LogFile).Escrever("ERRO AO ADICIONAR LANÇAMENTO: " + msg);
                                                XmlDocument xmlDoc = new XmlDocument();
                                                xmlDoc.LoadXml(string.Format(@"<root>Erro em geracao de documento. Verifique Log para saber o motivo do erro. Retorno SAP :{0}</root>", msg));
                                                var lst = new ValidacoesEstruturais().Validacao(header, lines, itemFolha.BANCO);
                                                var reg = new GravarLog(true, "ERRO").Escrever("#################################");
                                                new GravarLog(reg).Escrever("ERRO AO ADICIONAR LANÇAMENTO: " + msg);
                                                //Consulta consulta = new Consulta();
                                                //consulta.ConsultarPN("L10002");

                                                foreach (var err in lst)
                                                {
                                                    LogFile = new GravarLog(LogFile).Escrever(string.Format(@"Campo {0} Valor Campo{1}  Mensagem {2} Credito {3} - Debito: {4}", err.Campo, err.Valor, err.MsgErro, err.Credit, err.Debit));
                                                }
                                                reg = new GravarLog(reg).Escrever("#################################");
                                                TokenUtilizado = null;
                                                return BadRequest(xmlDoc.ToString());
                                            }
                                            else
                                            {
                                                string msg = comp.Company.GetNewObjectKey();
                                                LogFile = new GravarLog(LogFile).Escrever("LANÇAMENTO ADICIONADO Nº: " + msg);
                                                var strxml = string.Format(@"<AddObjectResponse CommandID=""Add Object"" Xmlsns=""{1}""><RetKey>{0}</RetKey><RetType>28</RetType></AddObjectResponse>", msg, "RamoBh");
                                                XmlDocument xmlDoc = new XmlDocument();
                                                xmlDoc.LoadXml(strxml);
                                                TokenUtilizado = null;
                                                return Ok(xmlDoc);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogFile = new GravarLog(LogFile).Escrever("ERRO APRESENTADO AO GERAR LANÇAMENTO:::>>>" + ex.Message);
                                    LogFile = new GravarLog(LogFile).Escrever("LINHA COM ERRO" + linhaDetalhada);
                                    LogFile = new GravarLog(LogFile).Escrever("LINHAS PROCESSADAS" + linhamapeada);

                                    XmlDocument xmlDoc = new XmlDocument();
                                    xmlDoc.LoadXml(string.Format("<root>ERRO :::>>> " + ex.Message + "</root>"));
                                    TokenUtilizado = null;
                                    return BadRequest(xmlDoc.ToString());
                                }
                            }
                            else
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(string.Format(@"<root>Token Invalido!</root>"));
                                TokenUtilizado = null;
                                return BadRequest(xmlDoc.ToString());
                            }
                        }
                        else
                        {
                            LogFile = new GravarLog(LogFile).Escrever("TOKEN INVALIDO! - TOKEN::" + itemToken.Token);
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(string.Format(@"<root>Token Invalido!</root>"));
                            TokenUtilizado = null;
                            return BadRequest(xmlDoc.ToString());
                        }
                    }
                    else
                    {
                        LogFile = new GravarLog(LogFile).Escrever("ERRO AO AUTENTICAR: OBJETO TOKEN NULO!");
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(string.Format(@"<root>ERRO AO AUTENTICAR: OBJETO TOKEN NULO!</root>"));
                        TokenUtilizado = null;
                        return BadRequest(xmlDoc.ToString());
                    }
                }

            }
            TokenUtilizado = null;
            return BadRequest("Sem Token");
        }

        private (bool, XmlDocument) ValidarEstrutura(string BANCO, string ID_FILIAL, string HEADER, string LINES)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (string.IsNullOrEmpty(BANCO))
            {
                xmlDoc.LoadXml(string.Format(@"<root>Requisicao invalida. Necessario informar o nome do banco de dados</root>"));
                return (false, xmlDoc);
            }
            else if (string.IsNullOrEmpty(ID_FILIAL))
            {
                xmlDoc.LoadXml(string.Format(@"<root>Requisicao invalida. Necessario informar o Id da filial da empresa</root>"));
                return (false, xmlDoc);
            }
            else if (string.IsNullOrEmpty(HEADER))
            {
                xmlDoc.LoadXml(string.Format(@"<root>Requisicao invalida. Necessario informar xml HEADER do lançamento.</root>"));
                return (false, xmlDoc);
            }
            else if (string.IsNullOrEmpty(LINES))
            {
                xmlDoc.LoadXml(string.Format(@"<root>Requisicao invalida. Necessario informar o xml LINES do lançamento.</root>"));
                return (false, xmlDoc);
            }

            else
            {
                return (true, null);
            }

        }
    }
}