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
using System.Data.SqlTypes;

namespace WebCRUDapi.Controllers
{
    public class UbicacionController : Controller
    {
        // private string baseURL = "https://localhost:44392/";
        private string baseURL = "https://localhost:44344/";
        // GET: Facturas
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync("/api/Ubicaciones").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<UbicacionCLS> ubicaciones = JsonConvert.DeserializeObject<List<UbicacionCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = ubicaciones,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );

        }

        public JsonResult Guardar(
            int IdUbicacion,
            int IdMaestro,
            string Nombre,
            string Tipo
            )
        {
            try
            {
                UbicacionCLS ubicacion = new UbicacionCLS();
                ubicacion.IdUbicacion = IdUbicacion;
                ubicacion.IdMaestro = IdMaestro;
                ubicacion.Nombre = Nombre;
                ubicacion.Tipo = Tipo;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string ubicacionJSON = JsonConvert.SerializeObject(ubicacion);
                HttpContent body = new StringContent(ubicacionJSON, Encoding.UTF8, "application/json");

                if (IdUbicacion == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/Ubicaciones", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Ubicacion creada satisfactoriamente"
                            },
                            JsonRequestBehavior.AllowGet
                            );
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/Ubicaciones/{IdUbicacion}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Ubicacion modificada satisfactoriamente"
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

        public JsonResult Eliminar(int IdUbicacion)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync($"/api/Ubicaciones/{IdUbicacion}").Result;

            if (response.IsSuccessStatusCode)
            {
                return Json(
                    new
                    {
                        success = true,
                        message = "Ubicacion eliminada satisfactoriamente"
                    },
                    JsonRequestBehavior.AllowGet
                    );
            }
            throw new Exception("Error al eliminar");
        }
    }
}