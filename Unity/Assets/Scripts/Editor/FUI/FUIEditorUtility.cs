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

            private const string ScribanTemplateRoot = "Assets/Config/Scriban";
            
            private const string FUIBindingGenerateRoot = "Assets/Scripts/ModelView/Client/Demo/FUI";
            private const string FUISystemBindingGenerateRoot = "Assets/Scripts/HotfixView/Client/Demo/FUIGen";
            private const string FUISystemLogicGenerateRoot = "Assets/Scripts/HotfixView/Client/Demo/FUI";
            private const string FUIEnumFilePath = "Assets/Scripts/ModelView/Client/Module/FUI/FUIViewId.cs";

            private const string UINamespace = "ET.Client";
            
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
                UIComponentFilter filter = new UIComponentFilter();

                #region [生成FUI绑定代码]

                // 每次都删除重建
                if (Directory.Exists(FUIBindingGenerateRoot))
                    Directory.Delete(FUIBindingGenerateRoot, true);

                Directory.CreateDirectory(FUIBindingGenerateRoot);
                
                UICodeGenerator.Generate(UIAssetsRoot, "_fui.bytes", new ScribanCodeGenerator(GetFUIBindingCodeExportSettings), filter);

                #endregion

                #region [生成FUISystem绑定代码与FUIEventHandler绑定代码]

                // 每次都删除重建
                if (Directory.Exists(FUISystemBindingGenerateRoot))
                    Directory.Delete(FUISystemBindingGenerateRoot, true);

                Directory.CreateDirectory(FUISystemBindingGenerateRoot);
                
                UICodeGenerator.Generate(UIAssetsRoot, "_fui.bytes", new ScribanCodeGenerator(GetFUISystemBindingCodeExportSettings), filter);
                UICodeGenerator.Generate(UIAssetsRoot, "_fui.bytes", new ScribanCodeGenerator(GetEventHandlerBindingCodeExportSettings), filter);

                #endregion

                #region [生成FUISystem逻辑代码]

                // 只生成一次
                if (!Directory.Exists(FUISystemLogicGenerateRoot))
                    Directory.CreateDirectory(FUISystemLogicGenerateRoot);
                
                UICodeGenerator.Generate(UIAssetsRoot, "_fui.bytes", new ScribanCodeGenerator(GetFUISystemLogicCodeExportSettings), filter);

                #endregion

                #region [生成界面枚举id]

                if (File.Exists(FUIEnumFilePath))
                    File.Delete(FUIEnumFilePath);
                
                FUIEnumCodeGenerator enumGenerator = new(FUIEnumFilePath, GetFUIViewEnumName);
                UICodeGenerator.Generate(UIAssetsRoot, "_fui.bytes", enumGenerator, filter);
                enumGenerator.Flush();

                #endregion
                
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

            private static bool GetFUIBindingCodeExportSettings(UIComponent component, out string templatePath, out string outputPath)
            {
                templatePath = string.Empty;
                outputPath = string.Empty;
                
                UIComponentExportType exportType = GetExportType(component);
                if (exportType != UIComponentExportType.UIForm)
                    return false;
                
                // 只生成UIForm代码
                templatePath = ScribanTemplateRoot + "/FUI.Binding.tpl";
                outputPath = FUIBindingGenerateRoot + "/" + component.PackageName + "/" + component.Name + ".cs";
                return true;
            }

            private static bool GetFUISystemBindingCodeExportSettings(UIComponent component, out string templatePath, out string outputPath)
            {
                templatePath = string.Empty;
                outputPath = string.Empty;
                
                UIComponentExportType exportType = GetExportType(component);
                if (exportType != UIComponentExportType.UIForm)
                    return false;
                
                // 只生成UIFormSystem代码
                templatePath = ScribanTemplateRoot + "/FUISystem.Binding.tpl";
                outputPath = FUISystemBindingGenerateRoot + "/" + component.PackageName + "/" + component.Name + "System.cs";
                return true;
            }

            private static bool GetEventHandlerBindingCodeExportSettings(UIComponent component, out string templatePath, out string outputPath)
            {
                templatePath = string.Empty;
                outputPath = string.Empty;
                
                UIComponentExportType exportType = GetExportType(component);
                if (exportType != UIComponentExportType.UIForm)
                    return false;
                
                // 只生成EventHandler代码
                outputPath = FUISystemBindingGenerateRoot + "/" + component.PackageName + "/" + component.Name + "EventHandler.cs";
                templatePath = ScribanTemplateRoot + "/FUIEventHandler.Binding.tpl";
                
                return true;
            }

            private static bool GetFUISystemLogicCodeExportSettings(UIComponent component, out string templatePath, out string outputPath)
            {
                templatePath = string.Empty;
                outputPath = string.Empty;
                
                UIComponentExportType exportType = GetExportType(component);
                if (exportType != UIComponentExportType.UIForm)
                    return false;
                
                // 只生成UIFormSystem代码
                outputPath = FUISystemLogicGenerateRoot + "/" + component.PackageName + "/" + component.Name + "System.cs";
                
                // 如果已经存在，则不再生成
                if (File.Exists(outputPath))
                    return false;
                
                templatePath = ScribanTemplateRoot + "/FUISystem.tpl";
                return true;
            }

            private static string GetFUIViewEnumName(UIComponent component)
            {
                UIComponentExportType exportType = GetExportType(component);
                if (exportType != UIComponentExportType.UIForm)
                    return string.Empty;

                return component.Name;
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