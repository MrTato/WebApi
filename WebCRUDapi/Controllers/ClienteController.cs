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
        private string baseURL = "https://localhost:44344/";
        // GET: Clientes
        public ActionResult Index()
        {
            if (!UsuarioAutenticado())
            {
                return RedirectToAction("Index", "Token");
            }
            return View();
        }

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session["token"] != null;
        }

        public ActionResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync("/api/Clientes").Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
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

        public ActionResult Guardar(
            int IdCliente,
            string Nombre,
            string Apellido,
            string Telefono,
            string Tipo,
            string Estado
            )
        {
            if (!UsuarioAutenticado())
            {
                return Json(
                    new
                    {
                        success = false,
                        message = "Usuario no autenticado"
                    },
                    JsonRequestBehavior.AllowGet
                    );
            }

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

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

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
                    else
                    {
                        return RedirectToAction("Index", "Token");
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
                    else
                    {
                        return RedirectToAction("Index", "Token");
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

        public ActionResult Eliminar(int IdCliente)
        {
            if (!UsuarioAutenticado())
            {
                return Json(
                    new
                    {
                        success = false,
                        message = "Usuario no autenticado"
                    },
                    JsonRequestBehavior.AllowGet
                    );
            }
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

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
                else
                {
                    return RedirectToAction("Index", "Token");
                }
                throw new Exception("Error al eliminar");
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
    }
}