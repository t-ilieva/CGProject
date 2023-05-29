using System;
using System.Drawing;

namespace Draw
{
	/// <summary>
	/// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
	/// </summary>
	/// 

	[Serializable]
	public class CircleShape : Shape
	{
		#region Constructor

		public CircleShape(RectangleF rect) : base(rect)
		{
		}

		public CircleShape(RectangleShape rectangle) : base(rectangle)
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
			{
				var h = base.Location.X + (base.Width / 2);
				var k = base.Location.Y + (base.Height / 2);
				var r = base.Width / 2;
				var x = point.X;
				var y = point.Y;

				var x1 = Math.Pow((x - h), 2);
				var y1 = Math.Pow((y - k), 2);
				var r2 = Math.Pow(r, 2);

				var result = x1 + y1;

				if (base.Contains(point) && result < r2)
					return true;
				else
					return false;
			}
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);

			grfx.FillEllipse(new SolidBrush(Color.FromArgb(Opacity, FillColor)), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
			grfx.DrawEllipse(new Pen(StrokeColor, BorderWidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
			grfx.ResetTransform();
		}
	}
}
