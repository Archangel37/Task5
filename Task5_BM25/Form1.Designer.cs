namespace Task5_BM25
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Evaluate = new System.Windows.Forms.Button();
            this.textBox_search_string = new System.Windows.Forms.TextBox();
            this.richTextBox_result = new System.Windows.Forms.RichTextBox();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.button_source = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Evaluate
            // 
            this.Evaluate.Location = new System.Drawing.Point(603, 67);
            this.Evaluate.Name = "Evaluate";
            this.Evaluate.Size = new System.Drawing.Size(141, 23);
            this.Evaluate.TabIndex = 0;
            this.Evaluate.Text = "Help me to choose";
            this.Evaluate.UseVisualStyleBackColor = true;
            this.Evaluate.Click += new System.EventHandler(this.Evaluate_Click);
            // 
            // textBox_search_string
            // 
            this.textBox_search_string.Location = new System.Drawing.Point(40, 70);
            this.textBox_search_string.Name = "textBox_search_string";
            this.textBox_search_string.Size = new System.Drawing.Size(543, 20);
            this.textBox_search_string.TabIndex = 1;
            // 
            // richTextBox_result
            // 
            this.richTextBox_result.Location = new System.Drawing.Point(40, 117);
            this.richTextBox_result.Name = "richTextBox_result";
            this.richTextBox_result.Size = new System.Drawing.Size(633, 323);
            this.richTextBox_result.TabIndex = 2;
            this.richTextBox_result.Text = "";
            // 
            // textBox_path
            // 
            this.textBox_path.Location = new System.Drawing.Point(40, 13);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(414, 20);
            this.textBox_path.TabIndex = 3;
            // 
            // button_source
            // 
            this.button_source.Location = new System.Drawing.Point(460, 11);
            this.button_source.Name = "button_source";
            this.button_source.Size = new System.Drawing.Size(123, 23);
            this.button_source.TabIndex = 4;
            this.button_source.Text = "Source Folder";
            this.button_source.UseVisualStyleBackColor = true;
            this.button_source.Click += new System.EventHandler(this.button_source_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 527);
            this.Controls.Add(this.button_source);
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.richTextBox_result);
            this.Controls.Add(this.textBox_search_string);
            this.Controls.Add(this.Evaluate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Evaluate;
        private System.Windows.Forms.TextBox textBox_search_string;
        private System.Windows.Forms.RichTextBox richTextBox_result;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Button button_source;
    }
}

