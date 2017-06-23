using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ILiveSmart
{
    /// <summary>
    /// PDU编解码类，完成PDU短信格式的编码与解码
    /// 目前只能发送接收USC2编码（中文编码）的
    /// 代码不是很安全，投入使用的话需要一定的改动 
    /// 短信右7bit编码格式，我没有做判断 可能会引发异常
    /// 私有类，只能命名空间内部使用 调试此类时须设为公有 完成后去掉public
    /// </summary>
    class PDUEncoding
    {
        private string serviceCenterAddress = "00";
        /// <summary>
        /// 消息服务中心(1-12个8位组)
        /// </summary>
        public string ServiceCenterAddress
        {
            get
            {
                int len = 2 * Convert.ToInt32(serviceCenterAddress.Substring(0, 2));
                string result = serviceCenterAddress.Substring(4, len - 2);

                result = ParityChange(result);
                result = result.TrimEnd('F', 'f');
                return result;
            }
            set                 //
            {
                if (value == null || value.Length == 0)      //号码为空
                {
                    serviceCenterAddress = "00";
                }
                else
                {
                    if (value[0] == '+')
                    {
                        value = value.TrimStart('+');
                    }
                    if (value.Substring(0, 2) != "86")
                    {
                        value = "86" + value;
                    }
                    value = "91" + ParityChange(value);
                    serviceCenterAddress = (value.Length / 2).ToString("X2") + value;
                }

            }
        }

        private string protocolDataUnitType = "11";
        /// <summary>
        /// 协议数据单元类型(1个8位组)
        /// </summary>
        public string ProtocolDataUnitType
        {
            set
            {

            }
            get
            {
                return "11";
            }
        }

        private string messageReference = "00";
        /// <summary>
        /// 所有成功的短信发送参考数目（0..255）
        /// (1个8位组)
        /// </summary>
        public string MessageReference
        {
            get
            {
                return "00";
            }
        }

        private string originatorAddress = "00";
        /// <summary>
        /// 发送方地址（手机号码）(2-12个8位组)
        /// </summary>
        public string OriginatorAddress
        {
            get
            {
                int len = Convert.ToInt32(originatorAddress.Substring(0, 2), 16);    //十六进制字符串转为整形数据
                string result = string.Empty;
                if (len % 2 == 1)       //号码长度是奇数，长度加1 编码时加了F
                {
                    len++;
                }
                result = originatorAddress.Substring(4, len);
                result = ParityChange(result).TrimEnd('F', 'f');    //奇偶互换，并去掉结尾F

                return result;
            }
        }

        private string destinationAddress = "00";
        /// <summary>
        /// 接收方地址（手机号码）(2-12个8位组)
        /// </summary>
        public string DestinationAddress
        {
            set
            {
                if (value == null || value.Length == 0)      //号码为空
                {
                    destinationAddress = "00";
                }
                else
                {
                    if (value[0] == '+')
                    {
                        value = value.TrimStart('+');
                    }
                    if (value.Substring(0, 2) == "86")
                    {
                        value = value.TrimStart('8', '6');
                    }
                    int len = value.Length;
                    value = ParityChange(value);

                    destinationAddress = len.ToString("X2") + "A1" + value;
                }
            }
        }

        private string protocolIdentifer = "00";
        /// <summary>
        /// 参数显示消息中心以何种方式处理消息内容
        /// （比如FAX,Voice）(1个8位组)
        /// </summary>
        public string ProtocolIdentifer
        {
            get
            {
                return protocolIdentifer;
            }
            set
            {

            }
        }

        private string dataCodingScheme = "08";     //暂时仅支持国内USC2编码
        /// <summary>
        /// 参数显示用户数据编码方案(1个8位组)
        /// </summary>
        public string DataCodingScheme
        {
            get
            {
                return dataCodingScheme;
            }
        }

        private string serviceCenterTimeStamp = "";
        /// <summary>
        /// 消息中心收到消息时的时间戳(7个8位组)
        /// </summary>
        public string ServiceCenterTimeStamp
        {
            get
            {
                string result = ParityChange(serviceCenterTimeStamp);
                result = "20" + result.Substring(0, 12);            //年加开始的“20”

                return result;
            }
        }

        private string validityPeriod = "C4";       //暂时固定有效期
        /// <summary>
        /// 短消息有效期(0,1,7个8位组)
        /// </summary>
        public string ValidityPeriod
        {
            get
            {
                return "C4";
            }
        }

        private string userDataLenghth = "";
        /// <summary>
        /// 用户数据长度(1个8位组)
        /// </summary>
        public string UserDataLenghth
        {
            get
            {
                return (userData.Length / 2).ToString("X2");
            }
        }

        private string userData = "";
        /// <summary>
        /// 用户数据(0-140个8位组)
        /// </summary>
        public string UserData
        {
            get
            {
                int len = Convert.ToInt32(userDataLenghth, 16) * 2;
                string result = string.Empty;

                if (dataCodingScheme == "08" || dataCodingScheme == "18")             //USC2编码
                {
                    //四个一组，每组译为一个USC2字符
                    for (int i = 0; i < len; i += 4)
                    {
                        string temp = userData.Substring(i, 4);

                        int byte1 = Convert.ToInt16(temp, 16);

                        result += ((char)byte1).ToString();
                    }
                }
                else
                {
                    result = PDU7bitDecoder(userData);
                }

                return result;
            }
            set
            {
                userData = string.Empty;
                Encoding encodingUTF = Encoding.BigEndianUnicode;

                byte[] Bytes = encodingUTF.GetBytes(value);

                for (int i = 0; i < Bytes.Length; i++)
                {
                    userData += BitConverter.ToString(Bytes, i, 1);
                }
                userDataLenghth = (userData.Length / 2).ToString("X2");
            }
        }


        /// <summary>
        /// 奇偶互换 (+F)
        /// </summary>
        /// <param name="str">要被转换的字符串</param>
        /// <returns>转换后的结果字符串</returns>
        private string ParityChange(string str)
        {
            string result = string.Empty;

            if (str.Length % 2 != 0)         //奇字符串 补F
            {
                str += "F";
            }
            for (int i = 0; i < str.Length; i += 2)
            {
                result += str[i + 1];
                result += str[i];
            }

            return result;
        }

        /// <summary>
        /// PDU编码器，完成PDU编码(USC2编码，最多70个字)
        /// </summary>
        /// <param name="phone">目的手机号码</param>
        /// <param name="Text">短信内容</param>
        /// <returns>编码后的PDU字符串</returns>
        public string PDUEncoder(string phone, string Text)
        {
            if (Text.Length > 70)
            {
                throw (new Exception("短信字数超过70"));
            }
            DestinationAddress = phone;
            UserData = Text;

            return serviceCenterAddress + protocolDataUnitType
                + messageReference + destinationAddress + protocolIdentifer
                + dataCodingScheme + validityPeriod + userDataLenghth + userData;
        }

        /// <summary>
        /// 完成手机或短信猫收到PDU格式短信的解码 暂时仅支持中文编码
        /// 未用DCS部分
        /// </summary>
        /// <param name="strPDU">短信PDU字符串</param>
        /// <param name="msgCenter">短消息服务中心 输出</param>
        /// <param name="phone">发送方手机号码 输出</param>
        /// <param name="msg">短信内容 输出</param>
        /// <param name="time">时间字符串 输出</param>
        public void PDUDecoder(string strPDU, out string msgCenter, out string phone, out string msg, out string time)
        {
            int lenSCA = Convert.ToInt32(strPDU.Substring(0, 2), 16) * 2 + 2;       //短消息中心占长度
            serviceCenterAddress = strPDU.Substring(0, lenSCA);

            int lenOA = Convert.ToInt32(strPDU.Substring(lenSCA + 2, 2), 16);           //OA占用长度
            if (lenOA % 2 == 1)                                                     //奇数则加1 F位
            {
                lenOA++;
            }
            lenOA += 4;                 //加号码编码的头部长度
            originatorAddress = strPDU.Substring(lenSCA + 2, lenOA);

            dataCodingScheme = strPDU.Substring(lenSCA + lenOA + 4, 2);             //DCS赋值，区分解码7bit

            serviceCenterTimeStamp = strPDU.Substring(lenSCA + lenOA + 6, 14);

            userDataLenghth = strPDU.Substring(lenSCA + lenOA + 20, 2);
            int lenUD = Convert.ToInt32(userDataLenghth, 16) * 2;
            userData = strPDU.Substring(lenSCA + lenOA + 22);

            msgCenter = ServiceCenterAddress;
            phone = OriginatorAddress;
            msg = UserData;
            time = ServiceCenterTimeStamp;
        }

        /// <summary>
        /// PDU7bit的解码，供UserData的get访问器调用
        /// </summary>
        /// <param name="len">用户数据长度</param>
        /// <param name="userData">数据部分PDU字符串</param>
        /// <returns></returns>
        private string PDU7bitDecoder(string userData)
        {
            string result = string.Empty;
            byte[] b = new byte[100];
            string temp = string.Empty;

            for (int i = 0; i < userData.Length; i += 2)
            {
                b[i / 2] = (byte)Convert.ToByte((userData[i].ToString() + userData[i + 1].ToString()), 16);
            }

            int j = 0;            //while计数
            int tmp = 1;            //temp中二进制字符字符个数
            while (j < userData.Length / 2 - 1)
            {
                string s = string.Empty;

                s = Convert.ToString(b[j], 2);

                while (s.Length < 8)            //s补满8位 byte转化来的 有的不足8位，直接解码将导致错误
                {
                    s = "0" + s;
                }

                result += (char)Convert.ToInt32(s.Substring(tmp) + temp, 2);        //加入一个字符 结果集 temp 上一位组剩余

                temp = s.Substring(0, tmp);             //前一位组多的部分

                if (tmp > 6)                            //多余的部分满7位，加入一个字符
                {
                    result += (char)Convert.ToInt32(temp, 2);
                    temp = string.Empty;
                    tmp = 0;
                }

                tmp++;
                j++;

                if (j == userData.Length / 2 - 1)           //最后一个字符
                {
                    result += (char)Convert.ToInt32(Convert.ToString(b[j], 2) + temp, 2);
                }
            }
            return result;
        }
    }
}