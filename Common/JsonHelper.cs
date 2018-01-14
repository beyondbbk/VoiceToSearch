using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Common.Attribute;

namespace Common
{
    public class JsonHelper
    {
        /// <summary>
        /// Json序列化，处理方式：命名风格为驼峰命名，时间格式：yyyy-MM-dd HH:mm:ss，null忽略
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="jssetting"></param>
        /// <returns></returns>
        public static string Serialize<T>(T t, JsonSerializerSettings jssetting)
        {
            if (jssetting == null)
            {
                jssetting = new JsonSerializerSettings();
                jssetting.NullValueHandling = NullValueHandling.Ignore;//忽略空值
                jssetting.ContractResolver = //改为驼峰命名风格
                    new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                jssetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";//时间格式
            }
            return JsonConvert.SerializeObject(t, jssetting);
        }

        public static string Serialize<T>(T t)
        {

            var jssetting = new JsonSerializerSettings();
            jssetting.NullValueHandling = NullValueHandling.Ignore;//忽略空值
            jssetting.ContractResolver = //改为驼峰命名风格
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            jssetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";//时间格式
            return JsonConvert.SerializeObject(t, jssetting);
        }


        /// <summary>
        /// Json反序列化，json -> onject转换
        /// </summary>
        /// <typeparam name="T">转换的目标类型</typeparam>
        /// <param name="json">Json数据</param>
        /// <returns>返回转换好的实例</returns>
        [MonitorException("将json数据反序列化成指定Model")]
        public static T Deserlize<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            return serializer.Deserialize(new JsonTextReader(sr), typeof(T)) as T;
        }

        /// <summary>
        /// 将json字符串反序列化为字典类型
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="jsonStr">json字符串</param>
        /// <returns>字典数据</returns>
        public static Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new Dictionary<TKey, TValue>();
            var jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);
            return jsonDict;
        }

        //public static XmlDocument

    }
}
