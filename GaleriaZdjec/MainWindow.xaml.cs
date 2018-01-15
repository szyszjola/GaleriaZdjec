using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace GaleriaZdjec
{

    public partial class MainWindow : Window
    {

        ScaleTransform st = new ScaleTransform(3, 3);
        Zdjecia zdjecia = new Zdjecia();
        object dummyNode = null;

        #region Zarzadzanie oknem Window

        public MainWindow()
        {
            InitializeComponent();
            zdjecia.ItemsUpdated += delegate { this.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(Odswiez)); };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (MessageBox.Show("Czy na pewno chcesz zakończyć pracę Galerii zdjęć?",
            "Irytujący komunikat", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.No)
                e.Cancel = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Persist the list of favorites
            IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForAssembly();
            using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("myFile", FileMode.Create, f))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                foreach (TreeViewItem item in UlubioneFolderyItems.Items)
                {
                    writer.WriteLine(item.Tag as string);
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Retrieve the list of favorites
            IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForAssembly();
            using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("myFile", FileMode.OpenOrCreate, f))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line = reader.ReadLine();
                while (line != null)
                {

                    DodajUlubione(line);
                    line = reader.ReadLine();
                }
            }

            // At least have the user's Pictures folder as a favorite if nothing else
            if (!UlubioneFolderyItems.HasItems)
            {
                DodajUlubione(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            }

         (UlubioneFolderyItems.Items[0] as TreeViewItem).IsSelected = true;
        }

        #endregion

        #region Tree View

        private void TreeViewFoldery_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Odswiez();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string s in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;
                item.Items.Add(dummyNode);
                item.Expanded += new RoutedEventHandler(folder_Expanded);
                FolderyItems.Items.Add(item);
            }
        }

        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);
                    }
                }
                catch (UnauthorizedAccessException) { }
            }
        }

        private void PokazZdjecie(bool? pokazFixBar)
        {
            string nazwaPliku = (ObrazkiBox.SelectedItem as ListBoxItem).Tag as string;


        }

        private void DodajZdjecieDoFolderu(string folder)
        {
            try
            {
                foreach (string s in Directory.GetFiles(folder, "*jpg"))
                {
                    Zdjecie zdjecie = new Zdjecie(s);
                    zdjecia.Add(zdjecie);

                    //Tworzenie ListBoxItem z Obrazem jako zawartością
                    ListBoxItem item = new ListBoxItem();
                    item.Padding = new Thickness(3, 8, 3, 8);
                    item.MouseDoubleClick += delegate { PokazZdjecie(false); };
                    TransformGroup tg = new TransformGroup();
                    tg.Children.Add(st);
                    tg.Children.Add(new RotateTransform());
                    item.LayoutTransform = tg;
                    item.Tag = s;

                    Image obrazek = new Image();
                    obrazek.Height = 35;

                    Uri uri = new Uri(s);
                    BitmapDecoder bd = BitmapDecoder.Create(uri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);
                    if (bd.Frames[0].Thumbnail != null)
                    {
                        obrazek.Source = bd.Frames[0].Thumbnail;
                    }
                    else
                        obrazek.Source = new BitmapImage(uri);

                    //Tworzenie toolTipa dla itemków
                    Image toolTipImage = new Image();
                    toolTipImage.Source = bd.Frames[0].Thumbnail;

                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Text = System.IO.Path.GetFileName(s);
                    TextBlock textBlock2 = new TextBlock();
                    textBlock2.Text = zdjecie.DateTime.ToString();

                    StackPanel sp = new StackPanel();
                    sp.Children.Add(toolTipImage);
                    sp.Children.Add(textBlock1);
                    sp.Children.Add(textBlock2);

                    item.ToolTip = sp;
                    item.Content = obrazek;

                    ObrazkiBox.Items.Add(item);

                }
            }
            catch (UnauthorizedAccessException) { }
            catch (IOException) { }
        }

        private void Odswiez()
        {
            try
            {
                this.Cursor = Cursors.Wait;

                ObrazkiBox.Items.Clear();
                zdjecia.Clear();

                if (TreeViewFoldery.SelectedItem == UlubioneFolderyItems)
                {
                    foreach (TreeViewItem item in UlubioneFolderyItems.Items)
                    {
                        DodajZdjecieDoFolderu(item.Tag as string);
                    }
                    MenuDodaj.IsEnabled = false;
                }
                else if (TreeViewFoldery.SelectedItem != FolderyItems)
                {
                    string folder = (TreeViewFoldery.SelectedItem as TreeViewItem).Tag as string;
                    DodajZdjecieDoFolderu(folder);

                    //Zaktualizuj tekst ulubionych nie zależnie od aktualnie ulubionego folderu
                    MenuDodaj.IsEnabled = true;
                    foreach (TreeViewItem item in UlubioneFolderyItems.Items)
                    {
                        if (item.Header as string == folder)
                        {
                            MenuDodaj.Header = "Usuń dany folder z ulubionych";
                            return;
                        }
                    }
                    MenuDodaj.Header = "Dodaj bieżący folder do ulubionych";
                }
            }
            catch { }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void DodajUlubione(string folder)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = folder;
            item.Tag = folder;
            UlubioneFolderyItems.Items.Add(item);
        }

        private void UsunUlubione(string folder)
        {
            for (int i = 0; i < UlubioneFolderyItems.Items.Count; i++)
            {
                if ((UlubioneFolderyItems.Items[i] as TreeViewItem).Header as string == folder)
                {
                    UlubioneFolderyItems.Items.RemoveAt(i);
                    break;
                }
            }
        }
        #endregion

        #region Menu
        private void MenuKoniec_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuNapraw_Click(object sender, RoutedEventArgs e)
        {
            PokazZdjecie(true);
        }

        private void MenuDrukuj_Click(object sender, RoutedEventArgs e)
        {
            string nazwaPliku = (ObrazkiBox.SelectedItem as ListBoxItem).Tag as string;
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(nazwaPliku, UriKind.RelativeOrAbsolute));
            PrintDialog pd = new PrintDialog();
            //if(pd.ShowDialog() == true)
               // pd.PrintVisual(image.GetFileName())
          //na tym skończyłam na piątek

        }

        private void MenuEdycja_Click(object sender, RoutedEventArgs e)
        {
            string nazwaPliku = (ObrazkiBox.SelectedItem as ListBoxItem).Tag as string;
            System.Diagnostics.Process.Start("mspaint.exe", nazwaPliku);
        }

        private void MenuOdwiez_Click(object sender, RoutedEventArgs e)
        {
            Odswiez();
        }

        private void MenuZmienNazwe_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void MenuUsun_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.dll ogolnie
        }

        private void MenuDodaj_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void ObrazkiBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                MenuUsun.IsEnabled = false;
                MenuZmienNazwe.IsEnabled = false;
                MenuNapraw.IsEnabled = false;
                MenuDrukuj.IsEnabled = false;
                MenuEdycja.IsEnabled = false;
                ///dokoncz

            }
            else
            {
                MenuUsun.IsEnabled = true;
                MenuZmienNazwe.IsEnabled = true;
                MenuNapraw.IsEnabled = true;
                MenuDrukuj.IsEnabled = true;
                MenuEdycja.IsEnabled = true;
            }
        }


    }
}
