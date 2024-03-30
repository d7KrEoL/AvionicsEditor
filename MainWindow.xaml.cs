using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvionicsEditor.MTypes.Map.Pins;
using BruTile.Wms;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI;
using Mapsui.UI.Wpf;
using Mapsui.Widgets;
using Mapsui.Widgets.ButtonWidget;
using Microsoft.SqlServer.Server;
using Microsoft.Win32;
using OsmSharp.IO.Xml;

namespace AvionicsEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Map.ICanvasMap MainMap;
        public Map.GameMap MainMap { get; private set; }
        public Image MapImage { get; private set; }
        public Mapsui.UI.Wpf.MapControl MapControl { get; private set; }
        //public Label InfoLabel { get; private set; } = new Label();
        private DateTime AnimTimer { get; set; }
        private SaveFileDialog Dialog_SaveFile = new SaveFileDialog();
        private OpenFileDialog Dialog_OpenFile = new OpenFileDialog();
        public MainWindow()
        {
            InitializeComponent();
            GVars.SetMainWindow(this);

            InitializeControls();
            Init_SaveFileDialog();
            Init_OpenFileDielog();
            InitializeMap();
        }
        
        public void SetDebugText(string text)
        {
            TBox_Debug.Text = text;
        }

        /*public void SetInfoText(string text)
        {
            InfoLabel.Content = text;
        }

        public void AddInfoText(string text)
        {
            InfoLabel.Content += text;
        }
        public void ShowInfoText(Point Pos)
        {
            InfoLabel.Margin = new Thickness(Pos.X, Pos.Y, 0, 0);
            InfoLabel.Visibility = Visibility.Visible;
        }
        public void HideInfoText()
        {
            InfoLabel.Visibility = Visibility.Hidden;
        }*/

        private void InitializeControls()
        {
            /*HideInfoText();
            PanelMap.Children.Add(InfoLabel);*/
        }
        private void Init_SaveFileDialog()
        {
            Dialog_SaveFile.Title = "Save Flight Plan Data (Avionics Editor)";
            Dialog_SaveFile.CheckFileExists = false;
            Dialog_SaveFile.CheckPathExists = true;
            Dialog_SaveFile.DefaultExt = ".safp";
            Dialog_SaveFile.CreatePrompt = true;
            Dialog_SaveFile.Filter = "San Andreas Flight Plans (*.safp)|*.safp";
            Dialog_SaveFile.InitialDirectory = $"{Environment.CurrentDirectory}\\export\\";
            Dialog_SaveFile.FileName = $"FlightPlanName.safp";
            Dialog_SaveFile.FileOk += BFile_SaveAs_Dialog_Click;
        }

        private void Init_OpenFileDielog()
        {
            Dialog_OpenFile.Title = "Open Flight Plan Data (Avionics Editor)";
            Dialog_OpenFile.CheckFileExists = true;
            Dialog_OpenFile.CheckPathExists = true;
            Dialog_OpenFile.Multiselect = false;
            Dialog_OpenFile.Filter = "San Andreas Flight Plans (*.safp)|*.safp|SAFP-Formatted Text Files (*.txt)|*.txt";
            Dialog_OpenFile.InitialDirectory = Dialog_SaveFile.InitialDirectory;
            Dialog_OpenFile.DefaultExt = ".safp";
            Dialog_OpenFile.FileName = "Default.safp";
            Dialog_OpenFile.FileOk += BFile_Load_Dialog_Click;
        }


        private void InitializeMap()
        {
            MapControl = new Mapsui.UI.Wpf.MapControl();
            PanelMap.Children.Add(MapControl);
            MapControl.Opacity = 0.9;
            MapControl.Loaded += MapControl_Loaded;
            MapControl.MouseLeftButtonDown += MapControl_OnClick;
            MapControl.MouseLeftButtonUp += MapControl_OnClickUp;
            MapControl.MouseRightButtonDown += MapControl_OnRightClick;
            MapControl.MouseRightButtonUp += MapControl_OnRightUp;
            MapControl.MouseWheel += MapControl_OnZoom;
            MapControl.MouseMove += MapControl_OnMouseMove;
            this.KeyDown += MapControl_OnMapControls;
        }

        private async void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            MainMap = new Map.GameMap(MapImage, new MTypes.Math.Vector2(0, 0), new MTypes.Math.Vector2(0, 0), new MTypes.Math.Vector2(0, 0), new MTypes.Math.Vector2(0, 0));
            ((MapControl)sender).Map = MainMap.MapVar;
            MainMap.MapVar.Navigator.CenterOnAndZoomTo(new MPoint(0, 0), 7.893, -1);
            //Content = (MapControl)sender;
        }

        private async void MapControl_OnClick(object sender, RoutedEventArgs e)
        {
            MapControl.Focus();
            switch(GVars.CurrentMode)
            {
                case Processing.EditorMode.None:
                    break;
                case Processing.EditorMode.AddWPT:
                    WindowModes.AddWaypoint.Processor.OnUpdate(this);
                    break;
                case Processing.EditorMode.AddTarget:
                    break;
                case Processing.EditorMode.EditPoints:
                    if (GVars.CurrentModeStage == 0)
                        WindowModes.EditPoints.Processor.OnStart(this);
                    else
                        WindowModes.EditPoints.Processor.OnStop(this);
                    break;
                case Processing.EditorMode.AddAproach:
                    break;
                case Processing.EditorMode.AddGlidepath:
                    break;
                case Processing.EditorMode.AddDRPM:
                    break;
                case Processing.EditorMode.AddBRDM:
                    break;
                case Processing.EditorMode.AddRunway:
                    break;
                case Processing.EditorMode.AddBeacon:
                    break;
                case Processing.EditorMode.CalcDistance:
                    WindowModes.CalcDistance.Processor.OnUpdate(this);
                    break;
                case Processing.EditorMode.CalcAngle:
                    WindowModes.CalcAngle.Processor.OnUpdate(this, true);
                    break;
                case Processing.EditorMode.GetPos:
                    WindowModes.GetPos.Processor.OnUpdate(this);
                    break;
                default:
                    break;
            }
        }

        private async void MapControl_OnClickUp(object sender, RoutedEventArgs e)
        {
            MapControl.Focus();
            switch (GVars.CurrentMode)
            {
                case Processing.EditorMode.None:
                    break;
                case Processing.EditorMode.AddWPT:
                    break;
                case Processing.EditorMode.AddTarget:
                    break;
                case Processing.EditorMode.EditPoints:
                    //WindowModes.EditPoints.Processor.OnStop(this);
                    break;
                case Processing.EditorMode.AddAproach:
                    break;
                case Processing.EditorMode.AddGlidepath:
                    break;
                case Processing.EditorMode.AddDRPM:
                    break;
                case Processing.EditorMode.AddBRDM:
                    break;
                case Processing.EditorMode.AddRunway:
                    break;
                case Processing.EditorMode.AddBeacon:
                    break;
                case Processing.EditorMode.CalcDistance:
                    break;
                case Processing.EditorMode.CalcAngle:
                    break;
                case Processing.EditorMode.GetPos:
                    MainWindowF.ModeFuncs.Tools.SetMode_None(this);
                    break;
                default:
                    break;
            }
        }

        private async void MapControl_OnRightClick(object sender, MouseEventArgs e)
        {
            MapControl.Focus();
            switch (GVars.CurrentMode)
            {
                case Processing.EditorMode.None:
                    if(Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        if (GVars.CurrentMode == Processing.EditorMode.CalcDistance) UpdateMode(Processing.EditorMode.CalcDistance);
                        else WindowModes.CalcDistance.Processor.OnUpdate(this);
                    }
                    else if(Keyboard.IsKeyDown(Key.LeftAlt))
                    {
                        if (GVars.CurrentMode == Processing.EditorMode.None) UpdateMode(Processing.EditorMode.CalcAngle);
                        WindowModes.CalcAngle.Processor.OnUpdate(this, true);
                    }
                    else WindowModes.GetPos.Processor.OnUpdate(this);
                    break;
                case Processing.EditorMode.AddWPT:
                    WindowModes.AddWaypoint.Processor.OnUpdateDelete(this);
                    break;
                case Processing.EditorMode.AddTarget:
                    break;
                case Processing.EditorMode.EditPoints:
                    if (GVars.CurrentMode == Processing.EditorMode.EditPoints) UpdateMode(Processing.EditorMode.EditPoints);
                    /*if (GVars.CurrentMode == Processing.EditorMode.None) UpdateMode(Processing.EditorMode.EditPoints);
                    WindowModes.EditPoints.Processor.OnStart(this);*/
                    WindowModes.EditPoints.Processor.OnStart(this);
                    break;
                case Processing.EditorMode.AddAproach:
                    break;
                case Processing.EditorMode.AddGlidepath:
                    break;
                case Processing.EditorMode.AddDRPM:
                    break;
                case Processing.EditorMode.AddBRDM:
                    break;
                case Processing.EditorMode.AddRunway:
                    break;
                case Processing.EditorMode.AddBeacon:
                    break;
                case Processing.EditorMode.CalcDistance:
                    WindowModes.CalcDistance.Processor.OnUpdate(this);
                    break;
                case Processing.EditorMode.CalcAngle:
                    WindowModes.CalcAngle.Processor.OnUpdate(this, true);
                    break;
                case Processing.EditorMode.GetPos:
                    MainWindowF.ModeFuncs.Tools.SetMode_None(this);
                    break;
                default:
                    break;
            }
        }

        private async void MapControl_OnRightUp(object sender, MouseEventArgs e)
        {
            MapControl.Focus();
            switch (GVars.CurrentMode)
            {
                case Processing.EditorMode.None:
                    break;
                case Processing.EditorMode.AddWPT:
                    break;
                case Processing.EditorMode.AddTarget:
                    break;
                case Processing.EditorMode.EditPoints:
                    WindowModes.EditPoints.Processor.OnStop(this);
                    break;
                case Processing.EditorMode.AddAproach:
                    break;
                case Processing.EditorMode.AddGlidepath:
                    break;
                case Processing.EditorMode.AddDRPM:
                    break;
                case Processing.EditorMode.AddBRDM:
                    break;
                case Processing.EditorMode.AddRunway:
                    break;
                case Processing.EditorMode.AddBeacon:
                    break;
                case Processing.EditorMode.CalcDistance:
                    break;
                case Processing.EditorMode.CalcAngle:
                    break;
                case Processing.EditorMode.GetPos:
                    break;
                default:
                    break;
            }
        }
        private async void MapControl_OnMouseMove(object sender, MouseEventArgs e)
        {
            MapControl.Focus();
            switch (GVars.CurrentMode)
            {
                case Processing.EditorMode.None:
                    break;
                case Processing.EditorMode.AddWPT:
                    break;
                case Processing.EditorMode.AddTarget:
                    break;
                case Processing.EditorMode.EditPoints:
                    WindowModes.EditPoints.Processor.OnUpdate(this);
                    break;
                case Processing.EditorMode.AddAproach:
                    break;
                case Processing.EditorMode.AddGlidepath:
                    break;
                case Processing.EditorMode.AddDRPM:
                    break;
                case Processing.EditorMode.AddBRDM:
                    break;
                case Processing.EditorMode.AddRunway:
                    break;
                case Processing.EditorMode.AddBeacon:
                    break;
                case Processing.EditorMode.CalcDistance:
                    WindowModes.CalcDistance.Processor.OnUpdate(this, true);
                    break;
                case Processing.EditorMode.CalcAngle:
                    WindowModes.CalcAngle.Processor.OnUpdate(this, false);
                    break;
                case Processing.EditorMode.GetPos:
                    break;
                default:
                    break;
            }
        }

        private async void MapControl_OnMapControls(object sender, KeyEventArgs e)
        {
            //SetDebugText($"KEYDOWN: {e.Key}");
            if (!MapControl.IsFocused) return;
            switch(e.Key)
            {
                case Key.Escape:
                    if (GVars.CurrentMode == Processing.EditorMode.EditPoints && GVars.CurrentModeStage > 0) WindowModes.EditPoints.Processor.OnStop(this, true);
                    else UpdateMode(0);
                    break;
                case Key.D0:
                    Z.Text = "0";
                    if (Keyboard.IsKeyDown(Key.LeftCtrl)) Z.Text = "0";
                    else if (Keyboard.IsKeyDown(Key.LeftShift)) Z.Text = "0";
                    break;
                case Key.D1:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl)) UpdateMode(Processing.EditorMode.AddWPT);
                    else if (Keyboard.IsKeyDown(Key.LeftShift)) UpdateMode(Processing.EditorMode.CalcDistance);
                    break;
                case Key.D2:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl)) UpdateMode(Processing.EditorMode.AddTarget);
                    else if (Keyboard.IsKeyDown(Key.LeftShift)) UpdateMode(Processing.EditorMode.CalcAngle);
                    break;
                case Key.D3:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl)) UpdateMode(Processing.EditorMode.EditPoints);
                    else if (Keyboard.IsKeyDown(Key.LeftShift)) UpdateMode(Processing.EditorMode.GetPos);
                    break;
                case Key.D4:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl)) UpdateMode(Processing.EditorMode.AddAproach);
                    else if (Keyboard.IsKeyDown(Key.LeftShift)) UpdateMode(Processing.EditorMode.GetPos);
                    break;
                case Key.D5:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl)) UpdateMode(Processing.EditorMode.AddGlidepath);
                    //else if (Keyboard.IsKeyDown(Key.LeftShift)) 
                    break;
                case Key.D6:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl)) UpdateMode(Processing.EditorMode.AddDRPM);
                    //else if (Keyboard.IsKeyDown(Key.LeftShift)) 
                    break;
                case Key.D7:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl)) UpdateMode(Processing.EditorMode.AddBRDM);
                    //else if (Keyboard.IsKeyDown(Key.LeftShift)) 
                    break;
                case Key.D8:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl)) UpdateMode(Processing.EditorMode.AddRunway);
                    //else if (Keyboard.IsKeyDown(Key.LeftShift)) 
                    break;
                case Key.D9:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl)) UpdateMode(Processing.EditorMode.AddBeacon);
                    //else if (Keyboard.IsKeyDown(Key.LeftShift)) 
                    break;
                case Key.Delete:
                    if (GVars.CurrentMode == Processing.EditorMode.EditPoints && GVars.CurrentModeStage > 0) WindowModes.EditPoints.Processor.DeletePoint(this);
                    break;
                case Key.Enter:
                    if (GVars.CurrentMode == Processing.EditorMode.EditPoints && GVars.CurrentModeStage > 0)
                    {
                        WindowModes.EditPoints.Processor.OnStop(this, true);
                    }
                    break;
            }
        }

        private async void MapControl_OnZoom(object sender, MouseWheelEventArgs e)
        {
            //GVars.ReloadPins();
            switch (GVars.CurrentMode)
            {
                case Processing.EditorMode.None:
                    break;
                case Processing.EditorMode.AddWPT:
                    break;
                case Processing.EditorMode.AddTarget:
                    break;
                case Processing.EditorMode.EditPoints:
                    break;
                case Processing.EditorMode.AddAproach:
                    break;
                case Processing.EditorMode.AddGlidepath:
                    break;
                case Processing.EditorMode.AddDRPM:
                    break;
                case Processing.EditorMode.AddBRDM:
                    break;
                case Processing.EditorMode.AddRunway:
                    break;
                case Processing.EditorMode.AddBeacon:
                    break;
                case Processing.EditorMode.CalcDistance:
                    int time = 0;
                    if (e.Delta > 0) MainWindowF.MapControl.OnZoom.CalcDistance.DoTask(this, true);
                    else MainWindowF.MapControl.OnZoom.CalcDistance.DoTask(this, false);

                    break;
                case Processing.EditorMode.CalcAngle:
                    WindowModes.CalcAngle.Processor.OnUpdate(this, false);
                    break;
                case Processing.EditorMode.GetPos:
                    break;
                default:
                    break;
            }
        }

        private void SetPos_Click(object sender, RoutedEventArgs e)
        {
            int PosX, PosY;
            int.TryParse(X.Text, out PosX);
            int.TryParse(Y.Text, out PosY);
            MPoint FlyToPos = new MPoint(PosX, PosY);
            //MainMap.MapVar.Navigator.FlyTo(FlyToPos, 10, 500);
            MainMap.MapVar.Navigator.CenterOn(FlyToPos, -1);
            MainMap.MapVar.Navigator.ZoomToLevel(0);
            GVars.RemovePins(PinType.SelectedPoint);
            var ResPin = new DefaultPin($"Goto Point [{X};{Y}]", (double)PosX, (double)PosY, PinType.SelectedPoint, MainMap);
            GVars.AddPin(ResPin);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int nWidth = (int)(System.Windows.SystemParameters.PrimaryScreenWidth * 0.4);
            int nHeight = (int)(System.Windows.SystemParameters.PrimaryScreenHeight * 0.55);
            this.Width = nWidth;
            this.Height = nHeight;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //SetDebugText($"Screen Size: {Width}x{Height}");
            if (this.WindowState == WindowState.Maximized) UpdateMapSize(1);
            else UpdateMapSize(0);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) UpdateMapSize(1);
            else UpdateMapSize(0);
            //SetDebugText($"Screen Size FSCR: {Width}x{Height}");
        }

        private void UpdateMapSize(int type)
        {
            
            switch (type)
            {
                case 0://Menu off, not maximized
                    PanelMap.Width = this.Width * 1;
                    PanelMap.Height = this.Height * 0.9;
                    MapControl.Width = PanelMap.Width * 0.99;
                    MapControl.Height = PanelMap.Height * 0.99;
                    break;
                case 1://Menu off, maximized
                    PanelMap.Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth*1.01;
                    PanelMap.Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight*0.9;
                    MapControl.Width = PanelMap.Width * 0.99;
                    MapControl.Height = PanelMap.Height * 0.99;
                    break;
                case 3://Menu on, not maximized
                    PanelMap.Width = this.Width * 0.95;
                    PanelMap.Height = this.Height * 0.9;
                    MapControl.Width = PanelMap.Width * 0.97;
                    MapControl.Height = PanelMap.Height * 0.99;
                    break;
                case 4://Menu on, maximized
                    PanelMap.Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth * 0.995;
                    PanelMap.Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight * 0.9;
                    MapControl.Width = PanelMap.Width * 0.97;
                    MapControl.Height = PanelMap.Height * 0.99;
                    break;
            }
            //Console.WriteLine($"Resize {PanelMap.Width} {PanelMap.Height} {MapControl.Width} {MapControl.Height}");
        }

        private void ShowLeftMenu(bool show)
        {
            if (show)
            {
                if (LeftMenu.Width != 0) return;
                LeftMenu.Width = this.Width * 0.07;
                AnimTimer = DateTime.Now.AddSeconds(0.5);
                if (this.WindowState == WindowState.Maximized) UpdateMapSize(4);
                else UpdateMapSize(3);
            }
            else if(LeftMenu.Width != 0)
            {
                LeftMenu.Width = 0;
                AnimTimer = DateTime.Now.AddSeconds(0.5);
                if (this.WindowState == WindowState.Maximized) UpdateMapSize(1);
                else UpdateMapSize(0);
            }
            //Console.WriteLine($"LeftMenu {show}");
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (DateTime.Compare(DateTime.Now, AnimTimer) > 0)
            {
                if (System.Windows.Input.Mouse.GetPosition(this).X < this.Width * 0.1) ShowLeftMenu(true);
                else ShowLeftMenu(false);
            }
        }

        private void BInst_AddWPT_Click(object sender, RoutedEventArgs e)
        {
            UpdateMode(Processing.EditorMode.AddWPT);
        }

        private void BInst_EditWPT_Click(object sender, RoutedEventArgs e)
        {
            UpdateMode(Processing.EditorMode.EditPoints);
        }

        private void UpdateMode(Processing.EditorMode mode)
        {
            MainWindowF.ModeFuncs.UpdateMode.Processor.OnModeChanged(this, mode);
        }

        public void SetAllColor(System.Windows.Media.Color color)
        {
            BInst_AddWPT.Background = new SolidColorBrush(color);
            //BInst_AddTGT.Background = new SolidColorBrush(color);
            BInst_EditWPT.Background = new SolidColorBrush(color);
            /*BInst_APR.Background = new SolidColorBrush(color);
            BInst_GLD.Background = new SolidColorBrush(color);
            BInst_DRDM.Background = new SolidColorBrush(color);
            BInst_BRDM.Background = new SolidColorBrush(color);
            BInst_RW.Background = new SolidColorBrush(color);
            BInst_BCN.Background = new SolidColorBrush(color);*/
            BInst_Dist.Background = new SolidColorBrush(color);
            BInst_Angle.Background = new SolidColorBrush(color);
            BInst_Pos.Background = new SolidColorBrush(color);
        }

        public void BInst_Pos_Click(object sender, RoutedEventArgs e)
        {
            UpdateMode(Processing.EditorMode.GetPos);
        }

        public void BInst_Dist_Click(object sender, RoutedEventArgs e)
        {
            UpdateMode(Processing.EditorMode.CalcDistance);
        }

        public void BInst_Angle_Click(object sender, RoutedEventArgs e)
        {
            UpdateMode(Processing.EditorMode.CalcAngle);
        }

        public void BFile_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            Dialog_SaveFile.ShowDialog();
        }

        public void BFile_SaveAs_Dialog_Click(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetDebugText($"Save File as: {Dialog_SaveFile.SafeFileName}");

            Dialog_OpenFile.InitialDirectory = Dialog_SaveFile.InitialDirectory;
            Dialog_OpenFile.FileName = Dialog_SaveFile.FileName; //= Dialog_SaveFile.FileName.Replace($"\\{Dialog_SaveFile.SafeFileName}", "");
            
            Files_IO.Writer.FlightPlan.Save(Dialog_SaveFile.FileName);

            Dialog_SaveFile.InitialDirectory = Dialog_SaveFile.FileName.Replace($"\\{Dialog_SaveFile.SafeFileName}", "");
            Dialog_SaveFile.FileName = Dialog_SaveFile.SafeFileName;
        }

        public void BFile_Save_Click(object sender, RoutedEventArgs e)
        {
            SetDebugText($"Save File: {Dialog_SaveFile.SafeFileName}");
            Files_IO.Writer.FlightPlan.Save(Dialog_SaveFile.FileName);
        }

        public void BFile_Open_Click(object sender, RoutedEventArgs e)
        {
            Dialog_OpenFile.ShowDialog();
        }

        public void BFile_Load_Dialog_Click(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetDebugText($"Load flight plan file: {Dialog_OpenFile.SafeFileName}");

            Dialog_SaveFile.InitialDirectory = Dialog_OpenFile.InitialDirectory;
            Dialog_SaveFile.FileName = Dialog_OpenFile.FileName;

            WindowModes.AddWaypoint.Processor.ClearWaypoints();
            Files_IO.Parser.FlightPlan.Open(Dialog_OpenFile.FileName);

            var pos = GVars.Waypoints[GVars.Waypoints.Count - 1].Position;
            var StartPoint = new MPoint(pos.X, pos.Y);
            MainMap.MapVar.Navigator.CenterOnAndZoomTo(StartPoint, 0.247, -1L);

            GVars.SetCurrentMode(0, 0);
        }

        public void Z_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!VerificateText(Z.Text)) Z.Text = Z.Text.Remove(Z.Text.Length - 1);
        }

        public void Z_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (!VerificateText(e.Text)) Z.Text = Z.Text.Remove(Z.Text.Length - 1);
        }

        public bool VerificateText(string text)
        {
            if (!IsTextValid(text)) return false;
            return true;
        }

        public bool IsTextValid(string text)
        {
            //if (Z.Text.Length > 15) return false;
            //string[] Arg = text.Split('_');
            //if (Arg.Length != 2) return false;

            for (int i = 0; i < text.Length; i++)
            {
                char[] chars = text.ToCharArray();
                foreach (char C in chars)
                {
                    //"ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
                    //if (char.IsLetter(C)) continue;
                    if (char.IsNumber(C)) continue;
                    if (C == '.' || C == ',') continue;
                    //char CheckChar = Char.ToUpper(C);
                    //if (CheckChar >= 'A' && CheckChar <= '0' && CheckChar >= '9' && CheckChar <= 'Z') continue;
                    //if (CheckChar >= '0' && CheckChar <= '9') continue;
                    else return false;
                }
            }
            return true;
        }

       
    }
}
