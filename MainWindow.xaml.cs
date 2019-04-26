using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace WpfApp12
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        public ObservableCollection<ObservableCollection<string>> Items { get; set; } = new ObservableCollection<ObservableCollection<string>>();

        public ObservableCollection<string> RowHeaders { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> ColumnHeaders { get; set; } = new ObservableCollection<string>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var random = new Random();
            var row = random.Next(50, 100);
            var col = random.Next(50, 100);

            RowHeaders.Clear();
            Enumerable.Range(0, row).ToList()
                .ForEach(r => RowHeaders.Add($"R{r}"));

            ColumnHeaders.Clear();
            Enumerable.Range(0, col).ToList()
                .ForEach(c => ColumnHeaders.Add($"C{c}"));

            Items.Clear();
            RowHeaders.ToList()
                .ForEach(r => Items.Add(new ObservableCollection<string>(ColumnHeaders.Select(c => $"{c}-{r}"))));
        }
    }
}
