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
    public class FacturaController : Controller
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

            HttpResponseMessage response = httpClient.GetAsync("/api/Facturas").Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<FacturaCLS> facturas = JsonConvert.DeserializeObject<List<FacturaCLS>>(data);

                return Json(
                    new
                    {
                        success = true,
                        data = facturas,
                        message = "done"
                    },
                    JsonRequestBehavior.AllowGet
                    );
            }
        }

        public ActionResult Guardar(
            int IdFactura,
            string Numero,
            string Fecha,
            int IdZonaCliente,
            string Total,
            int IdCliente
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
                FacturaCLS factura = new FacturaCLS();
                factura.IdFactura = IdFactura;
                factura.Numero = Numero;
                factura.Fecha = Fecha;
                factura.IdZonaCliente = IdZonaCliente;
                factura.Total = Total;
                factura.Cliente = new ClienteController().Elemento(IdCliente);

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string facturaJSON = JsonConvert.SerializeObject(factura);
                HttpContent body = new StringContent(facturaJSON, Encoding.UTF8, "application/json");

                if (IdFactura == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/Facturas", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Factura creada satisfactoriamente"
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
                    HttpResponseMessage response = httpClient.PutAsync($"/api/Facturas/{IdFactura}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Factura modificada satisfactoriamente"
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

        public ActionResult Eliminar(int IdFactura)
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

                HttpResponseMessage response = httpClient.DeleteAsync($"/api/Facturas/{IdFactura}").Result;

                if (response.IsSuccessStatusCode)
                {
                    return Json(
                        new
                        {
                            success = true,
                            message = "Factura eliminada satisfactoriamente"
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