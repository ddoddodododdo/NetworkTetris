
namespace Tetris201770001
{
    partial class NetworkTetris
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.downTimer = new System.Windows.Forms.Timer(this.components);
            this.scoreLabel = new System.Windows.Forms.Label();
            this.oclock = new System.Windows.Forms.Timer(this.components);
            this.onServerButton = new System.Windows.Forms.Button();
            this.clientButton = new System.Windows.Forms.Button();
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // downTimer
            // 
            this.downTimer.Enabled = true;
            this.downTimer.Interval = 1000;
            this.downTimer.Tick += new System.EventHandler(this.downTimer_Tick);
            // 
            // scoreLabel
            // 
            this.scoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.scoreLabel.Location = new System.Drawing.Point(95, 203);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.scoreLabel.Size = new System.Drawing.Size(124, 38);
            this.scoreLabel.TabIndex = 0;
            this.scoreLabel.Text = "label1";
            this.scoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // oclock
            // 
            this.oclock.Enabled = true;
            this.oclock.Interval = 1000;
            this.oclock.Tick += new System.EventHandler(this.oclock_Tick);
            // 
            // onServerButton
            // 
            this.onServerButton.Location = new System.Drawing.Point(21, 374);
            this.onServerButton.Name = "onServerButton";
            this.onServerButton.Size = new System.Drawing.Size(124, 23);
            this.onServerButton.TabIndex = 1;
            this.onServerButton.Text = "서버 열기";
            this.onServerButton.UseVisualStyleBackColor = true;
            this.onServerButton.Click += new System.EventHandler(this.onServerButton_Click);
            // 
            // clientButton
            // 
            this.clientButton.Location = new System.Drawing.Point(21, 403);
            this.clientButton.Name = "clientButton";
            this.clientButton.Size = new System.Drawing.Size(124, 23);
            this.clientButton.TabIndex = 2;
            this.clientButton.Text = "접속하기";
            this.clientButton.UseVisualStyleBackColor = true;
            this.clientButton.Click += new System.EventHandler(this.clientButton_Click);
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(21, 347);
            this.ipTextBox.Multiline = true;
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(124, 21);
            this.ipTextBox.TabIndex = 3;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Enabled = false;
            this.statusTextBox.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.statusTextBox.Location = new System.Drawing.Point(21, 251);
            this.statusTextBox.Multiline = true;
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(124, 90);
            this.statusTextBox.TabIndex = 4;
            // 
            // NetworkTetris
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(478, 537);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.ipTextBox);
            this.Controls.Add(this.clientButton);
            this.Controls.Add(this.onServerButton);
            this.Controls.Add(this.scoreLabel);
            this.Name = "NetworkTetris";
            this.Text = "테트리스";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetworkTetris_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer downTimer;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Timer oclock;
        private System.Windows.Forms.Button onServerButton;
        private System.Windows.Forms.Button clientButton;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.TextBox statusTextBox;
    }
}

