using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
	/// <summary>
	/// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
	/// </summary>
	/// 

	[Serializable]
	public class GroupShape : Shape
	{
		#region Constructor

		public GroupShape(RectangleF rect) : base(rect)
		{
		}

		public GroupShape(RectangleShape rectangle) : base(rectangle)
		{
		}


		#endregion

		public List<Shape> SubShape = new List<Shape>();

		/// <summary>
		/// Проверка за принадлежност на точка point към правоъгълника.
		/// В случая на правоъгълник този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		public override bool Contains(PointF point)
		{
			foreach (var shape in SubShape)
			{
				if (shape.Contains(point))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);

			foreach (var shape in SubShape)
			{
				shape.DrawSelf(grfx);
			}
		}

		public override void GroupTranslateTo(float x, float y)
		{
			base.GroupTranslateTo(x, y);

			foreach (var shape in SubShape)
			{
				shape.Location = new PointF(shape.Location.X + x, shape.Location.Y + y);
			}
		}

		public override void GroupFillColor(Color color)
		{
			base.GroupFillColor(color);
			foreach (var shape in SubShape)
			{
				shape.FillColor = color;
			}
		}

		public override void GroupBorderColor(Color color)
		{
			base.GroupBorderColor(color);
			foreach (var shape in SubShape)
			{
				shape.StrokeColor = color;
			}
		}

        public override void GroupBorderWidth(int borderWidth)
        {
            base.GroupBorderWidth(borderWidth);
			foreach (var shape in SubShape)
			{
				shape.BorderWidth = borderWidth;
			}
		}

        public override void GroupOpacity(int opacity)
        {
            base.GroupOpacity(opacity);
			foreach (var shape in SubShape)
			{
				shape.Opacity = opacity;
			}
		}

        public override void GroupRotate(float angle)
        {
        }
    }
}
