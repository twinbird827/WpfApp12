using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp12
{
    public class ScrollViewerSyncBehavior
    {
        // 列ﾍｯﾀﾞの依存関係ﾌﾟﾛﾊﾟﾃｨ
        public static DependencyProperty ColumnHeaderProperty =
            DependencyProperty.RegisterAttached("ColumnHeader",
                typeof(FrameworkElement),
                typeof(ScrollViewerSyncBehavior),
                new UIPropertyMetadata(OnHeaderCallback)
            );

        // 列ﾍｯﾀﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        public static void SetColumnHeader(DependencyObject target, object value)
        {
            target.SetValue(ColumnHeaderProperty, value);
        }

        // 列ﾍｯﾀﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        public static FrameworkElement GetColumnHeader(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(ColumnHeaderProperty);
        }

        // 行ﾍｯﾀﾞの依存関係ﾌﾟﾛﾊﾟﾃｨ
        public static DependencyProperty RowHeaderProperty =
            DependencyProperty.RegisterAttached("RowHeader",
                typeof(FrameworkElement),
                typeof(ScrollViewerSyncBehavior),
                new UIPropertyMetadata(OnHeaderCallback)
            );

        // 行ﾍｯﾀﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        public static void SetRowHeader(DependencyObject target, object value)
        {
            target.SetValue(RowHeaderProperty, value);
        }

        // 列ﾍｯﾀﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        public static FrameworkElement GetRowHeader(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(RowHeaderProperty);
        }

        // 列ﾌｯﾀの依存関係ﾌﾟﾛﾊﾟﾃｨ
        public static DependencyProperty ColumnFooterProperty =
            DependencyProperty.RegisterAttached("ColumnFooter",
                typeof(FrameworkElement),
                typeof(ScrollViewerSyncBehavior),
                new UIPropertyMetadata()
            );

        // 列ﾌｯﾀを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        public static void SetColumnFooter(DependencyObject target, object value)
        {
            target.SetValue(ColumnFooterProperty, value);
        }

        // 列ﾌｯﾀを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        public static FrameworkElement GetColumnFooter(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(ColumnFooterProperty);
        }

        // 行ﾌｯﾀの依存関係ﾌﾟﾛﾊﾟﾃｨ
        public static DependencyProperty RowFooterProperty =
            DependencyProperty.RegisterAttached("RowFooter",
                typeof(FrameworkElement),
                typeof(ScrollViewerSyncBehavior),
                new UIPropertyMetadata()
            );

        // 行ﾌｯﾀを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        public static void SetRowFooter(DependencyObject target, object value)
        {
            target.SetValue(RowFooterProperty, value);
        }

        // 列ﾌｯﾀを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        public static FrameworkElement GetRowFooter(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(RowFooterProperty);
        }

        // ﾍｯﾀﾞﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        private static void OnHeaderCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var viewer = sender as ScrollViewer ?? ScrollViewerFromFrameworkElement(sender as FrameworkElement);
            // ﾒｲﾝの表でｽｸﾛｰﾙされた際のｲﾍﾞﾝﾄ
            viewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            viewer.ScrollChanged += ScrollViewer_ScrollChanged;

            var target = e.NewValue as FrameworkElement;
            // ﾍｯﾀﾞ位置でﾏｳｽﾎｲｰﾙされた際のｲﾍﾞﾝﾄ
            target.PreviewMouseWheel -= FrameworkElement_MouseWheel;
            target.PreviewMouseWheel += FrameworkElement_MouseWheel;
            // ﾍｯﾀﾞ位置で矢印が押下された際のｲﾍﾞﾝﾄ
            target.PreviewKeyDown -= FrameworkElement_KeyDown;
            target.PreviewKeyDown += FrameworkElement_KeyDown;
        }

        // ﾒｲﾝの表でｽｸﾛｰﾙされた際のｲﾍﾞﾝﾄ
        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // ﾒｲﾝの表
            var sv = sender as ScrollViewer;
            // 列ﾍｯﾀﾞ
            var hfe = GetColumnHeader(sv);
            var hsv = hfe as ScrollViewer ?? ScrollViewerFromFrameworkElement(hfe);
            // 行ﾍｯﾀﾞ
            var rfe = GetRowHeader(sv);
            var rsv = rfe as ScrollViewer ?? ScrollViewerFromFrameworkElement(rfe);
            // 列ﾌｯﾀ
            var hfo = GetColumnFooter(sv);
            // 行ﾌｯﾀ
            var rfo = GetRowFooter(sv);

            if (hfo != null) hfo.Width = GetScrollWidth(sv);
            if (rfo != null) rfo.Height = GetScrollHeight(sv);
            if (hsv != null) hsv.ScrollToHorizontalOffset(sv.HorizontalOffset);
            if (rsv != null) rsv.ScrollToVerticalOffset(sv.VerticalOffset);
        }

        // ﾍｯﾀﾞ位置でﾏｳｽﾎｲｰﾙされた際のｲﾍﾞﾝﾄ
        private static void FrameworkElement_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // ﾍｯﾀﾞに対するﾏｳｽﾎｲｰﾙを無効化(ｽｸﾛｰﾙできないようにする)
            e.Handled = true;
        }

        // ﾍｯﾀﾞ位置で矢印が押下された際のｲﾍﾞﾝﾄ
        private static void FrameworkElement_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.Down:
                case Key.Left:
                case Key.Right:
                    e.Handled = true;
                    break;
            }
        }

        // ｺﾝﾄﾛｰﾙ内のｽｸﾛｰﾙを取得します。
        private static ScrollViewer ScrollViewerFromFrameworkElement(FrameworkElement frameworkElement)
        {
            if (VisualTreeHelper.GetChildrenCount(frameworkElement) == 0) return null;

            FrameworkElement child = VisualTreeHelper.GetChild(frameworkElement, 0) as FrameworkElement;

            if (child == null) return null;

            if (child is ScrollViewer)
            {
                return (ScrollViewer)child;
            }

            return ScrollViewerFromFrameworkElement(child);
        }

        // ｽｸﾛｰﾙﾊﾞｰの幅を取得します。
        private static double GetScrollWidth(ScrollViewer sv)
        {
            var sb = sv.Template.FindName("PART_VerticalScrollBar", sv) as ScrollBar;

            if (sb == null) return 0d;

            return sb.ActualWidth;
        }

        // ｽｸﾛｰﾙﾊﾞｰの高さを取得します。
        private static double GetScrollHeight(ScrollViewer sv)
        {
            var sb = sv.Template.FindName("PART_HorizontalScrollBar", sv) as ScrollBar;

            if (sb == null) return 0d;

            return sb.ActualHeight;
        }

    }
}
