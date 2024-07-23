using System;
using UnityEngine;

namespace ET.Client.Common
{
    public partial class GameObjectComponentSystem
    {
        private static void UpdateResizeScale(this GameObjectComponent self)
        {
            switch (self.CurrentResizeMode)
            {
                case GameObjectComponent.ResizeMode.None:
                    self.CurrentResizeScale = new Vector2(100, 100);
                    break;
                case GameObjectComponent.ResizeMode.Free:
                    if (self.IsResizeWithInitSize)
                        self.CurrentResizeScale = new Vector2(self.width * 100 / self.initWidth, self.height * 100 / self.initHeight);
                    else
                        self.CurrentResizeScale = new Vector2(self.width, self.height);
                    break;
                case GameObjectComponent.ResizeMode.MatchWidth:
                    if (self.IsResizeWithInitSize)
                        self.CurrentResizeScale = new Vector2(self.width * 100 / self.initWidth, self.width * 100 / self.initWidth);
                    else
                        self.CurrentResizeScale = new Vector2(self.width, self.width);
                    break;
                case GameObjectComponent.ResizeMode.MatchHeight:
                    if (self.IsResizeWithInitSize)
                        self.CurrentResizeScale = new Vector2(self.height * 100 / self.initHeight, self.height * 100 / self.initHeight);
                    else
                        self.CurrentResizeScale = new Vector2(self.height, self.height);
                    break;
                case GameObjectComponent.ResizeMode.MinOfWidthAndHeight:
                {
                    if (self.IsResizeWithInitSize)
                    {
                        float f = Mathf.Min(self.width * 100 / self.initWidth, self.height * 100 / self.initHeight);
                        self.CurrentResizeScale = new Vector2(f, f);
                    }
                    else
                    {
                        float f = Mathf.Min(self.width, self.height);
                        self.CurrentResizeScale = new Vector2(f, f);
                    }

                    break;
                }
                case GameObjectComponent.ResizeMode.MaxOfWidthAndHeight:
                {
                    if (self.IsResizeWithInitSize)
                    {
                        float f = Mathf.Max(self.width * 100 / self.initWidth, self.height * 100 / self.initHeight);
                        self.CurrentResizeScale = new Vector2(f, f);
                    }
                    else
                    {
                        float f = Mathf.Max(self.width, self.height);
                        self.CurrentResizeScale = new Vector2(f, f);
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            self.UpdateViewObjectScale();
        }
    }
}