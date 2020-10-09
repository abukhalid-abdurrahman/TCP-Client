using System;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace TCPRequest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создание пользователя для совершения запроса
            TcpClient client = new TcpClient();

            // Хост, на который будет совершен запрос
            string hostname = "193.19.171.246";
            // Соединение пользователя с хостом
            client.Connect(hostname, 80);

            // Получение потока канала соединения
            NetworkStream networkStream = client.GetStream();
            networkStream.ReadTimeout = 2000;

            // Запрос от пользователя
            string message = $@"GET / HTTP/1.1
                               Accept: text/html, charset=utf-8
                               Accept-Language: en-US
                               User-Agent: TCP Client
                               Connection: Keep-Alive
                               Keep-Alive: timeout=5, max=1000
                               Authorization: Basic UGF1bGlnOjBiMjhkNDhjLTM2MzMtNGI0Zi05OWRjLWUyYzk0NTQ2N2MwOA==
                               Host: {hostname}" + "\r\n\r\n";
            
            // Запись запроса в канал соединения
            var reader = new StreamReader(networkStream, Encoding.UTF8);
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            networkStream.Write(bytes, 0, bytes.Length);

            string responseFromServer = reader.ReadToEnd();

            Console.WriteLine($"Response: {responseFromServer}");
        }
    }
}
