﻿#pragma checksum "..\..\..\..\RDLCPrinter\MrDSoft.RDLC\LightIntergerSpinner.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CE8E62A00CBC16656ACD949C482B27B2"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34209
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace DSoft.RDLCReport {
    
    
    /// <summary>
    /// LightIntergerSpinner
    /// </summary>
    public partial class LightIntergerSpinner : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\..\RDLCPrinter\MrDSoft.RDLC\LightIntergerSpinner.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition ButtonColumn;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\RDLCPrinter\MrDSoft.RDLC\LightIntergerSpinner.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NumPager;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\RDLCPrinter\MrDSoft.RDLC\LightIntergerSpinner.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SpinnerUp;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\RDLCPrinter\MrDSoft.RDLC\LightIntergerSpinner.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SpinnerDown;
        
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
            System.Uri resourceLocater = new System.Uri("/SCZY;component/rdlcprinter/mrdsoft.rdlc/lightintergerspinner.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\RDLCPrinter\MrDSoft.RDLC\LightIntergerSpinner.xaml"
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
            
            #line 7 "..\..\..\..\RDLCPrinter\MrDSoft.RDLC\LightIntergerSpinner.xaml"
            ((DSoft.RDLCReport.LightIntergerSpinner)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ButtonColumn = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 3:
            this.NumPager = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.SpinnerUp = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\..\RDLCPrinter\MrDSoft.RDLC\LightIntergerSpinner.xaml"
            this.SpinnerUp.Click += new System.Windows.RoutedEventHandler(this.SpinnerUp_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SpinnerDown = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\..\RDLCPrinter\MrDSoft.RDLC\LightIntergerSpinner.xaml"
            this.SpinnerDown.Click += new System.Windows.RoutedEventHandler(this.SpinnerDown_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

