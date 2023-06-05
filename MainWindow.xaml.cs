using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Xceed.Document.NET;
using Xceed.Words.NET;


namespace Contracts
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> signMappings = new Dictionary<string, string>()
        {
            { "Швецов Д.В.", "Швецова Дмитрия Вадимовича" },
            { "Швецова С.В.", "Швецовой Светланы Вадимовны" },
            { "Абрамова Е.Г.", "Абрамовой Елены Геннадьевны" },
            { "Косыгина Р.В.", "Косыгина Романа Васильевича" }
        };

        private Dictionary<string, string> authorityMappings = new Dictionary<string, string>()
        {
            { "613", "доверенности №613 от \"18\" октября 2022г." },
            { "824", "доверенности №824 от \"18\" декабря 2021г." },
            { "488", "доверенности №488 от \"29\" августа 2022г." },
            { "707", "доверенности №707 от \"29\" июля 2022г." }
        };

        private Dictionary<string, string> cityMappings = new Dictionary<string, string>()
        {
            { "МСК", "Москва" },
            { "САМ", "Самара" },
            { "НН", "Нижний Новгород" },
            { "СПБ", "Санкт-Петербург" },
            { "НСК", "Новосибирск" }
        };

        public string? Sign { get; private set; }
        public string? City { get; private set; }
        public string? Authority { get; private set; }
        public long Price { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBoxSelectedSign(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            Sign = GetSignBySelectedItem(selectedItem);
        }

        private string GetSignBySelectedItem(ComboBoxItem selectedItem)
        {
            string selectedItemContent = selectedItem?.Content?.ToString() ?? string.Empty;

            if (signMappings.ContainsKey(selectedItemContent))
            {
                return signMappings[selectedItemContent];
            }

            return string.Empty;
        }

        private void ComboBoxSelectedAuthority(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox1 = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
            Authority = GetAuthorityBySelectedItem(selectedItem);
        }

        private string GetAuthorityBySelectedItem(ComboBoxItem selectedItem)
        {
            string selectedItemContent = selectedItem?.Content?.ToString() ?? string.Empty;

            if (authorityMappings.ContainsKey(selectedItemContent))
            {
                return authorityMappings[selectedItemContent];
            }

            return string.Empty;
        }

        private void ComboBoxSelectedCity(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox1 = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
            
            City = GetCityBySelectedItem(selectedItem);
        }

        private string GetCityBySelectedItem(ComboBoxItem selectedItem)
        {
            string selectedItemContent = selectedItem?.Content?.ToString() ?? string.Empty;

            if (cityMappings.ContainsKey(selectedItemContent))
            {
                return cityMappings[selectedItemContent];
            }

            return string.Empty;
        }
        private void TextBoxPrice(object sender, TextChangedEventArgs e)
        {
            TextBox priceTextBox = (TextBox)sender;
            if (long.TryParse(priceTextBox.Text, out long priceValue))
            {
                Price = priceValue;
            }
           
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Open dialog window
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word Documents|*.docx";
            openFileDialog.Title = "Выберите файл шаблона";
            if (openFileDialog.ShowDialog() == true)
            {
                // Path to template
                string templateFilePath = openFileDialog.FileName;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

                // Receiving data from the user
                string fontName = "Arial";
                float fontSize = 10;
                string colorFont = "Black";

                // Open dialog window
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Word Documents|*.docx";
                saveFileDialog.Title = "Выберите путь для сохранения файла";
                saveFileDialog.FileName = "Мой документ.docx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    // Path to save the file
                    string outputFilePath = saveFileDialog.FileName;

                    // Open template
                    using (var doc = DocX.Load(templateFilePath))
                    {
                        var font = new Xceed.Document.NET.Font(fontName);
                        var fontColor = Color.FromName(colorFont);
                        MyConverter myConverter = new MyConverter();
                        string amountInWords = MyConverter.ConvertNumberToWords((long)Price);
                        // Replace values
                        ReplaceText(doc, "{date}", DateTime.Now.ToString("dd MMMM yyyy"), font, fontSize, fontColor);
                        ReplaceText(doc, "{sign}", Sign, font, fontSize, fontColor);
                        ReplaceText(doc, "{authority}", Authority, font, fontSize, fontColor);
                        ReplaceText(doc, "{city}", City, font, fontSize, fontColor);
                        ReplaceText(doc, "{Price}", Price + " (" + amountInWords + ")", font, fontSize, fontColor);
                        // TODO: need add converter from number to words
                       

                        // Save new file
                        doc.SaveAs(outputFilePath);
                    }

                    // Open updated contract
                    Process.Start(new ProcessStartInfo(outputFilePath) { UseShellExecute = true });

                    MessageBox.Show("Документ успешно создан и сохранен.");
                }
            }
        }

        private void ReplaceText(DocX document, string searchValue, string? newValue, Xceed.Document.NET.Font font, float fontSize, Color fontColor)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                var replaceTextOptions = new StringReplaceTextOptions()
                {
                    SearchValue = searchValue,
                    NewValue = newValue,
                    NewFormatting = new Formatting { FontFamily = font, Size = fontSize, FontColor = fontColor }
                };
                document.ReplaceText(replaceTextOptions);
            }
        }

        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            // Clear fields
            SignComboBox.SelectedIndex = -1; 
            AtterneyComboBox.SelectedIndex = -1; 
            CityComboBox.SelectedIndex = -1;
            
            ValuePrice.Text = string.Empty; 
                                
            Sign = null;
            City = null;
            Authority = null;

            MessageBox.Show("sdsdf.");
        }
    }
}
