using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace wpf_laba_8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBox areaTextBox = new TextBox { Name = "AreaTextBox" };
        TextBox roomsTextBox = new TextBox { Name = "RoomsTextBox" };
        TextBox KitchenApart = new TextBox { Name = "KitchenApartment" };
        TextBox BathroomApart = new TextBox { Name = "BathroomApartment" };
            TextBox Toilet = new TextBox { Name = "ToiletApartment" };
            TextBox BasementApart = new TextBox { Name = "BasementApartment" };
            TextBox BalconyApart = new TextBox { Name = "BalconyApartment" };
            TextBox constructionYearTextBox = new TextBox { Name = "ConstructionYearTextBox" };
        TextBox materialTypeTextBox = new TextBox { Name = "MaterialTypeTextBox" };
        TextBox floorTextBox = new TextBox { Name = "FloorTextBox" };
        TextBox addressIdTextBox = new TextBox { Name = "AddressIdTextBox" };
        TextBox countryTextBox = new TextBox { Name = "CountryTextBox" };
        TextBox cityTextBox = new TextBox { Name = "CityTextBox" };
        TextBox districtTextBox = new TextBox { Name = "DistrictTextBox" };
        TextBox streetTextBox = new TextBox { Name = "StreetTextBox" };
        TextBox houseNumberTextBox = new TextBox { Name = "HouseNumberTextBox" };
        TextBox buildingTextBox = new TextBox { Name = "BuildingTextBox" };
        TextBox apartmentNumberTextBox = new TextBox { Name = "ApartmentNumberTextBox" };
        TextBox areaTextBoxRoom = new TextBox { Name = "AreaTextBox" };
        TextBox windowsCountTextBox = new TextBox { Name = "WindowsCountTextBox" };
        TextBox windowDirectionTextBox = new TextBox { Name = "WindowDirectionTextBox" };
        TextBox RoomOfApartment = new TextBox { Name = "RoomOfApartment" };
        DataService dataService;
        public class Apartment
        {
            public int ApartmentID { get; set; }
            public decimal Area { get; set; }
            public int Rooms { get; set; }
            public bool Kitchen { get; set; }
            public bool Bathroom { get; set; }
            public bool Toilet { get; set; }
            public bool Basement { get; set; }
            public bool Balcony { get; set; }
            public int ConstructionYear { get; set; }
            public string MaterialType { get; set; }
            public int Floor { get; set; }

            public int Adress { get; set; }
            public Address AddressUsed { get; set; }

            public ICollection<Room> RoomsAssociated { get; set; }
        }

        public class Room
        {
            public int RoomID { get; set; }
            public decimal Area { get; set; }
            public int WindowsCount { get; set; }
            public string WindowDirection { get; set; }

            public int ApartmentID { get; set; }
            public Apartment Apartment { get; set; }
        }

        public class Address
        {
            public int AddressID { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string Street { get; set; }
            public string HouseNumber { get; set; }
            public string Building { get; set; }
            public string ApartmentNumber { get; set; }
            public ICollection<Apartment> Apartments { get; set; }
        }

        public interface IRepository<T> where T : class
        {
            IEnumerable<T> GetAll();
            void Add(T entity);
            void Save();
        }

        public class Repository<T> : IRepository<T> where T : class
        {
            private readonly DbSet<T> _dbSet;
            private readonly DbContext _context;

            public Repository(DbSet<T> dbSet, DbContext context)
            {
                _dbSet = dbSet;
                _context = context;
            }

            public IEnumerable<T> GetAll()
            {
                return _dbSet.ToList();
            }

            public void Add(T entity)
            {
                _dbSet.Add(entity);
            }

            public void Save()
            {
                _context.SaveChanges();
            }
        }

        public interface IUnitOfWork : IDisposable
        {
            IRepository<Apartment> Apartments { get; }
            IRepository<Room> Rooms { get; }
            IRepository<Address> Addresses { get; }
            void Save();
        }

        public class UnitOfWork : IUnitOfWork
        {
            private readonly ApartmentDbContext _context;

            public UnitOfWork(ApartmentDbContext context)
            {
                _context = context;
                Apartments = new Repository<Apartment>(_context.Apartment, _context);
                Rooms = new Repository<Room>(_context.Room, _context);
                Addresses = new Repository<Address>(_context.Address, _context);
            }

            public IRepository<Apartment> Apartments { get; }
            public IRepository<Room> Rooms { get; }
            public IRepository<Address> Addresses { get; }

            public void Save()
            {
                _context.SaveChanges();
            }

            public void Dispose()
            {
                _context.Dispose();
            }
        }

        public class DataService
        {
            private readonly IUnitOfWork _unitOfWork;

            public DataService(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public DataTable GetAllApartmentsAsDataTable()
            {
                var apartments = _unitOfWork.Apartments.GetAll();
                return ConvertListToDataTable(apartments);
            }

            public DataTable GetAllRoomsAsDataTable()
            {
                var rooms = _unitOfWork.Rooms.GetAll();
                return ConvertListToDataTable(rooms);
            }

            public DataTable GetAllAddressesAsDataTable()
            {
                var addresses = _unitOfWork.Addresses.GetAll();
                return ConvertListToDataTable(addresses);
            }
            private DataTable ConvertListToDataTable<T>(IEnumerable<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);

                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    dataTable.Columns.Add(prop.Name);
                }

                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                return dataTable;
            }

            public DataTable GetApartmentDetails()
            {
                var query = from apartment in _unitOfWork.Apartments.GetAll()
                            join address in _unitOfWork.Addresses.GetAll() on apartment.AddressUsed.AddressID equals address.AddressID
                            join room in _unitOfWork.Rooms.GetAll() on apartment.ApartmentID equals room.ApartmentID
                            select new
                            {
                                apartment.ApartmentID,
                                ApartmentArea = apartment.Area,
                                apartment.Rooms,
                                apartment.ConstructionYear,
                                apartment.MaterialType,
                                apartment.Floor,
                                room.RoomID,
                                RoomArea = room.Area,
                                room.WindowsCount,
                                room.WindowDirection,
                                address.Country,
                                address.City,
                                address.District,
                                address.Street,
                                address.HouseNumber,
                                address.Building,
                                address.ApartmentNumber
                            };

                return ConvertQueryToDataTable(query);
            }
            private DataTable ConvertQueryToDataTable<T>(IEnumerable<T> query)
            {
                DataTable dataTable = new DataTable();

                PropertyInfo[] props = typeof(T).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    dataTable.Columns.Add(prop.Name);
                }

                foreach (var item in query)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (PropertyInfo prop in props)
                    {
                        row[prop.Name] = prop.GetValue(item);
                    }
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
            public void AddAddress(string country, string city, string district, string street, string houseNumber, string building, string apartmentNumber)
            {
                if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(street) || string.IsNullOrEmpty(houseNumber))
                {
                    MessageBox.Show("Please fill in all required fields (Country, City, Street, House Number).");
                    return;
                }

                if (country.Length > 100 || city.Length > 100 || district.Length > 100 || street.Length > 100 || houseNumber.Length > 20 || building.Length > 20 || apartmentNumber.Length > 20)
                {
                    MessageBox.Show("Input length exceeds maximum allowed length.");
                    return;
                }
                Address newAddress = new Address
                {
                    Country = country,
                    City = city,
                    District = district,
                    Street = street,
                    HouseNumber = houseNumber,
                    Building = building,
                    ApartmentNumber = apartmentNumber
                };
                _unitOfWork.Addresses.Add(newAddress);
                _unitOfWork.Save();
            }

            public void AddRoom(string areaOfRoom, string windowDirection, string windowCount, string apartmentID)
            {
                if (!decimal.TryParse(areaOfRoom, out decimal areaOfRoomValue) || !int.TryParse(windowCount, out int windowCountValue) || !int.TryParse(apartmentID, out int apartmentIDValue))
                {
                    MessageBox.Show("Input correct values into numeric fields");
                    return;
                }
                if (areaOfRoomValue < 1)
                {
                    MessageBox.Show("Check inputted values");
                    return;
                }
                if (windowDirection != "East" && windowDirection != "West" && windowDirection != "South" && windowDirection != "North")
                {
                    MessageBox.Show("Check window direction");
                    return;
                }
                if (!_unitOfWork.Apartments.GetAll().Any(a => a.ApartmentID == apartmentIDValue))
                {
                    MessageBox.Show("Apartment ID does not exist");
                    return;
                }
                int roomCount = _unitOfWork.Rooms.GetAll().Count(r => r.ApartmentID == apartmentIDValue);
                int maxRooms = _unitOfWork.Apartments.GetAll().FirstOrDefault(a => a.ApartmentID == apartmentIDValue)?.Rooms ?? 0;
                if (roomCount >= maxRooms)
                {
                    MessageBox.Show("Cannot add more rooms to this apartment");
                    return;
                }
                Room newRoom = new Room
                {
                    Area = areaOfRoomValue,
                    WindowsCount = windowCountValue,
                    WindowDirection = windowDirection,
                    ApartmentID = apartmentIDValue
                };
                _unitOfWork.Rooms.Add(newRoom);
                _unitOfWork.Save();
            }

            public void AddApartment(string area, string rooms, string kitchen, string bathroom, string wc, string basement, string balcony, string constructionYear, string material, string floor, string addressID)
            {
                if (!decimal.TryParse(area, out decimal areaValue) || !int.TryParse(rooms, out int roomsValue) || !int.TryParse(kitchen, out int kitchenValue) ||
                    !int.TryParse(bathroom, out int bathroomValue) || !int.TryParse(wc, out int wcValue) || !int.TryParse(basement, out int basementValue) ||
                    !int.TryParse(balcony, out int balconyValue) || !int.TryParse(constructionYear, out int constructionYearValue) || !int.TryParse(floor, out int floorValue) ||
                    !int.TryParse(addressID, out int addressIDValue))
                {
                    MessageBox.Show("Values are not numeric");
                    return;
                }
                if ((kitchenValue != 0 && kitchenValue != 1) || (bathroomValue != 0 && bathroomValue != 1) || (wcValue != 0 && wcValue != 1))
                {
                    MessageBox.Show("Bit values are not convertible");
                    return;
                }
                if (!_unitOfWork.Addresses.GetAll().Any(a => a.AddressID == addressIDValue))
                {
                    MessageBox.Show("Address ID does not exist");
                    return;
                }
                Apartment newApartment = new Apartment
                {
                    Area = areaValue,
                    Rooms = roomsValue,
                    Kitchen = kitchenValue == 1,
                    Bathroom = bathroomValue == 1,
                    Toilet = wcValue == 1,
                    Basement = basementValue == 1,
                    Balcony = balconyValue == 1,
                    ConstructionYear = constructionYearValue,
                    MaterialType = material,
                    Floor = floorValue,
                    Adress = addressIDValue
                };
                _unitOfWork.Apartments.Add(newApartment);
                _unitOfWork.Save();
            }
        }


        public class ApartmentDbContext : DbContext
        {
            public DbSet<Apartment> Apartment { get; set; }
            public DbSet<Room> Room { get; set; }
            public DbSet<Address> Address { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=ApartmentDB;Integrated Security=True;Encrypt=False;");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Apartment>()
                    .HasOne(a => a.AddressUsed)
                    .WithMany(ad => ad.Apartments)
                    .HasForeignKey(a => a.Adress);

                modelBuilder.Entity<Room>()
                    .HasOne(r => r.Apartment)
                    .WithMany(ap => ap.RoomsAssociated)
                    .HasForeignKey(r => r.ApartmentID);
            }
        }
        bool ShowedAll = false;
        public MainWindow()
        {
            InitializeComponent();
            PopulateDataGrids();
        }

        private void PopulateDataGrids()
        {
            using (ApartmentDbContext context = new ApartmentDbContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                dataService = new DataService(unitOfWork);
                dataGrid1.ItemsSource = dataService.GetAllApartmentsAsDataTable().DefaultView;
                dataGrid2.ItemsSource = dataService.GetAllRoomsAsDataTable().DefaultView;
                dataGrid3.ItemsSource = dataService.GetAllAddressesAsDataTable().DefaultView;
            }
        }

        private string connectionString = "Data Source=localhost;Initial Catalog=ApartmentDB;Integrated Security=True;Encrypt=False;";


            public DataTable ExecuteQuery(string query)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }

            public void ExecuteParameterizedQuery(string query, Dictionary<string, object> parameters)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    command.ExecuteNonQuery();
                }
            }

            public void ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(procedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    command.ExecuteNonQuery();
                }
            }

        private void showAll_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable();
                if (ShowedAll)
                {
                    dataTable = dataService.GetApartmentDetails();
                    ShowedAll = false;
                }
                else
                {
                    dataTable = dataService.GetAllApartmentsAsDataTable();
                    ShowedAll = true;
                }

            dataGrid1.ItemsSource = dataTable.DefaultView;
        }


        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Fields.Children.Clear();

            if (listview.SelectedItem == null)
                return;

            ListBoxItem selectedItem = (ListBoxItem)listview.SelectedItem;

            if (selectedItem.Name == "Adress")
            {
                AddAddressFields();
            }
            else if (selectedItem.Name == "RoomListItem")
            {
                AddRoomFields();
            }
            else if (selectedItem.Name == "ApartmentListItem")
            {
                AddApartmentFields();
            }
        }
        private void AddAddressFields()
        {
            countryTextBox.Text = string.Empty;
            cityTextBox.Text = string.Empty;
            districtTextBox.Text = string.Empty;
            streetTextBox.Text = string.Empty;
            houseNumberTextBox.Text = string.Empty;
            buildingTextBox.Text = string.Empty;
            apartmentNumberTextBox.Text = string.Empty;
            Fields.Children.Add(countryTextBox);
            Fields.Children.Add(cityTextBox);
            Fields.Children.Add(districtTextBox);
            Fields.Children.Add(streetTextBox);
            Fields.Children.Add(houseNumberTextBox);
            Fields.Children.Add(buildingTextBox);
            Fields.Children.Add(apartmentNumberTextBox);
        }

        private void AddRoomFields()
        {
            areaTextBoxRoom.Text = string.Empty;
            windowDirectionTextBox.Text = string.Empty;
            windowsCountTextBox.Text = string.Empty;
            RoomOfApartment.Text = string.Empty;
            Fields.Children.Add(areaTextBoxRoom);
            Fields.Children.Add(windowsCountTextBox);
            Fields.Children.Add(windowDirectionTextBox);
            Fields.Children.Add(RoomOfApartment);
        }

        private void AddApartmentFields()
        {
            areaTextBox.Text = string.Empty;
            roomsTextBox.Text = string.Empty;
            KitchenApart.Text = string.Empty;
            BathroomApart.Text = string.Empty;
            Toilet.Text = string.Empty;
            BasementApart.Text = string.Empty;
            BalconyApart.Text = string.Empty;
            constructionYearTextBox.Text = string.Empty;
            materialTypeTextBox.Text = string.Empty;
            floorTextBox.Text = string.Empty;
            addressIdTextBox.Text = string.Empty;
            Fields.Children.Add(areaTextBox);
            Fields.Children.Add(roomsTextBox);
            Fields.Children.Add(KitchenApart);
            Fields.Children.Add(BathroomApart);
            Fields.Children.Add(Toilet);
            Fields.Children.Add(BasementApart);
            Fields.Children.Add(BalconyApart);
            Fields.Children.Add(constructionYearTextBox);
            Fields.Children.Add(materialTypeTextBox);
            Fields.Children.Add(floorTextBox);
            Fields.Children.Add(addressIdTextBox);
        }
        private void Add_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (listview.SelectedIndex == 0)
            {
                string country = countryTextBox.Text;
                string city = cityTextBox.Text;
                string district = districtTextBox.Text;
                string street = streetTextBox.Text;
                string houseNumber = houseNumberTextBox.Text;
                string building = buildingTextBox.Text;
                string apartmentNumber = apartmentNumberTextBox.Text;
                dataService.AddAddress(country, city, district, street, houseNumber, building, apartmentNumber);
            }
            else if (listview.SelectedIndex == 1)
            {
                string areaOfRoom = areaTextBoxRoom.Text;
                string windowDirection = windowDirectionTextBox.Text;
                string windowCount = windowsCountTextBox.Text;
                string apartmentID = RoomOfApartment.Text;
                dataService.AddRoom(areaOfRoom, windowDirection, windowCount, apartmentID);
            }
            else if (listview.SelectedIndex == 2)
            {
                string area = areaTextBox.Text;
                string rooms = roomsTextBox.Text;
                string kitchen = KitchenApart.Text;
                string bath = BathroomApart.Text;
                string wc = Toilet.Text;
                string basement = BasementApart.Text;
                string balcony = BalconyApart.Text;
                string constructionYear = constructionYearTextBox.Text;
                string material = materialTypeTextBox.Text;
                string floor = floorTextBox.Text;
                string addressID = addressIdTextBox.Text;
                dataService.AddApartment(area, rooms, kitchen, bath, wc, basement, balcony, constructionYear, material, floor, addressID);
            }
            else
            {
                MessageBox.Show("Please select an item type first.");
                return;
            }
            PopulateDataGrids();
            listview.SelectedIndex = -1;
        }
        private void ExecuteNonQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        private void QueryCreator_Click(object sender, RoutedEventArgs e)
        {
            string table;
            if(tables.SelectedIndex == 0)
            {
                table = "Room";
            }
            else if(tables.SelectedIndex == 1)
            {
                table = "Apartment";
            }
            else if(tables.SelectedIndex == 2)
            {
                table = "Address";
            }
            else
            {
                MessageBox.Show("Select a table dummy");
                return;
            }
            string columns = Columns.Text;
            if(columns.IsNullOrEmpty())
            {
                columns = "*";
            }
            string orderBy = OrderBy.Text;
            string where = WhereBox.Text;
            string query;
            if (orderBy.IsNullOrEmpty() && where.IsNullOrEmpty())
            {
                query = $"SELECT {columns} FROM {table}";
            }
            else if (orderBy.IsNullOrEmpty())
            {
                query = $"SELECT {columns} from {table} where {where}";
            }
            else if (where.IsNullOrEmpty())
            {
                query = $"SELECT {columns} from {table} order by {orderBy}";
            }
            else
            {
                query = $"SELECT {columns} from {table} where {where} order by {orderBy}";
            }
            tables.SelectedIndex = -1;
            DataTable dataTable = new DataTable();
            dataTable = ExecuteQuery(query);
            dataGrid4.ItemsSource = dataTable.DefaultView;
        }

        private void SaveApartmentsChanges_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(dataGrid1, "Apartment","ApartmentID");
        }

        private void SaveRoomsChanges_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(dataGrid2, "Room","RoomID");
        }

        private void SaveAddressesChanges_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(dataGrid3, "Address","AddressID");
        }
        private bool IsNumeric(string value)
        {
            return double.TryParse(value, out _);
        }
        public void UpdateDataGrid(DataGrid dataGrid, string tableName, string primaryKeyColumnName)
        {
            using (ApartmentDbContext _context = new ApartmentDbContext())
            {
                try
                {
                    if (dataGrid.ItemsSource is DataView dataView)
                    {
                        DataTable dataTable = dataView.Table;
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (row[primaryKeyColumnName] == DBNull.Value)
                            {
                                MessageBox.Show("Primary key value is DBNull.");
                                continue; 
                            }

                            bool popa = Int32.TryParse(row[primaryKeyColumnName].ToString(), out int primaryKeyValue);
                            if (!popa)
                            {
                                MessageBox.Show("Incorrect Id value.");
                                return;
                            }

                            System.Type x;
                            if (tableName == "Apartment")
                            {
                                x = typeof(Apartment);
                            }
                            else if (tableName == "Room")
                            {
                                x = typeof(Room);
                            }
                            else
                            {
                                x = typeof(Address);
                            }

                            var entity = _context.Find(x, primaryKeyValue);

                            if (entity != null)
                            {
                                foreach (DataColumn column in dataTable.Columns)
                                {
                                    if (column.ColumnName != primaryKeyColumnName && column.ColumnName != "Photo")
                                    {
                                        var property = entity.GetType().GetProperty(column.ColumnName);

                                        if (property != null)
                                        {
                                            var value = row[column];
                                            if (value != DBNull.Value)
                                            {
                                                if (property.PropertyType == typeof(bool))
                                                {
                                                    if (value.ToString() != "True" && value.ToString() != "False")
                                                    {
                                                        MessageBox.Show("Incorrect values in bool fields");
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        bool kek;
                                                        if (value.ToString() == "True")
                                                        {
                                                            kek = true;
                                                        }
                                                        else
                                                        {
                                                            kek = false;
                                                        }
                                                        property.SetValue(entity, kek);
                                                    }
                                                }
                                                else if (property.PropertyType == typeof(string))
                                                {
                                                    property.SetValue(entity, value.ToString());
                                                }
                                                else if (property.PropertyType == typeof(int))
                                                {
                                                    bool kek = Int32.TryParse(value.ToString(), out int lol);
                                                    if (kek)
                                                    {
                                                        property.SetValue(entity, lol);
                                                    }
                                                }
                                                else if (property.PropertyType == typeof(decimal))
                                                {
                                                    property.SetValue(entity, Convert.ToDecimal(value));
                                                }
                                                else
                                                {
                                                    property.SetValue(entity, value);
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Stumbled upon empty field, field skipped");
                                                continue;
                                            }
                                        }
                                    }
                                }
                                _context.SaveChanges();
                            }
                            else
                            {
                                MessageBox.Show($"Entity with {primaryKeyColumnName} = '{primaryKeyValue}' not found.");
                            }
                        }

                        MessageBox.Show("Data updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Unable to update data: ItemsSource is not a DataTable.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating data: {ex.Message}");
                }
            }
        }

    }
}