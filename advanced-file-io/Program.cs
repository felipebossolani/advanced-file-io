using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace advanced_file_io
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Creator: Felipe Bossolani - fbossolani[at]gmail.com");
            Console.WriteLine(@"Examples based on: http://returnsmart.blogspot.com/2015/10/mcsd-programming-in-c-part-15-70-483.html");
            Console.WriteLine("Choose a Thread Method: ");
            Console.WriteLine("01- File");
            Console.WriteLine("02- FileInfo");
            Console.WriteLine("03- Path");
            Console.WriteLine("04- DriveInfo");
            Console.WriteLine("05- Directory");
            Console.WriteLine("06- DirectoryInfo");
            Console.WriteLine("07- Network - StreamReader");
            Console.WriteLine("08- Async IO Streams");

            int option = 0;
            int.TryParse(Console.ReadLine(), out option);

            switch (option)
            {
                case 1:
                    {
                        FileExample();
                        break;
                    }
                case 2:
                    {
                        FileInfoExample();
                        break;
                    }
                case 3:
                    {
                        PathExample();
                        break;
                    }
                case 4:
                    {
                        DriveInfoExample();
                        break;
                    }
                case 5:
                    {
                        DirectoryExample();
                        break;
                    }
                case 6:
                    {
                        DirectoryInfoExample();
                        break;
                    }
                case 7:
                    {
                        NetworkStreamReaderExample();
                        break;
                    }
                case 8:
                    {
                        var htmls = AsyncIOStreamExample();
                        foreach(var s in htmls.Result)
                        {
                            Console.WriteLine(s);
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid option...");
                        break;
                    }
            }           
        }

        private static async Task<string[]> AsyncIOStreamExample()
        {
            HttpClient client = new HttpClient();   // add System.Net.Http reference to your project

            Stopwatch clock = new Stopwatch();
            clock.Start();

            string microsoft = await client.GetStringAsync("http://www.microsoft.com");
            string msdn = await client.GetStringAsync("http://msdn.microsoft.com");
            string google = await client.GetStringAsync("http://www.google.com");

            clock.Stop();
            Console.WriteLine("Took {0:hh\\:mm\\:ss} seconds", clock.Elapsed);

            clock.Reset();
            clock.Start();

            Task<String> microsoftTask = client.GetStringAsync("http://www.microsoft.com");            
            Task<String> msdnTask = client.GetStringAsync("http://msdn.microsoft.com");
            Task<String> googleTask = client.GetStringAsync("http://www.google.com");
            await Task.WhenAll(microsoftTask, msdnTask, googleTask);

            clock.Stop();
            Console.WriteLine("Took {0:hh\\:mm\\:ss} seconds", clock.Elapsed);

            Console.WriteLine(microsoftTask.ToString());

            List<String> list = new List<string>()
            {
                microsoftTask.Result,
                googleTask.Result,
                msdnTask.Result
            };            
            return list.ToArray();
        }

        private static void NetworkStreamReaderExample()
        {
            Console.WriteLine("NetworkStreamReaderExample Example");
            WebRequest request = WebRequest.Create("http://www.google.com");
            WebResponse response = request.GetResponse();

            Console.WriteLine("Getting html string from www.google.com:");
            StreamReader responseStream = new StreamReader(response.GetResponseStream());
            string responseText = responseStream.ReadToEnd();

            Console.WriteLine(responseText);
            response.Close();
        }

        private static void DirectoryExample()
        {
            Console.WriteLine("Directory Example");

            string newFolder = "70-483";

            Console.WriteLine($@"Creating a new folder called {newFolder} into {path} directory");
            Directory.CreateDirectory(path + @"\" + newFolder);

            Console.WriteLine($@"Listing all files in {path} directory");
            
            foreach(var f in Directory.GetFiles(path))
            {
                Console.WriteLine(f);
            }

        }

        private static void DirectoryInfoExample()
        {
            Console.WriteLine("DirectoryInfo Example");

            string newPath = DateTime.Now.ToString("yyyyMMdd");

            Console.WriteLine($@"Creating a new subdirectory in {path} called {newPath}...");
            DirectoryInfo di = new DirectoryInfo(path);
            di.CreateSubdirectory(newPath);
            Console.WriteLine("Created");

            Console.WriteLine($@"Listing all subdrectories in {path}");
            foreach(var f in di.GetDirectories())
            {
                Console.WriteLine(f);
            }

            Console.WriteLine($@"Listing subdrectories that starts with letter S in {path}");
            foreach (var f in di.GetDirectories("S*"))
            {
                Console.WriteLine(f);
            }

            Console.WriteLine($@"Listing all files in {path} folder");
            foreach (var f in di.GetFiles())
            {
                Console.WriteLine(f);
            }

            Console.WriteLine($@"Listing all files that starts with letter T in {path} folder");
            foreach (var f in di.GetFiles("t*.*"))
            {
                Console.WriteLine(f);
            }

        }

        private static void DriveInfoExample()
        {
            Console.WriteLine("DriveInfo Example");

            Console.WriteLine("Listing drivers in your computer:");
            foreach(var d in DriveInfo.GetDrives())
            {
                Console.WriteLine($@"Letter: {d.Name}, DriveType: {d.DriveType}, VolumeLabel {d.VolumeLabel}, DriveFormat: {d.DriveFormat}");
            }
        }

        private static void PathExample()
        {
            Console.WriteLine("Path Examples");

            string p = @"c:\temp";
            string f = @"test.txt";

            Console.WriteLine($@"Path.Combine            {Path.Combine(p, f)}");
            Console.WriteLine($@"Path.GetDirectoryName   {Path.GetDirectoryName(file)}");
            Console.WriteLine($@"Path.GetExtension       {Path.GetExtension(file)}");
            Console.WriteLine($@"Path.GetFileName        {Path.GetFileName(file)}");
            Console.WriteLine($@"Path.GetPathRoot        {Path.GetPathRoot(file)}");
        }

        static string path = @"c:\temp";
        static string file = path + @"\test.txt";
        static string fileBackup = path + @"\testBackup.txt";
        static string fileBackup2 = path + @"\testBackup2.txt";

        private static void FileExample()
        {
            Console.WriteLine("File Examples");
            
            Console.WriteLine($@"Checking if {file} exists...");
            if (File.Exists(file)) {
                Console.WriteLine($@"{file} exists, creating a backup copy...");
                File.Copy(file, fileBackup, true);

                Console.WriteLine($@"{file} copied to {fileBackup}");
                Console.WriteLine($@"Now, I will move {fileBackup} to {fileBackup2}");

                if (File.Exists(fileBackup2)) File.Delete(fileBackup2);
                File.Move(fileBackup, fileBackup2);

                Console.WriteLine($@"Moved {fileBackup} to {fileBackup2}");
                Console.WriteLine($@"Now, I will delete {file}");

                File.Delete(file);
                Console.WriteLine($@"{file} Deleted");
            }
        }

        private static void FileInfoExample()
        {
            Console.WriteLine("FileInfo Examples");
                        
            string file = @"c:\temp\test.txt";
            
            Console.WriteLine($@"Instantiate FileInfo({file})");
            FileInfo fi = new FileInfo(file);

            Console.WriteLine($@"Checking if {file} exists...");
            if (fi.Exists)
            {
                Console.WriteLine($@"{file} exists, creating a backup copy...");
                fi.CopyTo(fileBackup, true);

                Console.WriteLine($@"{file} copied to {fileBackup}");
                Console.WriteLine($@"Now, I will move {fileBackup} to {fileBackup2}");

                if (File.Exists(fileBackup2)) File.Delete(fileBackup2);
                fi.MoveTo(fileBackup2);

                Console.WriteLine($@"Moved {file} to {fileBackup2}");
                Console.WriteLine($@"Now, I will delete {file}");

                fi.Delete();
                Console.WriteLine($@"{file} Deleted");
            }
        }
    }
}
