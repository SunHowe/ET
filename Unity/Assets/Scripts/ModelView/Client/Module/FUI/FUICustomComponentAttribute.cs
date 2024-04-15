using System;

namespace ET.Client
{
    /// <summary>
    /// 用于标记自定义组件
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class FUICustomComponentAttribute : BaseAttribute
    {
        public string URL { get; }

        public FUICustomComponentAttribute(string url)
        {
            this.URL = url;
        }
    }
}