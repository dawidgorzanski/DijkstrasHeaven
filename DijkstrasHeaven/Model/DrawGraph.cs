using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DijkstrasHeaven.Model
{
    public class DrawGraph
    {
        private Canvas _canvas;

        public Graph CurrentGraph { get; set; }
        public int Radius { get; set; }
        public int NodeRadius { get; set; }

        public event EventHandler NodeClicked;
        protected virtual void OnNodeClicked(Node clickedNode)
        {
            var handler = NodeClicked;
            if (handler != null)
                handler(clickedNode, EventArgs.Empty);
        }

        public DrawGraph(Canvas canvas, Graph graph)
        {
            this.CurrentGraph = graph;
            this._canvas = canvas;
        }

        //rysowanie głównego koła
        public void DrawMainCircle()
        {
            Ellipse mainEllipse = new Ellipse();
            mainEllipse.SetResourceReference(Ellipse.StrokeProperty, "ColorCircle");
            mainEllipse.StrokeThickness = 1;
            mainEllipse.Height = mainEllipse.Width = 2 * Radius;

            //Ustawiane w ten sposób, gdyz punkt (0,0) elementu to lewy górny róg, a nie jego środek
            Canvas.SetLeft(mainEllipse, (_canvas.ActualWidth / 2) - Radius + (NodeRadius / 2));
            Canvas.SetTop(mainEllipse, (_canvas.ActualHeight / 2) - Radius + (NodeRadius / 2));

            _canvas.Children.Insert(0, mainEllipse);
        }

        //rysowanie punktów
        private void DrawNodes()
        {
            double a = _canvas.ActualWidth / 2;
            double b = _canvas.ActualHeight / 2;

            for (int i = 0; i < CurrentGraph.Nodes.Count; i++)
            {
                double t = 2 * Math.PI * i / CurrentGraph.Nodes.Count;
                int x = (int)Math.Round(a + Radius * Math.Cos(t));
                int y = (int)Math.Round(b + Radius * Math.Sin(t));

                CurrentGraph.Nodes[i].PointOnScreen = new Point(x, y);

                Ellipse ellipse = new Ellipse();
                ellipse.SetResourceReference(Ellipse.StrokeProperty, "ColorPoints");
                ellipse.SetResourceReference(Ellipse.FillProperty, "ColorPoints");
                ellipse.Height = NodeRadius;
                ellipse.Width = NodeRadius;
                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);
                _canvas.Children.Add(ellipse);

                Label label = new Label();
                label.DataContext = CurrentGraph.Nodes[i];
                Binding binding = new Binding();
                binding.Path = new PropertyPath("ID");
                BindingOperations.SetBinding(label, Label.ContentProperty, binding);
                label.Height = 50;
                label.Width = 50;
                label.MouseEnter += Label_MouseEnter;
                label.MouseLeave += Label_MouseLeave;
                label.MouseLeftButtonUp += Label_MouseLeftButtonUp;
                
                Canvas.SetLeft(label, x - 15);
                Canvas.SetTop(label, y - 15);
                _canvas.Children.Add(label);
            }
        }

        private void Label_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Node clickedNode = ((Label)sender).DataContext as Node;
            OnNodeClicked(clickedNode);
        }

        private void Label_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Label_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        //rysowanie pełnego grafu
        public bool Draw()
        {
            if (CurrentGraph.Nodes.Count == 0)
                return false;

            //Rysowanie punktów
            DrawNodes();

            //Rysowanie linii
            foreach (Connection connection in CurrentGraph.Connections)
            {
                DrawLine(connection);
            }

            return true;
        }

        //rysowanie linii od punktu node1 do punktu node2
        private void DrawLine(Connection connection)
        {
            Line line = new Line();
            line.StrokeThickness = 1;

            line.Stroke = connection.LineColor;

            line.X1 = connection.Node1.PointOnScreen.X + NodeRadius / 2;
            line.X2 = connection.Node2.PointOnScreen.X + NodeRadius / 2;
            line.Y1 = connection.Node1.PointOnScreen.Y + NodeRadius / 2;
            line.Y2 = connection.Node2.PointOnScreen.Y + NodeRadius / 2;
            
            //Insert() zamiast Add(), aby linie były "pod spodem" - liczy się kolejność dodawania, im dalej na liście tym "wyżej"
            _canvas.Children.Insert(0, line);

            Label label = new Label();
            label.Foreground = Brushes.DarkBlue;
            label.Content = connection.Weight;
            double x = (connection.Node1.PointOnScreen.X + connection.Node2.PointOnScreen.X) / 2;
            double y = (connection.Node1.PointOnScreen.Y + connection.Node2.PointOnScreen.Y) / 2;
            Canvas.SetLeft(label, x - 15);
            Canvas.SetTop(label, y);
            _canvas.Children.Add(label);
        }

        public void ClearAll(bool OnlyView = true)
        {
            //OnlyView - czyści tylko "rysunek"
            //żeby nie było null
            if (!OnlyView)
                CurrentGraph = GraphCreator.CreateFullGraph();

            _canvas.Children.Clear();
        }
    }
}
