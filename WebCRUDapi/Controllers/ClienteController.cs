using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebCRUDapi.Models;
using System.Net.Http.Headers;

namespace WebCRUDapi.Controllers
{
    public class ClienteController : Controller
    {
        // private string baseURL = "https://localhost:44392/";
        private string baseURL = "https://localhost:44344/";
        // GET: Clientes
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync("/api/Clientes").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<ClienteCLS> clientes = JsonConvert.DeserializeObject<List<ClienteCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = clientes,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );

        }

        /**
         * Receieves the id of a client
         * Returns a class with all of the specified client's data
         */
        public ClienteCLS Elemento(int IdCliente)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync($"/api/Clientes/{IdCliente}").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            ClienteCLS cliente = JsonConvert.DeserializeObject<ClienteCLS>(data);

            return cliente;
        }

        public JsonResult Guardar(
            int IdCliente,
            string Nombre,
            string Apellido,
            string Telefono,
            string Tipo,
            string Estado
            )
        {
            try
            {
                ClienteCLS cliente = new ClienteCLS();
                cliente.IdCliente = IdCliente;
                cliente.Nombre = Nombre;
                cliente.Apellido = Apellido;
                cliente.Telefono = Telefono;
                cliente.Tipo = Tipo;
                cliente.Estado = Estado;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string clienteJson = JsonConvert.SerializeObject(cliente);
                HttpContent body = new StringContent(clienteJson, Encoding.UTF8, "application/json");

                if (IdCliente == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/Clientes", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Clente creado satisfactoriamente"
                            },
                            JsonRequestBehavior.AllowGet
                            );
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/Clientes/{IdCliente}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Clente modificado satisfactoriamente"
                            },
                            JsonRequestBehavior.AllowGet
                            );
                    }
                }
                throw new Exception("Error al guardar");
            } 
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = ex.InnerException
                    },
                    JsonRequestBehavior.AllowGet
                    );
            }
        }

        public JsonResult Eliminar(int IdCliente) 
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync($"/api/Clientes/{IdCliente}").Result;

            if (response.IsSuccessStatusCode)
            {
                return Json(
                    new
                    {
                        success = true,
                        message = "Clente eliminado satisfactoriamente"
                    },
                    JsonRequestBehavior.AllowGet
                    );
            }
            throw new Exception("Error al eliminar");
        }
    }
}