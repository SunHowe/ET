namespace ET.Client
{
    public static class FUIConfig
    {
        /// <summary>
        /// 是否开启模糊遮罩层
        /// </summary>
        public static bool EnableBlurMaskLayer { get; set; }

        /// <summary>
        /// 模糊遮罩降采样率
        /// </summary>
        public static int MaskLayerDownSample { get; set; } = 3;

        /// <summary>
        /// 模糊遮罩层模糊尺寸
        /// </summary>
        public static float MaskLayerBlurSize { get; set; } = 1f;
    }
}