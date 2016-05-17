using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
/// <summary>
/// CommonJsonModel 的摘要说明
/// </summary>
public class CommonJsonModel: CommonJsonModelAnalyzer
{
    /// <summary>
    /// The rawjson
    /// </summary>
     private string rawjson;
     /// <summary>
     /// The is value
     /// </summary>
        private bool isValue = false;
        /// <summary>
        /// The is model
        /// </summary>
        private bool isModel = false;
        /// <summary>
        /// The is collection
        /// </summary>
        private bool isCollection = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonJsonModel" /> class.
        /// </summary>
        /// <param name="rawjson">The rawjson.</param>
        /// <exception cref="System.Exception">missing rawjson</exception>
        public CommonJsonModel(string rawjson)
        {
            this.rawjson = rawjson;
            if (string.IsNullOrEmpty(rawjson))
                throw new Exception("missing rawjson");
            rawjson = rawjson.Trim();
            if (rawjson.StartsWith("{"))
            {
                isModel = true;
            }
            else if (rawjson.StartsWith("["))
            {
                isCollection = true;
            }
            else
            {
                isValue = true;
            }
        }
        /// <summary>
        /// Gets the rawjson.
        /// </summary>
        /// <value>
        /// The rawjson.
        /// </value>
        public string Rawjson
        {
            get { return rawjson; }
        }
        /// <summary>
        /// Determines whether this instance is value.
        /// </summary>
        /// <returns></returns>
        public bool IsValue()
        {
            return isValue;
        }
        /// <summary>
        /// Determines whether the specified key is value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool IsValue(string key)
        {
            if (!isModel)
                return false;
            if (string.IsNullOrEmpty(key))
                return false;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                CommonJsonModel model = new CommonJsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                {
                    CommonJsonModel submodel = new CommonJsonModel(model.Value);
                    return submodel.IsValue();
                }
            }
            return false;
        }
        /// <summary>
        /// Determines whether this instance is model.
        /// </summary>
        /// <returns></returns>
        public bool IsModel()
        {
            return isModel;
        }
        /// <summary>
        /// Determines whether the specified key is model.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool IsModel(string key)
        {
            if (!isModel)
                return false;
            if (string.IsNullOrEmpty(key))
                return false;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                CommonJsonModel model = new CommonJsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                {
                    CommonJsonModel submodel = new CommonJsonModel(model.Value);
                    return submodel.IsModel();
                }
            }
            return false;
        }
        /// <summary>
        /// Determines whether this instance is collection.
        /// </summary>
        /// <returns></returns>
        public bool IsCollection()
        {
            return isCollection;
        }
        /// <summary>
        /// Determines whether the specified key is collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool IsCollection(string key)
        {
            if (!isModel)
                return false;
            if (string.IsNullOrEmpty(key))
                return false;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                CommonJsonModel model = new CommonJsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                {
                    CommonJsonModel submodel = new CommonJsonModel(model.Value);
                    return submodel.IsCollection();
                }
            }
            return false;
        }

        /// <summary>
        /// 当模型是对象，返回拥有的key
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeys()
        {
            if (!isModel)
                return null;
            List<string> list = new List<string>();
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                string key = new CommonJsonModel(subjson).Key;
                if (!string.IsNullOrEmpty(key))
                    list.Add(key);
            }
            return list;
        }
        /// <summary>
        /// 当模型是对象，key对应是值，则返回key对应的值
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (!isModel)
                return null;
            if (string.IsNullOrEmpty(key))
                return null;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                CommonJsonModel model = new CommonJsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                    return model.Value;
            }
            return null;
        }
        /// <summary>
        /// 模型是对象，key对应是对象，返回key对应的对象
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public CommonJsonModel GetModel(string key)
        {
            if (!isModel)
                return null;
            if (string.IsNullOrEmpty(key))
                return null;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                CommonJsonModel model = new CommonJsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                {
                    CommonJsonModel submodel = new CommonJsonModel(model.Value);
                    if (!submodel.IsModel())
                        return null;
                    else
                        return submodel;
                }
            }
            return null;
        }
        /// <summary>
        /// 模型是对象，key对应是集合，返回集合
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public CommonJsonModel GetCollection(string key)
        {
            if (!isModel)
                return null;
            if (string.IsNullOrEmpty(key))
                return null;
            foreach (string subjson in base._GetCollection(this.rawjson))
            {
                CommonJsonModel model = new CommonJsonModel(subjson);
                if (!model.IsValue())
                    continue;
                if (model.Key == key)
                {
                    CommonJsonModel submodel = new CommonJsonModel(model.Value);
                    if (!submodel.IsCollection())
                        return null;
                    else
                        return submodel;
                }
            }
            return null;
        }
        /// <summary>
        /// 模型是集合，返回自身
        /// </summary>
        /// <returns></returns>
        public List<CommonJsonModel> GetCollection()
        {
            List<CommonJsonModel> list = new List<CommonJsonModel>();
            if (IsValue())
                return list;
            foreach (string subjson in base._GetCollection(rawjson))
            {
                list.Add(new CommonJsonModel(subjson));
            }
            return list;
        }


        /// <summary>
        /// 当模型是值对象，返回key
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        private string Key
        {
            get
            {
                if (IsValue())
                    return base._GetKey(rawjson);
                return null;
            }
        }
        /// <summary>
        /// 当模型是值对象，返回value
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        private string Value
        {
            get
            {
                if (!IsValue())
                    return null;
                return base._GetValue(rawjson);
            }
        }
    
}