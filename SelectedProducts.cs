using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Мотосалон
{
    internal class SelectedProducts : Products
    {
        public int selected;
        public void Main(Users account, string path, string file, string username, List<SelectedProducts> values = null, List<SelectedProducts> values_copy = null)
        {
            bool global_check = false;
            if (values != null)
            {
                global_check = true;
            }
            while (true)
            {
                Program.Json(path, file);
                string[] headers = ["ID", "Название", "Цена", "На складе", "Выбрано"];
                string[] info = ["Режим поиска"];
                int f_max = 5;
                int price = 0;
                bool test = false;
                if (values == null)
                {
                    string json = Read(path, file);
                    List<Products> products = Files.Deserialize<List<Products>>(path, file);
                    values = [];
                    int a = 0;
                    foreach (var i in products)
                    {
                        SelectedProducts sp = new SelectedProducts();
                        sp.id = i.id;
                        sp.title = i.title;
                        sp.price = i.price;
                        sp.amount = i.amount;
                        sp.selected = 0;
                        if (values_copy != null)
                        {
                            foreach (var item in values_copy)
                            {
                                if (item.id == sp.id)
                                {
                                    sp.selected = values_copy[a].selected;
                                }
                            }
                        }
                        values.Add(sp);
                        a++;
                    }
                    info = ["F2 - Поиск данных", "'+' - Добавить в корзину",
                        "'-' - Убрать из корзины", "Enter - Сделать заказ"];
                }
                else
                {
                    
                }
                foreach (var i in values)
                {
                    price += i.selected * i.price;
                }
                if (global_check)
                {
                    f_max = 0;
                }
                Program.Print(account, username, headers, values, info);
                for (int i = 0; i < Console.WindowWidth; i++)
                {
                    Console.Write("-");
                };
                Console.WriteLine();
                Console.Write($"Итог: {price}");
                int pos = 0;
                pos = Arrow.Show(3, values.Count() + 2, f_max);
                int pos_work = Console.GetCursorPosition().Top - 4;
                Console.Clear();
                if (pos == (int)Keys.Escape)
                {
                    return;
                }
                pos -= 2;
                if (values.Count() > 0 || pos == values.Count() + 1)
                {
                    if (username == null)
                    {
                        username = account.login;
                    }
                    string text = $"Имя: {username} | Роль: {(Roles)account.role}";
                    int padding = (Console.WindowWidth - text.Length) / 2;
                    Console.WriteLine(text.PadLeft(padding + text.Length));
                    Console.SetCursorPosition(0, values.Count() + 3);
                    for (int i = 0; i < Console.WindowWidth; i++)
                    {
                        Console.Write("-");
                    };
                    List<string> current = null;
                    if (pos > values.Count())
                    {
                        pos -= values.Count();
                    }
                    else
                    {
                        SelectedProducts sel_prod = values[pos - 1];
                        current = [sel_prod.id.ToString(), sel_prod.title,
                            sel_prod.price.ToString(), sel_prod.amount.ToString(),
                            sel_prod.selected.ToString()];
                    }
                    string[,] data = new string[headers.Length + 1, 2];
                    int a = 0;
                    foreach (string header in headers)
                    {
                        data[a, 0] = $"{header}:";
                        if (current == null)
                        {
                            data[a, 1] = "";
                        }
                        else
                        {
                            data[a, 1] = current[a];
                        }
                        a++;
                    }
                    data[a, 0] = "Сохранить";
                    data[a, 1] = null;
                    if (current == null)
                    {
                        switch (pos)
                        {
                            case (int)Keys.F2:
                                Read(path, "SelectedProducts", username, account, headers);
                                break;
                            case (int)Keys.Add:
                                if (values[pos_work].selected + 1 <= values[pos_work].amount)
                                {
                                    values[pos_work].selected += 1;
                                }
                                break;
                            case (int)Keys.Subtract:
                                if (values[pos_work].selected > 0)
                                {
                                    values[pos_work].selected -= 1;
                                }
                                break;
                        };
                    }
                    else
                    {
                        List<Accounting> list_acc = Files.Deserialize<List<Accounting>>(path, "Accounting");
                        List<Products> list_prod = Files.Deserialize<List<Products>>(path, "Products");
                        int max_id = 0;
                        foreach (var item in list_acc)
                        {
                            if (item.id > max_id)
                            {
                                max_id = item.id;
                            }
                        }
                        foreach (var item in values)
                        {
                            if (item.selected > 0)
                            {
                                Accounting accounting = new Accounting();
                                accounting.id = max_id + 1;
                                max_id++;
                                accounting.title = item.title;
                                accounting.amount = item.price * item.selected;
                                DateOnly date = DateOnly.FromDateTime(DateTime.Now);
                                accounting.date = $"{date.Day}.{date.Month}.{date.Year}";
                                accounting.profit = true;
                                list_acc.Add(accounting);
                                a = 0;
                                foreach (var prod in list_prod)
                                {
                                    if (prod.id == item.id)
                                    {
                                        list_prod[a].amount -= item.selected;
                                    }
                                    a++;
                                }
                            }
                        }
                        values = null;
                        Files.Serialize(list_acc, path, "Accounting");
                        Files.Serialize(list_prod, path, "Products");
                    }
                    values_copy = values;
                    values = null;
                    Console.Clear();
                }
            }
        }
    }
}