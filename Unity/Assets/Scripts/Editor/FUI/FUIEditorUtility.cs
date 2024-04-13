using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FairyGUI;
using FairyGUI.CodeGenerator;
using FairyGUI.CodeGenerator.Core;
using FairyGUI.CodeGenerator.Scriban;
using FairyGUI.Dynamic;
using FairyGUI.Dynamic.Editor;
using UnityEditor;

namespace ET
{
    public static class FUIEditorUtility
    {
        // ReSharper disable once MemberHidesStaticFromOuterClass
            private const string MenuRoot = "ET/FariyGUI";

            private const string MenuItemGenerateMapping = MenuRoot + "/Generate Mapping";
            private const string MenuItemGenerateCode = MenuRoot + "/Generate Code";

            private const string UIAssetsRoot = "Assets/Bundles/FUI";
            private const string UIMappingGeneratePath = UIAssetsRoot + "/" + nameof(UIPackageMapping) + ".asset";

            private const string ScribanTemplateRoot = "../Scriban";
            private const string UIBindingCodeGenerateRoot = "Assets/Scripts/HotfixView/Client/Demo/UIGen";
            private const string UILogicCodeGenerateRoot = "Assets/Scripts/HotfixView/Client/Demo/UI";

            private const string UINamespace = "GameLogic.UI";
            
            private sealed class UIAssetsImporter : AssetPostprocessor
            {
                private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
                {
                    var uiAssetsModified = false;
                    foreach (var asset in importedAssets)
                    {
                        // 检测是否在UIAssetsRoot目录下
                        if (!asset.StartsWith(UIAssetsRoot))
                            continue;
                        
                        if (!asset.EndsWith(".bytes")) 
                            continue;
                        
                        uiAssetsModified = true;
                        break;
                    }
                    
                    if (uiAssetsModified)
                    {
                        GenerateCode();
                        GenerateMapping();
                    }
                }
            }

            [MenuItem(MenuItemGenerateMapping, false)]
            public static void GenerateMapping()
            {
                UIPackageMappingUtility.GenerateMappingFile(UIAssetsRoot, UIMappingGeneratePath);
            }

            [MenuItem(MenuItemGenerateCode, false)]
            public static void GenerateCode()
            {
                var filter = new UIComponentFilter();
                
                if (!Directory.Exists(UIBindingCodeGenerateRoot))
                    Directory.CreateDirectory(UIBindingCodeGenerateRoot);
                
                // 记录此时的绑定文件列表
                var bindingFiles = Directory.GetFiles(UIBindingCodeGenerateRoot, "*.cs", SearchOption.AllDirectories);

                #region [生成UI绑定代码]

                // 每次都重新生成
                if (Directory.Exists(UIBindingCodeGenerateRoot))
                    Directory.Delete(UIBindingCodeGenerateRoot, true);
                Directory.CreateDirectory(UIBindingCodeGenerateRoot);

                UICodeGenerator.Generate(UIAssetsRoot, "_fui.bytes", new ScribanCodeGenerator(GetBindingCodeExportSettings), filter);

                #endregion

                #region [生成UI逻辑代码]

                // 一个文件只生成一次
                if (!Directory.Exists(UILogicCodeGenerateRoot))
                    Directory.CreateDirectory(UILogicCodeGenerateRoot);

                UICodeGenerator.Generate(UIAssetsRoot, "_fui.bytes", new ScribanCodeGenerator(GetLogicCodeExportSettings), filter);

                #endregion
                
                // 重新获取绑定文件列表 将不存在的绑定文件对应的逻辑文件删除
                var newBindingFiles = Directory.GetFiles(UIBindingCodeGenerateRoot, "*.cs", SearchOption.AllDirectories);
                foreach (var bindingFile in bindingFiles)
                {
                    if (newBindingFiles.Contains(bindingFile))
                        continue;

                    var logicFile = bindingFile.Replace(UIBindingCodeGenerateRoot, UILogicCodeGenerateRoot);
                    if (System.IO.File.Exists(logicFile))
                        System.IO.File.Delete(logicFile);
                }
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }

            private sealed class UIComponentFilter : IUIComponentFilter
            {
                public string Filter(UIComponent component)
                {
                    var exportType = GetExportType(component);
                    if (exportType == UIComponentExportType.None)
                        return component.ExtensionType.FullName;

                    return UINamespace + "." + component.PackageName + "." + component.Name;
                }
            }

            private static bool GetLogicCodeExportSettings(UIComponent component, out string templatePath, out string outputPath)
            {
                templatePath = string.Empty;
                outputPath = UILogicCodeGenerateRoot + "/" + component.PackageName + "/" + component.Name + ".cs";

                // 如果已经存在，则不再生成
                if (System.IO.File.Exists(outputPath))
                    return false;

                var exportType = GetExportType(component);
                switch (exportType)
                {
                    case UIComponentExportType.None:
                        return false;
                    case UIComponentExportType.UIForm:
                        templatePath = ScribanTemplateRoot + "/UIForm.tpl";
                        return true;
                    case UIComponentExportType.UIComponent:
                        templatePath = ScribanTemplateRoot + "/UIComponent.tpl";
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private static bool GetBindingCodeExportSettings(UIComponent component, out string templatePath, out string outputPath)
            {
                templatePath = string.Empty;
                outputPath = UIBindingCodeGenerateRoot + "/" + component.PackageName + "/" + component.Name + ".cs";

                var exportType = GetExportType(component);
                switch (exportType)
                {
                    case UIComponentExportType.None:
                        return false;
                    case UIComponentExportType.UIForm:
                        templatePath = ScribanTemplateRoot + "/UIForm.Binding.tpl";
                        return true;
                    case UIComponentExportType.UIComponent:
                        templatePath = ScribanTemplateRoot + "/UIComponent.Binding.tpl";
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private static UIComponentExportType GetExportType(UIComponent component)
            {
                // 以Form结尾的组件导出为UIForm
                if (component.Name.EndsWith("Form"))
                    return UIComponentExportType.UIForm;

                // 以Component结尾的组件导出为UIComponent
                if (component.Name.EndsWith("Component"))
                    return UIComponentExportType.UIComponent;

                // 当子节点中存在任意非拓展组件类型支持的自定义名字时 导出为UIComponent
                var extendTypeSupportNames = ExtendTypeSupportNames[component.ExtensionType];

                bool ExistsNotSupportName(UIComponentNode child)
                {
                    return !extendTypeSupportNames.Contains(child.Name) && !Regex.IsMatch(child.Name, @"^n[0-9]+$");
                }

                if (component.Nodes.Any(ExistsNotSupportName))
                    return UIComponentExportType.UIComponent;

                return UIComponentExportType.None;
            }

            private enum UIComponentExportType
            {
                None,
                UIForm,
                UIComponent,
            }

            private static readonly Dictionary<Type, List<string>> ExtendTypeSupportNames = new Dictionary<Type, List<string>>
            {
                { typeof(GButton), new List<string> { "icon", "title" } },
                { typeof(GLabel), new List<string> { "icon", "title" } },
                { typeof(GComboBox), new List<string> { "icon", "title" } },
                { typeof(GProgressBar), new List<string> { "title", "bar", "bar_v", "ani" } },
                { typeof(GSlider), new List<string> { "title", "bar", "bar_v", "grip" } },
                { typeof(GScrollBar), new List<string> { "grip", "bar", "arrow1", "arrow2" } },
                { typeof(GComponent), new List<string>() }
            };
    }
}