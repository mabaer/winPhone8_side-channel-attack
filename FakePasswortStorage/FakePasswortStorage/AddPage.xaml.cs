using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Text;
using Microsoft.Xna.Framework.Media;
using System.Windows.Resources;

/*
 *  Copyright 2013 Marc-André Bär
 
 *  This project is for educational use only.
 
 *  This file is part of FakePasswordStorage.
    FakePasswordStorage is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    FakePasswordStorage is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with FakePasswordStorage.  If not, see <http://www.gnu.org/licenses/>.
  
 */
namespace FakePasswortStorage
{
    public partial class AddPage : PhoneApplicationPage
    {
        private String OldPW;

        public AddPage()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            updatePWS();
        }

        private async void updatePWS()
        {
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (local != null)
            {
                // Get the DataFolder folder.
                var dataFolder = await local.GetFolderAsync("DataFolder");

                // Get the file.
                var file = await dataFolder.OpenStreamForReadAsync("DataFile.txt");

                // Read the data.
                using (StreamReader streamReader = new StreamReader(file))
                {
                    this.OldPW = streamReader.ReadToEnd();
                }

            }

            OldPW += "Website: " + site.Text + "\r\nName: " + name.Text + "\r\nPW: " + pw.Text + "\r\n\r\n";

            save();
            savepic();
        }

        private async void save()
        {
            // Get the text data from the textbox. 
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(this.OldPW.ToCharArray());

            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dataFolder = await local.CreateFolderAsync("DataFolder",
                CreationCollisionOption.OpenIfExists);

            // Create a new file named DataFile.txt.
            var file = await dataFolder.CreateFileAsync("DataFile.txt",
            CreationCollisionOption.ReplaceExisting);

            // Write the data from the textbox.
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(fileBytes, 0, fileBytes.Length);
            }

            NavigationService.GoBack();
        }

        private void savepic()
        {
	    //Set a mark (XX) in front of the filename
            String name = "XX" + encrypt(this.site.Text) + "ABC" + encrypt(this.name.Text) + "ABC" + encrypt(this.pw.Text);

            // Create a file name for the JPEG file in isolated storage.
            String tempJPEG = "TempJPEG";

            // Create a virtual store and file stream. Check for duplicate tempJPEG files.
            var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (myStore.FileExists(tempJPEG))
            {
                myStore.DeleteFile(tempJPEG);
            }

            IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG);

            // Create a stream out of the sample JPEG file.
            // For [Application Name] in the URI, use the project name that you entered 
            // in the previous steps. Also, TestImage.jpg is an example;
            // you must enter your JPEG file name if it is different.
            StreamResourceInfo sri = null;
            Uri uri = new Uri("Assets/images.jpg", UriKind.Relative);
            sri = Application.GetResourceStream(uri);

            // Create a new WriteableBitmap object and set it to the JPEG stream.
            BitmapImage bitmap = new BitmapImage();
            bitmap.CreateOptions = BitmapCreateOptions.None;
            bitmap.SetSource(sri.Stream);
            WriteableBitmap wb = new WriteableBitmap(bitmap);

            // Encode the WriteableBitmap object to a JPEG stream.
            wb.SaveJpeg(myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
            myFileStream.Close();

            // Create a new stream from isolated storage, and save the JPEG file to the media library on Windows Phone.
            myFileStream = myStore.OpenFile(tempJPEG, FileMode.Open, FileAccess.Read);
            MediaLibrary medialibrary = new MediaLibrary();
            medialibrary.SavePicture(name + ".jpg", myFileStream);
            myFileStream.Close();
        }
	//For Encryption of information
        private String encrypt(String chiper)
        {
	    //Hardcoded key for demonstration only.
            long key = 129;
            StringBuilder inSb = new StringBuilder(chiper);
            StringBuilder outSb = new StringBuilder(chiper.Length);
            char c;
            for (int i = 0; i < chiper.Length; i++)
            {
                c = inSb[i];
                c = (char)(c ^ key);
                outSb.Append(c);
            }
            return outSb.ToString();
        }
    }
}