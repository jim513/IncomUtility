﻿#pragma checksum "..\..\..\APP\APP_UI_ViewLogs.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "646E301DC8B05080CBBFA2D470934BE99AB0FD79AF42D5A30D4DD939C4100614"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using IncomUtility.APP;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace IncomUtility.APP {
    
    
    /// <summary>
    /// APP_UI_ViewLogs
    /// </summary>
    public partial class APP_UI_ViewLogs : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 28 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox tCmb_LogsType;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button tBtn_ReadLogInfo;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button tBtn_DownloadLogs;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar tPbar_LogDownBar;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button tBtn_StopDownload;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button tBtn_SaveToCSV;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button tBtn_ClrLog;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox tChb_RecordClrLog;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl LogTab;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid grid_LogAlarm;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid grid_LogWarning;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid grid_LogFault;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid grid_LogInfo;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid grid_LogCal;
        
        #line default
        #line hidden
        
        
        #line 145 "..\..\..\APP\APP_UI_ViewLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid grid_LogRelfex;
        
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
            System.Uri resourceLocater = new System.Uri("/IncomUtility;component/app/app_ui_viewlogs.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\APP\APP_UI_ViewLogs.xaml"
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
            this.tCmb_LogsType = ((System.Windows.Controls.ComboBox)(target));
            
            #line 28 "..\..\..\APP\APP_UI_ViewLogs.xaml"
            this.tCmb_LogsType.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.tCmb_LogsType_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tBtn_ReadLogInfo = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\APP\APP_UI_ViewLogs.xaml"
            this.tBtn_ReadLogInfo.Click += new System.Windows.RoutedEventHandler(this.tBtn_ReadLogInfo_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tBtn_DownloadLogs = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\APP\APP_UI_ViewLogs.xaml"
            this.tBtn_DownloadLogs.Click += new System.Windows.RoutedEventHandler(this.tBtn_DownloadLogs_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tPbar_LogDownBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 5:
            this.tBtn_StopDownload = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\APP\APP_UI_ViewLogs.xaml"
            this.tBtn_StopDownload.Click += new System.Windows.RoutedEventHandler(this.tBtn_StopDownload_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.tBtn_SaveToCSV = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\..\APP\APP_UI_ViewLogs.xaml"
            this.tBtn_SaveToCSV.Click += new System.Windows.RoutedEventHandler(this.tBtn_SaveToCSV_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.tBtn_ClrLog = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\APP\APP_UI_ViewLogs.xaml"
            this.tBtn_ClrLog.Click += new System.Windows.RoutedEventHandler(this.tBtn_ClrLog_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.tChb_RecordClrLog = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 9:
            this.LogTab = ((System.Windows.Controls.TabControl)(target));
            return;
            case 10:
            this.grid_LogAlarm = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 11:
            this.grid_LogWarning = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 12:
            this.grid_LogFault = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 13:
            this.grid_LogInfo = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 14:
            this.grid_LogCal = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 15:
            this.grid_LogRelfex = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

