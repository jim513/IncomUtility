﻿#pragma checksum "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "11395FE656F57893C7B7E89C7F0AD5752960EB51CF5A85A52E38A6E460B811C5"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using IncomUtility;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace IncomUtility {
    
    
    /// <summary>
    /// APP_UI_MonitorDeviceStatus
    /// </summary>
    public partial class APP_UI_MonitorDeviceStatus : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 35 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox tCmb_UpButton;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox tCmb_DownButton;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox tCmb_InhibitSwitch;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox tCmb_SinkSourceSwitchs;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button tBtn_ReadSwitchStauts;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker tDate_DatePicker;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.TimePicker tTime_TimePicker;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button tBtn_ReadTime;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button tBtn_SetTime;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tTxt_Memo1;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid instrument_status_grid_data;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button tBtn_ReadDeviceStatus;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid fault_status_grid_data;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid warning_status_grid_data;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/IncomUtility;component/app/app_ui_monitordevicestatus.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.tCmb_UpButton = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.tCmb_DownButton = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.tCmb_InhibitSwitch = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.tCmb_SinkSourceSwitchs = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.tBtn_ReadSwitchStauts = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
            this.tBtn_ReadSwitchStauts.Click += new System.Windows.RoutedEventHandler(this.tBtn_ReadSwitchStauts_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.tDate_DatePicker = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 7:
            this.tTime_TimePicker = ((Xceed.Wpf.Toolkit.TimePicker)(target));
            return;
            case 8:
            this.tBtn_ReadTime = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
            this.tBtn_ReadTime.Click += new System.Windows.RoutedEventHandler(this.tBtn_ReadTime_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.tBtn_SetTime = ((System.Windows.Controls.Button)(target));
            
            #line 66 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
            this.tBtn_SetTime.Click += new System.Windows.RoutedEventHandler(this.tBtn_SetTime_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.tTxt_Memo1 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.instrument_status_grid_data = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 12:
            this.tBtn_ReadDeviceStatus = ((System.Windows.Controls.Button)(target));
            
            #line 109 "..\..\..\APP\APP_UI_MonitorDeviceStatus.xaml"
            this.tBtn_ReadDeviceStatus.Click += new System.Windows.RoutedEventHandler(this.tBtn_ReadDeviceStatus_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.fault_status_grid_data = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 14:
            this.warning_status_grid_data = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

