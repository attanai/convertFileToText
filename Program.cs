using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace convertFileToText
{
    class Program
    {
        //List variables to store file paths and string.
        public static string originalFile = string.Empty;
        public static string newFile = string.Empty;
        public static string b64Text = string.Empty;


        static void Main(string[] args)
        {
            //Load arguments if they exist.
            try
            {
                if (args != null)
                {
                    try
                    {
                        originalFile = args[0];
                        try
                        {
                            newFile = args[1];
                        }
                        catch { }
                    }
                    catch { }
                }
            }
            catch { }

            //Instructions
            if (originalFile.Trim(new char[] { ' ', '/', '\\', '-' }) == "?" | originalFile.Trim(new char[] { ' ', '/', '\\', '-' }).ToUpper() == "H" | originalFile.Trim(new char[] { ' ', '/', '\\', '-' }).ToUpper() == "HELP")
            {
                string help =
                    "This program will convert a file into base64 text and save it to a textfile.\n\n"
                    + "\tSyntax: convert64 [File to be converted] [Name of new file]\n"
                    + "\tExample:\n"
                    + "\t\tconvert64\n"
                    + "\t\tconvert64 myfile.exe\n"
                    + "\t\tconvert64 myFile.exe myFile.txt\n"
                    + "\t\tconvert64 \"C:\\folder\\myFile.exe\" \"C:\\otherFolder\\newFileName.txt\"\n\n\n\n"
                    + "Press any key to continue...";
                exit(help);
            }

            //If the first filename is not in place, ask for it.
            if (originalFile == string.Empty)
            {
                Console.WriteLine("Please enter a filename to convert.");
                originalFile = Console.ReadLine();
            }

            //Make sure that originalFile is readable
            originalFile = originalFile.Trim(new char[] { ' ', '\'', '\"' });

            //Make sure that newFile exists
            if (newFile == string.Empty)
            {
                newFile = originalFile + ".txt";
            }

            //Load the original file into base64 string
            loadFile(originalFile);

            //save base64 text to newFile text file
            try
            {
                File.WriteAllText(newFile, b64Text);
                exit("File transcoded successfully.");
            }
            catch(Exception e) 
            {
                exit("Could not save file:\n" + e.InnerException.ToString());
            }

            
        }

        /// <summary>
        /// Encodes file as base64 string and saves to global variable.
        /// </summary>
        /// <param name="file">path of file to be encoded</param>
        /// <returns></returns>
        public static void loadFile(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    //Convert File to Base64
                    FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    byte[] filebytes = new byte[fs.Length];
                    fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                    b64Text = Convert.ToBase64String(filebytes);
                }
                catch (Exception e)
                {
                    exit("Could not encode file:\n" + e.InnerException.ToString());
                }
            }
            else
            {
                exit("Could not locate file \"" + originalFile + "\"");
            }
        }
        

        public static void exit(string Message)
        {
            Console.WriteLine(Message);
            Console.ReadLine();
            exit();
        }
        public static void exit()
        {
            Environment.Exit(0);
        }
        
        
        
        #region decoding methods...
        //These methods are used to convert the text back into a usable file.
        
        /// <summary>
        /// Decodes file from base64 text and saves it in specified location.
        /// </summary>
        /// <param name="base64string">String to be decoded</param>
        /// <param name="outputFolder">The path to the folder location of new file</param>
        /// <param name="fileName">The name of the new file</param>
        /// <returns></returns>
        public static bool decodeFile(string base64string, string outputFolder, string fileName)
        {
            try
            {
                if (outputFolder == "" | outputFolder == null)
                {
                    outputFolder = Application.StartupPath;
                }


                //Convert base64 to file
                byte[] filebytes = Convert.FromBase64String(base64string);
                File.WriteAllBytes(outputFolder + "\\" + fileName, filebytes);
                return true;
            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        /// Decodes file from base64 text and saves it in current directory.
        /// </summary>
        /// <param name="base64string">String to be decoded</param>
        /// <param name="fileName">The name of the new file</param>
        /// <returns></returns>
        public static bool decodeFile(string base64string, string fileName)
        {
            return decodeFile(base64string, null, fileName);
        }
        
        
        #endregion
        
    }
}
