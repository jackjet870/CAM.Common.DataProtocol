using System;
using System.Collections.Generic;

namespace CAM.Common.DataProtocol
{
    [Serializable]
    public class CCData<TDataType>
    {
        public ErrorInfo error { get; set; }
        public DataInfo info { get; set; }
        public TDataType data { get; set; }

        public CCData()
        {
            error = new ErrorInfo();
            info = new DataInfo();
        }
    }


    [Serializable]
    public class ErrorInfo
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int errorNo { get; set; }
        /// <summary>
        /// 是否是错误描述信息
        /// </summary>
        public bool hasError { get; set; }
        /// <summary>
        /// 是否包含多条错误信息
        /// </summary>
        public bool isMultiError { get; set; }
        /// <summary>
        /// 错误描述（单条错误）
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 多条错误信息描述
        /// </summary>
        public List<MultiErrorInfo> multiMessage { get; set; }

        public ErrorInfo()
        {
            errorNo = 0;
            hasError = false;
            isMultiError = false;
            message = "";
            multiMessage = new List<MultiErrorInfo>();
        }
    }

    [Serializable]
    public class MultiErrorInfo
    {
        public string key { get; set; }
        public string message { get; set; }
        public MultiErrorInfo()
        {
            key = "";
            message = "";
        }
    }


    [Serializable]
    public class DataInfo
    {
        public DataType dataType { get; set; }
        public PageInfo pageInfo { get; set; }

        public DataInfo()
        {
            dataType = DataType.AsObject;
            pageInfo = new PageInfo();
        }
    }

    [Serializable]
    public enum DataType
    {
        /// <summary>
        /// 单一对象
        /// </summary>
        AsObject = 0,
        /// <summary>
        /// 普通集合对象
        /// </summary>
        AsList = 1,
        /// <summary>
        /// 带分页信息的集合对象
        /// </summary>
        AsPageList = 2,
        /// <summary>
        /// 值
        /// </summary>
        AsValue = 3,
    }

    [Serializable]
    public class PageInfo
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页面尺寸：每页包含多少条数据
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 页面数量
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 数据条数
        /// </summary>
        public int TotalCount { get; set; }

        public PageInfo()
        {
            PageIndex = 0;
            PageSize = 0;
            PageCount = 0;
            TotalCount = 0;
        }
    }
}
