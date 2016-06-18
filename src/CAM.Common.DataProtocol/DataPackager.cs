using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CAM.Common.DataProtocol
{
    /// <summary>
    /// CAM框架内标准通讯数据格式包装器
    /// </summary>
    public class DataPackager
    {

        private static string convertObjectToJson(object obj)
        {
            string json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, });
            return json;
        }

        /// <summary>
        /// 包装一条错误信息
        /// </summary>
        /// <param name="errorMessage">错误描述文字</param>
        /// <param name="errorNo">错误码：999表示是未被限定的任意错误</param>
        /// <returns></returns>
        public static string packError(string errorMessage, int errorNo=999)
        {
            CCData<object> rd = new CCData<object>()
            {
                error = new ErrorInfo()
                {
                    errorNo = errorNo,
                    hasError = true,
                    message = errorMessage,
                },
            };
            return convertObjectToJson(rd);
        }

        /// <summary>
        /// 包装一条错误信息
        /// 支持包装混合型错误
        /// </summary>
        /// <param name="ex">错误信息</param>
        /// <param name="errorNo">错误码：999表示是未被限定的任意错误</param>
        /// <returns></returns>
        public static string packError(Exception ex, int errorNo = 999)
        {
            if (ex.Data.Count == 0)
            {
                return packError(ex.Message, errorNo);
            }
            else
            {
                CCData<object> rd = new CCData<object>()
                {
                    error = new ErrorInfo()
                    {
                        errorNo = errorNo,
                        hasError = true,
                        message = "",
                        isMultiError = true,
                    },
                };

                foreach (string key in ex.Data.Keys)
                {
                    rd.error.multiMessage.Add(new MultiErrorInfo() { key = key, message = ex.Data[key].ToString(), });
                }
                return convertObjectToJson(rd);
            }

        }



        /// <summary>
        /// 包装一个实例对象
        /// 或者是Mixin模式的组合对象
        /// </summary>
        /// <typeparam name="TObjectType">对象类型</typeparam>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        public static string packIt<TObjectType>(TObjectType sourceObject)
        {
            if (sourceObject == null)
            {
                return packError(new Exception("没有找到指定数据"));
            }

            CCData<TObjectType> rd = new CCData<TObjectType>()
            {
                data = sourceObject,
                info = new DataInfo()
                {
                    dataType = sourceObject.GetType().IsClass ? DataType.AsObject : DataType.AsValue,
                },
            };
            return convertObjectToJson(rd);
        }

        /// <summary>
        /// 包装一个Mixin模式的列表对象
        /// </summary>
        /// <typeparam name="TObjectType"></typeparam>
        /// <param name="sourceObjects"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public static string packIt<TObjectType>(TObjectType sourceObjects, PageInfo pageInfo)
        {
            if (sourceObjects == null)
            {
                return packError(new Exception("没有找到指定数据"));
            }
            if (pageInfo.PageSize == 0)
            {
                return packIt<TObjectType>(sourceObjects);
            }
            else
            {
                CCData<TObjectType> rd = new CCData<TObjectType>()
                {
                    info = new DataInfo()
                    {
                        dataType = DataType.AsPageList,
                        pageInfo = pageInfo,
                    },
                    data = sourceObjects,
                };
                return convertObjectToJson(rd);
            }
        }

        /// <summary>
        /// 解包还原object类型的对象
        /// </summary>
        /// <typeparam name="TObjectType"></typeparam>
        /// <param name="sourceObjectString"></param>
        /// <returns></returns>
        public static TObjectType unpackIt<TObjectType>(string sourceObjectString)
        {
            CCData<TObjectType> rd = JsonConvert.DeserializeObject<CCData<TObjectType>>(sourceObjectString);
            return rd.data;
        }

        /// <summary>
        /// 包装列表数据对象
        /// </summary>
        /// <typeparam name="TObjectType"></typeparam>
        /// <param name="sourceObjects"></param>
        /// <returns></returns>
        public static string packIt<TObjectType>(List<TObjectType> sourceObjects)
        {
            if (sourceObjects == null)
            {
                return packError(new Exception("没有找到指定数据"));
            }
            CCData<List<TObjectType>> rd = new CCData<List<TObjectType>>()
            {
                info = new DataInfo()
                {
                    dataType = DataType.AsList,
                },
                data = sourceObjects,
            };
            return convertObjectToJson(rd);
        }

        /// <summary>
        /// 包装带分页信息的列表数据对象
        /// </summary>
        /// <typeparam name="TObjectType"></typeparam>
        /// <param name="sourceObjects"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public static string packIt<TObjectType>(List<TObjectType> sourceObjects, PageInfo pageInfo)
        {
            if (sourceObjects == null)
            {
                return packError(new Exception("没有找到指定数据"));
            }
            if (pageInfo.PageSize == 0)
            {
                return packIt<TObjectType>(sourceObjects);
            }
            else
            {
                CCData<List<TObjectType>> rd = new CCData<List<TObjectType>>()
                {
                    info = new DataInfo()
                    {
                        dataType = DataType.AsPageList,
                        pageInfo = pageInfo,
                    },
                    data = sourceObjects,
                };
                return convertObjectToJson(rd);
            }
        }

        public static List<TObjectType> unpackList<TObjectType>(string sourceObjectsString)
        {
            CCData<List<TObjectType>> rd = JsonConvert.DeserializeObject<CCData<List<TObjectType>>>(sourceObjectsString);
            return rd.data;
        }

        public static List<TObjectType> unpackList<TObjectType>(string sourceObjectsString, out PageInfo pageInfo)
        {
            CCData<List<TObjectType>> rd = JsonConvert.DeserializeObject<CCData<List<TObjectType>>>(sourceObjectsString);
            pageInfo = rd.info.pageInfo;
            return rd.data;
        }

        public static PageInfo getPackagePageInfo(string sourceObjectsString)
        {
            CCData<object> rd = JsonConvert.DeserializeObject<CCData<object>>(sourceObjectsString);
            return rd.info.pageInfo;
        }

        public static ErrorInfo getPackageErrorInfo(string sourceObjectsString)
        {
            CCData<object> rd = JsonConvert.DeserializeObject<CCData<object>>(sourceObjectsString);
            return rd.error;
        }

    }
}
