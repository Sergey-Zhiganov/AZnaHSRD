using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using Мотосалон;

namespace Мотосалон
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string text = "Мотосалон \"Едет и ладно\"";
                int consoleWidth = Console.WindowWidth;
                int padding = (consoleWidth - text.Length) / 2;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Мотосалон";
                string?[,] data =
                {
                {"Логин:", ""},
                {"Пароль:" , "" },
                {"Войти", null }
                };
                Users account = null;
                List<Users> users = [];
                string username = null;
                while (account == null)
                {
                    Console.WriteLine(text.PadLeft(padding + text.Length));
                    for (int i = 0; i < Console.WindowWidth; i++)
                    {
                        Console.Write("-");
                    };
                    data = Input(data, 2);
                    if (data == null)
                    {
                        Console.SetCursorPosition(0, 5);
                        return;
                    }
                    string[] input_data = [data[0, 1], data[1, 1]];
                    users = Files.Deserialize<List<Users>>(path, "Users");
                    foreach (var acc in users)
                    {
                        if (input_data[0] == acc.login && input_data[1] == acc.password)
                        {
                            account = acc;
                            if (File.Exists($"{path}\\Employees.json"))
                            {
                                List<Employees> employees = Files.Deserialize<List<Employees>>(path, "Employees");
                                try
                                {
                                    foreach (Employees employee in employees)
                                    {
                                        if (employee.accountID == account.id)
                                        {
                                            username = employee.name;
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    Console.Clear();
                }
                switch (account.role)
                {
                    case (int)Roles.Администратор:
                        account.Main(account, path, "Users", username);
                        break;
                    case (int)Roles.Кадровик:
                        Employees employees = new Employees();
                        employees.Main(account, path, "Employees");
                        break;
                    case (int)Roles.Кладовщик:
                        Products products = new Products();
                        products.Main(account, path, "Products", username);
                        break;
                    case (int)Roles.Кассир:
                        SelectedProducts selectedProducts = new SelectedProducts();
                        selectedProducts.Main(account, path, "Products", username);
                        break;
                    case (int)Roles.Бухгалтер:
                        Accounting accounting = new Accounting();
                        accounting.Main(account, path, "Accounting", username);
                        break;
                }
            }
        }
        public static string?[,]? Input(string?[,] data, int hide = 0, string[] dop = null, List<int> ints = null)
        {
            int max = data.GetLength(0);
            for (int i = 0; i < max; i++)
            {
                if (hide != 0 && data[i, 0] == "Пароль:")
                {
                    Console.Write($"  {data[i, 0]} ");
                    foreach (char c in data[i, 1])
                    {
                        Console.Write("*");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"  {data[i, 0]} {data[i, 1]}");
                }
            };
            if (dop != null)
            {
                for (int i = 0; i < dop.Count(); i++)
                {
                    Console.SetCursorPosition(Console.WindowWidth - 20, i + 2);
                    Console.WriteLine(dop[i]);
                }
            }
            Console.SetCursorPosition(0, 2);
            while (true)
            {
                int pos = Arrow.Show(2, max + 1);
                if (pos == (int)Keys.Escape)
                {
                    return null;
                }
                else if (data[pos - 2, 1] == null)
                {
                    return data;
                }
                bool password_hide = false;
                if (hide != 0)
                {
                    if (pos - 1 == hide)
                    {
                        password_hide = true;
                    }
                }
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                string copy_data = data[pos - 2, 1];
                int left = data[pos - 2, 0].Length + copy_data.Length + 1;
                Console.SetCursorPosition(left + 2, pos);
                while (key.Key != ConsoleKey.Enter)
                {
                    key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
                    {
                        if (password_hide == false)
                        {
                            Console.Write(key.KeyChar);
                        }
                        if (key.Key == ConsoleKey.Delete || key.Key == ConsoleKey.Backspace)
                        {
                            try
                            {
                                Console.SetCursorPosition(data[pos - 2, 0].Length + 3, pos);
                                for (int i = 0; i < copy_data.Length; i++)
                                {
                                    Console.Write(" ");
                                }
                                copy_data = copy_data.Remove(copy_data.Length - 1);
                                Console.SetCursorPosition(data[pos - 2, 0].Length + 3, pos);
                                if (password_hide == false)
                                {
                                    Console.Write(copy_data);
                                }
                                else
                                {
                                    foreach (char c in copy_data)
                                    {
                                        Console.Write("*");
                                    }
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            if (password_hide == true)
                            {
                                Console.Write("*");
                            }
                            copy_data += key.KeyChar;
                        }
                    }
                    else
                    {
                        if (ints != null)
                        {
                            bool error = false;
                            if (data[pos - 2, 0] == "Д/Р:" || data[pos - 2, 0] == "Дата:")
                            {
                                try
                                {
                                    DateOnly.Parse(copy_data);
                                }
                                catch
                                {
                                    error = true;
                                }
                            }
                            else if (data[pos - 2, 0] == "Прибыль:")
                            {
                                try
                                {
                                    bool.Parse(copy_data);
                                }
                                catch
                                {
                                    error = true;
                                }
                            }
                            else if (ints.Contains(pos - 2))
                            {
                                try
                                {
                                    int.Parse(copy_data);
                                    if (data[pos - 2, 0].Contains("ID") && int.Parse(copy_data) <= 0)
                                    {
                                        error = true;
                                    }
                                    else if (data[pos - 2, 0] == "Роль:")
                                    {
                                        bool check = false;
                                        switch(int.Parse(copy_data))
                                        {
                                            case (int)Roles.Администратор:
                                                check = true;
                                                break;
                                            case (int)Roles.Кадровик:
                                                check = true;
                                                break;
                                            case (int)Roles.Кладовщик:
                                                check = true;
                                                break;
                                            case (int)Roles.Кассир:
                                                check = true;
                                                break;
                                            case (int)Roles.Бухгалтер:
                                                check = true;
                                                break;
                                        }
                                        if (check == false)
                                        {
                                            error = true;
                                        }
                                    }
                                }
                                catch
                                {
                                    error = true;
                                }
                            }
                            if (error == true)
                            {
                                Console.SetCursorPosition(data[pos - 2, 0].Length + 3, pos);
                                for (int i = 0; i < copy_data.Length; i++)
                                {
                                    Console.Write(" ");
                                }
                                Console.SetCursorPosition(data[pos - 2, 0].Length + 3, pos);
                                copy_data = data[pos - 2, 1];
                                if (password_hide == false)
                                {
                                    Console.Write(copy_data);
                                }
                            }
                        }
                        data[pos - 2, 1] = copy_data;
                    }
                }
            }
        }
        public static void Print<T>(Users account, string username, string[] headers, List<T> values, string[] info)
        {
            if (username == null)
            {
                username = account.login;
            }
            string text = $"Имя: {username} | Роль: {(Roles)account.role}";
            int padding = (Console.WindowWidth - text.Length) / 2;
            Console.WriteLine(text.PadLeft(padding + text.Length));
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            };
            int interval = (Console.WindowWidth - 2) / (headers.Length + 1);
            Console.Write("  ");
            foreach (string header in headers)
            {
                string header1 = header.Substring(0, Math.Min(interval - 1, header.Length));
                switch (interval)
                {
                    case 23:
                        Console.Write($"{header1,-23}");
                        break;
                    case 19:
                        Console.Write($"{header1,-19}");
                        break;
                    case 11:
                        Console.Write($"{header1,-11}");
                        break;
                }
            }
            Console.WriteLine();
            if (values.Count > 0)
            {
                Type type = values[0].GetType();
                FieldInfo[] fields = type.GetFields();
                if (fields[0].Name != "id")
                {
                    FieldInfo field1 = fields[0];
                    fields = fields.Skip(1).ToArray();
                    fields = fields.Concat(new[] { field1 }).ToArray();
                }
                foreach (var value in values)
                {
                    if (Console.GetCursorPosition().Left == 0)
                    {
                        Console.Write("  ");
                    }
                    foreach (var field in fields)
                    {
                        string field1 = field.GetValue(value).ToString();
                        field1 = field1.Substring(0, Math.Min(interval - 1, field1.Length));
                        field1 = field1.Substring(0, Math.Min(interval - 1, field1.Length));
                        switch (interval)
                        {
                            case 23:
                                Console.Write($"{field1,-23}");
                                break;
                            case 19:
                                Console.Write($"{field1,-19}");
                                break;
                            case 11:
                                Console.Write($"{field1,-11}");
                                break;
                        }
                    }
                    Console.WriteLine();
                }
            }
            for (int i = 0; i < info.Length; i++)
            {
                Console.SetCursorPosition(headers.Length * interval + 1, i + 2);
                Console.WriteLine(info[i]);
            }
        }
        public static void Json(string path, string file)
        {
            bool check = false;
            if (file == "Users")
            {
                check = true;
            }
            string file_full = $"{path}\\{file}.json";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            if (File.Exists(file_full) == false)
            {
                File.Create(file_full).Close();
                if (check == false)
                {
                    File.WriteAllText(file_full, "");
                }
                else
                {
                    Users admin = new Users();
                    admin.id = 1;
                    admin.login = "admin";
                    admin.password = "admin";
                    admin.role = 0;
                    List<Users> admins = new List<Users> { admin };
                    Files.Serialize<List<Users>>(admins, path, file);
                }
            }
        }
    }
}