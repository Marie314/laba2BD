using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Laba2_BD.Models;

namespace Laba2_BD
{
    public class Program
    {
        static void Print(string sqltext, IEnumerable items)
        {
            Console.WriteLine(sqltext);
            Console.WriteLine("Записи: ");
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
            Console.ReadKey();

        }
        static void Select(BarbershopContext db)
        {
            // LINQ запрос 1
            var query1 = from service in db.Services
                         select new
                         {
                             Id_услуги = service.Id,
                             Id_заказа = service.OrderId,
                             Id_вида_услуги = service.ServiceKindId,
                             Код_услги = service.Code,
                             Имя_услуги = service.Name,
                             Цена = service.Price,
                             Описание = service.Description
                         };
            string comment = "1. Данные из таблицы 'Услуги' (сторона один): \r\n";
            //для наглядности выводим не более 5 записей
            Print(comment, query1.Take(5).ToList());
            

            // LINQ запрос 2
            var query2 = from service in db.Services
                         where (service.Id > 30)
                         select new
                         {
                             Id_услуги = service.Id,
                             Id_заказа = service.OrderId,
                             Id_вида_услуги = service.ServiceKindId,
                             Код_услги = service.Code,
                             Имя_услуги = service.Name,
                             Цена = service.Price,
                             Описание = service.Description
                         };
            comment = "2. Данные из таблицы 'Услуги'(сторона один), где ID больше 30: \r\n";
            //для наглядности выводим не более 5 записей
            Print(comment, query2.Take(5).ToList());




            // LINQ запрос 3
            var query3 = from kind in db.ServiceKinds
                         group kind by kind.Name into k
                         select new
                         {
                             Название_вида_услуги = k.Key,
                             Количество_видов_услуг_с_таким_названием = k.Count(),
                         };

            comment = "3. Данные о количестве вида услуг, сгруппированого по названию: \r\n";
            //для наглядности выводим не более 5 записей
            Print(comment, query3.Take(5).ToList());


            // LINQ запрос 4
            var query4 = from service in db.Services
                             join kind in db.ServiceKinds
                             on service.ServiceKindId equals kind.Id
                             orderby service.ServiceKindId descending
                             select new
                             {
                                 Название_вида_услуги = kind.Name,
                                 Описание_вида_услуги = kind.Description,
                                 Цена_услуги = service.Price,
                                 Код_оказаной_услуги = service.Code
                             };
            comment = "4. Данные об оказаной услуге и данные о виде услуги: \r\n";
            //для наглядности выводим не более 5 записей
            Print(comment, query4.Take(5).ToList());


            // LINQ запрос 5
            var query5 = from service in db.Services
                         join kind in db.ServiceKinds
                         on service.ServiceKindId equals kind.Id
                         where (service.Price < 30)
                         orderby service.ServiceKindId descending
                         select new
                         {
                             Название_вида_услуги = kind.Name,
                             Описание_вида_услуги = kind.Description,
                             Цена_услуги = service.Price,
                             Код_оказаной_услуги = service.Code
                         };
            comment = "5. Данные об оказаной услуге и данные о виде услуги, где стоимость услуги меньше 30: \r\n";
            //для наглядности выводим не более 5 записей
            Print(comment, query5.Take(5).ToList());


        }

        static void Insert(BarbershopContext db)
        {
            //6 Создать новый вид услуги 
            ServiceKind kind = new ServiceKind
            {
                Name = "Каскасд",
                Description = "Ровный срез отдельных прядей"
            };
            // Добавить в DbSet
            db.ServiceKinds.Add(kind);
            // Сохранить изменения в базе данных
            db.SaveChanges();
            Console.WriteLine("Добавлена запись в таблицу 'Вид услуги' (сторона один)");
            
            
            //7 Создать новую услугу
            Service service = new Service
            {
                OrderId = 3,
                ServiceKindId = kind.Id,
                Code = 10000,
                Name = "Услуга0",
                Price = 12,
                Description = "Срез ножницами номер 2"
            };

            // Добавить в DbSet
            db.Services.Add(service);
            // Сохранить изменения в базе данных
            db.SaveChanges();
            Console.WriteLine("Добавлена запись в таблицу 'Услуга' (сторона многие)");

        }

        static void Delete(BarbershopContext db)
        {
            //подлежащие удалению записи в таблице ServiceKind
            int id = 1;
            var kind = db.ServiceKinds.Where(k => k.Id == id);


            //подлежащие удалению записи в связанной таблице Service
            var service = db.Services
                .Include("ServiceKind")
                .Where(s => ((s.ServiceKind.Id == id)));

            //Удаление нескольких записей в таблице Service    
            db.Services.RemoveRange(service);
            db.SaveChanges();
            Console.WriteLine("Записи из таблицы 'Услуга' были удалены (сторона многие)");

            //Удаление нескольких записей в таблице ServiceKind
            db.ServiceKinds.RemoveRange(kind);
            db.SaveChanges();
            Console.WriteLine("Записи из таблицы 'Вид услуги' были удалены (сторона один)");

        }

        static void Update(BarbershopContext db)
        {
            //подлежащие обновлению записи в таблице ServiceKind
            int id = 2;
            var kind = db.ServiceKinds.Where(k => k.Id == id).FirstOrDefault();
            //обновление
            if (kind != null)
            {
                kind.Name = "NNNN";
                kind.Description = "DDDDDDD";
            };

            Console.WriteLine("Записи из таблицы 'Вид услуги' были обновлены");
            // сохранить изменения в базе данных
            db.SaveChanges();

        }




        public static void Main(string[] args)
        {


            using (BarbershopContext db = new BarbershopContext())
            {
                Console.WriteLine("====== Вывод данные из таблицы  ========");
                Select(db);
                
                Console.WriteLine("====== Будет выполнена вставка данных ========");
                Insert(db);
                
                Console.WriteLine("====== Будет выполнено удаление данных ========");
                Delete(db);
               
                Console.WriteLine("====== Будет выполнено обновление данных ========");
                Update(db);
            }
            Console.Read();
        }

    }
}
