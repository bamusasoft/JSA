using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Jsa.ViewsModel.UIBehaviors
{
    [DebuggerNonUserCode]
    public class DataGridBehavior
    {
        public static DependencyProperty DisplayRowNumberProperty =
            DependencyProperty.RegisterAttached("DisplayRowNumber",
                                                typeof(bool),
                                                typeof(DataGridBehavior),
                                                new FrameworkPropertyMetadata(false, OnDisplayRowNumberChanged)
                );

        public static bool GetDisplayRowNumber(DependencyObject target)
        {
            return (bool)target.GetValue(DisplayRowNumberProperty);
        }

        public static void SetDisplayRowNumber(DependencyObject target, bool value)
        {
            target.SetValue(DisplayRowNumberProperty, value);
        }

        private static void OnDisplayRowNumberChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = target as DataGrid;
            if (dataGrid == null) return;
            if ((bool)e.NewValue)
            {
                EventHandler<DataGridRowEventArgs> loadRowHandler = null;
                loadRowHandler = (object sender, DataGridRowEventArgs ea) =>
                    {
                        if (GetDisplayRowNumber(dataGrid) == false)
                        {
                            dataGrid.LoadingRow -= loadRowHandler;
                            return;
                        }
                        ea.Row.Header = ea.Row.GetIndex() + 1;
                    };
                dataGrid.LoadingRow += loadRowHandler;
                ItemsChangedEventHandler itemsChangedHandler = null;
                itemsChangedHandler = (object sender, ItemsChangedEventArgs ea) =>
                    {
                        if (GetDisplayRowNumber(dataGrid) == false)
                        {
                            dataGrid.ItemContainerGenerator.ItemsChanged -= itemsChangedHandler;
                            return;
                        }
                        GetVisualChiledCollection<DataGridRow>(dataGrid).
                            ForEach(d => d.Header = d.GetIndex() + 1);
                    };
                dataGrid.ItemContainerGenerator.ItemsChanged += itemsChangedHandler;
            }
        }

        private static List<T> GetVisualChiledCollection<T>(object parent) where T : Visual
        {
            var visualCollection = new List<T>();
            GetVisualChiledCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        private static void GetVisualChiledCollection<T>(DependencyObject parent, List<T> visualCollection)
            where T : Visual
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    visualCollection.Add(child as T);
                }
                if (child != null)
                {
                    GetVisualChiledCollection(child, visualCollection);
                }
            }
        }
    }
}