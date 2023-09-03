using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Parent folder path containing subfolders with images and videos
        string parentFolderPath = @"";

        // Destination folder where images and videos will be copied
        string destinationFolderPath = @"";

        // Ensure the destination folder exists
        Directory.CreateDirectory(destinationFolderPath);

        // Array of valid image and video extensions
        string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".mp4", ".avi", ".mkv" };

        // Recursively search for files in subfolders of the parent folder
        ExtractImagesAndVideos(parentFolderPath, destinationFolderPath, validExtensions);

        Console.WriteLine("Extraction complete.");
    }

    static void ExtractImagesAndVideos(string sourceFolderPath, string destinationFolderPath, string[] validExtensions)
    {
        foreach (string subfolder in Directory.GetDirectories(sourceFolderPath))
        {
            // Recursively process subfolders
            ExtractImagesAndVideos(subfolder, destinationFolderPath, validExtensions);
        }

        // Process files in the current folder
        foreach (string filePath in Directory.GetFiles(sourceFolderPath))
        {
            string extension = Path.GetExtension(filePath).ToLower();

            // Check if the file has a valid image or video extension
            if (validExtensions.Contains(extension))
            {
                try
                {
                    // Generate a unique filename in the destination folder
                    string destinationFilePath = Path.Combine(destinationFolderPath, GetUniqueFileName(destinationFolderPath, Path.GetFileName(filePath)));

                    // Copy the file to the destination folder
                    File.Copy(filePath, destinationFilePath);

                    Console.WriteLine($"Copied: {filePath} to {destinationFilePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying {filePath}: {ex.Message}");
                }
            }
        }
    }

    // Generates a unique filename by appending a number to the base filename
    static string GetUniqueFileName(string folderPath, string baseFileName)
    {
        int number = 1;
        string fileName = baseFileName;

        while (File.Exists(Path.Combine(folderPath, fileName)))
        {
            string extension = Path.GetExtension(baseFileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(baseFileName);

            fileName = $"{fileNameWithoutExtension}_{number}{extension}";
            number++;
        }

        return fileName;
    }
}