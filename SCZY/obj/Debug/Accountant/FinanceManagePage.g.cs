﻿#pragma checksum "..\..\..\Accountant\FinanceManagePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "76E7549714DE95F7AD81D0669CDCA3A2"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18449
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using SCZY.Common;
using SCZY.OtherWindows;
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


namespace SCZY.Accountant {
    
    
    /// <summary>
    /// FinanceManagePage
    /// </summary>
    public partial class FinanceManagePage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 35 "..\..\..\Accountant\FinanceManagePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SeacherBox;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Accountant\FinanceManagePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image SeacherBtn_Mo;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\Accountant\FinanceManagePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image SeacherBtn;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\Accountant\FinanceManagePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ResetBtn_Mo;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\Accountant\FinanceManagePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ResetBtn;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\Accountant\FinanceManagePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ConsumersGrid;
        
        #line default
        #line hidden
        
        
        #line 167 "..\..\..\Accountant\FinanceManagePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContextMenu DataGridContextMenu;
        
        #line default
        #line hidden
        
        
        #line 189 "..\..\..\Accountant\FinanceManagePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SCZY.OtherWindows.DataGridPager gridPager;
        
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
            System.Uri resourceLocater = new System.Uri("/SCZY;component/accountant/financemanagepage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Accountant\FinanceManagePage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.SeacherBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 35 "..\..\..\Accountant\FinanceManagePage.xaml"
            this.SeacherBox.KeyUp += new System.Windows.Input.KeyEventHandler(this.SeacherBox_OnKeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SeacherBtn_Mo = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.SeacherBtn = ((System.Windows.Controls.Image)(target));
            
            #line 51 "..\..\..\Accountant\FinanceManagePage.xaml"
            this.SeacherBtn.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.SeacherBtn_OnMouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ResetBtn_Mo = ((System.Windows.Controls.Image)(target));
            return;
            case 5:
            this.ResetBtn = ((System.Windows.Controls.Image)(target));
            
            #line 82 "..\..\..\Accountant\FinanceManagePage.xaml"
            this.ResetBtn.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.ResetBtn_OnMouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ConsumersGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 126 "..\..\..\Accountant\FinanceManagePage.xaml"
            this.ConsumersGrid.LoadingRow += new System.EventHandler<System.Windows.Controls.DataGridRowEventArgs>(this.DebtWarnLoadRow);
            
            #line default
            #line hidden
            return;
            case 7:
            this.DataGridContextMenu = ((System.Windows.Controls.ContextMenu)(target));
            return;
            case 8:
            
            #line 168 "..\..\..\Accountant\FinanceManagePage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Repayment_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 169 "..\..\..\Accountant\FinanceManagePage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.EditRepayment_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 170 "..\..\..\Accountant\FinanceManagePage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DebtMemory_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.gridPager = ((SCZY.OtherWindows.DataGridPager)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

