using System;
using System.Drawing;

namespace Draw
{
	/// <summary>
	/// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
	/// </summary>
	/// 

	[Serializable]
	public class TriangleShape : Shape
	{
		#region Constructor

		public TriangleShape(RectangleF rect) : base(rect)
		{
		}

		public TriangleShape(RectangleShape rectangle) : base(rectangle)
		{
		}

		#endregion

		/// <summary>
		/// Проверка за принадлежност на точка point към правоъгълника.
		/// В случая на правоъгълник този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		public override bool Contains(PointF point)
		{

			PointF a = new PointF((int)Rectangle.X + (int)Rectangle.Width / 2, (int)Rectangle.Y);
			PointF b = new PointF((int)Rectangle.X, (int)Rectangle.Y + (int)Rectangle.Height);
			PointF c = new PointF((int)Rectangle.X + (int)Rectangle.Width, (int)Rectangle.Y + (int)Rectangle.Height);

			float area = triangleArea(a, b, c);
			float area1 = triangleArea(point, b, c);        
			float area2 = triangleArea(a, point, c);        
			float area3 = triangleArea(a, b, point);

			bool check = true;

			if(area != (area1 + area2 + area3))
            {
				check = false;
            }

			if (base.Contains(point) && check)
				// Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
				// В случая на правоъгълник - директно връщаме true
				return true;
			else
				// Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
				return false;
		}

		private float triangleArea(PointF p1, PointF p2, PointF p3)
		{
			return (float)Math.Abs((p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y)) / 2.0);
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{

			base.DrawSelf(grfx);

			Point a = new Point((int)Rectangle.X + (int)Rectangle.Width / 2, (int)Rectangle.Y);
			Point b = new Point((int)Rectangle.X, (int)Rectangle.Y + (int)Rectangle.Height);
			Point c = new Point((int)Rectangle.X + (int)Rectangle.Width, (int)Rectangle.Y + (int)Rectangle.Height);
			Point[] points = { a, b, c };

			grfx.FillPolygon(new SolidBrush(Color.FromArgb(Opacity, FillColor)), points);
			grfx.DrawPolygon(new Pen(StrokeColor, BorderWidth), points);
			grfx.ResetTransform();
		}
	}
}
