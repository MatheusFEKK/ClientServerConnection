using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameUI
{
    public partial class Form1 : Form
    {  
       private static string NicknameFromUser;
       private static string SessionIDFromUser;
       private static Socket handler;
       public static string Nickname
        {
            get { return NicknameFromUser; }
            set { NicknameFromUser = value; }
        }

        public static string SessionID   
        {
            get { return SessionID; }
            set { SessionIDFromUser = value; }
        }

        public static Socket Handler
        {
            get { return handler; }
            set { handler = value; }
        }
        public Form1()
        {
            InitializeComponent();

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Nickname1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
