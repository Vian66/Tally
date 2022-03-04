using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration; //for Config
using Tally.MyClass;
using Tally.Models;

namespace Tally.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IHttpClientFactory _clientFactory;
        Compress oCompress = new Compress();
        EncodeHelper oEncodeHelper = new EncodeHelper();
        public APIController(IConfiguration config, IHttpClientFactory clientFactory){
            _config = config;
            _clientFactory = clientFactory;
        }

        [HttpPost]
        [Route("Data/test")]
        public string test(param data)
        {
            return data.msg;
        }
        //======讀tallyAPI======//
        [HttpPost]
        [Route ("[action]")]
        public string ReadTally (JObject oJObject) {
            JObject jGetDataByte = new JObject ();
            jGetDataByte.Add (new JProperty ("EncodeServerName", oEncodeHelper.EnryptString ("W08DBRD01")));
            jGetDataByte.Add (new JProperty ("EncodeDBName", oEncodeHelper.EnryptString ("CS_V2_RD")));
            jGetDataByte.Add (new JProperty ("SPName", "TMT_QueryTallyProject"));//預存存序名稱
            jGetDataByte.Add (new JProperty ("params", oJObject));


            //去呼叫另一支API,主要是連資料庫用的
            byte[] resultByte = PostHttpRequest (jGetDataByte);
            String ResultStr = oCompress.DecompressString (resultByte);
            XElement xmlResult = new XElement ("Data");
            xmlResult = XElement.Parse (ResultStr);
            ResultStr = JsonConvert.SerializeObject (xmlResult);

            return ResultStr;
        }

        //======選tally內容======//
        [HttpPost]
        [Route ("[action]")]
        public string SelectTally (JObject oJObject) {
        //public string SelectTally (object oJObject) {
            JObject jGetDataByte = new JObject ();
            jGetDataByte.Add (new JProperty ("EncodeServerName", oEncodeHelper.EnryptString ("W08DBRD01")));
            jGetDataByte.Add (new JProperty ("EncodeDBName", oEncodeHelper.EnryptString ("CS_V2_RD")));
            jGetDataByte.Add (new JProperty ("SPName", "TMT_QueryTally"));//預存存序名稱
            jGetDataByte.Add (new JProperty ("Param", oJObject));



            //去呼叫另一支API,主要是連資料庫用的
            byte[] resultByte = PostHttpRequest (jGetDataByte);
            String ResultStr = oCompress.DecompressString (resultByte);
            XElement xmlResult = new XElement ("Data");
            xmlResult = XElement.Parse (ResultStr);
            ResultStr = JsonConvert.SerializeObject (xmlResult);

            return ResultStr;
        }

        //======Save tally======//
        [HttpPost]
        [Route ("[action]")]
        public string SaveTally (JObject oJObject) {
            JObject jGetDataByte = new JObject ();
            jGetDataByte.Add (new JProperty ("EncodeServerName", oEncodeHelper.EnryptString ("W08DBRD01")));
            jGetDataByte.Add (new JProperty ("EncodeDBName", oEncodeHelper.EnryptString ("CS_V2_RD")));
            jGetDataByte.Add (new JProperty ("SPName", "TMT_SaveTally_Temp"));//預存存序名稱
            jGetDataByte.Add (new JProperty ("Param", oJObject));


            //去呼叫另一支API,主要是連資料庫用的
            byte[] resultByte = PostHttpRequest (jGetDataByte);
            String ResultStr = oCompress.DecompressString (resultByte);
            XElement xmlResult = new XElement ("Data");
            xmlResult = XElement.Parse (ResultStr);
            ResultStr = JsonConvert.SerializeObject (xmlResult);

            return ResultStr;
        }

        //======Save tallyComplete======//
        [HttpPost]
        [Route ("[action]")]
        public string SaveTallyComplete () {
            JObject jGetDataByte = new JObject ();
            jGetDataByte.Add (new JProperty ("EncodeServerName", oEncodeHelper.EnryptString ("W08DBRD01")));
            jGetDataByte.Add (new JProperty ("EncodeDBName", oEncodeHelper.EnryptString ("CS_V2_RD")));
            jGetDataByte.Add (new JProperty ("SPName", "TMT_SaveTallyComplete"));//預存存序名稱
            //jGetDataByte.Add (new JProperty ("Param", oJObject));


            //去呼叫另一支API,主要是連資料庫用的
            byte[] resultByte = PostHttpRequest (jGetDataByte);
            String ResultStr = oCompress.DecompressString (resultByte);
            XElement xmlResult = new XElement ("Data");
            xmlResult = XElement.Parse (ResultStr);
            ResultStr = JsonConvert.SerializeObject (xmlResult);

            return ResultStr;
        }
        //======ClearTally_Temp======//
        public string ClearTallyTemp () {
            JObject jGetDataByte = new JObject ();
            jGetDataByte.Add (new JProperty ("EncodeServerName", oEncodeHelper.EnryptString ("W08DBRD01")));
            jGetDataByte.Add (new JProperty ("EncodeDBName", oEncodeHelper.EnryptString ("CS_V2_RD")));
            jGetDataByte.Add (new JProperty ("SPName","TMT_ClearSaveTally_Temp"));//預存存序名稱
            //jGetDataByte.Add (new JProperty ("Param", oJObject));


            //去呼叫另一支API,主要是連資料庫用的
            byte[] resultByte = PostHttpRequest (jGetDataByte);
            String ResultStr = oCompress.DecompressString (resultByte);
            XElement xmlResult = new XElement ("Data");
            xmlResult = XElement.Parse (ResultStr);
            ResultStr = JsonConvert.SerializeObject (xmlResult);

            return ResultStr;
        }
        



        
        /// <summary>
        /// 使用HttpPost呼叫API取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="requestUrl"></param>
        /// <param name="data"></param>
        /// <param name="RequestHeader"></param>
        /// <returns></returns>
        private byte[] PostHttpRequest (JObject data) {
            String strAPIMainUrl = "http://192.168.1.160:8080/OneDB/";

            var _client = _clientFactory.CreateClient ();
            _client.BaseAddress = new Uri (strAPIMainUrl);
            _client.DefaultRequestHeaders.Clear ();
            _client.DefaultRequestHeaders.Add ("Accept", "application/json");

            string _json = JsonConvert.SerializeObject (data);
            var _content = new StringContent (_json, System.Text.Encoding.UTF8, "application/json");
            var response = _client.PostAsync ("api/DB/GetDataByte", _content).Result;

            if (response.IsSuccessStatusCode){
                byte[] result = response.Content.ReadAsAsync<byte[]> ().Result;
                return result;
            }

            return null;
        }
        //API是負責連結資料庫用成站台方便大家共用，不用每次進行編碼編碼，並且傳參數接
        //資料庫的值，處理方式都寫在這支API裡

    }
}
