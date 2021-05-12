using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZswBlog.Common.Util
{
    /// <summary>
    /// Json文件读写
    /// 引用Newtonsoft.Json
    /// </summary>
    public class JsonFileHelper
    {
        //注意：section为根节点
        private string _jsonName;
        private string _path;
        private IConfiguration Configuration { get; set; }
        public JsonFileHelper(string jsonName)
        {
            _jsonName = jsonName;
            if (!jsonName.EndsWith(".json"))
                _path = $"{jsonName}.json";
            else
                _path = jsonName;
            //ReloadOnChange = true 当*.json文件被修改时重新加载            
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = _path, ReloadOnChange = true, Optional = true })
            .Build();
        }

        /// <summary>
        /// 读取Json返回实体对象
        /// </summary>
        /// <returns></returns>
        public T Read<T>() => Read<T>("");

        /// <summary>
        /// 根据节点读取Json返回实体对象
        /// </summary>
        /// <returns></returns>
        public T Read<T>(string section)
        {
            try
            {
                using (var file = new StreamReader(_path))
                using (var reader = new JsonTextReader(file))
                {
                    var jObj = (JObject)JToken.ReadFrom(reader);
                    if (!string.IsNullOrWhiteSpace(section))
                    {
                        var secJt = jObj[section];
                        if (secJt != null)
                        {
                            return JsonConvert.DeserializeObject<T>(secJt.ToString());
                        }
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<T>(jObj.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return default(T);
        }

        /// <summary>
        /// 读取Json返回集合
        /// </summary>
        /// <returns></returns>
        public List<T> ReadList<T>() => ReadList<T>("");

        /// <summary>
        /// 根据节点读取Json返回集合
        /// </summary>
        /// <returns></returns>
        public List<T> ReadList<T>(string section)
        {
            try
            {
                using (var file = new StreamReader(_path))
                using (var reader = new JsonTextReader(file))
                {
                    var jObj = (JObject)JToken.ReadFrom(reader);
                    if (!string.IsNullOrWhiteSpace(section))
                    {
                        var secJt = jObj[section];
                        if (secJt != null)
                        {
                            return JsonConvert.DeserializeObject<List<T>>(secJt.ToString());
                        }
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<List<T>>(jObj.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return default(List<T>);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <param name="t"></param>
        public void Write<T>(T t) => Write("", t);

        /// <summary>
        /// 写入指定section文件
        /// </summary>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <param name="t"></param>
        public void Write<T>(string section, T t)
        {
            try
            {
                JObject jObj;
                using (StreamReader file = new StreamReader(_path))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    jObj = (JObject)JToken.ReadFrom(reader);
                    var json = JsonConvert.SerializeObject(t);
                    if (string.IsNullOrWhiteSpace(section))
                        jObj = JObject.Parse(json);
                    else
                        jObj[section] = JObject.Parse(json);
                }

                using (var writer = new StreamWriter(_path))
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jObj.WriteTo(jsonWriter);
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 删除指定section节点
        /// </summary>
        /// <param name="section"></param>
        public void Remove(string section)
        {
            try
            {
                JObject jObj;
                using (StreamReader file = new StreamReader(_path))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    jObj = (JObject)JToken.ReadFrom(reader);
                    jObj.Remove(section);
                }

                using (var writer = new StreamWriter(_path))
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jObj.WriteTo(jsonWriter);
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 读取文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<T> GetConfig<T>(string fileName)
        {
            return await Task.Run(() =>
            {
                JsonFileHelper jsonFileHelper = new JsonFileHelper(fileName);
                T jObject = jsonFileHelper.Read<T>();
                return jObject;
            });
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<bool> SetConfig<T>(string fileName, T data)
        {
            JsonFileHelper jsonFileHelper = new JsonFileHelper(fileName);
            Dictionary<string, T> json = new Dictionary<string, T>
            {
                { "data", data }
            };
            jsonFileHelper.Write(json);
            JObject jObject = await GetConfig<JObject>(fileName);
            string str = jObject.GetValue("data").ToString();
            T baseConfigs = JsonConvert.DeserializeObject<T>(str);
            return baseConfigs != null && !baseConfigs.Equals(data);
        }
    }
}
