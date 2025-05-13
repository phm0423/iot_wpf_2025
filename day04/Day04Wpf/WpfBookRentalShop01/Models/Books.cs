using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBookRentalShop01.Models
{
    public class Books: ObservableObject
    {
        private int _idx;
        private string _author;
        private string _division;
        private string _Dnames;
        private string _names;
        private string _isbn;
        private DateTime _releasedate;
        private int _price;

        public int Idx
        {
            get => _idx;
            set => SetProperty(ref _idx, value);
        }
        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }
        public string Division
        {
            get => _division;
            set => SetProperty(ref _division, value);
        }
        public string Dnames
        {
            get => _Dnames;
            set => SetProperty(ref _Dnames, value);
        }
        public string Names
        {
            get => _names;
            set => SetProperty(ref _names, value);
        }
        public string Isbn
        {
            get => _isbn;
            set => SetProperty(ref _isbn, value);
        }
        public DateTime ReleaseDate
        {
            get => _releasedate;
            set => SetProperty(ref _releasedate, value);
        }
        public int Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }
    }
}
