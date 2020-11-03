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

namespace WebCRUDapi.Controllers
{
    public class ClientesController : Controller
    {
        private string baseURL = "https://localhost:44392/";
        // GET: Clientes
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync("/api/Clientes").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<ClientesCLS> clientes = JsonConvert.DeserializeObject<List<ClientesCLS>>(data);

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
                ClientesCLS cliente = new ClientesCLS();
                cliente.IdCliente = IdCliente;
                cliente.Nombre = Nombre;
                cliente.Apellido = Apellido;
                cliente.Telefono = Telefono;
                cliente.Tipo = Tipo;
                cliente.Estado = Estado;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

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
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

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