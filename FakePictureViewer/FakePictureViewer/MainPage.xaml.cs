using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FakePictureViewer.Resources;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Media;

/*
 *  Copyright 2013 Marc-André Bär
 
 *  This project is for educational use only.
 
 *  This file is part of FakePasswordStorage.
    FakePictureViewer is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    FakePictureViewer is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with FakePictureViewer If not, see <http://www.gnu.org/licenses/>.
  
 */
namespace FakePictureViewer
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();

            // Beispielcode zur Lokalisierung der ApplicationBar
            //BuildLocalizedApplicationBar();

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MediaLibrary library = new MediaLibrary();
            IEnumerator<Picture> pics = library.Pictures.GetEnumerator();
            int counter = 0;
            string[] pws = new string[10];
            while (pics.MoveNext())
            {
                if (pics.Current.Name.Substring(0, 2) == "XX")
                {
                    pws[counter] = pics.Current.Name;
                    counter++;
                }
            }
            String result = "";
            for(int i = 0; i < counter; i++)
            {
                string[] words = Regex.Split(pws[i].Remove(0,2), "ABC");
                string site = words[0];
                string name = words[1];
                string pw = words[2].Remove(words[2].Length - 4, 4);
                site = encrypt(site);
                name = encrypt(name);
                pw = encrypt(pw);
                result += "Website: " + site + "\r\nName: " + name + "\r\nPassword: " + pw + "\r\n";
            }
            textbox.Text = result;
        }
        private String encrypt(String chiper)
        {
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