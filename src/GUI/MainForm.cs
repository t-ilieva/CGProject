using System;
using System.Collections.Generic;
using System.Drawing;
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
                statusBar.Items[0].Text = "Последно действие: Промяна на цвят на границата на селектираните фигури.";
                viewPort.Invalidate();
            }
        }


        //СЕЛЕКЦИЯ
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.DeleteSelectedShapes();
            statusBar.Items[0].Text = "Последно действие: Изтриване на селектираните фигури.";
            viewPort.Invalidate();
        }


        //ГРУПИРАНЕ
        private void GroupButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupSelectedShapes();
            statusBar.Items[0].Text = "Последно действие: Групиране";
            viewPort.Invalidate();
        }

        //ИЗТРИВАНЕ НА ВСИЧКО
        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.DeleteAll();
            statusBar.Items[0].Text = "Последно действие: Изтриване на всичко";
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

        private void selectEllipsesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.SelectEllipses();
            statusBar.Items[0].Text = "Последно действие: Селекция на всички елипси";
            viewPort.Invalidate();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.SelectAllShapes();
            statusBar.Items[0].Text = "Последно действие: Селекция на всички фигури";
            viewPort.Invalidate();
        }

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

        private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.UnselectAllShapes();
            statusBar.Items[0].Text = "Последно действие: Премахване на селекция";
            viewPort.Invalidate();
        }
    }
}


    