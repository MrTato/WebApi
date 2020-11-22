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
        private string baseURL = "https://localhost:44344/";
        // GET: Facturas
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

            HttpResponseMessage response = httpClient.GetAsync("/api/Ubicaciones").Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
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
        }

        public ActionResult Guardar(
            int IdUbicacion,
            int IdMaestro,
            string Nombre,
            string Tipo
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
                UbicacionCLS ubicacion = new UbicacionCLS();
                ubicacion.IdUbicacion = IdUbicacion;
                ubicacion.IdMaestro = IdMaestro;
                ubicacion.Nombre = Nombre;
                ubicacion.Tipo = Tipo;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

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
                    else
                    {
                        return RedirectToAction("Index", "Token");
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

        public ActionResult Eliminar(int IdUbicacion)
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