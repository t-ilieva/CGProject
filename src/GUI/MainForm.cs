using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Draw
{
    /// <summary>
    /// Върху главната форма е поставен потребителски контрол,
    /// в който се осъществява визуализацията
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
        /// </summary>
        private DialogProcessor dialogProcessor = new DialogProcessor();

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        /// <summary>
        /// Изход от програмата. Затваря главната форма, а с това и програмата.
        /// </summary>
        void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
        /// </summary>
        void ViewPortPaint(object sender, PaintEventArgs e)
        {
            dialogProcessor.ReDraw(sender, e);
        }

        /// <summary>
        /// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
        /// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
        /// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
        /// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
        /// </summary>
        void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (SelectButton.Checked)
            {

                var select = dialogProcessor.ContainsPoint(e.Location);
                if (select != null)
                {
                    if (dialogProcessor.Selection.Contains(select))
                    {
                        dialogProcessor.Selection.Remove(select);
                    }
                    else
                    {
                        dialogProcessor.Selection.Add(select);
                    }
                }


                statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
                dialogProcessor.IsDragging = true;
                dialogProcessor.LastLocation = e.Location;
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прихващане на преместването на мишката.
        /// Ако сме в режм на "влачене", то избрания елемент се транслира.
        /// </summary>

        void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (dialogProcessor.IsDragging)
            {
                if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
                dialogProcessor.TranslateTo(e.Location);
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прихващане на отпускането на бутона на мишката.
        /// Излизаме от режим "влачене".
        /// </summary>
        void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            dialogProcessor.IsDragging = false;
        }
        private void pickUpSpeedButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Бутон, който поставя на произволно място правоъгълник със зададените размери.
        /// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
        /// </summary>
        /// 

        //ФИГУРИ
        void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomRectangle();
            statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
            viewPort.Invalidate();
        }

        void DrawSquareSpeedButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomSquare();
            statusBar.Items[0].Text = "Последно действие: Рисуване на квадрат";
            viewPort.Invalidate();
        }

        void DrawEllipseSpeedButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomElipse();
            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";
            viewPort.Invalidate();
        }

        void SelectFillColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SelectFillColor(colorDialog1.Color);
                statusBar.Items[0].Text = "Последно действие: Промяна на цвят на запълване на селектираните фигури.";
                viewPort.Invalidate();
            }
        }

        void SelectBorderColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SelectBorderColor(colorDialog1.Color);
                statusBar.Items[0].Text = "Последно действие: Промяна на цвят на граница на селектираните фигури.";
                viewPort.Invalidate();
            }
        }

        //ГРУПИРАНЕ
        private void GroupButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupSelectedShapes(dialogProcessor.Selection);
            statusBar.Items[0].Text = "Последно действие: Групиране";
            viewPort.Invalidate();
        }

        private void ungroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.UngroupShape();
            statusBar.Items[0].Text = "Последно действие: Разгрупиране";
            viewPort.Invalidate();
        }

        private void removeRectanglesFromGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupRemoveShapes("RectangleShape");
            statusBar.Items[0].Text = "Последно действие: Премахване на правоъгълници от селектираните групи";
            viewPort.Invalidate();
        }
        private void removeEllipsesFromGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupRemoveShapes("EllipseShape");
            statusBar.Items[0].Text = "Последно действие: Премахване на елипси от селектираните групи";
            viewPort.Invalidate();
        }

        private void removeSquaresFromGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupRemoveShapes("SquareShape");
            statusBar.Items[0].Text = "Последно действие: Премахване на квадрати от селектираните групи";
            viewPort.Invalidate();
        }


        //СЕЛЕКЦИЯ
        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void speedMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void selectRectanglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.SelectRectangles();
            statusBar.Items[0].Text = "Последно действие: Селекция на всички правоъгълници";
            viewPort.Invalidate();
        }

        private void selectSquaresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.SelectSquares();
            statusBar.Items[0].Text = "Последно действие: Селекция на всички квадрати";
            viewPort.Invalidate();
        }

        private void selectEllipsesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.SelectEllipses();
            statusBar.Items[0].Text = "Последно действие: Селекция на всички елипси";
            viewPort.Invalidate();
        }

        private void selectGroupsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dialogProcessor.SelectGroups();
            statusBar.Items[0].Text = "Последно действие: Селекция на всички групи";
            viewPort.Invalidate();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.SelectAllShapes();
            statusBar.Items[0].Text = "Последно действие: Селекция на всички фигури";
            viewPort.Invalidate();
        }

        private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.UnselectAllShapes();
            statusBar.Items[0].Text = "Последно действие: Премахване на селекция";
            viewPort.Invalidate();
        }

        private void reverseSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.ReverseSelection();
            statusBar.Items[0].Text = "Последно действие: Обръщане на селекция";
            viewPort.Invalidate();
        }

        //КОПИРАНЕ И ПОСТАВЯНЕ
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.CopyShapes();
            statusBar.Items[0].Text = "Последно действие: Копиране на селектирани фигури.";
            viewPort.Invalidate();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.PasteShapes();
            statusBar.Items[0].Text = "Последно действие: Поставяне на копираните фигури.";
            viewPort.Invalidate();
        }

        //ИЗТРИВАНЕ
        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.DeleteAll();
            statusBar.Items[0].Text = "Последно действие: Изтриване на всичко";
            viewPort.Invalidate();
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.DeleteSelectedShapes();
            statusBar.Items[0].Text = "Последно действие: Изтриване на селектираните фигури.";
            viewPort.Invalidate();
        }

        private void shapeNameComboBox_DropDown(object sender, EventArgs e)
        {
            nameComboBox.Items.Clear();
            foreach (var shape in dialogProcessor.ShapeList)
            {
                string name = shape.Name;

                if (name != "")
                {
                    nameComboBox.Items.Add(name);
                }
            }
            viewPort.Invalidate();
        }

        //ИМЕНУВАНЕ И ТЪРСЕНЕ ПО ИМЕ НА ФИГУРА
        private void enterNameButton_Click(object sender, EventArgs e)
        {

            foreach (var shape in dialogProcessor.Selection)
            {
                shape.Name = "";
                string name = nameTextBox.Text;
                List<string> names = dialogProcessor.GetNames();
                if (names.Contains(name))
                {
                    int counter = names.Where(x => x.StartsWith(name)).Count();
                    shape.Name = name + " (" + counter + ")";
                }
                else
                {
                    shape.Name = name;
                }

            }
            viewPort.Invalidate();
        }

        private void nameComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            string search = nameComboBox.Text;
            nameComboBox.SelectedItem = null;

            if (e.KeyChar == (char)Keys.Enter)
            {
                int index = nameComboBox.FindString(search);
                if (index != -1)
                {
                    nameComboBox.SelectedItem = nameComboBox.Items[index];
                }
            }
            viewPort.Invalidate();
        }

        private void nameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            dialogProcessor.Selection.Clear();
            if (nameComboBox.SelectedItem != null)
            {
                string name = nameComboBox.SelectedItem.ToString();

                foreach (var shape in dialogProcessor.ShapeList)
                {
                    if (shape.Name.Equals(name))
                    {

                        dialogProcessor.Selection.Add(shape);
                    }
                }
            }
            viewPort.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (dialogProcessor.Selection.Count > 0)
            {
                dialogProcessor.SelectBorderWidth(((int)trackBar1.Value));
                statusBar.Items[0].Text = "Последно действие: Промяна на дебелина на линия";
                viewPort.Invalidate();
            }
        }

        private void opacityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int opacity = 255;
            switch (opacityComboBox.SelectedIndex)
            {
                case 0:
                    opacity = 0;
                    break;
                case 1:
                    opacity = 25;
                    break;
                case 2:
                    opacity = 55;
                    break;
                case 3:
                    opacity = 90;
                    break;
                case 4:
                    opacity = 105;
                    break;
                case 5:
                    opacity = 130;
                    break;
                case 6:
                    opacity = 155;
                    break;
                case 7:
                    opacity = 180;
                    break;
                case 8:
                    opacity =205;
                    break;
                case 9:
                    opacity = 230;
                    break;
                case 10:
                    opacity = 255;
                    break;
            }

            dialogProcessor.SelectOpacity(opacity);
            statusBar.Items[0].Text = "Последно действие: Промяна на прозрачност";
            viewPort.Invalidate();
        }


        //ЗАПАЗВАНЕ И ИМПОРТИРАНЕ НА ФАЙЛ
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(viewPort.Width, viewPort.Height);
            viewPort.DrawToBitmap(bm, new Rectangle(0, 0, bm.Width, bm.Height));
            dialogProcessor.SaveFile(dialogProcessor.ShapeList, bm);
            statusBar.Items[0].Text = "Последно действие: Запазване на файл";
            viewPort.Invalidate();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.ImportFile();
            dialogProcessor.ShapeList = dialogProcessor.importedShapes;
            statusBar.Items[0].Text = "Последно действие: Импортиране на файл";
            viewPort.Invalidate();
        }
    }
}


