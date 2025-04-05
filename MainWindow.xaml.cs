using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DesktopWidget.Models;

namespace DesktopWidget;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private BackgroundManager _backgroundManager;
    private ContextMenu? _contextMenu;
    
    // 默认窗口大小
    private const double DEFAULT_WIDTH = 600;
    private const double DEFAULT_HEIGHT = 400;

    public MainWindow()
    {
        InitializeComponent();
        
        // 初始化背景管理器
        _backgroundManager = new BackgroundManager();
        _backgroundManager.BackgroundChanged += OnBackgroundChanged;
        
        // 获取上下文菜单
        _contextMenu = FindResource("WidgetContextMenu") as ContextMenu;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // 加载当前背景图片
        UpdateBackground(_backgroundManager.GetCurrentBackground());
        
        // 应用透明度设置
        this.Opacity = _backgroundManager.GetOpacity();
        
        // 应用窗口大小设置
        double savedWidth = _backgroundManager.GetWindowWidth();
        double savedHeight = _backgroundManager.GetWindowHeight();
        if (savedWidth > 0 && savedHeight > 0)
        {
            Width = savedWidth;
            Height = savedHeight;
        }
        
        // 检查是否需要更新背景图片
        _backgroundManager.CheckAndUpdateBackground();
    }
    
    private void OnBackgroundChanged(object? sender, BitmapImage backgroundImage)
    {
        // 在UI线程上更新背景图片
        Dispatcher.Invoke(() => UpdateBackground(backgroundImage));
    }
    
    private void UpdateBackground(BitmapImage image)
    {
        BackgroundImage.Source = image;
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // 允许拖动窗口
        DragMove();
    }

    private void BackgroundImage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        // 显示上下文菜单
        if (_contextMenu != null)
        {
            _contextMenu.PlacementTarget = sender as UIElement;
            _contextMenu.IsOpen = true;
            e.Handled = true;
        }
    }

    private void AddImage_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp|所有文件|*.*",
            Multiselect = true,
            Title = "选择背景图片"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            foreach (string fileName in openFileDialog.FileNames)
            {
                _backgroundManager.AddImage(fileName);
            }
            
            // 更新当前背景
            UpdateBackground(_backgroundManager.GetCurrentBackground());
        }
    }

    private void ChangeImage_Click(object sender, RoutedEventArgs e)
    {
        _backgroundManager.ChangeToNextImage();
    }

    private void SetInterval_Click(object sender, RoutedEventArgs e)
    {
        // 简单的输入对话框
        var inputDialog = new InputDialog("设置切换间隔", "请输入图片切换间隔（小时）:", "24");
        if (inputDialog.ShowDialog() == true)
        {
            if (int.TryParse(inputDialog.InputText, out int hours) && hours > 0)
            {
                _backgroundManager.SetChangeInterval(hours);
                MessageBox.Show($"图片切换间隔已设置为 {hours} 小时", "设置成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("请输入有效的小时数", "输入错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void SetOpacity_Click(object sender, RoutedEventArgs e)
    {
        // 获取当前透明度并转换为百分比显示
        int currentOpacityPercent = (int)(_backgroundManager.GetOpacity() * 100);
        
        // 简单的输入对话框
        var inputDialog = new InputDialog("设置透明度", "请输入窗口透明度（20-100%）:", currentOpacityPercent.ToString());
        if (inputDialog.ShowDialog() == true)
        {
            if (int.TryParse(inputDialog.InputText, out int opacityPercent) && opacityPercent >= 20 && opacityPercent <= 100)
            {
                double opacity = opacityPercent / 100.0;
                _backgroundManager.SetOpacity(opacity);
                this.Opacity = opacity;
                MessageBox.Show($"窗口透明度已设置为 {opacityPercent}%", "设置成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("请输入有效的透明度值（20-100）", "输入错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
    private void ResetWindowSize_Click(object sender, RoutedEventArgs e)
    {
        // 重置窗口大小为默认值
        Width = DEFAULT_WIDTH;
        Height = DEFAULT_HEIGHT;
        
        // 保存设置
        _backgroundManager.SetWindowSize(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        
        MessageBox.Show("窗口大小已重置为默认尺寸", "重置成功", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    
    // 处理调整大小的事件
    private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
        if (sender is Thumb thumb)
        {
            double newWidth = Width;
            double newHeight = Height;
            
            // 根据不同的拖动控件，调整宽度或高度
            if (thumb == ThumbRight || thumb == ThumbBottomRight)
            {
                newWidth = Math.Max(MinWidth, Width + e.HorizontalChange);
            }
            
            if (thumb == ThumbBottom || thumb == ThumbBottomRight)
            {
                newHeight = Math.Max(MinHeight, Height + e.VerticalChange);
            }
            
            Width = newWidth;
            Height = newHeight;
            
            // 保存窗口尺寸设置
            _backgroundManager.SetWindowSize(newWidth, newHeight);
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void DeleteImage_Click(object sender, RoutedEventArgs e)
    {
        _backgroundManager.RemoveCurrentBackground();
        UpdateBackground(_backgroundManager.GetCurrentBackground());
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}

// 简单的输入对话框
public class InputDialog : Window
{
    private TextBox _inputTextBox = null!;
    
    public string InputText { get; private set; } = string.Empty;
    
    public InputDialog(string title, string prompt, string defaultValue = "")
    {
        Title = title;
        Width = 300;
        Height = 150;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        ResizeMode = ResizeMode.NoResize;
        
        var grid = new Grid();
        Content = grid;
        
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
        
        var promptLabel = new Label
        {
            Content = prompt,
            Margin = new Thickness(10, 10, 10, 0)
        };
        Grid.SetRow(promptLabel, 0);
        grid.Children.Add(promptLabel);
        
        _inputTextBox = new TextBox
        {
            Text = defaultValue,
            Margin = new Thickness(10, 5, 10, 10)
        };
        Grid.SetRow(_inputTextBox, 1);
        grid.Children.Add(_inputTextBox);
        
        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 0, 10, 10)
        };
        Grid.SetRow(buttonPanel, 2);
        grid.Children.Add(buttonPanel);
        
        var okButton = new Button
        {
            Content = "确定",
            Width = 75,
            Margin = new Thickness(0, 0, 5, 0),
            IsDefault = true
        };
        okButton.Click += (s, e) => 
        {
            InputText = _inputTextBox.Text;
            DialogResult = true;
        };
        buttonPanel.Children.Add(okButton);
        
        var cancelButton = new Button
        {
            Content = "取消",
            Width = 75,
            IsCancel = true
        };
        buttonPanel.Children.Add(cancelButton);
    }
}