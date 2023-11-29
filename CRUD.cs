using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Мотосалон
{
    internal class CRUD
    {
        public static void Create(string path, string file, string[,] data, string[] dop, List<int> ints)
        {
            CreateOrUpdate(path, file, data, dop, ints);
        }
        public static string Read(string path, string file, string username = null, Users account = null, string[] headers = null, List<SelectedProducts> sp = null)
        {
            string json =  File.ReadAllText($"{path}//{file}.json");
            if ( headers == null )
            {
                return json;
            }
            Console.Clear();
            string text = $"Имя: {account.login} | Роль: {(Roles)account.role}";
            int padding = (Console.WindowWidth - text.Length) / 2;
            Console.WriteLine(text.PadLeft(padding + text.Length));
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            };
            Console.WriteLine("Выберите столбец");
            foreach (string header in headers )
            {
                Console.WriteLine($"  {header}");
            }
            int pos = Arrow.Show(3, headers.Length + 2);
            if ( pos == 0 )
            {
                return null;
            }
            pos -= 3;
            Console.SetCursorPosition(0, headers.Length + 3);
            Console.WriteLine();
            Console.Write("Введите значение для поиска: ");
            string value = Console.ReadLine();
            switch (file)
            {
                case "Users":
                    List<Users> acc = Files.Deserialize<List<Users>>(path, file);
                    List<Users> list = new List<Users>();
                    foreach (Users acc2 in acc)
                    {
                        switch (pos)
                        {
                            case 0:
                                if (acc2.id.ToString() == value)
                                {
                                    list.Add(acc2);
                                }
                                break;
                            case 1:
                                if (acc2.login == value)
                                {
                                    list.Add(acc2);
                                }
                                break;
                            case 2:
                                if (acc2.password == value)
                                {
                                    list.Add(acc2);
                                }
                                break;
                            case 3:
                                if (acc2.role.ToString() == value)
                                {
                                    list.Add(acc2);
                                }
                                break;
                        }
                    }
                    Console.Clear();
                    Users users = new Users();
                    users.Main(account, path, null, file, list);
                    break;
                case "Employees":
                    List<Employees> emp = Files.Deserialize<List<Employees>>(path, file);
                    List<Employees> list_emp = new List<Employees>();
                    Console.WriteLine(pos);
                    foreach (Employees emp2 in emp)
                    {
                        switch (pos)
                        {
                            case 0:
                                if (emp2.id.ToString() == value)
                                {
                                    list_emp.Add(emp2);
                                }
                                break;
                            case 1:
                                if (emp2.name == value)
                                {
                                    list_emp.Add(emp2);
                                }
                                break;
                            case 2:
                                if (emp2.surname == value)
                                {
                                    list_emp.Add(emp2);
                                }
                                break;
                            case 3:
                                if (emp2.middleName == value)
                                {
                                    list_emp.Add(emp2);
                                }
                                break;
                            case 4:
                                if (emp2.birth_date == value)
                                {
                                    list_emp.Add(emp2);
                                }
                                break;
                            case 5:
                                if (emp2.passport.ToString() == value)
                                {
                                    list_emp.Add(emp2);
                                }
                                break;
                            case 6:
                                if (emp2.post == value)
                                {
                                    list_emp.Add(emp2);
                                }
                                break;
                            case 7:
                                if (emp2.salary.ToString() == value)
                                {
                                    list_emp.Add(emp2);
                                }
                                break;
                            case 8:
                                if (emp2.accountID.ToString() == value)
                                {
                                    list_emp.Add(emp2);
                                }
                                break;
                        }
                    }
                    Console.Clear();
                    Employees employees = new Employees();
                    employees.Main(account, path, file, list_emp);
                    break;
                case "Products":
                    List<Products> prod = Files.Deserialize<List<Products>>(path, file);
                    List<Products> list_prod = new List<Products>();
                    foreach (Products prod2 in prod)
                    {
                        switch (pos)
                        {
                            case 0:
                                if (prod2.id.ToString() == value)
                                {
                                    list_prod.Add(prod2);
                                }
                                break;
                            case 1:
                                if (prod2.title == value)
                                {
                                    list_prod.Add(prod2);
                                }
                                break;
                            case 2:
                                if (prod2.price.ToString() == value)
                                {
                                    list_prod.Add(prod2);
                                }
                                break;
                            case 3:
                                if (prod2.amount.ToString() == value)
                                {
                                    list_prod.Add(prod2);
                                }
                                break;
                        }
                    }
                    Console.Clear();
                    Products products = new Products();
                    products.Main(account, path, file, username, list_prod);
                    break;
                case "SelectedProducts":
                    List<Products> selprod = Files.Deserialize<List<Products>>(path, file);
                    List<Products> list_selprod = new List<Products>();
                    foreach (Products selprod2 in selprod)
                    {
                        switch (pos)
                        {
                            case 0:
                                if (selprod2.id.ToString() == value)
                                {
                                    list_selprod.Add(selprod2);
                                }
                                break;
                            case 1:
                                if (selprod2.title == value)
                                {
                                    list_selprod.Add(selprod2);
                                }
                                break;
                            case 2:
                                if (selprod2.price.ToString() == value)
                                {
                                    list_selprod.Add(selprod2);
                                }
                                break;
                            case 3:
                                if (selprod2.amount.ToString() == value)
                                {
                                    list_selprod.Add(selprod2);
                                }
                                break;
                        }
                    }
                    Console.Clear();
                    List<SelectedProducts> list_selprod1 = new List<SelectedProducts>();
                    int a = 0;
                    foreach (var item in list_selprod)
                    {
                        SelectedProducts sp1 = new SelectedProducts();
                        sp1.id = item.id;
                        sp1.title = item.title;
                        sp1.price = item.price;
                        sp1.amount = item.amount;
                        sp1.selected = sp[a].selected;
                        a++;
                    }
                    SelectedProducts selproducts = new SelectedProducts();
                    selproducts.Main(account, path, file, username, list_selprod);
                    break;
                case "Accounting":
                    List<Accounting> accounting = Files.Deserialize<List<Accounting>>(path, file);
                    List<Accounting> list_acc = new List<Accounting>();
                    foreach (Accounting acc2 in accounting)
                    {
                        switch (pos)
                        {
                            case 0:
                                if (acc2.id.ToString() == value)
                                {
                                    list_acc.Add(acc2);
                                }
                                break;
                            case 1:
                                if (acc2.title == value)
                                {
                                    list_acc.Add(acc2);
                                }
                                break;
                            case 2:
                                if (acc2.amount.ToString() == value)
                                {
                                    list_acc.Add(acc2);
                                }
                                break;
                            case 3:
                                if (acc2.date.ToString() == value)
                                {
                                    list_acc.Add(acc2);
                                }
                                break;
                            case 4:
                                if (acc2.profit.ToString() == value)
                                {
                                    list_acc.Add(acc2);
                                }
                                break;
                        }
                    }
                    Console.Clear();
                    Accounting accounting1 = new Accounting();
                    accounting1.Main(account, path, null, file, list_acc);
                    break;
            }
            return null;
        }
        public static void Update(string path, string file, string[,] data, string[] dop, List<int> ints, int pos)
        {
            CreateOrUpdate(path, file, data, dop, ints, pos);
        }
        public static void Delete<T>(string path, string file, int pos)
        {
            switch(file)
            {
                case "Users":
                    List<Users> json = Files.Deserialize<List<Users>>(path, file);
                    json.RemoveAt(pos);
                    Files.Serialize<List<Users>>(json, path, file);
                    break;
                case "Employees":
                    List<Employees> json_emp = Files.Deserialize<List<Employees>>(path, file);
                    json_emp.RemoveAt(pos);
                    Files.Serialize<List<Employees>>(json_emp, path, file);
                    break;
                case "Products":
                    List<Products> json_prod = Files.Deserialize<List<Products>>(path, file);
                    json_prod.RemoveAt(pos);
                    Files.Serialize<List<Products>>(json_prod, path, file);
                    break;
                case "Accounting":
                    List<Accounting> json_acc = Files.Deserialize<List<Accounting>>(path, file);
                    json_acc.RemoveAt(pos);
                    Files.Serialize<List<Accounting>>(json_acc, path, file);
                    break;
            }
        }
        private static void CreateOrUpdate(string path, string file, string[,] data, string[] dop, List<int> ints, int pos = -1)
        {
            data = Program.Input(data, 0, dop, ints);
            if (data != null)
            {
                switch (file)
                {
                    case "Users":
                        Users insert = new Users();
                        try
                        {
                            insert.id = int.Parse(data[0, 1]);
                            insert.login = data[1, 1];
                            insert.password = data[2, 1];
                            insert.role = int.Parse(data[3, 1]);
                        }
                        catch
                        {
                            return;
                        }
                        List<Users> list_acc = Files.Deserialize<List<Users>>(path, file);
                        if (pos != -1)
                        {
                            list_acc.RemoveAt(pos);
                        }
                        if (list_acc.Count > 0)
                        {
                            foreach (Users acc in list_acc)
                            {
                                if (acc.id == insert.id || acc.login == insert.login)
                                {
                                    return;
                                }
                            }
                        }
                        if (pos == -1)
                        {
                            list_acc.Add(insert);
                        }
                        else
                        {
                            list_acc.Insert(pos, insert);
                        }
                        Files.Serialize(list_acc, path, file);
                        break;
                    case "Employees":
                        Employees insert1 = new Employees();
                        try
                        {
                            insert1.id = int.Parse(data[0, 1]);
                            insert1.name = data[1, 1];
                            insert1.surname = data[2, 1];
                            if (data[3,1] != "")
                            {
                                insert1.middleName = data[3, 1];
                            }
                            insert1.birth_date = data[4, 1];
                            insert1.passport = int.Parse(data[5, 1]);
                            insert1.post = data[6, 1];
                            insert1.salary = int.Parse(data[7, 1]);
                            if (data[8, 1] != "")
                            {
                                insert1.accountID = int.Parse(data[8, 1]);
                            }
                        }
                        catch
                        {
                            return;
                        }
                        List<Employees> list_emp = Files.Deserialize<List<Employees>>(path, file);
                        if (pos != -1)
                        {
                            list_emp.RemoveAt(pos);
                        }
                        try
                        {
                            if (list_emp.Count > 0)
                            {
                                foreach (Employees emp in list_emp)
                                {
                                    if (emp.id == insert1.id || emp.passport == insert1.passport || emp.accountID == insert1.accountID)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                        catch { }
                        if (pos == -1)
                        {
                            try
                            {
                                list_emp.Add(insert1);
                            }
                            catch
                            {
                                list_emp = [insert1];
                            }
                        }
                        else
                        {
                            list_emp.Insert(pos, insert1);
                        }
                        Files.Serialize(list_emp, path, file);
                        break;
                    case "Products":
                        Products insert_prod = new Products();
                        try
                        {
                            insert_prod.id = int.Parse(data[0, 1]);
                            insert_prod.title = data[1, 1];
                            insert_prod.price = int.Parse(data[2, 1]);
                            insert_prod.amount = int.Parse(data[3, 1]);
                        }
                        catch
                        {
                            return;
                        }
                        List<Products> list_prod = Files.Deserialize<List<Products>>(path, file);
                        if (pos != -1)
                        {
                            list_prod.RemoveAt(pos);
                        }
                        try
                        {
                            if (list_prod.Count > 0)
                            {
                                foreach (Products prod in list_prod)
                                {
                                    if (prod.id == insert_prod.id)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                        catch { }
                        if (pos == -1)
                        {
                            try
                            {
                                list_prod.Add(insert_prod);
                            }
                            catch
                            {
                                list_prod = [insert_prod];
                            }
                        }
                        else
                        {
                            list_prod.Insert(pos, insert_prod);
                        }
                        Files.Serialize(list_prod, path, file);
                        break;
                    case "Accounting":
                        Accounting insert_acc = new Accounting();
                        try
                        {
                            insert_acc.id = int.Parse(data[0, 1]);
                            insert_acc.title = data[1, 1];
                            insert_acc.amount = int.Parse(data[2, 1]);
                            insert_acc.date = data[3, 1];
                            insert_acc.profit = bool.Parse(data[4, 1]);
                        }
                        catch
                        {
                            return;
                        }
                        List<Accounting> list_acc1 = Files.Deserialize<List<Accounting>>(path, file);
                        if (pos != -1)
                        {
                            list_acc1.RemoveAt(pos);
                        }
                        try
                        {
                            if (list_acc1.Count > 0)
                            {
                                foreach (Accounting acc in list_acc1)
                                {
                                    if (acc.id == insert_acc.id)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                        catch { }
                        if (pos == -1)
                        {
                            try
                            {
                                list_acc1.Add(insert_acc);
                            }
                            catch
                            {
                                list_acc1 = [insert_acc];
                            }
                        }
                        else
                        {
                            list_acc1.Insert(pos, insert_acc);
                        }
                        Files.Serialize(list_acc1, path, file);
                        break;
                }
            }
        }
    }
}