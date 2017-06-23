using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Http;
using Crestron.SimplSharp.Net;

namespace ILiveSmart
{
    public class ALiYunSMS
    {
        
        //入侵提醒SMS_46695144 您的${zonemsg}有人非法入侵，请您确认！
        /*
         * AppKey	23635259 
         * AppSecret 52ee58cd2e6c4742a187c30e6d917b2f
         * AppCode 32519182942043718bc520d43f50b471	
         */
        private const string APPCode = "APPCODE f196cea6df5949ec8590f59a77f5ff79";
        /// <summary>
        /// SMS_46780073 非正常开门 您好，您的朋友${uname}正在使用非正常指纹回家，请妥善处理！！！
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="mobile">手机</param>
        public void SendUnLockMsg(string name,string mobile)
        {
            String querys = string.Format("ParamString={0}&RecNum={1}&SignName={2}&TemplateCode=SMS_48945056", HttpUtility.UrlEncode("{\"uname\":\"" + name + "\"}", System.Text.Encoding.UTF8), mobile, HttpUtility.UrlEncode("艾力智能", System.Text.Encoding.UTF8));

            String url = "http://sms.market.alicloudapi.com/singleSendSms?"+querys;
            //ParamString%3d+%7b%22uname%22%3a%2213867911360%22%7d%26RecNum%3d13867911360%26SignName%3d%e8%89%be%e5%8a%9b%e9%9b%86%e6%88%90%26TemplateCode%3dSMS_46780073
            Crestron.SimplSharp.Net.Http.HttpClient client = new Crestron.SimplSharp.Net.Http.HttpClient();
            
            HttpClientRequest request = this.GetRequest(url);

           
            try
            {
                HttpClientResponse httpResponse = client.Dispatch(request);
                ILiveDebug.Instance.WriteLine("SMS:" + querys);
                httpResponse.Encoding = Encoding.UTF8;
                string html = httpResponse.ContentString;
                ILiveDebug.Instance.WriteLine(html);
            }
            catch (Exception ex)
            {

                ILiveDebug.Instance.WriteLine("ex:"+ex.Message);
            }
        }
        /// <summary>
        /// 入侵提醒SMS_46695144 您的${zonemsg}有人非法入侵，请您确认！
        /// </summary>
        /// <param name="zongmsg">报警区域</param>
        /// <param name="mobile">手机</param>
        public void SendAlarmMsg(string zongmsg, string mobile)
        {
            String querys = string.Format("ParamString={0}&RecNum={1}&SignName={2}&TemplateCode=SMS_48575136", HttpUtility.UrlEncode("{\"zonemsg\":\"" + zongmsg + "\"}", System.Text.Encoding.UTF8), mobile, HttpUtility.UrlEncode("艾力智能", System.Text.Encoding.UTF8));
            String url = "http://sms.market.alicloudapi.com/singleSendSms?" + querys;
            Crestron.SimplSharp.Net.Http.HttpClient client = new Crestron.SimplSharp.Net.Http.HttpClient();

            HttpClientRequest request = this.GetRequest(url);


            try
            {
                HttpClientResponse httpResponse = client.Dispatch(request);
                httpResponse.Encoding = Encoding.UTF8;
                string html = httpResponse.ContentString;
            }
            catch (Exception ex)
            {

                ILiveDebug.Instance.WriteLine("ex:" + ex.Message);
            }
        }

        public HttpClientRequest GetRequest(string aUrl)
        {
            HttpClientRequest httpClientRequest = new HttpClientRequest();
            httpClientRequest.Url.Parse(aUrl);
            httpClientRequest.Header.SetHeaderValue("Accept", "application/json");
            httpClientRequest.Header.SetHeaderValue("Authorization", APPCode);
            httpClientRequest.KeepAlive = true;
            return httpClientRequest;
        }
    }
}

