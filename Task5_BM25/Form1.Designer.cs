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
            this.checkBox_TxtOrUrl = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Evaluate
            // 
            this.Evaluate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Evaluate.Location = new System.Drawing.Point(589, 80);
            this.Evaluate.Name = "Evaluate";
            this.Evaluate.Size = new System.Drawing.Size(172, 23);
            this.Evaluate.TabIndex = 5;
            this.Evaluate.Text = "Help me to choose";
            this.Evaluate.UseVisualStyleBackColor = true;
            this.Evaluate.Click += new System.EventHandler(this.Evaluate_Click);
            // 
            // textBox_search_string
            // 
            this.textBox_search_string.Location = new System.Drawing.Point(40, 82);
            this.textBox_search_string.Name = "textBox_search_string";
            this.textBox_search_string.Size = new System.Drawing.Size(543, 20);
            this.textBox_search_string.TabIndex = 4;
            // 
            // richTextBox_result
            // 
            this.richTextBox_result.Location = new System.Drawing.Point(40, 122);
            this.richTextBox_result.Name = "richTextBox_result";
            this.richTextBox_result.Size = new System.Drawing.Size(721, 503);
            this.richTextBox_result.TabIndex = 6;
            this.richTextBox_result.Text = "";
            // 
            // textBox_path
            // 
            this.textBox_path.Location = new System.Drawing.Point(40, 42);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(414, 20);
            this.textBox_path.TabIndex = 2;
            // 
            // button_source
            // 
            this.button_source.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_source.Location = new System.Drawing.Point(460, 40);
            this.button_source.Name = "button_source";
            this.button_source.Size = new System.Drawing.Size(123, 23);
            this.button_source.TabIndex = 3;
            this.button_source.Text = "Source Folder";
            this.button_source.UseVisualStyleBackColor = true;
            this.button_source.Click += new System.EventHandler(this.button_source_Click);
            // 
            // checkBox_TxtOrUrl
            // 
            this.checkBox_TxtOrUrl.AutoSize = true;
            this.checkBox_TxtOrUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox_TxtOrUrl.Location = new System.Drawing.Point(40, 13);
            this.checkBox_TxtOrUrl.Name = "checkBox_TxtOrUrl";
            this.checkBox_TxtOrUrl.Size = new System.Drawing.Size(183, 20);
            this.checkBox_TxtOrUrl.TabIndex = 1;
            this.checkBox_TxtOrUrl.Text = "Use http://bash.im/rss/";
            this.checkBox_TxtOrUrl.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 637);
            this.Controls.Add(this.checkBox_TxtOrUrl);
            this.Controls.Add(this.button_source);
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.richTextBox_result);
            this.Controls.Add(this.textBox_search_string);
            this.Controls.Add(this.Evaluate);
            this.Name = "Form1";
            this.Text = "What to read helper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Evaluate;
        private System.Windows.Forms.TextBox textBox_search_string;
        private System.Windows.Forms.RichTextBox richTextBox_result;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Button button_source;
        private System.Windows.Forms.CheckBox checkBox_TxtOrUrl;
    }
}

