using AddressBook_2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AddressBook_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Lista
        public ObservableCollection<Contact> contacts;
        //sökväg där filen ska sparas
        public string filePath = @$"C:\Users\Fadi\Desktop\\AddressBook-2.json";
        


        public MainWindow()
        {   
            //skapar listan
            InitializeComponent();
            
            try
            {               
                contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(Read(filePath));
            }
            catch
            {
                using (File.Create(filePath))
                contacts = new ObservableCollection<Contact>();
                
            }
            lv_Contacts.ItemsSource = contacts;
        }

        //Läser in vår sparade fil
        private string Read(string filePath)
        {
            using var sr = new StreamReader(filePath);
            return sr.ReadToEnd();
        }

        //sparar fil
        private void Save(string filePath, string content)
        {
            var text = Read(filePath);
            text += content;

            using var sw = new StreamWriter(filePath);
            sw.WriteLineAsync(content);
        }

        //skapar en ny kontakt och sparar fil
        public void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            var contact = contacts.FirstOrDefault(x => x.PhoneNumber == tb_PhoneNumber.Text);
            if (contact == null)
            {
                contacts.Add(new Contact
                {
                    FirstName = tb_FirstName.Text,
                    LastName = tb_LastName.Text,
                    PhoneNumber = tb_PhoneNumber.Text,
                    Email = tb_Email.Text,
                    Address = tb_Address.Text,
                    PostalCode = tb_PostalCode.Text,
                    City = tb_City.Text,
                });
            }
            else
            {
                MessageBox.Show("Det finns redan en kontakt med samma telefonnummer");
            }

            ClearFields();
            Save(filePath, JsonConvert.SerializeObject(contacts));
        }

        //Tömmer fälten 
        private void ClearFields()
        {
            tb_FirstName.Text = "";
            tb_LastName.Text = "";
            tb_PhoneNumber.Text = "";
            tb_Email.Text = "";
            tb_Address.Text = "";
            tb_PostalCode.Text = "";
            tb_City.Text = "";
        }

        //Raderar en kontakt och sparar fil
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {

            var button = sender as Button;
            var contact = (Contact)button!.DataContext;
            contacts.Remove(contact);
            ClearFields();

            btn_Update.Visibility = Visibility.Collapsed;
            btn_Add.Visibility = Visibility.Visible;

            Save(filePath, JsonConvert.SerializeObject(contacts));
        }

        //Fyller i fälten om en sparad kontakt när du klickar på en kontakt
        private void lv_Contacts_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            try
            {

                var contact = (Contact)lv_Contacts.SelectedItems[0]!;

                btn_Update.Visibility = Visibility.Visible;
                btn_Add.Visibility = Visibility.Collapsed;
               
                if(contact != null)
                {

                tb_FirstName.Text = contact.FirstName;
                tb_LastName.Text = contact.LastName;
                tb_PhoneNumber.Text = contact.PhoneNumber;
                tb_Email.Text = contact.Email;
                tb_Address.Text = contact.Address;
                tb_PostalCode.Text = contact.PostalCode;
                tb_City.Text = contact.City;
                }
                

            }
            catch
            {
                
            }
            
        }
        // uppdaterar en kontakt och sparar fil
        public void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            
            var contact = (Contact)lv_Contacts.SelectedItems[0]!;
            


            var index = contacts.IndexOf(contact);

           
            contacts[index].FirstName = tb_FirstName.Text;
            contacts[index].LastName = tb_LastName.Text;
            contacts[index].Email = tb_Email.Text;
            contacts[index].PhoneNumber = tb_PhoneNumber.Text;
            contacts[index].Address = tb_Address.Text;
            contacts[index].PostalCode = tb_PostalCode.Text;
            contacts[index].City = tb_City.Text;
            
            lv_Contacts.Items.Refresh();

            btn_Update.Visibility = Visibility.Collapsed;
            btn_Add.Visibility = Visibility.Visible;

            ClearFields();
            
            Save(filePath, JsonConvert.SerializeObject(contacts));
            
           
            
        }

       
   
    }
}
