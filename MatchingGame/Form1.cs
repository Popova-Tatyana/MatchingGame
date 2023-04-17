using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // Используется случайный объект, чтобы выбрать случайные значки для квадратов
        Random random = new Random();

        // Каждая из этих букв представляет собой значок
        // в шрифте Webdings,
        // и каждый значок появляется в этом списке дважды
        List<string> icons = new List<string>()
        {
             "!", "!", "N", "N", ",", ",", "k", "k",
             "b", "b", "v", "v", "w", "w", "z", "z"
        };

        /// <summary>
        /// Назначается каждому значку из списка значков случайный квадрат
        /// </summary>
        private void AssignIconsToSquares()
        {
            // Панель TableLayoutPanel имеет 16 меток,
            // и в списке значков 16 значков,
            // таким образом, значок выбирается случайным образом из списка
            // и добавляется к каждой метке
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }
        public Form1()
        {
            InitializeComponent();

            AssignIconsToSquares();
        }

        /// <summary>
        /// Щелчок по каждой метке обрабатывается этим обработчиком событий
        /// </summary>
        /// <param name="sender">Метка, по которой был произведен щелчок</param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            // Таймер включается только после того,
            // как игроку будут показаны два несоответствующих значка,
            // поэтому игнорируются любые щелчки, если таймер запущен
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // Если выбранная метка черная, игрок нажал
                // на значок, который уже был показан --
                // игнорируется щелчок
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                // Если значение firstClicked равно null, это первый значок
                // в паре, на который нажал игрок,
                // итак, установите первый щелчок на метку, по которой игрок
                // нажал, измените ее цвет на черный и верните
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                // Если игрок получает это, значит время не истекло
                // и firstClicked не равен нулю,
                // так что это должен быть второй значок, на который игрок нажал,
                // кнопку Установить его цвет на черный
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                // Проверяется, выиграл ли игрок
                CheckForWinner();

                // Если игрок нажал на две одинаковые иконки, они остаются
                // черными и сбрасывается firstClicked и secondClicked 
                // таким образом, игрок может нажать на другой значок
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // Если игрок получает это, значит он
                // нажал на две разные иконки, поэтому запустится
                // таймер (который подождет 750 миллисекунд
                // а затем скроет икноки)
                timer1.Start();
            }
        }

        // firstClicked указывает на первый элемент управления Label
        // на который нажимает игрок, но он будет равен null 
        // если игрок еще не нажал на метку
        Label firstClicked = null;

        // secondClicked указывает на второй элемент управления Label
        // на который нажимает игрок
        Label secondClicked = null;

        /// <summary>
        /// Этот таймер запускается, когда игрок нажимает
        /// два несовпадающих значка,
        /// так что отсчет идет 750 миллисекунды
        /// а затем выключается сам по себе и скрывает оба значка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Останавливает таймер, вызывая метод Stop()
            timer1.Stop();

            // Скрывает оба значка
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Сбрасывает firstClicked и secondClicked на null
            // чтобы при следующем нажатии на метку 
            // программа знала, что это первый щелчок
            firstClicked = null;
            secondClicked = null;
        }

        /// <summary>
        /// Проверяется каждый значок, чтобы убедиться, что он соответствует,
        /// сравнив его цвет переднего плана с цветом фона.
        /// Если все значки совпадут, игрок выигрывает
        /// </summary>
        private void CheckForWinner()
        {
            // Просматриваются все метки на панели TableLayoutPanel, 
            // проверяя каждую из них, чтобы увидеть, соответствует ли ее значок
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            // Если цикл не возвращается, значит он не находит
            // никаких несопоставимых значков 
            // Это означает, что пользователь выиграл. Показывается диалоговое окно и закрывается форма
            MessageBox.Show("Вы подобрали все иконки!", "Поздравляем");
            Close();
        }
    }
}
