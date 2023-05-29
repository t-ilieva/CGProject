using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Drawing.Imaging;

namespace Draw
{
    /// <summary>
    /// Класът, който ще бъде използван при управляване на диалога.
    /// </summary>
    public class DialogProcessor : DisplayProcessor
    {
        #region Constructor

        public DialogProcessor()
        {
            this.importedShapes = new List<Shape>();
            this.exportFormatDialog = new SaveFileDialog();
            this.importFormatDialog = new OpenFileDialog();
            this.formatter = new BinaryFormatter();
        }

        #endregion

        #region Properties

        private readonly BinaryFormatter formatter;
        private readonly SaveFileDialog exportFormatDialog;
        private readonly OpenFileDialog importFormatDialog;
        //private FileStream streamToFiles;
        public List<Shape> importedShapes;


        /// <summary>
        /// Избран елемент.
        /// </summary>
        private List<Shape> selection = new List<Shape>();
        public List<Shape> Selection
        {
            get { return selection; }
            set { selection = value; }
        }

        private List<Shape> shapeListCopy = new List<Shape>();
        public List<Shape> ShapeListCopy
        {
            get { return shapeListCopy; }
            set { shapeListCopy = value; }
        }

        /// <summary>
        /// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
        /// </summary>
        private bool isDragging;
        public bool IsDragging
        {
            get { return isDragging; }
            set { isDragging = value; }
        }

        /// <summary>
        /// Последна позиция на мишката при "влачене".
        /// Използва се за определяне на вектора на транслация.
        /// </summary>
        private PointF lastLocation;
        public PointF LastLocation
        {
            get { return lastLocation; }
            set { lastLocation = value; }
        }

        #endregion


        /// <summary>
        /// Проверява дали дадена точка е в елемента.
        /// Обхожда в ред обратен на визуализацията с цел намиране на
        /// "най-горния" елемент т.е. този който виждаме под мишката.
        /// </summary>
        /// <param name="point">Указана точка</param>
        /// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
        /// 


        public override void DrawShape(Graphics grfx, Shape item)
        {
            base.DrawShape(grfx, item);

            foreach (var shape in Selection)
            {
                grfx.DrawRectangle(Pens.Black, shape.Rectangle.X - 3, shape.Rectangle.Y - 3, shape.Rectangle.Width + 6, shape.Rectangle.Height + 6);
                grfx.ResetTransform();
            }

        }

        public Shape ContainsPoint(PointF point)
        {
            for (int i = ShapeList.Count - 1; i >= 0; i--)
            {
                if (ShapeList[i].Contains(point))
                {
                    //ShapeList[i].FillColor = Color.Red;

                    return ShapeList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
        /// </summary>
        /// <param name="p">Вектор на транслация.</param>
        public void TranslateTo(PointF p)
        {
            var x = p.X - lastLocation.X;
            var y = p.Y - lastLocation.Y;

            foreach (var shape in Selection)
            {
                if (shape is GroupShape)
                {
                    shape.GroupTranslateTo(x, y);
                }
                else
                {
                    shape.Location = new PointF(shape.Location.X + x, shape.Location.Y + y);
                }
            }

            lastLocation = p;

        }

        /// <summary>
        /// Добавя примитив - правоъгълник на произволно място върху клиентската област.
        /// </summary>
        /// 

        //ДОБАВЯНЕ НА ФИГУРИ
        public void AddRandomRectangle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            RectangleShape rect = new RectangleShape(new Rectangle(x, y, 100, 200));
            rect.FillColor = Color.White;
            rect.StrokeColor = Color.Black;
            rect.Opacity = 255;

            ShapeList.Add(rect);
        }

        public void AddRandomSquare()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            SquareShape square = new SquareShape(new Rectangle(x, y, 100, 100));
            square.FillColor = Color.White;
            square.StrokeColor = Color.Black;
            square.Opacity = 255;

            ShapeList.Add(square);
        }

        public void AddRandomElipse()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 100, 200));
            ellipse.FillColor = Color.White;
            ellipse.StrokeColor = Color.Black;
            ellipse.Opacity = 255;

            ShapeList.Add(ellipse);
        }

        public void AddRandomCircle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            CircleShape circle = new CircleShape(new Rectangle(x, y, 100, 100));
            circle.FillColor = Color.White;
            circle.StrokeColor = Color.Black;
            circle.Opacity = 255;

            ShapeList.Add(circle);
        }

        public void AddRandomEquilateralTriangle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            TriangleShape triangle = new TriangleShape(new Rectangle(x, y, 100, 100));
            triangle.FillColor = Color.White;
            triangle.StrokeColor = Color.Black;
            triangle.Opacity = 255;

            ShapeList.Add(triangle);
        }

        public void AddRandomLeftAngleTriangle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            TriangleShape2 triangle = new TriangleShape2(new Rectangle(x, y, 100, 100));
            triangle.FillColor = Color.White;
            triangle.StrokeColor = Color.Black;
            triangle.Opacity = 255;

            ShapeList.Add(triangle);
        }

        //МОДИФИКАЦИИ

        public void SelectFillColor(Color color)
        {
            foreach (var shape in Selection)
            {
                if(shape is GroupShape)
                {
                    shape.GroupFillColor(color);
                }
                else 
                {
                    shape.FillColor = color;
                }
            }
        }

        public void SelectBorderColor(Color color)
        {
            foreach (var shape in Selection)
            {
                if (shape is GroupShape)
                {
                    shape.GroupBorderColor(color);
                }
                else
                {
                    shape.StrokeColor = color;
                }
            }
        }

        public void SelectBorderWidth(int borderWidth)
        {
            foreach (var shape in Selection)
            {
                if (shape is GroupShape)
                {
                    shape.GroupBorderWidth(borderWidth);
                }
                else
                {
                    shape.BorderWidth = borderWidth;
                }
            }
        }

        public void SelectOpacity(int opacity)
        {
            foreach (var shape in Selection)
            {
                if (shape is GroupShape)
                {
                    shape.GroupOpacity(opacity);
                }
                else
                {
                    shape.Opacity = opacity;
                }
            }
        }


        //ГРУПИРАНЕ
        public void GroupSelectedShapes(List<Shape> shapes)
        {
            if (shapes.Count > 1)
            {
                float minX = float.PositiveInfinity;
                float minY = float.PositiveInfinity;
                float maxX = float.NegativeInfinity;
                float maxY = float.NegativeInfinity;
                foreach (var shape in shapes)
                {
                    if (minX > shape.Location.X)
                    {
                        minX = shape.Location.X;
                    }
                    if (minY > shape.Location.Y)
                    {
                        minY = shape.Location.Y;
                    }
                    if (maxX < shape.Location.X + shape.Width)
                    {
                        maxX = shape.Location.X + shape.Width;
                    }
                    if (maxY < shape.Location.Y + shape.Height)
                    {
                        maxY = shape.Location.Y + shape.Height;
                    }
                }
                GroupShape group = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));
                group.SubShape = shapes;

                foreach (var shape1 in shapes)
                {
                    ShapeList.Remove(shape1);
                }

                Selection = new List<Shape>();
                Selection.Add(group);
                ShapeList.Add(group);
            }
        }

        internal void GroupRemoveShapes(String shapeType)
        {
            List<Shape> newSelection = new List<Shape>(Selection);
            Selection.Clear();

            foreach (var shape in newSelection)
            {
                if (shape is GroupShape)
                {
                    GroupShape groupShape = (GroupShape)shape;
                    List<Shape> removed = new List<Shape>();
                    List<Shape> newSubShape = new List<Shape>();

                    foreach (var subShape in groupShape.SubShape)
                    {
                        if (subShape.GetType().Name.ToString() == shapeType)
                        {
                            removed.Add(subShape);
                        }
                        else if (shapeType.Equals("Triangles"))
                        {
                            if (subShape is TriangleShape || subShape is TriangleShape2)
                            {
                                removed.Add(subShape);
                            }
                            else
                            {
                                newSubShape.Add(subShape);
                            }
                        }
                        else
                        {
                            newSubShape.Add(subShape);
                        }
                    }

                    GroupSelectedShapes(newSubShape);
                    ShapeList.Remove(shape);
                    ShapeList.AddRange(removed);
                    Selection.AddRange(removed);
                }
            }
        }

        public void UngroupShape()
        {

            List<Shape> newSelection = new List<Shape>();

            foreach (var shape in Selection)
            {
                if (shape is GroupShape)
                {
                    GroupShape groupShape = (GroupShape)shape;

                    ShapeList.AddRange(groupShape.SubShape);
                    ShapeList.Remove(shape);

                    newSelection.AddRange(groupShape.SubShape);
                    groupShape.SubShape.Clear();
                }
            }

            Selection.Clear();
            Selection = newSelection;
        }


        //СЕЛЕКЦИЯ

        public override void Draw(Graphics grfx)
        {
            base.Draw(grfx);
        }
        public void SelectRectangles()
        {
            Selection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                if (shape is RectangleShape)
                {
                    Selection.Add(shape);
                }
            }
        }

        public void SelectSquares()
        {
            Selection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                if (shape is SquareShape)
                {
                    Selection.Add(shape);
                }
            }
        }

        public void SelectEllipses()
        {
            Selection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                if (shape is EllipseShape)
                {
                    Selection.Add(shape);
                }
            }
        }

        public void SelectCircles()
        {
            Selection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                if (shape is CircleShape)
                {
                    Selection.Add(shape);
                }
            }
        }

        public void SelectEquilateralTriangles()
        {
            Selection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                if (shape is TriangleShape)
                {
                    Selection.Add(shape);
                }
            }
        }

        public void SelectLeftAngleTriangles()
        {
            Selection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                if (shape is TriangleShape2)
                {
                    Selection.Add(shape);
                }
            }
        }

        public void SelectAllTriangles()
        {
            Selection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                if (shape is TriangleShape || shape is TriangleShape2)
                {
                    Selection.Add(shape);
                }
            }
        }

        public void SelectGroups()
        {
            Selection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                if (shape is GroupShape)
                {
                    Selection.Add(shape);
                }
            }
        }

        public void SelectAllShapes()
        {
            Selection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                Selection.Add(shape);
            }
        }

          public void UnselectAllShapes()
        {
            Selection = new List<Shape>();

        }

        public void ReverseSelection()
        {
            List<Shape> reverseSelection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                if (!Selection.Contains(shape))
                {
                    reverseSelection.Add(shape);
                }
            }

            Selection.Clear();
            Selection.AddRange(reverseSelection);
        }

        //КОПИРАНЕ И ПОСТАВЯНЕ

        public void CopyShapes()
        {
            shapeListCopy = new List<Shape>();

            foreach (var shape in Selection)
            {
                shapeListCopy.Add(shape);
            }
        }

        public void PasteShapes()
        {
            foreach (var shape in shapeListCopy)
            {
                int x = (int)shape.Location.X;
                int y = (int)shape.Location.Y;
                int width = (int)shape.Width;
                int height = (int)shape.Height;

                if (shape is GroupShape)
                {
                    CopyGroup((GroupShape)shape);
                }
                else
                {
                    switch (shape.GetType().Name.ToString())
                    {
                        case "RectangleShape":
                            RectangleShape rect = new RectangleShape(new Rectangle(x + 10, y + 10, width, height));
                            rect.FillColor = shape.FillColor;
                            rect.StrokeColor = shape.StrokeColor;
                            rect.Opacity = shape.Opacity;
                            rect.BorderWidth = shape.BorderWidth;
                            ShapeList.Add(rect);
                            break;

                        case "EllipseShape":
                            EllipseShape ellipse = new EllipseShape(new Rectangle(x + 10, y + 10, width, height));
                            ellipse.FillColor = shape.FillColor;
                            ellipse.StrokeColor = shape.StrokeColor;
                            ellipse.Opacity = shape.Opacity;
                            ellipse.BorderWidth = shape.BorderWidth;
                            ShapeList.Add(ellipse);
                            break;

                        case "SquareShape":
                            SquareShape square = new SquareShape(new Rectangle(x + 10, y + 10, width, height));
                            square.FillColor = shape.FillColor;
                            square.StrokeColor = shape.StrokeColor;
                            square.Opacity = shape.Opacity;
                            square.BorderWidth = shape.BorderWidth;
                            ShapeList.Add(square);
                            break;

                        case "CircleShape":
                            CircleShape circle = new CircleShape(new Rectangle(x + 10, y + 10, width, height));
                            circle.FillColor = shape.FillColor;
                            circle.StrokeColor = shape.StrokeColor;
                            circle.Opacity = shape.Opacity;
                            circle.BorderWidth = shape.BorderWidth;
                            ShapeList.Add(circle);
                            break;

                        case "TriangleShape":
                            TriangleShape triangle = new TriangleShape(new Rectangle(x + 10, y + 10, width, height));
                            triangle.FillColor = shape.FillColor;
                            triangle.StrokeColor = shape.StrokeColor;
                            triangle.Opacity = shape.Opacity;
                            triangle.BorderWidth = shape.BorderWidth;
                            ShapeList.Add(triangle);
                            break;

                        case "TriangleShape2":
                            TriangleShape2 triangle2 = new TriangleShape2(new Rectangle(x + 10, y + 10, width, height));
                            triangle2.FillColor = shape.FillColor;
                            triangle2.StrokeColor = shape.StrokeColor;
                            triangle2.Opacity = shape.Opacity;
                            triangle2.BorderWidth = shape.BorderWidth;
                            ShapeList.Add(triangle2);
                            break;
                    }
                }
            }
        }

        public void CopyGroup(GroupShape groupShape)
        {
            List<Shape> newSubShape = new List<Shape>();
            foreach (var shape in groupShape.SubShape)
            {
                int x = (int)shape.Location.X;
                int y = (int)shape.Location.Y;
                int width = (int)shape.Width;
                int height = (int)shape.Height;

                switch (shape.GetType().Name.ToString())
                {
                    case "RectangleShape":
                        RectangleShape rect = new RectangleShape(new Rectangle(x + 10, y + 10, width, height));
                        rect.FillColor = shape.FillColor;
                        rect.StrokeColor = shape.StrokeColor;
                        rect.Opacity = shape.Opacity;
                        rect.BorderWidth = shape.BorderWidth;
                        newSubShape.Add(rect);
                        break;

                    case "EllipseShape":
                        EllipseShape ellipse = new EllipseShape(new Rectangle(x + 10, y + 10, width, height));
                        ellipse.FillColor = shape.FillColor;
                        ellipse.StrokeColor = shape.StrokeColor;
                        ellipse.Opacity = shape.Opacity;
                        ellipse.BorderWidth = shape.BorderWidth;
                        newSubShape.Add(ellipse);
                        break;

                    case "SquareShape":
                        SquareShape square = new SquareShape(new Rectangle(x + 10, y + 10, width, height));
                        square.FillColor = shape.FillColor;
                        square.StrokeColor = shape.StrokeColor;
                        square.Opacity = shape.Opacity;
                        square.BorderWidth = shape.BorderWidth;
                        newSubShape.Add(square);
                        break;

                    case "CircleShape":
                        CircleShape circle = new CircleShape(new Rectangle(x + 10, y + 10, width, height));
                        circle.FillColor = shape.FillColor;
                        circle.StrokeColor = shape.StrokeColor;
                        circle.Opacity = shape.Opacity;
                        circle.BorderWidth = shape.BorderWidth;
                        newSubShape.Add(circle);
                        break;

                    case "TriangleShape":
                        TriangleShape triangle = new TriangleShape(new Rectangle(x + 10, y + 10, width, height));
                        triangle.FillColor = shape.FillColor;
                        triangle.StrokeColor = shape.StrokeColor;
                        triangle.Opacity = shape.Opacity;
                        triangle.BorderWidth = shape.BorderWidth;
                        newSubShape.Add(triangle);
                        break;

                    case "TriangleShap2e":
                        TriangleShape2 triangle2 = new TriangleShape2(new Rectangle(x + 10, y + 10, width, height));
                        triangle2.FillColor = shape.FillColor;
                        triangle2.StrokeColor = shape.StrokeColor;
                        triangle2.Opacity = shape.Opacity;
                        triangle2.BorderWidth = shape.BorderWidth;
                        newSubShape.Add(triangle2);
                        break;
                }
            }

            var gX = (int)groupShape.Location.X;
            var gY = (int)groupShape.Location.Y;
            var gH = (int)groupShape.Height;
            var gW = (int)groupShape.Width;

            GroupShape newGroupShape = new GroupShape(new Rectangle(gX + 10, gY + 10, gW, gH));
            newGroupShape.SubShape = newSubShape;
            ShapeList.Add(newGroupShape);
        }

        //ИЗТРИВАНЕ
        public void DeleteSelectedShapes()
        {
            foreach (var shape in Selection)
            {
                ShapeList.Remove(shape);
            }
            Selection.Clear();
        }

        public void DeleteAll()
        {
            Selection.Clear();
            ShapeList.Clear();
        }

        public List<string> GetNames()
        {
            List <string> names = new List<string>();
            foreach (var shape in ShapeList)
            {
                names.Add(shape.Name.ToString());
            }

            return names;
        }


        public void Rotate(Graphics grfx, float angle)
        {
            
            foreach (var shape in Selection)
            {
            }
        }

        //ЗАПАЗВАНЕ И ЧЕТЕНЕ НА ФЕЙЛОВЕ
        public void SaveFile(List<Shape> shapes, Bitmap bitmap)
        {
            exportFormatDialog.DefaultExt = "pg";
            exportFormatDialog.Title = "Save this file";
            exportFormatDialog.Filter = "PG file (*.pg)|*.pg|All files (*.*)|*.*";


            if (DialogResult.OK == exportFormatDialog.ShowDialog())
            {
                string filePath = exportFormatDialog.FileName;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                //bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);

                FileStream fileStream = new FileStream(filePath, FileMode.CreateNew);

                try
                {
                    formatter.Serialize(fileStream, shapes);

                }
                catch (SerializationException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                fileStream.Close();

            }


        }

        public void ImportFile()
        {
            importFormatDialog.ShowDialog();

            string filePath = importFormatDialog.FileName;

            if(filePath != null && File.Exists(filePath))
            {
                FileStream fileStream = File.Open(filePath, FileMode.Open);

                importedShapes = (List<Shape>)formatter.Deserialize(fileStream);
            }

        }


    }
}
