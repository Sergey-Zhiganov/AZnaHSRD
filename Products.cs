using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Мотосалон
{
    internal class Products : CRUD
    {
        public int id;
        public string title;
        public int price;
        public int amount;
        public void Main(Users account, string path, string file, string username, List<Products> values = null)
        {
            while (true)
            {
                Program.Json(path, file);
                string[] headers = ["ID", "Название", "Цена", "Кол-во"];
                string[] info = ["Режим поиска"];
                int f_max = 3;
                if (values == null)
                {
                    string json = Read(path, file);
                    values = Files.Deserialize<List<Products>>(path, file);
                    if (values == null)
                    {
                        values = [];
                    }
                    info = ["F1 - Добавить данные", "F2 - Поиск данных", "F3 - Удаление данных"];
                }
                else
                {
                    f_max = 0;
                }
                Program.Print(account, username, headers, values, info);
                int pos = 0;
                pos = Arrow.Show(3, values.Count() + 2, f_max);
                int pos_del = Console.GetCursorPosition().Top - 4;
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
                        Products prod = values[pos - 1];
                        current = [prod.id.ToString(), prod.title, prod.price.ToString(),
                            prod.amount.ToString()];
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
                            case (int)Keys.F1:
                                Create(path, file, data, null, [0, 2, 3]);
                                break;
                            case (int)Keys.F2:
                                Read(path, file, username, account, headers);
                                break;
                            case (int)Keys.F3:
                                Delete<Products>(path, file, pos_del);
                                break;
                        };
                    }
                    else
                    {
                        Update(path, file, data, null, [0, 2, 3], pos - 1);
                    }
                    values = null;
                    Console.Clear();
                }
            }
        }
    }
}