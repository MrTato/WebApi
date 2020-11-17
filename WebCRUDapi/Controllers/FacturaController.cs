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

            HttpResponseMessage response = httpClient.GetAsync("/api/Facturas").Result;
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

        public JsonResult Guardar(
            int IdFactura,
            string Numero,
            int IdCliente,
            DateTime Fecha,
            int IdZonaCliente,
            SqlMoney Total
            )
        {
            try
            {
                FacturaCLS factura = new FacturaCLS();
                factura.IdFactura = IdFactura;
                factura.Numero = Numero;
                factura.IdCliente = IdCliente;
                factura.Fecha = Fecha;
                factura.IdZonaCliente = IdZonaCliente;
                factura.Total = Total;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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

        public JsonResult Eliminar(int IdFactura)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
            throw new Exception("Error al eliminar");
        }
    }
}