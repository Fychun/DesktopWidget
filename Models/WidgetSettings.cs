using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DesktopWidget.Models
{
    public class WidgetSettings
    {
        // 背景图片路径列表
        public List<string> BackgroundImages { get; set; } = new List<string>();
        
        // 当前显示的图片索引
        public int CurrentImageIndex { get; set; } = 0;
        
        // 切换图片的时间间隔（以小时为单位）
        public int ChangeIntervalHours { get; set; } = 24;
        
        // 上次切换图片的时间
        public DateTime LastChangeTime { get; set; } = DateTime.Now;
        
        // 窗口透明度（范围：0.2-1.0）
        public double Opacity { get; set; } = 1.0;
        
        // 窗口宽度
        public double WindowWidth { get; set; } = 600;
        
        // 窗口高度
        public double WindowHeight { get; set; } = 400;
        
        // 保存设置的文件路径
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DesktopWidget", 
            "settings.json"
        );
        
        // 保存设置
        public void SaveSettings()
        {
            string? directoryPath = Path.GetDirectoryName(SettingsFilePath);
            if (!Directory.Exists(directoryPath) && directoryPath != null)
            {
                Directory.CreateDirectory(directoryPath);
            }
            
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            File.WriteAllText(SettingsFilePath, json);
        }
        
        // 加载设置
        public static WidgetSettings LoadSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                string json = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<WidgetSettings>(json);
                return settings ?? new WidgetSettings();
            }
            
            return new WidgetSettings();
        }
    }
} 