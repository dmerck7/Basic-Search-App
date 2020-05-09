using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Configuration;

namespace BasicSearchApp.Services
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// This service loads document content from the hard drive
    /// </summary>
    public class ResourceService : IResourceService
    {
        private readonly IConfigurationService configurationService;

        public ResourceService(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        // Load document content from the resource folder
        public string LoadResource(string filename)
        {
            StringBuilder fileContents = new StringBuilder();
            string path = this.configurationService.GetAppSetting("ResourcePath") + @"\documents\";

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(path + filename + ".txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    string line = sr.ReadToEnd();
                    fileContents.AppendLine(line);
                }
            }
            catch (IOException e)
            {
                Debug.WriteLine("The file could not be read:");
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return fileContents.ToString();
        }



    }
}
