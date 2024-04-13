using System.IO;
using YooAsset.Editor;

namespace ET
{
    [DisplayName("收集bytes文件")]
    public class CollectBytes : IFilterRule
    {
        public bool IsCollectAsset(FilterRuleData data)
        {
            return Path.GetExtension(data.AssetPath) == ".bytes";
        }
    }
}