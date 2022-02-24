using DefaultWebProject.Conexao;
using DefaultWebProject.Context;
using DefaultWebProject.Log;
using DefaultWebProject.Models;
using DefaultWebProject.Tokken;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using System.Xml;

namespace DefaultWebProject.Controllers
{
    public class ItemController : ApiController
    {
        private SBO_TesteContext db = new SBO_TesteContext();

        /// <summary>
        /// GetItemList
        /// </summary> 
        /// <returns></returns>  
        [EnableQuery]
        [System.Web.Http.Route("GetItemList")]
        [ResponseType(typeof(ItemModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetItemList()
        {
            IQueryable<ItemModel> query = QueryItems();
            return Ok(query);
        }

        /// <summary>
        /// GetItem(ID)
        /// </summary> 
        /// <returns></returns>  
        [System.Web.Http.Route("GetItem(ID)")]
        [ResponseType(typeof(ItemModel))]
        [System.Web.Http.HttpGet]
        [EnableQuery]
        public IHttpActionResult GetItem(string ID)
        {
            IQueryable<ItemModel> query = QueryItems();
            return Ok(query.Where(x=>x.ItemCode == ID));
        }

        // Metodo Auxiliar    
        public IQueryable<ItemModel> QueryItems()
        {
            try
            {
                return db.items;
            }
            catch (Exception ex)
            {

                return db.items;
            }
        }


    }
}