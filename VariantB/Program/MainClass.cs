//Гуренко Даниил
// Вариант 5
//Создать консольное приложение, удовлетворяющее следующим требованиям:
//Использовать возможности ООП: классы, наследование, полиморфизм, инкапсуляция.
//Реализовать несколько уровней абстракции (интерфейсы, абстрактные классы).
//Каждый класс должен иметь отражающее смысл название и информативный состав.
//Для хранения коллекций объектов предметной области использовать обобщенные коллекции.
//Обеспечить механизм добавления, удаления, изменения, очистки коллекции.
//Для изменения элементов коллекции и коллекций по различным алгоритмам использовать механизм позднего связывания (делегаты). 
//Для взаимодействия между коллекциями сущностей использовать механизм позднего связывания (события). 
//При кодировании должны быть использованы соглашения об оформлении кода: code convention.
//Использовать механизм обработки исключительных ситуаций.

////////////////////////////////////////////////////////////////////////////////////////////////
//Заказ.В сущностях(типах) хранится информация о заказах магазина и товарах в них.
//Для заказа необходимо хранить:
//— номер заказа;
//— товары в заказе;
//— дату поступления.
//Для товаров в заказе необходимо хранить:
//— товар;
//— количество.
//Для товара необходимо хранить:
//— название;
//— описание;
//— цену.
//Вывести полную информацию о заданном заказе.
//Вывести номера заказов, сумма которых не превосходит заданную и количество различных товаров равно заданному.
//Вывести номера заказов, содержащих заданный товар.
//Вывести номера заказов, не содержащих заданный товар и поступивших в течение текущего дня.
//Сформировать новый заказ, состоящий из товаров, заказанных в текущий день.
//Удалить все заказы, в которых присутствует заданное количество заданного товара.

using System;
using System.Collections.Generic;
using VariantC.TaskClasses;
using VariantC.Program;
using VariantB.Storage;
using VariantB.DelegateEventSort;
using VariantB.DataBase;

// Запромы к коллекциям в папке Program - Functions.cs

namespace VariantC
{
    class MainClass
    {
        static void Main(string[] args)
        {
            var AppleProduct = new Product();
            var TableProduct = new Product();
            var MouseProduct = new Product();
            var TShirtProduct = new Product("TShirt", "Синяя футболка, xl", 450);//создание продуктов-конструктор
            AppleProduct.CreateProduct("Apple", "Свежие яблоки \"Малинка\"", 15.60); //создание продуктов-методы
            TableProduct.CreateProduct("Table", "Лучший в мире стол", 2100);
            MouseProduct.CreateProduct("Mouse", "Logitech. Хорошее качество.", 800); 

            ProductStorage productInOrderStorage = new ProductStorage(); // коллекция продуктов
            OrderStorage storageOrder = new OrderStorage(); // коллекция заказов

            productInOrderStorage.AddProduct(new ProductInOrder(TableProduct, 3)); // Продукты в коллекцию
            productInOrderStorage.AddProduct(new ProductInOrder(AppleProduct, 55));
            productInOrderStorage.AddProduct(new ProductInOrder(TShirtProduct, 5));
            productInOrderStorage.AddProduct(new ProductInOrder(MouseProduct, 7));
            productInOrderStorage.AddProduct(new ProductInOrder(TShirtProduct, 10));
            productInOrderStorage.AddProduct(new ProductInOrder(TableProduct, 1));
            productInOrderStorage.AddProduct(new ProductInOrder(AppleProduct, 20));
            productInOrderStorage.AddProduct(new ProductInOrder(TShirtProduct, 2));//

            storageOrder.AddOrder("0996154567", new Order(83444, 14, new List<ProductInOrder>() {
            productInOrderStorage[0], productInOrderStorage[1]}));// создать добавить заказ // 1 ЗАКАЗ//

            storageOrder.AddOrder("0994433565", new Order(80111, 15, new List<ProductInOrder>() {
            productInOrderStorage[2]}));// добавить заказ // 2 ЗАКАЗ//

            storageOrder.AddOrder("0568462346", new Order(90999, 16, new List<ProductInOrder>() {
            productInOrderStorage[3], productInOrderStorage[4], productInOrderStorage[5]}));// добавить заказ // 3 ЗАКАЗ//

            storageOrder.AddOrder("0564750381", new Order(10000, 15, new List<ProductInOrder>() {
            productInOrderStorage[6], productInOrderStorage[7]}));// добавить заказ // 4 ЗАКАЗ//

            SetOrder(ref productInOrderStorage, ref storageOrder); // Меню добавления заказов. Метод в этом классе ниже.

            for (int i = 0; i < storageOrder.Count; i++) // Вывести все заказы
            {
                Console.WriteLine(storageOrder[i]);
            }
            Console.WriteLine("-------------------------------------------------");

            Functions.SearchOrdersWithSumAndCOuntOfProducts(storageOrder, 10000, 2); //Вывести номера заказов, сумма которых не превосходит заданную и количество различных товаров равно заданному.
            Functions.SearchThisProduction(storageOrder, "TShirt"); // Вывести номера заказов, содержащих заданный товар.
            Functions.SearchNotContainsProductAndToday(storageOrder, "Apple", 15);//Вывести номера заказов, не содержащих заданный товар и поступивших в течение текущего дня.
            storageOrder.AddOrder("0556833325", Functions.CreateOrder(storageOrder, 15)); // Создать заказ из товаров заказанных в этот день

            Console.WriteLine("---------------------------------------------");
            foreach (var item in storageOrder)
                Console.WriteLine(item);

            Functions.RemoveOrdersThisProductThisAmount(ref storageOrder, "TShirt", 2);//Удалить все заказы, в которых присутствует заданное количество заданного товара.
            Console.WriteLine("---------------------------------------------");
            foreach (var item in storageOrder)
                Console.WriteLine(item);
            Console.WriteLine($"За все время было выполнено {Order.orderCount} заказов."); //Выводит количество всех заказов

            Console.ReadKey();
        }


        public static void SetOrder(ref ProductStorage productInOrderStorage, ref OrderStorage storageOrder) // Метод меню.
        {
            int choice = 0;
            string productName = "";
            string productDescription = "";
            double price = 0;
            int amount = 0;
            int orderNumber = 0;
            string phoneNumber = "";
            Console.WriteLine("1.Сделать заказ.\n2.Выйти.");
            do
            {
                choice = Int32.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    int count = 0;
                    int select = 0;
                    do
                    {
                        count++;
                        Console.WriteLine("Имя продукта: ");
                        productName = Console.ReadLine();
                        Console.WriteLine("Описание: ");
                        productDescription = Console.ReadLine();
                        Console.WriteLine("Цена: ");
                        price = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Количество: ");
                        amount = Int32.Parse(Console.ReadLine());

                        var someProduct = new Product(productName, productDescription, price);
                        productInOrderStorage.AddProduct(new ProductInOrder(someProduct, amount));

                        Console.WriteLine("Еще товар - 1. Закончить - 2.");
                        select = Int32.Parse(Console.ReadLine());
                    } while (select != 2);

                    Console.WriteLine("Номер заказа: ");
                    orderNumber = Int32.Parse(Console.ReadLine());
                    Console.WriteLine("Ваш номер телефона: ");
                    phoneNumber = Console.ReadLine();

                    storageOrder.AddOrder(phoneNumber, new Order(orderNumber, new List<ProductInOrder>(productInOrderStorage.GetLastValues(count))));

                }
            } while (choice != 2);
        }

        public static void IsDeserialize(bool check, OrderStorage storageOrder) // Спрашивает, нужна ли десериализация
        {
            if(check)
            {
                storageOrder.Deserialize();
            }
        }

    }
}
