﻿#pragma checksum "..\..\..\Views\SyncIrsView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C1123E01BE29A48325B87745F53A04D7E45F4948"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace Jsa.ViewsModel.Views {
    
    
    /// <summary>
    /// SyncIrsView
    /// </summary>
    public partial class SyncIrsView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\Views\SyncIrsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Ribbon.Ribbon ribbon;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Views\SyncIrsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Ribbon.RibbonTab homeTab;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Views\SyncIrsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtYear;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Views\SyncIrsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ckbSyncProperties;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\Views\SyncIrsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ckbSyncCusomers;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\Views\SyncIrsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ckbSyncContracts;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\Views\SyncIrsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ckbSyncPayments;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Views\SyncIrsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ckbSyncShedPays;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Views\SyncIrsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblCurrentOperation;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\Views\SyncIrsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar prog;
        
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
            System.Uri resourceLocater = new System.Uri("/ViewsModel;component/views/syncirsview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\SyncIrsView.xaml"
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
            
            #line 4 "..\..\..\Views\SyncIrsView.xaml"
            ((Jsa.ViewsModel.Views.SyncIrsView)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.WindowClosing);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\Views\SyncIrsView.xaml"
            ((Jsa.ViewsModel.Views.SyncIrsView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.WindowLoaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ribbon = ((System.Windows.Controls.Ribbon.Ribbon)(target));
            return;
            case 3:
            this.homeTab = ((System.Windows.Controls.Ribbon.RibbonTab)(target));
            return;
            case 4:
            this.txtYear = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.ckbSyncProperties = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.ckbSyncCusomers = ((System.Windows.Controls.CheckBox)(target));
            
            #line 50 "..\..\..\Views\SyncIrsView.xaml"
            this.ckbSyncCusomers.Checked += new System.Windows.RoutedEventHandler(this.OnSyncCusomtersChecked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ckbSyncContracts = ((System.Windows.Controls.CheckBox)(target));
            
            #line 51 "..\..\..\Views\SyncIrsView.xaml"
            this.ckbSyncContracts.Checked += new System.Windows.RoutedEventHandler(this.OnSyncContractsChecked);
            
            #line default
            #line hidden
            
            #line 51 "..\..\..\Views\SyncIrsView.xaml"
            this.ckbSyncContracts.Unchecked += new System.Windows.RoutedEventHandler(this.OnSyncContractsUnchecked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ckbSyncPayments = ((System.Windows.Controls.CheckBox)(target));
            
            #line 52 "..\..\..\Views\SyncIrsView.xaml"
            this.ckbSyncPayments.Checked += new System.Windows.RoutedEventHandler(this.OnSyncPaymentsChecked);
            
            #line default
            #line hidden
            
            #line 52 "..\..\..\Views\SyncIrsView.xaml"
            this.ckbSyncPayments.Unchecked += new System.Windows.RoutedEventHandler(this.OnSyncPaymentsUnchecked);
            
            #line default
            #line hidden
            return;
            case 9:
            this.ckbSyncShedPays = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 10:
            this.lblCurrentOperation = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.prog = ((System.Windows.Controls.ProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

