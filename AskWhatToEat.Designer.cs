namespace AskWhatToEat
{
    partial class FormAsk
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAsk));
            ask = new Button();
            Mystia_Izakaya = new PictureBox();
            foodPicture = new PictureBox();
            drinkPicture = new PictureBox();
            foodName = new TextBox();
            foodPrice = new TextBox();
            drinkPrice = new TextBox();
            drinkName = new TextBox();
            totalPrice = new TextBox();
            ((System.ComponentModel.ISupportInitialize)Mystia_Izakaya).BeginInit();
            ((System.ComponentModel.ISupportInitialize)foodPicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)drinkPicture).BeginInit();
            SuspendLayout();
            // 
            // ask
            // 
            ask.Location = new Point(59, 265);
            ask.Name = "ask";
            ask.Size = new Size(110, 29);
            ask.TabIndex = 0;
            ask.Text = "ask Mystia";
            ask.UseVisualStyleBackColor = true;
            ask.Click += ask_Click;
            // 
            // Mystia_Izakaya
            // 
            Mystia_Izakaya.Image = Properties.Resources._00_01;
            Mystia_Izakaya.Location = new Point(12, 12);
            Mystia_Izakaya.Name = "Mystia_Izakaya";
            Mystia_Izakaya.Size = new Size(184, 184);
            Mystia_Izakaya.TabIndex = 1;
            Mystia_Izakaya.TabStop = false;
            // 
            // foodPicture
            // 
            foodPicture.Location = new Point(238, 12);
            foodPicture.Name = "foodPicture";
            foodPicture.Size = new Size(156, 156);
            foodPicture.TabIndex = 2;
            foodPicture.TabStop = false;
            // 
            // drinkPicture
            // 
            drinkPicture.Location = new Point(414, 12);
            drinkPicture.Name = "drinkPicture";
            drinkPicture.Size = new Size(156, 156);
            drinkPicture.TabIndex = 3;
            drinkPicture.TabStop = false;
            // 
            // foodName
            // 
            foodName.Location = new Point(238, 223);
            foodName.Name = "foodName";
            foodName.ReadOnly = true;
            foodName.Size = new Size(156, 27);
            foodName.TabIndex = 4;
            foodName.TextAlign = HorizontalAlignment.Center;
            // 
            // foodPrice
            // 
            foodPrice.Location = new Point(238, 267);
            foodPrice.Name = "foodPrice";
            foodPrice.ReadOnly = true;
            foodPrice.Size = new Size(156, 27);
            foodPrice.TabIndex = 5;
            foodPrice.TextAlign = HorizontalAlignment.Center;
            // 
            // drinkPrice
            // 
            drinkPrice.Location = new Point(414, 267);
            drinkPrice.Name = "drinkPrice";
            drinkPrice.ReadOnly = true;
            drinkPrice.Size = new Size(156, 27);
            drinkPrice.TabIndex = 7;
            drinkPrice.TextAlign = HorizontalAlignment.Center;
            // 
            // drinkName
            // 
            drinkName.Location = new Point(414, 223);
            drinkName.Name = "drinkName";
            drinkName.ReadOnly = true;
            drinkName.Size = new Size(156, 27);
            drinkName.TabIndex = 6;
            drinkName.TextAlign = HorizontalAlignment.Center;
            // 
            // totalPrice
            // 
            totalPrice.Location = new Point(596, 267);
            totalPrice.Name = "totalPrice";
            totalPrice.ReadOnly = true;
            totalPrice.Size = new Size(156, 27);
            totalPrice.TabIndex = 8;
            totalPrice.TextAlign = HorizontalAlignment.Center;
            // 
            // FormAsk
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(totalPrice);
            Controls.Add(drinkPrice);
            Controls.Add(drinkName);
            Controls.Add(foodPrice);
            Controls.Add(foodName);
            Controls.Add(drinkPicture);
            Controls.Add(foodPicture);
            Controls.Add(Mystia_Izakaya);
            Controls.Add(ask);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormAsk";
            Text = "What to eat today?";
            Load += FormAsk_Load;
            ((System.ComponentModel.ISupportInitialize)Mystia_Izakaya).EndInit();
            ((System.ComponentModel.ISupportInitialize)foodPicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)drinkPicture).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ask;
        private PictureBox Mystia_Izakaya;
        private PictureBox foodPicture;
        private PictureBox drinkPicture;
        private TextBox foodName;
        private TextBox foodPrice;
        private TextBox drinkPrice;
        private TextBox drinkName;
        private TextBox totalPrice;
    }
}
