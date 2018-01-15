using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaleriaZdjec
{
    class Zdjecia : Collection<Zdjecie>
    {
        Dictionary<string, FileSystemWatcher> watchers = new Dictionary<string, FileSystemWatcher>();

        public event EventHandler ItemsUpdated; //moja nazwa

        protected override void ClearItems()
        {
            base.ClearItems();
            watchers.Clear();
        }

        protected override void InsertItem(int index, Zdjecie item)
        {
            base.InsertItem(index, item);
            if (!watchers.ContainsKey(item.Sciezka))
            {
                FileSystemWatcher watcher = new FileSystemWatcher(item.Sciezka, "*.jpg");
                watcher.EnableRaisingEvents = true;
                watcher.Created += new System.IO.FileSystemEventHandler(UtworzoneZdjecie);
                watcher.Deleted += new System.IO.FileSystemEventHandler(UsunieteZdjecie);
                watcher.Renamed += new System.IO.RenamedEventHandler(ZmienionaNazwaZdjecia);
                watchers.Add(item.Sciezka, watcher);
            }
        }

        void UtworzoneZdjecie(object sender, System.IO.FileSystemEventArgs e)
        {
            Items.Add(new Zdjecie(e.FullPath));
            if (ItemsUpdated != null)
                ItemsUpdated(this, new EventArgs());
        }

        void UsunieteZdjecie(object sender, System.IO.FileSystemEventArgs e)
        {
            int index = -1;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].PelnaSciezka == e.FullPath)
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0)
                Items.RemoveAt(index);

            if (ItemsUpdated != null)
                ItemsUpdated(this, new EventArgs());
        }

        void ZmienionaNazwaZdjecia(object sender, System.IO.RenamedEventArgs e)
        {
            int index = -1;
            for (int i = 0; i < Items.Count; i++)
            {
                if(Items[i].PelnaSciezka == e.OldFullPath)
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0)
                Items[index] = new Zdjecie(e.FullPath);

            if(ItemsUpdated != null)
            {
                ItemsUpdated(this, new EventArgs());
            }
        }
    }
}
