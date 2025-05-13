using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WpfBookRentalShop01.Helpers;
using WpfBookRentalShop01.Models;

namespace WpfBookRentalShop01.ViewModels
{
    public partial class BooksViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialogCoordinator;
        public ObservableCollection<KeyValuePair<string, string>> Divisions { get; set; }

        private ObservableCollection<Books> _books;
        public ObservableCollection<Books> Books
        {
            get => _books;
            set => SetProperty(ref _books, value);
        }

        private Books _selectedBooks;

        public Books SelectedBooks
        {
            get => _selectedBooks;
            set
            {
                SetProperty(ref _selectedBooks, value);
                _isUpdate = true;
            }
        }

        private bool _isUpdate;

        public BooksViewModel(IDialogCoordinator coordinator)
        {
            this.dialogCoordinator = coordinator;
            InitVariable();
            LoadControlFromDb();
            LoadGridFromDb();
        }

        private void LoadControlFromDb()
        {
            // 1. 연결문자열(DB연결문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용쿼리
            string query = "SELECT division, names FROM divtbl";
            // Dictionary나 KeyValuePair 둘다 상관없음
            ObservableCollection<KeyValuePair<string, string>> divisions = new ObservableCollection<KeyValuePair<string, string>>();

            // 3. DB 연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))    // close문을 자동으로 실행
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();   // 데이터를 가져올때 - ExecuteReader 컨트롤 엔터로 실행하는 것

                    while (reader.Read())
                    {
                        var division = reader.GetString("division");
                        var names = reader.GetString("names");

                        divisions.Add(new KeyValuePair<string, string>(division, names));
                    }
                }
                catch (MySqlException ex)
                {
                    // 나중에...                
                }
            }   // conn.Close() 자동발생

            Divisions = divisions;
            OnPropertyChanged(nameof(Divisions)); // Divisions 속성값이 변경됨!
        }

        private void InitVariable()
        {
            SelectedBooks = new Books
            {
                Idx = 0,
                Author = string.Empty,
                Division = string.Empty,
                Dnames = string.Empty,
                Names = string.Empty,
                Isbn = string.Empty,
                ReleaseDate = DateTime.Now,
                Price = 0
            };
        }

        [RelayCommand]
        public void SetInit()
        {
            InitVariable();
        }

        private async void LoadGridFromDb()
        {
            try
            {
                string query = @"SELECT b.Idx, b.Author, b.Division, b.Names, b.ReleaseDate, b.ISBN, b.Price,
	                           d.Names AS dNames
                                  FROM bookstbl As b, divtbl AS d
                                 WHERE b.Division = d.Division
                                Order by b.idx;";
                ObservableCollection<Books> books = new ObservableCollection<Books>();

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var idx = reader.GetInt32("Idx");
                        var author = reader.GetString("Author");
                        var division = reader.GetString("Division");
                        var names = reader.GetString("Names");
                        var releasedate = reader.GetDateTime("ReleaseDate");
                        var isbn = reader.GetString("ISBN");
                        var price = reader.GetInt32("Price");
                        var dnames = reader.GetString("dNames");

                        books.Add(new Books
                        {
                            Idx = idx,
                            Author = author,
                            Division = division,
                            Names = names,
                            ReleaseDate = releasedate,
                            Isbn = isbn,
                            Price = price,
                            Dnames = dnames,
                        });
                    }
                }
                Books = books;
            }
            catch (Exception ex)
            {
                Common.LOGGER.Error(ex.Message);
                await this.dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
            }
            Common.LOGGER.Info("책 데이터 로드");
        }
    }
}
