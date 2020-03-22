using System;
using System.Windows.Forms;

namespace Small_IDE
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = "<body bgcolor = \"cyan\">" +
                "<p>Small is as the name suggest a <u><b>small \"programming\"</b></u> language and has the following commands<br></p>" +
                "<ol>" +
                "<li>DISPLAY</li>" +
                "<li>DISPLAY VAR</li>" +
                "<li>DISPLAY RANDINT MIN MAX</li>" +
                "<li>DISPLAY BITWISE NUMBER >> OR << AMOUNT" +
                "<li>DISPLAY READ</li>" +
                "<li>DISPLAY READ VAR</li>" +
                "<li>DRAW x y x y</li>" +
                "<li>DRAW VAR x y x y</li>" +
                "<li>DRAW WIDTH x</li>" +
                "<li>DRAW COLOUR R G B</li>" +
                "<li>DRAW COLOUR VAR R G B</li>" +
                "<li>DRAW CLEAR</li>" +
                "<li>DRAW MODE ELLIPSE/LINE/RECTANGLE</li>" +
                "<li>IF CONDITION GOTO LINE</li>" +
                "<li>IF VAR CONDITION GOTO LINE</li>" +
                "<li>NEWL</li>" +
                "<li>QUIT</li>" +
                "<li>GOTO LINE</li>" +
                "<li>MATH NUMBER_ONE -*/+ NUMBER_TWO</li>" +
                "<li>MATH VAR VARIABLE_NAME -*/+ WHATEVER YOU WANT TO DO</li>" +
                "<li>MATH RANDINT VAR VARIABLE_NAME MIN MAX</li>" +
                "<li>SET VARIABLE_NAME = VALUE</li>" +
                "<li>MSG MESSAGE</li>" +
                "<li>MSG VAR VARIABLE_NAME</li>" +
                "<li>INC VARIABLE</li>" +
                "<li>INC VARIABLE AMOUNT</li>" +
                "<li>DEC VARIABLE</li>" +
                "<li>DEC VARIABLE AMOUNT</li>" +
                "<li>BITWISE VAR VARIABLE_NAME >> OR << AMOUNT</li>" +
                "<li>CLS</li>" +
                "<li>KILL</li>" +
                "<li>SKIP</li>" +
                "<li>SKIP LINES</li>" +
                "<li>READ VAR <b>VARIABLE</b> FILE_DESTINATION</li></ol>" +
                "</body>";
        }
    }
}
