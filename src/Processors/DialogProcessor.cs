using System;
using System.Drawing;
using System.Collections.Generic;

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
        }

        #endregion

        #region Properties

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

            ShapeList.Add(rect);
        }

        public void AddRandomElipse()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 100, 200));
            ellipse.FillColor = Color.White;
            ellipse.StrokeColor = Color.Black;

            ShapeList.Add(ellipse);
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
                    shape.FillColor = color;
                }
            }
        }


        //ГРУПИРАНЕ
        public void GroupSelectedShapes()
        {
            if (Selection.Count > 1)
            {
                float minX = float.PositiveInfinity;
                float minY = float.PositiveInfinity;
                float maxX = float.NegativeInfinity;
                float maxY = float.NegativeInfinity;
                foreach (var shape in Selection)
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
                group.SubShape = Selection;

                foreach (var shape1 in Selection)
                {
                    ShapeList.Remove(shape1);
                }

                Selection = new List<Shape>();
                Selection.Add(group);
                ShapeList.Add(group);
            }
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

        public void SelectAllShapes()
        {
            Selection = new List<Shape>();

            foreach (var shape in ShapeList)
            {
                Selection.Add(shape);
            }
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
                            ShapeList.Add(rect);
                            break;

                        case "EllipseShape":
                            EllipseShape ellipse = new EllipseShape(new Rectangle(x + 10, y + 10, width, height));
                            ellipse.FillColor = shape.FillColor;
                            ellipse.StrokeColor = shape.StrokeColor;
                            ShapeList.Add(ellipse);
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
                        newSubShape.Add(rect);
                        break;

                    case "EllipseShape":
                        EllipseShape ellipse = new EllipseShape(new Rectangle(x + 10, y + 10, width, height));
                        ellipse.FillColor = shape.FillColor;
                        ellipse.StrokeColor = shape.StrokeColor;
                        newSubShape.Add(ellipse);
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
    }
}
