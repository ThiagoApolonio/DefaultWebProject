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
using SAPbobsCOM;
using System.Web.Http.OData;
using DefaultWebProject.Context;

namespace DefaultWebProject.Controllers
{
    public class BusinessPartnerController : ApiController
    {
        #region DECLARAÇOES DE VARIAVEIS LOCAIS 

        public static string LogFile = "";

        public static DateTime CompanyFirsTime;
        public static List<TokenUtilizado> TokenUtilizado { get; set; } = new List<TokenUtilizado>();

        private SBO_TesteContext db = new SBO_TesteContext();

        #endregion

        #region CONEXAO VIA XML LOCALIZADO NA RAIZ DA SOLUÇÃO E VALIDAÇAO DE TOKEN E LOGIN 
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
        /// Faça Seu Login Aqui 
        /// </summary> 
        /// <returns></returns> 
        [System.Web.Http.Route("LoginParceiroDeNegocio")]
        [ResponseType(typeof(PersonalAuthentication))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult Login()
        {

            LogFile = new GravarLog(LogFile).Escrever("#####################################################################");
            LogFile = new GravarLog(LogFile).Escrever("VALIDAÇÃO DE LOGIN POR TOKEN");
            GetToken();
            var listatoken = TokenUtilizado;
            foreach (var item in TokenUtilizado)
            {


                if (item.Token != null)
                {
                    LogFile = new GravarLog(LogFile).Escrever("INICIA VALIDAÇÃO DE TOKEN");
                    var (tokenValido, empresa) = new ValidarToken().ValidarTokenUtilizadoBP(item.Token);
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
        #endregion

        #region METODOS GET,POST,PUT PN

        /// <summary>
        /// Busque um PN via Json BusinessPartnerModel
        /// </summary> 
        /// <returns></returns>  
        
        [System.Web.Http.Route("GetPartnersList")]
        [ResponseType(typeof(BlinkModel))]
        [System.Web.Http.HttpGet]
        [EnableQuery]
        public IHttpActionResult GetPartnersList()
        {
            IQueryable<BlinkModel> query = QueryPN();
            return Ok(query);
        }
        [EnableQuery]
        [System.Web.Http.Route("GetPartners(ID)")]
        [ResponseType(typeof(BlinkModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetPartners(string CardCode)
        {
            IQueryable<BlinkModel> query = QueryPN();
            return Ok(query.Where(x => x.CardCode == CardCode));
        }

        // Metodo Auxiliar 
        [EnableQuery]
        public IQueryable<BlinkModel> QueryPN()
        {
            try
            {
                return db.BusinessPartner;
            }
            catch (Exception ex)
            {

                return db.BusinessPartner;
            }
        }

        /// <summary>
        /// Manutenção
        /// </summary> 
        /// <returns></returns>  
        [System.Web.Http.Route("PostPn")]
        [ResponseType(typeof(BusinessPartnerModel))]
        [System.Web.Http.HttpPost]
        public IHttpActionResult PostPn(BusinessPartnerModel listModel)
        {
            try
            {
                LogFile = new GravarLog(LogFile).Escrever("#####################################################################");
                LogFile = new GravarLog(LogFile).Escrever("INICIANDO");

                foreach (var itemToken in TokenUtilizado)
                {
                    if (itemToken.Token != null)
                    {
                        var (tokenValido, empresa) = new ValidarToken().ValidarTokenUtilizadoBP(itemToken.Token);
                        if (tokenValido)
                        {
                            foreach (var itemBp in listModel.ListBlink)
                            {
                                #region VALIDACAO BANCO DE DADOS 

                                if (empresa.Banco_Dados != empresa.Banco_Dados)
                                {
                                    LogFile = new GravarLog(LogFile).Escrever("BANCO DE DADOS INFORMADO INCORRETAMENTE!!");
                                    XmlDocument xmlDoc = new XmlDocument();
                                    xmlDoc.LoadXml(string.Format(@"<root>Banco de dados informado incorretamente.</root>"));
                                    TokenUtilizado = null;
                                    return BadRequest(xmlDoc.ToString());
                                }


                                LogFile = new GravarLog(LogFile).Escrever("VALIDANDO CONEXÃO COM SAP!");
                                CompanyList.ConexaoInicial(empresa);
                                var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == empresa.Banco_Dados).FirstOrDefault();
                                if (string.IsNullOrEmpty(LogFile))
                                {
                                    LogFile = new GravarLog().Escrever("#################################");
                                }
                                else
                                {
                                    LogFile = new GravarLog(LogFile).Escrever("#################################");
                                }

                                #endregion

                                #region VALIDACAO CAMPOS OBRIGATORIOS VAZIOS OU NULOS

                                //LogFile = new GravarLog(LogFile).Escrever("INICIADO MAPEAMENTO DE PARCEIRO DE NEGOCIO \n INICIADO VALIDAÇÃO DOS CAMPOS DE PARCEIRO DE NEGOCIO");
                                //if (listModel.ListBlink.Any(X => X.razaoSocial.Equals(string.Empty) //validação regra de negocio
                                //|| X.fantasia.Equals(string.Empty) || X.codigoTipoPedido.Equals(string.Empty) || X.codigoGrupo.Equals(string.Empty) || X.series.Equals(string.Empty) || X.companiaPrivada.Equals(string.Empty)))
                                //{

                                //    LogFile = new GravarLog(LogFile).Escrever("ERRO DE VALIDAÇÃO CAMPO VAZIO NULO OU INVALIDO");
                                //    return BadRequest("ERRO DE VALIDAÇÃO CAMPO VAZIO NULO OU INVALIDO OS SEGUINTES CAMPOS SAO OBRIGATORIOS razaoSocial fantasia,codigoTipoPedido,codigoGrupo,series,companiaPrivada");
                                //}

                                #endregion

                                #region ADICIONANDO PARCEIRO DE NEGOCIO

                                //using (var doc = new InstanciaSap(comp.Company))
                                //{

                                //    comp.Company.Connect();
                                //    switch (itemBp.AdicionarPnEnum)
                                //    {
                                //        #region Adicionando Apenas Um Parceiro De Negocio Informaçoes Basicas
                                //        case Enum.AdicionarPnEnum.ApenasPN:

                                //            //################################################################ - ADICIONANDO APENAS UM PARCEIRO DE NEGOCIO

                                //            doc.BusinessPartners.CardCode = itemBp.codigoCliente;
                                //            doc.BusinessPartners.CardName = itemBp.razaoSocial;
                                //            doc.BusinessPartners.AliasName = itemBp.fantasia;
                                //            doc.BusinessPartners.CardType = itemBp.codigoTipoPedido;
                                //            doc.BusinessPartners.SubjectToWithholdingTax = itemBp.SUBJECT;
                                //            doc.BusinessPartners.GroupCode = itemBp.codigoGrupo;
                                //            doc.BusinessPartners.Series = itemBp.series;
                                //            doc.BusinessPartners.CompanyPrivate = itemBp.companiaPrivada;

                                //            //################################################################ - ADICIONANDO INFORMAÇOES ADICIONAIS TELA PRINCIPAL PN

                                //            #region Informaçoes Adicionais Autogeradas

                                //            doc.BusinessPartners.EmailAddress = itemBp.email;
                                //            doc.BusinessPartners.EmailAddress = itemBp.emailNfe;
                                //            doc.BusinessPartners.Valid = itemBp.ativo;
                                //            doc.BusinessPartners.PayTermsGrpCode = itemBp.codigoCondicaoPagamento;
                                //            doc.BusinessPartners.BPPaymentMethods.PaymentMethodCode = itemBp.codigoFormaPagamento;
                                //            doc.BusinessPartners.DiscountPercent = itemBp.percentualDesconto;
                                //            doc.BusinessPartners.BPPaymentDates.PaymentDate = itemBp.dataUltimaCompra;
                                //            doc.BusinessPartners.ChannelBP = itemBp.canal;
                                //            doc.BusinessPartners.Phone1 = itemBp.telefone;
                                //            doc.BusinessPartners.Fax = itemBp.fax;
                                //            doc.BusinessPartners.CreditLimit = itemBp.limiteCredito;
                                //            doc.BusinessPartners.Addresses.Country = itemBp.pais;
                                //            doc.BusinessPartners.Currency = itemBp.moeda;
                                //            doc.BusinessPartners.CommissionGroupCode = itemBp.GrupoComissao;
                                //            doc.BusinessPartners.CommissionPercent = itemBp.comissaoPercentual;
                                //            doc.BusinessPartners.ContactPerson = itemBp.contatoPessoal;
                                //            doc.BusinessPartners.VatLiable = itemBp.VatLiable;
                                //            doc.BusinessPartners.ShippingType = itemBp.tipoEnvio;
                                //            #endregion
                                //            var ret = doc.BusinessPartners.Add();
                                //            if (ret != 0)
                                //            {
                                //                comp.Company.GetLastError(out ret, out string msg);
                                //                LogFile = new GravarLog(LogFile).Escrever(msg + "- Erro ao adicionar");
                                //                return BadRequest(msg);
                                //            }

                                //            return Ok(itemBp);
                                //        #endregion

                                //        #region ADICIONANDO PNcomEnderecoCobrança
                                //        case Enum.AdicionarPnEnum.PNcomEnderecoCobrança:
                                //            doc.BusinessPartners.CardCode = itemBp.codigoCliente;
                                //            doc.BusinessPartners.CardName = itemBp.razaoSocial;
                                //            doc.BusinessPartners.AliasName = itemBp.fantasia;
                                //            doc.BusinessPartners.CardType = itemBp.codigoTipoPedido;
                                //            doc.BusinessPartners.SubjectToWithholdingTax = itemBp.SUBJECT;
                                //            doc.BusinessPartners.GroupCode = itemBp.codigoGrupo;
                                //            doc.BusinessPartners.Series = itemBp.series;
                                //            doc.BusinessPartners.CompanyPrivate = itemBp.companiaPrivada;

                                //            //################################################################ - ADICIONANDO ENDEREÇO DE COBRANÇA 

                                //            doc.BusinessPartners.Address = itemBp.endereco;
                                //            doc.BusinessPartners.Addresses.StreetNo = itemBp.numero;
                                //            doc.BusinessPartners.Addresses.BuildingFloorRoom = itemBp.complemento;
                                //            doc.BusinessPartners.Addresses.Block = itemBp.bairro;
                                //            doc.BusinessPartners.Addresses.City = itemBp.cidade;
                                //            doc.BusinessPartners.Addresses.State = itemBp.estado;
                                //            doc.BusinessPartners.Addresses.ZipCode = itemBp.cep;
                                //            doc.BusinessPartners.Addresses.Country = itemBp.pais;
                                //            doc.BusinessPartners.Addresses.Add();
                                //            ret = doc.BusinessPartners.Add();
                                //            if (ret != 0)
                                //            {
                                //                comp.Company.GetLastError(out ret, out string msg);
                                //                LogFile = new GravarLog(LogFile).Escrever(msg + "- Erro ao adicionar");
                                //                return BadRequest(msg);
                                //            }
                                //            return Ok(itemBp);
                                //        #endregion

                                //        #region ADICIONANDO  PNcomInformacaoDeContato
                                //        case Enum.AdicionarPnEnum.PNcomInformacaoDeContato:
                                //            doc.BusinessPartners.CardCode = itemBp.codigoCliente;
                                //            doc.BusinessPartners.CardName = itemBp.razaoSocial;
                                //            doc.BusinessPartners.AliasName = itemBp.fantasia;
                                //            doc.BusinessPartners.CardType = itemBp.codigoTipoPedido;
                                //            doc.BusinessPartners.SubjectToWithholdingTax = itemBp.SUBJECT;
                                //            doc.BusinessPartners.GroupCode = itemBp.codigoGrupo;
                                //            doc.BusinessPartners.Series = itemBp.series;
                                //            doc.BusinessPartners.CompanyPrivate = itemBp.companiaPrivada;

                                //            //################################################################ - ADICIONANDO ENDEREÇO DE FUNCIONARIO

                                //            doc.BusinessPartners.ContactEmployees.Name = "TesteAdiçãoPn";
                                //            doc.BusinessPartners.ContactEmployees.Address = "EnderecoPN";
                                //            doc.BusinessPartners.ContactEmployees.E_Mail = "c1@abcd.com";
                                //            doc.BusinessPartners.ContactEmployees.Fax = "8433777778";
                                //            doc.BusinessPartners.ContactEmployees.MobilePhone = "8388888";
                                //            doc.BusinessPartners.ContactEmployees.Phone1 = "88880000";
                                //            doc.BusinessPartners.ContactEmployees.Name = "C2";
                                //            doc.BusinessPartners.ContactEmployees.Address = "BJ";
                                //            doc.BusinessPartners.ContactEmployees.E_Mail = "c2@abcd.com";
                                //            doc.BusinessPartners.ContactEmployees.Fax = "84338";
                                //            doc.BusinessPartners.ContactEmployees.MobilePhone = "877388888";
                                //            doc.BusinessPartners.ContactEmployees.Phone1 = "8888300";
                                //            doc.BusinessPartners.ContactEmployees.Add();
                                //            ret = doc.BusinessPartners.Add();
                                //            if (ret != 0)
                                //            {
                                //                comp.Company.GetLastError(out ret, out string msg);
                                //                LogFile = new GravarLog(LogFile).Escrever(msg + "- Erro ao adicionar");
                                //                return BadRequest(msg);
                                //            }
                                //            return Ok(itemBp);
                                //        #endregion

                                //        #region ADICIONANDO PNcompleto
                                //        case Enum.AdicionarPnEnum.PNcompleto:

                                //            doc.BusinessPartners.CardCode = itemBp.codigoCliente;
                                //            doc.BusinessPartners.CardName = itemBp.razaoSocial;
                                //            doc.BusinessPartners.AliasName = itemBp.fantasia;
                                //            doc.BusinessPartners.CardType = itemBp.codigoTipoPedido;
                                //            doc.BusinessPartners.SubjectToWithholdingTax = itemBp.SUBJECT;
                                //            doc.BusinessPartners.GroupCode = itemBp.codigoGrupo;
                                //            doc.BusinessPartners.Series = itemBp.series;
                                //            doc.BusinessPartners.CompanyPrivate = itemBp.companiaPrivada;
                                //            //################################################################ - ADICIONANDO ENDEREÇO DE COBRANÇA 
                                //            doc.BusinessPartners.Address = itemBp.endereco;
                                //            doc.BusinessPartners.Addresses.StreetNo = itemBp.numero;
                                //            doc.BusinessPartners.Addresses.BuildingFloorRoom = itemBp.complemento;
                                //            doc.BusinessPartners.Addresses.Block = itemBp.bairro;
                                //            doc.BusinessPartners.Addresses.City = itemBp.cidade;
                                //            doc.BusinessPartners.Addresses.State = itemBp.estado;
                                //            doc.BusinessPartners.Addresses.ZipCode = itemBp.cep;
                                //            doc.BusinessPartners.Addresses.Country = itemBp.pais;
                                //            doc.BusinessPartners.Addresses.Add();
                                //            //################################################################ - ADICIONANDO ENDEREÇO DE CONTATO
                                //            doc.BusinessPartners.ContactEmployees.Name = "TesteAdiçãoPn";
                                //            doc.BusinessPartners.ContactEmployees.Address = "EnderecoPN";
                                //            doc.BusinessPartners.ContactEmployees.E_Mail = "c1@abcd.com";
                                //            doc.BusinessPartners.ContactEmployees.Fax = "8433777778";
                                //            doc.BusinessPartners.ContactEmployees.MobilePhone = "8388888";
                                //            doc.BusinessPartners.ContactEmployees.Phone1 = "88880000";
                                //            doc.BusinessPartners.ContactEmployees.Name = "C2";
                                //            doc.BusinessPartners.ContactEmployees.Address = "BJ";
                                //            doc.BusinessPartners.ContactEmployees.E_Mail = "c2@abcd.com";
                                //            doc.BusinessPartners.ContactEmployees.Fax = "84338";
                                //            doc.BusinessPartners.ContactEmployees.MobilePhone = "877388888";
                                //            doc.BusinessPartners.ContactEmployees.Phone1 = "8888300";
                                //            doc.BusinessPartners.ContactEmployees.Add();
                                //            ret = doc.BusinessPartners.Add();
                                //            if (ret != 0)
                                //            {
                                //                comp.Company.GetLastError(out ret, out string msg);
                                //                LogFile = new GravarLog(LogFile).Escrever(msg + "- Erro ao adicionar");
                                //                return BadRequest(msg);
                                //            }
                                //            return Ok(itemBp);
                                //        #endregion

                                //        default:
                                //            LogFile = new GravarLog(LogFile).Escrever("ERRO FAVOR SELECIONAR ALGUM FLUXO 1= ADICIONAR APENAS O PN, 2 ADICIONAR O PN COM ENDEREÇOS DE COBRANÇA, 3 ADICIONA O PN E INFORMAÇOES DE CONTATO, 4 ADICIONA UM PN COM TODAS AS INFORMAÇOES DISPONIVEIS");
                                //            return BadRequest("ERRO FAVOR SELECIONAR ALGUM FLUXO 1= ADICIONAR APENAS O PN, 2 ADICIONAR O PN COM ENDEREÇOS DE COBRANÇA, 3 ADICIONA O PN E INFORMAÇOES DE CONTATO, 4 ADICIONA UM PN COM TODAS AS INFORMAÇOES DISPONIVEIS");

                                //    }
                                //}

                                #endregion
                            }
                        }
                    }
                }
                LogFile = new GravarLog(LogFile).Escrever("ERRO AO ADICIONAR PN TOKEN INVALIDO OU EXPIRADO");
                return BadRequest("ERRO AO ADICIONAR PN VERIFIQUE OS LOG");
            }
            catch (Exception ex)
            {

                return BadRequest(400 + ex.ToString());
            }

        }

        /// <summary>
        /// Manutenção
        /// </summary> 
        /// <returns></returns>          
        [System.Web.Http.Route("PutPn")]
        [ResponseType(typeof(BusinessPartnerModel))]
        [System.Web.Http.HttpPut]
        public IHttpActionResult PutPn(BusinessPartnerModel listModel)
        {
            try
            {
                LogFile = new GravarLog(LogFile).Escrever("#####################################################################");
                LogFile = new GravarLog(LogFile).Escrever("INICIANDO");

                foreach (var itemToken in TokenUtilizado)
                {
                    foreach (var itemBp in listModel.ListBlink)
                    {

                        if (itemToken.Token != null)
                        {
                            var (tokenValido, empresa) = new ValidarToken().ValidarTokenUtilizadoBP(itemToken.Token);
                            if (tokenValido)
                            {
                                #region VALIDANDDO BANCO DE DADOS 

                                if (empresa.Banco_Dados != empresa.Banco_Dados)
                                {
                                    LogFile = new GravarLog(LogFile).Escrever("BANCO DE DADOS INFORMADO INCORRETAMENTE!!");
                                    XmlDocument xmlDoc = new XmlDocument();
                                    xmlDoc.LoadXml(string.Format(@"<root>Banco de dados informado incorretamente.</root>"));
                                    TokenUtilizado = null;
                                    return BadRequest(xmlDoc.ToString());
                                }

                                LogFile = new GravarLog(LogFile).Escrever("VALIDANDO CONEXÃO COM SAP!");
                                CompanyList.ConexaoInicial(empresa);
                                var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == empresa.Banco_Dados).FirstOrDefault();

                                #endregion

                                #region VALIDACAO DE CAMPOS OBRIGATORIOS VAZIOS OU NULOS

                                if (string.IsNullOrEmpty(LogFile))
                                {
                                    LogFile = new GravarLog().Escrever("#################################");
                                }
                                else
                                {
                                    LogFile = new GravarLog(LogFile).Escrever("#################################");
                                }

                                //LogFile = new GravarLog(LogFile).Escrever("INICIADO MAPEAMENTO DE PARCEIRO DE NEGOCIO \n INICIADO VALIDAÇÃO DOS CAMPOS DE PARCEIRO DE NEGOCIO");
                                //if (listModel.ListBlink.Any(X => X.idClientes.Equals(string.Empty) && X.codigoCliente.Equals(string.Empty)))
                                //{
                                //    LogFile = new GravarLog(LogFile).Escrever("ERRO DE VALIDAÇÃO SEM CARD CODE");
                                //    return BadRequest("ERRO DE VALIDAÇÃO VOCÊ NÃO ADICIONOU O CODIGO DO PARCEIRO DE NEGOCIO(CARDCODE) ADICIONE AO CAMPO IDCLIENTE");
                                //}

                                #endregion                         

                                #region ATUALIZANDO PN
                                //using (var doc = new InstanciaSap(comp.Company))
                                //{
                                //    comp.Company.Connect();
                                //    if (doc.BusinessPartners.GetByKey(itemBp.codigoCliente))
                                //    {
                                //        doc.BusinessPartners.CardCode = itemBp.idClientes;
                                //        doc.BusinessPartners.CardCode = itemBp.codigoCliente;
                                //        doc.BusinessPartners.CardName = itemBp.razaoSocial;
                                //        doc.BusinessPartners.AliasName = itemBp.fantasia;
                                //        doc.BusinessPartners.FiscalTaxID.TaxId0 = itemBp.cnpj;
                                //        doc.BusinessPartners.FiscalTaxID.TaxId1 = itemBp.inscricaoEstadual;
                                //        doc.BusinessPartners.EmailAddress = itemBp.email;
                                //        doc.BusinessPartners.EmailAddress = itemBp.emailNfe;
                                //        doc.BusinessPartners.Valid = itemBp.ativo;
                                //        doc.BusinessPartners.PayTermsGrpCode = itemBp.codigoCondicaoPagamento;
                                //        doc.BusinessPartners.BPPaymentMethods.PaymentMethodCode = itemBp.codigoFormaPagamento;
                                //        doc.BusinessPartners.CardType = itemBp.codigoTipoPedido;
                                //        doc.BusinessPartners.DiscountPercent = itemBp.percentualDesconto;
                                //        doc.BusinessPartners.BPPaymentDates.PaymentDate = itemBp.dataUltimaCompra;
                                //        doc.BusinessPartners.GroupCode = itemBp.codigoGrupo;
                                //        doc.BusinessPartners.ChannelBP = itemBp.canal;
                                //        doc.BusinessPartners.Address = itemBp.endereco;
                                //        doc.BusinessPartners.Addresses.StreetNo = itemBp.numero;
                                //        doc.BusinessPartners.Addresses.BuildingFloorRoom = itemBp.complemento;
                                //        doc.BusinessPartners.Addresses.Block = itemBp.bairro;
                                //        doc.BusinessPartners.Addresses.City = itemBp.cidade;
                                //        doc.BusinessPartners.Addresses.State = itemBp.estado;
                                //        doc.BusinessPartners.Addresses.ZipCode = itemBp.cep;
                                //        doc.BusinessPartners.Phone1 = itemBp.telefone;
                                //        doc.BusinessPartners.Fax = itemBp.fax;
                                //        doc.BusinessPartners.Addresses.Country = itemBp.pais;
                                //        doc.BusinessPartners.CreditLimit = itemBp.limiteCredito;
                                //        var ret = doc.BusinessPartners.Update();
                                //        if (ret != 0)
                                //        {
                                //            comp.Company.GetLastError(out ret, out string msg);
                                //            return BadRequest(msg);
                                //        }


                                //    }
                                //}
                                //return Ok(itemBp);

                                #endregion

                            }
                        }
                    }
                }
                LogFile = new GravarLog(LogFile).Escrever("ERRO AO ADICIONAR PN TOKEN INVALIDO OU EXPIRADO");
                return BadRequest("ERRO AO ADICIONAR PN VERIFIQUE OS LOG");
            }
            catch (Exception ex)
            {

                return BadRequest(400 + ex.ToString());
            }

        }
        #endregion
    }

}