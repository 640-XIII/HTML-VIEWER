using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Small_IDE
{
    public partial class Form1 : Form
    {
        const char seperator = ' ';
        int penWidth = 1;
        int amount = 0;
        int forLine = 0;
        byte drawMode = 1;
        Color penColour = Color.Black;
        bool saved = false;
        short pointer = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            commandsTextbox.Text = commandsTextbox.Text.ToUpper();
            pointer = 0;

            try
            {
                string[] commandToExecute = new string[commandsTextbox.Lines.Length];
                string[,] varName = new string[1000, 2];

                outputTextbox.Text = "";


                for (int i = 0; i < commandsTextbox.Lines.Length; i++)
                {
                    if (commandsTextbox.Lines[i].Split(seperator)[0] == "SET")
                    {
                        for (int x = 3; x < commandsTextbox.Lines[i].Split(seperator).Length; x++)
                        {
                            varName[pointer, 1] += commandsTextbox.Lines[i].Split(seperator)[x] + " ";
                        }

                        varName[pointer, 0] = commandsTextbox.Lines[i].Split(seperator)[1];

                        pointer++;
                    }
                    else { }

                    commandToExecute[i] = commandsTextbox.Lines[i].ToString();
                    commandToExecute[i].TrimEnd(seperator);
                }


                for (int i = 0; i < commandsTextbox.Lines.Length; i++)
                {
                startLabel:
                    commandToExecute[i] = commandToExecute[i].TrimEnd(' ').TrimStart(' ');

                    if (commandToExecute[i].Split(seperator)[0] == "DISPLAY")
                    {
                        if (commandToExecute[i].Split(seperator)[1] == "VAR")
                        {
                            for (int x = 0; x < varName.Length / 2; x++)
                            {
                                if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                {
                                    if (commandToExecute[i].Contains("+") || commandToExecute[i].Contains("-") || commandToExecute[i].Contains("*") || commandToExecute[i].Contains("/"))
                                    {
                                        for (int j = 3; j < commandToExecute[i].Split(seperator).Length; j += 2)
                                        {
                                            switch (commandToExecute[i].Split(seperator)[j])
                                            {
                                                case ("+"):
                                                    varName[x, 1] = (int.Parse(varName[x, 1]) + int.Parse(commandToExecute[i].Split(seperator)[j + 1])).ToString();
                                                    break;
                                                case ("-"):
                                                    varName[x, 1] = (int.Parse(varName[x, 1]) - int.Parse(commandToExecute[i].Split(seperator)[j + 1])).ToString();
                                                    break;
                                                case ("*"):
                                                    varName[x, 1] = (int.Parse(varName[x, 1]) * int.Parse(commandToExecute[i].Split(seperator)[j + 1])).ToString();
                                                    break;
                                                case ("/"):
                                                    varName[x, 1] = (int.Parse(varName[x, 1]) / int.Parse(commandToExecute[i].Split(seperator)[j + 1])).ToString();
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        outputTextbox.Text += varName[x, 1];
                                    }
                                    else { outputTextbox.Text += varName[x, 1]; }

                                    break;
                                }
                                else { }
                            }
                        }
                        else if (commandToExecute[i].Split(seperator)[1] == "MATH")
                        {
                            int intToShow = int.Parse(commandToExecute[i].Split(seperator)[2]);

                            for (int x = 3; x < commandToExecute[i].Split(seperator).Length; x += 2)
                            {
                                switch (commandToExecute[i].Split(seperator)[x])
                                {
                                    case ("+"):
                                        intToShow += int.Parse(commandToExecute[i].Split(seperator)[x + 1]);
                                        break;
                                    case ("-"):
                                        intToShow -= int.Parse(commandToExecute[i].Split(seperator)[x + 1]);
                                        break;
                                    case ("*"):
                                        intToShow *= int.Parse(commandToExecute[i].Split(seperator)[x + 1]);
                                        break;
                                    case ("/"):
                                        intToShow /= int.Parse(commandToExecute[i].Split(seperator)[x + 1]);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            outputTextbox.Text += intToShow.ToString();
                        }
                        else if (commandToExecute[i].Split(seperator)[1] == "RANDINT")
                        {
                            Random randomNumberGen = new Random(DateTime.Now.Millisecond * DateTime.Now.Day - DateTime.Now.Year);
                            int minVal = int.Parse(commandToExecute[i].Split(seperator)[2]);
                            int maxVal = int.Parse(commandToExecute[i].Split(seperator)[3]);

                            outputTextbox.Text += randomNumberGen.Next(minVal, maxVal);
                        }
                        else if (commandToExecute[i].Split(seperator)[1] == "BITWISE")
                        {
                            if (commandToExecute[i].Split(seperator)[3] == ">>")
                            {
                                outputTextbox.Text += "{}".Replace("{}", ((int.Parse(commandToExecute[i].Split(seperator)[2])) >> int.Parse(commandToExecute[i].Split(seperator)[4])).ToString());
                            }
                            else if (commandToExecute[i].Split(seperator)[3] == "<<")
                            {
                                outputTextbox.Text += "{}".Replace("{}", ((int.Parse(commandToExecute[i].Split(seperator)[2])) << int.Parse(commandToExecute[i].Split(seperator)[4])).ToString());
                            }
                            else { throw new Exception(); }
                        } else if (commandToExecute[i].Split(seperator)[1] == "READ")
                        {
                            if (commandToExecute[i].Split(seperator)[2] == "VAR")
                            {
                                bool found = false;
                                string value = "";

                                for (int x = 0; x < varName.Length / 2; x++)
                                {
                                    if (varName[x, 0] == commandToExecute[i].Split(seperator)[3])
                                    {
                                        value = varName[x, 1];
                                        found = true;
                                    }
                                }

                                if (found == true)
                                {
                                    StreamReader inputFile = new StreamReader(value);
                                    outputTextbox.Text += inputFile.ReadToEnd();
                                    inputFile.Close();
                                }
                                else
                                {
                                    outputTextbox.Text = "UNABLE TO FIND VAR {}".Replace("{}", commandToExecute[i].Split(seperator)[2]);
                                }
                            }
                            else
                            {
                                StreamReader inputFile = new StreamReader(commandToExecute[i].Split(seperator)[2]);
                                outputTextbox.Text += inputFile.ReadToEnd();
                                inputFile.Close();
                            }
                        } else
                        {
                            for (int x = 1; x < commandToExecute[i].Split(seperator).Length; x++)
                            {
                                outputTextbox.Text = outputTextbox.Text + commandToExecute[i].Split(seperator)[x] + " ";
                            }
                        }
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "MATH")
                    {
                        try
                        {
                            if (commandToExecute[i].Split(seperator)[1] == "RANDINT")
                            {
                                Random randomNumGen = new Random(DateTime.Now.Millisecond);

                                if (commandToExecute[i].Split(seperator)[2] == "VAR")
                                {
                                    int first = int.Parse(commandToExecute[i].Split(seperator)[4]);
                                    int second = int.Parse(commandToExecute[i].Split(seperator)[5]);
                                    for (int x = 0; x < varName.Length / 2; x++)
                                    {
                                        if (varName[x, 0] == commandToExecute[i].Split(seperator)[3])
                                        {
                                            try
                                            {
                                                varName[x, 1] = randomNumGen.Next(first, second).ToString();
                                                break;
                                            }
                                            catch (Exception) { outputTextbox.Text += "CANNOT MATH TYPE VAR STRING"; }
                                        }
                                        else { }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        int first = int.Parse(commandToExecute[i].Split(seperator)[2]);
                                        int second = int.Parse(commandToExecute[i].Split(seperator)[3]);
                                        int result = randomNumGen.Next(first, second);

                                        outputTextbox.Text = result.ToString();
                                    }
                                    catch (Exception) { }
                                }

                                System.Threading.Thread.Sleep(1);
                            }
                            else
                            {
                                if (commandToExecute[i].Split(seperator)[1] == "VAR")
                                {
                                    for (int x = 0; x < varName.Length / 2; x++)
                                    {
                                        if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                        {
                                            try
                                            {
                                                for (int j = 3; j < commandToExecute[i].Split(seperator).Length; j += 2)
                                                {
                                                    int number = int.Parse(commandToExecute[i].Split(seperator)[j + 1]);

                                                    switch (commandToExecute[i].Split(seperator)[j])
                                                    {
                                                        case ("+"):
                                                            varName[x, 1] = (int.Parse(varName[x, 1]) + number).ToString();
                                                            break;
                                                        case ("-"):
                                                            varName[x, 1] = (int.Parse(varName[x, 1]) - number).ToString();
                                                            break;
                                                        case ("*"):
                                                            varName[x, 1] = (int.Parse(varName[x, 1]) * number).ToString();
                                                            break;
                                                        case ("/"):
                                                            varName[x, 1] = (int.Parse(varName[x, 1]) / number).ToString();
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                            catch (Exception) { outputTextbox.Text += "CANNOT MATH TYPE VAR STRING"; }
                                        }
                                        else { }
                                    }
                                }
                                else
                                {
                                    int first = int.Parse(commandToExecute[i].Split(seperator)[1]);
                                    int second = int.Parse(commandToExecute[i].Split(seperator)[3]);
                                    int result = 0;
                                    bool print = false;

                                    switch (commandToExecute[i].Split(seperator)[2])
                                    {
                                        case ("+"):
                                            result = first + second;
                                            print = true;

                                            break;
                                        case ("-"):
                                            result = first - second;
                                            print = true;

                                            break;
                                        case ("*"):
                                            result = first * second;
                                            print = true;

                                            break;
                                        case ("/"):
                                            result = first / second;
                                            print = true;

                                            break;
                                        default:
                                            outputTextbox.Text += "ERROR";
                                            break;
                                    }

                                    if (print == true)
                                    {
                                        outputTextbox.Text = result.ToString();
                                    }
                                    else { }
                                }
                            }
                        }
                        catch (Exception) { goto endLabel; }
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "GOTO")
                    {
                        i = int.Parse(commandToExecute[i].Split(seperator)[1]) - 1;
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "QUIT")
                    {
                        break;
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "INC")
                    {
                        int amountToIncrease = 0;
                        if (commandToExecute[i].Split(seperator).Length == 3) { amountToIncrease = int.Parse(commandToExecute[i].Split(seperator)[2]); } else { amountToIncrease = 1; }

                        for (int x = 0; x < varName.Length / 2; x++)
                        {
                            if (varName[x, 0] == commandToExecute[i].Split(seperator)[1])
                            {
                                varName[x, 1] = (int.Parse(varName[x, 1]) + amountToIncrease).ToString();
                                break;
                            }
                            else { }
                        }
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "DEC")
                    {
                        int amountToDecrease = 0;
                        if (commandToExecute[i].Split(seperator).Length == 3) { amountToDecrease = int.Parse(commandToExecute[i].Split(seperator)[2]); } else { amountToDecrease = 1; }

                        for (int x = 0; x < varName.Length / 2; x++)
                        {
                            if (varName[x, 0] == commandToExecute[i].Split(seperator)[1])
                            {
                                varName[x, 1] = (int.Parse(varName[x, 1]) - amountToDecrease).ToString();
                                break;
                            }
                            else { }
                        }
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "NEWL")
                    {
                        outputTextbox.Text += "\n";
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "IF")
                    {
                        if (commandToExecute[i].Split(seperator)[1] == "VAR")
                        {
                            try
                            {
                                switch (commandToExecute[i].Split(seperator)[3])
                                {
                                    case ("<"):
                                        for (int x = 0; x < varName.Length / 2; x++)
                                        {
                                            if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                            {
                                                if (int.Parse(varName[x, 1]) < int.Parse(commandToExecute[i].Split(seperator)[4]))
                                                {
                                                    i = int.Parse(commandToExecute[i].Split(seperator)[6]) - 2;
                                                }
                                                else { }

                                                break;
                                            }
                                            else { }
                                        }

                                        break;
                                    case (">"):
                                        for (int x = 0; x < varName.Length / 2; x++)
                                        {
                                            if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                            {
                                                if (int.Parse(varName[x, 1]) > int.Parse(commandToExecute[i].Split(seperator)[4]))
                                                {
                                                    i = int.Parse(commandToExecute[i].Split(seperator)[6]) - 2;
                                                }
                                                else { }

                                                break;
                                            }
                                            else { }
                                        }

                                        break;
                                    case ("<="):
                                        for (int x = 0; x < varName.Length / 2; x++)
                                        {
                                            if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                            {
                                                if (int.Parse(varName[x, 1]) <= int.Parse(commandToExecute[i].Split(seperator)[4]))
                                                {
                                                    i = int.Parse(commandToExecute[i].Split(seperator)[6]) - 2;
                                                }
                                                else { }

                                                break;
                                            }
                                            else { }
                                        }

                                        break;
                                    case (">="):
                                        for (int x = 0; x < varName.Length / 2; x++)
                                        {
                                            if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                            {
                                                if (int.Parse(varName[x, 1]) >= int.Parse(commandToExecute[i].Split(seperator)[4]))
                                                {
                                                    i = int.Parse(commandToExecute[i].Split(seperator)[6]) - 2;
                                                }
                                                else { }

                                                break;
                                            }
                                            else { }
                                        }

                                        break;
                                    case ("=="):
                                        for (int x = 0; x < varName.Length / 2; x++)
                                        {
                                            if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                            {
                                                if (varName[x, 1] == commandToExecute[i].Split(seperator)[4])
                                                {
                                                    i = int.Parse(commandToExecute[i].Split(seperator)[6]) - 2;
                                                }
                                                else { }

                                                break;
                                            }
                                            else { }
                                        }

                                        break;
                                    case ("!="):
                                        for (int x = 0; x < varName.Length / 2; x++)
                                        {
                                            if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                            {
                                                if (varName[x, 1] != commandToExecute[i].Split(seperator)[4])
                                                {
                                                    i = int.Parse(commandToExecute[i].Split(seperator)[6]) - 2;
                                                }
                                                else { }

                                                break;
                                            }
                                            else { }
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch (Exception) { outputTextbox.Text += "ERROR CONVERTING STRING TO INT"; }
                        }
                        else
                        {
                            try
                            {
                                switch (commandToExecute[i].Split(seperator)[2])
                                {
                                    case ("<"):
                                        if (int.Parse(commandToExecute[i].Split(seperator)[1]) < int.Parse(commandToExecute[i].Split(seperator)[3]))
                                        {
                                            i = int.Parse(commandToExecute[i].Split(seperator)[5]) - 1;
                                        }
                                        else { }

                                        break;
                                    case (">"):
                                        if (int.Parse(commandToExecute[i].Split(seperator)[1]) > int.Parse(commandToExecute[i].Split(seperator)[3]))
                                        {
                                            i = int.Parse(commandToExecute[i].Split(seperator)[5]) - 1;
                                        }
                                        else { }

                                        break;
                                    case ("<="):
                                        if (int.Parse(commandToExecute[i].Split(seperator)[1]) <= int.Parse(commandToExecute[i].Split(seperator)[3]))
                                        {
                                            i = int.Parse(commandToExecute[i].Split(seperator)[5]) - 1;
                                        }
                                        else { }

                                        break;
                                    case (">="):
                                        if (int.Parse(commandToExecute[i].Split(seperator)[1]) >= int.Parse(commandToExecute[i].Split(seperator)[3]))
                                        {
                                            i = int.Parse(commandToExecute[i].Split(seperator)[5]) - 1;
                                        }
                                        else { }

                                        break;
                                    case ("=="):
                                        if (int.Parse(commandToExecute[i].Split(seperator)[1]) == int.Parse(commandToExecute[i].Split(seperator)[3]))
                                        {
                                            i = int.Parse(commandToExecute[i].Split(seperator)[5]) - 1;
                                        }
                                        else { }

                                        break;
                                    case ("!="):
                                        if (int.Parse(commandToExecute[i].Split(seperator)[1]) != int.Parse(commandToExecute[i].Split(seperator)[3]))
                                        {
                                            i = int.Parse(commandToExecute[i].Split(seperator)[5]) - 1;
                                        }
                                        else { }

                                        break;
                                    default:
                                        break;
                                }

                                i--;
                            }
                            catch (Exception) { outputTextbox.Text += "ERROR CONVERTING STRING TO INT"; }
                        }
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "DRAW")
                    {
                        Graphics mainSurface = pictureBox1.CreateGraphics();
                        Pen drawingPen = new Pen(penColour, penWidth);

                        short[] coordinatesToDraw = new short[4];
                        byte p2 = 0;

                        if (commandToExecute[i].Split(seperator)[1] == "VAR")
                        {
                            for (int x = 2; x < 6; x++)
                            {
                                for (int j = 0; j < varName.Length / 2; j++)
                                {
                                    if (commandToExecute[i].Split(seperator)[x] == varName[j, 0])
                                    {
                                        coordinatesToDraw[p2] = short.Parse(varName[j, 1]);
                                        p2++;
                                    }
                                    else { }
                                }
                            }
                        }
                        else if (commandToExecute[i].Split(seperator)[1] == "COLOUR")
                        {
                            if (commandToExecute[i].Split(seperator)[2] == "VAR")
                            {
                                int[] rgbColours = new int[3];

                                for (int x = 3; x < 6; x++)
                                {
                                    for (int j = 0; j < varName.Length / 2; j++)
                                    {
                                        if (commandToExecute[i].Split(seperator)[x] == varName[j, 0])
                                        {
                                            rgbColours[x - 3] = int.Parse(varName[j, 1]);
                                        }
                                        else { }
                                    }
                                }

                                penColour = Color.FromArgb(rgbColours[0], rgbColours[1], rgbColours[2]);
                            }
                            else
                            {
                                penColour = Color.FromArgb(byte.Parse(commandToExecute[i].Split(seperator)[2]), byte.Parse(commandToExecute[i].Split(seperator)[3]), byte.Parse(commandToExecute[i].Split(seperator)[4]));
                            }
                        }
                        else if (commandToExecute[i].Split(seperator)[1] == "CLEAR")
                        {
                            mainSurface.Clear(groupBox1.BackColor);
                        }
                        else if (commandToExecute[i].Split(seperator)[1] == "MODE")
                        {
                            switch (commandToExecute[i].Split(seperator)[2])
                            {
                                case ("LINE"):
                                    drawMode = 1;
                                    break;
                                case ("ELLIPSE"):
                                    drawMode = 2;
                                    break;
                                case ("RECTANGLE"):
                                    drawMode = 3;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (commandToExecute[i].Split(seperator)[1] == "WIDTH")
                        {
                            penWidth = int.Parse(commandToExecute[i].Split(seperator)[2]);
                        }
                        else
                        {
                            for (int x = 1; x < 5; x++)
                            {
                                coordinatesToDraw[p2] = short.Parse(commandToExecute[i].Split(seperator)[x]);
                                p2++;
                            }
                        }

                        switch (drawMode)
                        {
                            case (1):
                                mainSurface.DrawLine(drawingPen, coordinatesToDraw[0], coordinatesToDraw[1], coordinatesToDraw[2], coordinatesToDraw[3]);
                                break;
                            case (2):
                                mainSurface.DrawEllipse(drawingPen, coordinatesToDraw[0], coordinatesToDraw[1], coordinatesToDraw[2], coordinatesToDraw[3]);
                                break;
                            case (3):
                                mainSurface.DrawRectangle(drawingPen, coordinatesToDraw[0], coordinatesToDraw[1], coordinatesToDraw[2], coordinatesToDraw[3]);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "MSG")
                    {
                        if (commandToExecute[i].Split(seperator)[1] == "VAR")
                        {
                            for (int x = 0; x < varName.Length / 2; x++)
                            {
                                if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                {
                                    MessageBox.Show(varName[x, 1]);
                                    break;
                                }
                                else { }
                            }
                        }
                        else
                        {
                            string msgToShow = "";

                            for (int x = 1; x < commandToExecute[i].Split(seperator).Length; x++)
                            {
                                msgToShow += commandToExecute[i].Split(seperator)[x] + " ";
                            }

                            MessageBox.Show(msgToShow);
                        }
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "BITWISE")
                    {
                        if (commandToExecute[i].Split(seperator)[1] == "VAR")
                        {
                            for (int x = 0; x < varName.Length / 2; x++)
                            {
                                if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                {
                                    try
                                    {
                                        if (commandToExecute[i].Split(seperator)[3] == ">>")
                                        {
                                            varName[x, 1] = (int.Parse(varName[x, 1]) >> int.Parse(commandToExecute[i].Split(seperator)[4])).ToString();
                                        }
                                        else if (commandToExecute[i].Split(seperator)[3] == "<<")
                                        {
                                            varName[x, 1] = (int.Parse(varName[x, 1]) << int.Parse(commandToExecute[i].Split(seperator)[4])).ToString();
                                        }
                                        else { throw new Exception(); }

                                        break;
                                    }
                                    catch
                                    {
                                        outputTextbox.Text += "ERROR LINE {}".Replace("{}", (i + 1).ToString());
                                    }
                                }
                                else { }
                            }
                        }
                        {
                            outputTextbox.Text += "ERROR LINE {}".Replace("{}", (i + 1).ToString());
                        }
                    }
                    else if (commandToExecute[i] == "CLS")
                    {
                        outputTextbox.Text = "";
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "KILL")
                    {
                        this.Close();
                    }
                    else if (commandToExecute[i].Split(seperator)[0] == "SKIP")
                    {
                        if (commandToExecute[i].Split(seperator).Length == 2)
                        {
                            i += int.Parse(commandToExecute[i].Split(seperator)[1]) + 1;
                        } else
                        {
                            i += 2;
                        }

                        goto startLabel;
                    } else if (commandToExecute[i].Split(seperator)[0] == "SLEEP")
                    {
                        if (commandToExecute[i].Split(seperator)[1] == "VAR")
                        {
                            int amountToSleep = 0;
                            bool found = false;

                            for (int x = 0; x < varName.Length / 2; x++)
                            {
                                if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                {
                                    amountToSleep = int.Parse(varName[x, 1]);
                                    found = true;
                                } else { }
                            }

                            if (found == true)
                            {
                                Thread.Sleep(amountToSleep);
                            } else
                            {
                                outputTextbox.Text += "\nVARIABLE {} NOT FOUND".Replace("{}", commandToExecute[i].Split(seperator)[2]);
                            }
                        }
                        else
                        {
                            Thread.Sleep(int.Parse(commandToExecute[i].Split(seperator)[1]));
                        }
                    } else if (commandToExecute[i].Split(seperator)[0] == "FOR")
                    {
                        forLine = i;// + 1;

                        if (commandToExecute[i].Split(seperator)[1] == "VAR")
                        {
                            bool found = false;

                            for (int x = 0; x < varName.Length / 2; x++)
                            {
                                if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                {
                                    amount = int.Parse(varName[x, 1]) - 1;
                                    found = true;
                                }
                            }

                            if (found == false)
                            {
                                outputTextbox.Text += "\nUKNOWN VARIABLE {}".Replace("{}", commandToExecute[i].Split(seperator)[2]);
                                break;
                            } else { }
                        } else { 
                            amount = int.Parse(commandToExecute[i].Split(seperator)[1]) - 1; 
                        }
                    } else if (commandToExecute[i] == "END")
                    {
                        if (amount > 0)
                        {
                            i = forLine;
                            amount--;
                        } else { }
                    } else if (commandToExecute[i].Split(seperator)[0] == "READ")
                    {
                        if (commandToExecute[i].Split(seperator)[1] == "VAR")
                        {
                            bool found = false;
                            int value = 0;

                            for (int x = 0; x < varName.Length / 2; x++)
                            {
                                if (varName[x, 0] == commandToExecute[i].Split(seperator)[2])
                                {
                                    found = true;
                                    value = x;
                                }
                            }

                            if (found == true)
                            {
                                StreamReader inputFile = new StreamReader(commandToExecute[i].Split(seperator)[3]);
                                varName[value, 1] = inputFile.ReadToEnd();
                                inputFile.Close();
                            } else
                            {
                                outputTextbox.Text = "UNABLE TO FIND VAR {}".Replace("{}", commandToExecute[i].Split(seperator)[1]);
                            }
                        } else
                        {
                            outputTextbox.Text += "ERROR LINE {}".Replace("{}", (i + 1).ToString());
                        }
                    } else
                    {
                        if (commandToExecute[i].Split(seperator)[0] != "SET" && commandToExecute[i].Split(seperator)[0] != "")
                        {
                            outputTextbox.Text += "UKNOWN COMMAND " + commandToExecute[i] + " LINE {}".Replace("{}", (i + 1).ToString());
                            break;
                        }
                        else { }
                    }
                }

              endLabel:
                byte endByte = 0xff;
            } catch (Exception) { outputTextbox.Text += "\nERROR"; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            outputTextbox.Enabled = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version: 2.0\nProgrammer: Eustathios Koutsos", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startSaveLabel:
            saveFD.FileName = "";
            saveFD.Filter = "ALL (*.*)|*.*|TEXT (*.txt)|*.txt|SML (*.sml)|*.sml";
            saveFD.InitialDirectory = "C:\\";
            saveFD.ShowDialog();

            try
            {

                StreamWriter outputFile = new StreamWriter(saveFD.FileName);
                outputFile.Write(commandsTextbox.Text);
                outputFile.Close();

                saved = true;
            } catch (Exception)
            {
                if (MessageBox.Show("You din't save the file, save it now?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    goto startSaveLabel;
                } else { }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saved == true)
            {
                try
                {
                    StreamWriter outputFile = new StreamWriter(saveFD.FileName);
                    outputFile.Write(commandsTextbox.Text);
                    outputFile.Close();

                    saved = true;
                } catch (Exception) { }
            } else
            {
                saveAsToolStripMenuItem_Click(null, null);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
        startLabel:
            openFD.FileName = "";
            openFD.Filter = "ALL (*.*)|*.*|TEXT (*.txt)|*.txt|SML (*.sml)|*.sml";
            openFD.InitialDirectory = "C:\\";
            openFD.ShowDialog();

            try { 
                StreamReader newFileStream = new StreamReader(openFD.FileName);
                saved = false;

                commandsTextbox.Text = newFileStream.ReadToEnd();
                newFileStream.Close();
            } catch (Exception) {
                if (MessageBox.Show("You didn't select a file, select one now ?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    goto startLabel;
                } else { }
            }
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 documentationForm = new Form2();
            documentationForm.Show();
        }

        private void GarbageCollectorTimer_Tick(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void commandsTextbox_TextChanged(object sender, EventArgs e)
        {
            sizeLabel.Text = "Size: {} bytes".Replace("{}", commandsTextbox.Text.Length.ToString());
        }
    }
}