﻿#pragma checksum "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C20FA4C9A9CADC3C3B2B64095607BA4F"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18449
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


namespace SCZY.Admin.ProductDialog.EditProduct_Provider {
    
    
    /// <summary>
    /// EditProduct_Provider
    /// </summary>
    public partial class EditProduct_Provider : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ProviderDataGrid;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel BtnStackPanel;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PriceBox;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock WanningBlock;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SaveButton;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CancelButton;
        
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
            System.Uri resourceLocater = new System.Uri("/SCZY;component/admin/productdialog/editproduct_provider/editproduct_provider.xam" +
                    "l", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml"
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
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.ProviderDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.BtnStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.PriceBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.WanningBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.SaveButton = ((System.Windows.Controls.Button)(target));
            
            #line 76 "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml"
            this.SaveButton.Click += new System.Windows.RoutedEventHandler(this.SaveButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CancelButton = ((System.Windows.Controls.Button)(target));
            
            #line 77 "..\..\..\..\..\Admin\ProductDialog\EditProduct_Provider\EditProduct_Provider.xaml"
            this.CancelButton.Click += new System.Windows.RoutedEventHandler(this.CloseWindowClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

