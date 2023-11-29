using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Мотосалон
{
    internal class Users : CRUD
    {
        public int id;
        public string login;
        public string password;
        public int role;
        public void Main(Users account, string path, string file, string username, List<Users> values = null)
        {
            while (true)
            {
                string account_type = "Users";
                string[] headers = ["ID", "Логин", "Пароль", "Роль"];
                string[] info = ["Режим поиска"];
                int f_max = 2;
                if (values == null)
                {
                    string json = Read(path, file);
                    values = Files.Deserialize<List<Users>>(path, file);
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
                if (values.Count() > 0)
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
                    pos -= 2;
                    List<string> current = null;
                    if (pos > values.Count())
                    {
                        pos -= values.Count();
                    }
                    else
                    {
                        Users acc = values[pos - 1];
                        current = [acc.id.ToString(), acc.login, acc.password, acc.role.ToString()];
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
                    string[] dop = ["Роли:", $"{(int)Roles.Администратор} - Администратор",
                        $"{(int)Roles.Кадровик} - Кадровик",
                        $"{(int)Roles.Кладовщик} - Кладовщик",
                        $"{(int)Roles.Кассир} - Кассир",
                        $"{(int)Roles.Бухгалтер} - Бухгалтер"];
                    if (current == null)
                    {
                        switch (pos)
                        {
                            case (int)Keys.F1:
                                Create(path, account_type, data, dop, [0, 3]);
                                break;
                            case (int)Keys.F2:
                                Read(path, file, username, account, headers);
                                break;
                            case (int)Keys.F3:
                                if (account.login != values[pos_del].login)
                                {
                                    Delete<Users>(path, file, pos_del);
                                }
                                break;
                        };
                        values = null;
                    }
                    else
                    {
                        Update(path, account_type, data, dop, [0, 3], pos - 1);
                    }
                    Console.Clear();
                }
            }
            
        }
    }
}