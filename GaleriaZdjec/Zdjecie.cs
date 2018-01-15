using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaleriaZdjec
{
    class Zdjecie
    {
        string nazwa;
        DateTime dateTime;
        string rozmiar;
        string sciezka;

        public Zdjecie(string nazwaPliku)
        {
            FileInfo info = new FileInfo(nazwaPliku);
            rozmiar = (info.Length / 1024).ToString("NO") + " KB";
            dateTime = info.LastWriteTime;
            nazwa = info.Name;
            sciezka = info.DirectoryName;
        }

        public string Nazwa
        {
            get { return nazwa; }
        }

        public DateTime DateTime
        {
            get { return dateTime; }
        }

        public string Rozmiar
        {
            get { return rozmiar; }
        }

        public string Sciezka
        {
            get { return sciezka; }
        }

        public string PelnaSciezka
        {
            get { return System.IO.Path.Combine(Sciezka, Nazwa); }
        }

        public override string ToString()
        {
            return PelnaSciezka;
        }
    }
}
