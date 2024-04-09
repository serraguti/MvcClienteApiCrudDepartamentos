using MvcClienteApiCrudDepartamentos.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcClienteApiCrudDepartamentos.Services
{
    public class ServiceApiDepartamentos
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue header;

        public ServiceApiDepartamentos(IConfiguration configuration)
        {
            this.header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi = configuration.GetValue<string>
                ("ApiUrls:ApiDepartamentos");
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Departamento>>
            GetDepartamentosAsync()
        {
            string request = "api/departamentos";
            List<Departamento> data = await
                this.CallApiAsync<List<Departamento>>(request);
            return data;
        }

        public async Task<Departamento> 
            FindDepartamentoAsync(int idDepartamento)
        {
            string request = "api/departamentos/" + idDepartamento;
            Departamento data = await this.CallApiAsync<Departamento>(request);
            return data;
        }

        //LOS METODOS DE ACCION SE PUEDEN HACER GENERICOS CUANDO 
        //RECIBIMOS OBJETOS.
        //PERO SI LOS PARAMETROS VAN POR URL, NO SUELEN SER GENERICOS
        public async Task DeleteDepartamentoAsync(int idDepartamento)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/departamentos/" + idDepartamento;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                //NO ENVIAMOS NADA PORQUE NO RECIBE NADA NI DEVUELVE
                //NADA
                HttpResponseMessage response =
                    await client.DeleteAsync(request);
                //PODRIAMOS DEVOLVER QUE ES LO QUE HA SUCEDIDO
                //POR EJEMPLO, DEVOLVIENDO STATUS CODE
                //return response.StatusCode;
            }
        }

        public async Task InsertDepartamentoAsync
            (int id, string nombre, string localidad)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/departamentos";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                //INSTANCIAMOS NUESTRO MODEL
                Departamento departamento = new Departamento();
                departamento.IdDepartamento = id;
                departamento.Nombre = nombre;
                departamento.Localidad = localidad;
                //CONVERTIMOS NUESTRO MODEL A JSON
                string json = JsonConvert.SerializeObject(departamento);
                //PARA ENVIAR DATOS (data) AL SERVICIO DEBEMOS 
                //UTILIZAR LA CLASE StringContent QUE NOS PEDIRA
                //LOS DATOS, SU ENCODING Y EL TIPO DE FORMATO
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        public async Task UpdateDepartamentoAsync(int id
            , string nombre, string localidad)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/departamentos";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Departamento departamento = new Departamento();
                departamento.IdDepartamento = id;
                departamento.Nombre = nombre;
                departamento.Localidad = localidad;
                string json = JsonConvert.SerializeObject(departamento);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }
    }
}
