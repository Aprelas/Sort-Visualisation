using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace КП
{//KamshilovDS
    internal class Array_Unit
    {
        public TextBox Arr_Element;

        public Array_Unit Set(Grid Window,Animator.Point point, int Width)
        {
            //this.Arr_Element = new TextBox();
            this.Arr_Element.TextWrapping = TextWrapping.Wrap;
            this.Arr_Element.Width = Width;
            this.Arr_Element.Height = 40;
            this.Arr_Element.FontSize = 16;
            this.Arr_Element.MinWidth = 20;
            this.Arr_Element.MaxWidth = 50;
            this.Arr_Element.HorizontalAlignment = HorizontalAlignment.Left;
            this.Arr_Element.VerticalAlignment = VerticalAlignment.Top;
            this.Arr_Element.Margin = new Thickness(point.X,point.Y,0,0);
            this.Arr_Element.HorizontalAlignment = HorizontalAlignment.Center;
            this.Arr_Element.IsReadOnly = true;
            GC.Collect();
            return this;
        }
        public int Value()
        {
            return Convert.ToInt32(this.Arr_Element.Text);
        }
        public void Value(double val)
        {
            this.Arr_Element.Text = val.ToString();
        }
        public TextBox Element()
        {
            return Arr_Element;
        }
    }

    public class Animator
    {

        private Storyboard storybord1 = new Storyboard();
        private Storyboard storybord2 = new Storyboard();
        private ThicknessAnimation Anim1 = new ThicknessAnimation();
        private ThicknessAnimation Anim2 = new ThicknessAnimation();

        public class Point
        {
            public double X { get; private set; }
            public double Y { get; private set; }
            public Point Create(double x, double y)
            {
                
                this.X = x;
                this.Y = y;
                return this;
            }
        }
        public void Clear(Grid window)
        {
            storybord1.Stop(window);
            storybord2.Stop(window);
            storybord1.Remove(window);
            storybord2.Remove(window);
        }
        void Add(UIElement Target, Thickness? from, Thickness? to, Storyboard storyboard, int step, double speed)
        {
            double time = 500 / speed;
            var duration = TimeSpan.FromSeconds(time);
            Anim1.From = from;
            Anim1.To = to;
            Anim1.Duration = new Duration(duration);
            Anim1.BeginTime = TimeSpan.FromSeconds(time * step);
            Storyboard.SetTargetProperty(Anim1, new PropertyPath(FrameworkElement.MarginProperty));
            storyboard.Children.Add(Anim1);
            GC.Collect();
        }

         public void Swap_Line(TextBox Arr_Element1, TextBox Arr_Element2, double speed)
         {
            Point p1 = new Point().Create(Arr_Element1.Margin.Left, Arr_Element1.Margin.Top);
            Point p2 = new Point().Create(Arr_Element2.Margin.Left, Arr_Element2.Margin.Top);
            double time = Math.Sqrt(p1.X * p2.X + p1.Y * p2.Y) / speed;
            Anim1.From = Arr_Element1.Margin; 
            Anim1.To = Arr_Element2.Margin;
            Anim1.Duration = new Duration(TimeSpan.FromSeconds(time));
            Anim2.From = Arr_Element2.Margin;
            Anim2.To = Arr_Element1.Margin;
            Anim2.Duration = new Duration(TimeSpan.FromSeconds(time)); 
            Arr_Element1.BeginAnimation(TextBox.MarginProperty,Anim1);
            Arr_Element2.BeginAnimation(TextBox.MarginProperty, Anim2);
            GC.Collect();
        }
        public void MoveTo(TextBox Arr_Element, double speed,Point To)
        {
            Point Pos = new Point().Create(Arr_Element.Margin.Left, Arr_Element.Margin.Top);
            Storyboard.SetTarget(this.storybord1, Arr_Element);
            Add(Arr_Element, Arr_Element.Margin, new Thickness(To.X, To.Y, 0, 0), storybord1, 0, speed);
            storybord1.Begin();
            GC.Collect();
        }
        public void MoveTo(Image Arr_Element, double speed, Point To)
        {
            Point Pos = new Point().Create(Arr_Element.Margin.Left, Arr_Element.Margin.Top);
            Storyboard.SetTarget(this.storybord1, Arr_Element);
            Add(Arr_Element, Arr_Element.Margin, new Thickness(To.X, To.Y, 0, 0), storybord1, 0, speed);
            storybord1.Begin();
            GC.Collect();
        }
        public void Swap_Circle(TextBox Arr_Element1, TextBox Arr_Element2, double speed)
        {
            Point p1 = new Point().Create(Arr_Element1.Margin.Left, Arr_Element1.Margin.Top);
            Point p2 = new Point().Create(Arr_Element2.Margin.Left, Arr_Element2.Margin.Top);
            Storyboard.SetTarget(this.storybord1, Arr_Element1);
            Storyboard.SetTarget(this.storybord2, Arr_Element2);
            double Height = 100;
            Add(Arr_Element1,  Arr_Element1.Margin, new Thickness(p1.X, p1.Y + -Height, 0, 0),storybord1,0,speed);
            Add(Arr_Element2,  Arr_Element2.Margin, new Thickness(p2.X, p2.Y + Height, 0, 0), storybord2,0,speed);
            Add(Arr_Element1, new Thickness(p1.X, p1.Y + -Height, 0, 0), new Thickness(p2.X, p2.Y + -Height, 0, 0), storybord1,1,speed);
            Add(Arr_Element2, new Thickness(p2.X, p2.Y + Height, 0, 0), new Thickness(p1.X, p1.Y + Height, 0, 0), storybord2, 1, speed);
            Add(Arr_Element1, new Thickness(p2.X, p2.Y + -Height, 0, 0), Arr_Element2.Margin, storybord1, 2,speed);
            Add(Arr_Element2,  new Thickness(p1.X, p1.Y + Height, 0, 0), Arr_Element1.Margin, storybord2, 2,speed);
            storybord1.Begin();
            storybord2.Begin();
        }
        public void Stop(Grid window)
        {
            storybord1.Stop(window);
            storybord2.Stop(window);
        }
        public void Resume(Grid window)
        {
            storybord1.Resume(window);
            storybord2.Resume(window);
        }
    }
}
