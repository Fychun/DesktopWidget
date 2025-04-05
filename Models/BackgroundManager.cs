using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Media.Imaging;

namespace DesktopWidget.Models
{
    public class BackgroundManager
    {
        private WidgetSettings _settings;
        private System.Timers.Timer? _changeTimer;
        private readonly string _defaultImagePath = "pack://application:,,,/Images/default.jpg";
        
        public event EventHandler<BitmapImage>? BackgroundChanged;
        
        public BackgroundManager()
        {
            _settings = WidgetSettings.LoadSettings();
            InitializeTimer();
        }
        
        private void InitializeTimer()
        {
            // 每小时检查一次是否需要切换图片
            _changeTimer = new System.Timers.Timer(3600000); // 3600000毫秒 = 1小时
            _changeTimer.Elapsed += OnTimerElapsed;
            _changeTimer.AutoReset = true;
            _changeTimer.Start();
        }
        
        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            CheckAndUpdateBackground();
        }
        
        // 检查并根据设置更新背景
        public void CheckAndUpdateBackground()
        {
            if (_settings.BackgroundImages.Count == 0)
            {
                return;
            }
            
            TimeSpan elapsed = DateTime.Now - _settings.LastChangeTime;
            if (elapsed.TotalHours >= _settings.ChangeIntervalHours)
            {
                ChangeToNextImage();
            }
        }
        
        // 更改到下一张图片
        public void ChangeToNextImage()
        {
            if (_settings.BackgroundImages.Count == 0)
            {
                return;
            }
            
            _settings.CurrentImageIndex = (_settings.CurrentImageIndex + 1) % _settings.BackgroundImages.Count;
            _settings.LastChangeTime = DateTime.Now;
            _settings.SaveSettings();
            
            NotifyBackgroundChanged();
        }
        
        // 获取当前背景图片
        public BitmapImage GetCurrentBackground()
        {
            if (_settings.BackgroundImages.Count == 0)
            {
                return new BitmapImage(new Uri(_defaultImagePath));
            }
            
            string imagePath = _settings.BackgroundImages[_settings.CurrentImageIndex];
            if (File.Exists(imagePath))
            {
                return LoadImage(imagePath);
            }
            
            return new BitmapImage(new Uri(_defaultImagePath));
        }
        
        // 加载图片
        private BitmapImage LoadImage(string path)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(path);
            image.EndInit();
            image.Freeze(); // 提高性能
            return image;
        }
        
        // 添加图片到背景集合
        public void AddImage(string path)
        {
            if (File.Exists(path) && !_settings.BackgroundImages.Contains(path))
            {
                _settings.BackgroundImages.Add(path);
                _settings.SaveSettings();
            }
        }
        
        // 移除背景图片
        public void RemoveImage(string path)
        {
            if (_settings.BackgroundImages.Contains(path))
            {
                _settings.BackgroundImages.Remove(path);
                if (_settings.CurrentImageIndex >= _settings.BackgroundImages.Count && _settings.BackgroundImages.Count > 0)
                {
                    _settings.CurrentImageIndex = 0;
                }
                _settings.SaveSettings();
                NotifyBackgroundChanged();
            }
        }
        
        // 设置图片切换间隔（小时）
        public void SetChangeInterval(int hours)
        {
            if (hours > 0)
            {
                _settings.ChangeIntervalHours = hours;
                _settings.SaveSettings();
            }
        }
        
        // 获取当前透明度
        public double GetOpacity()
        {
            return _settings.Opacity;
        }
        
        // 设置透明度
        public void SetOpacity(double opacity)
        {
            // 确保透明度不小于20%且不大于100%
            _settings.Opacity = Math.Clamp(opacity, 0.2, 1.0);
            _settings.SaveSettings();
        }
        
        // 获取窗口宽度
        public double GetWindowWidth()
        {
            return _settings.WindowWidth;
        }
        
        // 获取窗口高度
        public double GetWindowHeight()
        {
            return _settings.WindowHeight;
        }
        
        // 设置窗口尺寸
        public void SetWindowSize(double width, double height)
        {
            _settings.WindowWidth = Math.Max(200, width);  // 最小宽度
            _settings.WindowHeight = Math.Max(150, height); // 最小高度
            _settings.SaveSettings();
        }
        
        // 通知背景图片已更改
        private void NotifyBackgroundChanged()
        {
            BackgroundChanged?.Invoke(this, GetCurrentBackground());
        }
        
        // 获取设置对象
        public WidgetSettings GetSettings()
        {
            return _settings;
        }
    }
} 