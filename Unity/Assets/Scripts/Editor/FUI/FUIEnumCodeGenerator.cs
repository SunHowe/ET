﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FairyGUI.CodeGenerator;
using FairyGUI.CodeGenerator.Core;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// FUI枚举代码生成器
    /// </summary>
    public class FUIEnumCodeGenerator : IUICodeGenerator
    {
        private readonly string m_GeneratePath;
        private readonly Func<UIComponent, string> m_GetEnumName;
        private readonly List<string> m_Names = new List<string>();

        public FUIEnumCodeGenerator(string mGeneratePath, Func<UIComponent, string> mGetEnumName)
        {
            this.m_GeneratePath = mGeneratePath;
            this.m_GetEnumName = mGetEnumName;
        }

        public void Generate(UIComponent component)
        {
            string enumName = m_GetEnumName(component);
            if (string.IsNullOrEmpty(enumName))
                return;

            if (m_Names.Contains(enumName))
            {
                Debug.LogError("重复的界面名称：" + enumName);
                return;
            }
            
            m_Names.Add(enumName);
        }

        public void Flush()
        {
            m_Names.Sort();
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("//------------------------------------------------------------------------------");
            stringBuilder.AppendLine("// <auto-generated>");
            stringBuilder.AppendLine("//     This code was generated by a tool.");
            stringBuilder.AppendLine("//     Changes to this file may cause incorrect behavior and will be lost if");
            stringBuilder.AppendLine("//     the code is regenerated.");
            stringBuilder.AppendLine("// </auto-generated>");
            stringBuilder.AppendLine("//------------------------------------------------------------------------------");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("namespace ET.Client");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("    public enum FUIViewId");
            stringBuilder.AppendLine("    {");
            for (int i = 0; i < m_Names.Count; i++)
            {
                stringBuilder.AppendLine($"        {m_Names[i]} = {i + 1},");
            }
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("}");
            
            File.WriteAllText(m_GeneratePath, stringBuilder.ToString(), Encoding.UTF8);
        }
    }
}