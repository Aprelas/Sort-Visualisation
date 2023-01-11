using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace КП
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static void RemoveRoutedEventHandlers(UIElement element, RoutedEvent routedEvent)
        {
            // Get the EventHandlersStore instance which holds event handlers for the specified element.
            // The EventHandlersStore class is declared as internal.
            var eventHandlersStoreProperty = typeof(UIElement).GetProperty(
                "EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
            object eventHandlersStore = eventHandlersStoreProperty.GetValue(element, null);

            // If no event handlers are subscribed, eventHandlersStore will be null.
            // Credit: https://stackoverflow.com/a/16392387/1149773
            if (eventHandlersStore == null)
                return;

            // Invoke the GetRoutedEventHandlers method on the EventHandlersStore instance 
            // for getting an array of the subscribed event handlers.
            var getRoutedEventHandlers = eventHandlersStore.GetType().GetMethod(
                "GetRoutedEventHandlers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var routedEventHandlers = (RoutedEventHandlerInfo[])getRoutedEventHandlers.Invoke(
                eventHandlersStore, new object[] { routedEvent });

            // Iteratively remove all routed event handlers from the element.
            foreach (var routedEventHandler in routedEventHandlers)
                element.RemoveHandler(routedEvent, routedEventHandler.Handler);
        }
        Animator animator = new Animator();
        double speed;
        bool Animate = true;
        public class pointer
        {
            public Animator.Point point;
            public int Num;
            public pointer Create(Animator.Point point, int Num)
            {
                this.point = point;
                this.Num = Num;
                return this;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Instruction_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Для начала работы программы, в нижней части программы, следует выбрать желаемую размерность массива " +
                "(по умолчанию 2), затем нажать кнопку <Начать сортировку>, для остановки хода сортировки, следует нажать кнопку " +
                "<Остановить сортировку>, для продолжения <Продолжить сортировку>, " +
                "для измененич скорости, следует изменить значение возле надписи <Скорость> ");
        }
        private async void Merge(object sender, RoutedEventArgs e)
        {

            animator.Stop(Sort_Table);
            animator.Clear(Sort_Table);
            I_Image.Visibility = Visibility.Visible;
            J_Image.Visibility = Visibility.Visible;
            animator = new Animator();
            speed = Convert.ToDouble(Speed_TextBox.Text);
            List<pointer> Start_Poses = new List<pointer>();
            Sort_Table.Children.Clear();
            int Length = Int32.Parse(Array_length.Text);
            Array_Unit[] Array_Units = new Array_Unit[Length];
            var Rnd = new Random(DateTime.Now.Minute * 60 + DateTime.Now.Second);
            int Width = 70;

            for (int i = 0; i < Length; i++)
            {
                Animator.Point Tmp_Pos1 = new Animator.Point();
                double Horizontal_Pos = (i - Length / 2) * Width * 1.5;
                Array_Units[i] = new Array_Unit().Set(Sort_Table, Tmp_Pos1.Create(Horizontal_Pos, 0), Width);
                Sort_Table.Children.Add(Array_Units[i].Element());
                Array_Units[i].Value(Rnd.Next(0, 100));
                Start_Poses.Add(new pointer().Create(Tmp_Pos1, i % 2 == 1 ? i - 1 : i));
                if (i % 2 == 1)
                    Array_Units[i].Element().Background = Brushes.Brown;
                else
                    Array_Units[i].Element().Background = Brushes.Yellow;

            }

            J_Image.Margin = new Thickness(Start_Poses[1].point.X, Start_Poses[1].point.Y + Array_Units[1].Element().Height, 0, 0);
            I_Image.Margin = new Thickness(Start_Poses[0].point.X, Start_Poses[0].point.Y + Array_Units[0].Element().Height, 0, 0);
            I_Image.HorizontalAlignment = HorizontalAlignment.Left;
            J_Image.HorizontalAlignment = HorizontalAlignment.Left;
            I_Image.VerticalAlignment = VerticalAlignment.Top;
            J_Image.VerticalAlignment = VerticalAlignment.Top;
            I_Image.HorizontalAlignment = HorizontalAlignment.Center;
            J_Image.HorizontalAlignment = HorizontalAlignment.Center;

            I_Image.Visibility = Visibility.Visible;
            J_Image.Visibility = Visibility.Visible;
            Sort_Table.Children.Add(I_Image);
            Sort_Table.Children.Add(J_Image);
            pointer TMP_pointer = new pointer();
            int L = 2;
            var TMP = new Array_Unit();
            while (L < Array_Units.Count() * 2)
            {
                int i1 = 0;
                int step = 1;
                while (i1 < Array_Units.Count() - 1)
                {

                    for (int i2 = i1; i2 < Array_Units.Count() && i2 < L * step; i2++)
                    {
                        if (Convert.ToInt32(Array_Units[i1].Element().Text) > Convert.ToInt32(Array_Units[i2].Element().Text))
                        {
                            TMP = Array_Units[i1];
                            Array_Units[i1] = Array_Units[i2];
                            Array_Units[i2] = TMP;
                        }
                    }
                    if (i1 == L * step - 1)
                        step++;
                    i1++;
                }
                step = 0;
                double mid = 0;
                I_Image.Margin = new Thickness(Start_Poses[0].point.X, Start_Poses[0].point.Y + Array_Units[0].Element().Height, 0, 0);
                for (int i = 0; i < Array_Units.Count(); i++)
                {
                    if (Animate)
                    {
                        if (i % L == 0)
                        {
                            I_Image.Visibility = Visibility.Visible;
                            J_Image.Visibility = Visibility.Visible;
                            step++;
                            if (i != 0)
                                I_Image.Margin = new Thickness(Start_Poses[i].point.X, Start_Poses[i].point.Y + Array_Units[i].Element().Height, 0, 0);
                            if (L * (step - 1) + L / 2 < Start_Poses.Count) {
                                J_Image.Margin = new Thickness(Start_Poses[L * (step - 1) + L / 2].point.X, Start_Poses[L * (step - 1) + L / 2].point.Y + Array_Units[L * (step - 1) + L / 2].Element().Height, 0, 0);
                            }
                            else
                            {
                                J_Image.Visibility = Visibility.Hidden;
                            }
                            mid = J_Image.Margin.Left;
                            if (I_Image.Margin.Left >= mid)
                                I_Image.Visibility = Visibility.Hidden;
                        }

                        double time = 500 / speed;
                        var duration = TimeSpan.FromSeconds(time / 2);
                        await Task.Delay(duration);
                        //if (Move_Indexes)
                        {
                            if (mid > Array_Units[i].Element().Margin.Left)
                            {

                                I_Image.Margin = new Thickness(I_Image.Margin.Left + Array_Units[0].Element().Width * 1.5, Start_Poses[i].point.Y + Array_Units[0].Element().Height, 0, 0);
                                if (I_Image.Margin.Left >= mid)
                                    I_Image.Visibility = Visibility.Hidden;
                            }
                            else
                            {
                                if (J_Image.Margin.Left == Start_Poses.Last().point.X)
                                    J_Image.Visibility = Visibility.Hidden;

                                J_Image.Margin = new Thickness(J_Image.Margin.Left + Array_Units[0].Element().Width * 1.5, Start_Poses[i].point.Y + Array_Units[0].Element().Height, 0, 0);
                                if (J_Image.Margin.Left >= mid + Array_Units[0].Element().Width * 1.5 * (L / 2))
                                    J_Image.Visibility = Visibility.Hidden;
                            }
                        }
                        duration = TimeSpan.FromSeconds(time / 4);
                        await Task.Delay(duration);
                        time = 500 / speed;
                        duration = TimeSpan.FromSeconds(1.4 * time);
                        Array_Unit AU = new Array_Unit();
                        Animator.Point Tmp_Pos1 = new Animator.Point();
                        Tmp_Pos1.Create(Array_Units[i].Element().Margin.Left, Array_Units[i].Element().Margin.Top);
                        AU.Set(Sort_Table, Tmp_Pos1, Width);
                        AU.Element().Text = Convert.ToString(Array_Units[i].Value());
                        if (step % 2 == 0)
                            AU.Element().Background = Brushes.Brown;
                        else
                            AU.Element().Background = Brushes.Yellow;
                        if (Sort_Table.Children.Contains(Array_Units[i].Element()))
                            Sort_Table.Children.Add(AU.Element());
                        else break;
                        Array_Units[i] = AU;
                        Start_Poses[i].point.Create(Start_Poses[i].point.X, Start_Poses[i].point.Y + Array_Units[i].Element().Height * 2);
                        animator.MoveTo(Array_Units[i].Element(), speed, Start_Poses[i].point);
                        await Task.Delay(duration);
                    }
                    else
                    {
                        i--;
                        await Task.Delay(10);
                    }
                    //I_Image.Visibility = Visibility.Visible;
                    //J_Image.Visibility = Visibility.Visible;
                }
                L *= 2;
                //I_Image.Margin = new Thickness(Start_Poses[0].point.X, Start_Poses[0].point.Y + Array_Units[0].Element().Height, 0, 0);
            }
            I_Image.Visibility = Visibility.Hidden;
            J_Image.Visibility = Visibility.Hidden;
            GC.Collect();
        }
        private async void Fast(object sender, RoutedEventArgs e)
        {

            animator.Stop(Sort_Table);
            animator.Clear(Sort_Table);
            animator = new Animator();
            speed = Convert.ToDouble(Speed_TextBox.Text);
            List<pointer> Start_Poses = new List<pointer>();
            Sort_Table.Children.Clear();
            int Length = Int32.Parse(Array_length.Text);
            Array_Unit[] Array_Units = new Array_Unit[Length];
            var Rnd = new Random(DateTime.Now.Minute * 60 + DateTime.Now.Second);
            int Width = 70;
            for (int i = 0; i < Length; i++)
            {
                Animator.Point Tmp_Pos1 = new Animator.Point();
                double Horizontal_Pos = (i - Length / 2) * Width * 1.5;
                Array_Units[i] = new Array_Unit().Set(Sort_Table, Tmp_Pos1.Create(Horizontal_Pos, 0), Width);
                Sort_Table.Children.Add(Array_Units[i].Element());
                Array_Units[i].Value(Rnd.Next(0, 100));
                Start_Poses.Add(new pointer().Create(Tmp_Pos1, i % 2 == 1 ? i - 1 : i));
            }
            bool sorted(int i1,int j)
            {
                for(int i = i1; i<j;i++ )
                {
                    if (Array_Units[i].Value() > Array_Units[i + 1].Value())
                        return false;
                }
                return true;
            }
            async Task shell(int i_index, int j_index)
            {
                TimeSpan duration;
                double time;
                int from = i_index;
                int to = j_index;
                int mid = (int)Math.Floor(Convert.ToDouble(Array_Units[i_index].Value() + Array_Units[j_index].Value()) / 2);
                Label label = new Label();
                label.Content = mid;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Top;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                Sort_Table.Children.Add(label);
                label.Margin = new Thickness(Array_Units[j_index].Arr_Element.Margin.Left, Array_Units[j_index].Arr_Element.Margin.Top + Array_Units[1].Element().Height, 0, 0);
                while (i_index != j_index)
                {
                    while ((int)Array_Units[i_index].Value() < mid)
                    {
                        if (i_index == j_index)
                            break;
                        i_index++;
                    }
                    while ((int)Array_Units[j_index].Value() >= mid)
                    {
                        if (i_index == j_index)
                            break;

                        j_index--;
                    }
                    if (i_index != j_index)
                    {
                        Array_Unit AU = new Array_Unit();
                        AU = Array_Units[i_index];
                        Array_Units[i_index] = Array_Units[j_index];
                        Array_Units[j_index] = AU;
                    }
                    if (i_index != j_index&&i_index<mid)
                        i_index++;
                    if (i_index != j_index&&j_index>=mid)
                        j_index--;

                }
                for(int i=from; i != to+1; i++)
                {
                    Array_Unit AU = new Array_Unit();
                    Animator.Point Tmp_Pos1 = new Animator.Point();
                    Tmp_Pos1.Create(Array_Units[i].Element().Margin.Left, Array_Units[i].Element().Margin.Top);
                    AU.Set(Sort_Table, Tmp_Pos1, Width);
                    AU.Element().Text = Convert.ToString(Array_Units[i].Value());
                    if (Sort_Table.Children.Contains(Array_Units[i].Element()))
                        Sort_Table.Children.Add(AU.Element());
                    else break;
                    Array_Units[i] = AU;
                    Start_Poses[i].point.Create(Start_Poses[i].point.X, Start_Poses[i].point.Y + Array_Units[i].Element().Height * 2);
                    animator.MoveTo(Array_Units[i].Element(), speed, Start_Poses[i].point);
                    time = 500 / speed;
                    duration = TimeSpan.FromSeconds(time);
                    await Task.Delay(duration);
                }
                
                if (i_index - from > 1&&!sorted(from,i_index-1))
                    await shell(from, i_index-1);
                else
                    Array_Units[from].Arr_Element.Background = Brushes.Green;
                if (sorted(from, i_index-1))
                    for (int i = from; i < i_index -1; i++)
                        Array_Units[i].Arr_Element.Background = Brushes.Green;
                time = 500 / speed / 2;
                duration = TimeSpan.FromSeconds(time);
                await Task.Delay(duration);
                if (to - i_index + 1 > 1 && !sorted(i_index,to))
                    await shell(i_index, to);
                else
                    Array_Units[to].Arr_Element.Background = Brushes.Green;
                        
                if (i_index-1 == from && j_index == to&&!sorted(from,to))
                {
                    Array_Unit AU = new Array_Unit();
                    AU = Array_Units[i_index];
                    Array_Units[i_index] = Array_Units[j_index];
                    Array_Units[j_index] = AU;
                }
                if (sorted(i_index, to))
                    for (int i = i_index; i < to + 1; i++)
                        Array_Units[i].Arr_Element.Background = Brushes.Green;
            }
            int start = 0;
            int end = Length-1;
            shell(start, end);
        }
        private void Censor(object sender, TextChangedEventArgs e)
        {
                TextBox textBox = sender as TextBox;
                Int32 selectionStart = textBox.SelectionStart;
                Int32 selectionLength = textBox.SelectionLength;

                String newText = String.Empty;
                foreach (Char c in textBox.Text.ToCharArray())
                {
                    if (Char.IsDigit(c) || Char.IsControl(c)) newText += c;
                }

                textBox.Text = newText;
                textBox.SelectionStart = selectionStart <= textBox.Text.Length ?
                selectionStart : textBox.Text.Length;
            if( textBox.Text.Length>2)
                speed = Convert.ToDouble(textBox.Text);
        }


        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            animator.Stop(Sort_Table);
            Animate = false;
        }

        private void Resume_Button_Click(object sender, RoutedEventArgs e)
        {
            animator.Resume(Sort_Table);
            Animate = true;
        }


        private void Fast_Sort_Button(object sender, RoutedEventArgs e)
        {
            this.Title = "Быстрая сортировка";
            RemoveRoutedEventHandlers(Sort_Start, Button.ClickEvent);
            Sort_Start.Click += new RoutedEventHandler(Fast);
        }

        private void Merge_Sort_Button(object sender, RoutedEventArgs e)
        {
            this.Title = "Сортировка слиянием";
            RemoveRoutedEventHandlers(Sort_Start, Button.ClickEvent);
            Sort_Start.Click += new RoutedEventHandler(Merge);
        }
    }
}