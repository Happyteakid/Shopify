using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Domain;

internal static class Email
{
    public static void SendEmail(Order order, List<OrderedProduct> products) //Lista pozwala na modyfikacje kolekcji, nie chcemy jej modyfikowac dlatego nie powinno sie przekazywac tego jako lista, jak juz to arrayem
    {
        var emailTo = order.Address.Email;
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("codecoolshop123@gmail.com", "crdittdmwumbitlg"),
            EnableSsl = true
        };
        var message = CreateMessage(order, products);
        client.Send("codecoolshop123@gmail.com", emailTo,
            $"CodeCool Shop - You buy {products.Sum(p => p.Quantity)} products", message); //jezeli products bedzie pusty to Sum wywali Ci bledem
    }

    private static string CreateMessage(Order order, List<OrderedProduct> products)
    {
        var message = $"Hello {order.Address.FullName},\nHere is you confirmation order.\nYou buy:\n\n";
        foreach (var product in products)
            message +=
                $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\n\n"; //daj jakies klamerki bo to strasznie wyglada ciezko sie czyta czy ten kod nadal jest w foreachu czy nie

        message +=
            $"Total price: {products.Sum(p => p.Quantity * p.Price)}\nTotal products: {products.Sum(p => p.Quantity)}\n\n";
        message +=
            $"Your shipping address:\n\nFull name: {order.Address.FullName}\nStreet: {order.Address.Street}\nCity: {order.Address.City}\nZip code: {order.Address.Zip}\n" +
            $"Country: {order.Address.Country}\nEmail: {order.Address.Email}\nPhone: {order.Address.Phone}\n";
        return message;
    }
}
