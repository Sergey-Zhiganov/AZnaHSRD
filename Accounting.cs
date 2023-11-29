using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Мотосалон
{
    internal class Accounting : CRUD
    {
        public int id;
        public string title;
        public int amount;
        public string date;
        public bool profit;
        public void Main(Users account, string path, string file, string username, List<Accounting> values = null)
        {
            while (true)
            {
                Program.Json(path, file);
                string[] headers = ["ID", "Название", "Сумма", "Дата", "Прибыль"];
                string[] info = ["Режим поиска"];
                int f_max = 3;
                int profit = 0;
                if (values == null)
                {
                    values = Files.Deserialize<List<Accounting>>(path, file);
                    if (values == null)
                    {
                        values = [];
                    }
                    else
                    {
                        foreach (var i in values)
                        {
                            if (i.profit == true)
                            {
                                profit += i.amount;
                            }
                            else
                            {
                                profit -= i.amount;
                            }
                        }
                    }
                    info = ["F1 - Добавить данные", "F2 - Поиск данных", "F3 - Удаление данных"];
                }
                else
                {
                    f_max = 0;
                }
                Program.Print(account, username, headers, values, info);
                Console.SetCursorPosition(0, values.Count() + 3);
                for (int i = 0; i < Console.WindowWidth; i++)
                {
                    Console.Write("-");
                };
                Console.WriteLine();
                Console.Write($"Прибыль: {profit}");
                Console.SetCursorPosition(0, 2);
                int pos = 0;
                pos = Arrow.Show(3, values.Count() + 2, 1);
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
                        Accounting accounting = values[pos - 1];
                        current = [accounting.id.ToString(), accounting.title,
                            accounting.amount.ToString(), accounting.date,
                            accounting.profit.ToString()];
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
                                Create(path, file, data, null, [0, 2]);
                                break;
                            case (int)Keys.F2:
                                Read(path, file, username, account, headers);
                                break;
                            case (int)Keys.F3:
                                Delete<Accounting>(path, file, pos_del);
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