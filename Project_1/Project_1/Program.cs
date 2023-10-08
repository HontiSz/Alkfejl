using CsvHelper.Configuration;
using CsvHelper;
using Project_1.Models;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Project_1
{
    class Program
    {
        private static User me = null;
        static void Main()
        {
            do
            {
                Console.WriteLine("Regisztrációhoz írja be a 'register' szót!" +
                    "\nJelszavak listázásához írja be a 'list' szót!" +
                    "\nJelszó hozzáadásához írja be az 'add' szót!" +
                    "\nKijelentkezéshez írja be a 'logout' parancsot!" +
                    "\nKilépéshez írja be az 'exit' szót!");
                string? command = Console.ReadLine();
                if(command != null)
                {
                    if(command == "exit")
                    {
                        break;
                    }
                    switch(command)
                    {
                        case "register": register();break;
                        case "list": list();break;
                        case "add": addPassword(); break;
                        default: Console.WriteLine("Helytelen parancs!");break;
                    }
                }
            } while (true);
        }

        private static void register()
        {
            User user;
            do
            {
                Console.WriteLine("Adja meg a Felhasználónevet, jelszót, email címet, és teljes nevét vesszővel elválasztva!" +
                    "\n(Példa: Username,StrongPassword,myemail@gmail.com,Tejes Név)");

                string? data = Console.ReadLine();
                string[] datas = data.Split(',');

                if (datas.Length < 4)
                {
                    Console.WriteLine("Minden adatot meg kell adni!");
                    continue;
                }
                else if (datas.Length > 4)
                {
                    Console.WriteLine("Túl sok adatot adott meg!");
                    continue;
                } 
                else if (datas[3].Split(' ').Length != 2)
                {
                    Console.WriteLine("Pontosan kettő nevet kell megadni!");
                }
                else
                {
                    user = new User(
                        datas[0], 
                        datas[1], 
                        datas[2],
                        datas[3].Split(' ')[0], 
                        datas[3].Split(' ')[1]);

                    bool mode = true;
                    try
                    {
                        using (StreamWriter writer = new(User.UserCsvPath, append: mode))
                        {
                            CsvConfiguration config = new(CultureInfo.InvariantCulture)
                            {
                                HasHeaderRecord = false
                            };
                            using CsvWriter csv = new(writer, config);
                            csv.WriteRecords(new User[]
                            {
                            user,
                            });
                        }
                    } catch(IOException e)
                    {
                        Console.WriteLine("Sikertelen regisztráció!" +
                            "\nHiba:\n" + e.Message);
                    } catch (Exception e)
                    {
                        Console.WriteLine("Sikertelen regisztráció!" +
                            "\nHiba:\n" + e.Message);
                    }
                    me = user;
                    Console.WriteLine("Sikeres regisztráció!");
                    break;
                }
            } while (true);
        }

        private static void addPassword()
        {
            if (me == null)
            {
                do
                {
                    Console.WriteLine("Ehhez a művelethez először be kell jelentkeznie!" +
                        "\nKérem a felhasználónevét és a jelszavát vesszővel elválasztva!" +
                        "\n(Példa: Username,StrongPassword)");

                    if (login())
                    {
                        break;
                    }

                    Console.WriteLine("Sikertelen bejelentkezés!" +
                        "\nKilépéshez írja be az 'exit' szót!" +
                        "\nFolytatáshoz nyomjon Entert!");

                    if (Console.ReadLine() == "exit")
                    {
                        return;
                    }

                } while (true);
            }
            VaultEntry vaultEntry;
            do
            {
                Console.WriteLine("Adja meg a felhasználónevét, jelszavát, és a weboldalt!" +
                    "\n(Példa: Username,StrongPassword,facebook.com" +
                    "\n kilépéshez írha be az 'exit' szót!");
                string? data = Console.ReadLine();

                if (data == "exit")
                {
                    return;
                }

                string[] datas = data.Split(',');

                if (datas.Length < 3)
                {
                    Console.WriteLine("Minden adatot meg kell adni!");
                    continue;
                }
                if (datas.Length > 3)
                {
                    Console.WriteLine("Túl sok adatot adott meg!");
                    continue;
                }
                vaultEntry = new VaultEntry(
                    me.Username,
                    datas[0],
                    datas[1],
                    datas[2]);

                bool mode = true;
                using (StreamWriter writer = new(VaultEntry.VaultCsvPath, append: mode))
                {
                    CsvConfiguration config = new(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false
                    };
                    using CsvWriter csv = new(writer, config);
                    csv.WriteRecords(new VaultEntry[]
                    {
                            vaultEntry,
                    });
                }
                Console.WriteLine("Sikeres jelszófelvétel!");
                break;
            } while (true);
        }

        private static void list()
        {
            if (me == null) {

                do
                {
                    Console.WriteLine("Ehhez a művelethez először be kell jelentkeznie!" +
                        "\nKérem a felhasználónevét és a jelszavát vesszővel elválasztva!" +
                        "\n(Példa: Username,StrongPassword)");

                    if (login())
                    {
                        break;
                    }

                    Console.WriteLine("Sikertelen bejelentkezés!" +
                        "\nKilépéshez írja be az 'exit' szót!" +
                        "\nFolytatáshoz nyomjon Entert!");

                    if (Console.ReadLine() == "exit")
                    {
                        return;
                    }

                } while (true);
            }

            using StreamReader reader = new StreamReader(VaultEntry.VaultCsvPath);
            using CsvReader csv = new(
                reader, CultureInfo.InvariantCulture);
            var vaultEntries = csv.GetRecords<VaultEntry>()
                .Where(el => el.Userid == me.Username).ToArray();

            foreach(VaultEntry vaultEntry in vaultEntries)
            {
                Console.WriteLine(vaultEntry);
            }
        }

        private static bool login()
        {
            string? data = Console.ReadLine();
            string[] datas = data.Split(',');

            using StreamReader reader = new(User.UserCsvPath);
            using CsvReader csv = new(
                reader, CultureInfo.InvariantCulture);
            User? user = csv.GetRecords<User>()
                .Where(el => el.Username == datas[0])
                .FirstOrDefault();
            if(user == null)
            {
                return false;
            }
            if(user.Password == datas[1])
            {
                me = user;
                Console.WriteLine("Sikeres bejelentkezés!");
                return true;
            }
            return false;
        }

        private static void logout()
        {
            me = null;
            Console.WriteLine("Sikeres kijelentkezés!");
        }
    }
}