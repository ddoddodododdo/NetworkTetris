using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Tetris201770001
{
    enum NetworkStatus
    {
        notConnected,
        Server,
        Client
    }
    public partial class NetworkTetris : Form
    {
        Game myGame;
        Game playerGame;

        Socket socket;
        Thread receiveThread;
        IPAddress ipAdress;
        int port = 9999;
        byte[] receiveData = new byte[1024];
        string encodingData;

        private const int EDGE_SIZE_X = 2;
        private const int EDGE_SIZE_Y = 2;
        private int downTickInterval = 600;
        private int lastScore = 0;
        private int attackPoint = 100;
        private int playTime = 0;
        private bool isWin = false;
        private bool putUp = false;
        private bool putShift = false;
        private bool isPlay;
        private bool[,] playerBoard = new bool[Game.BY, Game.BX];

        Point boardStartPoint;
        Point boardEndPoint;
        Point playerBoardStartPoint;
        Point playerBoardEndPoint;
        Rectangle startButtonRect;
        Rectangle quitButtonRect;


        NetworkStatus networkStatus = NetworkStatus.notConnected;
        SolidBrush[] blockBrushes = new SolidBrush[8];
        SolidBrush backgroundBrush = new SolidBrush(Color.FromArgb(255, 30, 30, 40));

        public NetworkTetris()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(255, 70, 70, 80);
            SetBlockBrushes();
            SetSize();
            Reset();
        }

        private void Reset()
        {
            myGame = new Game();
            playerGame = new Game();
            downTickInterval = 600;
            isPlay = false;
        } 
        private void SetBlockBrushes()
        {
            blockBrushes[0] = new SolidBrush(Color.FromArgb(000, 255, 255));
            blockBrushes[1] = new SolidBrush(Color.FromArgb(255, 255, 000));
            blockBrushes[2] = new SolidBrush(Color.FromArgb(051, 000, 255));
            blockBrushes[3] = new SolidBrush(Color.FromArgb(255, 100, 000));
            blockBrushes[4] = new SolidBrush(Color.FromArgb(255, 000, 000));
            blockBrushes[5] = new SolidBrush(Color.FromArgb(000, 200, 000));
            blockBrushes[6] = new SolidBrush(Color.FromArgb(255, 000, 255));
            blockBrushes[7] = new SolidBrush(Color.FromArgb(200, 200, 200));
        }
        private void SetSize()
        {
            int formSizeX = EDGE_SIZE_X * 5 + Game.BX * 2+ Game.OPTION_SIZE_X * 2;
            int formSizeY = EDGE_SIZE_Y * 2 + Game.BY;
            SetClientSizeCore(formSizeX * Game.CELL_SIZE, formSizeY * Game.CELL_SIZE); //클라이언트 사이즈

            boardStartPoint = new Point(EDGE_SIZE_X * 2 + Game.OPTION_SIZE_X, EDGE_SIZE_Y); //보드판 사이즈
            boardEndPoint = new Point(boardStartPoint.X + Game.BX, EDGE_SIZE_Y + Game.BY);
            playerBoardStartPoint = new Point(EDGE_SIZE_X * 4 + Game.OPTION_SIZE_X * 2 + Game.BX, EDGE_SIZE_Y);
            playerBoardEndPoint = new Point(playerBoardStartPoint.X + Game.BX, EDGE_SIZE_X + Game.BY);

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawGame(e.Graphics);
            DrawOption(e.Graphics);
            if(!isPlay) DrawStartScene(e.Graphics);
            DoubleBuffered = true;
        }
        private void DrawStartScene(Graphics g)
        {
            Font font = new Font("Gill Sans MT", 23, FontStyle.Bold);
            SolidBrush brush = new SolidBrush(Color.FromArgb(200, 200, 200));
            Rectangle rect = new Rectangle(boardStartPoint.X * Game.CELL_SIZE, boardStartPoint.Y * Game.CELL_SIZE    //씬 배경
                , (boardEndPoint.X - boardStartPoint.X) * Game.CELL_SIZE, (boardEndPoint.Y - boardStartPoint.Y) * Game.CELL_SIZE);
            g.FillRectangle(brush, rect);

            
            //START배경
            rect = new Rectangle(rect.X + rect.Width / 4, rect.Y + rect.Height / 3, rect.Width / 2, rect.Height / 10);
            brush = new SolidBrush(Color.FromArgb(20, 20, 20));
            g.FillRectangle(brush, rect);
            startButtonRect = rect;

            //START 글자
            brush = new SolidBrush(Color.FromArgb(250, 250, 250));
            g.DrawString("START", font, brush, rect.X, rect.Y+5);

            //QUIT배경
            rect.Y += EDGE_SIZE_Y * 3 * Game.CELL_SIZE;
            brush = new SolidBrush(Color.FromArgb(20, 20, 20));
            g.FillRectangle(brush, rect);
            quitButtonRect = rect;

            //QUIT 글자
            brush = new SolidBrush(Color.FromArgb(250, 250, 250));
            g.DrawString("  QUIT", font, brush, rect.X, rect.Y + 5);

            if (lastScore != 0)
            {
                rect.Y -= EDGE_SIZE_Y * 5 * Game.CELL_SIZE;
                brush = new SolidBrush(Color.FromArgb(20, 20, 20));
                g.DrawString(Convert.ToString(lastScore) + "점", font, brush, rect.X, rect.Y);
                if(isWin) g.DrawString("W I N", font, brush, rect.X, rect.Y - 50);
                else g.DrawString("L O S E", font, brush, rect.X, rect.Y - 50);
            }
        }
        
        #region DrawGame()
        private void DrawGame(Graphics g)
        {
            DrawBackground(g, boardStartPoint, boardEndPoint);
            DrawBlock(g, boardStartPoint);
            DrawLine(g, boardStartPoint, boardEndPoint);

            if (networkStatus != NetworkStatus.notConnected)
            {
                DrawBackground(g, playerBoardStartPoint, playerBoardEndPoint);
                DrawPlayerBoard(g, playerBoardStartPoint);
                DrawLine(g, playerBoardStartPoint, playerBoardEndPoint);
            }
        }
        private void DrawPlayerBoard(Graphics g, Point startPoint)
        {
            Rectangle rect;
            for (int yy = 0; yy < Game.BY; yy++)
            {
                for (int xx = 0; xx < Game.BX; xx++)
                {
                    if (playerBoard[yy, xx])
                    {
                        rect = new Rectangle((xx + startPoint.X) * Game.CELL_SIZE, (yy + startPoint.Y) * Game.CELL_SIZE, Game.CELL_SIZE, Game.CELL_SIZE);
                        g.FillRectangle(blockBrushes[7], rect);
                    }
                }
            }
        }
        private void DrawBackground(Graphics g, Point startPoint, Point endPoint)
        {
            startPoint.X *= Game.CELL_SIZE;
            startPoint.Y *= Game.CELL_SIZE;
            endPoint.X = endPoint.X * Game.CELL_SIZE - startPoint.X;
            endPoint.Y = endPoint.Y * Game.CELL_SIZE - startPoint.Y;
            g.FillRectangle(backgroundBrush, new Rectangle(startPoint.X, startPoint.Y
                , endPoint.X, endPoint.Y));
        }
        private void DrawBlock(Graphics g, Point startPoint)
        {
            DrawStackedBlock(g, startPoint);
            for (int yy = 0; yy < 4; yy++)
            {
                for (int xx = 0; xx < 4; xx++)
                {
                    DrawNowBlock(g, startPoint, yy, xx);
                    DrawDropPreView(g, startPoint, yy, xx);
                }
            }

        }
        private void DrawLine(Graphics g, Point startPoint, Point endPoint)
        {
            Point drawStartPoint = new Point();
            Point drawEndPoint = new Point();
            Pen linePen = new Pen(Color.FromArgb(255, 50, 50, 50), 1);
            for(int xx = 0; xx <= endPoint.X - startPoint.X; xx++) //세로줄
            {
                drawStartPoint.X = (startPoint.X + xx) * Game.CELL_SIZE;
                drawStartPoint.Y = startPoint.Y * Game.CELL_SIZE;
                drawEndPoint.X = drawStartPoint.X;
                drawEndPoint.Y = endPoint.Y * Game.CELL_SIZE;
                g.DrawLine(linePen, drawStartPoint, drawEndPoint);
            }

            for (int yy = 0; yy <= endPoint.Y - startPoint.Y; yy++) //가로줄
            {
                drawStartPoint.X = startPoint.X * Game.CELL_SIZE;
                drawStartPoint.Y = (startPoint.Y + yy) * Game.CELL_SIZE;
                drawEndPoint.X = endPoint.X * Game.CELL_SIZE;
                drawEndPoint.Y = drawStartPoint.Y;
                g.DrawLine(linePen, drawStartPoint, drawEndPoint);
            }

        }
        private void DrawStackedBlock(Graphics g, Point startPoint) // 쌓여있는 블록
        {
            Rectangle rect;
            for (int yy = 0; yy < Game.BY; yy++)
            {
                for (int xx = 0; xx < Game.BX; xx++)
                {
                    if (myGame.gameBoard[yy, xx])
                    {
                        rect = new Rectangle((xx + startPoint.X) * Game.CELL_SIZE, (yy + startPoint.Y) * Game.CELL_SIZE, Game.CELL_SIZE, Game.CELL_SIZE);
                        g.FillRectangle(blockBrushes[myGame.gameColorBoard[yy, xx]], rect);
                    }
                }
            }
        }
        private void DrawNowBlock(Graphics g, Point startPoint, int yy, int xx)
        {
            Rectangle rect;
            if (myGame.BlockCheck(yy, xx, -1))
            {
                rect = myGame.GetRect(yy + startPoint.Y, xx + startPoint.X);
                g.FillRectangle(blockBrushes[myGame.now.blockNum], rect);
            }
        } 
        private void DrawDropPreView(Graphics g, Point startPoint, int yy, int xx) //Drop 미리보기
        {

            Rectangle rect;
            if (myGame.BlockCheck(yy, xx, -1))
            {   
                int dis = myGame.GetFloorDistance();
                if (dis != 0)
                {
                    Color color = blockBrushes[myGame.now.blockNum].Color;
                    Pen gPen = new Pen(color, 2);
                    rect = myGame.GetRect(yy + dis + startPoint.Y, xx + startPoint.X);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(30, color.R, color.G, color.B)), rect);
                    g.DrawRectangle(gPen, rect);
                }
            }
        }
        #endregion
        #region DrawOption()
        private void DrawOption(Graphics g)
        {
            DrawHoldArea(g);
            DrawNextArea(g);
            DrawString(g);
        }
        private void DrawHoldArea(Graphics g)
        {
            Point startPoint = new Point(EDGE_SIZE_X, EDGE_SIZE_Y);
            Point endPoint = new Point(EDGE_SIZE_X + Game.OPTION_SIZE_X, EDGE_SIZE_Y + 6);
            DrawBackground(g, startPoint, endPoint);

            if (myGame.holdBlockNum != -1)
            {
                Rectangle rect;
                for (int yy = 0; yy < 4; yy++)
                {
                    for (int xx = 0; xx < 4; xx++)
                    {
                        if (myGame.BlockCheck(yy, xx, 3))
                        {
                            rect = new Rectangle((xx + startPoint.X) * Game.CELL_SIZE
                                , (startPoint.Y + yy + 1) * Game.CELL_SIZE
                                , Game.CELL_SIZE, Game.CELL_SIZE);
                            g.FillRectangle(blockBrushes[myGame.holdBlockNum], rect);
                        }
                    }
                }
            }

            DrawLine(g, startPoint, endPoint);

        }
        private void DrawNextArea(Graphics g)
        {
            Point startPoint = new Point(EDGE_SIZE_X * 3 + Game.BX + Game.OPTION_SIZE_X, EDGE_SIZE_Y);
            Point endPoint = new Point(startPoint.X + Game.OPTION_SIZE_X, 18);
            DrawBackground(g, startPoint, endPoint);
            if (isPlay)
            {
                Rectangle rect;
                for (int nxt = 0; nxt < myGame.nextBlocks.Length; nxt++)
                {
                    for (int yy = 0; yy < 4; yy++)
                    {
                        for (int xx = 0; xx < 4; xx++)
                        {
                            if (myGame.BlockCheck(yy, xx, nxt))
                            {
                                rect = new Rectangle((xx + startPoint.X) * Game.CELL_SIZE
                                    , (startPoint.Y + yy + 1 + nxt * 5) * Game.CELL_SIZE
                                    , Game.CELL_SIZE, Game.CELL_SIZE);
                                g.FillRectangle(blockBrushes[myGame.nextBlocks[nxt].blockNum], rect);
                            }
                        }
                    }
                }
            }
            DrawLine(g, startPoint, endPoint);
        }
        private void DrawString(Graphics g)
        {
            int nextX = (EDGE_SIZE_X * 3 + Game.BX + Game.OPTION_SIZE_X) * Game.CELL_SIZE;
            int nextY = EDGE_SIZE_Y * Game.CELL_SIZE;
            Font font = new Font("Gill Sans MT", 23, FontStyle.Bold);
            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 254, 255, 255));
            g.DrawString("HOLD", font, brush, EDGE_SIZE_X * Game.CELL_SIZE, EDGE_SIZE_Y * Game.CELL_SIZE - Game.CELL_SIZE);
            g.DrawString("SCORE", font, brush, EDGE_SIZE_X * Game.CELL_SIZE-10, (EDGE_SIZE_Y * 4) * Game.CELL_SIZE);

            //플레이시간
            g.DrawString("TIME", font, brush, EDGE_SIZE_X * Game.CELL_SIZE, (Game.BY - 2) * Game.CELL_SIZE);
            string secondStr = playTime % 60 < 10 ? "0" + playTime % 60 : "" + playTime % 60;
            string minuteStr = playTime / 60 < 10 ? "0" + playTime / 60 : "" + playTime / 60;
            g.DrawString(minuteStr +":" + secondStr, 
                font, brush, EDGE_SIZE_X * Game.CELL_SIZE, (Game.BY) * Game.CELL_SIZE);

            g.DrawString("NEXT", font, brush, nextX,  nextY - Game.CELL_SIZE);
            g.DrawString("HELP", font, brush, nextX, nextY * 10);
            font = new Font("Gill Sans MT", 11, FontStyle.Bold);
            g.DrawString("MOVE　　↑", font, brush, nextX-10, nextY * 11);
            g.DrawString("　　　← ↓ →", font, brush, nextX, nextY * 11.5f);
            g.DrawString("DROP :  Space Bar", font, brush, nextX-10, nextY * 12);
            g.DrawString("HOLD :  SHIFT", font, brush, nextX - 10, nextY * 12.5f);
            font = new Font("Gill Sans MT", 11);
            g.DrawString("   (-100 SCORE)", font, brush, nextX - 10, nextY * 13f);

            scoreLabel.Location = new Point(EDGE_SIZE_X * Game.CELL_SIZE-10, (EDGE_SIZE_Y * 5) * Game.CELL_SIZE);
            scoreLabel.Text = Convert.ToString(myGame.gameScore);
        }
        #endregion

        #region Event
        private void NetworkTetris_FormClosing(object sender, FormClosingEventArgs e)
        {
            receiveThread.Abort();
            socket.Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (isPlay)
            {
                switch (e.KeyCode)
                {
                    case Keys.ShiftKey:
                        if(!putShift) myGame.UseHold();
                        putShift = true;
                        break;
                    case Keys.Down:
                        if (!myGame.MoveDown()) CheckGameOver();
                        break;
                    case Keys.Right:
                        myGame.MoveRight();
                        break;
                    case Keys.Left:
                        myGame.MoveLeft();
                        break;
                    case Keys.Up:
                        if(!putUp) myGame.MoveTurn();
                        putUp = true;
                        break;
                    case Keys.Space:
                        myGame.MoveDrop();
                        CheckGameOver();
                        break;
                }

                Invalidate();
            }
            
        }
       
        private void downTimer_Tick(object sender, EventArgs e)
        {
            if (isPlay)
            {
                myGame.gameScore++;
                if (!myGame.MoveDown()) CheckGameOver();
                if (downTickInterval > 200) downTimer.Interval = downTickInterval--;
                if (networkStatus != NetworkStatus.notConnected)
                {
                    SendToNetwork(myGame.gameBoard);
                    AttackCheck();
                }
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if((startButtonRect.X < e.Location.X && 
                e.Location.X < startButtonRect.X + startButtonRect.Width)&&
                (startButtonRect.Y <= e.Location.Y && 
                e.Location.Y <= startButtonRect.Y + startButtonRect.Height))
            {
                OffController();
                if(networkStatus != NetworkStatus.notConnected) SendToNetwork("Start");
                isPlay = true;
            }
            
            else if ((quitButtonRect.X <= e.Location.X &&
                e.Location.X <= quitButtonRect.X + quitButtonRect.Width) &&
                (quitButtonRect.Y <= e.Location.Y &&
                e.Location.Y <= quitButtonRect.Y + quitButtonRect.Height))
            {
                Close();
            }
        }

        private void oclock_Tick(object sender, EventArgs e)
        {
            if (isPlay) playTime++;
            else playTime = 0;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (isPlay)
            {
                switch (e.KeyCode)
                {
                    case Keys.ShiftKey:
                        putShift = false;
                        break;
                    case Keys.Up:
                        putUp = false;
                        break;
                }
            }
        }
        #endregion

        #region NetWork

        private void onServerButton_Click(object sender, EventArgs e)
        {
            OnServer();
            Invalidate();
        }

        private void clientButton_Click(object sender, EventArgs e)
        {
            ConnectToServer();
            Invalidate();
        }

        private void OffController()
        {
            onServerButton.Enabled = false;
            clientButton.Enabled = false;
            ipTextBox.Enabled = false;
        }

        private void OnServer()
        {
            
            ipAdress = Dns.Resolve(Dns.GetHostName()).AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAdress, port);

            ipTextBox.SelectedText = ipAdress.ToString();
            statusTextBox.AppendText("서버를 열었습니다. \r\n");

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(10);
            try
            {
                socket = socket.Accept();
            }catch(Exception e) 
            {
                statusTextBox.AppendText("연결 실패.\r\n");
                return;
            }

            OffController();
            networkStatus = NetworkStatus.Server;
            statusTextBox.AppendText("클라이언트와 연결.\r\n");
            receiveThread = new Thread(new ThreadStart(ReceiveFromNetwork));
            receiveThread.Start();
            ipTextBox.Text = "";
        }

        private void ConnectToServer()
        {
            ipAdress = IPAddress.Parse(ipTextBox.Text);
            IPEndPoint endPoint = new IPEndPoint(ipAdress, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(endPoint);
            }catch(Exception e)
            {
                statusTextBox.AppendText("연결 실패.\r\n");
                return;
            }

            OffController();
            networkStatus = NetworkStatus.Client;
            statusTextBox.AppendText("서버에 연결됐습니다. \r\n");
            receiveThread = new Thread(new ThreadStart(ReceiveFromNetwork));
            receiveThread.Start();
            ipTextBox.Text = "";
        }

        private void ReceiveFromNetwork()
        {
            while (networkStatus != NetworkStatus.notConnected)
            {
                int dataLength = socket.Receive(receiveData);
                encodingData = Encoding.UTF8.GetString(receiveData, 0, dataLength);

                if (encodingData.Equals("Start"))
                {
                    isPlay = true;
                }
                else if (encodingData.Equals("GameOver"))
                {
                    lastScore = myGame.gameScore;
                    isWin = true;
                    Reset();
                    Invalidate();
                }
                else if (encodingData.Equals("Attack"))
                {
                    myGame.Attacked();
                }
                else
                {
                    int idx = 0;
                    for (int yy = 0; yy < Game.BY; yy++)
                    {
                        for (int xx = 0; xx < Game.BX; xx++)
                        {
                            if (encodingData[idx] == '0') playerBoard[yy, xx] = false;
                            else if (encodingData[idx] == '1') playerBoard[yy, xx] = true;
                            idx++;
                        }
                    }
                }

            }
        }
        private void SendToNetwork(bool[,] gameBoard)
        {
            string data = "";
            for(int y = 0; y < Game.BY; y++)
            {
                for(int x = 0; x < Game.BX; x++)
                {
                    if (gameBoard[y, x]) data += 1;
                    else data += 0;
                }
            }

            byte[] sendData = Encoding.UTF8.GetBytes(data);
            socket.Send(sendData);
        }

        private void SendToNetwork(String data)
        {
            byte[] sendData = Encoding.UTF8.GetBytes(data);
            socket.Send(sendData);
        }

        private void AttackCheck()
        {
            if (myGame.gameScore > attackPoint)
            {
                SendToNetwork("Attack");
                attackPoint += 100;
            }
        }
        

        #endregion

        public void CheckGameOver()
        {
            if (myGame.CheckGameOver())
            {
                if (networkStatus != NetworkStatus.notConnected)
                {
                    SendToNetwork("GameOver");
                    isWin = false;
                }
                lastScore = myGame.gameScore;
                Reset();
            }
        }

    }
}
