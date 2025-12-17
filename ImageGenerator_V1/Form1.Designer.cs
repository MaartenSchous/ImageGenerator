namespace ImageGenerator_V1
{
    partial class Form1
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
            buttonRunPause = new Button();
            labelRunPause = new Label();
            labelWebhostQueue = new Label();
            labelLocalQueue = new Label();
            labelWebhostEvaluationQueue = new Label();
            labelLocalEvaluationQueue = new Label();
            comboBoxTier = new ComboBox();
            textBoxPrompt = new TextBox();
            buttonOpenThisQueue = new Button();
            pictureBox1 = new PictureBox();
            dataGridView1 = new DataGridView();
            textBoxOrigin = new TextBox();
            buttonApproveforgeneration = new Button();
            buttonUploadToWebhost = new Button();
            buttonDiscardTicket = new Button();
            buttonQueueAgainNS = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // buttonRunPause
            // 
            buttonRunPause.Location = new Point(12, 24);
            buttonRunPause.Name = "buttonRunPause";
            buttonRunPause.Size = new Size(113, 49);
            buttonRunPause.TabIndex = 0;
            buttonRunPause.Text = "Generator Ready";
            buttonRunPause.UseVisualStyleBackColor = true;
            buttonRunPause.Click += buttonRunPause_Click;
            // 
            // labelRunPause
            // 
            labelRunPause.AutoSize = true;
            labelRunPause.Location = new Point(143, 41);
            labelRunPause.Name = "labelRunPause";
            labelRunPause.Size = new Size(73, 15);
            labelRunPause.TabIndex = 1;
            labelRunPause.Text = "Server status";
            // 
            // labelWebhostQueue
            // 
            labelWebhostQueue.AutoSize = true;
            labelWebhostQueue.Location = new Point(314, 24);
            labelWebhostQueue.Name = "labelWebhostQueue";
            labelWebhostQueue.Size = new Size(92, 15);
            labelWebhostQueue.TabIndex = 2;
            labelWebhostQueue.Text = "Webhost Queue";
            // 
            // labelLocalQueue
            // 
            labelLocalQueue.AutoSize = true;
            labelLocalQueue.Location = new Point(529, 24);
            labelLocalQueue.Name = "labelLocalQueue";
            labelLocalQueue.Size = new Size(73, 15);
            labelLocalQueue.TabIndex = 3;
            labelLocalQueue.Text = "Local Queue";
            // 
            // labelWebhostEvaluationQueue
            // 
            labelWebhostEvaluationQueue.AutoSize = true;
            labelWebhostEvaluationQueue.Location = new Point(256, 51);
            labelWebhostEvaluationQueue.Name = "labelWebhostEvaluationQueue";
            labelWebhostEvaluationQueue.Size = new Size(150, 15);
            labelWebhostEvaluationQueue.TabIndex = 4;
            labelWebhostEvaluationQueue.Text = "Webhost Evaluation Queue";
            // 
            // labelLocalEvaluationQueue
            // 
            labelLocalEvaluationQueue.AutoSize = true;
            labelLocalEvaluationQueue.Location = new Point(471, 51);
            labelLocalEvaluationQueue.Name = "labelLocalEvaluationQueue";
            labelLocalEvaluationQueue.Size = new Size(131, 15);
            labelLocalEvaluationQueue.TabIndex = 5;
            labelLocalEvaluationQueue.Text = "Local Evaluation Queue";
            // 
            // comboBoxTier
            // 
            comboBoxTier.FormattingEnabled = true;
            comboBoxTier.Location = new Point(12, 229);
            comboBoxTier.Name = "comboBoxTier";
            comboBoxTier.Size = new Size(121, 23);
            comboBoxTier.TabIndex = 6;
            // 
            // textBoxPrompt
            // 
            textBoxPrompt.Location = new Point(152, 229);
            textBoxPrompt.Multiline = true;
            textBoxPrompt.Name = "textBoxPrompt";
            textBoxPrompt.Size = new Size(235, 163);
            textBoxPrompt.TabIndex = 7;
            textBoxPrompt.Text = "A bouquet of flowers eating a bee. Watercolor. Vibrant colors, high detail.";
            // 
            // buttonOpenThisQueue
            // 
            buttonOpenThisQueue.Location = new Point(12, 200);
            buttonOpenThisQueue.Name = "buttonOpenThisQueue";
            buttonOpenThisQueue.Size = new Size(121, 23);
            buttonOpenThisQueue.TabIndex = 8;
            buttonOpenThisQueue.Text = "Open this Queue";
            buttonOpenThisQueue.UseVisualStyleBackColor = true;
            buttonOpenThisQueue.Click += buttonOpenThisQueue_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(406, 229);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(276, 339);
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 258);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(121, 310);
            dataGridView1.TabIndex = 10;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // textBoxOrigin
            // 
            textBoxOrigin.Location = new Point(152, 200);
            textBoxOrigin.Name = "textBoxOrigin";
            textBoxOrigin.RightToLeft = RightToLeft.No;
            textBoxOrigin.Size = new Size(530, 23);
            textBoxOrigin.TabIndex = 11;
            textBoxOrigin.Text = "From: Webhost";
            // 
            // buttonApproveforgeneration
            // 
            buttonApproveforgeneration.Location = new Point(152, 170);
            buttonApproveforgeneration.Name = "buttonApproveforgeneration";
            buttonApproveforgeneration.Size = new Size(138, 24);
            buttonApproveforgeneration.TabIndex = 12;
            buttonApproveforgeneration.Text = "Approve for generation";
            buttonApproveforgeneration.UseVisualStyleBackColor = true;
            buttonApproveforgeneration.Click += buttonApproveforgeneration_Click;
            // 
            // buttonUploadToWebhost
            // 
            buttonUploadToWebhost.Location = new Point(544, 169);
            buttonUploadToWebhost.Name = "buttonUploadToWebhost";
            buttonUploadToWebhost.Size = new Size(138, 24);
            buttonUploadToWebhost.TabIndex = 13;
            buttonUploadToWebhost.Text = "UploadToWebhost";
            buttonUploadToWebhost.UseVisualStyleBackColor = true;
            buttonUploadToWebhost.Click += buttonUploadToWebhost_Click;
            // 
            // buttonDiscardTicket
            // 
            buttonDiscardTicket.Location = new Point(296, 170);
            buttonDiscardTicket.Name = "buttonDiscardTicket";
            buttonDiscardTicket.Size = new Size(105, 23);
            buttonDiscardTicket.TabIndex = 14;
            buttonDiscardTicket.Text = "Discard ticket";
            buttonDiscardTicket.UseVisualStyleBackColor = true;
            buttonDiscardTicket.Click += buttonDiscardTicket_Click;
            // 
            // buttonQueueAgainNS
            // 
            buttonQueueAgainNS.Location = new Point(425, 170);
            buttonQueueAgainNS.Name = "buttonQueueAgainNS";
            buttonQueueAgainNS.Size = new Size(113, 23);
            buttonQueueAgainNS.TabIndex = 15;
            buttonQueueAgainNS.Text = "Repeat generation";
            buttonQueueAgainNS.UseVisualStyleBackColor = true;
            buttonQueueAgainNS.Click += buttonQueueAgainNS_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(694, 580);
            Controls.Add(buttonQueueAgainNS);
            Controls.Add(buttonDiscardTicket);
            Controls.Add(buttonUploadToWebhost);
            Controls.Add(buttonApproveforgeneration);
            Controls.Add(textBoxOrigin);
            Controls.Add(dataGridView1);
            Controls.Add(pictureBox1);
            Controls.Add(buttonOpenThisQueue);
            Controls.Add(textBoxPrompt);
            Controls.Add(comboBoxTier);
            Controls.Add(labelLocalEvaluationQueue);
            Controls.Add(labelWebhostEvaluationQueue);
            Controls.Add(labelLocalQueue);
            Controls.Add(labelWebhostQueue);
            Controls.Add(labelRunPause);
            Controls.Add(buttonRunPause);
            Name = "Form1";
            Text = "Image Generator";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonRunPause;
        private Label labelRunPause;
        private Label labelWebhostQueue;
        private Label labelLocalQueue;
        private Label labelWebhostEvaluationQueue;
        private Label labelLocalEvaluationQueue;
        private ComboBox comboBoxTier;
        private TextBox textBoxPrompt;
        private Button buttonOpenThisQueue;
        private PictureBox pictureBox1;
        private DataGridView dataGridView1;
        private TextBox textBoxOrigin;
        private Button buttonApproveforgeneration;
        private Button buttonUploadToWebhost;
        private Button buttonDiscardTicket;
        private Button buttonQueueAgainNS;
    }
}
