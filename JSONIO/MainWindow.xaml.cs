using System;
using System.Collections.Generic;
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
using JSONIOLIB;

namespace JSONIO
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        IParser _Parser;
        public MainWindow()
        {
            InitializeComponent();
            _Parser = Parser.Create();


        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
           CSHBox.Text =  _Parser.GetClass(JSONBox.Text); 
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            RootClass newClass = JSONConverter.Deserialize<RootClass>(JSONBox.Text);
        }

        private void SerializeBtn(object sender, RoutedEventArgs e)
        {
            Random ran = new Random();
            RootClass newClass = new RootClass();
            newClass.ArrayDouble = new double[] { ran.NextDouble(), ran.NextDouble() };
            newClass.ArrayInt = new int[] {ran.Next(), ran.Next() };
            newClass.Hour = DateTime.Now.Hour;
            newClass.Seconds = DateTime.Now.Second;
            newClass.Say = ran.Next().ToString() + " = Число";
            newClass.Object1 = new ClassName0() { Exp = 2.718, PI = 3.14};
            newClass.Object2 = new ClassName1() { Speed = ran.NextDouble() * 100};
            newClass.Object3 = new ClassName2() { Power = new ClassName4() { Wattage = ran.Next() } };
            newClass.Object4 = new ClassName3() { Current = new ClassName5() { Active = new ClassName6() { Sin = ran.NextDouble() } } };
            JSONBox.Text = JSONConverter.Serialize(newClass);
        }
    }
}
